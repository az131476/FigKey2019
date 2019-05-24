using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.TriggersPage
{
	public class Triggers : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<TriggerConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<DiagnosticActionsConfiguration>, IUpdateObserver<DiagnosticsDatabaseConfiguration>, IUpdateObserver, ISplitButtonExClient
	{
		public delegate void DataChangedHandler(TriggerConfiguration data);

		private readonly GUIElementManager_Control mGuiElementManager;

		private readonly PageValidator mPageValidator;

		private bool mIsInitControls;

		private LoggerType mLoggerType;

		private readonly SplitButtonEx mSplitButtonEx;

		private IContainer components;

		private RadioButton radioButtonPermanentLogging;

		private RadioButton radioButtonTriggeredLogging;

		private Label labelPostTriggerTime;

		private TextBox textBoxPostTriggerTime;

		private Label labelPostTriggerTimeUnit;

		private GroupBox groupBoxTriggerUsage;

		private GroupBox groupBoxTriggerTime;

		private GroupBox mGroupBoxTriggers;

		private TriggerTree mTriggerTree;

		private Button buttonRemove;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private RadioButton radioButtonConditionedOnOffLogging;

		private Label labelMB;

		private ComboBox comboBoxRingBufferSize;

		private CheckBox checkBoxIsActive;

		private Label labelUseRingBufferSize;

		private Label labelNumOfFiles;

		private LinkLabel linkLabelNumOfFilesForCardSize;

		private Button mButtonEstimateRequiredRingbufferSize;

		private LinkLabel mLinkLabelPreTriggerTime;

		private SplitButton mSplitButton;

		private Label mLabelAddEvent;

		private TableLayoutPanel tableLayoutPanelTriggers;

		private Panel panelRingBufferSize;

		private Panel panelOptionsAndTriggers;

		private Label labelPreTriggerTimeUnit;

		private TextBox textBoxPreTriggerTime;

		private PictureBox pictureBoxModeInfo;

		private TableLayoutPanel tableLayoutPanelGridOps;

		private Label labelMB2;

		private Label labelTotalMemValue;

		private Label labelTotal;

		private TextBox textBoxMaxNumOfFiles;

		private Label labelMaxNumOfFiles;

		private ComboBox comboBoxOperatingMode;

		private Label labelOperatingMode;

		private Label labelOperatingModeExplanation;

		private Label labelClosingBracket;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.mTriggerTree.ApplicationDatabaseManager;
			}
			set
			{
				this.mTriggerTree.ApplicationDatabaseManager = value;
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get;
			set;
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get;
			set;
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
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
				return this.mTriggerTree.ModelValidator;
			}
			set
			{
				this.mTriggerTree.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.mTriggerTree.ModelEditor;
			}
			set
			{
				this.mTriggerTree.ModelEditor = value;
			}
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
				return PageTypeHelper.GetTriggerPageForMemory(this.MemoryNr);
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
					this.mTriggerTree.DisplayErrors();
				}
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
				EventPresenter eventPresenter;
				if (this.mTriggerTree.TryGetSelectedPresenter(out eventPresenter))
				{
					if (eventPresenter is RecordTriggerPresenter)
					{
						list.Add(new RecordTrigger((eventPresenter as RecordTriggerPresenter).RecordTrigger));
					}
					else if (eventPresenter != null && eventPresenter.Event != null && !(eventPresenter.Event is DummyEvent))
					{
						list.Add(new DummyAction((Event)eventPresenter.Event.Clone()));
					}
				}
				return list;
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

		public Triggers()
		{
			this.InitializeComponent();
			this.mTriggerTree.SelectionChanged += new EventHandler(this.OnTriggerTreeSelectionChanged);
			this.mGuiElementManager = new GUIElementManager_Control();
			CustomErrorProvider customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.mPageValidator = new PageValidator(customErrorProvider);
			this.mSplitButtonEx = new SplitButtonEx(this);
			this.mIsInitControls = false;
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is TriggerConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContextHelper.GetTriggerUpdateContextForMemoryNr(this.MemoryNr));
			}
			this.mSplitButton.AutoSize = false;
			GUIUtil.InitSplitButtonMenuEventTypes(this.mSplitButtonEx);
			this.mIsInitControls = true;
			this.InitPossibleRingBufferSizes();
			this.InitPossibleOperatingModes();
			this.mIsInitControls = false;
			this.mTriggerTree.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.mSplitButtonEx.UpdateSplitMenu();
			this.UpdateLabels();
			this.ResetValidationFramework();
			this.mTriggerTree.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			bool flag = true;
			bool flag2 = false;
			this.mPageValidator.General.ResetAllErrorProviders();
			this.mPageValidator.General.ResetAllFormatErrors();
			if (this.mTriggerTree.TriggerConfiguration == null)
			{
				return true;
			}
			if ((long)this.MemoryNr > (long)((ulong)this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories))
			{
				return true;
			}
			bool flag3;
			flag &= this.mPageValidator.Control.UpdateModel<bool>(this.checkBoxIsActive.Checked, this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.IsActive, this.mGuiElementManager.GetGUIElement(this.checkBoxIsActive), out flag3);
			flag2 |= flag3;
			if (this.checkBoxIsActive.Checked)
			{
				double size;
				if (!GUIUtil.DisplayStringToFloatNumber(this.comboBoxRingBufferSize.Text, out size))
				{
					((IModelValidationResultCollector)this.mPageValidator).SetErrorText(ValidationErrorClass.FormatError, this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size, Resources.ErrorNumberExpected);
					flag = false;
				}
				else
				{
					double sizeInMBytes = GUIUtil.RoundSize(size, 2u);
					uint num;
					if (!GUIUtil.TryConvertSizeToKBytes(sizeInMBytes, out num))
					{
						((IModelValidationResultCollector)this.mPageValidator).SetErrorText(ValidationErrorClass.FormatError, this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.GetFractionedMByteDisplayString((uint)RingBufferMemoriesManager.MinRingBufferSizeKB), GUIUtil.GetFractionedMByteDisplayString((uint)RingBufferMemoriesManager.MaxSumRingBufferSizesKB)));
						flag = false;
					}
					else
					{
						flag &= this.mPageValidator.Control.UpdateModel<uint>(num, this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size, this.mGuiElementManager.GetGUIElement(this.comboBoxRingBufferSize), out flag3);
						this.comboBoxRingBufferSize.Text = GUIUtil.GetFractionedMByteDisplayString(num);
						flag2 |= flag3;
					}
				}
				flag &= this.mPageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxMaxNumOfFiles.Text, this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles, this.mGuiElementManager.GetGUIElement(this.textBoxMaxNumOfFiles), out flag3);
				flag2 |= flag3;
				flag &= this.mPageValidator.Control.UpdateModel<RingBufferOperatingMode>(GUIUtil.MapString2RingBufferOperatingMode(this.comboBoxOperatingMode.SelectedItem.ToString()), this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.OperatingMode, this.mGuiElementManager.GetGUIElement(this.comboBoxOperatingMode), out flag3);
				flag2 |= flag3;
			}
			else
			{
				this.comboBoxRingBufferSize.Text = GUIUtil.GetFractionedMByteDisplayString(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size.Value);
			}
			TriggerMode value = TriggerMode.Triggered;
			if (this.radioButtonPermanentLogging.Checked)
			{
				value = TriggerMode.Permanent;
			}
			if (this.radioButtonConditionedOnOffLogging.Checked)
			{
				value = TriggerMode.OnOff;
			}
			flag &= this.mPageValidator.Control.UpdateModel<TriggerMode>(value, this.mTriggerTree.TriggerConfiguration.TriggerMode, this.mGuiElementManager.GetGUIElement(this.radioButtonPermanentLogging), out flag3);
			flag2 |= flag3;
			if (this.checkBoxIsActive.Checked && this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.Triggered)
			{
				flag &= this.mPageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxPostTriggerTime.Text, this.mTriggerTree.TriggerConfiguration.PostTriggerTime, this.mGuiElementManager.GetGUIElement(this.textBoxPostTriggerTime), out flag3);
				flag2 |= flag3;
			}
			else
			{
				this.textBoxPostTriggerTime.Text = this.mTriggerTree.TriggerConfiguration.PostTriggerTime.Value.ToString(CultureInfo.InvariantCulture);
			}
			((IModelValidationResultCollector)this.mPageValidator).ResetAllModelErrors();
			flag &= this.mTriggerTree.ModelValidator.ValidateRingBufferSettings(this.mTriggerTree.TriggerConfiguration, flag2, this.mPageValidator);
			if (this.checkBoxIsActive.Checked)
			{
				if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.Triggered)
				{
					flag &= this.mTriggerTree.ModelValidator.ValidatePostTriggerTime(this.mTriggerTree.TriggerConfiguration, flag2, this.mPageValidator);
				}
				flag &= this.mTriggerTree.ValidateInput(false);
			}
			else
			{
				this.mTriggerTree.ResetValidationFramework();
			}
			this.mPageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.DisplayMaxTotalMemorySize();
			this.DisplayOperatingModeLabelText();
			return flag;
		}

		bool IPropertyWindow.HasErrors()
		{
			bool flag = this.mPageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
			return flag | this.mTriggerTree.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			bool flag = this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
			return flag | this.mTriggerTree.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			bool flag = this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
			return flag | this.mTriggerTree.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.mPageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			bool flag = arg_13_0.HasErrors(errorClasses);
			return flag | this.mTriggerTree.HasFormatErrors();
		}

		private void ResetValidationFramework()
		{
			this.mPageValidator.General.Reset();
			this.mGuiElementManager.Reset();
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.mLoggerType == data || data == LoggerType.Unknown)
			{
				return;
			}
			this.mLoggerType = data;
		}

		void IUpdateObserver<TriggerConfiguration>.Update(TriggerConfiguration data)
		{
			if ((long)this.MemoryNr > (long)((ulong)this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories))
			{
				return;
			}
			if (data.MemoryNr == this.MemoryNr)
			{
				this.mTriggerTree.TriggerConfiguration = data;
			}
			if (this.mTriggerTree.TriggerConfiguration == null)
			{
				return;
			}
			this.mIsInitControls = true;
			this.mSplitButtonEx.UpdateSplitMenu();
			this.UpdateLabels();
			this.InitPossibleRingBufferSizes();
			this.ShowOrHideIsMemoryActiveCheckBox();
			if (!this.mPageValidator.General.HasFormatError(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles))
			{
				this.textBoxMaxNumOfFiles.Text = this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles.ToString();
			}
			if (!this.mPageValidator.General.HasFormatError(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.OperatingMode))
			{
				this.comboBoxOperatingMode.SelectedItem = GUIUtil.MapRingBufferOperatingMode2String(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.OperatingMode.Value);
			}
			this.DisplayNumberOfFilesForCardSize();
			if (string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.comboBoxRingBufferSize)))
			{
				this.comboBoxRingBufferSize.Text = GUIUtil.GetFractionedMByteDisplayString(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size.Value);
			}
			this.checkBoxIsActive.Checked = this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.IsActive.Value;
			if (string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxPostTriggerTime)))
			{
				this.textBoxPostTriggerTime.Text = this.mTriggerTree.TriggerConfiguration.PostTriggerTime.ToString();
			}
			this.textBoxPreTriggerTime.Text = this.ConfigurationManagerService.MetaInformation.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value.ToString(CultureInfo.InvariantCulture);
			this.radioButtonTriggeredLogging.Text = string.Format(Resources_Trigger.TriggerModeOptLabelTriggered, this.mTriggerTree.TriggerConfiguration.Triggers.Count);
			this.radioButtonConditionedOnOffLogging.Text = (this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport ? string.Format(Resources_Trigger.TriggerModeOptLabelConditioned, this.mTriggerTree.TriggerConfiguration.OnOffTriggers.Count) : string.Format(Resources_Trigger.TriggerModeOptLabelConditionedNoMarkers, this.mTriggerTree.TriggerConfiguration.OnOffTriggers.Count));
			this.radioButtonPermanentLogging.Text = (this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport ? string.Format(Resources_Trigger.TriggerModeOptLabelPermanent, this.mTriggerTree.TriggerConfiguration.PermanentMarkers.Count) : Resources_Trigger.TriggerModeOptLabelPermanentWithoutMarkers);
			this.radioButtonPermanentLogging.Checked = (TriggerMode.Permanent == this.mTriggerTree.TriggerConfiguration.TriggerMode.Value);
			this.radioButtonTriggeredLogging.Checked = (TriggerMode.Triggered == this.mTriggerTree.TriggerConfiguration.TriggerMode.Value);
			this.radioButtonConditionedOnOffLogging.Checked = (TriggerMode.OnOff == this.mTriggerTree.TriggerConfiguration.TriggerMode.Value);
			this.EnableControlsForMemoryActivityAndLoggingMode();
			this.DisplayImageForTriggerMode();
			this.mIsInitControls = false;
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode data)
		{
			this.mTriggerTree.DisplayMode = data;
		}

		void IUpdateObserver<DiagnosticActionsConfiguration>.Update(DiagnosticActionsConfiguration data)
		{
			this.DiagnosticActionsConfiguration = data;
			this.mTriggerTree.DiagnosticActionsConfiguration = this.DiagnosticActionsConfiguration;
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<DiagnosticsDatabaseConfiguration>.Update(DiagnosticsDatabaseConfiguration data)
		{
			this.DiagnosticsDatabaseConfiguration = data;
			this.mTriggerTree.DiagnosticsDatabaseConfiguration = this.DiagnosticsDatabaseConfiguration;
			((IPropertyWindow)this).ValidateInput();
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			if (action is RecordTrigger)
			{
				if (this.mTriggerTree.NextAddAction != TriggerTree.AddAction.Condition)
				{
					return ConfigClipboardManager.AcceptType.Action;
				}
				if (!this.Accept(action.Event))
				{
					return ConfigClipboardManager.AcceptType.None;
				}
				return ConfigClipboardManager.AcceptType.Event;
			}
			else
			{
				if (action.Event == null)
				{
					return ConfigClipboardManager.AcceptType.None;
				}
				if (!this.Accept(action.Event))
				{
					return ConfigClipboardManager.AcceptType.None;
				}
				return ConfigClipboardManager.AcceptType.Event;
			}
		}

		public bool Accept(Event evt)
		{
			if (evt == null)
			{
				return false;
			}
			if (evt is CombinedEvent)
			{
				if (this.mTriggerTree.NextAddAction == TriggerTree.AddAction.Condition)
				{
					EventPresenter eventPresenter;
					if (!(evt as CombinedEvent).IsConjunction.Value && this.mTriggerTree.TryGetSelectedPresenter(out eventPresenter) && eventPresenter.Parent != null && eventPresenter.Parent.Event != null && eventPresenter.Parent.Event is CombinedEvent && (eventPresenter.Parent.Event as CombinedEvent).IsConjunction.Value)
					{
						return true;
					}
				}
				else if ((evt as CombinedEvent).IsConjunction.Value)
				{
					return true;
				}
				return false;
			}
			return true;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			if (this.Accept(action) != ConfigClipboardManager.AcceptType.Action)
			{
				return false;
			}
			RecordTrigger recordTrigger = action as RecordTrigger;
			this.ResetTriggerEffect(recordTrigger);
			this.ResetTriggerName(recordTrigger);
			this.mTriggerTree.AddTrigger(recordTrigger);
			return true;
		}

		public bool Insert(Event evt)
		{
			if (this.Accept(evt))
			{
				if (this.mTriggerTree.NextAddAction == TriggerTree.AddAction.Condition)
				{
					this.mTriggerTree.AddCondition(evt);
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.Create(evt);
					this.ResetTriggerEffect(recordTrigger);
					this.ResetTriggerName(recordTrigger);
					this.mTriggerTree.AddTrigger(recordTrigger);
				}
				return true;
			}
			return false;
		}

		private void ResetTriggerEffect(RecordTrigger rt)
		{
			if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.Triggered && rt.TriggerEffect.Value != TriggerEffect.Normal && rt.TriggerEffect.Value != TriggerEffect.Single && rt.TriggerEffect.Value != TriggerEffect.EndMeasurement)
			{
				rt.TriggerEffect.Value = TriggerEffect.Normal;
			}
			if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.Permanent && rt.TriggerEffect.Value != TriggerEffect.Marker)
			{
				rt.TriggerEffect.Value = TriggerEffect.Marker;
			}
			if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.OnOff && rt.TriggerEffect.Value != TriggerEffect.Marker && rt.TriggerEffect.Value != TriggerEffect.LoggingOn && rt.TriggerEffect.Value != TriggerEffect.LoggingOff)
			{
				rt.TriggerEffect.Value = TriggerEffect.LoggingOn;
			}
		}

		private void ResetTriggerName(RecordTrigger rt)
		{
			if (rt.Name == null || string.IsNullOrEmpty(rt.Name.Value))
			{
				rt.Name = new ValidatedProperty<string>(this.mTriggerTree.CreateMarkerName(rt));
				((IPropertyWindow)this).ModelEditor.UpdateReferencedTriggerNameInDataTransferTriggers(rt.Event.Id, rt.Name.Value);
			}
		}

		private void OnTriggerTreeSelectionChanged(object sender, EventArgs e)
		{
			this.buttonRemove.Enabled = this.mTriggerTree.CanRemoveSelection;
			this.mSplitButtonEx.UpdateSplitMenu();
			this.UpdateLabels();
		}

		private void checkBoxIsActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			this.EnableControlsForMemoryActivityAndLoggingMode();
			((IPropertyWindow)this).ValidateInput();
		}

		private void comboBoxRingBufferSize_Validating(object sender, CancelEventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			((IPropertyWindow)this).ValidateInput();
		}

		private void comboBoxRingBufferSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			((IPropertyWindow)this).ValidateInput();
		}

		private void ButtonEstimateRequiredRingbufferSize_Click(object sender, EventArgs e)
		{
			this.ShowBufferSizeCalculator();
		}

		private void LinkLabelPreTriggerTime_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.ShowBufferSizeCalculator();
		}

		private void ShowBufferSizeCalculator()
		{
			using (BufferSizeCalculatorForm bufferSizeCalculatorForm = new BufferSizeCalculatorForm(this.ConfigurationManagerService, this.mTriggerTree.TriggerConfiguration.PostTriggerTime.Value))
			{
				if (DialogResult.OK == bufferSizeCalculatorForm.ShowDialog(this))
				{
					this.comboBoxRingBufferSize.Text = bufferSizeCalculatorForm.EstimatedRingBufferSizeMB.ToString(CultureInfo.InvariantCulture);
					((IPropertyWindow)this).ValidateInput();
				}
			}
		}

		private void linkLabelNumOfFilesForCardSize_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.mPageValidator.General.HasError(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size))
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				return;
			}
			RingBufferMemoriesManager.CurrentCardSizeMB = (long)((ulong)CardSizeNumOfFilesCalculator.DisplayDialog(this.mTriggerTree.TriggerConfiguration, this.mTriggerTree.ModelValidator.LoggerSpecifics, (uint)RingBufferMemoriesManager.CurrentCardSizeMB));
			this.DisplayNumberOfFilesForCardSize();
		}

		private void textBoxMaxNumOfFiles_Validating(object sender, CancelEventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			((IPropertyWindow)this).ValidateInput();
		}

		private void radioButtonLoggingMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			this.EnableControlsForMemoryActivityAndLoggingMode();
			this.DisplayImageForTriggerMode();
			TriggerConfiguration triggerConfiguration = this.mTriggerTree.TriggerConfiguration;
			this.mTriggerTree.Reset();
			this.mTriggerTree.TriggerConfiguration = triggerConfiguration;
			((IPropertyWindow)this).ValidateInput();
		}

		private void textBoxPostTriggerTime_Validating(object sender, CancelEventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			((IPropertyWindow)this).ValidateInput();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.mTriggerTree.RemoveSelection();
		}

		private void comboBoxOperatingMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			((IPropertyWindow)this).ValidateInput();
			this.DisplayOperatingModeLabelText();
		}

		private List<RecordTrigger> CreateTrigger(string selectedTriggerName)
		{
			List<RecordTrigger> list = new List<RecordTrigger>();
			if (selectedTriggerName == Vocabulary.TriggerTypeNameColCANId)
			{
				RecordTrigger recordTrigger = this.CreateCanIdTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Vocabulary.TriggerTypeNameColLINId)
			{
				RecordTrigger recordTrigger = this.CreateLinIdTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				RecordTrigger recordTrigger = this.CreateCanDataTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				RecordTrigger recordTrigger = this.CreateLinDataTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				RecordTrigger recordTrigger = this.CreateFlexrayIdTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				list = this.CreateSymbolicMessageTrigger(BusType.Bt_CAN);
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				list = this.CreateSymbolicMessageTrigger(BusType.Bt_LIN);
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				list = this.CreateSymbolicMessageTrigger(BusType.Bt_FlexRay);
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				RecordTrigger recordTrigger = this.CreateSymbolicSignalTrigger(BusType.Bt_CAN);
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				RecordTrigger recordTrigger = this.CreateSymbolicSignalTrigger(BusType.Bt_LIN);
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				RecordTrigger recordTrigger = this.CreateSymbolicSignalTrigger(BusType.Bt_FlexRay);
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				RecordTrigger recordTrigger = this.CreateMsgTimeoutTrigger(BusType.Bt_CAN);
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				RecordTrigger recordTrigger = this.CreateMsgTimeoutTrigger(BusType.Bt_LIN);
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				RecordTrigger recordTrigger = this.CreateDigitalInputTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				RecordTrigger recordTrigger = this.CreateAnalogInputTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColKey)
			{
				RecordTrigger recordTrigger = this.CreateKeyTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColCANBusStatistics)
			{
				RecordTrigger recordTrigger = this.CreateCanBusStatisticsTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				RecordTrigger recordTrigger = this.CreateIgnitionTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				int num;
				bool flag;
				if (this.mTriggerTree.ModelValidator.IsActiveVoCANEventConfigured(out num, out flag))
				{
					InformMessageBox.Error(Resources_Trigger.ErrorOnlyOneVoCANEventAllowed);
				}
				else
				{
					RecordTrigger recordTrigger = this.CreateVoCanRecordingTrigger();
					if (recordTrigger != null)
					{
						list.Add(recordTrigger);
						string value;
						if (!((IPropertyWindow)this).SemanticChecker.IsCANChannelConfigSoundForVoCANTrigger(out value))
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.AppendLine(value);
							stringBuilder.AppendLine(Resources.QuestionAdjustSettingsAutomatically);
							if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.Yes)
							{
								((IPropertyWindow)this).ModelEditor.SetChannelConfigurationForVoCAN();
							}
						}
					}
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColANDCondition || selectedTriggerName == Resources_Trigger.TriggerTypeNameColORCondition)
			{
				RecordTrigger recordTrigger = this.CreateConditionGroupTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				RecordTrigger recordTrigger = this.CreateCcpXcpSignalTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
			{
				RecordTrigger recordTrigger = this.CreateDiagnosticSignalTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			else if (selectedTriggerName == Resources_Trigger.TriggerTypeNameColIncEvent)
			{
				RecordTrigger recordTrigger = this.CreateIncEventTrigger();
				if (recordTrigger != null)
				{
					list.Add(recordTrigger);
				}
			}
			return list;
		}

		private void AssignCommonTriggerDefaults(ref RecordTrigger trigger)
		{
			trigger.Action.Value = TriggerAction.None;
			trigger.TriggerEffect.Value = this.GetDefaultTriggerEffectForCurrentModeAndMemoryNr();
		}

		private RecordTrigger CreateCanIdTrigger()
		{
			RecordTrigger result;
			using (CANIdCondition cANIdCondition = new CANIdCondition(this.mTriggerTree.ModelValidator))
			{
				cANIdCondition.ResetToDefaults();
				if (DialogResult.OK != cANIdCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateCanIdTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					CANIdEvent cANIdEvent = recordTrigger.Event as CANIdEvent;
					if (cANIdEvent != null)
					{
						cANIdEvent.Assign(cANIdCondition.CANIdEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateCanBusStatisticsTrigger()
		{
			RecordTrigger result;
			using (CANBusStatisticsCondition cANBusStatisticsCondition = new CANBusStatisticsCondition(this.mTriggerTree.ModelValidator))
			{
				cANBusStatisticsCondition.ResetToDefaults();
				if (DialogResult.OK != cANBusStatisticsCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateCanBusStatisticsTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					CANBusStatisticsEvent cANBusStatisticsEvent = recordTrigger.Event as CANBusStatisticsEvent;
					if (cANBusStatisticsEvent != null)
					{
						cANBusStatisticsEvent.Assign(cANBusStatisticsCondition.CANBusStatisticsEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateLinIdTrigger()
		{
			RecordTrigger result;
			using (LINIdCondition lINIdCondition = new LINIdCondition(this.mTriggerTree.ModelValidator))
			{
				lINIdCondition.ResetToDefaults();
				if (DialogResult.OK != lINIdCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateLinIdTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					LINIdEvent lINIdEvent = recordTrigger.Event as LINIdEvent;
					if (lINIdEvent != null)
					{
						lINIdEvent.Assign(lINIdCondition.LINIdEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateFlexrayIdTrigger()
		{
			RecordTrigger result;
			using (FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition(this.mTriggerTree.ModelValidator))
			{
				flexrayIdCondition.ResetToDefaults();
				if (DialogResult.OK != flexrayIdCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateFlexrayIdTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					FlexrayIdEvent flexrayIdEvent = recordTrigger.Event as FlexrayIdEvent;
					if (flexrayIdEvent != null)
					{
						flexrayIdEvent.Assign(flexrayIdCondition.FlexrayIdEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateCanDataTrigger()
		{
			RecordTrigger result;
			using (CANDataCondition cANDataCondition = new CANDataCondition(this.mTriggerTree.ModelValidator, null))
			{
				cANDataCondition.InitDefaultValues();
				if (DialogResult.OK != cANDataCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateCanDataTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					CANDataEvent cANDataEvent = recordTrigger.Event as CANDataEvent;
					if (cANDataEvent != null)
					{
						cANDataEvent.Assign(cANDataCondition.CANDataEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateLinDataTrigger()
		{
			RecordTrigger result;
			using (LINDataCondition lINDataCondition = new LINDataCondition(this.mTriggerTree.ModelValidator, null))
			{
				lINDataCondition.InitDefaultValues();
				if (DialogResult.OK != lINDataCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateLinDataTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					LINDataEvent lINDataEvent = recordTrigger.Event as LINDataEvent;
					if (lINDataEvent != null)
					{
						lINDataEvent.Assign(lINDataCondition.LINDataEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private List<RecordTrigger> CreateSymbolicMessageTrigger(BusType busType)
		{
			string[] array = null;
			string[] array2 = null;
			string[] array3 = null;
			string[] array4 = null;
			BusType[] array5 = null;
			bool[] array6 = null;
			if (!this.mTriggerTree.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				switch (busType)
				{
				case BusType.Bt_LIN:
					arg = Vocabulary.LIN;
					break;
				case BusType.Bt_FlexRay:
					arg = Vocabulary.Flexray;
					break;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			if (!this.ApplicationDatabaseManager.SelectMessageInDatabase(busType, ref array, ref array4, ref array2, ref array3, ref array5, ref array6, true))
			{
				return null;
			}
			List<RecordTrigger> list = new List<RecordTrigger>();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				string value;
				if (!this.mTriggerTree.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(array[i], array3[i], array2[i], array5[i], out value))
				{
					stringBuilder.AppendLine(value);
					stringBuilder.AppendLine();
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateSymbolicMessageTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					SymbolicMessageEvent symbolicMessageEvent = recordTrigger.Event as SymbolicMessageEvent;
					if (symbolicMessageEvent != null)
					{
						symbolicMessageEvent.MessageName.Value = array[i];
						symbolicMessageEvent.DatabaseName.Value = array4[i];
						symbolicMessageEvent.DatabasePath.Value = this.mTriggerTree.ModelValidator.GetFilePathRelativeToConfiguration(array2[i]);
						symbolicMessageEvent.NetworkName.Value = array3[i];
						symbolicMessageEvent.BusType.Value = busType;
						symbolicMessageEvent.IsFlexrayPDU.Value = array6[i];
						symbolicMessageEvent.ChannelNumber.Value = 1u;
						IList<uint> channelAssignmentOfDatabase = this.mTriggerTree.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageEvent.DatabasePath.Value, symbolicMessageEvent.NetworkName.Value);
						if (channelAssignmentOfDatabase.Count > 0)
						{
							symbolicMessageEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
							if (symbolicMessageEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
							{
								symbolicMessageEvent.ChannelNumber.Value = 1u;
								if (array[i].EndsWith(Constants.FlexrayChannelB_Postfix))
								{
									symbolicMessageEvent.ChannelNumber.Value = 2u;
								}
							}
						}
					}
					list.Add(recordTrigger);
				}
			}
			if (stringBuilder.Length > 0)
			{
				InformMessageBox.Error(stringBuilder.ToString());
			}
			return list;
		}

		private RecordTrigger CreateSymbolicSignalTrigger(BusType busType)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string value = "";
			string text4 = "";
			bool value2 = false;
			if (!this.mTriggerTree.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg;
				switch (busType)
				{
				case BusType.Bt_LIN:
					arg = Vocabulary.LIN;
					break;
				case BusType.Bt_FlexRay:
					arg = Vocabulary.Flexray;
					break;
				default:
					arg = Vocabulary.CAN;
					break;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			if (!this.ApplicationDatabaseManager.SelectSignalInDatabase(ref text, ref text2, ref value, ref text3, ref text4, ref busType, ref value2))
			{
				return null;
			}
			string message;
			if (!this.mTriggerTree.ModelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, text4, text3, busType, out message))
			{
				InformMessageBox.Error(message);
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateSymbolicSignalTrigger();
			SymbolicSignalEvent symbolicSignalEvent = recordTrigger.Event as SymbolicSignalEvent;
			this.AssignCommonTriggerDefaults(ref recordTrigger);
			if (symbolicSignalEvent == null)
			{
				return null;
			}
			symbolicSignalEvent.MessageName.Value = text;
			symbolicSignalEvent.SignalName.Value = text2;
			symbolicSignalEvent.DatabaseName.Value = value;
			symbolicSignalEvent.DatabasePath.Value = this.mTriggerTree.ModelValidator.GetFilePathRelativeToConfiguration(text3);
			symbolicSignalEvent.NetworkName.Value = text4;
			symbolicSignalEvent.ChannelNumber.Value = 1u;
			IList<uint> channelAssignmentOfDatabase = this.mTriggerTree.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicSignalEvent.DatabasePath.Value, symbolicSignalEvent.NetworkName.Value);
			if (channelAssignmentOfDatabase.Count > 0)
			{
				symbolicSignalEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
				if (symbolicSignalEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
				{
					symbolicSignalEvent.ChannelNumber.Value = 1u;
					if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						symbolicSignalEvent.ChannelNumber.Value = 2u;
					}
				}
			}
			symbolicSignalEvent.BusType.Value = busType;
			symbolicSignalEvent.IsFlexrayPDU.Value = value2;
			symbolicSignalEvent.LowValue.Value = 0.0;
			symbolicSignalEvent.HighValue.Value = 0.0;
			symbolicSignalEvent.Relation.Value = CondRelation.Equal;
			RecordTrigger result;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.mTriggerTree.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSignalEvent);
				if (DialogResult.OK != symbolicSignalCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateMsgTimeoutTrigger(BusType bustype)
		{
			RecordTrigger result;
			using (MsgTimeoutCondition msgTimeoutCondition = new MsgTimeoutCondition(this.mTriggerTree.ModelValidator, this.ApplicationDatabaseManager))
			{
				msgTimeoutCondition.InitDefaultValues(bustype);
				if (DialogResult.OK != msgTimeoutCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateOnMessageTimeoutTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					MsgTimeoutEvent msgTimeoutEvent = recordTrigger.Event as MsgTimeoutEvent;
					if (msgTimeoutEvent != null)
					{
						msgTimeoutEvent.Assign(msgTimeoutCondition.MsgTimeoutEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateDigitalInputTrigger()
		{
			RecordTrigger recordTrigger = RecordTrigger.CreateDigitalInputTrigger();
			this.AssignCommonTriggerDefaults(ref recordTrigger);
			DigitalInputEvent digitalInputEvent = recordTrigger.Event as DigitalInputEvent;
			if (digitalInputEvent != null)
			{
				digitalInputEvent.Edge.Value = false;
				digitalInputEvent.DigitalInput.Value = 1u;
			}
			return recordTrigger;
		}

		private RecordTrigger CreateAnalogInputTrigger()
		{
			RecordTrigger result;
			using (AnalogInputCondition analogInputCondition = new AnalogInputCondition(this.mTriggerTree.ModelValidator))
			{
				analogInputCondition.ResetToDefaults();
				if (DialogResult.OK != analogInputCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateAnalogInputTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					AnalogInputEvent analogInputEvent = recordTrigger.Event as AnalogInputEvent;
					if (analogInputEvent != null)
					{
						analogInputEvent.Assign(analogInputCondition.AnalogInputEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateKeyTrigger()
		{
			RecordTrigger recordTrigger = RecordTrigger.CreateKeyTrigger();
			this.AssignCommonTriggerDefaults(ref recordTrigger);
			KeyEvent keyEvent = recordTrigger.Event as KeyEvent;
			if (keyEvent != null)
			{
				keyEvent.Number.Value = 1u;
				keyEvent.IsOnPanel.Value = false;
			}
			return recordTrigger;
		}

		private RecordTrigger CreateIgnitionTrigger()
		{
			RecordTrigger result = RecordTrigger.CreateIgnitionTrigger();
			this.AssignCommonTriggerDefaults(ref result);
			return result;
		}

		private RecordTrigger CreateVoCanRecordingTrigger()
		{
			RecordTrigger result;
			using (VoCanRecordingCondition voCanRecordingCondition = new VoCanRecordingCondition(this.mTriggerTree.ModelValidator))
			{
				voCanRecordingCondition.ResetToDefaults();
				if (DialogResult.OK != voCanRecordingCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateVoCanRecordingTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					VoCanRecordingEvent voCanRecordingEvent = recordTrigger.Event as VoCanRecordingEvent;
					if (voCanRecordingEvent != null)
					{
						voCanRecordingEvent.Assign(voCanRecordingCondition.VoCanRecordingEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateConditionGroupTrigger()
		{
			RecordTrigger result = RecordTrigger.CreateConditionGroupTrigger();
			this.AssignCommonTriggerDefaults(ref result);
			return result;
		}

		private RecordTrigger CreateCcpXcpSignalTrigger()
		{
			if (!CcpXcpManager.Instance().CheckTriggerSignalEvents())
			{
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateCcpXcpSignalTrigger();
			CcpXcpSignalEvent ccpXcpSignalEvent = recordTrigger.Event as CcpXcpSignalEvent;
			this.AssignCommonTriggerDefaults(ref recordTrigger);
			if (ccpXcpSignalEvent == null)
			{
				return null;
			}
			ccpXcpSignalEvent.SignalName.Value = string.Empty;
			ccpXcpSignalEvent.LowValue.Value = 0.0;
			ccpXcpSignalEvent.HighValue.Value = 0.0;
			ccpXcpSignalEvent.Relation.Value = CondRelation.Equal;
			ccpXcpSignalEvent.CcpXcpEcuName.Value = string.Empty;
			RecordTrigger result;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.mTriggerTree.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(ccpXcpSignalEvent);
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
				if (DialogResult.OK != symbolicSignalCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					ccpXcpSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateDiagnosticSignalTrigger()
		{
			if (!this.mTriggerTree.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddSigReq);
				return null;
			}
			if (!this.DiagnosticActionsConfiguration.DiagnosticActions.Any<DiagnosticAction>())
			{
				InformMessageBox.Error(Resources.ErrorNoDiagnosticSignalRequests);
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateDiagnosticSignalTrigger();
			DiagnosticSignalEvent diagnosticSignalEvent = recordTrigger.Event as DiagnosticSignalEvent;
			this.AssignCommonTriggerDefaults(ref recordTrigger);
			if (diagnosticSignalEvent == null)
			{
				return null;
			}
			diagnosticSignalEvent.SignalName.Value = string.Empty;
			diagnosticSignalEvent.LowValue.Value = 0.0;
			diagnosticSignalEvent.HighValue.Value = 0.0;
			diagnosticSignalEvent.Relation.Value = CondRelation.Equal;
			diagnosticSignalEvent.DiagnosticEcuName.Value = string.Empty;
			RecordTrigger result;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.mTriggerTree.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new DiagnosticSignalEvent(diagnosticSignalEvent);
				symbolicSignalCondition.DiagnosticActionsConfiguration = this.DiagnosticActionsConfiguration;
				symbolicSignalCondition.DiagnosticsDatabaseConfiguration = this.DiagnosticsDatabaseConfiguration;
				if (DialogResult.OK != symbolicSignalCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					diagnosticSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					result = recordTrigger;
				}
			}
			return result;
		}

		private RecordTrigger CreateIncEventTrigger()
		{
			if (!IncludeFileManager.Instance.OutParameters.Any((IncludeFileParameterPresenter outParam) => IncludeFileManager.Instance.IsValidOutParameterForTriggers(outParam)))
			{
				InformMessageBox.Info(Resources_Trigger.NoValidOutParametersAvailable);
				return null;
			}
			RecordTrigger result;
			using (IncludeParameterCondition includeParameterCondition = new IncludeParameterCondition())
			{
				if (DialogResult.OK != includeParameterCondition.ShowDialog())
				{
					result = null;
				}
				else
				{
					RecordTrigger recordTrigger = RecordTrigger.CreateIncEventTrigger();
					this.AssignCommonTriggerDefaults(ref recordTrigger);
					IncEvent incEvent = recordTrigger.Event as IncEvent;
					if (incEvent != null)
					{
						incEvent.Assign(includeParameterCondition.IncEvent);
					}
					result = recordTrigger;
				}
			}
			return result;
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			if ("combined event".Equals(item.Tag))
			{
				if (!this.mTriggerTree.CanAddCombinedEvent)
				{
					return false;
				}
				item.Text = (this.mTriggerTree.SelectedLevelImpliesConjunction ? Resources_Trigger.TriggerTypeNameColANDCondition : Resources_Trigger.TriggerTypeNameColORCondition);
				if (this.mSplitButtonEx.DefaultAction == Resources_Trigger.TriggerTypeNameColANDCondition || this.mSplitButtonEx.DefaultAction == Resources_Trigger.TriggerTypeNameColORCondition)
				{
					this.mSplitButtonEx.DefaultAction = item.Text;
				}
				return true;
			}
			else
			{
				if (text == Resources_Trigger.TriggerTypeNameColCANBusStatistics)
				{
					return this.mTriggerTree.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN) > 0u && this.mTriggerTree.ModelValidator.LoggerSpecifics.Type != LoggerType.VN1630log;
				}
				if (text == Resources_Trigger.TriggerTypeNameColCANData || text == Vocabulary.TriggerTypeNameColCANId || text == Resources_Trigger.TriggerTypeNameColCANMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicCAN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
				{
					return this.mTriggerTree.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN) > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColLINData || text == Vocabulary.TriggerTypeNameColLINId || text == Resources_Trigger.TriggerTypeNameColLINMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicLIN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
				{
					return this.mTriggerTree.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN) > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
				{
					return this.mTriggerTree.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay) > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColKey)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.IO.NumberOfKeys > 0u || this.mTriggerTree.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColAnalogInput)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColDigitalInput)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs > 0u;
				}
				if (text == Resources_Trigger.TriggerTypeNameColIgnition)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.IsIgnitionEventSupported;
				}
				if (text == Resources_Trigger.TriggerTypeNameColVoCanRecording)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.IsVoCANSupported && this.mTriggerTree.CanAddVoCANEvent;
				}
				if (text == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.IsCCPXCPSignalEventSupported;
				}
				if (text == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
				{
					return this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported;
				}
				return text == Resources_Trigger.TriggerTypeNameColIncEvent && this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.IsIncludeFilesSupported;
			}
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
			switch (this.mTriggerTree.NextAddAction)
			{
			case TriggerTree.AddAction.Condition:
				this.AddCondition(itemText);
				return;
			case TriggerTree.AddAction.Trigger:
				this.AddTrigger(itemText);
				return;
			case TriggerTree.AddAction.Marker:
				this.AddMarker(itemText);
				return;
			default:
				return;
			}
		}

		private void AddTrigger(string eventDisplayName)
		{
			List<RecordTrigger> list = this.CreateTrigger(eventDisplayName);
			if (list == null)
			{
				return;
			}
			foreach (RecordTrigger current in list)
			{
				current.Name.Value = this.mTriggerTree.CreateMarkerName(current);
				this.mTriggerTree.AddTrigger(current);
			}
		}

		private void AddMarker(string eventDisplayName)
		{
			List<RecordTrigger> list = this.CreateTrigger(eventDisplayName);
			if (list == null)
			{
				return;
			}
			foreach (RecordTrigger current in list)
			{
				current.TriggerEffect.Value = TriggerEffect.Marker;
				current.Action.Value = TriggerAction.None;
				current.Name.Value = this.mTriggerTree.CreateMarkerName(current);
				this.mTriggerTree.AddTrigger(current);
			}
		}

		private void AddCondition(string eventDisplayName)
		{
			List<RecordTrigger> list = this.CreateTrigger(eventDisplayName);
			if (list == null || !list.Any<RecordTrigger>())
			{
				return;
			}
			this.mTriggerTree.AddCondition(list[0].Event);
		}

		private void UpdateLabels()
		{
			switch (this.mTriggerTree.NextAddAction)
			{
			case TriggerTree.AddAction.Condition:
				this.mLabelAddEvent.Text = Resources_Trigger.TriggerPageAddCondition;
				break;
			case TriggerTree.AddAction.Trigger:
				this.mLabelAddEvent.Text = Resources_Trigger.TriggerPageAddTrigger;
				break;
			case TriggerTree.AddAction.Marker:
				this.mLabelAddEvent.Text = Resources_Trigger.TriggerPageAddMarker;
				break;
			}
			if (this.mTriggerTree != null && this.mTriggerTree.TriggerConfiguration != null)
			{
				switch (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value)
				{
				case TriggerMode.Triggered:
					this.mGroupBoxTriggers.Text = Vocabulary.TriggerPageGroupBoxTriggered;
					break;
				case TriggerMode.Permanent:
					this.mGroupBoxTriggers.Text = Vocabulary.TriggerPageGroupBoxPermanent;
					return;
				case TriggerMode.OnOff:
					this.mGroupBoxTriggers.Text = (this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport ? Resources_Trigger.TriggerPageGroupBoxOnOff : Resources_Trigger.TriggerPageGroupBoxOnOffGL10x0);
					return;
				default:
					return;
				}
			}
		}

		private void InitPossibleOperatingModes()
		{
			this.comboBoxOperatingMode.Items.Clear();
			foreach (RingBufferOperatingMode operatingMode in Enum.GetValues(typeof(RingBufferOperatingMode)))
			{
				this.comboBoxOperatingMode.Items.Add(GUIUtil.MapRingBufferOperatingMode2String(operatingMode));
			}
			if (this.comboBoxOperatingMode.Items.Count > 0)
			{
				this.comboBoxOperatingMode.SelectedIndex = 0;
			}
		}

		private void InitPossibleRingBufferSizes()
		{
			this.comboBoxRingBufferSize.Items.Clear();
			IList<uint> ringBufferSizes = GUIUtil.GetRingBufferSizes();
			foreach (uint current in ringBufferSizes)
			{
				if (current * 1024u >= this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.MinRingBufferSize && current * 1024u < this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.MaxRingBufferSize)
				{
					this.comboBoxRingBufferSize.Items.Add(current.ToString(CultureInfo.InvariantCulture));
				}
			}
			this.comboBoxRingBufferSize.Items.Add(GUIUtil.GetFractionedMByteDisplayString(this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.MaxRingBufferSize));
		}

		private void DisplayNumberOfFilesForCardSize()
		{
			string str = GUIUtil.MapMemoryCardSize2String((uint)RingBufferMemoriesManager.CurrentCardSizeMB);
			this.linkLabelNumOfFilesForCardSize.Text = Resources.NumOfFilesWhenUsingCapacity + str + ":";
			this.linkLabelNumOfFilesForCardSize.LinkArea = new LinkArea(Resources.NumOfFilesWhenUsingCapacity.Length, this.linkLabelNumOfFilesForCardSize.Text.Length - Resources.NumOfFilesWhenUsingCapacity.Length - 1);
			this.linkLabelNumOfFilesForCardSize.Visible = !this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly;
			this.labelNumOfFiles.Visible = !this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly;
			if (this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.IsActive.Value)
			{
				long num = Math.Min(RingBufferMemoriesManager.GetNumOfFilesForCurrentCardSize(this.MemoryNr), (long)((ulong)this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.MaxLoggerFiles));
				this.labelNumOfFiles.Text = num.ToString(CultureInfo.InvariantCulture);
				return;
			}
			this.labelNumOfFiles.Text = "";
		}

		private void DisplayMaxTotalMemorySize()
		{
			if (!this.mPageValidator.General.HasFormatError(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles) && !this.mPageValidator.General.HasFormatError(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size))
			{
				ulong num = (ulong)(this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles.Value * this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.Size.Value / 1024u);
				if (num < 1uL && this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles.Value > 0u)
				{
					num = 1uL;
				}
				this.labelTotalMemValue.Text = num.ToString();
				return;
			}
			this.labelTotalMemValue.Text = Resources.Unknown;
		}

		private void ShowOrHideIsMemoryActiveCheckBox()
		{
			bool flag = this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u;
			this.checkBoxIsActive.Visible = flag;
			if (flag)
			{
				this.checkBoxIsActive.Text = string.Format(Resources.UseMemWithRingBufferSize, this.mTriggerTree.TriggerConfiguration.MemoryNr);
			}
			this.labelUseRingBufferSize.Visible = !flag;
			this.panelRingBufferSize.Visible = (flag || !this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly);
			this.textBoxPreTriggerTime.Visible = this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly;
			this.labelPreTriggerTimeUnit.Visible = this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly;
		}

		private void EnableControlsForMemoryActivityAndLoggingMode()
		{
			bool flag = this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories <= 1u || this.checkBoxIsActive.Checked;
			this.labelUseRingBufferSize.Enabled = flag;
			this.comboBoxRingBufferSize.Enabled = flag;
			if (this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				this.labelMaxNumOfFiles.Visible = true;
				this.textBoxMaxNumOfFiles.Visible = true;
				this.labelTotal.Visible = true;
				this.labelTotalMemValue.Visible = true;
				this.labelMB2.Visible = true;
				this.labelMaxNumOfFiles.Enabled = flag;
				this.textBoxMaxNumOfFiles.Enabled = flag;
				this.labelTotal.Enabled = flag;
				this.labelTotalMemValue.Enabled = flag;
				this.labelMB2.Enabled = flag;
				this.labelClosingBracket.Enabled = flag;
			}
			else
			{
				this.labelMaxNumOfFiles.Visible = false;
				this.textBoxMaxNumOfFiles.Visible = false;
				this.labelTotal.Visible = false;
				this.labelTotalMemValue.Visible = false;
				this.labelMB2.Visible = false;
			}
			this.labelOperatingMode.Enabled = flag;
			this.comboBoxOperatingMode.Enabled = flag;
			this.labelOperatingModeExplanation.Enabled = flag;
			this.labelMB.Enabled = flag;
			this.mButtonEstimateRequiredRingbufferSize.Enabled = flag;
			this.linkLabelNumOfFilesForCardSize.Enabled = flag;
			this.radioButtonPermanentLogging.Enabled = flag;
			this.radioButtonTriggeredLogging.Enabled = flag;
			this.radioButtonConditionedOnOffLogging.Enabled = flag;
			this.mLinkLabelPreTriggerTime.Enabled = flag;
			this.labelPostTriggerTime.Enabled = flag;
			this.textBoxPostTriggerTime.Enabled = flag;
			if (flag)
			{
				bool hasMarkerSupport = this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport;
				bool enabled = !this.mTriggerTree.ModelValidator.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly;
				switch (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value)
				{
				case TriggerMode.Permanent:
					this.labelUseRingBufferSize.Enabled = enabled;
					this.comboBoxRingBufferSize.Enabled = enabled;
					this.labelMB.Enabled = enabled;
					this.mButtonEstimateRequiredRingbufferSize.Enabled = false;
					this.mLinkLabelPreTriggerTime.Enabled = false;
					this.textBoxPreTriggerTime.Enabled = false;
					this.labelPostTriggerTime.Enabled = false;
					this.textBoxPostTriggerTime.Enabled = false;
					flag = hasMarkerSupport;
					this.mTriggerTree.ShowColumnComment(false);
					this.mTriggerTree.ShowColumnName(hasMarkerSupport);
					break;
				case TriggerMode.OnOff:
					this.labelUseRingBufferSize.Enabled = enabled;
					this.comboBoxRingBufferSize.Enabled = enabled;
					this.labelMB.Enabled = enabled;
					this.mButtonEstimateRequiredRingbufferSize.Enabled = false;
					this.mLinkLabelPreTriggerTime.Enabled = false;
					this.textBoxPreTriggerTime.Enabled = false;
					this.labelPostTriggerTime.Enabled = false;
					this.textBoxPostTriggerTime.Enabled = false;
					this.mTriggerTree.ShowColumnComment(true);
					this.mTriggerTree.ShowColumnName(hasMarkerSupport);
					break;
				default:
					this.labelUseRingBufferSize.Enabled = true;
					this.comboBoxRingBufferSize.Enabled = true;
					this.labelMB.Enabled = true;
					this.mButtonEstimateRequiredRingbufferSize.Enabled = true;
					this.mLinkLabelPreTriggerTime.Enabled = true;
					this.textBoxPreTriggerTime.Enabled = true;
					this.labelPostTriggerTime.Enabled = true;
					this.textBoxPostTriggerTime.Enabled = true;
					this.mTriggerTree.ShowColumnComment(true);
					this.mTriggerTree.ShowColumnName(true);
					break;
				}
			}
			this.mSplitButton.Enabled = flag;
			this.buttonRemove.Enabled = (flag && this.mTriggerTree.CanRemoveSelection);
			this.mTriggerTree.Enabled = flag;
		}

		private void DisplayImageForTriggerMode()
		{
			if (this.radioButtonTriggeredLogging.Checked)
			{
				this.pictureBoxModeInfo.Image = Resources.ImageTriggerModeTriggered;
				return;
			}
			if (this.radioButtonConditionedOnOffLogging.Checked)
			{
				this.pictureBoxModeInfo.Image = Resources.ImageTriggerModeCondLongTerm;
				return;
			}
			if (this.radioButtonPermanentLogging.Checked)
			{
				if (this.mTriggerTree.ModelValidator.LoggerSpecifics.Recording.HasMarkerSupport)
				{
					this.pictureBoxModeInfo.Image = Resources.ImageTriggerModePermLongTerm;
					return;
				}
				this.pictureBoxModeInfo.Image = Resources.ImageTriggerModePermLongTerm_noMarker;
			}
		}

		private TriggerEffect GetDefaultTriggerEffectForCurrentModeAndMemoryNr()
		{
			if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value == TriggerMode.OnOff)
			{
				TriggerEffect result = TriggerEffect.LoggingOn;
				if (this.mTriggerTree.TriggerConfiguration.OnOffTriggers.Count <= 0)
				{
					return result;
				}
				foreach (RecordTrigger current in this.mTriggerTree.TriggerConfiguration.OnOffTriggers)
				{
					if (current.TriggerEffect.Value == TriggerEffect.LoggingOn)
					{
						result = TriggerEffect.LoggingOff;
						break;
					}
					if (current.TriggerEffect.Value == TriggerEffect.LoggingOff)
					{
						result = TriggerEffect.LoggingOn;
						break;
					}
				}
				return result;
			}
			else
			{
				if (this.mTriggerTree.TriggerConfiguration.TriggerMode.Value != TriggerMode.Permanent)
				{
					return TriggerEffect.Normal;
				}
				return TriggerEffect.Marker;
			}
		}

		private void DisplayOperatingModeLabelText()
		{
			string text = string.Empty;
			switch (this.mTriggerTree.TriggerConfiguration.MemoryRingBuffer.OperatingMode.Value)
			{
			case RingBufferOperatingMode.overwriteOldest:
				text = Resources.LabelMemoryOperatingModeOverwrite;
				break;
			case RingBufferOperatingMode.stopLogging:
				text = Resources.LabelMemoryOperatingModeStop;
				break;
			}
			this.labelOperatingModeExplanation.Text = text;
		}

		public bool Serialize(TriggersPage triggersPage)
		{
			if (triggersPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.mTriggerTree.Serialize(triggersPage);
		}

		public bool DeSerialize(TriggersPage triggersPage)
		{
			if (triggersPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.mTriggerTree.DeSerialize(triggersPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Triggers));
			this.radioButtonPermanentLogging = new RadioButton();
			this.radioButtonTriggeredLogging = new RadioButton();
			this.labelPostTriggerTime = new Label();
			this.textBoxPostTriggerTime = new TextBox();
			this.labelPostTriggerTimeUnit = new Label();
			this.groupBoxTriggerUsage = new GroupBox();
			this.pictureBoxModeInfo = new PictureBox();
			this.radioButtonConditionedOnOffLogging = new RadioButton();
			this.mSplitButton = new SplitButton();
			this.groupBoxTriggerTime = new GroupBox();
			this.labelPreTriggerTimeUnit = new Label();
			this.textBoxPreTriggerTime = new TextBox();
			this.mLinkLabelPreTriggerTime = new LinkLabel();
			this.mGroupBoxTriggers = new GroupBox();
			this.tableLayoutPanelGridOps = new TableLayoutPanel();
			this.mLabelAddEvent = new Label();
			this.buttonRemove = new Button();
			this.mTriggerTree = new TriggerTree();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxRingBufferSize = new ComboBox();
			this.checkBoxIsActive = new CheckBox();
			this.textBoxMaxNumOfFiles = new TextBox();
			this.comboBoxOperatingMode = new ComboBox();
			this.labelMB = new Label();
			this.labelUseRingBufferSize = new Label();
			this.linkLabelNumOfFilesForCardSize = new LinkLabel();
			this.labelNumOfFiles = new Label();
			this.mButtonEstimateRequiredRingbufferSize = new Button();
			this.tableLayoutPanelTriggers = new TableLayoutPanel();
			this.panelRingBufferSize = new Panel();
			this.labelClosingBracket = new Label();
			this.labelOperatingModeExplanation = new Label();
			this.labelOperatingMode = new Label();
			this.labelMB2 = new Label();
			this.labelTotalMemValue = new Label();
			this.labelTotal = new Label();
			this.labelMaxNumOfFiles = new Label();
			this.panelOptionsAndTriggers = new Panel();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxTriggerUsage.SuspendLayout();
			((ISupportInitialize)this.pictureBoxModeInfo).BeginInit();
			this.groupBoxTriggerTime.SuspendLayout();
			this.mGroupBoxTriggers.SuspendLayout();
			this.tableLayoutPanelGridOps.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.tableLayoutPanelTriggers.SuspendLayout();
			this.panelRingBufferSize.SuspendLayout();
			this.panelOptionsAndTriggers.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.radioButtonPermanentLogging, "radioButtonPermanentLogging");
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonPermanentLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonPermanentLogging.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonPermanentLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonPermanentLogging.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonPermanentLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonPermanentLogging.IconAlignment2"));
			this.radioButtonPermanentLogging.Name = "radioButtonPermanentLogging";
			this.radioButtonPermanentLogging.UseVisualStyleBackColor = true;
			this.radioButtonPermanentLogging.CheckedChanged += new EventHandler(this.radioButtonLoggingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonTriggeredLogging, "radioButtonTriggeredLogging");
			this.radioButtonTriggeredLogging.Checked = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonTriggeredLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonTriggeredLogging.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonTriggeredLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonTriggeredLogging.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonTriggeredLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonTriggeredLogging.IconAlignment2"));
			this.radioButtonTriggeredLogging.Name = "radioButtonTriggeredLogging";
			this.radioButtonTriggeredLogging.TabStop = true;
			this.radioButtonTriggeredLogging.UseVisualStyleBackColor = true;
			this.radioButtonTriggeredLogging.CheckedChanged += new EventHandler(this.radioButtonLoggingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelPostTriggerTime, "labelPostTriggerTime");
			this.labelPostTriggerTime.Name = "labelPostTriggerTime";
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPostTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPostTriggerTime.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxPostTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPostTriggerTime.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPostTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPostTriggerTime.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxPostTriggerTime, "textBoxPostTriggerTime");
			this.textBoxPostTriggerTime.Name = "textBoxPostTriggerTime";
			this.textBoxPostTriggerTime.Validating += new CancelEventHandler(this.textBoxPostTriggerTime_Validating);
			componentResourceManager.ApplyResources(this.labelPostTriggerTimeUnit, "labelPostTriggerTimeUnit");
			this.labelPostTriggerTimeUnit.Name = "labelPostTriggerTimeUnit";
			this.groupBoxTriggerUsage.Controls.Add(this.pictureBoxModeInfo);
			this.groupBoxTriggerUsage.Controls.Add(this.radioButtonConditionedOnOffLogging);
			this.groupBoxTriggerUsage.Controls.Add(this.radioButtonPermanentLogging);
			this.groupBoxTriggerUsage.Controls.Add(this.radioButtonTriggeredLogging);
			componentResourceManager.ApplyResources(this.groupBoxTriggerUsage, "groupBoxTriggerUsage");
			this.groupBoxTriggerUsage.Name = "groupBoxTriggerUsage";
			this.groupBoxTriggerUsage.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBoxModeInfo, "pictureBoxModeInfo");
			this.pictureBoxModeInfo.Name = "pictureBoxModeInfo";
			this.pictureBoxModeInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButtonConditionedOnOffLogging, "radioButtonConditionedOnOffLogging");
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonConditionedOnOffLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonConditionedOnOffLogging.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonConditionedOnOffLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonConditionedOnOffLogging.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonConditionedOnOffLogging, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonConditionedOnOffLogging.IconAlignment2"));
			this.radioButtonConditionedOnOffLogging.Name = "radioButtonConditionedOnOffLogging";
			this.radioButtonConditionedOnOffLogging.TabStop = true;
			this.radioButtonConditionedOnOffLogging.UseVisualStyleBackColor = true;
			this.radioButtonConditionedOnOffLogging.CheckedChanged += new EventHandler(this.radioButtonLoggingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.mSplitButton, "mSplitButton");
			this.mSplitButton.Name = "mSplitButton";
			this.mSplitButton.ShowSplitAlways = true;
			this.mSplitButton.UseVisualStyleBackColor = true;
			this.groupBoxTriggerTime.Controls.Add(this.labelPreTriggerTimeUnit);
			this.groupBoxTriggerTime.Controls.Add(this.textBoxPreTriggerTime);
			this.groupBoxTriggerTime.Controls.Add(this.mLinkLabelPreTriggerTime);
			this.groupBoxTriggerTime.Controls.Add(this.labelPostTriggerTime);
			this.groupBoxTriggerTime.Controls.Add(this.labelPostTriggerTimeUnit);
			this.groupBoxTriggerTime.Controls.Add(this.textBoxPostTriggerTime);
			componentResourceManager.ApplyResources(this.groupBoxTriggerTime, "groupBoxTriggerTime");
			this.groupBoxTriggerTime.Name = "groupBoxTriggerTime";
			this.groupBoxTriggerTime.TabStop = false;
			componentResourceManager.ApplyResources(this.labelPreTriggerTimeUnit, "labelPreTriggerTimeUnit");
			this.labelPreTriggerTimeUnit.Name = "labelPreTriggerTimeUnit";
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPreTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPreTriggerTime.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxPreTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPreTriggerTime.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPreTriggerTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPreTriggerTime.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxPreTriggerTime, "textBoxPreTriggerTime");
			this.textBoxPreTriggerTime.Name = "textBoxPreTriggerTime";
			this.textBoxPreTriggerTime.ReadOnly = true;
			componentResourceManager.ApplyResources(this.mLinkLabelPreTriggerTime, "mLinkLabelPreTriggerTime");
			this.mLinkLabelPreTriggerTime.Name = "mLinkLabelPreTriggerTime";
			this.mLinkLabelPreTriggerTime.TabStop = true;
			this.mLinkLabelPreTriggerTime.UseCompatibleTextRendering = true;
			this.mLinkLabelPreTriggerTime.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LinkLabelPreTriggerTime_LinkClicked);
			componentResourceManager.ApplyResources(this.mGroupBoxTriggers, "mGroupBoxTriggers");
			this.mGroupBoxTriggers.Controls.Add(this.tableLayoutPanelGridOps);
			this.mGroupBoxTriggers.Controls.Add(this.mTriggerTree);
			this.mGroupBoxTriggers.Name = "mGroupBoxTriggers";
			this.mGroupBoxTriggers.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelGridOps, "tableLayoutPanelGridOps");
			this.tableLayoutPanelGridOps.Controls.Add(this.mLabelAddEvent, 0, 0);
			this.tableLayoutPanelGridOps.Controls.Add(this.mSplitButton, 2, 0);
			this.tableLayoutPanelGridOps.Controls.Add(this.buttonRemove, 4, 0);
			this.tableLayoutPanelGridOps.Name = "tableLayoutPanelGridOps";
			componentResourceManager.ApplyResources(this.mLabelAddEvent, "mLabelAddEvent");
			this.mLabelAddEvent.Name = "mLabelAddEvent";
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Image = Resources.ImageDelete;
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.mTriggerTree, "mTriggerTree");
			this.mTriggerTree.ApplicationDatabaseManager = null;
			this.mTriggerTree.DiagnosticActionsConfiguration = null;
			this.mTriggerTree.DiagnosticsDatabaseConfiguration = null;
			this.mTriggerTree.DisplayMode = null;
			this.mTriggerTree.ModelEditor = null;
			this.mTriggerTree.ModelValidator = null;
			this.mTriggerTree.Name = "mTriggerTree";
			this.mTriggerTree.TriggerConfiguration = null;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.comboBoxRingBufferSize.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxRingBufferSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRingBufferSize.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxRingBufferSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRingBufferSize.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxRingBufferSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRingBufferSize.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxRingBufferSize, "comboBoxRingBufferSize");
			this.comboBoxRingBufferSize.Name = "comboBoxRingBufferSize";
			this.comboBoxRingBufferSize.SelectedIndexChanged += new EventHandler(this.comboBoxRingBufferSize_SelectedIndexChanged);
			this.comboBoxRingBufferSize.Validating += new CancelEventHandler(this.comboBoxRingBufferSize_Validating);
			componentResourceManager.ApplyResources(this.checkBoxIsActive, "checkBoxIsActive");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxIsActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsActive.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxIsActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsActive.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxIsActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsActive.IconAlignment2"));
			this.checkBoxIsActive.Name = "checkBoxIsActive";
			this.checkBoxIsActive.UseVisualStyleBackColor = true;
			this.checkBoxIsActive.CheckedChanged += new EventHandler(this.checkBoxIsActive_CheckedChanged);
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxMaxNumOfFiles, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMaxNumOfFiles.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMaxNumOfFiles, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMaxNumOfFiles.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMaxNumOfFiles, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMaxNumOfFiles.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxMaxNumOfFiles, "textBoxMaxNumOfFiles");
			this.textBoxMaxNumOfFiles.Name = "textBoxMaxNumOfFiles";
			this.textBoxMaxNumOfFiles.Validating += new CancelEventHandler(this.textBoxMaxNumOfFiles_Validating);
			this.comboBoxOperatingMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxOperatingMode.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxOperatingMode, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxOperatingMode.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxOperatingMode, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxOperatingMode.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxOperatingMode, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxOperatingMode.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxOperatingMode, "comboBoxOperatingMode");
			this.comboBoxOperatingMode.Name = "comboBoxOperatingMode";
			this.comboBoxOperatingMode.SelectedIndexChanged += new EventHandler(this.comboBoxOperatingMode_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelMB, "labelMB");
			this.labelMB.Name = "labelMB";
			componentResourceManager.ApplyResources(this.labelUseRingBufferSize, "labelUseRingBufferSize");
			this.labelUseRingBufferSize.Name = "labelUseRingBufferSize";
			componentResourceManager.ApplyResources(this.linkLabelNumOfFilesForCardSize, "linkLabelNumOfFilesForCardSize");
			this.linkLabelNumOfFilesForCardSize.Name = "linkLabelNumOfFilesForCardSize";
			this.linkLabelNumOfFilesForCardSize.TabStop = true;
			this.linkLabelNumOfFilesForCardSize.UseCompatibleTextRendering = true;
			this.linkLabelNumOfFilesForCardSize.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelNumOfFilesForCardSize_LinkClicked);
			componentResourceManager.ApplyResources(this.labelNumOfFiles, "labelNumOfFiles");
			this.labelNumOfFiles.Name = "labelNumOfFiles";
			componentResourceManager.ApplyResources(this.mButtonEstimateRequiredRingbufferSize, "mButtonEstimateRequiredRingbufferSize");
			this.mButtonEstimateRequiredRingbufferSize.Name = "mButtonEstimateRequiredRingbufferSize";
			this.mButtonEstimateRequiredRingbufferSize.UseVisualStyleBackColor = true;
			this.mButtonEstimateRequiredRingbufferSize.Click += new EventHandler(this.ButtonEstimateRequiredRingbufferSize_Click);
			componentResourceManager.ApplyResources(this.tableLayoutPanelTriggers, "tableLayoutPanelTriggers");
			this.tableLayoutPanelTriggers.Controls.Add(this.panelRingBufferSize, 0, 0);
			this.tableLayoutPanelTriggers.Controls.Add(this.panelOptionsAndTriggers, 0, 1);
			this.tableLayoutPanelTriggers.Name = "tableLayoutPanelTriggers";
			this.panelRingBufferSize.Controls.Add(this.labelNumOfFiles);
			this.panelRingBufferSize.Controls.Add(this.labelClosingBracket);
			this.panelRingBufferSize.Controls.Add(this.labelOperatingModeExplanation);
			this.panelRingBufferSize.Controls.Add(this.labelOperatingMode);
			this.panelRingBufferSize.Controls.Add(this.comboBoxOperatingMode);
			this.panelRingBufferSize.Controls.Add(this.labelMB2);
			this.panelRingBufferSize.Controls.Add(this.labelTotalMemValue);
			this.panelRingBufferSize.Controls.Add(this.labelTotal);
			this.panelRingBufferSize.Controls.Add(this.textBoxMaxNumOfFiles);
			this.panelRingBufferSize.Controls.Add(this.labelMaxNumOfFiles);
			this.panelRingBufferSize.Controls.Add(this.linkLabelNumOfFilesForCardSize);
			this.panelRingBufferSize.Controls.Add(this.labelUseRingBufferSize);
			this.panelRingBufferSize.Controls.Add(this.checkBoxIsActive);
			this.panelRingBufferSize.Controls.Add(this.mButtonEstimateRequiredRingbufferSize);
			this.panelRingBufferSize.Controls.Add(this.comboBoxRingBufferSize);
			this.panelRingBufferSize.Controls.Add(this.labelMB);
			componentResourceManager.ApplyResources(this.panelRingBufferSize, "panelRingBufferSize");
			this.panelRingBufferSize.Name = "panelRingBufferSize";
			componentResourceManager.ApplyResources(this.labelClosingBracket, "labelClosingBracket");
			this.labelClosingBracket.Name = "labelClosingBracket";
			componentResourceManager.ApplyResources(this.labelOperatingModeExplanation, "labelOperatingModeExplanation");
			this.labelOperatingModeExplanation.Name = "labelOperatingModeExplanation";
			componentResourceManager.ApplyResources(this.labelOperatingMode, "labelOperatingMode");
			this.labelOperatingMode.Name = "labelOperatingMode";
			componentResourceManager.ApplyResources(this.labelMB2, "labelMB2");
			this.labelMB2.Name = "labelMB2";
			componentResourceManager.ApplyResources(this.labelTotalMemValue, "labelTotalMemValue");
			this.labelTotalMemValue.Name = "labelTotalMemValue";
			componentResourceManager.ApplyResources(this.labelTotal, "labelTotal");
			this.labelTotal.Name = "labelTotal";
			componentResourceManager.ApplyResources(this.labelMaxNumOfFiles, "labelMaxNumOfFiles");
			this.labelMaxNumOfFiles.Name = "labelMaxNumOfFiles";
			this.panelOptionsAndTriggers.Controls.Add(this.groupBoxTriggerUsage);
			this.panelOptionsAndTriggers.Controls.Add(this.mGroupBoxTriggers);
			this.panelOptionsAndTriggers.Controls.Add(this.groupBoxTriggerTime);
			componentResourceManager.ApplyResources(this.panelOptionsAndTriggers, "panelOptionsAndTriggers");
			this.panelOptionsAndTriggers.Name = "panelOptionsAndTriggers";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.tableLayoutPanelTriggers);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "Triggers";
			this.groupBoxTriggerUsage.ResumeLayout(false);
			this.groupBoxTriggerUsage.PerformLayout();
			((ISupportInitialize)this.pictureBoxModeInfo).EndInit();
			this.groupBoxTriggerTime.ResumeLayout(false);
			this.groupBoxTriggerTime.PerformLayout();
			this.mGroupBoxTriggers.ResumeLayout(false);
			this.tableLayoutPanelGridOps.ResumeLayout(false);
			this.tableLayoutPanelGridOps.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.tableLayoutPanelTriggers.ResumeLayout(false);
			this.panelRingBufferSize.ResumeLayout(false);
			this.panelRingBufferSize.PerformLayout();
			this.panelOptionsAndTriggers.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
