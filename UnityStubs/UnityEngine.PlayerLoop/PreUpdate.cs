using System.Runtime.InteropServices;

namespace UnityEngine.PlayerLoop;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PreUpdate
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct IMGUISendQueuedEvents
	{
	}
}
