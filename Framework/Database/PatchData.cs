namespace VanityCameraTweaks.Framework.Database;

internal static class PatchData
{
	internal static readonly Dictionary<string, object> DollCameraZoomParams = new()
	{
		{ "ForceRelaxedPostureDefault", true },
		{ "MeshCameraOrientedMinY", 1.4f },
		{ "MeshCameraOrientedMaxY", 2.3f },
		{ "CameraMinY", 585f },
		{ "CameraMaxY", 586.1129f },
		{ "NominalZCoord", 0f }
	};
};