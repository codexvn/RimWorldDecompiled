using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.LowLevel;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct DeltaStateEvent : IInputEventTypeInfo
{
	public const int Type = 1145852993;

	public FourCC typeStatic
	{
		get
		{
			StubWarner.LogStub("typeStatic", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\DeltaStateEvent.cs", 5);
			return default(FourCC);
		}
	}
}
