using System;
using System.Collections.Generic;

namespace Waylith.Rendering;

public struct LayerMask : IEquatable<LayerMask>
{
	private static readonly object Gate;

	private static readonly Dictionary<string, int> NameToIdMap;

	private static readonly Dictionary<int, string> IdToNameMap;

	private const int FirstDynamicLayerId = 16;

	private static int NextLayerId;

	private int _value;

	public int value
	{
		readonly get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	static LayerMask()
	{
		Gate = new object();
		NameToIdMap = new Dictionary<string, int>(StringComparer.Ordinal);
		IdToNameMap = new Dictionary<int, string>();
		NextLayerId = 16;
		NameToIdMap["Default"] = 0;
		IdToNameMap[0] = "Default";
	}

	public static int NameToLayer(string layerName)
	{
		if (string.IsNullOrWhiteSpace(layerName))
		{
			return -1;
		}
		lock (Gate)
		{
			if (NameToIdMap.TryGetValue(layerName, out var result))
			{
				return result;
			}
			if (NextLayerId >= 32)
			{
				throw new InvalidOperationException("Cannot allocate render layer '" + layerName + "': only 32 layers are supported.");
			}
			int num = NextLayerId++;
			NameToIdMap[layerName] = num;
			IdToNameMap[num] = layerName;
			return num;
		}
	}

	public static string LayerToName(int layer)
	{
		if ((uint)layer >= 32u)
		{
			return string.Empty;
		}
		lock (Gate)
		{
			string text;
			return IdToNameMap.TryGetValue(layer, out text) ? text : string.Empty;
		}
	}

	public static int GetMask(params string[] layerNames)
	{
		if (layerNames == null || layerNames.Length == 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < layerNames.Length; i++)
		{
			int num2 = NameToLayer(layerNames[i]);
			if ((uint)num2 < 32u)
			{
				num |= 1 << num2;
			}
		}
		return num;
	}

	public readonly bool Equals(LayerMask other)
	{
		return _value == other._value;
	}

	public override readonly bool Equals(object? obj)
	{
		if (obj is LayerMask other)
		{
			return Equals(other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		return _value;
	}

	public override readonly string ToString()
	{
		return _value.ToString();
	}

	public static implicit operator int(LayerMask mask)
	{
		return mask._value;
	}

	public static implicit operator LayerMask(int intVal)
	{
		LayerMask result = default(LayerMask);
		result._value = intVal;
		return result;
	}

	public static bool operator ==(LayerMask left, LayerMask right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(LayerMask left, LayerMask right)
	{
		return !left.Equals(right);
	}
}
