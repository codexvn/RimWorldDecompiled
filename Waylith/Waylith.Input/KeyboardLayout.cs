using System.Collections.Generic;
using Brutal.GlfwApi;

namespace Waylith.Input;

public static class KeyboardLayout
{
	internal static bool Available;

	private static readonly Key[] CharacterKeys = new Key[49]
	{
		Key.A,
		Key.B,
		Key.C,
		Key.D,
		Key.E,
		Key.F,
		Key.G,
		Key.H,
		Key.I,
		Key.J,
		Key.K,
		Key.L,
		Key.M,
		Key.N,
		Key.O,
		Key.P,
		Key.Q,
		Key.R,
		Key.S,
		Key.T,
		Key.U,
		Key.V,
		Key.W,
		Key.X,
		Key.Y,
		Key.Z,
		Key.Digit1,
		Key.Digit2,
		Key.Digit3,
		Key.Digit4,
		Key.Digit5,
		Key.Digit6,
		Key.Digit7,
		Key.Digit8,
		Key.Digit9,
		Key.Digit0,
		Key.Backquote,
		Key.Quote,
		Key.Semicolon,
		Key.Comma,
		Key.Period,
		Key.Slash,
		Key.Backslash,
		Key.LeftBracket,
		Key.RightBracket,
		Key.Minus,
		Key.Equals,
		Key.OEM1,
		Key.OEM2
	};

	private static readonly Dictionary<Key, char> UsLayoutChar = BuildUsLayoutChars();

	private static readonly Dictionary<char, Key> UsLayoutKeyForChar = BuildUsLayoutKeyForChar();

	private static Dictionary<char, Key>? charToPhysical;

	private static Dictionary<Key, char> BuildUsLayoutChars()
	{
		Dictionary<Key, char> dictionary = new Dictionary<Key, char>();
		for (Key key = Key.A; key <= Key.Z; key++)
		{
			dictionary[key] = (char)(97 + (key - 1));
		}
		for (Key key2 = Key.Digit0; key2 <= Key.Digit9; key2++)
		{
			dictionary[key2] = (char)(48 + (key2 - 27));
		}
		dictionary[Key.Backquote] = '`';
		dictionary[Key.Quote] = '\'';
		dictionary[Key.Semicolon] = ';';
		dictionary[Key.Comma] = ',';
		dictionary[Key.Period] = '.';
		dictionary[Key.Slash] = '/';
		dictionary[Key.Backslash] = '\\';
		dictionary[Key.LeftBracket] = '[';
		dictionary[Key.RightBracket] = ']';
		dictionary[Key.Minus] = '-';
		dictionary[Key.Equals] = '=';
		return dictionary;
	}

	private static Dictionary<char, Key> BuildUsLayoutKeyForChar()
	{
		Dictionary<char, Key> dictionary = new Dictionary<char, Key>();
		foreach (KeyValuePair<Key, char> item in BuildUsLayoutChars())
		{
			item.Deconstruct(out var key, out var value);
			Key value2 = key;
			char key2 = value;
			dictionary[key2] = value2;
		}
		return dictionary;
	}

	public static void Invalidate()
	{
		charToPhysical = null;
	}

	public static char? TypedChar(Key key)
	{
		if (!Available || !UsLayoutChar.ContainsKey(key))
		{
			return null;
		}
		GlfwKey glfwKey = GlfwKeyMap.ToGlfwKey(key);
		if (glfwKey == GlfwKey.Unknown)
		{
			return null;
		}
		string keyName = Glfw.GetKeyName(glfwKey, 0);
		if (string.IsNullOrEmpty(keyName))
		{
			return null;
		}
		if (keyName.Length != 1)
		{
			return null;
		}
		return char.ToLowerInvariant(keyName[0]);
	}

	public static Key PhysicalKeyTyping(char c)
	{
		if (!Available)
		{
			return UsLayoutKeyForChar.GetValueOrDefault(char.ToLowerInvariant(c), Key.None);
		}
		Dictionary<char, Key> dictionary = charToPhysical;
		if (dictionary == null)
		{
			dictionary = new Dictionary<char, Key>();
			Key[] characterKeys = CharacterKeys;
			foreach (Key key in characterKeys)
			{
				char? c2 = TypedChar(key);
				if (c2.HasValue)
				{
					char valueOrDefault = c2.GetValueOrDefault();
					dictionary.TryAdd(valueOrDefault, key);
				}
			}
			charToPhysical = dictionary;
		}
		return dictionary.GetValueOrDefault(char.ToLowerInvariant(c), Key.None);
	}

	public static Key LogicalToPhysical(Key logical)
	{
		if (!UsLayoutChar.TryGetValue(logical, out var value))
		{
			return logical;
		}
		Key key = PhysicalKeyTyping(value);
		if (key == Key.None)
		{
			return logical;
		}
		return key;
	}

	public static Key PhysicalToLogical(Key physical)
	{
		char? c = TypedChar(physical);
		if (c.HasValue)
		{
			char valueOrDefault = c.GetValueOrDefault();
			return UsLayoutKeyForChar.GetValueOrDefault(valueOrDefault, physical);
		}
		return physical;
	}

	public static string? DisplayName(Key key)
	{
		char? c = TypedChar(key);
		if (c.HasValue)
		{
			char valueOrDefault = c.GetValueOrDefault();
			return char.ToUpperInvariant(valueOrDefault).ToString();
		}
		return null;
	}
}
