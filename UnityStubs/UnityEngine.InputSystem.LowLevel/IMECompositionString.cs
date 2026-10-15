using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.LowLevel;

public struct IMECompositionString : IEnumerable<char>, IEnumerable
{
	private readonly string characters;

	public int Count => characters?.Length ?? 0;

	public char this[int index] => characters[index];

	public IMECompositionString(string characters)
	{
		this.characters = characters ?? "";
	}

	public override string ToString()
	{
		return characters ?? "";
	}

	public static implicit operator string(IMECompositionString s)
	{
		return s.ToString();
	}

	public IEnumerator<char> GetEnumerator()
	{
		return ((IEnumerable<char>)(characters ?? "")).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
