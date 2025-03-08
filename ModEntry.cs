# region global using directives

global using System;
global using System.Collections.Generic;
global using UnityEngine;
global using UnityModManagerNet;
global using UnityEngine.UI;
global using HarmonyLib;

#endregion

namespace VanityCameraTweaks;

internal static class ModEntry
{
	internal static bool ModEnabledState { get; set; } = false;
	internal static UnityModManager.ModEntry.ModLogger Logger { get; set; } = null!;
	internal static HarmonyLib.Harmony HarmonyInstance { get; set; } = null!;

	internal static void Log(string message)
	{
		Logger?.Log(message);
	}

	internal static void LogError(Exception exception)
	{
		Logger?.Log(string.Format("{0} \n {1}", exception.ToString(), exception.StackTrace));
	}

	static bool Load(UnityModManager.ModEntry modEntry)
	{
		try
		{
			modEntry.OnToggle = OnToggle;
			Logger = modEntry.Logger;
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

	internal static bool OnToggle(UnityModManager.ModEntry modEntry, bool toggleState)
	{
		ModEnabledState = toggleState;
		return true;
	}
};