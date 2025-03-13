#region using directives

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
	private static GUIStyle BoxStyle { get; set; } = null!;
	private static GUIStyle MajorElementStyle { get; set; } = null!;
	private static GUIStyle ToggleStyle { get; set; } = null!;
	private static JObject SettingStrings { get; set; } = null!;

	private static void AddSetting(PropertyInfo property)
	{
		object setting = null;
		string settingName = GetSettingName(property);
		object settingValue = property.GetValue(ModEntry.SettingsInstance);

		GUILayout.BeginHorizontal();
		{
			CreateLabel(settingName, MajorElementStyle);
			CreateSpacer();

			if (property.PropertyType == typeof(bool))
			{
				setting = GUILayout.Toggle((bool)settingValue, $"{settingValue}", ToggleStyle, GUILayout.Width(AbsoluteWidth));
			}
			else
			{
				UMMRangeAttribute range = GetRange(property);
				setting = GUILayout.HorizontalSlider((float)settingValue, range.Min, range.Max, GUILayout.Width(AbsoluteWidth));
				CreateLabel($"{(property.PropertyType == typeof(int) ? (int)settingValue : (float)settingValue):p0}", MajorElementStyle);
			}

			property.SetValue(ModEntry.SettingsInstance, setting);
		}
		GUILayout.EndHorizontal();
		CreateDescriptionElement(property);
	}

	internal static void Build()
	{
		InitialiseGUIStyles();
		InitialiseStrings();
		CreateSettings();
	}
	internal static void CreateDescriptionElement(PropertyInfo property)
	{
		string settingDescription = GetSettingDescription(property);
		GUILayout.BeginVertical();
		{
			CreateSpacer(5);
			GUILayout.Box(settingDescription, BoxStyle, GUILayout.ExpandWidth(false));
		}
		GUILayout.EndVertical();
	}

	private static void CreateLabel(string text, GUIStyle style = null)
	{
		if (style is null)
		{
			style = GUI.skin.label;
		}

		GUILayout.Label(text, style, GUILayout.ExpandWidth(false));
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

	internal static void InitialiseGUIStyles()
	{
		BoxStyle = new GUIStyle(GUI.skin.box)
		{
			fontSize = 13,
			wordWrap = true,
			padding = new RectOffset(5, 5, 5, 10),
			margin = new RectOffset(2, 2, 2, 2),
			normal = { textColor = Color.grey },
		};

		MajorElementStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 13,
			margin = new RectOffset(2, 2, 2, 2),
			normal = { textColor = Color.white }
		};

		ToggleStyle = new GUIStyle(GUI.skin.toggle)
		{
			fontSize = 13,
			normal = { textColor = Color.white }
		};
	}

	internal static void InitialiseStrings()
	{
		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Strings/Settings.json");
		SettingStrings = JObject.Parse(File.ReadAllText(path));
	}
};