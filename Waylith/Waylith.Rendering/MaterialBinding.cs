using System;
using System.Threading;

namespace Waylith.Rendering;

public sealed class MaterialBinding
{
	private MaterialUniformBytes uniforms;

	private MaterialTextureResourceSet<Texture?> resources;

	private long resolvedDataVersion;

	public int Id { get; }

	public int MaterialId { get; }

	public int MaterialVersion { get; private set; }

	public string DebugName { get; private set; } = string.Empty;

	public Shader Shader { get; private set; }

	public MaterialLayout MaterialLayout { get; private set; }

	public ref readonly MaterialUniformBytes Uniforms => ref uniforms;

	public ref readonly MaterialTextureResourceSet<Texture?> Resources => ref resources;

	public int RenderQueue { get; private set; }

	public ShaderVariantKey ShaderVariant { get; private set; }

	public long ShaderVariantId { get; }

	public PipelineState PipelineState { get; private set; }

	public int PipelineSortId { get; private set; }

	public TextureWrapMode? WrapModeOverride { get; private set; }

	public bool UsesGlobals { get; private set; }

	public long GlobalVersion { get; private set; }

	public long GlobalRegistrationVersion { get; private set; }

	public long ResolvedDataVersion => Interlocked.Read(in resolvedDataVersion);

	public MaterialStorageArray[] StorageArrays { get; private set; } = Array.Empty<MaterialStorageArray>();

	public MaterialBinding(int id, int materialId, int materialVersion, string debugName, Shader shader, MaterialLayout materialLayout, MaterialUniformBytes uniforms, MaterialTextureResourceSet<Texture?> resources, int renderQueue, ShaderVariantKey shaderVariant, PipelineState pipelineState, TextureWrapMode? wrapModeOverride, bool usesGlobals)
	{
		Id = id;
		MaterialId = materialId;
		ShaderVariantId = ShaderVariantIdRegistry.GetOrCreateId(shaderVariant);
		Update(materialVersion, debugName, shader, materialLayout, uniforms, resources, renderQueue, shaderVariant, pipelineState, wrapModeOverride, usesGlobals);
	}

	public void UpdateStorageArrays(MaterialStorageArray[] storageArrays)
	{
		if (StorageArrays != storageArrays)
		{
			StorageArrays = storageArrays;
			Interlocked.Increment(ref resolvedDataVersion);
		}
	}

	public void Update(int materialVersion, string debugName, Shader shader, MaterialLayout materialLayout, MaterialUniformBytes uniforms, MaterialTextureResourceSet<Texture?> resources, int renderQueue, ShaderVariantKey shaderVariant, PipelineState pipelineState, TextureWrapMode? wrapModeOverride, bool usesGlobals)
	{
		bool num = resolvedDataVersion == 0L || Shader != shader || MaterialLayout != materialLayout || !this.uniforms.Equals(in uniforms) || !this.resources.Equals(in resources) || WrapModeOverride != wrapModeOverride;
		MaterialVersion = materialVersion;
		DebugName = debugName;
		Shader = shader;
		MaterialLayout = materialLayout;
		this.uniforms = uniforms;
		this.resources = resources;
		RenderQueue = renderQueue;
		ShaderVariant = shaderVariant;
		PipelineState = pipelineState;
		PipelineSortId = PipelineSortIdRegistry.GetOrCreateId(ShaderVariantId, in pipelineState);
		WrapModeOverride = wrapModeOverride;
		UsesGlobals = usesGlobals;
		GlobalVersion = GlobalShaderState.Version;
		GlobalRegistrationVersion = GlobalShaderState.RegistrationVersion;
		if (num)
		{
			Interlocked.Increment(ref resolvedDataVersion);
		}
	}
}
