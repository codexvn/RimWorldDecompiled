using System;

namespace UnityEngine.Rendering;

public struct LocalKeyword : IEquatable<LocalKeyword>
{
	private readonly Shader _shader;

	private readonly string _name;

	public bool isValid
	{
		get
		{
			if (_shader != null)
			{
				return !string.IsNullOrWhiteSpace(_name);
			}
			return false;
		}
	}

	internal string Name => _name;

	public LocalKeyword(Shader shader, string name)
	{
		_shader = shader;
		_name = name ?? string.Empty;
	}

	public bool Equals(LocalKeyword rhs)
	{
		if ((object)_shader == rhs._shader)
		{
			return string.Equals(_name, rhs._name, StringComparison.Ordinal);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is LocalKeyword rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_shader, _name);
	}

	public static bool operator ==(LocalKeyword lhs, LocalKeyword rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(LocalKeyword lhs, LocalKeyword rhs)
	{
		return !lhs.Equals(rhs);
	}
}
