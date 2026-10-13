using System;

namespace Waylith.Rendering;

public sealed class MaterialLayout
{
	public const int MaxUniformBytes = 512;

	public const int MaxTextureSlots = 16;

	public const int MaxStorageBufferSlots = 8;

	public MaterialUniformSlot[] UniformSlots { get; }

	public MaterialTextureSlot[] TextureSlots { get; }

	public MaterialStorageBufferSlot[] StorageBufferSlots { get; }

	public MaterialUniformBlockSlot[] UniformBlocks { get; }

	public int UniformSize { get; }

	public int VersionHash { get; }

	public bool HasErrors { get; }

	public bool HasStorageBuffers => StorageBufferSlots.Length != 0;

	public MaterialLayout(MaterialUniformSlot[] uniformSlots, MaterialTextureSlot[] textureSlots, int uniformSize, MaterialStorageBufferSlot[]? storageBufferSlots = null, bool hasErrors = false, MaterialUniformBlockSlot[]? uniformBlocks = null)
	{
		UniformBlocks = uniformBlocks ?? Array.Empty<MaterialUniformBlockSlot>();
		UniformSlots = uniformSlots ?? Array.Empty<MaterialUniformSlot>();
		TextureSlots = textureSlots ?? Array.Empty<MaterialTextureSlot>();
		StorageBufferSlots = storageBufferSlots ?? Array.Empty<MaterialStorageBufferSlot>();
		UniformSize = System.Math.Max(0, uniformSize);
		HasErrors = ValidateSlots(UniformSlots, TextureSlots, StorageBufferSlots, UniformSize) || hasErrors;
		VersionHash = ComputeVersionHash(UniformSlots, TextureSlots, StorageBufferSlots, UniformSize);
	}

	public static MaterialLayout Empty()
	{
		return new MaterialLayout(Array.Empty<MaterialUniformSlot>(), Array.Empty<MaterialTextureSlot>(), 0);
	}

	public static int GetStd140Size(MaterialUniformType type)
	{
		return type switch
		{
			MaterialUniformType.Matrix3x3 => 48, 
			MaterialUniformType.Matrix4x4 => 64, 
			MaterialUniformType.Vector2 => 8, 
			MaterialUniformType.Vector3 => 16, 
			MaterialUniformType.Vector4 => 16, 
			MaterialUniformType.TextureST => 16, 
			MaterialUniformType.TexelSize => 16, 
			MaterialUniformType.Int => 4, 
			MaterialUniformType.Float => 4, 
			_ => throw new ArgumentOutOfRangeException("type"), 
		};
	}

	private static bool ValidateSlots(ReadOnlySpan<MaterialUniformSlot> uniformSlots, ReadOnlySpan<MaterialTextureSlot> textureSlots, ReadOnlySpan<MaterialStorageBufferSlot> storageBufferSlots, int uniformSize)
	{
		bool flag = false;
		if ((uint)uniformSize > 512u)
		{
			flag |= LogError("uniformSize", $"Material uniform block is {uniformSize} bytes, max is {512}.");
		}
		ReadOnlySpan<MaterialUniformSlot> readOnlySpan = uniformSlots;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			MaterialUniformSlot materialUniformSlot = readOnlySpan[i];
			int num = materialUniformSlot.Offset + GetStd140Size(materialUniformSlot.Type);
			if (materialUniformSlot.Offset < 0 || num > 512)
			{
				flag |= LogError("uniformSlots", $"Material uniform '{materialUniformSlot.PropertyName}' at {materialUniformSlot.Offset}..{num} is outside 0..{512}.");
			}
		}
		ReadOnlySpan<MaterialTextureSlot> readOnlySpan2 = textureSlots;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			MaterialTextureSlot materialTextureSlot = readOnlySpan2[i];
			if (materialTextureSlot.Kind != MaterialTextureKind.Sampler && (uint)materialTextureSlot.ResourceSlot >= 16u)
			{
				flag |= LogError("textureSlots", $"Material texture slot {materialTextureSlot.ResourceSlot} is outside 0..{15}.");
			}
			if (materialTextureSlot.Binding <= 0)
			{
				flag |= LogError("textureSlots", $"Material texture binding {materialTextureSlot.Binding} must be positive.");
			}
		}
		if (storageBufferSlots.Length > 8)
		{
			flag |= LogError("storageBufferSlots", $"Material declares {storageBufferSlots.Length} storage buffer slots, max is {8}.");
		}
		ReadOnlySpan<MaterialStorageBufferSlot> readOnlySpan3 = storageBufferSlots;
		for (int i = 0; i < readOnlySpan3.Length; i++)
		{
			MaterialStorageBufferSlot materialStorageBufferSlot = readOnlySpan3[i];
			if (materialStorageBufferSlot.Binding <= 0)
			{
				flag |= LogError("storageBufferSlots", $"Material storage buffer '{materialStorageBufferSlot.PropertyName}' binding {materialStorageBufferSlot.Binding} must be positive.");
			}
		}
		return flag;
	}

	private static bool LogError(string name, string message)
	{
		RenderLog.Log.Error(name, message, "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\DrawCallCore\\MaterialLayout.cs", 237);
		return true;
	}

	private static int ComputeVersionHash(ReadOnlySpan<MaterialUniformSlot> uniformSlots, ReadOnlySpan<MaterialTextureSlot> textureSlots, ReadOnlySpan<MaterialStorageBufferSlot> storageBufferSlots, int uniformSize)
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(uniformSize);
		ReadOnlySpan<MaterialUniformSlot> readOnlySpan = uniformSlots;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			MaterialUniformSlot value = readOnlySpan[i];
			hashCode.Add(value);
		}
		ReadOnlySpan<MaterialTextureSlot> readOnlySpan2 = textureSlots;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			MaterialTextureSlot value2 = readOnlySpan2[i];
			hashCode.Add(value2);
		}
		ReadOnlySpan<MaterialStorageBufferSlot> readOnlySpan3 = storageBufferSlots;
		for (int i = 0; i < readOnlySpan3.Length; i++)
		{
			MaterialStorageBufferSlot value3 = readOnlySpan3[i];
			hashCode.Add(value3);
		}
		return hashCode.ToHashCode();
	}
}
