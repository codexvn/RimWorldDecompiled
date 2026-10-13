using System;

namespace Waylith.Rendering;

public readonly struct ShaderVariantKey : IEquatable<ShaderVariantKey>
{
	private readonly int _hashCode;

	public Shader Shader { get; }

	public long DynamicKeywordKeyId { get; }

	public string DynamicKeywordKey { get; }

	public bool UseTexture { get; }

	public bool OutlineMrt { get; }

	public bool Instanced { get; }

	public ShaderVariantKey(Shader shader, long dynamicKeywordKeyId, string dynamicKeywordKey, bool useTexture, bool outlineMrt = false, bool instanced = false)
	{
		Shader = shader;
		DynamicKeywordKeyId = dynamicKeywordKeyId;
		DynamicKeywordKey = dynamicKeywordKey;
		UseTexture = useTexture;
		OutlineMrt = outlineMrt;
		Instanced = instanced;
		_hashCode = HashCode.Combine(shader.Id, dynamicKeywordKeyId, useTexture, outlineMrt, instanced);
	}

	public bool Equals(ShaderVariantKey other)
	{
		if (Shader.Id == other.Shader.Id && DynamicKeywordKeyId == other.DynamicKeywordKeyId && UseTexture == other.UseTexture && OutlineMrt == other.OutlineMrt)
		{
			return Instanced == other.Instanced;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is ShaderVariantKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _hashCode;
	}

	public static bool operator ==(ShaderVariantKey left, ShaderVariantKey right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ShaderVariantKey left, ShaderVariantKey right)
	{
		return !left.Equals(right);
	}
}
