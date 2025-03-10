#region using directives

using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using UnityEngine;
using UnityModManagerNet;
using VanityCameraTweaks.Framework.Attributes;


#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal static class Builder
{
	private static Dictionary<string, string> SettingStrings { get; set; } = null!;
	private static GUIStyle LabelStyle { get; set; } = null!;
	private static float AbsoluteWidth { get; } = 300f;

	internal static void Initialise(UnityModManager.ModEntry modEntry)
	{
		var jsonFile = File.ReadAllText("Framework/Strings/Settings.json");
		SettingStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonFile);
		LabelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 14,
			fontStyle = FontStyle.Bold,
			normal = { textColor = Color.cyan }, // TODO: placeholder values
		};
	}

	internal static PropertyInfo[] GetProperties()
	{
		var properties = ModEntry.SettingsInstance.GetType().GetProperties()
			.Where((property) => property.GetCustomAttribute<UMMIgnoreAttribute>() is null)
			.ToArray();
		return properties;
	}

	private static UMMIntervalAttribute GetInterval(PropertyInfo property)
	{
		return property.GetCustomAttribute<UMMIntervalAttribute>()!;
	}

	private static UMMRangeAttribute GetRange(PropertyInfo property)
	{
		return property.GetCustomAttribute<UMMRangeAttribute>()!;
	}

	private static void AddFloatSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);
		UMMRangeAttribute range = GetRange(Property);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();
			var sliderSetting = GUILayout.HorizontalSlider(
				(float)Property.GetValue(ModEntry.SettingsInstance),
				range.Min,
				range.Max,
				GUILayout.Width(AbsoluteWidth)
			);
			Property.SetValue(ModEntry.SettingsInstance, sliderSetting);
			CreateLabel($"{(float)Property.GetValue(ModEntry.SettingsInstance):p0}");
		}
		GUILayout.EndHorizontal();
	}

	private static void AddBoolSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();
			bool value = (bool)Property.GetValue(ModEntry.SettingsInstance);
			var toggleSetting = GUILayout.Toggle(
				value,
				$"{value}",
				GUILayout.Width(AbsoluteWidth)
			);
			Property.SetValue(ModEntry.SettingsInstance, toggleSetting);
		}
		GUILayout.EndHorizontal();
	}

	private static void CreateSpacer(float pixels = 10)
	{
		GUILayout.Space(pixels);
	}

	private static void CreateLabel(string text)
	{
		GUILayout.Label(text, LabelStyle, GUILayout.ExpandWidth(false));
	}

	private static void AddIntSetting(PropertyInfo Property)
	{
		string settingName = GetSettingName(Property);
		UMMRangeAttribute range = GetRange(Property);

		GUILayout.BeginHorizontal();
		CreateLabel(settingName);
		CreateSpacer();
		var sliderSetting = GUILayout.HorizontalSlider(
			(int)Property.GetValue(ModEntry.SettingsInstance),
			(int)range.Min,
			(int)range.Max,
			GUILayout.Width(AbsoluteWidth)
		);
		Property.SetValue(ModEntry.SettingsInstance, sliderSetting);
		CreateLabel($"{(int)Property.GetValue(ModEntry.SettingsInstance):p0}");
		GUILayout.EndHorizontal();
	}

	private static void CreateSettings()
	{
		PropertyInfo[] properties = GetProperties();

		if (properties.Length == 0)
		{
			return;
		}

		foreach(PropertyInfo property in properties)
		{
			switch(property.PropertyType.Name)
			{
				case "Single":
					AddFloatSetting(property);
					break;
				case "Boolean":
					AddBoolSetting(property);
					break;
				case "Int32":
					AddIntSetting(property);
					break;
			}
		}
	}

	private static string GetSettingName(PropertyInfo property)
	{
		return SettingStrings[FormatPropertyName(property)];
	}

	private static string FormatPropertyName(PropertyInfo property)
	{
		return JsonNamingPolicy.CamelCase.ConvertName(property.Name);
	}

	internal static void BuildSettings()
	{
		CreateSettings();
	}
};
