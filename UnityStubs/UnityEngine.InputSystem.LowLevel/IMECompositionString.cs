using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.LowLevel;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct IMECompositionString : IEnumerable<char>, IEnumerable
{
	public IEnumerator<char> GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\IMECompositionString.cs", 5);
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\IMECompositionString.cs", 6);
		return null;
	}
}
