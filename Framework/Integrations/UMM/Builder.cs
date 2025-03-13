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
	private static float AbsoluteWidth { get; } = 300f;
	private static GUIStyle LabelStyle { get; set; } = null!;
	private static JObject SettingStrings { get; set; } = null!;

	private static void AddSetting(PropertyInfo property)
	{
		object setting = null;
		string settingName = GetSettingName(property);
		object settingValue = property.GetValue(ModEntry.SettingsInstance);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName);
			CreateSpacer();

			if (property.PropertyType == typeof(bool))
			{
				setting = GUILayout.Toggle((bool)settingValue, $"{settingValue}", GUILayout.Width(AbsoluteWidth));
			}
			else
			{
				UMMRangeAttribute range = GetRange(property);
				setting = GUILayout.HorizontalSlider((float)settingValue, range.Min, range.Max, GUILayout.Width(AbsoluteWidth));
				CreateLabel($"{(property.PropertyType == typeof(int) ? (int)settingValue : (float)settingValue):p0}");
			}

			property.SetValue(ModEntry.SettingsInstance, setting);
		}
		GUILayout.EndHorizontal();
		CreateDescriptionElement(property);
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

	private static void CreateLabel(string text)
	{
		GUILayout.Label(text, LabelStyle, GUILayout.ExpandWidth(false));
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
			AddSetting(property);
		}
	}

	private static void CreateSpacer(float pixels = 10)
	{
		GUILayout.Space(pixels);
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

	internal static void InitialiseStrings()
	{
		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Strings/Settings.json");
		SettingStrings = JObject.Parse(File.ReadAllText(path));
	}
};