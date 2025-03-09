# region using directives

using UnityModManagerNet;

# endregion


namespace VanityCameraTweaks.Classes;

internal sealed class Settings : UnityModManager.ModSettings
{
	internal float CameraDistance { get; set; } = 0.0f;
	internal bool ForceRelaxedPosture { get; set; } = true;
	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};