using DevComponents.DotNetBar;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.McModule;
using Vector.McModule.Explorer;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	internal class CcpXcpSignalRequests : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<CcpXcpSignalConfiguration>, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<CANChannelConfiguration>, IUpdateObserver<FlexrayChannelConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(CcpXcpSignalConfiguration data);

		private class StatisticsNode
		{
			public TreeListNode Node;

			public BusType BusType;

			public CcpXcpSignalRequests.StatisticsNodeType Type;

			public uint SignalCount;

			public uint ByteCount;

			public double BytesPerSecond;

			public double RelativeLoad;

			public long Limit;

			public bool HasLimit
			{
				get
				{
					return this.Limit != -1L;
				}
			}

			public bool HasError
			{
				get
				{
					return this.Limit >= 0L && (ulong)this.ByteCount > (ulong)this.Limit;
				}
			}

			public StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType type, TreeListNode node, BusType busType) : this(node)
			{
				this.Type = type;
				this.BusType = busType;
			}

			private StatisticsNode(TreeListNode node)
			{
				this.Node = node;
				if (this.Node != null)
				{
					this.Node.Tag = this;
				}
				this.SignalCount = 0u;
				this.BytesPerSecond = 0.0;
				this.RelativeLoad = 0.0;
				this.Limit = -1L;
			}

			public void Reset()
			{
				this.SignalCount = 0u;
				this.ByteCount = 0u;
				this.BytesPerSecond = 0.0;
				this.RelativeLoad = 0.0;
			}

			public void Update()
			{
				this.Node.Visible = (this.SignalCount > 0u);
				this.Node[CcpXcpSignalRequests.SignalCountColumnIndex] = this.SignalCount;
				this.Node[CcpXcpSignalRequests.ByteCountColumnIndex] = this.ByteCount;
				this.Node[CcpXcpSignalRequests.LoadColumnIndex] = Math.Ceiling(this.BytesPerSecond);
				if (this.BusType == BusType.Bt_FlexRay)
				{
					this.Node[CcpXcpSignalRequests.RelativeLoadColumnIndex] = null;
					return;
				}
				this.Node[CcpXcpSignalRequests.RelativeLoadColumnIndex] = Math.Round(this.RelativeLoad);
			}
		}

		private enum StatisticsNodeType
		{
			ChannelNode,
			EcuNode,
			PollingNode,
			DaqNode,
			DaqListNode
		}

		private string configurationFolderPath;

		private CANChannelConfiguration canChannelConfiguration;

		private DatabaseConfiguration databaseConfiguration;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool ccpXcpSignalExplorerShowGroups = true;

		private int CcpXcpSignalExplorerHeight = 535;

		private int CcpXcpSignalExplorerWidth = 450;

		public static readonly uint FlexRayABchannelnumber = 256u;

		public static readonly int DaqRowIndex = 0;

		public static readonly int PollingRowIndex = 1;

		public static readonly int NavigationColumnIndex = 0;

		public static readonly int SignalCountColumnIndex = 1;

		public static readonly int ByteCountColumnIndex = 2;

		public static readonly int LoadColumnIndex = 3;

		public static readonly int RelativeLoadColumnIndex = 4;

		public static readonly uint[] trackBarCycleTimes = new uint[]
		{
			50u,
			100u,
			500u,
			1000u,
			5000u,
			10000u,
			50000u,
			100000u,
			500000u,
			1000000u
		};

		public static readonly string[] trackBarFrequencies = new string[]
		{
			"20 kHz",
			"10 kHz",
			"2 kHz",
			"1 kHz",
			"200 Hz",
			"100 Hz",
			"20 Hz",
			"10 Hz",
			"2 Hz",
			"1 Hz"
		};

		private bool statisticsInitialized;

		private Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> canChannelNodes;

		private Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> flexRayChannelNodes;

		private Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> ethernetChannelNodes;

		private Dictionary<string, CcpXcpSignalRequests.StatisticsNode> ecuNodes;

		private Dictionary<string, CcpXcpSignalRequests.StatisticsNode> ecuPollingNodes;

		private Dictionary<string, CcpXcpSignalRequests.StatisticsNode> ecuDaqNodes;

		private Dictionary<string, Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>> ecuDaqListNodes;

		private bool clicked;

		private IContainer components;

		private GroupBox mGroupBoxCcpXcpDescriptions;

		private Button buttonAddAction;

		private Button buttonRemoveAction;

		private TableLayoutPanel tableLayoutPanel1;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private Button buttonImport;

		private Panel panel;

		private CcpXcpSignalRequestsGrid ccpXcpSignalRequestsGrid;

		private Vector.UtilityFunctions.ExpandableSplitter expandableSplitterStatistics;

		private Panel panelStatistics;

		private TreeList treeListStatistics;

		private TreeListColumn treeListColumnChannel;

		private TreeListColumn treeListColumnSignalCount;

		private TreeListColumn treeListColumnAdditionalLoad;

		private TreeListColumn treeListColumnAdditionalRelativeLoad;

		private RepositoryItemProgressBar repositoryItemProgressBar;

		private RepositoryItemTextEdit repositoryItemTextEdit;

		private Label labelHint;

		private Panel panelFrequencyNonCyclicDaqEvents;

		private TextBox textBoxBarFrequencyNonCyclicDaqEvents;

		private Label labelFrequencyNonCyclicDaqEvents;

		private TrackBar trackBarFrequencyNonCyclicDaqEvents;

		private Panel panelStatisticsGrid;

		private Label label1;

		private Label labelFrequencyMicrosecond;

		private Label label9;

		private Label label8;

		private Label label7;

		private Label label6;

		private Label label5;

		private Label label4;

		private Label label3;

		private Label label2;

		private TableLayoutPanel tableLayoutPanel2;

		private TreeListColumn treeListColumnByteCount;

		private Panel panel1;

		private Label labelStatisticsView;

		private SplitButton splitButtonExport;

		private Button buttonAddSignal;

		private Label label_signallist;

		private Label label_signal;

		private Button buttonRemoveSignal;

		private Button buttonEditCondition;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.ccpXcpSignalRequestsGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.ccpXcpSignalRequestsGrid.ApplicationDatabaseManager = value;
			}
		}

		public string ConfigurationFolderPath
		{
			get
			{
				return this.configurationFolderPath;
			}
			set
			{
				this.configurationFolderPath = value;
				this.ccpXcpSignalRequestsGrid.ConfigurationFolderPath = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.ccpXcpSignalRequestsGrid.ModelValidator;
			}
			set
			{
				this.ccpXcpSignalRequestsGrid.ModelValidator = value;
				if (value != null)
				{
					this.BuildStatisticsTree();
				}
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get
			{
				return this.ccpXcpSignalRequestsGrid.SemanticChecker;
			}
			set
			{
				this.ccpXcpSignalRequestsGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.ccpXcpSignalRequestsGrid.ModelEditor;
			}
			set
			{
				this.ccpXcpSignalRequestsGrid.ModelEditor = value;
			}
		}

		public DisplayMode DisplayMode
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
				return PageType.CcpXcpSignalRequests;
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
					if (((IPropertyWindow)this).HasErrors())
					{
						this.ccpXcpSignalRequestsGrid.ExpandAllGroups();
					}
					this.ccpXcpSignalRequestsGrid.DisplayErrors();
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

		public CcpXcpSignalRequests()
		{
			this.InitializeComponent();
			this.ccpXcpSignalRequestsGrid.SelectionChanged += new EventHandler(this.OnSignalGridSelectionChanged);
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.splitButtonExport.SplitMenu = new ContextMenu(new MenuItem[]
			{
				new MenuItem(Resources.ExportAllSignals, new EventHandler(this.splitButtonExport_Click)),
				new MenuItem(Resources.ExportActiveSignals, new EventHandler(this.splitButtonExportActive_Click)),
				new MenuItem(Resources.ExportSelectedSignals, new EventHandler(this.splitButtonExportSelection_Click))
			});
		}

		private void buttonAddAction_Click(object sender, EventArgs e)
		{
			if (!(from Database db in this.ccpXcpSignalRequestsGrid.DatabaseConfiguration.ActiveCCPXCPDatabases
			where db.FileType == DatabaseFileType.A2L && !db.IsFileNotFound
			select db).Any<Database>())
			{
				InformMessageBox.Error(Resources.ErrorNoA2lDatabaseAdded);
				return;
			}
			CcpXcpSignalListSelection ccpXcpSignalListSelection = new CcpXcpSignalListSelection(this.ccpXcpSignalRequestsGrid.ModelValidator, this.ApplicationDatabaseManager, this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration, null);
			if (DialogResult.OK == ccpXcpSignalListSelection.ShowDialog())
			{
				ActionCcpXcp resultAction = ccpXcpSignalListSelection.ResultAction;
				if (resultAction != null)
				{
					this.ccpXcpSignalRequestsGrid.AddAction(resultAction);
				}
			}
		}

		private void buttonRemoveAction_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.RemoveSelectedActions(true);
		}

		private void buttonEditCondition_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.EditAction();
		}

		private void buttonImport_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.XCPSignalImport))
			{
				this.ccpXcpSignalRequestsGrid.AddSignalsFromFile(GenericOpenFileDialog.FileName);
			}
		}

		private void splitButtonExport_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.WriteSignalsToFile(CcpXcpSignalRequestsGrid.SignalExport.All);
		}

		private void splitButtonExportSelection_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.WriteSignalsToFile(CcpXcpSignalRequestsGrid.SignalExport.Selected);
		}

		private void splitButtonExportActive_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.WriteSignalsToFile(CcpXcpSignalRequestsGrid.SignalExport.Active);
		}

		private void buttonAddSignal_Click(object sender, EventArgs e)
		{
			if (!(from Database db in this.ccpXcpSignalRequestsGrid.DatabaseConfiguration.ActiveCCPXCPDatabases
			where db.FileType == DatabaseFileType.A2L && !db.IsFileNotFound
			select db).Any<Database>())
			{
				InformMessageBox.Error(Resources.ErrorNoA2lDatabaseAdded);
				return;
			}
			this.OpenSymbolSelectionDialog();
		}

		private void buttonRemoveSignal_Click(object sender, EventArgs e)
		{
			this.ccpXcpSignalRequestsGrid.RemoveSelectedSignals(true);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is CcpXcpSignalConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.Databases);
			}
			this.ccpXcpSignalRequestsGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.ResetValidationFramework();
			this.ccpXcpSignalRequestsGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return true;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			this.pageValidator.General.Reset();
			if (this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration != null)
			{
				if (this.trackBarFrequencyNonCyclicDaqEvents.Value < CcpXcpSignalRequests.trackBarCycleTimes.Length)
				{
					bool flag3;
					flag &= this.pageValidator.Control.UpdateModel<uint>(CcpXcpSignalRequests.trackBarCycleTimes[this.trackBarFrequencyNonCyclicDaqEvents.Value], this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.CycleTimeForNonCyclicDaqEvents, this.guiElementManager.GetGUIElement(this.trackBarFrequencyNonCyclicDaqEvents), out flag3);
					flag2 |= flag3;
				}
				if (this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated.Value)
				{
					((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.GlobalModelError, this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated, Resources_CcpXcp.CcpXcpErrorSignalListConfig);
					this.pageValidator.Tree.StoreMapping(this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated, this.guiElementManager.GetGUIElement(this.labelStatisticsView));
					flag = false;
				}
			}
			flag &= this.ccpXcpSignalRequestsGrid.ValidateInput(flag2);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		bool IPropertyWindow.HasErrors()
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
			return flag | this.ccpXcpSignalRequestsGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
			return flag | this.ccpXcpSignalRequestsGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
			return flag | this.ccpXcpSignalRequestsGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return false;
			}
			IPageValidatorGeneral arg_34_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			bool flag = arg_34_0.HasErrors(errorClasses);
			return flag | this.ccpXcpSignalRequestsGrid.HasFormatErrors();
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return;
			}
			this.ccpXcpSignalRequestsGrid.DatabaseConfiguration = data;
			this.databaseConfiguration = data;
			if (data == null)
			{
				return;
			}
			this.BuildStatisticsTree();
			this.UpdateStatisticTree();
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<CcpXcpSignalConfiguration>.Update(CcpXcpSignalConfiguration data)
		{
			if (!this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics.Recording.IsCcpXcpSupported)
			{
				return;
			}
			this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration = data;
			if (data != null)
			{
				int num = Array.IndexOf<uint>(CcpXcpSignalRequests.trackBarCycleTimes, data.CycleTimeForNonCyclicDaqEvents.Value);
				if (num >= this.trackBarFrequencyNonCyclicDaqEvents.Minimum && num <= this.trackBarFrequencyNonCyclicDaqEvents.Maximum)
				{
					this.trackBarFrequencyNonCyclicDaqEvents.Value = num;
					this.textBoxBarFrequencyNonCyclicDaqEvents.Text = CcpXcpSignalRequests.trackBarFrequencies[this.trackBarFrequencyNonCyclicDaqEvents.Value];
				}
				this.UpdateStatisticTree();
				this.UpdateTrackBarFrequencyNonCyclicDaqEvents();
			}
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<CANChannelConfiguration>.Update(CANChannelConfiguration data)
		{
			this.canChannelConfiguration = data;
			if (data != null)
			{
				this.UpdateStatisticTree();
			}
		}

		void IUpdateObserver<FlexrayChannelConfiguration>.Update(FlexrayChannelConfiguration data)
		{
			if (data != null)
			{
				this.UpdateStatisticTree();
			}
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

		private void OnSignalGridSelectionChanged(object sender, EventArgs e)
		{
			this.buttonRemoveAction.Enabled = false;
			this.buttonEditCondition.Enabled = false;
			this.buttonAddSignal.Enabled = true;
			this.buttonRemoveSignal.Enabled = false;
			this.splitButtonExport.Enabled = this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.Actions.Any<ActionCcpXcp>();
			CcpXcpSignal ccpXcpSignal = null;
			if (this.ccpXcpSignalRequestsGrid.TryGetSelectedSignal(out ccpXcpSignal))
			{
				this.buttonRemoveAction.Enabled = this.ccpXcpSignalRequestsGrid.IsGroupRowSelected;
				this.buttonEditCondition.Enabled = this.ccpXcpSignalRequestsGrid.IsGroupRowSelected;
				this.buttonRemoveSignal.Enabled = (!this.ccpXcpSignalRequestsGrid.IsGroupRowSelected && !(ccpXcpSignal is CcpXcpSignalDummy));
			}
		}

		private void OpenSymbolSelectionDialog()
		{
			DisplaySettings displaySettings = new DisplaySettings();
			displaySettings.mSignalTypes = new HashSet<EnumSignalType>
			{
				EnumSignalType.kValue,
				EnumSignalType.kArray,
				EnumSignalType.kCurve,
				EnumSignalType.kMap,
				EnumSignalType.kString
			};
			displaySettings.mMaxDimension = 65535;
			displaySettings.mShowGroups = this.ccpXcpSignalExplorerShowGroups;
			if ((from Database db in this.ccpXcpSignalRequestsGrid.DatabaseConfiguration.ActiveCCPXCPDatabases
			where db.FileType == DatabaseFileType.A2L && !db.IsFileNotFound
			select db).Any<Database>())
			{
				CcpXcpSymbolSelection ccpXcpSymbolSelection = new CcpXcpSymbolSelection(this, this.ccpXcpSignalRequestsGrid.DatabaseConfiguration.ActiveCCPXCPDatabases, displaySettings);
				ccpXcpSymbolSelection.Height = Math.Max(this.CcpXcpSignalExplorerHeight, ccpXcpSymbolSelection.MaximumSize.Height);
				ccpXcpSymbolSelection.Width = Math.Max(this.CcpXcpSignalExplorerWidth, ccpXcpSymbolSelection.MaximumSize.Height);
				ccpXcpSymbolSelection.ShowDialog();
				this.ccpXcpSignalExplorerShowGroups = displaySettings.mShowGroups;
				this.CcpXcpSignalExplorerHeight = ccpXcpSymbolSelection.Height;
				this.CcpXcpSignalExplorerWidth = ccpXcpSymbolSelection.Width;
				return;
			}
			InformMessageBox.Error(Resources.ErrorNoA2lDatabaseAdded);
		}

		public void AddSignals(Dictionary<ISignal, IDatabase> signals)
		{
			this.ccpXcpSignalRequestsGrid.AddSignals(signals);
		}

		public bool Serialize(CcpXcpSignalRequestsPage page)
		{
			if (page == null)
			{
				return false;
			}
			page.CcpXcpSignalExplorerShowGroups = this.ccpXcpSignalExplorerShowGroups.ToString();
			page.CcpXcpSignalExplorerHeight = this.CcpXcpSignalExplorerHeight.ToString();
			page.CcpXcpSignalExplorerWidth = this.CcpXcpSignalExplorerWidth.ToString();
			page.StatisticsSplitterExpanded = this.expandableSplitterStatistics.Expanded.ToString();
			page.StatisticsSplitterPosition = this.expandableSplitterStatistics.SplitPosition.ToString();
			bool flag = true;
			flag &= this.ccpXcpSignalRequestsGrid.Serialize(page);
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.treeListStatistics.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				page.StatisticsTreeLayout = Convert.ToBase64String(array, 0, array.Length);
			}
			catch
			{
				flag = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return flag;
		}

		public bool DeSerialize(CcpXcpSignalRequestsPage page)
		{
			if (page == null)
			{
				return false;
			}
			bool flag;
			if (page.CcpXcpSignalExplorerShowGroups != null && bool.TryParse(page.CcpXcpSignalExplorerShowGroups, out flag))
			{
				this.ccpXcpSignalExplorerShowGroups = flag;
			}
			int ccpXcpSignalExplorerHeight;
			if (page.CcpXcpSignalExplorerHeight != null && int.TryParse(page.CcpXcpSignalExplorerHeight, out ccpXcpSignalExplorerHeight))
			{
				this.CcpXcpSignalExplorerHeight = ccpXcpSignalExplorerHeight;
			}
			int ccpXcpSignalExplorerWidth;
			if (page.CcpXcpSignalExplorerWidth != null && int.TryParse(page.CcpXcpSignalExplorerWidth, out ccpXcpSignalExplorerWidth))
			{
				this.CcpXcpSignalExplorerWidth = ccpXcpSignalExplorerWidth;
			}
			bool expanded;
			if (page.StatisticsSplitterExpanded != null && bool.TryParse(page.StatisticsSplitterExpanded, out expanded))
			{
				this.expandableSplitterStatistics.Expanded = expanded;
			}
			int splitPosition;
			if (page.StatisticsSplitterPosition != null && int.TryParse(page.StatisticsSplitterPosition, out splitPosition))
			{
				this.expandableSplitterStatistics.SplitPosition = splitPosition;
			}
			bool flag2 = true;
			flag2 &= this.ccpXcpSignalRequestsGrid.DeSerialize(page);
			if (page.StatisticsTreeLayout == null)
			{
				flag2 = false;
			}
			else
			{
				MemoryStream memoryStream = null;
				try
				{
					byte[] buffer = Convert.FromBase64String(page.StatisticsTreeLayout);
					memoryStream = new MemoryStream(buffer);
					Dictionary<TreeListColumn, string> dictionary = new Dictionary<TreeListColumn, string>();
					foreach (TreeListColumn treeListColumn in this.treeListStatistics.Columns)
					{
						dictionary.Add(treeListColumn, treeListColumn.Caption);
					}
					this.treeListStatistics.RestoreLayoutFromStream(memoryStream);
					foreach (TreeListColumn current in dictionary.Keys)
					{
						current.Caption = dictionary[current];
					}
				}
				catch
				{
					flag2 = false;
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
			}
			return flag2;
		}

		private void BuildStatisticsTree()
		{
			if (this.databaseConfiguration == null)
			{
				return;
			}
			if (this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration == null)
			{
				return;
			}
			this.statisticsInitialized = false;
			this.treeListStatistics.Nodes.Clear();
			Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
			Dictionary<uint, bool> dictionary2 = new Dictionary<uint, bool>();
			Dictionary<uint, bool> dictionary3 = new Dictionary<uint, bool>();
			Dictionary<string, bool> dictionary4 = new Dictionary<string, bool>();
			Dictionary<string, bool> dictionary5 = new Dictionary<string, bool>();
			if (this.canChannelNodes != null && this.flexRayChannelNodes != null && this.ethernetChannelNodes != null && this.ecuNodes != null && this.ecuDaqNodes != null)
			{
				foreach (KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> current in this.canChannelNodes)
				{
					if (!dictionary.ContainsKey(current.Key))
					{
						dictionary.Add(current.Key, current.Value.Node.Expanded);
					}
				}
				foreach (KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> current2 in this.flexRayChannelNodes)
				{
					if (!dictionary2.ContainsKey(current2.Key))
					{
						dictionary2.Add(current2.Key, current2.Value.Node.Expanded);
					}
				}
				foreach (KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> current3 in this.ethernetChannelNodes)
				{
					if (!dictionary3.ContainsKey(current3.Key))
					{
						dictionary3.Add(current3.Key, current3.Value.Node.Expanded);
					}
				}
				foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current4 in this.ecuNodes)
				{
					if (!dictionary4.ContainsKey(current4.Key))
					{
						dictionary4.Add(current4.Key, current4.Value.Node.Expanded);
					}
				}
				foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current5 in this.ecuDaqNodes)
				{
					if (!dictionary5.ContainsKey(current5.Key))
					{
						dictionary5.Add(current5.Key, current5.Value.Node.Expanded);
					}
				}
			}
			ILoggerSpecifics loggerSpecifics = this.ccpXcpSignalRequestsGrid.ModelValidator.LoggerSpecifics;
			this.canChannelNodes = new Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>();
			this.flexRayChannelNodes = new Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>();
			this.ethernetChannelNodes = new Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>();
			this.ecuNodes = new Dictionary<string, CcpXcpSignalRequests.StatisticsNode>();
			this.ecuPollingNodes = new Dictionary<string, CcpXcpSignalRequests.StatisticsNode>();
			this.ecuDaqNodes = new Dictionary<string, CcpXcpSignalRequests.StatisticsNode>();
			this.ecuDaqListNodes = new Dictionary<string, Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>>();
			uint j;
			for (j = 1u; j <= loggerSpecifics.CAN.NumberOfChannels; j += 1u)
			{
				if ((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
				where db.BusType.Value == BusType.Bt_CAN && db.ChannelNumber.Value == j
				select db).Any<Database>())
				{
					this.AddChannelStatistics(BusType.Bt_CAN, j);
				}
			}
			uint i;
			for (i = 1u; i <= loggerSpecifics.Flexray.NumberOfChannels; i += 1u)
			{
				if ((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
				where db.BusType.Value == BusType.Bt_FlexRay && db.ChannelNumber.Value == i
				select db).Any<Database>())
				{
					this.AddChannelStatistics(BusType.Bt_FlexRay, i);
				}
			}
			if (loggerSpecifics.Flexray.NumberOfChannels >= 2u)
			{
				if ((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
				where db.BusType.Value == BusType.Bt_FlexRay && db.ChannelNumber.Value == CcpXcpSignalRequests.FlexRayABchannelnumber
				select db).Any<Database>())
				{
					this.AddChannelStatistics(BusType.Bt_FlexRay, CcpXcpSignalRequests.FlexRayABchannelnumber);
				}
			}
			if ((from Database db in this.databaseConfiguration.ActiveCCPXCPDatabases
			where db.BusType.Value == BusType.Bt_Ethernet
			select db).Any<Database>())
			{
				this.AddChannelStatistics(BusType.Bt_Ethernet, 1u);
			}
			foreach (KeyValuePair<uint, bool> current6 in dictionary)
			{
				if (this.canChannelNodes.ContainsKey(current6.Key))
				{
					this.canChannelNodes[current6.Key].Node.Expanded = current6.Value;
				}
			}
			foreach (KeyValuePair<uint, bool> current7 in dictionary2)
			{
				if (this.flexRayChannelNodes.ContainsKey(current7.Key))
				{
					this.flexRayChannelNodes[current7.Key].Node.Expanded = current7.Value;
				}
			}
			foreach (KeyValuePair<uint, bool> current8 in dictionary3)
			{
				if (this.ethernetChannelNodes.ContainsKey(current8.Key))
				{
					this.ethernetChannelNodes[current8.Key].Node.Expanded = current8.Value;
				}
			}
			foreach (KeyValuePair<string, bool> current9 in dictionary4)
			{
				if (this.ecuNodes.ContainsKey(current9.Key))
				{
					this.ecuNodes[current9.Key].Node.Expanded = current9.Value;
				}
			}
			foreach (KeyValuePair<string, bool> current10 in dictionary5)
			{
				if (this.ecuDaqNodes.ContainsKey(current10.Key))
				{
					this.ecuDaqNodes[current10.Key].Node.Expanded = current10.Value;
				}
			}
			this.statisticsInitialized = true;
		}

		private void AddChannelStatistics(BusType busType, uint channelNumber)
		{
			TreeListNode treeListNode = null;
			switch (busType)
			{
			case BusType.Bt_CAN:
				if (!this.canChannelNodes.ContainsKey(channelNumber))
				{
					treeListNode = this.treeListStatistics.AppendNode(this.CreateRowObject(GUIUtil.MapCANChannelNumber2String(channelNumber), BusType.Bt_CAN), null);
					this.canChannelNodes.Add(channelNumber, new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.ChannelNode, treeListNode, busType));
				}
				break;
			case BusType.Bt_LIN:
			case BusType.Bt_J1708:
				return;
			case BusType.Bt_FlexRay:
				if (!this.flexRayChannelNodes.ContainsKey(channelNumber))
				{
					treeListNode = this.treeListStatistics.AppendNode(this.CreateRowObject(GUIUtil.MapFlexrayChannelNumber2String(channelNumber), BusType.Bt_FlexRay), null);
					this.flexRayChannelNodes.Add(channelNumber, new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.ChannelNode, treeListNode, busType));
				}
				break;
			case BusType.Bt_Ethernet:
				if (!this.ethernetChannelNodes.ContainsKey(channelNumber))
				{
					treeListNode = this.treeListStatistics.AppendNode(this.CreateRowObject(GUIUtil.MapEthernetChannelNumber2String(channelNumber), BusType.Bt_Ethernet), null);
					this.ethernetChannelNodes.Add(channelNumber, new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.ChannelNode, treeListNode, busType));
				}
				break;
			default:
				return;
			}
			this.AddEcuStatistics(treeListNode, busType, channelNumber);
		}

		private void AddEcuStatistics(TreeListNode channelNode, BusType busType, uint channelNumber)
		{
			IEnumerable<Database> source = from db in this.databaseConfiguration.ActiveCCPXCPDatabases
			where db.BusType.Value == busType && db.ChannelNumber.Value == channelNumber
			select db;
			foreach (Database current in from db in source
			where db.CcpXcpEcuDisplayName != null && !string.IsNullOrEmpty(db.CcpXcpEcuDisplayName.Value)
			select db)
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode;
				if (!this.ecuNodes.TryGetValue(current.CcpXcpEcuDisplayName.Value, out statisticsNode))
				{
					TreeListNode node = channelNode.Nodes.Add(this.CreateRowObject(current.CcpXcpEcuDisplayName.Value, busType));
					statisticsNode = new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.EcuNode, node, busType);
					this.ecuNodes.Add(current.CcpXcpEcuDisplayName.Value, statisticsNode);
				}
				this.AddDaqStatistics(statisticsNode.Node, current);
				if (busType == BusType.Bt_CAN && !this.ecuPollingNodes.ContainsKey(current.CcpXcpEcuDisplayName.Value))
				{
					this.ecuPollingNodes.Add(current.CcpXcpEcuDisplayName.Value, new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.PollingNode, statisticsNode.Node.Nodes.Add(this.CreateRowObject(Resources.Polling, busType)), busType));
				}
			}
		}

		private void AddDaqStatistics(TreeListNode ecuNode, Database database)
		{
			if (ecuNode == null || database == null)
			{
				return;
			}
			A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(database);
			if (a2LDatabase == null || a2LDatabase.DeviceConfig == null)
			{
				return;
			}
			if (a2LDatabase.DaqEvents == null || !a2LDatabase.DaqEvents.Any<KeyValuePair<uint, IDaqEvent>>())
			{
				return;
			}
			CcpXcpSignalRequests.StatisticsNode statisticsNode;
			TreeListNode treeListNode;
			if (this.ecuDaqNodes.TryGetValue(database.CcpXcpEcuDisplayName.Value, out statisticsNode))
			{
				treeListNode = statisticsNode.Node;
			}
			else
			{
				treeListNode = ecuNode.Nodes.Add(this.CreateRowObject(Vocabulary.Daq, database.BusType.Value));
				statisticsNode = new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.DaqNode, treeListNode, database.BusType.Value);
				this.ecuDaqNodes.Add(database.CcpXcpEcuDisplayName.Value, statisticsNode);
			}
			using (Dictionary<uint, IDaqEvent>.KeyCollection.Enumerator enumerator = a2LDatabase.DaqEvents.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint daqId = enumerator.Current;
					IDaqEvent daqEvent;
					if (a2LDatabase.DaqEvents.TryGetValue(daqId, out daqEvent))
					{
						Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> dictionary;
						if (!this.ecuDaqListNodes.TryGetValue(database.CcpXcpEcuDisplayName.Value, out dictionary))
						{
							dictionary = new Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>();
							this.ecuDaqListNodes.Add(database.CcpXcpEcuDisplayName.Value, dictionary);
						}
						if (!dictionary.ContainsKey(daqId))
						{
							TreeListNode node = treeListNode.Nodes.Add(this.CreateRowObject(daqEvent.Name, database.BusType.Value));
							CcpXcpSignalRequests.StatisticsNode statisticsNode2 = new CcpXcpSignalRequests.StatisticsNode(CcpXcpSignalRequests.StatisticsNodeType.DaqListNode, node, database.BusType.Value);
							if (a2LDatabase.DaqLists.Any((IDaqList dl) => dl.IsEventChannelFixed && (uint)dl.FixedEventChannel == daqId))
							{
								statisticsNode2.Limit = 0L;
							}
							else
							{
								statisticsNode2.Limit = -1L;
								if (!statisticsNode.HasLimit)
								{
									uint num = 0u;
									bool flag;
									if (CcpXcpManager.Instance().GetUseEcuTimestamp(database, out flag) && flag)
									{
										CcpXcpManager.Instance().GetEcuTimestampWidth(database, out num);
									}
									uint num2 = 0u;
									if (a2LDatabase.DeviceConfig.ProtocolType == EnumProtocolType.ProtocolCcp)
									{
										num2 = 8u;
									}
									else if (CcpXcpManager.Instance().GetMaxDTO(database, out num2))
									{
										long num3 = 0L;
										uint num4 = CcpXcpSignalRequests.SizeOfIdField(a2LDatabase.DeviceConfig.Daq.Identifier);
										switch (a2LDatabase.DeviceConfig.Daq.Identifier)
										{
										case EnumIdentifier.kDAQ_ID_FIELD_ABSOLUTE_ODT:
											num3 = (long)(Constants.MaxDaqPidXcp + 1) * (long)((ulong)(num2 - num4)) - (long)((ulong)num);
											break;
										case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_BYTE:
											num3 = (long)(256 * (int)(Constants.MaxDaqPidXcp + 1)) * (long)((ulong)(num2 - num4)) - (long)((ulong)num);
											break;
										case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_WORD:
											num3 = (long)(65536 * (int)(Constants.MaxDaqPidXcp + 1)) * (long)((ulong)(num2 - num4)) - (long)((ulong)num);
											break;
										case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_WORD_ALIGN:
											num3 = (long)(65536 * (int)(Constants.MaxDaqPidXcp + 1)) * (long)((ulong)(num2 - num4)) - (long)((ulong)num);
											break;
										}
										if (num3 > (long)((ulong)-1))
										{
											num3 = (long)((ulong)-1);
										}
										statisticsNode.Limit = num3;
									}
								}
							}
							dictionary.Add(daqId, statisticsNode2);
						}
					}
				}
			}
		}

		private void ResetStatistics()
		{
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current in this.ecuPollingNodes)
			{
				current.Value.Reset();
			}
			foreach (Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> current2 in this.ecuDaqListNodes.Values)
			{
				foreach (CcpXcpSignalRequests.StatisticsNode current3 in current2.Values)
				{
					current3.Reset();
				}
			}
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current4 in this.ecuDaqNodes)
			{
				current4.Value.Reset();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current5 in this.ecuNodes.Values)
			{
				current5.Reset();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current6 in this.canChannelNodes.Values)
			{
				current6.Reset();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current7 in this.flexRayChannelNodes.Values)
			{
				current7.Reset();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current8 in this.ethernetChannelNodes.Values)
			{
				current8.Reset();
			}
			this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated.Value = false;
		}

		private void UpdateStatisticTree()
		{
			if (this.databaseConfiguration == null)
			{
				return;
			}
			if (!this.statisticsInitialized)
			{
				return;
			}
			if (this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration == null)
			{
				return;
			}
			this.ResetStatistics();
			foreach (CcpXcpSignal current in this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.ActiveSignals)
			{
				if (!this.ccpXcpSignalRequestsGrid.HasDatabaseError(current))
				{
					Database database = CcpXcpManager.Instance().GetDatabase(current);
					if (database != null && current.ActionCcpXcp.Mode != ActionCcpXcp.ActivationMode.Triggered)
					{
						if (current.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
						{
							CcpXcpSignalRequests.StatisticsNode statisticsNode = null;
							Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> dictionary;
							if (this.ecuDaqListNodes.TryGetValue(database.CcpXcpEcuDisplayName.Value, out dictionary) && dictionary.ContainsKey(current.DaqEventId.Value))
							{
								statisticsNode = dictionary[current.DaqEventId.Value];
							}
							else if (this.ecuDaqNodes.ContainsKey(database.CcpXcpEcuDisplayName.Value))
							{
								statisticsNode = this.ecuDaqNodes[database.CcpXcpEcuDisplayName.Value];
							}
							if (statisticsNode != null)
							{
								statisticsNode.SignalCount += 1u;
								uint num = 0u;
								double num2 = 0.0;
								if (CcpXcpManager.Instance().TryGetByteCountOfSignal(current, out num, out num2))
								{
									statisticsNode.ByteCount += num;
									statisticsNode.BytesPerSecond += num2;
								}
							}
						}
						else if (current.MeasurementMode.Value == CcpXcpMeasurementMode.Polling && database.BusType.Value == BusType.Bt_CAN && this.ecuPollingNodes.ContainsKey(database.CcpXcpEcuDisplayName.Value))
						{
							CcpXcpSignalRequests.StatisticsNode statisticsNode2 = this.ecuPollingNodes[database.CcpXcpEcuDisplayName.Value];
							statisticsNode2.SignalCount += 1u;
							uint num3 = 0u;
							double num4 = 0.0;
							if (CcpXcpManager.Instance().TryGetByteCountOfSignal(current, out num3, out num4))
							{
								statisticsNode2.ByteCount += num3;
								statisticsNode2.BytesPerSecond += num4;
							}
						}
					}
				}
			}
			foreach (KeyValuePair<string, Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>> current2 in this.ecuDaqListNodes)
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode3;
				if (this.ecuDaqNodes.TryGetValue(current2.Key, out statisticsNode3))
				{
					foreach (CcpXcpSignalRequests.StatisticsNode current3 in current2.Value.Values)
					{
						statisticsNode3.SignalCount += current3.SignalCount;
						statisticsNode3.ByteCount += current3.ByteCount;
						statisticsNode3.BytesPerSecond += current3.BytesPerSecond;
					}
				}
			}
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current4 in this.ecuPollingNodes)
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode4;
				if (this.ecuNodes.TryGetValue(current4.Key, out statisticsNode4))
				{
					statisticsNode4.SignalCount += current4.Value.SignalCount;
					statisticsNode4.ByteCount += current4.Value.ByteCount;
					statisticsNode4.BytesPerSecond += current4.Value.BytesPerSecond;
				}
			}
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current5 in this.ecuDaqNodes)
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode5;
				if (this.ecuNodes.TryGetValue(current5.Key, out statisticsNode5))
				{
					statisticsNode5.SignalCount += current5.Value.SignalCount;
					statisticsNode5.ByteCount += current5.Value.ByteCount;
					statisticsNode5.BytesPerSecond += current5.Value.BytesPerSecond;
				}
				this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated.Value |= current5.Value.HasError;
			}
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current6 in this.ecuNodes)
			{
				Database database2 = CcpXcpManager.Instance().GetDatabase(current6.Key);
				if (database2 != null)
				{
					if (database2.BusType.Value == BusType.Bt_CAN)
					{
						this.UpdateChannelNode(this.canChannelNodes, database2.ChannelNumber.Value, current6.Value);
					}
					else if (database2.BusType.Value == BusType.Bt_FlexRay)
					{
						this.UpdateChannelNode(this.flexRayChannelNodes, database2.ChannelNumber.Value, current6.Value);
					}
					else if (database2.BusType.Value == BusType.Bt_Ethernet)
					{
						this.UpdateChannelNode(this.ethernetChannelNodes, database2.ChannelNumber.Value, current6.Value);
					}
				}
			}
			this.UpdateRelativeLoad(this.ecuPollingNodes);
			foreach (KeyValuePair<string, Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>> current7 in this.ecuDaqListNodes)
			{
				Database database3 = CcpXcpManager.Instance().GetDatabase(current7.Key);
				if (database3 != null)
				{
					A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(database3);
					if (a2LDatabase != null && a2LDatabase.DaqLists != null && a2LDatabase.DeviceConfig != null)
					{
						uint num5 = 0u;
						if (a2LDatabase.DeviceConfig.ProtocolType == EnumProtocolType.ProtocolCcp)
						{
							num5 = 8u;
						}
						else if (!CcpXcpManager.Instance().GetMaxDTO(database3, out num5))
						{
							continue;
						}
						uint num6 = CcpXcpSignalRequests.SizeOfIdField(a2LDatabase.DeviceConfig.Daq.Identifier);
						uint num7 = 0u;
						bool flag;
						if (CcpXcpManager.Instance().GetUseEcuTimestamp(database3, out flag) && flag)
						{
							CcpXcpManager.Instance().GetEcuTimestampWidth(database3, out num7);
						}
						if (database3.CcpXcpEcuList.Any<CcpXcpEcu>())
						{
							A2LDatabase.ExtendSizeOfStaticDaqLists(a2LDatabase.DeviceConfig, database3.CcpXcpEcuList[0].ExtendStaticDaqListToMaxSize);
							a2LDatabase.UpdateDaqLists();
						}
						using (Dictionary<uint, CcpXcpSignalRequests.StatisticsNode>.Enumerator enumerator8 = current7.Value.GetEnumerator())
						{
							while (enumerator8.MoveNext())
							{
								KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node = enumerator8.Current;
								if (database3 != null && database3.BusType.Value != BusType.Bt_FlexRay)
								{
									double baudrate = this.GetBaudrate(database3.BusType.Value, database3.ChannelNumber.Value);
									if (Math.Abs(baudrate) > 4.94065645841247E-324)
									{
										KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node9 = node;
										CcpXcpSignalRequests.StatisticsNode arg_717_0 = node9.Value;
										KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node2 = node;
										arg_717_0.RelativeLoad = this.GetRelativeLoad(node2.Value.BytesPerSecond, baudrate);
									}
									ValidatedProperty<bool> expr_72C = this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated;
									bool arg_752_0 = expr_72C.Value;
									KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node3 = node;
									expr_72C.Value = (arg_752_0 | node3.Value.RelativeLoad > 100.0);
									KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node4 = node;
									if (node4.Value.HasLimit)
									{
										long num8 = 0L;
										foreach (IDaqList current8 in a2LDatabase.DaqLists.Where(delegate(IDaqList list)
										{
											if (list.IsEventChannelFixed)
											{
												uint arg_1F_0 = (uint)list.FixedEventChannel;
												KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node8 = node;
												return arg_1F_0 == node8.Key;
											}
											return false;
										}))
										{
											num8 += (long)((ulong)((uint)current8.NumberOdts * (num5 - num6 - num7)));
										}
										ValidatedProperty<bool> expr_7F2 = this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated;
										bool arg_818_0 = expr_7F2.Value;
										KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node5 = node;
										expr_7F2.Value = (arg_818_0 | node5.Value.RelativeLoad > 100.0);
										KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node6 = node;
										node6.Value.Limit = num8;
										ValidatedProperty<bool> expr_845 = this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated;
										bool arg_860_0 = expr_845.Value;
										KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> node7 = node;
										expr_845.Value = (arg_860_0 | node7.Value.HasError);
									}
								}
							}
						}
					}
				}
			}
			this.UpdateRelativeLoad(this.ecuDaqNodes);
			this.UpdateRelativeLoad(this.ecuNodes);
			this.UpdateRelativeLoad(BusType.Bt_CAN, this.canChannelNodes);
			this.UpdateRelativeLoad(BusType.Bt_Ethernet, this.ethernetChannelNodes);
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current9 in this.ecuPollingNodes)
			{
				current9.Value.Update();
			}
			foreach (Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> current10 in this.ecuDaqListNodes.Values)
			{
				foreach (CcpXcpSignalRequests.StatisticsNode current11 in current10.Values)
				{
					current11.Update();
				}
			}
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current12 in this.ecuDaqNodes)
			{
				current12.Value.Update();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current13 in this.ecuNodes.Values)
			{
				current13.Update();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current14 in this.canChannelNodes.Values)
			{
				current14.Update();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current15 in this.flexRayChannelNodes.Values)
			{
				current15.Update();
			}
			foreach (CcpXcpSignalRequests.StatisticsNode current16 in this.ethernetChannelNodes.Values)
			{
				current16.Update();
			}
		}

		private static uint SizeOfIdField(EnumIdentifier identifierType)
		{
			uint result = 0u;
			switch (identifierType)
			{
			case EnumIdentifier.kDAQ_ID_FIELD_ABSOLUTE_ODT:
				result = 1u;
				break;
			case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_BYTE:
				result = 2u;
				break;
			case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_WORD:
				result = 3u;
				break;
			case EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_WORD_ALIGN:
				result = 4u;
				break;
			}
			return result;
		}

		private void UpdateChannelNode(Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> channelNodes, uint channelNumber, CcpXcpSignalRequests.StatisticsNode ecuNode)
		{
			if (channelNodes == null || ecuNode == null)
			{
				return;
			}
			CcpXcpSignalRequests.StatisticsNode statisticsNode;
			if (channelNodes.TryGetValue(channelNumber, out statisticsNode))
			{
				statisticsNode.SignalCount += ecuNode.SignalCount;
				statisticsNode.ByteCount += ecuNode.ByteCount;
				statisticsNode.BytesPerSecond += ecuNode.BytesPerSecond;
			}
		}

		private void UpdateRelativeLoad(Dictionary<string, CcpXcpSignalRequests.StatisticsNode> nodes)
		{
			foreach (KeyValuePair<string, CcpXcpSignalRequests.StatisticsNode> current in nodes)
			{
				Database database = CcpXcpManager.Instance().GetDatabase(current.Key);
				if (database != null)
				{
					this.UpdateRelativeLoad(current.Value, database.BusType.Value, database.ChannelNumber.Value);
				}
			}
		}

		private void UpdateRelativeLoad(BusType busType, Dictionary<uint, CcpXcpSignalRequests.StatisticsNode> nodes)
		{
			foreach (KeyValuePair<uint, CcpXcpSignalRequests.StatisticsNode> current in nodes)
			{
				this.UpdateRelativeLoad(current.Value, busType, current.Key);
			}
		}

		private void UpdateRelativeLoad(CcpXcpSignalRequests.StatisticsNode node, BusType busType, uint channelNumber)
		{
			double baudrate = this.GetBaudrate(busType, channelNumber);
			if (Math.Abs(baudrate) > 4.94065645841247E-324)
			{
				node.RelativeLoad = this.GetRelativeLoad(node.BytesPerSecond, baudrate);
			}
			this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.StatisticsViolated.Value |= (node.RelativeLoad > 100.0);
		}

		private void treeListStatistics_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
		{
			if (e.Column == this.treeListStatistics.Columns[CcpXcpSignalRequests.RelativeLoadColumnIndex])
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode = e.Node.Tag as CcpXcpSignalRequests.StatisticsNode;
				if (statisticsNode == null)
				{
					return;
				}
				if (statisticsNode.BusType == BusType.Bt_FlexRay)
				{
					e.RepositoryItem = this.repositoryItemTextEdit;
					return;
				}
				e.RepositoryItem = this.repositoryItemProgressBar;
			}
		}

		private void treeListStatistics_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
		{
			CcpXcpSignalRequests.StatisticsNode statisticsNode = e.Node.Tag as CcpXcpSignalRequests.StatisticsNode;
			if (statisticsNode == null)
			{
				return;
			}
			if (e.Column == this.treeListStatistics.Columns[CcpXcpSignalRequests.NavigationColumnIndex])
			{
				if (statisticsNode.HasError || this.HaveChildNodesError(e.Node))
				{
					e.Appearance.ForeColor = Color.Red;
				}
			}
			else if (e.Column == this.treeListStatistics.Columns[CcpXcpSignalRequests.ByteCountColumnIndex] && (statisticsNode.Type == CcpXcpSignalRequests.StatisticsNodeType.DaqListNode || statisticsNode.Type == CcpXcpSignalRequests.StatisticsNodeType.DaqNode) && statisticsNode.HasError)
			{
				e.Appearance.ForeColor = Color.Red;
			}
			if (e.Column != this.treeListStatistics.Columns[CcpXcpSignalRequests.ByteCountColumnIndex] && e.Column != this.treeListStatistics.Columns[CcpXcpSignalRequests.LoadColumnIndex])
			{
				if (e.Column != this.treeListStatistics.Columns[CcpXcpSignalRequests.SignalCountColumnIndex])
				{
					return;
				}
			}
			string text;
			try
			{
				text = Convert.ToInt64(e.CellValue).ToString("N0", ProgramUtils.Culture);
			}
			catch (Exception)
			{
				text = e.CellValue.ToString();
			}
			if (e.Column == this.treeListStatistics.Columns[CcpXcpSignalRequests.ByteCountColumnIndex] && (statisticsNode.Type == CcpXcpSignalRequests.StatisticsNodeType.DaqListNode || statisticsNode.Type == CcpXcpSignalRequests.StatisticsNodeType.DaqNode) && statisticsNode.HasLimit)
			{
				e.CellText = string.Concat(new string[]
				{
					text,
					" ",
					Resources.Of,
					" ",
					statisticsNode.Limit.ToString("N0", ProgramUtils.Culture)
				});
				return;
			}
			e.CellText = text;
		}

		private void UpdateTrackBarFrequencyNonCyclicDaqEvents()
		{
			if (this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.CycleTimeForNonCyclicDaqEvents != null)
			{
				int num = Array.IndexOf<uint>(CcpXcpSignalRequests.trackBarCycleTimes, this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration.CycleTimeForNonCyclicDaqEvents.Value);
				if (num >= this.trackBarFrequencyNonCyclicDaqEvents.Minimum && num <= this.trackBarFrequencyNonCyclicDaqEvents.Maximum)
				{
					this.trackBarFrequencyNonCyclicDaqEvents.Value = num;
				}
			}
		}

		private object[] CreateRowObject(string name, BusType busType)
		{
			if (busType == BusType.Bt_FlexRay)
			{
				object[] array = new object[5];
				array[0] = name;
				array[1] = 0;
				array[2] = 0;
				array[3] = 0;
				return array;
			}
			return new object[]
			{
				name,
				0,
				0,
				0,
				0
			};
		}

		private double GetBaudrate(BusType busType, uint channel)
		{
			if (busType == BusType.Bt_CAN && (ulong)(channel - 1u) < (ulong)((long)this.canChannelConfiguration.CANChannels.Count))
			{
				return this.canChannelConfiguration.CANChannels[(int)(channel - 1u)].CANChipConfiguration.Baudrate;
			}
			if (busType == BusType.Bt_Ethernet)
			{
				return 100000000.0;
			}
			return 0.0;
		}

		private double GetRelativeLoad(double bytesPerSecond, double baudrate)
		{
			return bytesPerSecond * 100.0 / (baudrate * 0.125);
		}

		private bool HaveChildNodesError(TreeListNode parentNode)
		{
			foreach (TreeListNode treeListNode in parentNode.Nodes)
			{
				CcpXcpSignalRequests.StatisticsNode statisticsNode = treeListNode.Tag as CcpXcpSignalRequests.StatisticsNode;
				if (statisticsNode != null && statisticsNode.HasError)
				{
					bool result = true;
					return result;
				}
				if (treeListNode.HasChildren && this.HaveChildNodesError(treeListNode))
				{
					bool result = true;
					return result;
				}
			}
			return false;
		}

		private void trackBarFrequencyNonCyclicDaqEvents_Scroll(object sender, EventArgs e)
		{
			bool arg_06_0 = this.clicked;
		}

		private void trackBarFrequencyNonCyclicDaqEvents_MouseDown(object sender, MouseEventArgs e)
		{
			this.clicked = true;
		}

		private void trackBarFrequencyNonCyclicDaqEvents_MouseUp(object sender, MouseEventArgs e)
		{
			if (!this.clicked)
			{
				return;
			}
			this.clicked = false;
			this.textBoxBarFrequencyNonCyclicDaqEvents.Text = CcpXcpSignalRequests.trackBarFrequencies[this.trackBarFrequencyNonCyclicDaqEvents.Value];
			((IPropertyWindow)this).ValidateInput();
		}

		private void trackBarFrequencyNonCyclicDaqEvents_KeyUp(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode & Keys.Up) == Keys.Up || (e.KeyCode & Keys.Down) == Keys.Down || (e.KeyCode & Keys.Prior) == Keys.Prior || (e.KeyCode & Keys.Next) == Keys.Next)
			{
				this.textBoxBarFrequencyNonCyclicDaqEvents.Text = CcpXcpSignalRequests.trackBarFrequencies[this.trackBarFrequencyNonCyclicDaqEvents.Value];
				((IPropertyWindow)this).ValidateInput();
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpSignalRequests));
			this.mGroupBoxCcpXcpDescriptions = new GroupBox();
			this.panel = new Panel();
			this.panel1 = new Panel();
			this.ccpXcpSignalRequestsGrid = new CcpXcpSignalRequestsGrid();
			this.labelStatisticsView = new Label();
			this.expandableSplitterStatistics = new Vector.UtilityFunctions.ExpandableSplitter();
			this.panelStatistics = new Panel();
			this.panelStatisticsGrid = new Panel();
			this.labelHint = new Label();
			this.treeListStatistics = new TreeList();
			this.treeListColumnChannel = new TreeListColumn();
			this.treeListColumnSignalCount = new TreeListColumn();
			this.treeListColumnByteCount = new TreeListColumn();
			this.treeListColumnAdditionalLoad = new TreeListColumn();
			this.treeListColumnAdditionalRelativeLoad = new TreeListColumn();
			this.repositoryItemProgressBar = new RepositoryItemProgressBar();
			this.repositoryItemTextEdit = new RepositoryItemTextEdit();
			this.panelFrequencyNonCyclicDaqEvents = new Panel();
			this.tableLayoutPanel2 = new TableLayoutPanel();
			this.label9 = new Label();
			this.labelFrequencyMicrosecond = new Label();
			this.label8 = new Label();
			this.label1 = new Label();
			this.label7 = new Label();
			this.label2 = new Label();
			this.label6 = new Label();
			this.label3 = new Label();
			this.label5 = new Label();
			this.label4 = new Label();
			this.textBoxBarFrequencyNonCyclicDaqEvents = new TextBox();
			this.labelFrequencyNonCyclicDaqEvents = new Label();
			this.trackBarFrequencyNonCyclicDaqEvents = new TrackBar();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonAddSignal = new Button();
			this.splitButtonExport = new SplitButton();
			this.buttonAddAction = new Button();
			this.buttonRemoveAction = new Button();
			this.buttonImport = new Button();
			this.label_signallist = new Label();
			this.label_signal = new Label();
			this.buttonRemoveSignal = new Button();
			this.buttonEditCondition = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.mGroupBoxCcpXcpDescriptions.SuspendLayout();
			this.panel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panelStatistics.SuspendLayout();
			this.panelStatisticsGrid.SuspendLayout();
			((ISupportInitialize)this.treeListStatistics).BeginInit();
			((ISupportInitialize)this.repositoryItemProgressBar).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEdit).BeginInit();
			this.panelFrequencyNonCyclicDaqEvents.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((ISupportInitialize)this.trackBarFrequencyNonCyclicDaqEvents).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mGroupBoxCcpXcpDescriptions, "mGroupBoxCcpXcpDescriptions");
			this.mGroupBoxCcpXcpDescriptions.Controls.Add(this.panel);
			this.mGroupBoxCcpXcpDescriptions.Controls.Add(this.tableLayoutPanel1);
			this.mGroupBoxCcpXcpDescriptions.Name = "mGroupBoxCcpXcpDescriptions";
			this.mGroupBoxCcpXcpDescriptions.TabStop = false;
			this.panel.AllowDrop = true;
			this.panel.Controls.Add(this.panel1);
			this.panel.Controls.Add(this.expandableSplitterStatistics);
			this.panel.Controls.Add(this.panelStatistics);
			componentResourceManager.ApplyResources(this.panel, "panel");
			this.panel.Name = "panel";
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Controls.Add(this.ccpXcpSignalRequestsGrid);
			this.panel1.Controls.Add(this.labelStatisticsView);
			this.panel1.Name = "panel1";
			this.ccpXcpSignalRequestsGrid.AllowDrop = true;
			componentResourceManager.ApplyResources(this.ccpXcpSignalRequestsGrid, "ccpXcpSignalRequestsGrid");
			this.ccpXcpSignalRequestsGrid.ApplicationDatabaseManager = null;
			this.ccpXcpSignalRequestsGrid.CcpXcpSignalConfiguration = null;
			this.ccpXcpSignalRequestsGrid.ConfigurationFolderPath = null;
			this.ccpXcpSignalRequestsGrid.CurrentLogger = null;
			this.ccpXcpSignalRequestsGrid.DatabaseConfiguration = null;
			this.ccpXcpSignalRequestsGrid.ModelEditor = null;
			this.ccpXcpSignalRequestsGrid.ModelValidator = null;
			this.ccpXcpSignalRequestsGrid.Name = "ccpXcpSignalRequestsGrid";
			this.ccpXcpSignalRequestsGrid.SemanticChecker = null;
			componentResourceManager.ApplyResources(this.labelStatisticsView, "labelStatisticsView");
			this.labelStatisticsView.Name = "labelStatisticsView";
			this.expandableSplitterStatistics.BackColor = SystemColors.Control;
			this.expandableSplitterStatistics.BackColor2 = SystemColors.ControlDarkDark;
			this.expandableSplitterStatistics.BackColor2SchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.BackColorSchemePart = eColorSchemePart.None;
			componentResourceManager.ApplyResources(this.expandableSplitterStatistics, "expandableSplitterStatistics");
			this.expandableSplitterStatistics.ExpandableControl = this.panelStatistics;
			this.expandableSplitterStatistics.ExpandFillColor = SystemColors.ControlDarkDark;
			this.expandableSplitterStatistics.ExpandFillColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.ExpandLineColor = Color.FromArgb(0, 0, 0);
			this.expandableSplitterStatistics.ExpandLineColorSchemePart = eColorSchemePart.ItemText;
			this.expandableSplitterStatistics.GripDarkColor = Color.FromArgb(0, 0, 0);
			this.expandableSplitterStatistics.GripDarkColorSchemePart = eColorSchemePart.ItemText;
			this.expandableSplitterStatistics.GripLightColor = SystemColors.ControlLightLight;
			this.expandableSplitterStatistics.GripLightColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.HotBackColor = SystemColors.ActiveCaption;
			this.expandableSplitterStatistics.HotBackColor2 = SystemColors.ControlLightLight;
			this.expandableSplitterStatistics.HotBackColor2SchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.HotBackColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.HotExpandFillColor = SystemColors.ControlDarkDark;
			this.expandableSplitterStatistics.HotExpandFillColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.HotExpandLineColor = Color.FromArgb(0, 0, 0);
			this.expandableSplitterStatistics.HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
			this.expandableSplitterStatistics.HotGripDarkColor = SystemColors.ControlDarkDark;
			this.expandableSplitterStatistics.HotGripDarkColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.HotGripLightColor = SystemColors.ControlLightLight;
			this.expandableSplitterStatistics.HotGripLightColorSchemePart = eColorSchemePart.None;
			this.expandableSplitterStatistics.Name = "expandableSplitterStatistics";
			this.expandableSplitterStatistics.Style = eSplitterStyle.Office2007;
			this.expandableSplitterStatistics.TabStop = false;
			this.panelStatistics.Controls.Add(this.panelStatisticsGrid);
			this.panelStatistics.Controls.Add(this.panelFrequencyNonCyclicDaqEvents);
			componentResourceManager.ApplyResources(this.panelStatistics, "panelStatistics");
			this.panelStatistics.Name = "panelStatistics";
			componentResourceManager.ApplyResources(this.panelStatisticsGrid, "panelStatisticsGrid");
			this.panelStatisticsGrid.Controls.Add(this.labelHint);
			this.panelStatisticsGrid.Controls.Add(this.treeListStatistics);
			this.panelStatisticsGrid.Name = "panelStatisticsGrid";
			componentResourceManager.ApplyResources(this.labelHint, "labelHint");
			this.labelHint.Name = "labelHint";
			componentResourceManager.ApplyResources(this.treeListStatistics, "treeListStatistics");
			this.treeListStatistics.Columns.AddRange(new TreeListColumn[]
			{
				this.treeListColumnChannel,
				this.treeListColumnSignalCount,
				this.treeListColumnByteCount,
				this.treeListColumnAdditionalLoad,
				this.treeListColumnAdditionalRelativeLoad
			});
			this.treeListStatistics.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.treeListStatistics.LookAndFeel.UseDefaultLookAndFeel = false;
			this.treeListStatistics.LookAndFeel.UseWindowsXPTheme = true;
			this.treeListStatistics.Name = "treeListStatistics";
			this.treeListStatistics.OptionsBehavior.AutoMoveRowFocus = true;
			this.treeListStatistics.OptionsBehavior.AutoSelectAllInEditor = false;
			this.treeListStatistics.OptionsBehavior.Editable = false;
			this.treeListStatistics.OptionsBehavior.ImmediateEditor = false;
			this.treeListStatistics.OptionsBehavior.PopulateServiceColumns = true;
			this.treeListStatistics.OptionsBehavior.ShowEditorOnMouseUp = true;
			this.treeListStatistics.OptionsMenu.EnableColumnMenu = false;
			this.treeListStatistics.OptionsMenu.EnableFooterMenu = false;
			this.treeListStatistics.OptionsMenu.ShowAutoFilterRowItem = false;
			this.treeListStatistics.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.treeListStatistics.OptionsSelection.EnableAppearanceFocusedRow = false;
			this.treeListStatistics.OptionsView.ShowFocusedFrame = false;
			this.treeListStatistics.OptionsView.ShowIndicator = false;
			this.treeListStatistics.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemProgressBar,
				this.repositoryItemTextEdit
			});
			this.treeListStatistics.CustomNodeCellEdit += new GetCustomNodeCellEditEventHandler(this.treeListStatistics_CustomNodeCellEdit);
			this.treeListStatistics.CustomDrawNodeCell += new CustomDrawNodeCellEventHandler(this.treeListStatistics_CustomDrawNodeCell);
			componentResourceManager.ApplyResources(this.treeListColumnChannel, "treeListColumnChannel");
			this.treeListColumnChannel.FieldName = "Channel";
			this.treeListColumnChannel.Name = "treeListColumnChannel";
			this.treeListColumnChannel.OptionsColumn.AllowEdit = false;
			this.treeListColumnChannel.OptionsColumn.AllowMove = false;
			this.treeListColumnChannel.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumnChannel.OptionsColumn.AllowSort = false;
			this.treeListColumnChannel.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.treeListColumnSignalCount, "treeListColumnSignalCount");
			this.treeListColumnSignalCount.FieldName = "Signals";
			this.treeListColumnSignalCount.Name = "treeListColumnSignalCount";
			this.treeListColumnSignalCount.OptionsColumn.AllowEdit = false;
			this.treeListColumnSignalCount.OptionsColumn.AllowMove = false;
			this.treeListColumnSignalCount.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumnSignalCount.OptionsColumn.AllowSort = false;
			this.treeListColumnSignalCount.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.treeListColumnByteCount, "treeListColumnByteCount");
			this.treeListColumnByteCount.FieldName = "Bytes";
			this.treeListColumnByteCount.Name = "treeListColumnByteCount";
			this.treeListColumnByteCount.OptionsColumn.AllowMove = false;
			this.treeListColumnByteCount.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumnByteCount.OptionsColumn.AllowSort = false;
			this.treeListColumnByteCount.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.treeListColumnAdditionalLoad, "treeListColumnAdditionalLoad");
			this.treeListColumnAdditionalLoad.FieldName = "Additional Load";
			this.treeListColumnAdditionalLoad.Name = "treeListColumnAdditionalLoad";
			this.treeListColumnAdditionalLoad.OptionsColumn.AllowEdit = false;
			this.treeListColumnAdditionalLoad.OptionsColumn.AllowMove = false;
			this.treeListColumnAdditionalLoad.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumnAdditionalLoad.OptionsColumn.AllowSort = false;
			this.treeListColumnAdditionalLoad.OptionsColumn.ReadOnly = true;
			componentResourceManager.ApplyResources(this.treeListColumnAdditionalRelativeLoad, "treeListColumnAdditionalRelativeLoad");
			this.treeListColumnAdditionalRelativeLoad.ColumnEdit = this.repositoryItemProgressBar;
			this.treeListColumnAdditionalRelativeLoad.FieldName = "Additional Relative Load";
			this.treeListColumnAdditionalRelativeLoad.Name = "treeListColumnAdditionalRelativeLoad";
			this.treeListColumnAdditionalRelativeLoad.OptionsColumn.AllowEdit = false;
			this.treeListColumnAdditionalRelativeLoad.OptionsColumn.AllowMove = false;
			this.treeListColumnAdditionalRelativeLoad.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumnAdditionalRelativeLoad.OptionsColumn.AllowSort = false;
			this.treeListColumnAdditionalRelativeLoad.OptionsColumn.ReadOnly = true;
			this.repositoryItemProgressBar.DisplayFormat.FormatString = "{0}%";
			this.repositoryItemProgressBar.DisplayFormat.FormatType = FormatType.Numeric;
			this.repositoryItemProgressBar.EndColor = Color.Red;
			this.repositoryItemProgressBar.LookAndFeel.SkinName = "VS2010";
			this.repositoryItemProgressBar.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
			this.repositoryItemProgressBar.LookAndFeel.UseDefaultLookAndFeel = false;
			this.repositoryItemProgressBar.Name = "repositoryItemProgressBar";
			this.repositoryItemProgressBar.PercentView = false;
			this.repositoryItemProgressBar.ProgressViewStyle = ProgressViewStyle.Solid;
			this.repositoryItemProgressBar.ReadOnly = true;
			this.repositoryItemProgressBar.ShowTitle = true;
			this.repositoryItemProgressBar.StartColor = Color.FromArgb(0, 192, 0);
			this.repositoryItemTextEdit.Appearance.Options.UseTextOptions = true;
			this.repositoryItemTextEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(this.repositoryItemTextEdit, "repositoryItemTextEdit");
			this.repositoryItemTextEdit.Name = "repositoryItemTextEdit";
			this.repositoryItemTextEdit.ReadOnly = true;
			componentResourceManager.ApplyResources(this.panelFrequencyNonCyclicDaqEvents, "panelFrequencyNonCyclicDaqEvents");
			this.panelFrequencyNonCyclicDaqEvents.Controls.Add(this.tableLayoutPanel2);
			this.panelFrequencyNonCyclicDaqEvents.Controls.Add(this.textBoxBarFrequencyNonCyclicDaqEvents);
			this.panelFrequencyNonCyclicDaqEvents.Controls.Add(this.labelFrequencyNonCyclicDaqEvents);
			this.panelFrequencyNonCyclicDaqEvents.Controls.Add(this.trackBarFrequencyNonCyclicDaqEvents);
			this.panelFrequencyNonCyclicDaqEvents.Name = "panelFrequencyNonCyclicDaqEvents";
			componentResourceManager.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.label9, 9, 0);
			this.tableLayoutPanel2.Controls.Add(this.labelFrequencyMicrosecond, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label8, 8, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label7, 7, 0);
			this.tableLayoutPanel2.Controls.Add(this.label2, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.label6, 6, 0);
			this.tableLayoutPanel2.Controls.Add(this.label3, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.label5, 5, 0);
			this.tableLayoutPanel2.Controls.Add(this.label4, 4, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.label9.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.labelFrequencyMicrosecond.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.labelFrequencyMicrosecond, "labelFrequencyMicrosecond");
			this.labelFrequencyMicrosecond.Name = "labelFrequencyMicrosecond";
			this.label8.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.label1.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.label7.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			this.label2.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.label6.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.label3.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.label5.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.label4.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.textBoxBarFrequencyNonCyclicDaqEvents, "textBoxBarFrequencyNonCyclicDaqEvents");
			this.textBoxBarFrequencyNonCyclicDaqEvents.Name = "textBoxBarFrequencyNonCyclicDaqEvents";
			this.textBoxBarFrequencyNonCyclicDaqEvents.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelFrequencyNonCyclicDaqEvents, "labelFrequencyNonCyclicDaqEvents");
			this.labelFrequencyNonCyclicDaqEvents.Name = "labelFrequencyNonCyclicDaqEvents";
			componentResourceManager.ApplyResources(this.trackBarFrequencyNonCyclicDaqEvents, "trackBarFrequencyNonCyclicDaqEvents");
			this.trackBarFrequencyNonCyclicDaqEvents.LargeChange = 1;
			this.trackBarFrequencyNonCyclicDaqEvents.Maximum = 9;
			this.trackBarFrequencyNonCyclicDaqEvents.Name = "trackBarFrequencyNonCyclicDaqEvents";
			this.trackBarFrequencyNonCyclicDaqEvents.Value = 7;
			this.trackBarFrequencyNonCyclicDaqEvents.Scroll += new EventHandler(this.trackBarFrequencyNonCyclicDaqEvents_Scroll);
			this.trackBarFrequencyNonCyclicDaqEvents.KeyUp += new KeyEventHandler(this.trackBarFrequencyNonCyclicDaqEvents_KeyUp);
			this.trackBarFrequencyNonCyclicDaqEvents.MouseDown += new MouseEventHandler(this.trackBarFrequencyNonCyclicDaqEvents_MouseDown);
			this.trackBarFrequencyNonCyclicDaqEvents.MouseUp += new MouseEventHandler(this.trackBarFrequencyNonCyclicDaqEvents_MouseUp);
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonAddSignal, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.splitButtonExport, 10, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonAddAction, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemoveAction, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonImport, 8, 0);
			this.tableLayoutPanel1.Controls.Add(this.label_signallist, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label_signal, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemoveSignal, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonEditCondition, 6, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonAddSignal, "buttonAddSignal");
			this.buttonAddSignal.Name = "buttonAddSignal";
			this.buttonAddSignal.UseVisualStyleBackColor = true;
			this.buttonAddSignal.Click += new EventHandler(this.buttonAddSignal_Click);
			componentResourceManager.ApplyResources(this.splitButtonExport, "splitButtonExport");
			this.splitButtonExport.Name = "splitButtonExport";
			this.splitButtonExport.ShowSplitAlways = true;
			this.splitButtonExport.UseVisualStyleBackColor = true;
			this.splitButtonExport.Click += new EventHandler(this.splitButtonExport_Click);
			componentResourceManager.ApplyResources(this.buttonAddAction, "buttonAddAction");
			this.buttonAddAction.Name = "buttonAddAction";
			this.buttonAddAction.UseVisualStyleBackColor = true;
			this.buttonAddAction.Click += new EventHandler(this.buttonAddAction_Click);
			componentResourceManager.ApplyResources(this.buttonRemoveAction, "buttonRemoveAction");
			this.buttonRemoveAction.Image = Resources.ImageDelete;
			this.buttonRemoveAction.Name = "buttonRemoveAction";
			this.buttonRemoveAction.UseVisualStyleBackColor = true;
			this.buttonRemoveAction.Click += new EventHandler(this.buttonRemoveAction_Click);
			componentResourceManager.ApplyResources(this.buttonImport, "buttonImport");
			this.buttonImport.Name = "buttonImport";
			this.buttonImport.UseVisualStyleBackColor = true;
			this.buttonImport.Click += new EventHandler(this.buttonImport_Click);
			componentResourceManager.ApplyResources(this.label_signallist, "label_signallist");
			this.label_signallist.Name = "label_signallist";
			componentResourceManager.ApplyResources(this.label_signal, "label_signal");
			this.label_signal.Name = "label_signal";
			componentResourceManager.ApplyResources(this.buttonRemoveSignal, "buttonRemoveSignal");
			this.buttonRemoveSignal.Image = Resources.ImageDelete;
			this.buttonRemoveSignal.Name = "buttonRemoveSignal";
			this.buttonRemoveSignal.UseVisualStyleBackColor = true;
			this.buttonRemoveSignal.Click += new EventHandler(this.buttonRemoveSignal_Click);
			componentResourceManager.ApplyResources(this.buttonEditCondition, "buttonEditCondition");
			this.buttonEditCondition.Name = "buttonEditCondition";
			this.buttonEditCondition.UseVisualStyleBackColor = true;
			this.buttonEditCondition.Click += new EventHandler(this.buttonEditCondition_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.mGroupBoxCcpXcpDescriptions);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CcpXcpSignalRequests";
			this.mGroupBoxCcpXcpDescriptions.ResumeLayout(false);
			this.mGroupBoxCcpXcpDescriptions.PerformLayout();
			this.panel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panelStatistics.ResumeLayout(false);
			this.panelStatisticsGrid.ResumeLayout(false);
			((ISupportInitialize)this.treeListStatistics).EndInit();
			((ISupportInitialize)this.repositoryItemProgressBar).EndInit();
			((ISupportInitialize)this.repositoryItemTextEdit).EndInit();
			this.panelFrequencyNonCyclicDaqEvents.ResumeLayout(false);
			this.panelFrequencyNonCyclicDaqEvents.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			((ISupportInitialize)this.trackBarFrequencyNonCyclicDaqEvents).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
