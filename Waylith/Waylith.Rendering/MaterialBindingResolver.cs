using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Waylith.Rendering;

internal static class MaterialBindingResolver
{
	public static ShaderVariantKey CreateShaderVariant(Material material, string globalKeywordKey)
	{
		Shader shader = material.Shader;
		material.GetKeywordState(out string key, out long keyId);
		string text = CombineKeywordKeys(key, globalKeywordKey);
		long dynamicKeywordKeyId = (string.Equals(text, key, StringComparison.Ordinal) ? keyId : ShaderKeywordIdMaker.GetOrCreateId(text));
		return new ShaderVariantKey(shader, dynamicKeywordKeyId, text, shader.UseTexture);
	}

	public static void ResolveMaterialBindingData(Material material, out MaterialUniformBytes uniforms, out MaterialTextureResourceSet<Texture?> resources, out bool usedGlobals)
	{
		ResolveMaterialData(material, default(DrawPropertyRange), null, out uniforms, out resources, out usedGlobals);
	}

	internal static void ResolveMaterialData(Material material, DrawPropertyRange propertyRange, DrawPropertyArena? propertyArena, out MaterialUniformBytes uniforms, out MaterialTextureResourceSet<Texture?> resources, out bool usedGlobals)
	{
		ReadOnlySpan<DrawProperty> overrides = propertyArena?.GetProperties(propertyRange) ?? default(ReadOnlySpan<DrawProperty>);
		MaterialLayout materialLayout = material.Shader.MaterialLayout;
		ReadOnlySpan<DrawProperty> materialProperties = material.PropertiesForReading;
		uniforms = default(MaterialUniformBytes);
		resources = default(MaterialTextureResourceSet<Texture>);
		usedGlobals = false;
		MaterialTextureSlot[] textureSlots = materialLayout.TextureSlots;
		for (int i = 0; i < textureSlots.Length; i++)
		{
			MaterialTextureSlot materialTextureSlot = textureSlots[i];
			if (materialTextureSlot.Kind != MaterialTextureKind.Sampler)
			{
				ResolveTexture(overrides, materialProperties, materialTextureSlot.PropertyId, ref usedGlobals, out Texture texture);
				resources.Set(materialTextureSlot.ResourceSlot, texture);
			}
		}
		MaterialUniformSlot[] uniformSlots = materialLayout.UniformSlots;
		MaterialUniformSlot slot;
		MaterialUniformValue materialUniformValue;
		MaterialUniformValue value;
		for (int i = 0; i < uniformSlots.Length; value = materialUniformValue, uniforms.Write(slot.Offset, value), i++)
		{
			slot = uniformSlots[i];
			if (slot.Type == MaterialUniformType.TexelSize && slot.TextureResourceSlot >= 0)
			{
				Texture texture2 = resources[slot.TextureResourceSlot];
				if (texture2 != null)
				{
					materialUniformValue = TexelSize(texture2.width, texture2.height);
					continue;
				}
			}
			materialUniformValue = ResolveUniform(slot, overrides, materialProperties, material, ref usedGlobals);
		}
	}

	internal static void ResolveUniformBlock(Material material, MaterialUniformBlockSlot block, Span<byte> image)
	{
		image.Clear();
		MaterialLayout materialLayout = material.Shader.MaterialLayout;
		ReadOnlySpan<DrawProperty> materialProperties = material.PropertiesForReading;
		bool usedGlobals = false;
		MaterialUniformBytes reference = default(MaterialUniformBytes);
		Span<byte> span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref reference, 1));
		MaterialUniformSlot[] members = block.Members;
		for (int i = 0; i < members.Length; i++)
		{
			MaterialUniformSlot slot = members[i];
			int std140Size = MaterialLayout.GetStd140Size(slot.Type);
			if (slot.Offset < 0 || slot.Offset + std140Size > image.Length)
			{
				continue;
			}
			MaterialUniformValue value;
			if (slot.Type == MaterialUniformType.TexelSize && slot.TextureResourceSlot >= 0)
			{
				Texture texture = ResolveSlotTexture(materialLayout, slot.TextureResourceSlot, materialProperties);
				if (texture != null)
				{
					value = TexelSize(texture.width, texture.height);
					goto IL_00da;
				}
			}
			value = ResolveUniform(slot, default(ReadOnlySpan<DrawProperty>), materialProperties, material, ref usedGlobals);
			goto IL_00da;
			IL_00da:
			reference.Write(0, value);
			span.Slice(0, std140Size).CopyTo(image.Slice(slot.Offset));
		}
		MaterialBlockArraySlot[] arrays = block.Arrays;
		for (int i = 0; i < arrays.Length; i++)
		{
			MaterialBlockArraySlot slot2 = arrays[i];
			if (slot2.Offset < 0 || slot2.Offset >= image.Length)
			{
				continue;
			}
			Span<byte> destination = image.Slice(slot2.Offset);
			if ((material.TryCopyArray(in slot2, destination) || GlobalShaderState.TryCopyArray(slot2.PropertyId, destination, slot2.Stride, slot2.ElementBytes, slot2.Length)) && slot2.Kind == MaterialBlockArrayKind.Matrix)
			{
				Span<Matrix4x4> span2 = MemoryMarshal.Cast<byte, Matrix4x4>(destination.Slice(0, System.Math.Min(destination.Length, slot2.Length * 64) / 64 * 64));
				for (int j = 0; j < span2.Length; j++)
				{
					span2[j] = Matrix4x4.Transpose(span2[j]);
				}
			}
		}
	}

	private static Texture? ResolveSlotTexture(MaterialLayout layout, int resourceSlot, ReadOnlySpan<DrawProperty> materialProperties)
	{
		MaterialTextureSlot[] textureSlots = layout.TextureSlots;
		for (int i = 0; i < textureSlots.Length; i++)
		{
			MaterialTextureSlot materialTextureSlot = textureSlots[i];
			if (materialTextureSlot.Kind != MaterialTextureKind.Sampler && materialTextureSlot.ResourceSlot == resourceSlot)
			{
				bool usedGlobals = false;
				ResolveTexture(default(ReadOnlySpan<DrawProperty>), materialProperties, materialTextureSlot.PropertyId, ref usedGlobals, out Texture texture);
				return texture;
			}
		}
		return null;
	}

	private static MaterialUniformValue ResolveUniform(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, Material material, ref bool usedGlobals)
	{
		return slot.Type switch
		{
			MaterialUniformType.Matrix3x3 => MaterialUniformValue.FromMatrix(slot.Type, ResolveMatrix(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.Matrix4x4 => MaterialUniformValue.FromMatrix(slot.Type, ResolveMatrix(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.Vector2 => MaterialUniformValue.FromVector(slot.Type, ResolveVector(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.Vector3 => MaterialUniformValue.FromVector(slot.Type, ResolveVector(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.Vector4 => MaterialUniformValue.FromVector(slot.Type, ResolveVector(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.TexelSize => MaterialUniformValue.FromVector(slot.Type, ResolveVector(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.TextureST => MaterialUniformValue.FromVector(slot.Type, ResolveTextureST(slot, overrides, materialProperties, material.Shader.FlipTextureST, material.Shader.PrecompiledStages?.ScaleOffsetTextureST ?? false, ref usedGlobals)), 
			MaterialUniformType.Int => MaterialUniformValue.FromInt(ResolveInt(slot, overrides, materialProperties, ref usedGlobals)), 
			MaterialUniformType.Float => MaterialUniformValue.FromFloat(ResolveFloat(slot, overrides, materialProperties, ref usedGlobals)), 
			_ => slot.DefaultValue, 
		};
	}

	public static MaterialUniformValue TexelSize(int width, int height)
	{
		return MaterialUniformValue.FromVector(MaterialUniformType.TexelSize, new Vector4(1f / (float)System.Math.Max(1, width), 1f / (float)System.Math.Max(1, height), width, height));
	}

	private static Vector4 ResolveTextureST(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, bool flipV, bool scaleFirst, ref bool usedGlobals)
	{
		Vector4 result = ResolveVector(slot, overrides, materialProperties, ref usedGlobals);
		if (flipV)
		{
			result.Y = 1f - result.Y;
			result.W = 0f - result.W;
		}
		if (!scaleFirst)
		{
			return result;
		}
		return new Vector4(result.Z, result.W, result.X, result.Y);
	}

	private static Vector4 ResolveVector(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, ref bool usedGlobals)
	{
		if (DrawPropertySpanLookup.TryGetVectorOrColor(overrides, slot.PropertyId, out var value))
		{
			return value;
		}
		if (DrawPropertySpanLookup.TryGetVectorOrColor(materialProperties, slot.PropertyId, out var value2))
		{
			return value2;
		}
		if (!slot.HasAuthoredDefault && GlobalShaderState.TryGetVector(slot.PropertyId, out var value3))
		{
			usedGlobals = true;
			return value3;
		}
		return slot.DefaultValue.Vector4;
	}

	private static float ResolveFloat(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, ref bool usedGlobals)
	{
		if (DrawPropertySpanLookup.TryGetFloat(overrides, slot.PropertyId, out var value))
		{
			return value;
		}
		if (DrawPropertySpanLookup.TryGetFloat(materialProperties, slot.PropertyId, out value))
		{
			return value;
		}
		if (!slot.HasAuthoredDefault && GlobalShaderState.TryGetFloat(slot.PropertyId, out value))
		{
			usedGlobals = true;
			return value;
		}
		return slot.DefaultValue.Float;
	}

	private static int ResolveInt(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, ref bool usedGlobals)
	{
		if (DrawPropertySpanLookup.TryGetInt(overrides, slot.PropertyId, out var value))
		{
			return value;
		}
		if (DrawPropertySpanLookup.TryGetInt(materialProperties, slot.PropertyId, out value))
		{
			return value;
		}
		return slot.DefaultValue.Int;
	}

	private static Matrix4x4 ResolveMatrix(MaterialUniformSlot slot, ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, ref bool usedGlobals)
	{
		if (DrawPropertySpanLookup.TryGetMatrix(overrides, slot.PropertyId, out var value))
		{
			return value;
		}
		if (DrawPropertySpanLookup.TryGetMatrix(materialProperties, slot.PropertyId, out value))
		{
			return value;
		}
		if (!slot.HasAuthoredDefault && GlobalShaderState.TryGetMatrix(slot.PropertyId, out value))
		{
			usedGlobals = true;
			return value;
		}
		return slot.DefaultValue.Matrix4x4;
	}

	private static void ResolveTexture(ReadOnlySpan<DrawProperty> overrides, ReadOnlySpan<DrawProperty> materialProperties, int propertyId, ref bool usedGlobals, out Texture? texture)
	{
		if (!DrawPropertySpanLookup.TryGetTexture<Texture>(overrides, propertyId, out texture) && !DrawPropertySpanLookup.TryGetTexture<Texture>(materialProperties, propertyId, out texture))
		{
			if (GlobalShaderState.TryGetTexture(propertyId, out texture) && texture != null)
			{
				usedGlobals = true;
			}
			else
			{
				texture = null;
			}
		}
	}

	internal static string CombineKeywordKeys(string materialKeywordKey, string? globalKeywordKey)
	{
		if (string.IsNullOrEmpty(materialKeywordKey))
		{
			return globalKeywordKey ?? string.Empty;
		}
		if (string.IsNullOrEmpty(globalKeywordKey))
		{
			return materialKeywordKey;
		}
		return materialKeywordKey + ";" + globalKeywordKey;
	}
}
