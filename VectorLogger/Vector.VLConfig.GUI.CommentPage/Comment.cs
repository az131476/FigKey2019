using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CommentPage
{
	public class Comment : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<CommentConfiguration>, IUpdateObserver
	{
		public delegate void DataChangedHandler(CommentConfiguration data);

		private VLProject vlProject;

		private CommentConfiguration commentConfiguration;

		private LoggerType loggerType;

		private string projectFileName;

		private string projectFilePath;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private GUIElementManager_Control guiElementManager;

		private IContainer components;

		private Label labelCommentLoggerType;

		private TextBox textBoxCommentLoggerType;

		private GroupBox groupBoxComment;

		private TextBox textBoxCommentConfig;

		private Label labelCommentConfig;

		private TextBox textBoxCommentVersion;

		private Label labelCommentVersion;

		private TextBox textBoxCommentName;

		private Label labelCommentName;

		private ErrorProvider errorProviderLocalModel;

		private CheckBox checkBoxCommentCopyToLTL;

		private TextBox textBoxCommentComment;

		private Label labelCommentComment;

		private Label labelCommentConfigPath;

		private TextBox textBoxCommentConfigPath;

		private ErrorProvider errorProviderGlobalModel;

		private ErrorProvider errorProviderFormat;

		public LoggerType LoggerType
		{
			get
			{
				return this.loggerType;
			}
			set
			{
				this.loggerType = value;
			}
		}

		public VLProject VLProject
		{
			get
			{
				return this.vlProject;
			}
			set
			{
				this.vlProject = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get;
			set;
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
				return PageType.Comment;
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
				base.Visible = value;
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public Comment()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is CommentConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.Comment);
			}
			this.LoggerType = ((IPropertyWindow)this).ModelValidator.LoggerSpecifics.Type;
			this.projectFileName = "";
			this.projectFilePath = "";
			this.commentConfiguration = null;
			this.RefreshView();
		}

		void IPropertyWindow.Reset()
		{
			this.ResetValidationFramework();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return this.ValidateInput();
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.LoggerType == data)
			{
				return;
			}
			this.LoggerType = data;
			this.RefreshView();
		}

		void IUpdateObserver<CommentConfiguration>.Update(CommentConfiguration data)
		{
			this.commentConfiguration = data;
			this.RefreshView();
			this.ValidateInput();
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

		public void RefreshView()
		{
			this.textBoxCommentLoggerType.Text = GUIUtil.MapLoggerType2String(this.LoggerType);
			this.checkBoxCommentCopyToLTL.Visible = (((IPropertyWindow)this).ModelValidator.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.LTL);
			this.projectFileName = this.vlProject.GetProjectFileName();
			this.textBoxCommentConfig.Text = this.projectFileName;
			if (string.IsNullOrEmpty(this.vlProject.FilePath))
			{
				this.projectFilePath = string.Empty;
			}
			else
			{
				try
				{
					this.projectFilePath = Path.GetDirectoryName(this.vlProject.FilePath);
				}
				catch (ArgumentException)
				{
					this.projectFilePath = string.Empty;
				}
			}
			this.textBoxCommentConfigPath.Text = this.projectFilePath;
			if (this.commentConfiguration != null)
			{
				if (!this.pageValidator.General.HasFormatError(this.commentConfiguration.Name))
				{
					this.textBoxCommentName.Text = this.commentConfiguration.Name.Value;
				}
				if (!this.pageValidator.General.HasFormatError(this.commentConfiguration.Version))
				{
					this.textBoxCommentVersion.Text = this.commentConfiguration.Version.Value;
				}
				if (!this.pageValidator.General.HasFormatError(this.commentConfiguration.Comment))
				{
					this.textBoxCommentComment.Text = this.commentConfiguration.Comment.Value;
				}
				if (!this.pageValidator.General.HasFormatError(this.commentConfiguration.CopyToLTL))
				{
					this.checkBoxCommentCopyToLTL.Checked = this.commentConfiguration.CopyToLTL.Value;
				}
			}
		}

		private bool ValidateInput()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = true;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			flag3 &= this.ValidateTextControl(this.textBoxCommentName.Text, this.commentConfiguration.Name, this.textBoxCommentName, out flag2, string.Format(Resources.InvalidCommentString, "Name"));
			flag |= flag2;
			flag3 &= this.ValidateTextControl(this.textBoxCommentVersion.Text, this.commentConfiguration.Version, this.textBoxCommentVersion, out flag2, string.Format(Resources.InvalidCommentString, "Version"));
			flag |= flag2;
			flag3 &= this.ValidateTextControl(this.textBoxCommentComment.Text, this.commentConfiguration.Comment, this.textBoxCommentComment, out flag2, string.Format(Resources.InvalidCommentString, "Comment"));
			flag |= flag2;
			flag3 &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxCommentCopyToLTL.Checked, this.commentConfiguration.CopyToLTL, this.guiElementManager.GetGUIElement(this.checkBoxCommentCopyToLTL), out flag2);
			flag |= flag2;
			((IPropertyWindow)this).ModelValidator.Validate(this.commentConfiguration, flag, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag3;
		}

		private bool ValidateTextControl(string value, ValidatedProperty<string> targetProperty, TextBox control, out bool valueChanged, string errorMessage)
		{
			bool result = this.pageValidator.Control.UpdateModel<string>(value, targetProperty, this.guiElementManager.GetGUIElement(control), out valueChanged);
			if (value.IndexOfAny("{}".ToCharArray()) >= 0)
			{
				this.pageValidator.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, targetProperty, errorMessage);
				result = false;
			}
			return result;
		}

		private void ValidatingControl(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void checkBoxCommentCopyToLTL_CheckedChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void checkBoxCommentCopyToLTL_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox != null && checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Comment));
			this.labelCommentLoggerType = new Label();
			this.textBoxCommentLoggerType = new TextBox();
			this.groupBoxComment = new GroupBox();
			this.textBoxCommentConfigPath = new TextBox();
			this.labelCommentConfigPath = new Label();
			this.checkBoxCommentCopyToLTL = new CheckBox();
			this.textBoxCommentComment = new TextBox();
			this.labelCommentComment = new Label();
			this.textBoxCommentVersion = new TextBox();
			this.labelCommentVersion = new Label();
			this.textBoxCommentName = new TextBox();
			this.labelCommentName = new Label();
			this.textBoxCommentConfig = new TextBox();
			this.labelCommentConfig = new Label();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.groupBoxComment.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelCommentLoggerType, "labelCommentLoggerType");
			this.errorProviderFormat.SetError(this.labelCommentLoggerType, componentResourceManager.GetString("labelCommentLoggerType.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentLoggerType, componentResourceManager.GetString("labelCommentLoggerType.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentLoggerType, componentResourceManager.GetString("labelCommentLoggerType.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentLoggerType.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentLoggerType.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentLoggerType.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentLoggerType, (int)componentResourceManager.GetObject("labelCommentLoggerType.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentLoggerType, (int)componentResourceManager.GetObject("labelCommentLoggerType.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentLoggerType, (int)componentResourceManager.GetObject("labelCommentLoggerType.IconPadding2"));
			this.labelCommentLoggerType.Name = "labelCommentLoggerType";
			componentResourceManager.ApplyResources(this.textBoxCommentLoggerType, "textBoxCommentLoggerType");
			this.errorProviderFormat.SetError(this.textBoxCommentLoggerType, componentResourceManager.GetString("textBoxCommentLoggerType.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentLoggerType, componentResourceManager.GetString("textBoxCommentLoggerType.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentLoggerType, componentResourceManager.GetString("textBoxCommentLoggerType.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentLoggerType.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentLoggerType.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentLoggerType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentLoggerType.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentLoggerType, (int)componentResourceManager.GetObject("textBoxCommentLoggerType.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentLoggerType, (int)componentResourceManager.GetObject("textBoxCommentLoggerType.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentLoggerType, (int)componentResourceManager.GetObject("textBoxCommentLoggerType.IconPadding2"));
			this.textBoxCommentLoggerType.Name = "textBoxCommentLoggerType";
			this.textBoxCommentLoggerType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.groupBoxComment, "groupBoxComment");
			this.groupBoxComment.Controls.Add(this.textBoxCommentConfigPath);
			this.groupBoxComment.Controls.Add(this.labelCommentConfigPath);
			this.groupBoxComment.Controls.Add(this.checkBoxCommentCopyToLTL);
			this.groupBoxComment.Controls.Add(this.textBoxCommentComment);
			this.groupBoxComment.Controls.Add(this.labelCommentComment);
			this.groupBoxComment.Controls.Add(this.textBoxCommentVersion);
			this.groupBoxComment.Controls.Add(this.labelCommentVersion);
			this.groupBoxComment.Controls.Add(this.textBoxCommentName);
			this.groupBoxComment.Controls.Add(this.labelCommentName);
			this.groupBoxComment.Controls.Add(this.textBoxCommentConfig);
			this.groupBoxComment.Controls.Add(this.labelCommentConfig);
			this.groupBoxComment.Controls.Add(this.textBoxCommentLoggerType);
			this.groupBoxComment.Controls.Add(this.labelCommentLoggerType);
			this.errorProviderLocalModel.SetError(this.groupBoxComment, componentResourceManager.GetString("groupBoxComment.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxComment, componentResourceManager.GetString("groupBoxComment.Error1"));
			this.errorProviderFormat.SetError(this.groupBoxComment, componentResourceManager.GetString("groupBoxComment.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxComment, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxComment.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxComment, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxComment.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxComment, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxComment.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxComment, (int)componentResourceManager.GetObject("groupBoxComment.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxComment, (int)componentResourceManager.GetObject("groupBoxComment.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxComment, (int)componentResourceManager.GetObject("groupBoxComment.IconPadding2"));
			this.groupBoxComment.Name = "groupBoxComment";
			this.groupBoxComment.TabStop = false;
			componentResourceManager.ApplyResources(this.textBoxCommentConfigPath, "textBoxCommentConfigPath");
			this.errorProviderFormat.SetError(this.textBoxCommentConfigPath, componentResourceManager.GetString("textBoxCommentConfigPath.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentConfigPath, componentResourceManager.GetString("textBoxCommentConfigPath.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentConfigPath, componentResourceManager.GetString("textBoxCommentConfigPath.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfigPath.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfigPath.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfigPath.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentConfigPath, (int)componentResourceManager.GetObject("textBoxCommentConfigPath.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentConfigPath, (int)componentResourceManager.GetObject("textBoxCommentConfigPath.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentConfigPath, (int)componentResourceManager.GetObject("textBoxCommentConfigPath.IconPadding2"));
			this.textBoxCommentConfigPath.Name = "textBoxCommentConfigPath";
			this.textBoxCommentConfigPath.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelCommentConfigPath, "labelCommentConfigPath");
			this.errorProviderFormat.SetError(this.labelCommentConfigPath, componentResourceManager.GetString("labelCommentConfigPath.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentConfigPath, componentResourceManager.GetString("labelCommentConfigPath.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentConfigPath, componentResourceManager.GetString("labelCommentConfigPath.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfigPath.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfigPath.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentConfigPath, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfigPath.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentConfigPath, (int)componentResourceManager.GetObject("labelCommentConfigPath.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentConfigPath, (int)componentResourceManager.GetObject("labelCommentConfigPath.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentConfigPath, (int)componentResourceManager.GetObject("labelCommentConfigPath.IconPadding2"));
			this.labelCommentConfigPath.Name = "labelCommentConfigPath";
			componentResourceManager.ApplyResources(this.checkBoxCommentCopyToLTL, "checkBoxCommentCopyToLTL");
			this.errorProviderGlobalModel.SetError(this.checkBoxCommentCopyToLTL, componentResourceManager.GetString("checkBoxCommentCopyToLTL.Error"));
			this.errorProviderLocalModel.SetError(this.checkBoxCommentCopyToLTL, componentResourceManager.GetString("checkBoxCommentCopyToLTL.Error1"));
			this.errorProviderFormat.SetError(this.checkBoxCommentCopyToLTL, componentResourceManager.GetString("checkBoxCommentCopyToLTL.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCommentCopyToLTL, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCommentCopyToLTL, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCommentCopyToLTL, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxCommentCopyToLTL, (int)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxCommentCopyToLTL, (int)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxCommentCopyToLTL, (int)componentResourceManager.GetObject("checkBoxCommentCopyToLTL.IconPadding2"));
			this.checkBoxCommentCopyToLTL.Name = "checkBoxCommentCopyToLTL";
			this.checkBoxCommentCopyToLTL.UseVisualStyleBackColor = true;
			this.checkBoxCommentCopyToLTL.CheckedChanged += new EventHandler(this.checkBoxCommentCopyToLTL_CheckedChanged);
			this.checkBoxCommentCopyToLTL.Paint += new PaintEventHandler(this.checkBoxCommentCopyToLTL_Paint);
			this.checkBoxCommentCopyToLTL.Validating += new CancelEventHandler(this.ValidatingControl);
			componentResourceManager.ApplyResources(this.textBoxCommentComment, "textBoxCommentComment");
			this.errorProviderFormat.SetError(this.textBoxCommentComment, componentResourceManager.GetString("textBoxCommentComment.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentComment, componentResourceManager.GetString("textBoxCommentComment.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentComment, componentResourceManager.GetString("textBoxCommentComment.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentComment.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentComment.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentComment.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentComment, (int)componentResourceManager.GetObject("textBoxCommentComment.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentComment, (int)componentResourceManager.GetObject("textBoxCommentComment.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentComment, (int)componentResourceManager.GetObject("textBoxCommentComment.IconPadding2"));
			this.textBoxCommentComment.Name = "textBoxCommentComment";
			this.textBoxCommentComment.Validating += new CancelEventHandler(this.ValidatingControl);
			componentResourceManager.ApplyResources(this.labelCommentComment, "labelCommentComment");
			this.errorProviderFormat.SetError(this.labelCommentComment, componentResourceManager.GetString("labelCommentComment.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentComment, componentResourceManager.GetString("labelCommentComment.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentComment, componentResourceManager.GetString("labelCommentComment.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentComment.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentComment.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentComment, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentComment.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentComment, (int)componentResourceManager.GetObject("labelCommentComment.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentComment, (int)componentResourceManager.GetObject("labelCommentComment.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentComment, (int)componentResourceManager.GetObject("labelCommentComment.IconPadding2"));
			this.labelCommentComment.Name = "labelCommentComment";
			componentResourceManager.ApplyResources(this.textBoxCommentVersion, "textBoxCommentVersion");
			this.errorProviderFormat.SetError(this.textBoxCommentVersion, componentResourceManager.GetString("textBoxCommentVersion.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentVersion, componentResourceManager.GetString("textBoxCommentVersion.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentVersion, componentResourceManager.GetString("textBoxCommentVersion.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentVersion.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentVersion.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentVersion.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentVersion, (int)componentResourceManager.GetObject("textBoxCommentVersion.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentVersion, (int)componentResourceManager.GetObject("textBoxCommentVersion.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentVersion, (int)componentResourceManager.GetObject("textBoxCommentVersion.IconPadding2"));
			this.textBoxCommentVersion.Name = "textBoxCommentVersion";
			this.textBoxCommentVersion.Validating += new CancelEventHandler(this.ValidatingControl);
			componentResourceManager.ApplyResources(this.labelCommentVersion, "labelCommentVersion");
			this.errorProviderFormat.SetError(this.labelCommentVersion, componentResourceManager.GetString("labelCommentVersion.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentVersion, componentResourceManager.GetString("labelCommentVersion.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentVersion, componentResourceManager.GetString("labelCommentVersion.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentVersion.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentVersion.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentVersion, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentVersion.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentVersion, (int)componentResourceManager.GetObject("labelCommentVersion.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentVersion, (int)componentResourceManager.GetObject("labelCommentVersion.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentVersion, (int)componentResourceManager.GetObject("labelCommentVersion.IconPadding2"));
			this.labelCommentVersion.Name = "labelCommentVersion";
			componentResourceManager.ApplyResources(this.textBoxCommentName, "textBoxCommentName");
			this.errorProviderFormat.SetError(this.textBoxCommentName, componentResourceManager.GetString("textBoxCommentName.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentName, componentResourceManager.GetString("textBoxCommentName.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentName, componentResourceManager.GetString("textBoxCommentName.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentName.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentName.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentName.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentName, (int)componentResourceManager.GetObject("textBoxCommentName.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentName, (int)componentResourceManager.GetObject("textBoxCommentName.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentName, (int)componentResourceManager.GetObject("textBoxCommentName.IconPadding2"));
			this.textBoxCommentName.Name = "textBoxCommentName";
			this.textBoxCommentName.Validating += new CancelEventHandler(this.ValidatingControl);
			componentResourceManager.ApplyResources(this.labelCommentName, "labelCommentName");
			this.errorProviderFormat.SetError(this.labelCommentName, componentResourceManager.GetString("labelCommentName.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentName, componentResourceManager.GetString("labelCommentName.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentName, componentResourceManager.GetString("labelCommentName.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentName.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentName.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentName, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentName.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentName, (int)componentResourceManager.GetObject("labelCommentName.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentName, (int)componentResourceManager.GetObject("labelCommentName.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentName, (int)componentResourceManager.GetObject("labelCommentName.IconPadding2"));
			this.labelCommentName.Name = "labelCommentName";
			componentResourceManager.ApplyResources(this.textBoxCommentConfig, "textBoxCommentConfig");
			this.errorProviderFormat.SetError(this.textBoxCommentConfig, componentResourceManager.GetString("textBoxCommentConfig.Error"));
			this.errorProviderLocalModel.SetError(this.textBoxCommentConfig, componentResourceManager.GetString("textBoxCommentConfig.Error1"));
			this.errorProviderGlobalModel.SetError(this.textBoxCommentConfig, componentResourceManager.GetString("textBoxCommentConfig.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfig.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfig.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCommentConfig.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCommentConfig, (int)componentResourceManager.GetObject("textBoxCommentConfig.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCommentConfig, (int)componentResourceManager.GetObject("textBoxCommentConfig.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCommentConfig, (int)componentResourceManager.GetObject("textBoxCommentConfig.IconPadding2"));
			this.textBoxCommentConfig.Name = "textBoxCommentConfig";
			this.textBoxCommentConfig.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelCommentConfig, "labelCommentConfig");
			this.errorProviderFormat.SetError(this.labelCommentConfig, componentResourceManager.GetString("labelCommentConfig.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCommentConfig, componentResourceManager.GetString("labelCommentConfig.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCommentConfig, componentResourceManager.GetString("labelCommentConfig.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.labelCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfig.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfig.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCommentConfig, (ErrorIconAlignment)componentResourceManager.GetObject("labelCommentConfig.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCommentConfig, (int)componentResourceManager.GetObject("labelCommentConfig.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCommentConfig, (int)componentResourceManager.GetObject("labelCommentConfig.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCommentConfig, (int)componentResourceManager.GetObject("labelCommentConfig.IconPadding2"));
			this.labelCommentConfig.Name = "labelCommentConfig";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxComment);
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "Comment";
			this.groupBoxComment.ResumeLayout(false);
			this.groupBoxComment.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			base.ResumeLayout(false);
		}
	}
}
