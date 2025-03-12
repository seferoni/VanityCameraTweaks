﻿#region using directives

using Kingmaker.UI.ServiceWindow;
using Kingmaker.View.Equipment;
using Kingmaker.View.Animation;
using Kingmaker.EntitySystem.Entities;
using UnityEngine.EventSystems;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.Harmony;

[HarmonyPatch]
internal static class Patches
{
	[HarmonyPatch(typeof(UnitViewHandsEquipment), "get_ActiveMainHandWeaponStyle")]
	[HarmonyPostfix]
	internal static void RelaxUnarmedMainhandPosture(UnitViewHandsEquipment __instance, ref WeaponAnimationStyle __result)
	{
		if (!ModEntry.ModEnabledState)
		{
			return;
		}

		if (!ModEntry.SettingsInstance.ForceRelaxedPosture)
		{
			return;
		}

		if (!Utilities.IsWeaponAnimationViable(__result))
		{
			return;
		}

		if (__instance.ActiveOffHandWeaponStyle != WeaponAnimationStyle.None)
		{
			return;
		}

		__result = WeaponAnimationStyle.None;
	}

	[HarmonyPatch(typeof(UnitViewHandsEquipment), "get_ActiveOffHandWeaponStyle")]
	[HarmonyPostfix]
	internal static void RelaxUnarmedOffhandPosture(UnitViewHandsEquipment __instance, ref WeaponAnimationStyle __result)
	{
		if (!ModEntry.ModEnabledState)
		{
			return;
		}

		if (!ModEntry.SettingsInstance.ForceRelaxedPosture)
		{
			return;
		}

		if (!Utilities.IsWeaponAnimationViable(__result))
		{
			return;
		}

		__result = WeaponAnimationStyle.None;
	}

	[HarmonyPatch(typeof(DollRoom), "SetupInfo")]
	[HarmonyPostfix]
	internal static void TranslateDollRoomCamera(UnitEntityData player, DollRoom __instance)
	{
		if (Utilities.ExceedsSizeConstraints(player))
		{
			ModEntry.DebugLog("Doll model exceeds size stipulations, aborting execution.");
			return;
		}

		PatchData.DollInstance = player;
		DollCamera cameraInstance = Utilities.GetDollCamera(__instance);

		if (cameraInstance is null)
		{
			ModEntry.Log("Could not fetch a DollCamera instance from DollRoom, aborting execution.");
			return;
		}

		PatchData.ZoomScalar = 0f;
		PatchData.DollCameraInstance = cameraInstance;

		if (!ModEntry.ModEnabledState)
		{
			ModEntry.DebugLog("Mod is disabled, resetting camera transform.");
			cameraInstance.transform.position = PatchData.CameraDefaults;
			return;
		}

		cameraInstance.transform.position = Utilities.GetTranslatedCoordinatesByPlayer(player);
	}

	[HarmonyPatch(typeof(DollRoomCharacterController), "OnScroll")]
	[HarmonyPostfix]
	internal static void OnZoom(PointerEventData eventData)
	{
		if (!ModEntry.ModEnabledState)
		{
			return;
		}

		if (PatchData.DollInstance is null || PatchData.DollCameraInstance is null)
		{
			return;
		}

		PatchData.ZoomScalar += eventData.scrollDelta.y * 0.1f;
		PatchData.DollCameraInstance.transform.position = Utilities.InterpolateCameraCoordinatesByScalar(PatchData.ZoomScalar, PatchData.DollInstance);
	}
};