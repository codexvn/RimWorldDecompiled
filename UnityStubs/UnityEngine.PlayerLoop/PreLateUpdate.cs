using System.Runtime.InteropServices;

namespace UnityEngine.PlayerLoop;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PreLateUpdate
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ScriptRunBehaviourLateUpdate
	{
	}
}
