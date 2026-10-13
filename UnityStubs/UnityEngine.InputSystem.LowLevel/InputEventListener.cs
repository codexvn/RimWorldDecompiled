using System;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.LowLevel;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct InputEventListener : IObservable<InputEventPtr>
{
	public IDisposable Subscribe(IObserver<InputEventPtr> observer)
	{
		StubWarner.LogStub("Subscribe", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputEventListener.cs", 5);
		return null;
	}

	public static InputEventListener operator +(InputEventListener _, Action<InputEventPtr, InputDevice> callback)
	{
		InputSystem.AddOnEventCallback(callback);
		return default(InputEventListener);
	}

	public static InputEventListener operator -(InputEventListener _, Action<InputEventPtr, InputDevice> callback)
	{
		InputSystem.RemoveOnEventCallback(callback);
		return default(InputEventListener);
	}
}
