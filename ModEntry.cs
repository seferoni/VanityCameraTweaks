# region global using directives

global using System;
global using System.Collections.Generic;
global using UnityEngine;
global using UnityModManagerNet;
global using UnityEngine.UI;
global using HarmonyLib;
using VanityCameraTweaks.Classes;

#endregion

namespace VanityCameraTweaks;

internal static class ModEntry
{
	internal static bool ModEnabledState { get; set; } = false;
	internal static Settings SettingsInstance { get; set; } = null!;
	internal static UnityModManager.ModEntry.ModLogger LoggerInstance { get; set; } = null!;
	internal static HarmonyLib.Harmony HarmonyInstance { get; set; } = null!;

	internal static void Log(string message)
	{
		LoggerInstance?.Log(message);
	}

	internal static void LogError(Exception exception)
	{
		LoggerInstance?.Log(string.Format("{0} \n {1}", exception.ToString(), exception.StackTrace));
	}

	internal static bool Load(UnityModManager.ModEntry modEntry)
	{
		try
		{
			modEntry.OnToggle = OnToggle;
			LoggerInstance = modEntry.Logger;
			SettingsInstance = Settings.Load<Settings>(modEntry);
			HarmonyInstance = new HarmonyLib.Harmony(modEntry.Info.Id);
			HarmonyInstance.PatchAll();
		}
		catch(Exception exception)
		{
			LogError(exception);
			throw (exception);
		}
		
		return true;
	}

	static void OnGUI(UnityModManager.ModEntry modEntry)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("CameraDistance", GUILayout.ExpandWidth(false));
		GUILayout.Space(10);
		SettingsInstance.CameraDistance = GUILayout.HorizontalSlider(SettingsInstance.CameraDistance, 1f, 10f, GUILayout.Width(300f));
		GUILayout.Label($" {SettingsInstance.CameraDistance:p0}", GUILayout.ExpandWidth(false));
		GUILayout.EndHorizontal();
	}

	internal static bool OnToggle(UnityModManager.ModEntry modEntry, bool toggleState)
	{
		ModEnabledState = toggleState;
		return true;
	}
};