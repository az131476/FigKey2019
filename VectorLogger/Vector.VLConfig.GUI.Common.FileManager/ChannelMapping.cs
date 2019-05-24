using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ChannelMapping : UserControl
	{
		public delegate void FileConversionParametersChangedHandler(object sender, EventArgs e);

		private class MappingPresenter : IDXDataErrorInfo
		{
			private uint mDestinationChannel;

			private ILoggerSpecifics mLoggerSpecifics;

			private FileConversionParameters mParameters;

			private uint mHighestDestinationChannel;

			public string SourceChannelString
			{
				get
				{
					if (this.BusType == BusType.Bt_CAN)
					{
						return GUIUtil.MapCANChannelNumber2String(this.SourceChannel);
					}
					if (this.BusType == BusType.Bt_LIN)
					{
						return GUIUtil.MapLINChannelNumber2String(this.SourceChannel, this.SourceChannel > this.mLoggerSpecifics.LIN.NumberOfChannels);
					}
					return GUIUtil.MapFlexrayChannelNumber2String(this.SourceChannel);
				}
			}

			public Image Image
			{
				get
				{
					if (this.DestinationChannel == 0u)
					{
						return Resources.ImageMappingNo;
					}
					if (this.SourceChannel != this.DestinationChannel)
					{
						return Resources.ImageMapping;
					}
					return Resources.ImageMappingSame;
				}
			}

			public string DestinationChannelString
			{
				get
				{
					if (this.DestinationChannel == 0u)
					{
						return Resources.Ignore;
					}
					if (this.BusType == BusType.Bt_CAN)
					{
						return GUIUtil.MapCANChannelNumber2String(this.DestinationChannel);
					}
					if (this.BusType == BusType.Bt_LIN)
					{
						return GUIUtil.MapLINChannelNumber2String(this.DestinationChannel, false);
					}
					return GUIUtil.MapFlexrayABChannel2String(this.DestinationChannel);
				}
				set
				{
					if (string.Compare(value.ToString(), Resources.Ignore, true) == 0)
					{
						this.DestinationChannel = 0u;
						return;
					}
					if (this.BusType == BusType.Bt_CAN)
					{
						this.DestinationChannel = GUIUtil.MapCANChannelString2Number(value);
						return;
					}
					if (this.BusType == BusType.Bt_LIN)
					{
						this.DestinationChannel = GUIUtil.MapLINChannelString2Number(value);
						return;
					}
					if (this.BusType == BusType.Bt_FlexRay)
					{
						this.DestinationChannel = GUIUtil.MapFlexrayABChannelString2ABChannel(value);
					}
				}
			}

			public bool MappingActive
			{
				get;
				set;
			}

			public BusType BusType
			{
				get;
				set;
			}

			public uint SourceChannel
			{
				get;
				set;
			}

			public uint DestinationChannel
			{
				get
				{
					return this.mDestinationChannel;
				}
				set
				{
					this.mDestinationChannel = value;
					this.StoreMappingToParams();
				}
			}

			public MappingPresenter(ILoggerSpecifics loggerSpecifics, FileConversionParameters parameters, BusType busType, uint sourceChannel, uint destinationChannel, uint highestDestinationChannel)
			{
				this.mLoggerSpecifics = loggerSpecifics;
				this.mParameters = parameters;
				this.BusType = busType;
				this.SourceChannel = sourceChannel;
				this.DestinationChannel = destinationChannel;
				this.mHighestDestinationChannel = highestDestinationChannel;
			}

			private void StoreMappingToParams()
			{
				if (this.BusType == BusType.Bt_CAN)
				{
					this.mParameters.CanChannelMapping[(int)((UIntPtr)(this.SourceChannel - 1u))] = this.DestinationChannel;
					return;
				}
				if (this.BusType == BusType.Bt_LIN)
				{
					this.mParameters.LinChannelMapping[(int)((UIntPtr)(this.SourceChannel - 1u))] = this.DestinationChannel;
					return;
				}
				if (this.BusType == BusType.Bt_FlexRay)
				{
					this.mParameters.FlexRayChannelMapping[(int)((UIntPtr)(this.SourceChannel - 1u))] = this.DestinationChannel;
				}
			}

			public bool HasError()
			{
				return this.DestinationChannel > this.mHighestDestinationChannel && (this.BusType != BusType.Bt_LIN || this.DestinationChannel > this.mHighestDestinationChannel + Constants.MaximumNumberOfLINprobeChannels);
			}

			public void GetPropertyError(string propertyName, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
			{
				if (propertyName == "DestinationChannelString" && this.MappingActive && this.HasError())
				{
					info.ErrorText = string.Format(Resources.ErrorDestinationChannelNotValidForCurrentLoggerType, new object[0]);
				}
			}

			public void GetError(DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
			{
			}
		}

		public const uint MaxNumberOfCanSourceChannels = 16u;

		public const uint MaxNumberOfLinSourceChannels = 2u;

		public const uint MaxNumberOfFlexRaySourceChannels = 2u;

		private const uint IgnoredChannelNumber = 0u;

		private ILoggerSpecifics mLoggerSpecifics;

		private FileConversionParameters mParameters;

		private BindingList<ChannelMapping.MappingPresenter> mMappingList;

		private IContainer components;

		private GridControl mGridControlMapping;

		private GridView mGridViewMapping;

		private GridColumn colSourceChannel;

		private GridColumn colMapping;

		private GridColumn colDestinationChannel;

		private CheckBox mCheckBoxActivateMapping;

		private CheckBox mCheckBoxHideIdentities;

		private Button mButtonResetMapping;

		private RepositoryItemComboBox mRepoItemDestChannelCan;

		private RepositoryItemComboBox mRepoItemDestChannelLin;

		private RepositoryItemComboBox mRepoItemDestChannelFlexRay;

		private RepositoryItemPictureEdit mRepositoryItemPictureEdit;

		private XtraToolTipController toolTipController;

		public FileConversionParameters FileConversionParameters
		{
			get
			{
				return this.mParameters;
			}
			set
			{
				this.mParameters = value;
				if (this.mLoggerSpecifics != null)
				{
					this.ApplyParametersToControls();
				}
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			set
			{
				this.mLoggerSpecifics = value;
				this.PopulateDestinationChannelComboboxes();
				this.UpdateChannelMappingGrid();
			}
		}

		public bool IsCLFConversionMode
		{
			get;
			set;
		}

		public bool HasError
		{
			get
			{
				if (this.mParameters.UseChannelMapping)
				{
					foreach (ChannelMapping.MappingPresenter current in this.mMappingList)
					{
						if (current.HasError())
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		public ChannelMapping()
		{
			this.InitializeComponent();
			this.mMappingList = new BindingList<ChannelMapping.MappingPresenter>();
			this.mGridControlMapping.DataSource = this.mMappingList;
			BaseEdit.DefaultErrorIcon = Resources.IconError.ToBitmap();
		}

		private void mButtonResetMapping_Click(object sender, EventArgs e)
		{
			this.mCheckBoxHideIdentities.Checked = false;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.mParameters.CanChannelMapping.Length))
			{
				this.mParameters.CanChannelMapping[(int)((UIntPtr)(num - 1u))] = num;
				num += 1u;
			}
			uint num2 = 1u;
			while ((ulong)num2 <= (ulong)((long)this.mParameters.LinChannelMapping.Length))
			{
				this.mParameters.LinChannelMapping[(int)((UIntPtr)(num2 - 1u))] = num2;
				num2 += 1u;
			}
			uint num3 = 1u;
			while ((ulong)num3 <= (ulong)((long)this.mParameters.FlexRayChannelMapping.Length))
			{
				this.mParameters.FlexRayChannelMapping[(int)((UIntPtr)(num3 - 1u))] = num3;
				num3 += 1u;
			}
			this.UpdateChannelMappingGrid();
		}

		private void mCheckBoxActivateMapping_CheckedChanged(object sender, EventArgs e)
		{
			this.mCheckBoxHideIdentities.Enabled = this.mCheckBoxActivateMapping.Checked;
			this.mButtonResetMapping.Enabled = this.mCheckBoxActivateMapping.Checked;
			this.mGridControlMapping.Enabled = this.mCheckBoxActivateMapping.Checked;
			this.mParameters.UseChannelMapping = this.mCheckBoxActivateMapping.Checked;
			foreach (ChannelMapping.MappingPresenter current in this.mMappingList)
			{
				current.MappingActive = this.mParameters.UseChannelMapping;
			}
			this.mGridViewMapping.RefreshData();
		}

		private void mCheckBoxHideIdentities_CheckedChanged(object sender, EventArgs e)
		{
			this.mParameters.HideChannelMappingIdentities = this.mCheckBoxHideIdentities.Checked;
			this.mGridViewMapping.RefreshData();
		}

		private void mGridViewMapping_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			if (e.Column != this.colMapping)
			{
				ChannelMapping.MappingPresenter mappingPresenter = this.mMappingList[this.mGridViewMapping.GetDataSourceRowIndex(e.RowHandle)];
				if (mappingPresenter.BusType == BusType.Bt_CAN)
				{
					e.RepositoryItem = this.mRepoItemDestChannelCan;
					return;
				}
				if (mappingPresenter.BusType == BusType.Bt_LIN)
				{
					e.RepositoryItem = this.mRepoItemDestChannelLin;
					return;
				}
				e.RepositoryItem = this.mRepoItemDestChannelFlexRay;
			}
		}

		private void mRepoItemDestChannelCan_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mGridViewMapping.PostEditor();
			this.mGridViewMapping.RefreshData();
		}

		private void mRepoItemDestChannelLin_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mGridViewMapping.PostEditor();
			this.mGridViewMapping.RefreshData();
		}

		private void mRepoItemDestChannelFlexRay_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mGridViewMapping.PostEditor();
			this.mGridViewMapping.RefreshData();
		}

		private void mGridViewMapping_CustomRowFilter(object sender, RowFilterEventArgs e)
		{
			if (this.mCheckBoxHideIdentities.Checked && this.mMappingList[e.ListSourceRow].SourceChannel == this.mMappingList[e.ListSourceRow].DestinationChannel)
			{
				e.Visible = false;
			}
			else
			{
				e.Visible = true;
			}
			e.Handled = true;
			this.mGridViewMapping.UpdateCurrentRow();
		}

		private void PopulateDestinationChannelComboboxes()
		{
			this.mRepoItemDestChannelCan.Items.Clear();
			if (this.mLoggerSpecifics == null)
			{
				return;
			}
			for (uint num = 1u; num <= this.mLoggerSpecifics.FileConversion.NumberOfCanMappingChannels; num += 1u)
			{
				this.mRepoItemDestChannelCan.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			this.mRepoItemDestChannelCan.Items.Add(Resources.Ignore);
			this.mRepoItemDestChannelLin.Items.Clear();
			for (uint num2 = 1u; num2 <= this.mLoggerSpecifics.FileConversion.NumberOfLinMappingChannels + Constants.MaximumNumberOfLINprobeChannels; num2 += 1u)
			{
				this.mRepoItemDestChannelLin.Items.Add(GUIUtil.MapLINChannelNumber2String(num2, false));
			}
			this.mRepoItemDestChannelLin.Items.Add(Resources.Ignore);
			this.mRepoItemDestChannelFlexRay.Items.Clear();
			for (uint num3 = 1u; num3 <= this.mLoggerSpecifics.FileConversion.NumberOfFlexRayMappingChannels; num3 += 1u)
			{
				this.mRepoItemDestChannelFlexRay.Items.Add(GUIUtil.MapFlexrayABChannel2String(num3));
			}
			this.mRepoItemDestChannelFlexRay.Items.Add(Resources.Ignore);
		}

		private void ApplyParametersToControls()
		{
			this.mCheckBoxActivateMapping.Checked = this.mParameters.UseChannelMapping;
			this.mCheckBoxHideIdentities.Checked = this.mParameters.HideChannelMappingIdentities;
			this.UpdateChannelMappingGrid();
		}

		private void UpdateChannelMappingGrid()
		{
			this.mMappingList.Clear();
			if (this.mLoggerSpecifics == null || this.mParameters == null || this.mParameters.CanChannelMapping == null)
			{
				return;
			}
			uint num = this.mLoggerSpecifics.CAN.NumberOfChannels + this.mLoggerSpecifics.CAN.NumberOfVirtualChannels;
			uint num2 = 1u;
			while ((ulong)num2 <= (ulong)Math.Min((long)((ulong)num), (long)this.mParameters.CanChannelMapping.Length))
			{
				this.mMappingList.Add(new ChannelMapping.MappingPresenter(this.mLoggerSpecifics, this.mParameters, BusType.Bt_CAN, num2, this.mParameters.CanChannelMapping[(int)((UIntPtr)(num2 - 1u))], this.mLoggerSpecifics.FileConversion.NumberOfCanMappingChannels));
				num2 += 1u;
			}
			if (this.mLoggerSpecifics.Type != LoggerType.GL1020FTE)
			{
				uint numberOfChannels = this.mLoggerSpecifics.LIN.NumberOfChannels;
				uint num3 = 1u;
				while ((ulong)num3 <= (ulong)Math.Min((long)((ulong)(numberOfChannels + Constants.MaximumNumberOfLINprobeChannels)), (long)this.mParameters.LinChannelMapping.Length))
				{
					this.mMappingList.Add(new ChannelMapping.MappingPresenter(this.mLoggerSpecifics, this.mParameters, BusType.Bt_LIN, num3, this.mParameters.LinChannelMapping[(int)((UIntPtr)(num3 - 1u))], this.mLoggerSpecifics.FileConversion.NumberOfLinMappingChannels));
					num3 += 1u;
				}
				uint numberOfChannels2 = this.mLoggerSpecifics.Flexray.NumberOfChannels;
				uint num4 = 1u;
				while ((ulong)num4 <= (ulong)Math.Min((long)((ulong)numberOfChannels2), (long)this.mParameters.FlexRayChannelMapping.Length))
				{
					this.mMappingList.Add(new ChannelMapping.MappingPresenter(this.mLoggerSpecifics, this.mParameters, BusType.Bt_FlexRay, num4, this.mParameters.FlexRayChannelMapping[(int)((UIntPtr)(num4 - 1u))], this.mLoggerSpecifics.FileConversion.NumberOfFlexRayMappingChannels));
					num4 += 1u;
				}
			}
			foreach (ChannelMapping.MappingPresenter current in this.mMappingList)
			{
				current.MappingActive = this.mParameters.UseChannelMapping;
			}
			this.mGridViewMapping.RefreshData();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ChannelMapping));
			this.mGridControlMapping = new GridControl();
			this.mGridViewMapping = new GridView();
			this.colSourceChannel = new GridColumn();
			this.mRepoItemDestChannelCan = new RepositoryItemComboBox();
			this.colMapping = new GridColumn();
			this.mRepositoryItemPictureEdit = new RepositoryItemPictureEdit();
			this.colDestinationChannel = new GridColumn();
			this.mRepoItemDestChannelLin = new RepositoryItemComboBox();
			this.mRepoItemDestChannelFlexRay = new RepositoryItemComboBox();
			this.toolTipController = new XtraToolTipController(this.components);
			this.mCheckBoxActivateMapping = new CheckBox();
			this.mCheckBoxHideIdentities = new CheckBox();
			this.mButtonResetMapping = new Button();
			((ISupportInitialize)this.mGridControlMapping).BeginInit();
			((ISupportInitialize)this.mGridViewMapping).BeginInit();
			((ISupportInitialize)this.mRepoItemDestChannelCan).BeginInit();
			((ISupportInitialize)this.mRepositoryItemPictureEdit).BeginInit();
			((ISupportInitialize)this.mRepoItemDestChannelLin).BeginInit();
			((ISupportInitialize)this.mRepoItemDestChannelFlexRay).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mGridControlMapping, "mGridControlMapping");
			this.mGridControlMapping.MainView = this.mGridViewMapping;
			this.mGridControlMapping.Name = "mGridControlMapping";
			this.mGridControlMapping.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.mRepoItemDestChannelCan,
				this.mRepoItemDestChannelLin,
				this.mRepoItemDestChannelFlexRay,
				this.mRepositoryItemPictureEdit
			});
			this.mGridControlMapping.ToolTipController = this.toolTipController;
			this.mGridControlMapping.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewMapping
			});
			this.mGridViewMapping.Columns.AddRange(new GridColumn[]
			{
				this.colSourceChannel,
				this.colMapping,
				this.colDestinationChannel
			});
			this.mGridViewMapping.GridControl = this.mGridControlMapping;
			this.mGridViewMapping.Name = "mGridViewMapping";
			this.mGridViewMapping.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.mGridViewMapping.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.mGridViewMapping.OptionsCustomization.AllowColumnMoving = false;
			this.mGridViewMapping.OptionsCustomization.AllowColumnResizing = false;
			this.mGridViewMapping.OptionsCustomization.AllowFilter = false;
			this.mGridViewMapping.OptionsCustomization.AllowGroup = false;
			this.mGridViewMapping.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridViewMapping.OptionsCustomization.AllowSort = false;
			this.mGridViewMapping.OptionsMenu.EnableColumnMenu = false;
			this.mGridViewMapping.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.mGridViewMapping.OptionsSelection.EnableAppearanceFocusedRow = false;
			this.mGridViewMapping.OptionsSelection.UseIndicatorForSelection = false;
			this.mGridViewMapping.OptionsView.ShowGroupPanel = false;
			this.mGridViewMapping.OptionsView.ShowIndicator = false;
			this.mGridViewMapping.PaintStyleName = "WindowsXP";
			this.mGridViewMapping.RowHeight = 20;
			this.mGridViewMapping.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.mGridViewMapping_CustomRowCellEdit);
			this.mGridViewMapping.CustomRowFilter += new RowFilterEventHandler(this.mGridViewMapping_CustomRowFilter);
			componentResourceManager.ApplyResources(this.colSourceChannel, "colSourceChannel");
			this.colSourceChannel.ColumnEdit = this.mRepoItemDestChannelCan;
			this.colSourceChannel.FieldName = "SourceChannelString";
			this.colSourceChannel.MinWidth = 50;
			this.colSourceChannel.Name = "colSourceChannel";
			this.colSourceChannel.OptionsColumn.AllowEdit = false;
			componentResourceManager.ApplyResources(this.mRepoItemDestChannelCan, "mRepoItemDestChannelCan");
			this.mRepoItemDestChannelCan.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepoItemDestChannelCan.Buttons"))
			});
			this.mRepoItemDestChannelCan.Name = "mRepoItemDestChannelCan";
			this.mRepoItemDestChannelCan.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepoItemDestChannelCan.SelectedIndexChanged += new EventHandler(this.mRepoItemDestChannelCan_SelectedIndexChanged);
			this.colMapping.ColumnEdit = this.mRepositoryItemPictureEdit;
			this.colMapping.FieldName = "Image";
			this.colMapping.MaxWidth = 30;
			this.colMapping.MinWidth = 30;
			this.colMapping.Name = "colMapping";
			this.colMapping.OptionsColumn.AllowEdit = false;
			this.colMapping.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.colMapping, "colMapping");
			this.mRepositoryItemPictureEdit.Name = "mRepositoryItemPictureEdit";
			this.mRepositoryItemPictureEdit.ReadOnly = true;
			componentResourceManager.ApplyResources(this.colDestinationChannel, "colDestinationChannel");
			this.colDestinationChannel.ColumnEdit = this.mRepoItemDestChannelCan;
			this.colDestinationChannel.FieldName = "DestinationChannelString";
			this.colDestinationChannel.MinWidth = 50;
			this.colDestinationChannel.Name = "colDestinationChannel";
			this.colDestinationChannel.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.mRepoItemDestChannelLin, "mRepoItemDestChannelLin");
			this.mRepoItemDestChannelLin.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepoItemDestChannelLin.Buttons"))
			});
			this.mRepoItemDestChannelLin.Name = "mRepoItemDestChannelLin";
			this.mRepoItemDestChannelLin.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepoItemDestChannelLin.SelectedIndexChanged += new EventHandler(this.mRepoItemDestChannelLin_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.mRepoItemDestChannelFlexRay, "mRepoItemDestChannelFlexRay");
			this.mRepoItemDestChannelFlexRay.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepoItemDestChannelFlexRay.Buttons"))
			});
			this.mRepoItemDestChannelFlexRay.Name = "mRepoItemDestChannelFlexRay";
			this.mRepoItemDestChannelFlexRay.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepoItemDestChannelFlexRay.SelectedIndexChanged += new EventHandler(this.mRepoItemDestChannelFlexRay_SelectedIndexChanged);
			this.toolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.BackColor");
			this.toolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.ForeColor");
			this.toolTipController.Appearance.Options.UseBackColor = true;
			this.toolTipController.Appearance.Options.UseForeColor = true;
			this.toolTipController.Appearance.Options.UseTextOptions = true;
			this.toolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.BackColor");
			this.toolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.ForeColor");
			this.toolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.toolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.toolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.toolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.MaxWidth = 500;
			this.toolTipController.ShowPrefix = true;
			this.toolTipController.UseNativeLookAndFeel = true;
			componentResourceManager.ApplyResources(this.mCheckBoxActivateMapping, "mCheckBoxActivateMapping");
			this.mCheckBoxActivateMapping.Name = "mCheckBoxActivateMapping";
			this.mCheckBoxActivateMapping.UseVisualStyleBackColor = true;
			this.mCheckBoxActivateMapping.CheckedChanged += new EventHandler(this.mCheckBoxActivateMapping_CheckedChanged);
			componentResourceManager.ApplyResources(this.mCheckBoxHideIdentities, "mCheckBoxHideIdentities");
			this.mCheckBoxHideIdentities.Name = "mCheckBoxHideIdentities";
			this.mCheckBoxHideIdentities.UseVisualStyleBackColor = true;
			this.mCheckBoxHideIdentities.CheckedChanged += new EventHandler(this.mCheckBoxHideIdentities_CheckedChanged);
			componentResourceManager.ApplyResources(this.mButtonResetMapping, "mButtonResetMapping");
			this.mButtonResetMapping.Name = "mButtonResetMapping";
			this.mButtonResetMapping.UseVisualStyleBackColor = true;
			this.mButtonResetMapping.Click += new EventHandler(this.mButtonResetMapping_Click);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.mButtonResetMapping);
			base.Controls.Add(this.mCheckBoxHideIdentities);
			base.Controls.Add(this.mCheckBoxActivateMapping);
			base.Controls.Add(this.mGridControlMapping);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ChannelMapping";
			((ISupportInitialize)this.mGridControlMapping).EndInit();
			((ISupportInitialize)this.mGridViewMapping).EndInit();
			((ISupportInitialize)this.mRepoItemDestChannelCan).EndInit();
			((ISupportInitialize)this.mRepositoryItemPictureEdit).EndInit();
			((ISupportInitialize)this.mRepoItemDestChannelLin).EndInit();
			((ISupportInitialize)this.mRepoItemDestChannelFlexRay).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
