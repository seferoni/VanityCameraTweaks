#region using directives

using Kingmaker.View.Animation;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.EntitySystem.Entities;

#endregion

namespace VanityCameraTweaks.Framework.Database;

#region tuple aliases

using Range = (float Min, float Max);

#endregion

internal static class PatchData
{
	// Runtime variables.
	private static float zoomScalar = 1f;

	internal static float ZoomScalar
	{
		get => zoomScalar;
		set => zoomScalar = Mathf.Clamp(value, 0f, 1f);
	}
	internal static DollCamera DollCameraInstance { get; set; } = null!;
	internal static UnitEntityData DollInstance { get; set; } = null!;

	// Static variables.
	internal static Vector3 CameraDefaults { get; } = new(999.85f, 586.1129f, 0.987f);
	internal static Range ZoomFineOffset { get; } = (0f, 0.5f);
	internal static Range CameraNominalY { get; } = (585.4f, 586.2f);
	internal static Range MeshCameraOrientedY { get; } = (1.5f, 2.3f);
	internal static Dictionary<string, float> DollCameraZ { get; } = new()
	{
		{ "Small", -1.5f },
		{ "Medium", -0.5f }
	};
	internal static WeaponAnimationStyle[] TargetedAnimations { get; } =
	[
		WeaponAnimationStyle.Fist,
		WeaponAnimationStyle.MartialArts
	];
};