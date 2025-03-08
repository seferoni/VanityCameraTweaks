#region using directives

using System.Reflection;
using Kingmaker.UI.ServiceWindow;
using UnityEngine.EventSystems;

# endregion

namespace VanityCameraTweaks.Harmony;

[HarmonyPatch]
internal class Patches
{
	[HarmonyPatch(typeof(DollRoomCharacterController), "OnScroll")]
	[HarmonyPrefix]
	internal static void PrefixOnScroll(PointerEventData eventData)
	{
		ModEntry.Log("OnScroll called!");
		ModEntry.Log(string.Format("Got a delta-y of {0}.", eventData.scrollDelta.y));
		ModEntry.Log(string.Format("Got a DollCamera state of {0}.", (bool)DollCamera.Current));
	}

	[HarmonyPatch(typeof(DollRoom), "Show")]
	[HarmonyPostfix]
	internal static void PostfixOnShow(bool visible, DollRoom __instance)
	{
		if (!visible)
		{
			ModEntry.Log("Hiding DollRoom.");
			return;
		}

		FieldInfo cameraField = typeof(DollRoom).GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance);

		if (cameraField is null)
		{
			ModEntry.Log("Could not find m_Camera.");
			return;
		}

		DollCamera cameraInstance = (DollCamera)cameraField.GetValue(__instance);

		if (cameraInstance is null)
		{
			ModEntry.Log("m_Camera is null.");
			return;
		}

		ModEntry.Log("Camera instance found!");
		ModEntry.Log(string.Format("Got Camera coordinates as [{0}, {1}, {2}]", cameraInstance.transform.position.x, cameraInstance.transform.position.y, cameraInstance.transform.position.z));
		Vector3 newCoords = new Vector3
		(
			cameraInstance.transform.position.x, // 999.85f
			cameraInstance.transform.position.y, // 586.1129f
			cameraInstance.transform.position.z  // 0.987f
		);
		// TODO: consider excluding these translations during character creation
		// TODO: consider adapting these translations for each character race
		cameraInstance.transform.position = newCoords;
	}
};