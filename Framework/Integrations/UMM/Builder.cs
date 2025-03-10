#region using directives

using System.IO;
using System.Reflection;
using System.Text.Json;
using UnityModManagerNet;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal static class Builder
{
	private static Dictionary<string, string> SettingStrings { get; set; } = null!;

	internal static void Initialise(UnityModManager.ModEntry modEntry)
	{
		var jsonFile = File.ReadAllText("Framework/Strings/Settings.json");
		SettingStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonFile);
	}

	internal static PropertyInfo[] GetProperties(Settings settingInstance)
	{   // TODO: need to account for properties with the UMMIgnoreAttribute
		return settingInstance.GetType().GetProperties();
	}

	private static void AddFloatSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);
	}

	private static void AddBoolSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);
	}

	private static void AddIntSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);
	}

	private static void CreateSettings(Settings settingInstance)
	{
		PropertyInfo[] properties = GetProperties(settingInstance);

		if (properties.Length == 0)
		{
			return;
		}

		foreach(PropertyInfo property in properties)
		{
			// TODO: process each property by type, add the appropriate setting
		}
	}

	private static string GetSettingName(PropertyInfo property)
	{
		return SettingStrings[FormatPropertyName(property)];
	}

	private static string FormatPropertyName(PropertyInfo property)
	{   // TODO: are we really using camelCase to format our json dictionary keys?
		return JsonNamingPolicy.CamelCase.ConvertName(property.Name);
	}

	internal static void BuildSettings(UnityModManager.ModEntry modEntry, Settings settingInstance)
	{
		CreateSettings(settingInstance);
	}
};
