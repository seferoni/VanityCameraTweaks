#region using directives

using VanityCameraTweaks.Framework.Attributes;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal sealed class Settings : UnityModManager.ModSettings
{
	[UMMInclude]
	[UMMRange(-1.5f, 0.987f)]
	internal float CameraDistance { get; set; } = (float)PatchData.DollCameraZoomParams["NominalZCoords"];

	[UMMInclude]
	internal bool ForceRelaxedPosture { get; set; } = (bool)PatchData.DollCameraZoomParams["ForceRelaxedPostureDefault"];
	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};