using System;
using System.Collections.Generic;

namespace Waylith.Rendering;

internal sealed class MaterialBindingCache : IDisposable
{
	private sealed class Entry
	{
		private readonly List<MaterialBinding> bindings = new List<MaterialBinding>();

		public bool TryGet(ShaderVariantKey shaderVariant, out MaterialBinding binding)
		{
			for (int i = 0; i < bindings.Count; i++)
			{
				if (bindings[i].ShaderVariant.Equals(shaderVariant))
				{
					binding = bindings[i];
					return true;
				}
			}
			binding = null;
			return false;
		}

		public void Add(MaterialBinding binding)
		{
			bindings.Add(binding);
		}
	}

	private readonly Dictionary<int, Entry> entries = new Dictionary<int, Entry>();

	private readonly MaterialReleaseListener releaseListener = new MaterialReleaseListener();

	private int nextBindingId = 1;

	private static readonly int BlendSrcId = ShaderPropertyId.GetOrCreate("_BlendSrc");

	private static readonly int BlendDstId = ShaderPropertyId.GetOrCreate("_BlendDst");

	private static readonly int ZTestModeId = ShaderPropertyId.GetOrCreate("_ZTestMode");

	private static readonly int ZWriteModeId = ShaderPropertyId.GetOrCreate("_ZWriteMode");

	private static readonly int CullModeId = ShaderPropertyId.GetOrCreate("_CullMode");

	public void Dispose()
	{
		releaseListener.Dispose();
		entries.Clear();
	}

	public bool TryGetOrCreate(Material material, string globalKeywordKey, out MaterialBinding binding)
	{
		int materialId;
		while (releaseListener.TryDequeueReleased(out materialId))
		{
			entries.Remove(materialId);
		}
		ShaderVariantKey shaderVariant = MaterialBindingResolver.CreateShaderVariant(material, globalKeywordKey);
		if (!entries.TryGetValue(material.Id, out Entry value))
		{
			value = new Entry();
			entries.Add(material.Id, value);
		}
		MaterialLayout materialLayout = material.Shader.MaterialLayout;
		if (value.TryGet(shaderVariant, out binding))
		{
			if (binding.MaterialVersion != material.Version || binding.GlobalRegistrationVersion != GlobalShaderState.RegistrationVersion || (binding.UsesGlobals && binding.GlobalVersion != GlobalShaderState.Version) || binding.MaterialLayout != materialLayout)
			{
				MaterialBindingResolver.ResolveMaterialBindingData(material, out MaterialUniformBytes uniforms, out MaterialTextureResourceSet<Texture> resources, out bool usedGlobals);
				binding.Update(material.Version, material.DebugName, material.Shader, materialLayout, uniforms, resources, material.RenderQueue, shaderVariant, ResolvePipelineState(material), material.WrapModeOverride, usedGlobals);
			}
			binding.UpdateStorageArrays(material.StorageArrays);
			return true;
		}
		MaterialBindingResolver.ResolveMaterialBindingData(material, out MaterialUniformBytes uniforms2, out MaterialTextureResourceSet<Texture> resources2, out bool usedGlobals2);
		binding = new MaterialBinding(nextBindingId++, material.Id, material.Version, material.DebugName, material.Shader, materialLayout, uniforms2, resources2, material.RenderQueue, shaderVariant, ResolvePipelineState(material), material.WrapModeOverride, usedGlobals2);
		binding.UpdateStorageArrays(material.StorageArrays);
		value.Add(binding);
		return true;
	}

	public void Clear()
	{
		entries.Clear();
	}

	private static PipelineState ResolvePipelineState(Material material)
	{
		PipelineState pipelineState = material.Shader.PipelineState;
		BlendState blend = pipelineState.Blend;
		DepthState depthState = pipelineState.Depth;
		FaceCull cull = pipelineState.Cull;
		bool flag = false;
		float value;
		bool flag2 = material.TryGetFloat(BlendSrcId, out value);
		float value2;
		bool flag3 = material.TryGetFloat(BlendDstId, out value2);
		if (flag2 || flag3)
		{
			BlendFactor blendFactor = ToBlendFactor(flag2 ? ((int)value) : 5);
			BlendFactor blendFactor2 = ToBlendFactor(flag3 ? ((int)value2) : 10);
			blend = new BlendState(Enabled: true, blendFactor, blendFactor2, blend.ColorOp, blendFactor, blendFactor2, blend.AlphaOp);
			flag = true;
		}
		if (material.TryGetFloat(ZTestModeId, out var value3))
		{
			int num = (int)value3;
			depthState = depthState with
			{
				Test = (num != 0 && num != 8)
			};
			flag = true;
		}
		if (material.TryGetFloat(ZWriteModeId, out var value4))
		{
			depthState = depthState with
			{
				Write = ((int)value4 != 0)
			};
			flag = true;
		}
		if (material.TryGetFloat(CullModeId, out var value5))
		{
			cull = ToFaceCull((int)value5);
			flag = true;
		}
		if (!flag)
		{
			return pipelineState;
		}
		return pipelineState with
		{
			Blend = blend,
			Depth = depthState,
			Cull = cull
		};
	}

	private static FaceCull ToFaceCull(int unityCullMode)
	{
		return unityCullMode switch
		{
			1 => FaceCull.Front, 
			2 => FaceCull.Back, 
			_ => FaceCull.None, 
		};
	}

	private static BlendFactor ToBlendFactor(int unityBlendMode)
	{
		return unityBlendMode switch
		{
			0 => BlendFactor.Zero, 
			1 => BlendFactor.One, 
			2 => BlendFactor.DstColor, 
			3 => BlendFactor.SrcColor, 
			4 => BlendFactor.OneMinusDstColor, 
			5 => BlendFactor.SrcAlpha, 
			6 => BlendFactor.OneMinusSrcColor, 
			10 => BlendFactor.OneMinusSrcAlpha, 
			_ => BlendFactor.One, 
		};
	}
}
