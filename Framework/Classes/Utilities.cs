#region using directives

using System.Reflection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ServiceWindow;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Classes;

internal static class Utilities
{
	internal static bool IsWithinSizeConstraints(UnitEntityData player)
	{
		return player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny;
	}

	internal static float GetNormalisedYScalar(float yValue)
	{
		return Mathf.InverseLerp(
			(float)PatchData.DollCameraZoomParams["MeshCameraOrientedMinY"],
			(float)PatchData.DollCameraZoomParams["MeshCameraOrientedMaxY"], 
			yValue);
	}

	internal static float GetCameraYBySize(UnitEntityData player)
	{
		float yBounds = GetNormalisedYScalar(player.View.CameraOrientedBoundsSize.y);
		return Mathf.Lerp(
			(float)PatchData.DollCameraZoomParams["CameraMinY"],
			(float)PatchData.DollCameraZoomParams["CameraMaxY"],
			yBounds);
	}

	internal static DollCamera GetDollCamera(DollRoom dollRoom)
	{
		return (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(dollRoom);
	}
};
