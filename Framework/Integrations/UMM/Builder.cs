#region using directives

using Kingmaker.UI.SettingsUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using VanityCameraTweaks.Framework.Attributes;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal static class Builder
{
	private static JObject SettingStrings { get; set; } = null!;
	private static GUIStyle LabelStyle { get; set; } = null!;
	private static float AbsoluteWidth { get; } = 300f;

	internal static void InitialiseStrings()
	{
		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Strings/Settings.json");
		SettingStrings = JObject.Parse(File.ReadAllText(path));
	}

	internal static void CreateDescriptionElement(PropertyInfo property)
	{
		string settingDescription = GetSettingDescription(property);
		GUILayout.BeginVertical();
		{
			CreateSpacer(5);
			CreateLabel(settingDescription);
		}
		GUILayout.EndVertical();
	}

	internal static void Build()
	{
		LabelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 12,
			fontStyle = FontStyle.Bold,
			normal = { textColor = Color.grey }
		};
		InitialiseStrings();
		CreateSettings();
	}

	internal static PropertyInfo[] GetProperties()
	{
		var properties = ModEntry.SettingsInstance.GetType().GetProperties()
			.Where((property) => property.GetCustomAttribute<UMMIncludeAttribute>() is not null)
			.ToArray();
		return properties;
	}

	private static UMMRangeAttribute GetRange(PropertyInfo property)
	{
		return property.GetCustomAttribute<UMMRangeAttribute>()!;
	}

	private static void AddFloatSetting(PropertyInfo property)
	{
		string settingName = GetSettingName(property);
		UMMRangeAttribute range = GetRange(property);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();
			var sliderSetting = GUILayout.HorizontalSlider(
				(float)property.GetValue(ModEntry.SettingsInstance),
				range.Min,
				range.Max,
				GUILayout.Width(AbsoluteWidth)
			);
			property.SetValue(ModEntry.SettingsInstance, sliderSetting);
			CreateLabel($"{(float)property.GetValue(ModEntry.SettingsInstance):p0}");
		}
		GUILayout.EndHorizontal();
		CreateDescriptionElement(property);
	}

	private static void AddBoolSetting(PropertyInfo property)
	{
		string settingName = GetSettingName(property);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();
			bool value = (bool)property.GetValue(ModEntry.SettingsInstance);
			var toggleSetting = GUILayout.Toggle(
				value,
				$"{value}",
				GUILayout.Width(AbsoluteWidth)
			);
			property.SetValue(ModEntry.SettingsInstance, toggleSetting);
		}
		GUILayout.EndHorizontal();
		CreateDescriptionElement(property);
	}

	private static void CreateSpacer(float pixels = 10)
	{
		GUILayout.Space(pixels);
	}

	private static void CreateLabel(string text)
	{
		GUILayout.Label(text, LabelStyle, GUILayout.ExpandWidth(false));
	}

	private static void AddIntSetting(PropertyInfo property)
	{
		string settingName = GetSettingName(property);
		UMMRangeAttribute range = GetRange(property);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();
			var sliderSetting = GUILayout.HorizontalSlider(
				(int)property.GetValue(ModEntry.SettingsInstance),
				(int)range.Min,
				(int)range.Max,
				GUILayout.Width(AbsoluteWidth)
			);
			property.SetValue(ModEntry.SettingsInstance, sliderSetting);
			CreateLabel($"{(int)property.GetValue(ModEntry.SettingsInstance):p0}");
		}
		GUILayout.EndHorizontal();
		CreateDescriptionElement(property);
	}

	private static void CreateSettings()
	{
		PropertyInfo[] properties = GetProperties();

		if (properties.Length == 0)
		{
			return;
		}

		foreach (PropertyInfo property in properties)
		{
			switch (property.PropertyType.Name)
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
		return (string)SettingStrings[$"{FormatPropertyName(property)}Name"];
	}

	private static string GetSettingDescription(PropertyInfo property)
	{
		return (string)SettingStrings[$"{FormatPropertyName(property)}Description"];
	}

	private static string FormatPropertyName(PropertyInfo property)
	{
		return property.Name;
	}
};