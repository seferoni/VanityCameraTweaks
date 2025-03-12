#region using directives

using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

public sealed class Settings :
	UnityModManager.ModSettings,
	IDrawable
{
	[Draw("Zoom Fine Offset", DrawType.Slider, Min = 0f, Max = 1f)] public float ZoomFineOffset = 0f;

	[Draw("Force Relaxed Posture When Unarmed")] public bool ForceRelaxedPosture = true;

	public void OnChange()
	{
		return;
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};