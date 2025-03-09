#region using directives

using System.Reflection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ServiceWindow;

#endregion

namespace VanityCameraTweaks.Framework.Classes;

internal static class Utilities
{
	internal static bool IsWithinSizeConstraints(UnitEntityData player)
	{
		return player.Descriptor.State.Size >= Size.Large || player.Descriptor.State.Size <= Size.Tiny;
	}

	internal static DollCamera GetDollCamera(DollRoom dollRoom)
	{
		return (DollCamera)typeof(DollRoom)
			.GetField("m_Camera", BindingFlags.NonPublic | BindingFlags.Instance)
			.GetValue(dollRoom);
	}
};
