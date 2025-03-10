#region using directives

using VanityCameraTweaks.Framework.Attributes;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal sealed class Settings : UnityModManager.ModSettings
{
	internal float CameraDistance { get; set; } = (float)PatchData.DollCameraZoomParams["NominalZCoords"];
	internal bool ForceRelaxedPosture { get; set; } = (bool)PatchData.DollCameraZoomParams["ForceRelaxedPostureDefault"];
	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};