#region using directives

using Kingmaker.Enums;
using Kingmaker.View.Animation;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.EntitySystem.Entities;
using System.Reflection;
using VanityCameraTweaks.Framework.Database;
using Kingmaker.Blueprints;

#endregion

namespace VanityCameraTweaks.Framework.Classes;

internal static class Utilities
{
	internal static bool IsViableForZoom(UnitEntityData player)
	{
		if (player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny)
		{
			return false;
		}

		var raceBlueprint = player.Descriptor.Progression.Race;

		if (raceBlueprint == null)
		{
			return false;
		}

		if (!PatchData.RaceHeightScalars.ContainsKey(raceBlueprint.RaceId))
		{
			return false;
		}

		return true;
	}

	internal static float GetNormalisedYPosScalarByPlayer(UnitEntityData player)
	{
		return PatchData.RaceHeightScalars[player.Descriptor.Progression.Race.RaceId];
	}

	internal static Vector3 GetTranslatedCoordinatesByPlayer(UnitEntityData playerInstance)
	{
		return new Vector3(
			PatchData.CameraDefaults.x,
			GetCameraYPosByPlayer(playerInstance),
			GetCameraZPosByPlayer(playerInstance)
		);
	}

	internal static float GetZoomFineOffset()
	{
		return -Mathf.Lerp(
			PatchData.ZoomFineOffset.Min,
			PatchData.ZoomFineOffset.Max,
			ModEntry.SettingsInstance.ZoomFineOffset
		);
	}

	internal static float GetCameraYPosByPlayer(UnitEntityData player)
	{
		float yScalar = GetNormalisedYPosScalarByPlayer(player);
		float cameraHeight = Mathf.Lerp(
			PatchData.CameraNominalY.Min,
			PatchData.CameraNominalY.Max,
			yScalar
		);
		ModEntry.DebugLog($"Got a camera height of {cameraHeight} with a y-scalar of {yScalar}.");
		return cameraHeight;
	}

	internal static float GetCameraZPosByPlayer(UnitEntityData player)
	{
		string playerSize = player.Descriptor.State.Size.ToString();
		float nominalZoom = PatchData.DollCameraZ[playerSize];
		return nominalZoom + GetZoomFineOffset();
	}

	internal static DollCamera GetDollCamera(DollRoom dollRoom)
	{
		return (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(dollRoom);
	}

	internal static Vector3 InterpolateCameraCoordinatesByScalar(float zoomScalar, UnitEntityData playerInstance)
	{
		return Vector3.Lerp(
			GetTranslatedCoordinatesByPlayer(playerInstance),
			PatchData.CameraDefaults,
			zoomScalar
		);
	}

	internal static bool IsWeaponAnimationViable(WeaponAnimationStyle animationStyle)
	{
		return Array.Exists(PatchData.TargetedAnimations, style => style == animationStyle);
	}
};
