#region global using directives

global using System;
global using System.Collections.Generic;
global using UnityEngine;
global using UnityModManagerNet;
global using UnityEngine.UI;
global using HarmonyLib;
global using Utilities = VanityCameraTweaks.Framework.Classes.Utilities;

#endregion

#region local using directives

using VanityCameraTweaks.Framework.Integrations.UMM;
using System.Diagnostics;

#endregion

namespace VanityCameraTweaks;

internal static class ModEntry
{
	internal static bool ModEnabledState { get; set; } = false;
	internal static Settings SettingsInstance { get; set; } = null!;
	internal static UnityModManager.ModEntry.ModLogger LoggerInstance { get; set; } = null!;
	internal static Harmony HarmonyInstance { get; set; } = null!;

	[Conditional("DEBUG")]
	internal static void DebugLog(string message)
	{
		LoggerInstance?.Log(message);
	}

	internal static void Log(string message)
	{
		LoggerInstance?.Log(message);
	}

	internal static void LogError(Exception exception)
	{
		LoggerInstance?.Log($"{exception.ToString()} \n {exception.StackTrace}");
	}

	internal static bool Load(UnityModManager.ModEntry modEntry)
	{
		try
		{
			modEntry.OnToggle = OnToggle;
			Initialise(modEntry);
		}
		catch(Exception exception)
		{
			LogError(exception);
			throw exception;
		}

		return true;
	}

	internal static void OnGUI(UnityModManager.ModEntry modEntry)
	{
		Builder.Build();
	}

	internal static void OnSaveGUI(UnityModManager.ModEntry modEntry)
	{
		SettingsInstance.Save(modEntry);
	}

	internal static void Initialise(UnityModManager.ModEntry modEntry)
	{
		LoggerInstance = modEntry.Logger;
		SettingsInstance = Settings.Load<Settings>(modEntry);
		HarmonyInstance = new Harmony(modEntry.Info.Id);
		HarmonyInstance.PatchAll();

		modEntry.OnSaveGUI = OnSaveGUI;
		modEntry.OnToggle = OnToggle;
		modEntry.OnGUI = OnGUI;
	}

	internal static bool OnToggle(UnityModManager.ModEntry modEntry, bool toggleState)
	{
		ModEnabledState = toggleState;
		return true;
	}
};