#region using directives

using VanityCameraTweaks.Framework.Attributes;
using VanityCameraTweaks.Framework.Database;

#endregion

namespace VanityCameraTweaks.Framework.Integrations.UMM;

internal sealed class Settings : UnityModManager.ModSettings, IDrawable
{

	[Draw("Camera Distance", Min  = -1.5f, Max = 1f, Precision = 1)] 
	internal float CameraDistance = 0f;

	[Draw("Force Relaxed Posture")]
	internal bool ForceRelaxedPosture = true;

	public void OnChange()
	{
		return;
	}

	public override void Save(UnityModManager.ModEntry modEntry)
	{
		Save(this, modEntry);
	}
};