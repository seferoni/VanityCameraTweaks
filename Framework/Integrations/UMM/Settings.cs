#region using directives

using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

public sealed class Settings : UnityModManager.ModSettings, IDrawable
{

	[Draw("Camera Distance", DrawType.Slider, Min  = -1.5f, Max = 1f)] public float CameraDistance = -0.5f;

	[Draw("Force Relaxed Posture")] public bool ForceRelaxedPosture = true;

	public void OnChange()
	{
		return;
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};