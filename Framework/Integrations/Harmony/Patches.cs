#region using directives

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
	private static float zoomScalar = 0f;

	internal static float ZoomScalar
	{
		get => zoomScalar;
		set => zoomScalar = Mathf.Clamp(value, 0f, 1f);
	}
	internal static DollCamera DollCameraInstance { get; set; } = null!;
	internal static UnitEntityData DollInstance { get; set; } = null!;


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

		if (!Classes.Utilities.IsWeaponAnimationViable(__result))
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

		if (!Classes.Utilities.IsWeaponAnimationViable(__result))
		{
			return;
		}

		__result = WeaponAnimationStyle.None;
	}

	[HarmonyPatch(typeof(DollRoom), "SetupInfo")]
	[HarmonyPostfix]
	internal static void TranslateDollRoomCamera(UnitEntityData player, DollRoom __instance)
	{
		if (Classes.Utilities.ExceedsSizeConstraints(player))
		{
			return;
		}

		DollInstance = player;
		DollCamera cameraInstance = Classes.Utilities.GetDollCamera(__instance);

		if (cameraInstance is null)
		{
			ModEntry.Log("Could not fetch a DollCamera instance from DollRoom, aborting execution.");
			return;
		}

		ZoomScalar = 0f;
		DollCameraInstance = cameraInstance;

		if (!ModEntry.ModEnabledState)
		{
			cameraInstance.transform.position = Classes.Utilities.GetDefaultCoordinates();
			return;
		}

		cameraInstance.transform.position = Classes.Utilities.GetTranslatedCoordinatesByPlayer(player);
	}

	[HarmonyPatch(typeof(DollRoomCharacterController), "OnScroll")]
	[HarmonyPostfix]
	internal static void OnZoom(PointerEventData eventData)
	{
		if (!ModEntry.ModEnabledState)
		{
			return;
		}

		if (DollInstance is null || DollCameraInstance is null)
		{
			return;
		}

		ZoomScalar += eventData.scrollDelta.y * 0.1f;
		DollCameraInstance.transform.position = Classes.Utilities.InterpolateCameraCoordinatesByScalar(ZoomScalar, DollInstance);
	}
};