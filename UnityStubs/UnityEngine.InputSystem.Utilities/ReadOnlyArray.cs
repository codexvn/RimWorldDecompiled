using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem.Utilities;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ReadOnlyArray<TValue> : IReadOnlyList<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>
{
	public int Count
	{
		get
		{
			StubWarner.LogStub("Count", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ReadOnlyArray.cs", 5);
			return 0;
		}
	}

	public TValue this[int index]
	{
		get
		{
			StubWarner.LogStub("Item", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ReadOnlyArray.cs", 6);
			return default(TValue);
		}
	}

	IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ReadOnlyArray.cs", 7);
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		StubWarner.LogStub("GetEnumerator", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ReadOnlyArray.cs", 8);
		return null;
	}
}
