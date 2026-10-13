using System.Collections;
using UnityStubs.Internal;

namespace UnityEngine;

public abstract class CustomYieldInstruction : IEnumerator
{
	public abstract bool keepWaiting { get; }

	public object Current => null;

	public bool MoveNext()
	{
		return keepWaiting;
	}

	public void Reset()
	{
		StubWarner.LogStub("Reset", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CustomYieldInstruction.cs", 8);
	}
}
