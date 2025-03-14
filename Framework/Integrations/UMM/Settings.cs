#region using directives

using VanityCameraTweaks.Framework.Attributes;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

public sealed class Settings : UnityModManager.ModSettings
{
	[UMMInclude]
	public bool ForceRelaxedPosture { get; set; } = true;

	[UMMRange(0f, 1f)]
	[UMMInclude]
	public float ZoomFineOffset { get; set; } = 1f;

	public void OnChange()
	{
		return;
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};