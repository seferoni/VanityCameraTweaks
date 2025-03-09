#region using directives

using System.Reflection;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.View.Equipment;
using Kingmaker.View.Animation;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View;



#endregion

namespace VanityCameraTweaks.Harmony;

[HarmonyPatch]
internal static class Patches
{
	[HarmonyPatch(typeof(UnitViewHandsEquipment), "get_ActiveMainHandWeaponStyle")]
	[HarmonyPostfix]
	internal static void RelaxUnarmedMainhandPosture(UnitViewHandsEquipment __instance, ref WeaponAnimationStyle __result)
	{
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
		DollCamera cameraInstance = (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(__instance);

		if (cameraInstance is null)
		{
			ModEntry.Log("m_Camera was null.");
			return;
		}

		ModEntry.Log("Descriptor.State.Size: " + (float)player.Descriptor.State.Size);
		ModEntry.Log("Descriptor.OriginalSize " + player.Descriptor.OriginalSize);
		ModEntry.Log("GetSizeScale: " + player.View.GetSizeScale());
		ModEntry.Log(string.Format("player.View.transform.localScale: [{0}, {1}, {2}", player.View.transform.localScale.x, player.View.transform.localScale.y, player.View.transform.localScale.z));
		ModEntry.Log(string.Format("player.View.transform.position: [{0}, {1}, {2}", player.View.transform.position.x, player.View.transform.position.y, player.View.transform.position.z));
		ModEntry.Log(string.Format("player.View.transform.localPosition: [{0}, {1}, {2}", player.View.transform.localPosition.x, player.View.transform.localPosition.y, player.View.transform.localPosition.z));
		ModEntry.Log(string.Format("player.View.CameraOrientedBoundsSize: [{0}, {1}", player.View.CameraOrientedBoundsSize.x, player.View.CameraOrientedBoundsSize.y));
		ModEntry.Log(string.Format("player.View.CameraOrientedCoreBoundsSize: [{0}, {1}", player.View.CameraOrientedCoreBoundsSize.x, player.View.CameraOrientedCoreBoundsSize.y));

		Vector3 newCoords = new Vector3
		(
			cameraInstance.transform.position.x, // 999.85f
			cameraInstance.transform.position.y, // 586.1129f
			-1f  // 0.987f
		);
		// TODO: consider adapting these translations for each character race
		cameraInstance.transform.position = newCoords;
	}
};