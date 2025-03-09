#region using directives

using System.IO;
using System.Text.Json;
using UnityModManagerNet;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal static class Builder
{
	internal static Dictionary<string, string> SettingStrings { get; set; } = null!;

	internal static void Initialise(UnityModManager.ModEntry modEntry)
	{
		var jsonFile = File.ReadAllText("Framework/Strings/Settings.json");
		SettingStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonFile);
	}

	internal static void BuildSettings(UnityModManager.ModEntry modEntry)
	{
		
	}
};
