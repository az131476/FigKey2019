using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Vector.Legal.General;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig
{
	public static class ProgramUtils
	{
		private static CultureInfo culture;

		public static CultureInfo Culture
		{
			get
			{
				if (ProgramUtils.culture == null)
				{
					ProgramUtils.culture = new CultureInfo("en-US");
				}
				return ProgramUtils.culture;
			}
		}

		public static bool IsBeta
		{
			get
			{
				return AssemblyAttributeBetaVersion.IsBeta();
			}
		}

		public static void SetCulture(GlobalOptions.Languages language)
		{
			if (language == GlobalOptions.Languages.German)
			{
				CultureInfo currentUICulture = new CultureInfo("de");
				Thread.CurrentThread.CurrentUICulture = currentUICulture;
			}
			else if (language == GlobalOptions.Languages.English)
			{
				CultureInfo currentUICulture2 = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = currentUICulture2;
			}
			ProgramUtils.culture = Thread.CurrentThread.CurrentUICulture;
		}

		public static void ApplyProgramUICulture()
		{
			Thread.CurrentThread.CurrentUICulture = ProgramUtils.culture;
		}

		public static GlobalOptions.Languages GetCurrCultureLanguage()
		{
			if (Thread.CurrentThread.CurrentUICulture.Name == "de" || Thread.CurrentThread.CurrentUICulture.Name == "de-DE")
			{
				return GlobalOptions.Languages.German;
			}
			return GlobalOptions.Languages.English;
		}

		public static int GetCurrCultureLanguageCode()
		{
			if (ProgramUtils.GetCurrCultureLanguage() == GlobalOptions.Languages.German)
			{
				return 49;
			}
			return 1;
		}

		public static bool DisplayDisclaimer()
		{
			int language = 1;
			if (Thread.CurrentThread.CurrentUICulture.Name == "de-DE" || Thread.CurrentThread.CurrentUICulture.Name == "de")
			{
				language = 49;
			}
			int num = ProgramUtils.DisclaimerId();
			int num2 = Disclaimer.DisplayTheVersion(language);
			if (num != num2)
			{
				InformMessageBox.Info("Invalid license agreement DLL!");
				return false;
			}
			int strictness = 0;
			if (Settings.Default.IsUSA)
			{
				strictness = 1;
			}
			int betaVersion = 0;
			if (ProgramUtils.IsBeta)
			{
				betaVersion = 1;
			}
			return Disclaimer.ShowTheDisclaimer(language, strictness, betaVersion, 0) != 2;
		}

		public static int DisclaimerId()
		{
			return (DateTime.UtcNow.Day + DateTime.UtcNow.Hour + 11) * (DateTime.UtcNow.Hour + 49);
		}

		public static bool CheckParametersForGLC(string[] args)
		{
			if (args != null && args.Count<string>() > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i];
					if (text.ToLower().EndsWith(Vocabulary.FileExtensionDotGLC))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckParametersForAllowMultipleInstances(string[] args)
		{
			return args != null && args.Count<string>() > 0 && (args.Contains("-AMI") || args.Contains("-AllowMultipleInstances"));
		}
	}
}
