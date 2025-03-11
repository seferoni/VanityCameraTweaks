namespace VanityCameraTweaks.Framework.Database;

internal static class PatchData
{
	internal static object GetValue(string key) => key switch
	{
		"ForceRelaxedPostureDefault" => true,
		"MeshCameraOrientedMinY" => 1.4f,
		"MeshCameraOrientedMaxY" => 2.3f,
		"CameraMinY" => 585f,
		"CameraMaxY" => 586.1129f,
		_ => throw new KeyNotFoundException()
	};
};