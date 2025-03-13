#region using directives

using Kingmaker.Enums;
using Kingmaker.View.Animation;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.EntitySystem.Entities;
using System.Reflection;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Classes;

internal static class Utilities
{
	internal static bool ExceedsSizeConstraints(UnitEntityData player)
	{
		return player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny;
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
			ModEntry.SettingsInstance.ZoomFineOffset);
	}

	internal static float GetNormalisedYPosScalar(float yValue)
	{
		return Mathf.InverseLerp(
			PatchData.MeshCameraOrientedY.Min,
			PatchData.MeshCameraOrientedY.Max,
			yValue);
	}

	internal static float GetCameraYPosByPlayer(UnitEntityData player)
	{
		float yBounds = GetNormalisedYPosScalar(player.View.CameraOrientedBoundsSize.y);
		float cameraHeight = Mathf.Lerp(
			PatchData.CameraNominalY.Min,
			PatchData.CameraNominalY.Max,
			yBounds);
		ModEntry.DebugLog($"Got a camera height of {cameraHeight} with a y-scalar of {yBounds}.");
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
