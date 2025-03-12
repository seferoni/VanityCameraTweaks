#region using directives

using Kingmaker.View.Animation;

#endregion

namespace VanityCameraTweaks.Framework.Database;

internal static class PatchData
{
	internal static object GetValue(string key) => key switch
	{
		"MeshCameraOrientedMinY" => 1.5f,
		"MeshCameraOrientedMaxY" => 2.3f,
		"CameraNominalMinY" => 585.4f,
		"CameraNominalMaxY" => 586.2f,
		"SmallDollCameraZ" => -1.5f,
		"MediumDollCameraZ" => -0.5f,
		"ZoomFineOffsetMin" => 0f,
		"ZoomFineOffsetMax" => 0.3f,
		"CameraDefaultX" => 999.85f,
		"CameraDefaultY" => 586.1129f,
		"CameraDefaultZ" => 0.987f,
		_ => throw new KeyNotFoundException()
	};

	internal static WeaponAnimationStyle[] TargetedAnimations =
	[
		WeaponAnimationStyle.Fist,
		WeaponAnimationStyle.MartialArts
	];
};