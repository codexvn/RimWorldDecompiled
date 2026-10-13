using System;

namespace UnityEngine.LowLevel;

public struct PlayerLoopSystem
{
	public delegate void UpdateFunction();

	public Type type;

	public PlayerLoopSystem[] subSystemList;

	public UpdateFunction updateDelegate;

	public nint updateFunction;

	public nint loopConditionFunction;

	public override string ToString()
	{
		return type?.Name ?? string.Empty;
	}
}
