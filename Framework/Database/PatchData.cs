#region using directives

using Kingmaker.View.Animation;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Blueprints;

#endregion

namespace VanityCameraTweaks.Framework.Database;

#region tuple aliases

using Range = (float Min, float Max);

#endregion

internal static class PatchData
{
	// Runtime variables.
	private static float zoomScalar = 0f;

	internal static float ZoomScalar
	{
		get => zoomScalar;
		set => zoomScalar = Mathf.Clamp(value, 0f, 1f);
	}
	internal static DollCamera DollCameraInstance { get; set; } = null!;
	internal static UnitEntityData DollInstance { get; set; } = null!;

	// Compile-time constants.
	internal static Vector3 CameraDefaults { get; } = new(999.85f, 586.1129f, 0.987f);
	internal static Range ZoomFineOffset { get; } = (0f, 0.5f);
	internal static Range CameraNominalY { get; } = (585.4f, 586.2f);
	internal static Dictionary<string, float> DollCameraZ { get; } = new()
	{
		{ "Small", -1.5f },
		{ "Medium", -0.5f }
	};
	internal static Dictionary<Race, float> RaceHeightScalars { get; } = new()
	{
		{ Race.Aasimar, 0.75f },
		{ Race.Dwarf, 0.25f },
		{ Race.Elf, 0.85f },
		{ Race.Gnome, 0f },
		{ Race.Goblin, 0f },
		{ Race.HalfElf, 0.8f },
		{ Race.Halfling, 0f },
		{ Race.HalfOrc, 1f },
		{ Race.Human, 0.8f },
		{ Race.Tiefling, 0.8f }
	};
	internal static WeaponAnimationStyle[] TargetedAnimations { get; } =
	[
		WeaponAnimationStyle.Fist,
		WeaponAnimationStyle.MartialArts
	];
};