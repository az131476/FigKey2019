using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Vector.VLConfig.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"), CompilerGenerated]
	public sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU1
		{
			get
			{
				return (string)this["LRU1"];
			}
			set
			{
				this["LRU1"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU2
		{
			get
			{
				return (string)this["LRU2"];
			}
			set
			{
				this["LRU2"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU3
		{
			get
			{
				return (string)this["LRU3"];
			}
			set
			{
				this["LRU3"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU4
		{
			get
			{
				return (string)this["LRU4"];
			}
			set
			{
				this["LRU4"] = value;
			}
		}

		[ApplicationScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
		public bool IsUSA
		{
			get
			{
				return (bool)this["IsUSA"];
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU5
		{
			get
			{
				return (string)this["LRU5"];
			}
			set
			{
				this["LRU5"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU6
		{
			get
			{
				return (string)this["LRU6"];
			}
			set
			{
				this["LRU6"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU7
		{
			get
			{
				return (string)this["LRU7"];
			}
			set
			{
				this["LRU7"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU8
		{
			get
			{
				return (string)this["LRU8"];
			}
			set
			{
				this["LRU8"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU9
		{
			get
			{
				return (string)this["LRU9"];
			}
			set
			{
				this["LRU9"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string LRU10
		{
			get
			{
				return (string)this["LRU10"];
			}
			set
			{
				this["LRU10"] = value;
			}
		}

		[ApplicationScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
		public bool IsGL1020FTEInitialLogger
		{
			get
			{
				return (bool)this["IsGL1020FTEInitialLogger"];
			}
		}

		private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
		{
		}

		private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
		{
		}
	}
}
