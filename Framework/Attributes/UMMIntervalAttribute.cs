namespace VanityCameraTweaks.Framework.Attributes;

/// <summary>
/// Defines the interval for a numerical UMM setting.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class UMMIntervalAttribute : Attribute
{
	/// <summary>
	/// The interval value for the targeted numerical UMM property.
	/// </summary>
	internal float Value { get; set; }

	internal UMMIntervalAttribute(float interval)
	{
		Value = interval;
	}
};