using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace dbxTweaker.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0"), CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}
		[DefaultSettingValue("C:\\"), UserScopedSetting, DebuggerNonUserCode]
		public string UnpatchedCasDirectory
		{
			get
			{
				return (string)this["UnpatchedCasDirectory"];
			}
			set
			{
				this["UnpatchedCasDirectory"] = value;
			}
		}
		[DefaultSettingValue("C:\\"), UserScopedSetting, DebuggerNonUserCode]
		public string PatchedCasDirectory
		{
			get
			{
				return (string)this["PatchedCasDirectory"];
			}
			set
			{
				this["PatchedCasDirectory"] = value;
			}
		}
		[DefaultSettingValue("C:\\"), UserScopedSetting, DebuggerNonUserCode]
		public string lastDirectory
		{
			get
			{
				return (string)this["lastDirectory"];
			}
			set
			{
				this["lastDirectory"] = value;
			}
		}
	}
}
