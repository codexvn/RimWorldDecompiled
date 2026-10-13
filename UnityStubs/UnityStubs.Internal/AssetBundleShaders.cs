using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace UnityStubs.Internal;

internal static class AssetBundleShaders
{
	public static Waylith.Rendering.Shader Build(UnityShaderAsset asset, string bundleName, out Waylith.Rendering.Shader[] extraPasses)
	{
		extraPasses = Array.Empty<Waylith.Rendering.Shader>();
		string failure;
		Waylith.Rendering.Shader shader = BuildPass(asset, asset.Pass, asset.Name, bundleName, out failure);
		if (shader == null)
		{
			Warn(asset, bundleName, failure);
			return null;
		}
		List<Waylith.Rendering.Shader> list = new List<Waylith.Rendering.Shader>(asset.Passes.Count - 1);
		for (int i = 1; i < asset.Passes.Count; i++)
		{
			Waylith.Rendering.Shader shader2 = BuildPass(asset, asset.Passes[i], $"{asset.Name} (pass {i + 1})", bundleName, out failure);
			if (shader2 != null)
			{
				list.Add(shader2);
				continue;
			}
			Debug.LogWarning($"AssetBundle '{bundleName}': shader '{asset.Name}' pass {i + 1} can't be drawn: {failure}.");
		}
		extraPasses = list.ToArray();
		return shader;
	}

	private static Waylith.Rendering.Shader BuildPass(UnityShaderAsset asset, UnityShaderPass pass, string key, string bundleName, out string failure)
	{
		UnityShaderBindingPlan unityShaderBindingPlan = UnityShaderBindingPlan.Build(pass);
		if (unityShaderBindingPlan.UniformSize > 512)
		{
			failure = $"its constant buffers need {unityShaderBindingPlan.UniformSize} bytes, the material block holds {512}";
			return null;
		}
		if (unityShaderBindingPlan.StorageBinding.Count > 8)
		{
			failure = $"it needs {unityShaderBindingPlan.StorageBinding.Count} storage buffers (arrays and oversized constant buffers), a material has {8}";
			return null;
		}
		byte[] array = SpirvModule.Rewrite(pass.VertexSpirv, 4, pass, unityShaderBindingPlan, out failure);
		byte[] array2 = ((array == null) ? null : SpirvModule.Rewrite(pass.FragmentSpirv, 8, pass, unityShaderBindingPlan, out failure));
		if (array2 == null)
		{
			return null;
		}
		if (pass.Keywords.Length != 0)
		{
			Debug.LogWarning($"AssetBundle '{bundleName}': shader '{key}' has no keywordless variant, using the one built with [{string.Join(", ", pass.Keywords)}].");
		}
		ShaderProgramDefinition shaderProgramDefinition = BuildDefinition(asset, pass, bundleName);
		MaterialLayout materialLayout = BuildMaterialLayout(asset, pass, unityShaderBindingPlan, shaderProgramDefinition);
		ShaderVertexInputLayout vertexInputLayout = BuildVertexLayout(pass);
		PipelineState pipelineState = shaderProgramDefinition.ToPipelineState();
		if (shaderProgramDefinition.blendMode == ShaderBlendMode.Custom)
		{
			BlendOp blendOp = MapBlendOp(pass.State.BlendOp, asset, bundleName);
			pipelineState = pipelineState with
			{
				Blend = pipelineState.Blend with
				{
					ColorOp = blendOp,
					AlphaOp = blendOp
				}
			};
		}
		return new Waylith.Rendering.Shader(key, asset.Name, shaderProgramDefinition, null, materialLayout, pipelineState, null, vertexInputLayout, new PrecompiledShaderStages(array, array2, ScaleOffsetTextureST: true));
	}

	private static void Warn(UnityShaderAsset asset, string bundleName, string reason)
	{
		Debug.LogWarning($"AssetBundle '{bundleName}': shader '{asset.Name}' can't be used: {reason}.");
	}

	private static ShaderProgramDefinition BuildDefinition(UnityShaderAsset asset, UnityShaderPass pass, string bundleName)
	{
		UnityShaderState state = pass.State;
		bool flag = (int)state.SrcBlend != 1 || (int)state.DstBlend != 0 || (int)state.BlendOp != 0;
		ShaderProgramDefinition shaderProgramDefinition = new ShaderProgramDefinition();
		shaderProgramDefinition.useTexture = true;
		shaderProgramDefinition.flipTextureST = false;
		shaderProgramDefinition.transparent = flag;
		ShaderProgramDefinition shaderProgramDefinition2 = shaderProgramDefinition;
		int num = (int)state.ZTest;
		bool flag2 = ((num == 0 || num == 8) ? true : false);
		shaderProgramDefinition2.depthTest = !flag2;
		shaderProgramDefinition.depthWrite = state.ZWrite != 0f;
		ShaderProgramDefinition shaderProgramDefinition3 = shaderProgramDefinition;
		shaderProgramDefinition3.cullMode = (int)state.Culling switch
		{
			0 => FaceCull.None, 
			1 => FaceCull.Front, 
			_ => FaceCull.Back, 
		};
		shaderProgramDefinition.colorWrite = (int)state.ColorMask != 0;
		shaderProgramDefinition.depthBiasConstantFactor = 0f - state.OffsetUnits;
		shaderProgramDefinition.depthBiasSlopeFactor = 0f - state.OffsetFactor;
		shaderProgramDefinition.blendMode = ((!flag) ? ShaderBlendMode.Opaque : ShaderBlendMode.Custom);
		shaderProgramDefinition.renderQueue = new RenderQueueValue(ParseQueue(asset, pass));
		ShaderProgramDefinition shaderProgramDefinition4 = shaderProgramDefinition;
		if (asset.GrabTexture != null)
		{
			shaderProgramDefinition4.renderQueue = new RenderQueueValue((int)shaderProgramDefinition4.renderQueue + 1);
			shaderProgramDefinition4.sceneGrab = new SceneGrabDeclaration
			{
				name = asset.GrabTexture,
				queue = shaderProgramDefinition4.renderQueue
			};
		}
		if (flag && (!TryMapBlend(state.SrcBlend, out shaderProgramDefinition4.blendSrcFactor) || !TryMapBlend(state.DstBlend, out shaderProgramDefinition4.blendDstFactor)))
		{
			Debug.LogWarning($"AssetBundle '{bundleName}': shader '{asset.Name}' blends with destination alpha (Blend {state.SrcBlend} {state.DstBlend}), falling back to alpha blending.");
			shaderProgramDefinition4.blendMode = ShaderBlendMode.Alpha;
		}
		int num2 = (int)state.StencilComp;
		if ((num2 != 8 && num2 != 0) || state.StencilPass != 0f || state.StencilFail != 0f || state.StencilZFail != 0f)
		{
			shaderProgramDefinition4.stencilEnabled = true;
			shaderProgramDefinition4.stencilCompare = ((num2 == 0) ? StencilCompare.Always : ((StencilCompare)(num2 - 1)));
			shaderProgramDefinition4.stencilPassOp = (StencilOp)(int)state.StencilPass;
			shaderProgramDefinition4.stencilFailOp = (StencilOp)(int)state.StencilFail;
			shaderProgramDefinition4.stencilDepthFailOp = (StencilOp)(int)state.StencilZFail;
			shaderProgramDefinition4.stencilRef = (int)state.StencilRef;
			shaderProgramDefinition4.stencilReadMask = (int)state.StencilReadMask;
			shaderProgramDefinition4.stencilWriteMask = (int)state.StencilWriteMask;
		}
		return shaderProgramDefinition4;
	}

	private static BlendOp MapBlendOp(float unity, UnityShaderAsset asset, string bundleName)
	{
		switch ((int)unity)
		{
		case 0:
			return BlendOp.Add;
		case 1:
			return BlendOp.Subtract;
		case 2:
			return BlendOp.ReverseSubtract;
		case 3:
			return BlendOp.Min;
		case 4:
			return BlendOp.Max;
		default:
			Debug.LogWarning($"AssetBundle '{bundleName}': shader '{asset.Name}' uses BlendOp {unity}, which isn't supported, blending with Add instead.");
			return BlendOp.Add;
		}
	}

	private static bool TryMapBlend(float unity, out BlendFactor factor)
	{
		factor = (int)unity switch
		{
			0 => BlendFactor.Zero, 
			1 => BlendFactor.One, 
			2 => BlendFactor.DstColor, 
			3 => BlendFactor.SrcColor, 
			4 => BlendFactor.OneMinusDstColor, 
			5 => BlendFactor.SrcAlpha, 
			6 => BlendFactor.OneMinusSrcColor, 
			10 => BlendFactor.OneMinusSrcAlpha, 
			_ => (BlendFactor)255, 
		};
		return factor != (BlendFactor)255;
	}

	private static int ParseQueue(UnityShaderAsset asset, UnityShaderPass pass)
	{
		if (!pass.State.Tags.TryGetValue("QUEUE", out var value) && !asset.SubShaderTags.TryGetValue("QUEUE", out value))
		{
			return 2000;
		}
		int num = value.IndexOfAny(new char[2] { '+', '-' }, 1);
		int num2 = ((num < 0) ? value : value.Substring(0, num)).Trim().ToLowerInvariant() switch
		{
			"background" => 1000, 
			"alphatest" => 2450, 
			"geometrylast" => 2500, 
			"transparent" => 3000, 
			"overlay" => 4000, 
			_ => 2000, 
		};
		if (num > 0 && int.TryParse(value.Substring(num).Replace(" ", ""), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
		{
			num2 += result;
		}
		return num2;
	}

	private static MaterialLayout BuildMaterialLayout(UnityShaderAsset asset, UnityShaderPass pass, UnityShaderBindingPlan plan, ShaderProgramDefinition definition)
	{
		SceneGrabPoint sceneGrabPoint = ((definition.sceneGrab == null) ? null : SceneGrabRegistry.Define(definition.sceneGrab.name, definition.sceneGrab.queue));
		List<MaterialTextureSlot> list = new List<MaterialTextureSlot>();
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		HashSet<int> hashSet = new HashSet<int>();
		foreach (UnityShaderBinding binding in pass.Bindings)
		{
			if (binding.Kind == UnityShaderBindingKind.Texture && !dictionary.ContainsKey(binding.Name))
			{
				int count = dictionary.Count;
				dictionary[binding.Name] = count;
				bool vertexVisible = (binding.Stages & 4) != 0;
				ShaderMetadataParser.TryParseTextureDefault(asset.Properties.Find((UnityShaderProperty p) => p.Name == binding.Name)?.DefaultTexture ?? string.Empty, out var textureDefault);
				bool flag = sceneGrabPoint != null && binding.Name == definition.sceneGrab.name;
				list.Add(new MaterialTextureSlot(count, plan.ImageBinding[binding.PackedIndex & 0xFFFFFF], flag ? sceneGrabPoint.TexturePropertyName : binding.Name, flag ? sceneGrabPoint.TexturePropertyId : ShaderPropertyId.GetOrCreate(binding.Name), vertexVisible, binding.HasSeparateSampler ? MaterialTextureKind.SampledImage : MaterialTextureKind.Combined, textureDefault));
				if (binding.HasSeparateSampler && hashSet.Add(binding.PackedSampler & 0xFFFFFF))
				{
					string text = "sampler" + binding.Name;
					list.Add(new MaterialTextureSlot(-1, plan.SamplerBinding[binding.PackedSampler & 0xFFFFFF], text, ShaderPropertyId.GetOrCreate(text), vertexVisible, MaterialTextureKind.Sampler));
				}
			}
		}
		List<MaterialUniformSlot> list2 = new List<MaterialUniformSlot>();
		List<MaterialUniformBlockSlot> list3 = new List<MaterialUniformBlockSlot>();
		int value2;
		foreach (UnityShaderCBuffer cBuffer in pass.CBuffers)
		{
			bool flag2 = plan.MovedBlock.ContainsKey(cBuffer.Name);
			if (!plan.CBufferBase.TryGetValue(cBuffer.Name, out var value) && !flag2)
			{
				continue;
			}
			List<MaterialUniformSlot> list4 = list2;
			if (flag2)
			{
				list2 = new List<MaterialUniformSlot>();
				List<MaterialBlockArraySlot> list5 = new List<MaterialBlockArraySlot>();
				foreach (UnityShaderParam param2 in cBuffer.Params)
				{
					if (param2.ArraySize == 0)
					{
						continue;
					}
					if (param2.IsMatrix && param2.Rows == 4 && param2.Columns == 4)
					{
						list5.Add(new MaterialBlockArraySlot(param2.Offset, ShaderPropertyId.GetOrCreate(param2.Name), MaterialBlockArrayKind.Matrix, param2.ArraySize, 64, 64));
					}
					else if (!param2.IsMatrix)
					{
						value2 = param2.Columns;
						if (value2 >= 1 && value2 <= 4)
						{
							list5.Add(new MaterialBlockArraySlot(param2.Offset, ShaderPropertyId.GetOrCreate(param2.Name), (param2.Columns != 1) ? MaterialBlockArrayKind.Vector : MaterialBlockArrayKind.Float, param2.ArraySize, 16, 4 * param2.Columns));
						}
					}
				}
				list3.Add(new MaterialUniformBlockSlot(ShaderPropertyId.GetOrCreate(UnityShaderBindingPlan.BlockSlotName(cBuffer.Name)), cBuffer.Size, null, list5.ToArray()));
			}
			foreach (UnityShaderParam param in cBuffer.Params)
			{
				if (param.ArraySize > 0 || !TryMapType(param, out var type))
				{
					continue;
				}
				UnityShaderProperty unityShaderProperty = asset.Properties.Find((UnityShaderProperty p) => p.Name == param.Name);
				int value3 = -1;
				if (type == MaterialUniformType.Vector4 && param.Name.EndsWith("_ST", StringComparison.Ordinal))
				{
					type = MaterialUniformType.TextureST;
					if (!dictionary.TryGetValue(param.Name.Substring(0, param.Name.Length - 3), out value3))
					{
						value3 = -1;
					}
				}
				else if (type == MaterialUniformType.Vector4 && param.Name.EndsWith("_TexelSize", StringComparison.Ordinal))
				{
					type = MaterialUniformType.TexelSize;
					if (!dictionary.TryGetValue(param.Name.Substring(0, param.Name.Length - 10), out value3))
					{
						value3 = -1;
					}
				}
				list2.Add(new MaterialUniformSlot(value + param.Offset, type, param.Name, ShaderPropertyId.GetOrCreate(param.Name), DefaultFor(type, unityShaderProperty), value3, unityShaderProperty != null && unityShaderProperty.Type == UnityShaderPropertyType.Color, unityShaderProperty != null));
			}
			if (flag2)
			{
				list3[list3.Count - 1] = list3[list3.Count - 1]with
				{
					Members = list2.ToArray()
				};
				list2 = list4;
			}
		}
		List<MaterialStorageBufferSlot> list6 = new List<MaterialStorageBufferSlot>();
		foreach (KeyValuePair<string, int> item in plan.StorageBinding)
		{
			item.Deconstruct(out var key, out value2);
			string text2 = key;
			int binding2 = value2;
			list6.Add(new MaterialStorageBufferSlot(binding2, text2, ShaderPropertyId.GetOrCreate(text2)));
		}
		return new MaterialLayout(list2.ToArray(), list.ToArray(), Math.Max(16, plan.UniformSize), list6.ToArray(), hasErrors: false, list3.ToArray());
	}

	private static bool TryMapType(UnityShaderParam param, out MaterialUniformType type)
	{
		type = MaterialUniformType.Float;
		if (param.IsMatrix)
		{
			type = ((param.Rows == 4 && param.Columns == 4) ? MaterialUniformType.Matrix4x4 : MaterialUniformType.Matrix3x3);
			bool flag = param.Rows == param.Columns;
			if (flag)
			{
				int rows = param.Rows;
				bool flag2 = (uint)(rows - 3) <= 1u;
				flag = flag2;
			}
			return flag;
		}
		switch (param.Columns)
		{
		case 1:
			type = MaterialUniformType.Float;
			return true;
		case 2:
			type = MaterialUniformType.Vector2;
			return true;
		case 3:
			type = MaterialUniformType.Vector3;
			return true;
		case 4:
			type = MaterialUniformType.Vector4;
			return true;
		default:
			return false;
		}
	}

	private static MaterialUniformValue DefaultFor(MaterialUniformType type, UnityShaderProperty property)
	{
		switch (type)
		{
		case MaterialUniformType.Matrix3x3:
		case MaterialUniformType.Matrix4x4:
			return MaterialUniformValue.FromMatrix(type, System.Numerics.Matrix4x4.Identity);
		case MaterialUniformType.TextureST:
			return MaterialUniformValue.FromVector(type, new System.Numerics.Vector4(0f, 0f, 1f, 1f));
		case MaterialUniformType.Float:
			return MaterialUniformValue.FromFloat((property != null) ? property.Default[0] : 0f);
		default:
			return MaterialUniformValue.FromVector(type, (property == null) ? System.Numerics.Vector4.Zero : new System.Numerics.Vector4(property.Default[0], property.Default[1], property.Default[2], property.Default[3]));
		}
	}

	private static ShaderVertexInputLayout BuildVertexLayout(UnityShaderPass pass)
	{
		List<ShaderVertexInputSlot> list = new List<ShaderVertexInputSlot>();
		foreach (var (num, num2) in pass.VertexChannels)
		{
			if (num2 >= 0 && num >= 0 && num <= 13)
			{
				VertexAttributeSemantic vertexAttributeSemantic = (VertexAttributeSemantic)num;
				List<ShaderVertexInputSlot> list2 = list;
				ShaderVertexInputSlot shaderVertexInputSlot = new ShaderVertexInputSlot();
				shaderVertexInputSlot.location = num2;
				shaderVertexInputSlot.semantic = vertexAttributeSemantic;
				shaderVertexInputSlot.required = vertexAttributeSemantic == VertexAttributeSemantic.Position;
				ShaderVertexInputSlot shaderVertexInputSlot2 = shaderVertexInputSlot;
				shaderVertexInputSlot2.defaultValue = vertexAttributeSemantic switch
				{
					VertexAttributeSemantic.Normal => new System.Numerics.Vector4(0f, 1f, 0f, 0f), 
					VertexAttributeSemantic.Tangent => new System.Numerics.Vector4(1f, 0f, 0f, 1f), 
					VertexAttributeSemantic.Color => System.Numerics.Vector4.One, 
					_ => System.Numerics.Vector4.Zero, 
				};
				list2.Add(shaderVertexInputSlot);
			}
		}
		return new ShaderVertexInputLayout(list.ToArray(), null);
	}
}
