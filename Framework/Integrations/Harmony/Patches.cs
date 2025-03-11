#region using directives

using Kingmaker.UI.ServiceWindow;
using Kingmaker.View.Equipment;
using Kingmaker.View.Animation;
using Kingmaker.EntitySystem.Entities;

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

		if (!__instance.IsDollRoom)
		{
			return;
		}

		if (__result != WeaponAnimationStyle.Fist)
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

		if (!__instance.IsDollRoom)
		{
			return;
		}

		if (__result != WeaponAnimationStyle.Fist)
		{
			return;
		}

		__result = WeaponAnimationStyle.None;
	}

	[HarmonyPatch(typeof(DollRoom), "SetupInfo")]
	[HarmonyPostfix]
	internal static void TranslateDollRoomCamera(UnitEntityData player, DollRoom __instance)
	{
		if (!ModEntry.ModEnabledState)
		{
			return;
		}

		if (!Classes.Utilities.IsWithinSizeConstraints(player))
		{
			return;
		}

		DollCamera cameraInstance = Classes.Utilities.GetDollCamera(__instance);

		if (cameraInstance is null)
		{
			ModEntry.Log("Could not fetch a DollCamera instance from DollRoom, aborting execution.");
			return;
		}

		Vector3 newCoords = new Vector3
		(
			cameraInstance.transform.position.x,
			Classes.Utilities.GetCameraYBySize(player),
			ModEntry.SettingsInstance.CameraDistance
		);

		ModEntry.Log($"Got a CameraDistance of {ModEntry.SettingsInstance.CameraDistance}.");
		cameraInstance.transform.position = newCoords;
	}
};