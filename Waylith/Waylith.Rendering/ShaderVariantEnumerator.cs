using System;
using System.Collections.Generic;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public static class ShaderVariantEnumerator
{
	private static readonly ComputeShaderJob[] ComputeKernels = new ComputeShaderJob[11]
	{
		new ComputeShaderJob("Compute/EncodeBC1Fast.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/EncodeBC4Fast.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/EncodeBC5Fast.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/EncodeBC7Fast.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/MipmapDownsample.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/MipmapKaiser.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/MipmapAlphaHistogram.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/MipmapSolveCoverage.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/MipmapApplyAlphaScale.comp.hlsl", new string[1] { "main" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/GpuProbe.comp.hlsl", new string[2] { "AluProbeCS", "BandwidthProbeCS" }, ShaderSourceLanguage.Hlsl),
		new ComputeShaderJob("Compute/Nv12ToRgba.comp.glsl", new string[1] { "main" }, ShaderSourceLanguage.Glsl)
	};

	private static readonly FixedGraphicsShaderJob[] PostFxShaders = new FixedGraphicsShaderJob[1]
	{
		new FixedGraphicsShaderJob("PostFxComposite.frag.glsl", "PostFxComposite.frag.glsl", RuntimeShaderStage.Fragment)
	};

	private static readonly InlineShaderJob[] InlineShaders = new InlineShaderJob[4]
	{
		new InlineShaderJob("#version 450\nlayout(location = 0) out vec2 vUv;\nvoid main() {\n    vUv = vec2((gl_VertexIndex << 1) & 2, gl_VertexIndex & 2);\n    gl_Position = vec4(vUv * 2.0 - 1.0, 0.0, 1.0);\n}\n", "Fullscreen.vert", RuntimeShaderStage.Vertex),
		new InlineShaderJob("#version 450\nlayout(location = 0) in vec2 vUv;\nlayout(location = 0) out vec4 outColor;\nlayout(set = 0, binding = 0) uniform sampler2D uSource;\nlayout(push_constant) uniform Push { vec4 srcUvRect; vec4 tint; } pc;\nvoid main() {\n    vec2 uv = pc.srcUvRect.xy + vUv * pc.srcUvRect.zw;\n    outColor = texture(uSource, uv) * pc.tint;\n}\n", "RenderTextureBlit.frag", RuntimeShaderStage.Fragment),
		new InlineShaderJob("#version 450\nvoid main() {}", "DepthOnly.frag", RuntimeShaderStage.Fragment),
		new InlineShaderJob("#version 450\nlayout(location = 1) in vec2 vUv;\nlayout(set = 2, binding = 1) uniform sampler2D _MainTex;\nvoid main() {\n    if (texture(_MainTex, vUv).a < 0.5) discard;\n}", "DepthOnlyAlphaTest.frag", RuntimeShaderStage.Fragment)
	};

	public static ShaderWarmupPlan Enumerate(IReadOnlyList<ShaderKeywordContribution> keywordContributions, IReadOnlyList<string> globalKeywordKeys)
	{
		ShaderWarmupPlan shaderWarmupPlan = new ShaderWarmupPlan();
		HashSet<(string, RuntimeShaderStage, string)> dedup = new HashSet<(string, RuntimeShaderStage, string)>();
		Dictionary<string, bool> instancedCache = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
		Dictionary<string, List<string>> contributionsByShader = IndexContributions(keywordContributions);
		IReadOnlyList<string> readOnlyList2;
		if (globalKeywordKeys == null || globalKeywordKeys.Count <= 0)
		{
			IReadOnlyList<string> readOnlyList = new _003C_003Ez__ReadOnlySingleElementList<string>(string.Empty);
			readOnlyList2 = readOnlyList;
		}
		else
		{
			readOnlyList2 = globalKeywordKeys;
		}
		IReadOnlyList<string> readOnlyList3 = readOnlyList2;
		foreach (Shader allShader in ShaderRegistry.AllShaders)
		{
			if (allShader.PrecompiledStages != null)
			{
				continue;
			}
			List<string> list = GatherMaterialKeywordSets(allShader, contributionsByShader);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (string item in readOnlyList3)
			{
				foreach (string item2 in list)
				{
					hashSet.Add(MaterialBindingResolver.CombineKeywordKeys(item2, item));
				}
			}
			foreach (string item3 in hashSet)
			{
				EmitShaderVariants(shaderWarmupPlan, dedup, instancedCache, allShader, item3);
				Dictionary<string, string> passShaderOverrides = allShader.Program.passShaderOverrides;
				if (passShaderOverrides == null)
				{
					continue;
				}
				foreach (string value in passShaderOverrides.Values)
				{
					if (ShaderRegistry.TryResolve(value, out Shader shader))
					{
						EmitShaderVariants(shaderWarmupPlan, dedup, instancedCache, shader, item3);
					}
				}
			}
		}
		shaderWarmupPlan.Compute.AddRange(ComputeKernels);
		shaderWarmupPlan.FixedGraphics.AddRange(PostFxShaders);
		shaderWarmupPlan.Inline.AddRange(InlineShaders);
		return shaderWarmupPlan;
	}

	private static void EmitShaderVariants(ShaderWarmupPlan plan, HashSet<(string, RuntimeShaderStage, string)> dedup, Dictionary<string, bool> instancedCache, Shader shader, string dynamicKey)
	{
		bool useTexture = shader.UseTexture;
		int num = ((!VertexSupportsInstancing(shader.VertexShaderFileName, instancedCache)) ? 1 : 2);
		for (int i = 0; i < num; i++)
		{
			bool instanced = i == 1;
			AddGraphics(plan, dedup, shader.VertexShaderFileName, RuntimeShaderStage.Vertex, new ShaderVariantKey(shader, 0L, dynamicKey, useTexture, outlineMrt: false, instanced));
			AddGraphics(plan, dedup, shader.FragmentShaderFileName, RuntimeShaderStage.Fragment, new ShaderVariantKey(shader, 0L, dynamicKey, useTexture, outlineMrt: false, instanced));
			AddGraphics(plan, dedup, shader.FragmentShaderFileName, RuntimeShaderStage.Fragment, new ShaderVariantKey(shader, 0L, dynamicKey, useTexture, outlineMrt: true, instanced));
		}
	}

	private static void AddGraphics(ShaderWarmupPlan plan, HashSet<(string, RuntimeShaderStage, string)> dedup, string file, RuntimeShaderStage stage, ShaderVariantKey variant)
	{
		if (!string.IsNullOrWhiteSpace(file))
		{
			string item = ShaderModuleCache.BuildDefineBlock(variant, stage);
			if (dedup.Add((file, stage, item)))
			{
				plan.Graphics.Add(new GraphicsShaderJob(file, stage, variant));
			}
		}
	}

	private static bool VertexSupportsInstancing(string vertexFile, Dictionary<string, bool> cache)
	{
		if (string.IsNullOrWhiteSpace(vertexFile))
		{
			return false;
		}
		if (cache.TryGetValue(vertexFile, out var value))
		{
			return value;
		}
		bool flag = false;
		try
		{
			string text = ShaderSourceLoader.ReadShaderSource(vertexFile);
			flag = text.Contains("INSTANCED", StringComparison.Ordinal) || text.Contains("gl_InstanceIndex", StringComparison.Ordinal);
		}
		catch
		{
		}
		cache[vertexFile] = flag;
		return flag;
	}

	private static Dictionary<string, List<string>> IndexContributions(IReadOnlyList<ShaderKeywordContribution> contributions)
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>(StringComparer.Ordinal);
		foreach (ShaderKeywordContribution contribution in contributions)
		{
			if (!string.IsNullOrWhiteSpace(contribution.ShaderKey) && !string.IsNullOrWhiteSpace(contribution.Keywords))
			{
				if (!dictionary.TryGetValue(contribution.ShaderKey, out var value))
				{
					value = (dictionary[contribution.ShaderKey] = new List<string>());
				}
				value.Add(contribution.Keywords);
			}
		}
		return dictionary;
	}

	private static List<string> GatherMaterialKeywordSets(Shader shader, Dictionary<string, List<string>> contributionsByShader)
	{
		List<string> list = new List<string> { string.Empty };
		if (contributionsByShader.TryGetValue(shader.Key, out List<string> value))
		{
			foreach (string item in value)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		if (!string.Equals(shader.ShaderPath, shader.Key, StringComparison.Ordinal) && contributionsByShader.TryGetValue(shader.ShaderPath, out List<string> value2))
		{
			foreach (string item2 in value2)
			{
				if (!list.Contains(item2))
				{
					list.Add(item2);
				}
			}
		}
		return list;
	}
}
