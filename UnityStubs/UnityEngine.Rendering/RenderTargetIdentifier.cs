using System;

namespace UnityEngine.Rendering;

public struct RenderTargetIdentifier : IEquatable<RenderTargetIdentifier>
{
	public const int AllDepthSlices = -1;

	internal BuiltinRenderTextureType Type { get; }

	internal int NameId { get; }

	internal Texture Texture { get; }

	public RenderTargetIdentifier(BuiltinRenderTextureType type)
	{
		Type = type;
		NameId = 0;
		Texture = null;
	}

	public RenderTargetIdentifier(int nameID)
	{
		Type = BuiltinRenderTextureType.PropertyName;
		NameId = nameID;
		Texture = null;
	}

	public RenderTargetIdentifier(string name)
		: this(Shader.PropertyToID(name))
	{
	}

	public RenderTargetIdentifier(Texture tex)
	{
		Type = ((tex is RenderTexture) ? BuiltinRenderTextureType.RenderTexture : BuiltinRenderTextureType.BindableTexture);
		NameId = 0;
		Texture = tex;
	}

	public static implicit operator RenderTargetIdentifier(BuiltinRenderTextureType type)
	{
		return new RenderTargetIdentifier(type);
	}

	public static implicit operator RenderTargetIdentifier(int nameID)
	{
		return new RenderTargetIdentifier(nameID);
	}

	public static implicit operator RenderTargetIdentifier(Texture tex)
	{
		return new RenderTargetIdentifier(tex);
	}

	public bool Equals(RenderTargetIdentifier rhs)
	{
		if (Type == rhs.Type && NameId == rhs.NameId)
		{
			return (object)Texture == rhs.Texture;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is RenderTargetIdentifier rhs)
		{
			return Equals(rhs);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Type, NameId, Texture);
	}

	public static bool operator ==(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
	{
		return !lhs.Equals(rhs);
	}
}
