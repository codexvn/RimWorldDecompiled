using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.Rendering;

public sealed class Shader
{
	private static int NextId;

	public const int UnsetRenderQueue = -1;

	public const int GeometryRenderQueue = 2000;

	public const int TransparentRenderQueue = 3000;

	private string[] keywords;

	private readonly bool materialLayoutFromSource;

	private readonly bool vertexInputLayoutFromSource;

	private bool stageInterfaceBroken;

	private int[]? declaredPropertyIds;

	private SceneGrabPoint? sceneGrabPoint;

	private bool sceneGrabResolved;

	private static readonly Dictionary<byte, Shader> NoPassOverrides = new Dictionary<byte, Shader>();

	private Dictionary<byte, Shader>? passOverridesById;

	public int Id { get; }

	public ShaderProgramDefinition Program { get; private set; }

	public string? IconShaderKey { get; private set; }

	public string Key { get; }

	public string Name => Key;

	public string ShaderPath { get; }

	public string VertexShaderFileName { get; private set; }

	public string FragmentShaderFileName { get; private set; }

	public bool UseTexture { get; private set; }

	public bool Transparent { get; private set; }

	public bool DepthTest { get; private set; }

	public bool DepthWrite { get; private set; }

	public bool FlipTextureST { get; private set; }

	public int DefaultRenderQueue { get; private set; }

	public PipelineState PipelineState { get; private set; }

	public MaterialLayout MaterialLayout { get; private set; }

	public ShaderVertexInputLayout VertexInputLayout { get; private set; }

	public PrecompiledShaderStages? PrecompiledStages { get; }

	public bool HasErrors
	{
		get
		{
			if (!MaterialLayout.HasErrors && !VertexInputLayout.HasErrors)
			{
				return stageInterfaceBroken;
			}
			return true;
		}
	}

	public bool CanReloadFromSource
	{
		get
		{
			if (PrecompiledStages == null)
			{
				if (!materialLayoutFromSource)
				{
					return vertexInputLayoutFromSource;
				}
				return true;
			}
			return false;
		}
	}

	public ReadOnlySpan<string> Keywords => keywords;

	public string KeywordKey { get; private set; }

	public SceneGrabPoint? SceneGrab
	{
		get
		{
			if (Volatile.Read(in sceneGrabResolved))
			{
				return sceneGrabPoint;
			}
			sceneGrabPoint = ResolveSceneGrab();
			Volatile.Write(ref sceneGrabResolved, value: true);
			return sceneGrabPoint;
		}
	}

	private int[] DeclaredPropertyIds
	{
		get
		{
			int[] array = declaredPropertyIds;
			if (array != null)
			{
				return array;
			}
			MaterialLayout materialLayout = MaterialLayout;
			array = new int[materialLayout.UniformSlots.Length + materialLayout.TextureSlots.Length];
			int num = 0;
			MaterialUniformSlot[] uniformSlots = materialLayout.UniformSlots;
			foreach (MaterialUniformSlot materialUniformSlot in uniformSlots)
			{
				array[num++] = materialUniformSlot.PropertyId;
			}
			MaterialTextureSlot[] textureSlots = materialLayout.TextureSlots;
			foreach (MaterialTextureSlot materialTextureSlot in textureSlots)
			{
				array[num++] = materialTextureSlot.PropertyId;
			}
			return declaredPropertyIds = array;
		}
	}

	public Shader(string key, string shaderPath, ShaderProgramDefinition definition, IEnumerable<string>? keywords = null, MaterialLayout? materialLayout = null, PipelineState? pipelineState = null, string? iconShaderKey = null, ShaderVertexInputLayout? vertexInputLayout = null, PrecompiledShaderStages? precompiledStages = null)
	{
		Id = Interlocked.Increment(ref NextId);
		ShaderPath = ShaderRegistry.NormalizePath(shaderPath);
		Key = (string.IsNullOrWhiteSpace(key) ? ShaderPath : key);
		ApplyDefinition(definition, keywords, pipelineState, iconShaderKey);
		materialLayoutFromSource = materialLayout == null;
		vertexInputLayoutFromSource = vertexInputLayout == null;
		MaterialLayout = materialLayout ?? ShaderSourceLoader.CreateMaterialLayout(definition.fragmentShaderFileName);
		VertexInputLayout = vertexInputLayout ?? ShaderSourceLoader.CreateVertexInputLayout(definition.vertexShaderFileName);
		PrecompiledStages = precompiledStages;
		stageInterfaceBroken = ValidateStageInterface();
		SceneGrabRegistry.DefineFrom(definition);
	}

	[MemberNotNull(new string[] { "Program", "VertexShaderFileName", "FragmentShaderFileName", "PipelineState", "keywords", "KeywordKey" })]
	private void ApplyDefinition(ShaderProgramDefinition definition, IEnumerable<string>? keywords, PipelineState? pipelineState, string? iconShaderKey)
	{
		Program = definition;
		IconShaderKey = iconShaderKey;
		VertexShaderFileName = definition.vertexShaderFileName;
		FragmentShaderFileName = definition.fragmentShaderFileName;
		UseTexture = definition.useTexture;
		Transparent = definition.transparent;
		DepthTest = definition.depthTest;
		DepthWrite = definition.depthWrite;
		FlipTextureST = definition.flipTextureST;
		PipelineState = pipelineState ?? definition.ToPipelineState();
		DefaultRenderQueue = (((int)definition.renderQueue >= 0) ? ((int)definition.renderQueue) : (Transparent ? 3000 : 2000));
		this.keywords = ((keywords != null) ? CopyKeywords(keywords) : Array.Empty<string>());
		KeywordKey = CreateKeywordKey(this.keywords);
	}

	internal void Redefine(ShaderProgramDefinition definition, IEnumerable<string>? keywords, PipelineState? pipelineState, string? iconShaderKey)
	{
		ApplyDefinition(definition, keywords, pipelineState, iconShaderKey);
		SceneGrabRegistry.RedefineFrom(definition);
		passOverridesById = null;
		Volatile.Write(ref sceneGrabResolved, value: false);
	}

	private bool ValidateStageInterface()
	{
		if (PrecompiledStages == null && materialLayoutFromSource && vertexInputLayoutFromSource)
		{
			return ShaderSourceLoader.HasStageInterfaceErrors(VertexShaderFileName, FragmentShaderFileName);
		}
		return false;
	}

	internal void ReloadLayouts()
	{
		MaterialLayout materialLayout = (materialLayoutFromSource ? ShaderSourceLoader.CreateMaterialLayout(FragmentShaderFileName) : MaterialLayout);
		ShaderVertexInputLayout vertexInputLayout = (vertexInputLayoutFromSource ? ShaderSourceLoader.CreateVertexInputLayout(VertexShaderFileName) : VertexInputLayout);
		MaterialLayout = materialLayout;
		VertexInputLayout = vertexInputLayout;
		stageInterfaceBroken = ValidateStageInterface();
		declaredPropertyIds = null;
	}

	private SceneGrabPoint? ResolveSceneGrab()
	{
		string text = Program.sceneGrab?.name;
		if (text == null || text.Length <= 0)
		{
			return null;
		}
		bool shouldAppend;
		if (!SceneGrabRegistry.TryResolve(text, out SceneGrabPoint point))
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory = log;
			LogString<Warning> message = new LogString<Warning>(83, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Shader '");
				message.AppendFormatted(Key);
				message.AppendLiteral("' references scene grab '");
				message.AppendFormatted(text);
				message.AppendLiteral("', but no shader defines a queue for it; ignoring.");
			}
			logCategory.Warning(message, "ResolveSceneGrab", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Shader.cs", 155);
			return null;
		}
		if (DefaultRenderQueue < point.Queue)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			LogString<Warning> message = new LogString<Warning>(82, 4, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Shader '");
				message.AppendFormatted(Key);
				message.AppendLiteral("' (default queue ");
				message.AppendFormatted(DefaultRenderQueue);
				message.AppendLiteral(") samples scene grab '");
				message.AppendFormatted(text);
				message.AppendLiteral("', which is taken later, at queue ");
				message.AppendFormatted(point.Queue);
				message.AppendLiteral(".");
			}
			logCategory2.Warning(message, "ResolveSceneGrab", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Shader.cs", 160);
		}
		return point;
	}

	public bool HasPassOverride(string passName)
	{
		return Program.passShaderOverrides?.ContainsKey(passName) ?? false;
	}

	public void CollectVertexInputLayouts(List<ShaderVertexInputLayout> into)
	{
		into.Add(VertexInputLayout);
		Dictionary<string, string> passShaderOverrides = Program.passShaderOverrides;
		if (passShaderOverrides == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> item in passShaderOverrides)
		{
			if (ShaderRegistry.TryResolve(item.Value, out Shader shader))
			{
				into.Add(shader.VertexInputLayout);
			}
		}
	}

	public bool TryGetPassOverride(byte passId, out Shader overrideShader)
	{
		if (passOverridesById == null)
		{
			passOverridesById = BuildPassOverrides();
		}
		if (passOverridesById.TryGetValue(passId, out Shader value))
		{
			overrideShader = value;
			return true;
		}
		overrideShader = null;
		return false;
	}

	private Dictionary<byte, Shader> BuildPassOverrides()
	{
		Dictionary<string, string> passShaderOverrides = Program.passShaderOverrides;
		if (passShaderOverrides == null || passShaderOverrides.Count == 0)
		{
			return NoPassOverrides;
		}
		Dictionary<byte, Shader> dictionary = new Dictionary<byte, Shader>();
		foreach (KeyValuePair<string, string> item in passShaderOverrides)
		{
			if (ShaderRegistry.TryResolve(item.Value, out Shader shader))
			{
				dictionary[RenderPassRegistry.Register(item.Key)] = shader;
			}
		}
		return dictionary;
	}

	private static string[] CopyKeywords(IEnumerable<string> keywords)
	{
		List<string> list = keywords.Where((string keyword) => !string.IsNullOrWhiteSpace(keyword)).Distinct<string>(StringComparer.Ordinal).ToList();
		list.Sort(StringComparer.Ordinal);
		if (list.Count != 0)
		{
			return list.ToArray();
		}
		return Array.Empty<string>();
	}

	private static string CreateKeywordKey(string[] keywords)
	{
		if (keywords.Length != 0)
		{
			return string.Join(";", keywords);
		}
		return string.Empty;
	}

	public bool HasProperty(int propertyId)
	{
		return Array.IndexOf(DeclaredPropertyIds, propertyId) >= 0;
	}

	public int GetPropertyCount()
	{
		return DeclaredPropertyIds.Length;
	}

	public int GetPropertyNameId(int propertyIndex)
	{
		return DeclaredPropertyIds[propertyIndex];
	}

	public static int PropertyToID(string name)
	{
		return ShaderPropertyId.GetOrCreate(name);
	}
}
