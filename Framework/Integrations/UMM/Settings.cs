#region using directives

using VanityCameraTweaks.Framework.Attributes;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal sealed class Settings : UnityModManager.ModSettings
{
	internal float CameraDistance { get; set; }
	internal bool ForceRelaxedPosture { get; set; }
	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};