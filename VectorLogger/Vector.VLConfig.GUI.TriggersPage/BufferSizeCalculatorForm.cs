using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI.XtraGridUtils;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class BufferSizeCalculatorForm : Form
	{
		private readonly IConfigurationManagerService mConfigurationManagerService;

		private readonly BufferSizeCalculator mCalculator;

		private readonly BindingList<ChannelItem> mBindingListCanLin = new BindingList<ChannelItem>();

		private readonly GeneralService mGridViewCanLinGeneralService;

		private readonly KeyboardNavigationService mGridViewCanLinKeyboardNavigationService;

		private readonly BindingList<ChannelItem> mBindingListFlexRay = new BindingList<ChannelItem>();

		private readonly GeneralService mGridViewFlexRayGeneralService;

		private readonly KeyboardNavigationService mGridViewFlexRayKeyboardNavigationService;

		private IContainer components;

		private Button mButtonCancel;

		private Button mButtonOK;

		private Label mLabelEstiamtedRingBufferSize;

		private GridControl mGridControlCanLin;

		private GridView mGridViewCanLin;

		private GridColumn mColCanLinChannel;

		private GridColumn mColCanLinBusload;

		private GridColumn mColCanLinInfo;

		private Label mLabelActiveChannelsCanLin;

		private Label label2;

		private TextBox mTextBoxPreTriggerTime;

		private Label label3;

		private Label mLabelMaxSizeExceeded;

		private ErrorProvider mErrorProvider;

		private Label mLabelPostTriggerTime;

		private Label mLabelDigitalInputs;

		private Label mLabelAnalogInputs;

		private TitledGroup mTitledGroupTriggerTimes;

		private TitledGroup mTitledGroupDataRate;

		private TitledGroup mTitledGroupResult;

		private Panel mPanelTriggerTimes;

		private Panel mPanelResult;

		private Panel mPanelDataRate;

		private TableLayoutPanel mTableLayoutPanelChannels;

		private GridControl mGridControlFlexRay;

		private GridView mGridViewFlexRay;

		private GridColumn mColFlexrayChannel;

		private GridColumn mColFlexrayFrames;

		private GridColumn mColFlexrayPayload;

		private Label mLabelActiveChannelsFlexray;

		private GridColumn mColFlexrayInfo;

		private XtraToolTipController mXtraToolTipController;

		private Button mButtonHelp;

		private TableLayoutPanel tableLayoutPanel1;

		private ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.mConfigurationManagerService.LoggerSpecifics;
			}
		}

		public bool MetaInformationChanged
		{
			get
			{
				return this.mCalculator.MetaInformationChanged;
			}
		}

		public uint EstimatedRingBufferSizeMB
		{
			get
			{
				return this.mCalculator.EstimatedRingBufferSizeMB;
			}
		}

		public BufferSizeCalculatorForm(IConfigurationManagerService configurationManagerService, uint postTriggerTimeMilliseconds)
		{
			this.mConfigurationManagerService = configurationManagerService;
			this.mCalculator = new BufferSizeCalculator(this.mConfigurationManagerService, postTriggerTimeMilliseconds);
			this.InitializeComponent();
			this.mGridViewCanLinKeyboardNavigationService = new KeyboardNavigationService(this.mGridViewCanLin);
			this.mGridViewCanLinGeneralService = new GeneralService(this.mGridViewCanLin);
			this.mGridViewFlexRayKeyboardNavigationService = new KeyboardNavigationService(this.mGridViewFlexRay);
			this.mGridViewFlexRayGeneralService = new GeneralService(this.mGridViewFlexRay);
			BaseEdit.DefaultErrorIcon = MainImageList.Instance.GetImage(MainImageList.IconIndex.Error);
		}

		private void BufferSizeCalculator_Load(object sender, EventArgs e)
		{
			this.InitGui();
			this.mTextBoxPreTriggerTime.Text = this.mCalculator.PreTriggerTimeSeconds.ToString(CultureInfo.InvariantCulture);
		}

		private void InitGui()
		{
			this.mErrorProvider.SetIconAlignment(this.mTextBoxPreTriggerTime, ErrorIconAlignment.MiddleLeft);
			bool flag = this.mCalculator.NumberOfConfiguredCANChannels > 0u;
			bool flag2 = this.mCalculator.NumberOfConfiguredLINChannels > 0u;
			bool flag3 = this.mCalculator.NumberOfConfiguredFlexrayChannels > 0u;
			this.mLabelActiveChannelsCanLin.Text = string.Format(Resources_Trigger.RingBufferCalculationActiveChannelsWithBusType, (flag && flag2) ? Vocabulary.CAN_LIN : (flag ? Vocabulary.CAN : Vocabulary.LIN));
			this.mGridViewCanLinGeneralService.InitAppearance();
			this.mGridControlCanLin.DataSource = this.mBindingListCanLin;
			using (new GridBatchUpdate(this.mGridViewCanLin))
			{
				this.mBindingListCanLin.Clear();
				ReadOnlyCollection<ChannelItem> activeChannelItemsCanLin = this.mCalculator.ActiveChannelItemsCanLin;
				foreach (ChannelItem current in activeChannelItemsCanLin)
				{
					this.mBindingListCanLin.Add(current);
				}
			}
			this.mGridViewCanLin.BestFitColumns();
			this.mLabelActiveChannelsFlexray.Text = string.Format(Resources_Trigger.RingBufferCalculationActiveChannelsWithBusType, Vocabulary.Flexray);
			this.mGridControlFlexRay.TabStop = flag3;
			this.mGridViewFlexRayGeneralService.InitAppearance();
			this.mGridControlFlexRay.DataSource = this.mBindingListFlexRay;
			using (new GridBatchUpdate(this.mGridViewFlexRay))
			{
				this.mBindingListFlexRay.Clear();
				ReadOnlyCollection<ChannelItem> activeChannelItemsFlexray = this.mCalculator.ActiveChannelItemsFlexray;
				foreach (ChannelItem current2 in activeChannelItemsFlexray)
				{
					this.mBindingListFlexRay.Add(current2);
				}
			}
			this.mGridViewFlexRay.BestFitColumns();
			int height = base.Height;
			base.Height = this.MinimumSize.Height;
			GridViewInfo gridViewInfo = (GridViewInfo)this.mGridViewCanLin.GetViewInfo();
			int num = (gridViewInfo.ColumnRowHeight + 1) * 3 - this.mGridControlCanLin.Height;
			this.MinimumSize = new Size(this.MinimumSize.Width, this.MinimumSize.Height + num);
			base.Height = height;
			if (!flag3)
			{
				int height2 = this.mGridControlCanLin.Height;
				base.Height = this.MinimumSize.Height;
				int height3 = this.mGridControlCanLin.Height;
				this.mTableLayoutPanelChannels.SuspendLayout();
				this.mTableLayoutPanelChannels.RowStyles[1].Height = 100f;
				this.mTableLayoutPanelChannels.RowStyles[2].Height = 0f;
				this.mTableLayoutPanelChannels.RowStyles[3].SizeType = SizeType.Absolute;
				this.mTableLayoutPanelChannels.RowStyles[3].Height = 0f;
				this.mTableLayoutPanelChannels.RowStyles[4].Height = 0f;
				this.mTableLayoutPanelChannels.ResumeLayout(true);
				int num2 = this.mGridControlCanLin.Height - height3;
				this.MinimumSize = new Size(this.MinimumSize.Width, this.MinimumSize.Height - num2);
				base.Height = height;
				int num3 = this.mGridControlCanLin.Height - height2;
				base.Height -= num3;
			}
			int num4 = this.mConfigurationManagerService.AnalogInputConfiguration.AnalogInputs.Count((AnalogInput t) => t.IsActive.Value);
			this.mLabelAnalogInputs.Text = ((num4 > 0) ? string.Format(Resources_Trigger.RingBufferCalculationAnalogInputsWithDataRate, num4, this.MsgsPerSec(this.mCalculator.MsgsPerSecAnalog), BufferSizeCalculator.DataRate(this.mCalculator.MsgsPerSecAnalog * 20u)) : string.Format(Resources_Trigger.RingBufferCalculationAnalogInputs, num4));
			int num5 = this.mConfigurationManagerService.DigitalInputConfiguration.DigitalInputs.Count((DigitalInput t) => t.IsActiveFrequency.Value || t.IsActiveOnChange.Value);
			this.mLabelDigitalInputs.Text = ((num5 > 0) ? string.Format(Resources_Trigger.RingBufferCalculationDigitalInputsWithDataRate, num5, this.MsgsPerSec(this.mCalculator.MsgsPerSecDigital), BufferSizeCalculator.DataRate(this.mCalculator.MsgsPerSecDigital * 20u)) : string.Format(Resources_Trigger.RingBufferCalculationDigitalInputs, num5));
			if (this.LoggerSpecifics.IO.NumberOfDigitalInputs == 0u)
			{
				int num6 = this.mLabelDigitalInputs.Top - this.mLabelAnalogInputs.Top;
				this.mLabelDigitalInputs.Visible = false;
				this.mLabelAnalogInputs.Top += num6;
				this.mTableLayoutPanelChannels.Height += num6;
				base.Height -= num6;
				this.MinimumSize = new Size(this.MinimumSize.Width, this.MinimumSize.Height - num6);
			}
			this.mLabelPostTriggerTime.Text = string.Format(Resources_Trigger.RingBufferCalculationPostTriggerTime, this.mCalculator.PostTriggerTimeMilliseconds);
			if (this.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly)
			{
				num = this.mLabelPostTriggerTime.Bottom - this.label2.Bottom;
				this.MinimumSize = new Size(this.MinimumSize.Width, this.MinimumSize.Height - num);
				this.mPanelTriggerTimes.Height -= num;
				this.mPanelDataRate.Top -= num;
				this.mPanelDataRate.Height += num;
				base.Height -= num;
				this.mLabelPostTriggerTime.Visible = false;
			}
			this.mLabelMaxSizeExceeded.Text = string.Format(Resources_Trigger.RingBufferCalculationMaxSizeExceeded, this.LoggerSpecifics.DataStorage.MaxRingBufferSize / 1024u);
		}

		private string MsgsPerSec(uint msgsPerSec)
		{
			return msgsPerSec + " msgs/s";
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			this.mCalculator.UpdateModel();
			base.Close();
		}

		private void TextBoxPreTriggerTime_TextChanged(object sender, EventArgs e)
		{
			int num;
			this.mCalculator.PreTriggerTimeSeconds = (uint)(this.ParseInt32(this.mTextBoxPreTriggerTime.Text, 0, out num, 0, 86400) ? num : 0);
			this.Calculate();
		}

		private void TextBoxPreTriggerTime_Validating(object sender, CancelEventArgs e)
		{
			int preTriggerTimeSeconds;
			if (!this.ParseInt32(this.mTextBoxPreTriggerTime.Text, 0, out preTriggerTimeSeconds, 0, 86400))
			{
				this.mButtonOK.Enabled = false;
				this.mErrorProvider.SetError(this.mTextBoxPreTriggerTime, string.Format(Resources.ErrorUIntExpectedWithMaxVal, 86400));
				this.mCalculator.PreTriggerTimeSeconds = 0u;
			}
			else
			{
				this.mButtonOK.Enabled = true;
				this.mErrorProvider.SetError(this.mTextBoxPreTriggerTime, string.Empty);
				this.mCalculator.PreTriggerTimeSeconds = (uint)preTriggerTimeSeconds;
				this.mTextBoxPreTriggerTime.Text = preTriggerTimeSeconds.ToString(CultureInfo.InvariantCulture);
			}
			this.Calculate();
		}

		private void GridControl_ProcessGridKey(object sender, KeyEventArgs e)
		{
			if (sender == this.mGridControlCanLin)
			{
				this.mGridViewCanLinKeyboardNavigationService.GridControlProcessGridKey(e);
				return;
			}
			if (sender == this.mGridControlFlexRay)
			{
				this.mGridViewFlexRayKeyboardNavigationService.GridControlProcessGridKey(e);
			}
		}

		private void GridControl_EditorKeyDown(object sender, KeyEventArgs e)
		{
			if (sender == this.mGridControlCanLin)
			{
				this.mGridViewCanLinKeyboardNavigationService.GridControlEditorKeyDown(e);
				return;
			}
			if (sender == this.mGridControlFlexRay)
			{
				this.mGridViewFlexRayKeyboardNavigationService.GridControlEditorKeyDown(e);
			}
		}

		private void GridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (sender == this.mGridViewCanLin)
			{
				this.mGridViewCanLinGeneralService.PopupMenuShowing(e);
			}
			if (sender == this.mGridViewFlexRay)
			{
				this.mGridViewFlexRayGeneralService.PopupMenuShowing(e);
			}
			if (e.Menu is GridViewColumnMenu)
			{
				foreach (DXMenuItem dXMenuItem in e.Menu.Items)
				{
					GridStringId gridStringId = (GridStringId)dXMenuItem.Tag;
					if (gridStringId == GridStringId.MenuColumnClearSorting)
					{
						dXMenuItem.Visible = false;
						break;
					}
				}
			}
		}

		private void GridView_ShownEditor(object sender, EventArgs e)
		{
			if (sender == this.mGridViewCanLin)
			{
				this.mGridViewCanLinKeyboardNavigationService.GridViewShownEditor();
				return;
			}
			if (sender == this.mGridViewFlexRay)
			{
				this.mGridViewFlexRayKeyboardNavigationService.GridViewShownEditor();
			}
		}

		private void GridView_HiddenEditor(object sender, EventArgs e)
		{
			this.Calculate();
		}

		private void GridView_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
		{
			if (sender == this.mGridViewCanLin)
			{
				if (this.mGridViewCanLin.FocusedColumn == this.mColCanLinBusload)
				{
					e.Value = this.ParseInt32(e.Value as string, 50, 0, 100);
					return;
				}
			}
			else if (sender == this.mGridViewFlexRay)
			{
				if (this.mGridViewFlexRay.FocusedColumn == this.mColFlexrayFrames)
				{
					e.Value = this.ParseInt32(e.Value as string, 1000, 0, 2147483647);
					return;
				}
				if (this.mGridViewFlexRay.FocusedColumn == this.mColFlexrayPayload)
				{
					e.Value = this.ParseInt32(e.Value as string, 42, 0, 255);
				}
			}
		}

		private string ParseInt32(string strVal, int defaultVal, int minVal = -2147483648, int maxVal = 2147483647)
		{
			int num;
			if (!this.ParseInt32(strVal, defaultVal, out num, minVal, maxVal))
			{
				return defaultVal.ToString(CultureInfo.InvariantCulture);
			}
			return num.ToString(CultureInfo.InvariantCulture);
		}

		private bool ParseInt32(string strVal, int defaultVal, out int intVal, int minVal = -2147483648, int maxVal = 2147483647)
		{
			intVal = defaultVal;
			if (strVal == null)
			{
				return false;
			}
			if (strVal == string.Empty)
			{
				return true;
			}
			if (!int.TryParse(strVal, out intVal))
			{
				return false;
			}
			if (intVal < minVal)
			{
				intVal = minVal;
			}
			if (intVal > maxVal)
			{
				intVal = maxVal;
			}
			return true;
		}

		private void BufferSizeCalculator_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void Calculate()
		{
			bool flag = this.mCalculator.Calculate();
			this.mLabelEstiamtedRingBufferSize.Text = string.Format(Vocabulary.RingBufferCalculationEstimatedSize, this.EstimatedRingBufferSizeMB);
			this.mLabelMaxSizeExceeded.Visible = !flag;
			this.mButtonOK.Enabled = (flag && string.IsNullOrEmpty(this.mErrorProvider.GetError(this.mTextBoxPreTriggerTime)));
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BufferSizeCalculatorForm));
			this.mButtonCancel = new Button();
			this.mButtonOK = new Button();
			this.mLabelEstiamtedRingBufferSize = new Label();
			this.mGridControlCanLin = new GridControl();
			this.mGridViewCanLin = new GridView();
			this.mColCanLinChannel = new GridColumn();
			this.mColCanLinBusload = new GridColumn();
			this.mColCanLinInfo = new GridColumn();
			this.mXtraToolTipController = new XtraToolTipController(this.components);
			this.mTitledGroupTriggerTimes = new TitledGroup();
			this.mTextBoxPreTriggerTime = new TextBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.mLabelPostTriggerTime = new Label();
			this.mTitledGroupDataRate = new TitledGroup();
			this.mTableLayoutPanelChannels = new TableLayoutPanel();
			this.mLabelActiveChannelsCanLin = new Label();
			this.mGridControlFlexRay = new GridControl();
			this.mGridViewFlexRay = new GridView();
			this.mColFlexrayChannel = new GridColumn();
			this.mColFlexrayFrames = new GridColumn();
			this.mColFlexrayPayload = new GridColumn();
			this.mColFlexrayInfo = new GridColumn();
			this.mLabelActiveChannelsFlexray = new Label();
			this.mLabelDigitalInputs = new Label();
			this.mLabelAnalogInputs = new Label();
			this.mTitledGroupResult = new TitledGroup();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.mLabelMaxSizeExceeded = new Label();
			this.mPanelTriggerTimes = new Panel();
			this.mPanelDataRate = new Panel();
			this.mPanelResult = new Panel();
			this.mButtonHelp = new Button();
			this.mErrorProvider = new ErrorProvider(this.components);
			((ISupportInitialize)this.mGridControlCanLin).BeginInit();
			((ISupportInitialize)this.mGridViewCanLin).BeginInit();
			this.mTitledGroupTriggerTimes.SuspendLayout();
			this.mTitledGroupDataRate.SuspendLayout();
			this.mTableLayoutPanelChannels.SuspendLayout();
			((ISupportInitialize)this.mGridControlFlexRay).BeginInit();
			((ISupportInitialize)this.mGridViewFlexRay).BeginInit();
			this.mTitledGroupResult.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.mPanelTriggerTimes.SuspendLayout();
			this.mPanelDataRate.SuspendLayout();
			this.mPanelResult.SuspendLayout();
			((ISupportInitialize)this.mErrorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonOK, "mButtonOK");
			this.mButtonOK.DialogResult = DialogResult.OK;
			this.mButtonOK.Name = "mButtonOK";
			this.mButtonOK.UseVisualStyleBackColor = true;
			this.mButtonOK.Click += new EventHandler(this.ButtonOK_Click);
			componentResourceManager.ApplyResources(this.mLabelEstiamtedRingBufferSize, "mLabelEstiamtedRingBufferSize");
			this.mLabelEstiamtedRingBufferSize.Name = "mLabelEstiamtedRingBufferSize";
			componentResourceManager.ApplyResources(this.mGridControlCanLin, "mGridControlCanLin");
			this.mGridControlCanLin.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mGridControlCanLin.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mGridControlCanLin.LookAndFeel.UseWindowsXPTheme = true;
			this.mGridControlCanLin.MainView = this.mGridViewCanLin;
			this.mGridControlCanLin.Name = "mGridControlCanLin";
			this.mGridControlCanLin.ToolTipController = this.mXtraToolTipController;
			this.mGridControlCanLin.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewCanLin
			});
			this.mGridControlCanLin.ProcessGridKey += new KeyEventHandler(this.GridControl_ProcessGridKey);
			this.mGridControlCanLin.EditorKeyDown += new KeyEventHandler(this.GridControl_EditorKeyDown);
			this.mGridViewCanLin.Columns.AddRange(new GridColumn[]
			{
				this.mColCanLinChannel,
				this.mColCanLinBusload,
				this.mColCanLinInfo
			});
			this.mGridViewCanLin.GridControl = this.mGridControlCanLin;
			this.mGridViewCanLin.Name = "mGridViewCanLin";
			this.mGridViewCanLin.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mGridViewCanLin.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mGridViewCanLin.OptionsCustomization.AllowColumnMoving = false;
			this.mGridViewCanLin.OptionsCustomization.AllowFilter = false;
			this.mGridViewCanLin.OptionsCustomization.AllowGroup = false;
			this.mGridViewCanLin.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridViewCanLin.OptionsCustomization.AllowSort = false;
			this.mGridViewCanLin.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewCanLin.OptionsFind.AllowFindPanel = false;
			this.mGridViewCanLin.OptionsLayout.Columns.StoreAllOptions = true;
			this.mGridViewCanLin.OptionsLayout.Columns.StoreAppearance = true;
			this.mGridViewCanLin.OptionsLayout.StoreAllOptions = true;
			this.mGridViewCanLin.OptionsLayout.StoreAppearance = true;
			this.mGridViewCanLin.OptionsNavigation.UseTabKey = false;
			this.mGridViewCanLin.OptionsSelection.MultiSelect = true;
			this.mGridViewCanLin.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewCanLin.OptionsView.ShowGroupPanel = false;
			this.mGridViewCanLin.OptionsView.ShowIndicator = false;
			this.mGridViewCanLin.PopupMenuShowing += new PopupMenuShowingEventHandler(this.GridView_PopupMenuShowing);
			this.mGridViewCanLin.HiddenEditor += new EventHandler(this.GridView_HiddenEditor);
			this.mGridViewCanLin.ShownEditor += new EventHandler(this.GridView_ShownEditor);
			this.mGridViewCanLin.ValidatingEditor += new BaseContainerValidateEditorEventHandler(this.GridView_ValidatingEditor);
			componentResourceManager.ApplyResources(this.mColCanLinChannel, "mColCanLinChannel");
			this.mColCanLinChannel.FieldName = "Name";
			this.mColCanLinChannel.Name = "mColCanLinChannel";
			this.mColCanLinChannel.OptionsColumn.AllowEdit = false;
			this.mColCanLinChannel.OptionsColumn.AllowFocus = false;
			this.mColCanLinBusload.AppearanceCell.Options.UseTextOptions = true;
			this.mColCanLinBusload.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.mColCanLinBusload.AppearanceHeader.Options.UseTextOptions = true;
			this.mColCanLinBusload.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(this.mColCanLinBusload, "mColCanLinBusload");
			this.mColCanLinBusload.FieldName = "BusLoad";
			this.mColCanLinBusload.Name = "mColCanLinBusload";
			componentResourceManager.ApplyResources(this.mColCanLinInfo, "mColCanLinInfo");
			this.mColCanLinInfo.FieldName = "Info";
			this.mColCanLinInfo.Name = "mColCanLinInfo";
			this.mColCanLinInfo.OptionsColumn.AllowEdit = false;
			this.mColCanLinInfo.OptionsColumn.AllowFocus = false;
			this.mXtraToolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.BackColor");
			this.mXtraToolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.ForeColor");
			this.mXtraToolTipController.Appearance.Options.UseBackColor = true;
			this.mXtraToolTipController.Appearance.Options.UseForeColor = true;
			this.mXtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.mXtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.BackColor");
			this.mXtraToolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.ForeColor");
			this.mXtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.mXtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.MaxWidth = 500;
			this.mXtraToolTipController.ShowPrefix = true;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mTitledGroupTriggerTimes.AutoSizeGroup = true;
			this.mTitledGroupTriggerTimes.BackColor = SystemColors.Window;
			this.mTitledGroupTriggerTimes.Controls.Add(this.mTextBoxPreTriggerTime);
			this.mTitledGroupTriggerTimes.Controls.Add(this.label2);
			this.mTitledGroupTriggerTimes.Controls.Add(this.label3);
			this.mTitledGroupTriggerTimes.Controls.Add(this.mLabelPostTriggerTime);
			componentResourceManager.ApplyResources(this.mTitledGroupTriggerTimes, "mTitledGroupTriggerTimes");
			this.mTitledGroupTriggerTimes.Image = null;
			this.mTitledGroupTriggerTimes.Name = "mTitledGroupTriggerTimes";
			this.mXtraToolTipController.SetTitle(this.mTitledGroupTriggerTimes, componentResourceManager.GetString("mTitledGroupTriggerTimes.Title"));
			componentResourceManager.ApplyResources(this.mTextBoxPreTriggerTime, "mTextBoxPreTriggerTime");
			this.mTextBoxPreTriggerTime.Name = "mTextBoxPreTriggerTime";
			this.mTextBoxPreTriggerTime.TextChanged += new EventHandler(this.TextBoxPreTriggerTime_TextChanged);
			this.mTextBoxPreTriggerTime.Validating += new CancelEventHandler(this.TextBoxPreTriggerTime_Validating);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.mLabelPostTriggerTime, "mLabelPostTriggerTime");
			this.mLabelPostTriggerTime.Name = "mLabelPostTriggerTime";
			this.mTitledGroupDataRate.AutoSizeGroup = true;
			this.mTitledGroupDataRate.BackColor = SystemColors.Window;
			this.mTitledGroupDataRate.Controls.Add(this.mTableLayoutPanelChannels);
			this.mTitledGroupDataRate.Controls.Add(this.mLabelDigitalInputs);
			this.mTitledGroupDataRate.Controls.Add(this.mLabelAnalogInputs);
			componentResourceManager.ApplyResources(this.mTitledGroupDataRate, "mTitledGroupDataRate");
			this.mTitledGroupDataRate.Image = null;
			this.mTitledGroupDataRate.Name = "mTitledGroupDataRate";
			this.mXtraToolTipController.SetTitle(this.mTitledGroupDataRate, componentResourceManager.GetString("mTitledGroupDataRate.Title"));
			componentResourceManager.ApplyResources(this.mTableLayoutPanelChannels, "mTableLayoutPanelChannels");
			this.mTableLayoutPanelChannels.Controls.Add(this.mLabelActiveChannelsCanLin, 0, 0);
			this.mTableLayoutPanelChannels.Controls.Add(this.mGridControlCanLin, 0, 1);
			this.mTableLayoutPanelChannels.Controls.Add(this.mGridControlFlexRay, 0, 4);
			this.mTableLayoutPanelChannels.Controls.Add(this.mLabelActiveChannelsFlexray, 0, 3);
			this.mTableLayoutPanelChannels.Name = "mTableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.mLabelActiveChannelsCanLin, "mLabelActiveChannelsCanLin");
			this.mLabelActiveChannelsCanLin.Name = "mLabelActiveChannelsCanLin";
			componentResourceManager.ApplyResources(this.mGridControlFlexRay, "mGridControlFlexRay");
			this.mGridControlFlexRay.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mGridControlFlexRay.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mGridControlFlexRay.LookAndFeel.UseWindowsXPTheme = true;
			this.mGridControlFlexRay.MainView = this.mGridViewFlexRay;
			this.mGridControlFlexRay.Name = "mGridControlFlexRay";
			this.mGridControlFlexRay.ToolTipController = this.mXtraToolTipController;
			this.mGridControlFlexRay.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewFlexRay
			});
			this.mGridControlFlexRay.ProcessGridKey += new KeyEventHandler(this.GridControl_ProcessGridKey);
			this.mGridControlFlexRay.EditorKeyDown += new KeyEventHandler(this.GridControl_EditorKeyDown);
			this.mGridViewFlexRay.Columns.AddRange(new GridColumn[]
			{
				this.mColFlexrayChannel,
				this.mColFlexrayFrames,
				this.mColFlexrayPayload,
				this.mColFlexrayInfo
			});
			this.mGridViewFlexRay.GridControl = this.mGridControlFlexRay;
			this.mGridViewFlexRay.Name = "mGridViewFlexRay";
			this.mGridViewFlexRay.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mGridViewFlexRay.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mGridViewFlexRay.OptionsCustomization.AllowColumnMoving = false;
			this.mGridViewFlexRay.OptionsCustomization.AllowFilter = false;
			this.mGridViewFlexRay.OptionsCustomization.AllowGroup = false;
			this.mGridViewFlexRay.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridViewFlexRay.OptionsCustomization.AllowSort = false;
			this.mGridViewFlexRay.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewFlexRay.OptionsFind.AllowFindPanel = false;
			this.mGridViewFlexRay.OptionsLayout.Columns.StoreAllOptions = true;
			this.mGridViewFlexRay.OptionsLayout.Columns.StoreAppearance = true;
			this.mGridViewFlexRay.OptionsLayout.StoreAllOptions = true;
			this.mGridViewFlexRay.OptionsLayout.StoreAppearance = true;
			this.mGridViewFlexRay.OptionsNavigation.UseTabKey = false;
			this.mGridViewFlexRay.OptionsSelection.MultiSelect = true;
			this.mGridViewFlexRay.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewFlexRay.OptionsView.ShowGroupPanel = false;
			this.mGridViewFlexRay.OptionsView.ShowIndicator = false;
			this.mGridViewFlexRay.PopupMenuShowing += new PopupMenuShowingEventHandler(this.GridView_PopupMenuShowing);
			this.mGridViewFlexRay.HiddenEditor += new EventHandler(this.GridView_HiddenEditor);
			this.mGridViewFlexRay.ShownEditor += new EventHandler(this.GridView_ShownEditor);
			this.mGridViewFlexRay.ValidatingEditor += new BaseContainerValidateEditorEventHandler(this.GridView_ValidatingEditor);
			componentResourceManager.ApplyResources(this.mColFlexrayChannel, "mColFlexrayChannel");
			this.mColFlexrayChannel.FieldName = "Name";
			this.mColFlexrayChannel.Name = "mColFlexrayChannel";
			this.mColFlexrayChannel.OptionsColumn.AllowEdit = false;
			this.mColFlexrayChannel.OptionsColumn.AllowFocus = false;
			this.mColFlexrayFrames.AppearanceCell.Options.UseTextOptions = true;
			this.mColFlexrayFrames.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.mColFlexrayFrames.AppearanceHeader.Options.UseTextOptions = true;
			this.mColFlexrayFrames.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(this.mColFlexrayFrames, "mColFlexrayFrames");
			this.mColFlexrayFrames.FieldName = "MsgsPerSec";
			this.mColFlexrayFrames.Name = "mColFlexrayFrames";
			this.mColFlexrayPayload.AppearanceCell.Options.UseTextOptions = true;
			this.mColFlexrayPayload.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.mColFlexrayPayload.AppearanceHeader.Options.UseTextOptions = true;
			this.mColFlexrayPayload.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(this.mColFlexrayPayload, "mColFlexrayPayload");
			this.mColFlexrayPayload.FieldName = "Payload";
			this.mColFlexrayPayload.Name = "mColFlexrayPayload";
			componentResourceManager.ApplyResources(this.mColFlexrayInfo, "mColFlexrayInfo");
			this.mColFlexrayInfo.FieldName = "Info";
			this.mColFlexrayInfo.Name = "mColFlexrayInfo";
			this.mColFlexrayInfo.OptionsColumn.AllowEdit = false;
			this.mColFlexrayInfo.OptionsColumn.AllowFocus = false;
			componentResourceManager.ApplyResources(this.mLabelActiveChannelsFlexray, "mLabelActiveChannelsFlexray");
			this.mLabelActiveChannelsFlexray.Name = "mLabelActiveChannelsFlexray";
			componentResourceManager.ApplyResources(this.mLabelDigitalInputs, "mLabelDigitalInputs");
			this.mLabelDigitalInputs.Name = "mLabelDigitalInputs";
			componentResourceManager.ApplyResources(this.mLabelAnalogInputs, "mLabelAnalogInputs");
			this.mLabelAnalogInputs.Name = "mLabelAnalogInputs";
			this.mTitledGroupResult.AutoSizeGroup = true;
			this.mTitledGroupResult.BackColor = SystemColors.Window;
			this.mTitledGroupResult.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.mTitledGroupResult, "mTitledGroupResult");
			this.mTitledGroupResult.Image = null;
			this.mTitledGroupResult.Name = "mTitledGroupResult";
			this.mTitledGroupResult.TabStop = false;
			this.mXtraToolTipController.SetTitle(this.mTitledGroupResult, componentResourceManager.GetString("mTitledGroupResult.Title"));
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.mLabelEstiamtedRingBufferSize, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.mLabelMaxSizeExceeded, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.mLabelMaxSizeExceeded, "mLabelMaxSizeExceeded");
			this.mLabelMaxSizeExceeded.ForeColor = Color.Red;
			this.mLabelMaxSizeExceeded.Name = "mLabelMaxSizeExceeded";
			componentResourceManager.ApplyResources(this.mPanelTriggerTimes, "mPanelTriggerTimes");
			this.mPanelTriggerTimes.Controls.Add(this.mTitledGroupTriggerTimes);
			this.mPanelTriggerTimes.Name = "mPanelTriggerTimes";
			componentResourceManager.ApplyResources(this.mPanelDataRate, "mPanelDataRate");
			this.mPanelDataRate.Controls.Add(this.mTitledGroupDataRate);
			this.mPanelDataRate.Name = "mPanelDataRate";
			componentResourceManager.ApplyResources(this.mPanelResult, "mPanelResult");
			this.mPanelResult.Controls.Add(this.mTitledGroupResult);
			this.mPanelResult.Name = "mPanelResult";
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.ButtonHelp_Click);
			this.mErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProvider.ContainerControl = this;
			base.AcceptButton = this.mButtonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Window;
			base.CancelButton = this.mButtonCancel;
			base.Controls.Add(this.mButtonHelp);
			base.Controls.Add(this.mPanelResult);
			base.Controls.Add(this.mPanelDataRate);
			base.Controls.Add(this.mPanelTriggerTimes);
			base.Controls.Add(this.mButtonOK);
			base.Controls.Add(this.mButtonCancel);
			base.MinimizeBox = false;
			base.Name = "BufferSizeCalculatorForm";
			base.ShowInTaskbar = false;
			base.Load += new EventHandler(this.BufferSizeCalculator_Load);
			base.HelpRequested += new HelpEventHandler(this.BufferSizeCalculator_HelpRequested);
			((ISupportInitialize)this.mGridControlCanLin).EndInit();
			((ISupportInitialize)this.mGridViewCanLin).EndInit();
			this.mTitledGroupTriggerTimes.ResumeLayout(false);
			this.mTitledGroupTriggerTimes.PerformLayout();
			this.mTitledGroupDataRate.ResumeLayout(false);
			this.mTitledGroupDataRate.PerformLayout();
			this.mTableLayoutPanelChannels.ResumeLayout(false);
			this.mTableLayoutPanelChannels.PerformLayout();
			((ISupportInitialize)this.mGridControlFlexRay).EndInit();
			((ISupportInitialize)this.mGridViewFlexRay).EndInit();
			this.mTitledGroupResult.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.mPanelTriggerTimes.ResumeLayout(false);
			this.mPanelDataRate.ResumeLayout(false);
			this.mPanelResult.ResumeLayout(false);
			((ISupportInitialize)this.mErrorProvider).EndInit();
			base.ResumeLayout(false);
		}
	}
}
