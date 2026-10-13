using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.LowLevel;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StateEvent : IInputEventTypeInfo
{
	public const int kStateDataSizeToSubtract = 1;

	public const int Type = 1398030676;

	public FourCC typeStatic
	{
		get
		{
			StubWarner.LogStub("typeStatic", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\StateEvent.cs", 5);
			return default(FourCC);
		}
	}
}
