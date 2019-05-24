using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public static class InformMessageBox
	{
		private static string sCaptionTitle;

		private static string sCaptionFormat;

		public static IWin32Window Owner
		{
			get;
			set;
		}

		public static string CaptionTitle
		{
			get
			{
				string arg_17_0;
				if ((arg_17_0 = InformMessageBox.sCaptionTitle) == null)
				{
					arg_17_0 = (InformMessageBox.sCaptionTitle = Vocabulary.VLConfigApplicationTitle);
				}
				return arg_17_0;
			}
			set
			{
				InformMessageBox.sCaptionTitle = value;
			}
		}

		private static string CaptionFormat
		{
			get
			{
				string arg_21_0;
				if ((arg_21_0 = InformMessageBox.sCaptionFormat) == null)
				{
					arg_21_0 = (InformMessageBox.sCaptionFormat = InformMessageBox.CaptionTitle + " - {0}");
				}
				return arg_21_0;
			}
		}

		public static void Info(string message)
		{
			InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.None, message);
		}

		public static void Info(string resString, params string[] args)
		{
			InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.None, resString, args);
		}

		public static void Warning(string message)
		{
			InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.None, message);
		}

		public static void Warning(string resString, params string[] args)
		{
			InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.None, resString, args);
		}

		public static void Error(string message)
		{
			InformMessageBox.Show(EnumInfoType.Error, EnumQuestionType.None, message);
		}

		public static void Error(string resString, params string[] args)
		{
			InformMessageBox.Show(EnumInfoType.Error, EnumQuestionType.None, resString, args);
		}

		public static DialogResult Question(string message)
		{
			return InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.Question, message);
		}

		public static DialogResult Question(string resString, params string[] args)
		{
			return InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.Question, resString, args);
		}

		public static DialogResult QuestionWithCancel(string message)
		{
			return InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.QuestionWithCancel, message);
		}

		public static DialogResult QuestionWithCancel(string resString, params string[] args)
		{
			return InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.QuestionWithCancel, resString, args);
		}

		[Conditional("DEBUG")]
		public static void DebugInfo(string message)
		{
			string captionTitle = InformMessageBox.CaptionTitle;
			InformMessageBox.CaptionTitle = "DEBUG INFO";
			InformMessageBox.Info(message);
			InformMessageBox.CaptionTitle = captionTitle;
		}

		public static void Show(EnumInfoType infoType, string message)
		{
			InformMessageBox.Show(infoType, EnumQuestionType.None, message);
		}

		public static void Show(EnumInfoType infoType, string resString, params string[] args)
		{
			InformMessageBox.Show(infoType, EnumQuestionType.None, string.Format(resString, args));
		}

		public static DialogResult Show(EnumQuestionType questionType, string message)
		{
			return InformMessageBox.Show(EnumInfoType.Info, questionType, message);
		}

		public static DialogResult Show(EnumQuestionType questionType, string resString, params string[] args)
		{
			return InformMessageBox.Show(EnumInfoType.Info, questionType, string.Format(resString, args));
		}

		public static DialogResult Show(EnumInfoType infoType, EnumQuestionType questionType, string resString, params string[] args)
		{
			return InformMessageBox.Show(infoType, questionType, string.Format(resString, args));
		}

		public static DialogResult Show(EnumInfoType infoType, EnumQuestionType questionType, string message)
		{
			string caption = InformMessageBox.CaptionTitle;
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Asterisk;
			MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;
			DialogResult dialogResult = DialogResult.OK;
			switch (questionType)
			{
			case EnumQuestionType.Question:
				buttons = MessageBoxButtons.YesNo;
				dialogResult = DialogResult.Yes;
				icon = MessageBoxIcon.Question;
				break;
			case EnumQuestionType.QuestionDefaultNo:
				buttons = MessageBoxButtons.YesNo;
				dialogResult = DialogResult.No;
				icon = MessageBoxIcon.Question;
				defaultButton = MessageBoxDefaultButton.Button2;
				break;
			case EnumQuestionType.QuestionWithCancel:
				buttons = MessageBoxButtons.YesNoCancel;
				dialogResult = DialogResult.Yes;
				icon = MessageBoxIcon.Question;
				break;
			}
			switch (infoType)
			{
			case EnumInfoType.Warning:
				caption = string.Format(InformMessageBox.CaptionFormat, Resources_General.Warning);
				icon = MessageBoxIcon.Exclamation;
				break;
			case EnumInfoType.Error:
				caption = string.Format(InformMessageBox.CaptionFormat, Resources_General.Error);
				icon = MessageBoxIcon.Hand;
				break;
			}
			DialogResult result;
			try
			{
				result = (AutomationMode.IsActive ? dialogResult : MessageBox.Show(InformMessageBox.Owner, message, caption, buttons, icon, defaultButton));
			}
			catch (InvalidOperationException)
			{
				if (InformMessageBox.Owner == null || !(InformMessageBox.Owner is Form))
				{
					result = DialogResult.None;
				}
				else
				{
					object obj = (InformMessageBox.Owner as Form).Invoke(new Func<EnumInfoType, EnumQuestionType, string, DialogResult>(InformMessageBox.Show), new object[]
					{
						infoType,
						questionType,
						message
					});
					try
					{
						result = (DialogResult)Enum.Parse(typeof(DialogResult), obj.ToString());
					}
					catch (Exception)
					{
						result = DialogResult.None;
					}
				}
			}
			return result;
		}
	}
}
