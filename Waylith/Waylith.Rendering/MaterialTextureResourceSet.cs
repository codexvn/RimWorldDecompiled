using System;
using System.Collections.Generic;

namespace Waylith.Rendering;

public struct MaterialTextureResourceSet<T> : IEquatable<MaterialTextureResourceSet<T>>
{
	private T resource0;

	private T resource1;

	private T resource2;

	private T resource3;

	private T resource4;

	private T resource5;

	private T resource6;

	private T resource7;

	private T resource8;

	private T resource9;

	private T resource10;

	private T resource11;

	private T resource12;

	private T resource13;

	private T resource14;

	private T resource15;

	public readonly T this[int index] => index switch
	{
		0 => resource0, 
		1 => resource1, 
		2 => resource2, 
		3 => resource3, 
		4 => resource4, 
		5 => resource5, 
		6 => resource6, 
		7 => resource7, 
		8 => resource8, 
		9 => resource9, 
		10 => resource10, 
		11 => resource11, 
		12 => resource12, 
		13 => resource13, 
		14 => resource14, 
		15 => resource15, 
		_ => throw new ArgumentOutOfRangeException("index"), 
	};

	public void Set(int slot, T resource)
	{
		switch (slot)
		{
		case 0:
			resource0 = resource;
			break;
		case 1:
			resource1 = resource;
			break;
		case 2:
			resource2 = resource;
			break;
		case 3:
			resource3 = resource;
			break;
		case 4:
			resource4 = resource;
			break;
		case 5:
			resource5 = resource;
			break;
		case 6:
			resource6 = resource;
			break;
		case 7:
			resource7 = resource;
			break;
		case 8:
			resource8 = resource;
			break;
		case 9:
			resource9 = resource;
			break;
		case 10:
			resource10 = resource;
			break;
		case 11:
			resource11 = resource;
			break;
		case 12:
			resource12 = resource;
			break;
		case 13:
			resource13 = resource;
			break;
		case 14:
			resource14 = resource;
			break;
		case 15:
			resource15 = resource;
			break;
		default:
			throw new ArgumentOutOfRangeException("slot");
		}
	}

	public readonly bool Equals(in MaterialTextureResourceSet<T> other)
	{
		EqualityComparer<T> @default = EqualityComparer<T>.Default;
		for (int i = 0; i < 16; i++)
		{
			if (!@default.Equals(this[i], other[i]))
			{
				return false;
			}
		}
		return true;
	}

	public readonly bool Equals(MaterialTextureResourceSet<T> other)
	{
		return Equals(in other);
	}

	public override readonly bool Equals(object? obj)
	{
		if (obj is MaterialTextureResourceSet<T> other)
		{
			return Equals(in other);
		}
		return false;
	}

	public override readonly int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		for (int i = 0; i < 16; i++)
		{
			hashCode.Add(this[i]);
		}
		return hashCode.ToHashCode();
	}

	public static bool operator ==(in MaterialTextureResourceSet<T> left, in MaterialTextureResourceSet<T> right)
	{
		return left.Equals(in right);
	}

	public static bool operator !=(in MaterialTextureResourceSet<T> left, in MaterialTextureResourceSet<T> right)
	{
		return !(left == right);
	}
}
