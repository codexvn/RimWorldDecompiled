using System;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Hash128 : IComparable, IComparable<Hash128>, IEquatable<Hash128>
{
	public const ulong kConst = 16045690984833335023uL;

	public int CompareTo(object obj)
	{
		StubWarner.LogStub("CompareTo", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Hash128.cs", 5);
		return 0;
	}

	public int CompareTo(Hash128 rhs)
	{
		StubWarner.LogStub("CompareTo", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Hash128.cs", 6);
		return 0;
	}

	public bool Equals(Hash128 obj)
	{
		StubWarner.LogStub("Equals", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Hash128.cs", 7);
		return false;
	}

	public void Append<T0>(T0[] data)
	{
		StubWarner.LogStub("Append", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Hash128.cs", 9);
	}

	public static Hash128 Compute(int val)
	{
		StubWarner.LogStub("Compute", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Hash128.cs", 10);
		return default(Hash128);
	}
}
