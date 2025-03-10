namespace VanityCameraTweaks.Framework.Database;

internal static class PatchData
{
	internal static readonly Dictionary<string, object> DollCameraZoomParams = new()
	{
		{ "MinZCoord", 0.987f },
		{ "NominalZCoord", 0f },
		{ "MaxZCoord", -1.5f },
		{ "MinYCoord", 585f },
		{ "MaxYCoord", 586.1129f },
		{ "ForceRelaxedPostureDefault", true }
	};
};