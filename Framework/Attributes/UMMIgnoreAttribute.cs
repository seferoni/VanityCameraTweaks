namespace VanityCameraTweaks.Framework.Attributes;

/// <summary>
/// Designates ignored properties when building UMM settings.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class UMMIgnoreAttribute : Attribute;