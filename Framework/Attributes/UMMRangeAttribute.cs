namespace VanityCameraTweaks.Framework.Attributes;

/// <summary>
/// Defines the range for a numerical UMM setting.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class UMMRangeAttribute : Attribute
{
	/// <summary>
	/// The range for the targeted numerical UMM property.
	/// </summary>
	internal float Min { get; set; }

	internal float Max { get; set; }

	internal UMMRangeAttribute(float min, float max)
	{
		Min = min;
		Max = max;
	}
};