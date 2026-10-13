using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;

namespace Waylith.Rendering;

public sealed class ShaderModuleCache : IDisposable
{
	public readonly record struct MeshSamplerKey(FilterMode FilterMode, TextureWrapMode WrapMode, int MinimumMipmapLevel, int MipMapBiasBits, int MaxAnisotropyBits);

	private sealed record SourceIdentifiers(HashSet<string>? Names);

	private readonly DeviceEx device;

	private readonly Dictionary<ShaderVariantKey, VkShaderModule> vertexShaders = new Dictionary<ShaderVariantKey, VkShaderModule>();

	private readonly Dictionary<ShaderVariantKey, VkShaderModule> fragmentShaders = new Dictionary<ShaderVariantKey, VkShaderModule>();

	private readonly HashSet<ShaderVariantKey> failedVertexVariants = new HashSet<ShaderVariantKey>();

	private readonly HashSet<ShaderVariantKey> failedFragmentVariants = new HashSet<ShaderVariantKey>();

	private VkShaderModule _depthOnlyFragmentShader;

	private VkShaderModule _depthOnlyAlphaTestFragmentShader;

	private VkShaderModule _magentaVertexShader;

	private VkShaderModule _magentaFragmentShader;

	private const string MagentaVertexSource = "#version 450\nlayout(location = 0) in vec3 inPosition;\nlayout(push_constant) uniform Push { mat4 modelMatrix; } push;\nlayout(set = 0, binding = 0) uniform VertexUniforms { mat4 ViewProj; } uVertex;\nvoid main() { gl_Position = uVertex.ViewProj * (push.modelMatrix * vec4(inPosition, 1.0)); }\n";

	private const string MagentaFragmentSource = "#version 450\nlayout(location = 0) out vec4 outColor;\nvoid main() { outColor = vec4(1.0, 0.0, 1.0, 1.0); }\n";

	internal const string DepthOnlyFragmentName = "DepthOnly.frag";

	internal const string DepthOnlyFragmentSource = "#version 450\nvoid main() {}";

	internal const string DepthOnlyAlphaTestFragmentName = "DepthOnlyAlphaTest.frag";

	internal const string DepthOnlyAlphaTestFragmentSource = "#version 450\nlayout(location = 1) in vec2 vUv;\nlayout(set = 2, binding = 1) uniform sampler2D _MainTex;\nvoid main() {\n    if (texture(_MainTex, vUv).a < 0.5) discard;\n}";

	private static readonly ConditionalWeakTable<string, SourceIdentifiers> IdentifiersBySource = new ConditionalWeakTable<string, SourceIdentifiers>();

	public ShaderModuleCache(DeviceEx device)
	{
		this.device = device;
	}

	private static string LoadShaderSource(string fileName)
	{
		return ShaderSourceLoader.ReadShaderSource(fileName);
	}

	public VkShaderModule GetOrCreateVertexShader(ShaderVariantKey variant, out bool usedFallback)
	{
		if (vertexShaders.TryGetValue(variant, out var value))
		{
			usedFallback = false;
			return value;
		}
		if (failedVertexVariants.Contains(variant))
		{
			usedFallback = true;
			return GetOrCreateMagentaVertexShader();
		}
		PrecompiledShaderStages precompiledStages = variant.Shader.PrecompiledStages;
		if ((object)precompiledStages != null)
		{
			value = RuntimeShaderCompiler.CreateShaderModule(device, precompiledStages.Vertex, variant.Shader.Name + ".vert");
			vertexShaders.Add(variant, value);
			usedFallback = false;
			return value;
		}
		var (source, sourceName) = BuildVariantSource(variant, RuntimeShaderStage.Vertex);
		if (!TryCompileShaderModule(device, source, RuntimeShaderStage.Vertex, sourceName, out value))
		{
			failedVertexVariants.Add(variant);
			usedFallback = true;
			return GetOrCreateMagentaVertexShader();
		}
		vertexShaders.Add(variant, value);
		usedFallback = false;
		return value;
	}

	public VkShaderModule GetOrCreateDepthOnlyFragmentShader()
	{
		if (!_depthOnlyFragmentShader.IsNull())
		{
			return _depthOnlyFragmentShader;
		}
		_depthOnlyFragmentShader = CompileShaderModule(device, "#version 450\nvoid main() {}", RuntimeShaderStage.Fragment, "DepthOnly.frag");
		return _depthOnlyFragmentShader;
	}

	public VkShaderModule GetOrCreateDepthOnlyAlphaTestFragmentShader()
	{
		if (!_depthOnlyAlphaTestFragmentShader.IsNull())
		{
			return _depthOnlyAlphaTestFragmentShader;
		}
		_depthOnlyAlphaTestFragmentShader = CompileShaderModule(device, "#version 450\nlayout(location = 1) in vec2 vUv;\nlayout(set = 2, binding = 1) uniform sampler2D _MainTex;\nvoid main() {\n    if (texture(_MainTex, vUv).a < 0.5) discard;\n}", RuntimeShaderStage.Fragment, "DepthOnlyAlphaTest.frag");
		return _depthOnlyAlphaTestFragmentShader;
	}

	public VkShaderModule GetOrCreateFragmentShader(ShaderVariantKey variant)
	{
		if (fragmentShaders.TryGetValue(variant, out var value))
		{
			return value;
		}
		if (failedFragmentVariants.Contains(variant))
		{
			return GetOrCreateMagentaFragmentShader();
		}
		PrecompiledShaderStages precompiledStages = variant.Shader.PrecompiledStages;
		if ((object)precompiledStages != null)
		{
			value = RuntimeShaderCompiler.CreateShaderModule(device, precompiledStages.Fragment, variant.Shader.Name + ".frag");
			fragmentShaders.Add(variant, value);
			return value;
		}
		var (source, sourceName) = BuildVariantSource(variant, RuntimeShaderStage.Fragment);
		if (!TryCompileShaderModule(device, source, RuntimeShaderStage.Fragment, sourceName, out value))
		{
			failedFragmentVariants.Add(variant);
			return GetOrCreateMagentaFragmentShader();
		}
		fragmentShaders.Add(variant, value);
		return value;
	}

	public VkShaderModule GetOrCreateMagentaVertexShader()
	{
		if (!_magentaVertexShader.IsNull())
		{
			return _magentaVertexShader;
		}
		_magentaVertexShader = CompileShaderModule(device, "#version 450\nlayout(location = 0) in vec3 inPosition;\nlayout(push_constant) uniform Push { mat4 modelMatrix; } push;\nlayout(set = 0, binding = 0) uniform VertexUniforms { mat4 ViewProj; } uVertex;\nvoid main() { gl_Position = uVertex.ViewProj * (push.modelMatrix * vec4(inPosition, 1.0)); }\n", RuntimeShaderStage.Vertex, "Magenta.vert");
		return _magentaVertexShader;
	}

	public VkShaderModule GetOrCreateMagentaFragmentShader()
	{
		if (!_magentaFragmentShader.IsNull())
		{
			return _magentaFragmentShader;
		}
		_magentaFragmentShader = CompileShaderModule(device, "#version 450\nlayout(location = 0) out vec4 outColor;\nvoid main() { outColor = vec4(1.0, 0.0, 1.0, 1.0); }\n", RuntimeShaderStage.Fragment, "Magenta.frag");
		return _magentaFragmentShader;
	}

	private static VkShaderModule CompileShaderModule(DeviceEx device, string source, RuntimeShaderStage stage, string sourceName)
	{
		if (!TryCompileShaderModule(device, source, stage, sourceName, out var module))
		{
			throw new InvalidOperationException("Failed to compile built-in shader '" + sourceName + "'.");
		}
		return module;
	}

	private static bool TryCompileShaderModule(DeviceEx device, string source, RuntimeShaderStage stage, string sourceName, out VkShaderModule module)
	{
		module = default(VkShaderModule);
		try
		{
			module = RuntimeShaderCompiler.CompileModule(device, source, sourceName, "main", RuntimeShaderLanguage.Glsl, stage);
			return true;
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(38, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(ex.Message);
				message.AppendLiteral(" Substituting magenta fallback shader.");
			}
			log.Error(message, "TryCompileShaderModule", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Materials\\ShaderModuleCache.cs", 199);
			return false;
		}
	}

	internal static (string Source, string SourceName) BuildVariantSource(ShaderVariantKey variant, RuntimeShaderStage stage)
	{
		string text = StageFile(variant, stage);
		string source = LoadShaderSource(text);
		string text2 = BuildMentionedDefines(variant, source);
		string text3 = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(text));
		if (text2.Length > 0)
		{
			text3 = text3 + "_kw" + Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text2)).AsSpan(0, 4));
		}
		return (Source: InjectDefineBlock(source, text2), SourceName: "Mesh." + text3 + "." + ((stage == RuntimeShaderStage.Vertex) ? "vert" : "frag"));
	}

	internal static string BuildDefineBlock(ShaderVariantKey variant, RuntimeShaderStage stage)
	{
		try
		{
			return BuildMentionedDefines(variant, LoadShaderSource(StageFile(variant, stage)));
		}
		catch (Exception ex) when (((ex is IOException || ex is InvalidOperationException) ? 1 : 0) != 0)
		{
			return BuildAllDefines(variant);
		}
	}

	private static string StageFile(ShaderVariantKey variant, RuntimeShaderStage stage)
	{
		if (stage != 0)
		{
			return variant.Shader.FragmentShaderFileName;
		}
		return variant.Shader.VertexShaderFileName;
	}

	private static string BuildMentionedDefines(ShaderVariantKey variant, string source)
	{
		string text = BuildAllDefines(variant);
		HashSet<string> names = IdentifiersBySource.GetValue(source, ReadIdentifiers).Names;
		if (names == null)
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		string[] array = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text2 in array)
		{
			string[] array2 = text2.Split(' ', 3);
			if (array2 != null && array2.Length >= 2)
			{
				string text3 = array2[1];
				if (names.Contains(text3.TrimEnd('\r')))
				{
					stringBuilder.Append(text2).Append('\n');
				}
			}
		}
		return stringBuilder.ToString();
	}

	private static SourceIdentifiers ReadIdentifiers(string source)
	{
		return new SourceIdentifiers((source.Contains("##", StringComparison.Ordinal) || source.Contains("#include", StringComparison.Ordinal)) ? null : (from match in Identifier().Matches(source)
			select match.Value).ToHashSet<string>(StringComparer.Ordinal));
	}

	[GeneratedRegex("[A-Za-z_][A-Za-z0-9_]*")]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex Identifier()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__Identifier_0.Instance;
	}

	private static string BuildAllDefines(ShaderVariantKey variant)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (variant.UseTexture)
		{
			stringBuilder.AppendLine("#define USE_TEXTURE 1");
		}
		if (variant.OutlineMrt)
		{
			stringBuilder.AppendLine("#define OUTLINE_MRT 1");
		}
		if (variant.Instanced)
		{
			stringBuilder.AppendLine("#define INSTANCED 1");
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		hashSet.Add("OUTLINE_MRT");
		hashSet.Add("INSTANCED");
		AppendKeywordDefines(stringBuilder, variant.Shader.KeywordKey, hashSet);
		AppendKeywordDefines(stringBuilder, variant.DynamicKeywordKey, hashSet);
		return stringBuilder.ToString();
	}

	private static string InjectDefineBlock(string source, string defineBlock)
	{
		if (string.IsNullOrEmpty(defineBlock))
		{
			return source;
		}
		if (source.StartsWith("#version", StringComparison.Ordinal))
		{
			int num = source.IndexOf('\n');
			if (num >= 0)
			{
				return source.Insert(num + 1, defineBlock);
			}
		}
		return defineBlock + source;
	}

	private static void AppendKeywordDefines(StringBuilder builder, string keywordKey, HashSet<string> emittedKeywords)
	{
		if (string.IsNullOrWhiteSpace(keywordKey))
		{
			return;
		}
		int num = 0;
		while (num < keywordKey.Length)
		{
			int num2 = keywordKey.IndexOf(';', num);
			if (num2 < 0)
			{
				num2 = keywordKey.Length;
			}
			string text = keywordKey.Substring(num, num2 - num).Trim();
			if (!string.IsNullOrEmpty(text) && emittedKeywords.Add(text))
			{
				builder.Append("#define ");
				builder.Append(text);
				builder.AppendLine(" 1");
			}
			num = num2 + 1;
		}
	}

	public static string FormatVariantName(ShaderVariantKey variant)
	{
		StringBuilder stringBuilder = new StringBuilder((variant.Shader.PrecompiledStages != null) ? variant.Shader.Name : Path.GetFileNameWithoutExtension(variant.Shader.FragmentShaderFileName));
		if (!string.IsNullOrEmpty(variant.Shader.KeywordKey) || !string.IsNullOrEmpty(variant.DynamicKeywordKey))
		{
			string s = $"{variant.Shader.KeywordKey}\0{variant.DynamicKeywordKey}\0{variant.UseTexture}\0{variant.OutlineMrt}\0{variant.Instanced}";
			byte[] array = SHA256.HashData(Encoding.UTF8.GetBytes(s));
			stringBuilder.Append("_kw").Append(Convert.ToHexStringLower(array.AsSpan(0, 4)));
		}
		return stringBuilder.ToString();
	}

	public void Clear()
	{
		foreach (VkShaderModule value in vertexShaders.Values)
		{
			device.DestroyShaderModule(value, device.VkAllocator);
		}
		foreach (VkShaderModule value2 in fragmentShaders.Values)
		{
			device.DestroyShaderModule(value2, device.VkAllocator);
		}
		vertexShaders.Clear();
		fragmentShaders.Clear();
		failedVertexVariants.Clear();
		failedFragmentVariants.Clear();
	}

	public void Dispose()
	{
		foreach (VkShaderModule value in vertexShaders.Values)
		{
			device.DestroyShaderModule(value, device.VkAllocator);
		}
		foreach (VkShaderModule value2 in fragmentShaders.Values)
		{
			device.DestroyShaderModule(value2, device.VkAllocator);
		}
		if (!_depthOnlyFragmentShader.IsNull())
		{
			device.DestroyShaderModule(_depthOnlyFragmentShader, device.VkAllocator);
		}
		if (!_depthOnlyAlphaTestFragmentShader.IsNull())
		{
			device.DestroyShaderModule(_depthOnlyAlphaTestFragmentShader, device.VkAllocator);
		}
		if (!_magentaVertexShader.IsNull())
		{
			device.DestroyShaderModule(_magentaVertexShader, device.VkAllocator);
		}
		if (!_magentaFragmentShader.IsNull())
		{
			device.DestroyShaderModule(_magentaFragmentShader, device.VkAllocator);
		}
		vertexShaders.Clear();
		fragmentShaders.Clear();
		failedVertexVariants.Clear();
		failedFragmentVariants.Clear();
	}
}
