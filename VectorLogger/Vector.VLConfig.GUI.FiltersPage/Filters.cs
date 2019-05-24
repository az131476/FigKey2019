using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.FiltersPage
{
	public class Filters : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<FilterConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver, ISplitButtonExClient
	{
		public delegate void DataChangedHandler(FilterConfiguration data);

		private SelectColumnInCSVFile selectColumnDialog;

		private LoggerType loggerType;

		private readonly SplitButtonEx mSplitButtonEx;

		private IContainer components;

		private GroupBox groupBoxFilter;

		private Label label1;

		private SplitButton mSplitButton;

		private Button buttonRemoveFilter;

		private FilterGrid filterGrid;

		private Label labelFilters;

		private Label labelMemoryInactive;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.filterGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.filterGrid.ApplicationDatabaseManager = value;
			}
		}

		public GlobalOptions GlobalOptions
		{
			get;
			set;
		}

		public int MemoryNr
		{
			get;
			set;
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.filterGrid.ModelValidator;
			}
			set
			{
				this.filterGrid.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get;
			set;
		}

		IUpdateService IPropertyWindow.UpdateService
		{
			get;
			set;
		}

		IUpdateObserver IPropertyWindow.UpdateObserver
		{
			get
			{
				return this;
			}
		}

		PageType IPropertyWindow.Type
		{
			get
			{
				return PageTypeHelper.GetFilterPageForMemory(this.MemoryNr);
			}
		}

		bool IPropertyWindow.IsVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				bool visible = base.Visible;
				base.Visible = value;
				if (!visible && base.Visible)
				{
					this.filterGrid.DisplayErrors();
				}
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public SplitButton SplitButton
		{
			get
			{
				return this.mSplitButton;
			}
		}

		public string SplitButtonEmptyDefault
		{
			get
			{
				return Resources.SplitButtonEmptyDefault;
			}
		}

		public Filters()
		{
			this.InitializeComponent();
			this.filterGrid.SelectionChanged += new EventHandler(this.OnFilterGridSelectionChanged);
			this.selectColumnDialog = new SelectColumnInCSVFile();
			this.mSplitButtonEx = new SplitButtonEx(this);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is FilterConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContextHelper.GetFilterUpdateContextForForMemoryNr(this.MemoryNr));
			}
			GUIUtil.InitSplitButtonMenuFilterTypes(this.mSplitButtonEx);
			this.filterGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.mSplitButtonEx.UpdateSplitMenu();
			this.filterGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return (long)this.MemoryNr > (long)((ulong)this.filterGrid.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories) || !this.filterGrid.Enabled || this.filterGrid.ValidateInput(false);
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.filterGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.filterGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.filterGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.filterGrid.HasFormatErrors();
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
		}

		void IUpdateObserver<FilterConfiguration>.Update(FilterConfiguration data)
		{
			if ((long)this.MemoryNr > (long)((ulong)this.filterGrid.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories))
			{
				return;
			}
			if (data.MemoryNr == this.MemoryNr)
			{
				if (this.filterGrid.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					this.groupBoxFilter.Text = string.Format(Resources.FiltersOnMem, this.MemoryNr);
				}
				else
				{
					this.groupBoxFilter.Text = Resources.Filters;
				}
				this.filterGrid.FilterConfiguration = data;
				this.EnableButtonRemove();
				this.mSplitButtonEx.UpdateSplitMenu();
				if (this.filterGrid.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					bool flag = this.filterGrid.ModelValidator.GetActiveMemoryNumbers.Contains(this.MemoryNr);
					if (!flag)
					{
						this.filterGrid.Reset();
					}
					this.filterGrid.Enabled = flag;
					this.mSplitButton.Enabled = flag;
					this.EnableButtonRemove();
					this.labelFilters.Visible = flag;
					this.labelMemoryInactive.Visible = !flag;
				}
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode data)
		{
			this.filterGrid.DisplayMode = data;
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return ConfigClipboardManager.AcceptType.None;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return false;
		}

		public bool Insert(Event evt)
		{
			return false;
		}

		private void OnFilterGridSelectionChanged(object sender, EventArgs e)
		{
			this.EnableButtonRemove();
		}

		private void buttonRemoveFilter_Click(object sender, EventArgs e)
		{
			Filter filter;
			if (this.filterGrid.TryGetSelectedFilter(out filter))
			{
				this.filterGrid.RemoveFilter(filter);
			}
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			if (text == Resources.FilterTypeNameCANChn || text == Resources.FilterTypeNameSymbolicCAN || text == Resources.FilterTypeNameCANId)
			{
				return this.filterGrid.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN) > 0u;
			}
			if (text == Resources.FilterTypeNameLINChn || text == Resources.FilterTypeNameSymbolicLIN || text == Resources.FilterTypeNameLINId)
			{
				return this.filterGrid.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN) > 0u;
			}
			if (text == Resources.FilterTypeNameFlexRayChn || text == Resources.FilterTypeNameSymbolicFlexray || text == Resources.FilterTypeNameFlexrayId)
			{
				return this.filterGrid.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay) > 0u;
			}
			return text == Resources.FilterTypeNameCANSignalList && this.GlobalOptions.EnableSignalListFilterFeature && this.filterGrid.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN) > 0u;
		}

		public void ItemClicked(ToolStripItem item)
		{
			this.AddItem(item.Text);
		}

		public void DefaultActionClicked()
		{
			this.AddItem(this.mSplitButtonEx.DefaultAction);
		}

		private void AddItem(string itemText)
		{
			List<Filter> list = this.CreateFilter(itemText);
			if (list == null)
			{
				return;
			}
			foreach (Filter current in list)
			{
				this.filterGrid.AddFilter(current);
			}
		}

		private List<Filter> CreateFilter(string selectedFilterName)
		{
			List<Filter> list = new List<Filter>();
			if (selectedFilterName == Resources.FilterTypeNameCANChn)
			{
				Filter filter = this.CreateChannelFilter(BusType.Bt_CAN);
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameLINChn)
			{
				Filter filter = this.CreateChannelFilter(BusType.Bt_LIN);
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameFlexRayChn)
			{
				Filter filter = this.CreateChannelFilter(BusType.Bt_FlexRay);
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameSymbolicCAN)
			{
				list = this.CreateSymbolicMessageFilter(BusType.Bt_CAN);
			}
			else if (selectedFilterName == Resources.FilterTypeNameSymbolicLIN)
			{
				list = this.CreateSymbolicMessageFilter(BusType.Bt_LIN);
			}
			else if (selectedFilterName == Resources.FilterTypeNameSymbolicFlexray)
			{
				list = this.CreateSymbolicMessageFilter(BusType.Bt_FlexRay);
			}
			else if (selectedFilterName == Resources.FilterTypeNameCANId)
			{
				Filter filter = this.CreateCANIdFilter();
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameLINId)
			{
				Filter filter = this.CreateLINIdFilter();
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameFlexrayId)
			{
				Filter filter = this.CreateFlexrayIdFilter();
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			else if (selectedFilterName == Resources.FilterTypeNameCANSignalList)
			{
				Filter filter = this.CreateSignalListFilter(BusType.Bt_CAN);
				if (filter != null)
				{
					list.Add(filter);
				}
			}
			if (list != null)
			{
				foreach (Filter current in list)
				{
					current.LimitIntervalPerFrame.Value = Constants.DefaultLimitInterval_ms;
					current.Action.Value = ((this.filterGrid.FilterConfiguration.DefaultFilter.Action.Value == FilterActionType.Pass) ? FilterActionType.Stop : FilterActionType.Pass);
				}
			}
			return list;
		}

		private ChannelFilter CreateChannelFilter(BusType busType)
		{
			return new ChannelFilter
			{
				BusType = 
				{
					Value = busType
				},
				ChannelNumber = 
				{
					Value = this.filterGrid.ModelValidator.GetFirstActiveOrDefaultChannel(busType)
				},
				Action = 
				{
					Value = FilterActionType.Stop
				}
			};
		}

		private List<Filter> CreateSymbolicMessageFilter(BusType busType)
		{
			string[] array = null;
			string[] array2 = null;
			string[] array3 = null;
			string[] array4 = null;
			BusType[] array5 = null;
			bool[] array6 = null;
			if (!this.filterGrid.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				if (busType == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			if (this.filterGrid.ApplicationDatabaseManager.SelectMessageInDatabase(busType, ref array, ref array3, ref array2, ref array4, ref array5, ref array6, true))
			{
				List<Filter> list = new List<Filter>();
				new List<string>();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					string value;
					if (!this.filterGrid.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(array[i], array4[i], array2[i], array5[i], out value))
					{
						stringBuilder.AppendLine(value);
						stringBuilder.AppendLine();
					}
					else
					{
						SymbolicMessageFilter symbolicMessageFilter = new SymbolicMessageFilter();
						symbolicMessageFilter.Action.Value = FilterActionType.Stop;
						symbolicMessageFilter.MessageName.Value = array[i];
						symbolicMessageFilter.DatabaseName.Value = array3[i];
						symbolicMessageFilter.DatabasePath.Value = this.filterGrid.ModelValidator.GetFilePathRelativeToConfiguration(array2[i]);
						symbolicMessageFilter.IsFlexrayPDU.Value = array6[i];
						symbolicMessageFilter.NetworkName.Value = array4[i];
						symbolicMessageFilter.BusType.Value = busType;
						symbolicMessageFilter.ChannelNumber.Value = 1u;
						IList<uint> channelAssignmentOfDatabase = this.filterGrid.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageFilter.DatabasePath.Value, symbolicMessageFilter.NetworkName.Value);
						if (channelAssignmentOfDatabase.Count > 0)
						{
							symbolicMessageFilter.ChannelNumber.Value = channelAssignmentOfDatabase[0];
							if (symbolicMessageFilter.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
							{
								symbolicMessageFilter.ChannelNumber.Value = 1u;
								if (array[i].EndsWith(Constants.FlexrayChannelB_Postfix))
								{
									symbolicMessageFilter.ChannelNumber.Value = 2u;
								}
							}
						}
						list.Add(symbolicMessageFilter);
					}
				}
				if (stringBuilder.Length > 0)
				{
					InformMessageBox.Error(stringBuilder.ToString());
				}
				return list;
			}
			return null;
		}

		private CANIdFilter CreateCANIdFilter()
		{
			CANIdCondition cANIdCondition = new CANIdCondition();
			if (DialogResult.OK == cANIdCondition.ShowDialog())
			{
				return new CANIdFilter
				{
					ChannelNumber = 
					{
						Value = this.filterGrid.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN)
					},
					Action = 
					{
						Value = FilterActionType.Stop
					},
					IsExtendedId = 
					{
						Value = cANIdCondition.IsExtendedId
					},
					CANId = 
					{
						Value = cANIdCondition.MessageID
					},
					IsIdRange = 
					{
						Value = cANIdCondition.IsRange
					},
					CANIdLast = 
					{
						Value = cANIdCondition.LastMessageID
					}
				};
			}
			return null;
		}

		private LINIdFilter CreateLINIdFilter()
		{
			LINIdCondition lINIdCondition = new LINIdCondition();
			if (DialogResult.OK == lINIdCondition.ShowDialog())
			{
				return new LINIdFilter
				{
					ChannelNumber = 
					{
						Value = this.filterGrid.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_LIN)
					},
					Action = 
					{
						Value = FilterActionType.Stop
					},
					LINId = 
					{
						Value = lINIdCondition.MessageID
					},
					IsIdRange = 
					{
						Value = lINIdCondition.IsRange
					},
					LINIdLast = 
					{
						Value = lINIdCondition.LastMessageID
					}
				};
			}
			return null;
		}

		private FlexrayIdFilter CreateFlexrayIdFilter()
		{
			FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition();
			if (DialogResult.OK == flexrayIdCondition.ShowDialog())
			{
				return new FlexrayIdFilter
				{
					ChannelNumber = 
					{
						Value = this.filterGrid.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_FlexRay)
					},
					Action = 
					{
						Value = FilterActionType.Stop
					},
					FlexrayId = 
					{
						Value = flexrayIdCondition.FrameId
					},
					FlexrayIdLast = 
					{
						Value = flexrayIdCondition.LastFrameId
					},
					IsIdRange = 
					{
						Value = flexrayIdCondition.IsIdRange
					},
					BaseCycle = 
					{
						Value = flexrayIdCondition.CycleTime
					},
					CycleRepetition = 
					{
						Value = flexrayIdCondition.CycleRepetiton
					}
				};
			}
			return null;
		}

		private SignalListFileFilter CreateSignalListFilter(BusType busType)
		{
			if (DialogResult.OK != GenericOpenFileDialog.ShowDialog(FileType.CSVFile))
			{
				return null;
			}
			SignalListFileFilter signalListFileFilter = new SignalListFileFilter(busType);
			signalListFileFilter.ChannelNumber.Value = this.filterGrid.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			signalListFileFilter.Action.Value = FilterActionType.Stop;
			signalListFileFilter.FilePath.Value = this.filterGrid.ModelValidator.GetFilePathRelativeToConfiguration(GenericOpenFileDialog.FileName);
			uint num = 0u;
			string previewText;
			if (FileSystemServices.GetNumberOfColumnsInCSVFile(GenericOpenFileDialog.FileName, out num, out previewText))
			{
				if (num > 1u)
				{
					this.selectColumnDialog.TotalNumberOfColumns = num;
					this.selectColumnDialog.Filename = Path.GetFileName(signalListFileFilter.FilePath.Value);
					this.selectColumnDialog.PreviewText = previewText;
					if (DialogResult.OK != this.selectColumnDialog.ShowDialog())
					{
						return null;
					}
					signalListFileFilter.Column.Value = this.selectColumnDialog.SelectedColumn - 1u;
				}
				return signalListFileFilter;
			}
			string message = string.Format(Resources.ErrorUnableToReadFile, signalListFileFilter.FilePath.Value);
			InformMessageBox.Error(message);
			return null;
		}

		private void EnableButtonRemove()
		{
			if (!this.filterGrid.Enabled)
			{
				this.buttonRemoveFilter.Enabled = false;
				return;
			}
			Filter filter;
			if (this.filterGrid.TryGetSelectedFilter(out filter))
			{
				this.buttonRemoveFilter.Enabled = !(filter is DefaultFilter);
				return;
			}
			this.buttonRemoveFilter.Enabled = false;
		}

		public bool Serialize(FiltersPage filtersPage)
		{
			if (filtersPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.filterGrid.Serialize(filtersPage);
		}

		public bool DeSerialize(FiltersPage filtersPage)
		{
			if (filtersPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.filterGrid.DeSerialize(filtersPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Filters));
			this.groupBoxFilter = new GroupBox();
			this.buttonRemoveFilter = new Button();
			this.mSplitButton = new SplitButton();
			this.label1 = new Label();
			this.labelMemoryInactive = new Label();
			this.labelFilters = new Label();
			this.filterGrid = new FilterGrid();
			this.groupBoxFilter.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxFilter, "groupBoxFilter");
			this.groupBoxFilter.Controls.Add(this.buttonRemoveFilter);
			this.groupBoxFilter.Controls.Add(this.mSplitButton);
			this.groupBoxFilter.Controls.Add(this.label1);
			this.groupBoxFilter.Controls.Add(this.labelMemoryInactive);
			this.groupBoxFilter.Controls.Add(this.labelFilters);
			this.groupBoxFilter.Controls.Add(this.filterGrid);
			this.groupBoxFilter.Name = "groupBoxFilter";
			this.groupBoxFilter.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonRemoveFilter, "buttonRemoveFilter");
			this.buttonRemoveFilter.Image = Resources.ImageDelete;
			this.buttonRemoveFilter.Name = "buttonRemoveFilter";
			this.buttonRemoveFilter.UseVisualStyleBackColor = true;
			this.buttonRemoveFilter.Click += new EventHandler(this.buttonRemoveFilter_Click);
			componentResourceManager.ApplyResources(this.mSplitButton, "mSplitButton");
			this.mSplitButton.Name = "mSplitButton";
			this.mSplitButton.ShowSplitAlways = true;
			this.mSplitButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.labelMemoryInactive, "labelMemoryInactive");
			this.labelMemoryInactive.Name = "labelMemoryInactive";
			componentResourceManager.ApplyResources(this.labelFilters, "labelFilters");
			this.labelFilters.Name = "labelFilters";
			componentResourceManager.ApplyResources(this.filterGrid, "filterGrid");
			this.filterGrid.ApplicationDatabaseManager = null;
			this.filterGrid.DisplayMode = null;
			this.filterGrid.FilterConfiguration = null;
			this.filterGrid.ModelValidator = null;
			this.filterGrid.Name = "filterGrid";
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxFilter);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "Filters";
			this.groupBoxFilter.ResumeLayout(false);
			this.groupBoxFilter.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
