using System;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace UnityEngine.InputSystem;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct InputBinding : IEquatable<InputBinding>
{
	public const string kSeparatorString = ";";

	public const char Separator = ';';

	public bool Equals(InputBinding other)
	{
		StubWarner.LogStub("Equals", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\InputBinding.cs", 5);
		return false;
	}
}
