using System.Runtime.InteropServices;

namespace UnityEngine.PlayerLoop;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Update
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ScriptRunBehaviourUpdate
	{
	}
}
