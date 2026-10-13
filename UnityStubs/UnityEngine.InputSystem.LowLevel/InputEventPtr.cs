using System;

namespace UnityEngine.InputSystem.LowLevel;

public struct InputEventPtr : IEquatable<InputEventPtr>
{
	internal InputEventData Data { get; }

	internal InputEventPtr(InputEventData data)
	{
		Data = data;
	}

	public bool Equals(InputEventPtr other)
	{
		return Data == other.Data;
	}

	public bool IsA<T0>()
	{
		if (Data == null)
		{
			return false;
		}
		if (!(typeof(T0) == typeof(StateEvent)))
		{
			if (typeof(T0) == typeof(DeltaStateEvent))
			{
				return Data.Kind == InputEventKind.Delta;
			}
			return false;
		}
		return Data.Kind == InputEventKind.State;
	}
}
