#region using directives

using VanityCameraTweaks.Framework.Attributes;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

public sealed class Settings : UnityModManager.ModSettings
{
	[UMMRange(0f, 1f)]
	[UMMInclude]
	public float ZoomFineOffset { get; set; } = 0f;

	[UMMInclude]
	public bool ForceRelaxedPosture { get; set; } = true;

	public void OnChange()
	{
		return;
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};