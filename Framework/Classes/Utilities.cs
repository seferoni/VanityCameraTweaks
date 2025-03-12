#region using directives

using System.Reflection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ServiceWindow;
using VanityCameraTweaks.Framework.Database;
using Kingmaker.View.Animation;

#endregion

namespace VanityCameraTweaks.Framework.Classes;

internal static class Utilities
{
	internal static bool ExceedsSizeConstraints(UnitEntityData player)
	{
		return player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny;
	}

	internal static Vector3 InterpolateCameraCoordinatesByScalar(float zoomScalar, UnitEntityData playerInstance)
	{
		return Vector3.Lerp(
			GetTranslatedCoordinatesByPlayer(playerInstance),
			GetDefaultCoordinates(),
			zoomScalar
		);
	}

	internal static Vector3 GetDefaultCoordinates()
	{
		return new Vector3(
			(float)PatchData.GetValue("CameraDefaultX"),
			(float)PatchData.GetValue("CameraDefaultY"),
			(float)PatchData.GetValue("CameraDefaultZ"));
	}

	internal static Vector3 GetTranslatedCoordinatesByPlayer(UnitEntityData playerInstance)
	{
		return new Vector3(
			(float)PatchData.GetValue("CameraDefaultX"),
			GetCameraYPosByPlayer(playerInstance),
			GetCameraZPosByPlayer(playerInstance)
		);
	}

	internal static float GetZoomFineOffset()
	{
		return -Mathf.Lerp(
			(float)PatchData.GetValue("ZoomFineOffsetMin"),
			(float)PatchData.GetValue("ZoomFineOffsetMax"),
			ModEntry.SettingsInstance.ZoomFineOffset);
	}

	internal static float GetNormalisedYPosScalar(float yValue)
	{
		return Mathf.InverseLerp(
			(float)PatchData.GetValue("MeshCameraOrientedMinY"),
			(float)PatchData.GetValue("MeshCameraOrientedMaxY"),
			yValue);
	}

	internal static float GetCameraYPosByPlayer(UnitEntityData player)
	{
		float yBounds = GetNormalisedYPosScalar(player.View.CameraOrientedBoundsSize.y);
		float cameraHeight = Mathf.Lerp(
			(float)PatchData.GetValue("CameraNominalMinY"),
			(float)PatchData.GetValue("CameraNominalMaxY"),
			yBounds);
		ModEntry.Log($"Got a camera height of {cameraHeight}");
		return cameraHeight;
	}

	internal static float GetCameraZPosByPlayer(UnitEntityData player)
	{
		string playerSize = player.Descriptor.State.Size.ToString();
		ModEntry.Log($"Got a player size of {playerSize}");
		float nominalZoom = (float)PatchData.GetValue($"{playerSize}DollCameraZ");
		return nominalZoom + GetZoomFineOffset();
	}

	internal static DollCamera GetDollCamera(DollRoom dollRoom)
	{
		return (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(dollRoom);
	}

	internal static bool IsWeaponAnimationViable(WeaponAnimationStyle animationStyle)
	{
		return Array.Exists(PatchData.TargetedAnimations, style => style == animationStyle);
	}
};
