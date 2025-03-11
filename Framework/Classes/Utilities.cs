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
	internal static bool ExceedsSizeConstraints(UnitEntityData player)
	{
		return player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny;
	}

	internal static float GetNormalisedYScalar(float yValue)
	{
		return  Mathf.InverseLerp(
			(float)PatchData.GetValue("MeshCameraOrientedMinY"),
			(float)PatchData.GetValue("MeshCameraOrientedMaxY"),
			yValue);
	}

	internal static float GetCameraYBySize(UnitEntityData player)
	{
		float yBounds = GetNormalisedYScalar(player.View.CameraOrientedBoundsSize.y);
		ModEntry.Log($"Got a normalised y-scalar of {yBounds}");
		float cameraHeight = Mathf.Lerp(
			(float)PatchData.GetValue("CameraMinY"),
			(float)PatchData.GetValue("CameraMaxY"),
			yBounds);
		ModEntry.Log($"Got a camera height of {cameraHeight}");
		return cameraHeight;
	}

	internal static DollCamera GetDollCamera(DollRoom dollRoom)
	{
		return (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(dollRoom);
	}
};
