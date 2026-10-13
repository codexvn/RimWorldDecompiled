using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Touch
{
	public int fingerId
	{
		get
		{
			StubWarner.LogStub("fingerId", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Touch.cs", 5);
			return 0;
		}
	}

	public TouchPhase phase
	{
		get
		{
			StubWarner.LogStub("phase", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Touch.cs", 6);
			return TouchPhase.Began;
		}
	}

	public Vector2 position
	{
		get
		{
			StubWarner.LogStub("position", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Touch.cs", 7);
			return default(Vector2);
		}
	}
}
