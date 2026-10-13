using System.Collections.Generic;

namespace Waylith.Rendering;

public static class RenderPassRegistry
{
	public const byte AnyPass = 0;

	private static byte nextId = 1;

	private static readonly Dictionary<string, byte> ids = new Dictionary<string, byte>();

	public static byte Register(string name)
	{
		if (ids.TryGetValue(name, out var value))
		{
			return value;
		}
		byte b = nextId++;
		ids[name] = b;
		return b;
	}

	public static string GetName(byte id)
	{
		if (id == 0)
		{
			return "AnyPass";
		}
		foreach (KeyValuePair<string, byte> id2 in ids)
		{
			if (id2.Value == id)
			{
				return id2.Key;
			}
		}
		return id.ToString();
	}
}
