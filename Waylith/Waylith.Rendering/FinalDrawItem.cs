using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Brutal.VulkanApi;

namespace Waylith.Rendering;

public sealed class FinalDrawItem
{
	private static readonly ConcurrentQueue<FinalDrawItem> pool = new ConcurrentQueue<FinalDrawItem>();

	private MeshDrawGeometry mesh;

	private MaterialBinding binding;

	private bool hasOverride;

	private MaterialUniformBytes overrideUniforms;

	private MaterialTextureResourceSet<Texture?> overrideResources;

	internal VkPipeline cachedPipeline;

	internal VkPipelineLayout cachedPipelineLayout;

	internal int cachedPipelineVersion = -1;

	internal VkDescriptorSet cachedInstancedMaterialSet;

	internal long cachedInstancedResolvedDataVersion = -1L;

	internal int cachedInstancedResourceGeneration = -1;

	public InstancedDrawInfo? Instancing { get; private set; }

	public InstanceStorageBinding[]? Storage { get; private set; }

	public Matrix4x4 Matrix { get; private set; }

	public byte PassId { get; private set; }

	public int Layer { get; private set; }

	public int RenderQueue { get; private set; }

	public MeshDrawGeometry Mesh => mesh;

	public MaterialBinding Binding => binding;

	public bool HasOverride => hasOverride;

	public ref readonly MaterialUniformBytes Uniforms
	{
		get
		{
			if (hasOverride)
			{
				return ref overrideUniforms;
			}
			return ref binding.Uniforms;
		}
	}

	public ref readonly MaterialTextureResourceSet<Texture?> Resources
	{
		get
		{
			if (hasOverride)
			{
				return ref overrideResources;
			}
			return ref binding.Resources;
		}
	}

	public DrawSortKey SortKey { get; private set; }

	public FinalDrawItem(Matrix4x4 matrix, MeshDrawGeometry meshDraw, MaterialBinding materialBinding, InstancedDrawInfo? instancing = null)
	{
		Reset(matrix, 0, meshDraw, materialBinding, hasOverride: false, default(MaterialUniformBytes), default(MaterialTextureResourceSet<Texture>), 0, materialBinding.RenderQueue, instancing, 0, 0);
	}

	private FinalDrawItem()
	{
		binding = null;
	}

	internal static FinalDrawItem Rent(Matrix4x4 matrix, byte passId, MeshDrawGeometry meshDraw, MaterialBinding materialBinding, bool hasOverride, in MaterialUniformBytes overrideUniforms, in MaterialTextureResourceSet<Texture?> overrideResources, int queueOrder, int renderQueue, InstancedDrawInfo? instancing = null, int layer = 0, ushort hostOrder = 0, InstanceStorageBinding[]? storage = null)
	{
		if (!pool.TryDequeue(out FinalDrawItem result))
		{
			result = new FinalDrawItem();
		}
		result.Reset(matrix, passId, meshDraw, materialBinding, hasOverride, in overrideUniforms, in overrideResources, queueOrder, renderQueue, instancing, layer, hostOrder);
		result.Storage = storage;
		return result;
	}

	public static void Return(FinalDrawItem item)
	{
		if (item.hasOverride)
		{
			item.overrideResources = default(MaterialTextureResourceSet<Texture>);
			item.hasOverride = false;
		}
		item.Instancing = null;
		item.Storage = null;
		item.cachedInstancedMaterialSet = default(VkDescriptorSet);
		item.cachedInstancedResolvedDataVersion = -1L;
		item.cachedInstancedResourceGeneration = -1;
		pool.Enqueue(item);
	}

	[MemberNotNull("binding")]
	private void Reset(Matrix4x4 matrix, byte passId, MeshDrawGeometry meshDraw, MaterialBinding materialBinding, bool hasOverride, in MaterialUniformBytes overrideUniforms, in MaterialTextureResourceSet<Texture?> overrideResources, int queueOrder, int renderQueue, InstancedDrawInfo? instancing = null, int layer = 0, ushort hostOrder = 0)
	{
		mesh = meshDraw;
		binding = materialBinding;
		this.hasOverride = hasOverride;
		if (hasOverride)
		{
			this.overrideUniforms = overrideUniforms;
			this.overrideResources = overrideResources;
		}
		Instancing = instancing;
		Matrix = matrix;
		PassId = passId;
		SceneGrabPoint sceneGrab = materialBinding.Shader.SceneGrab;
		if (sceneGrab != null && renderQueue < sceneGrab.Queue)
		{
			renderQueue = sceneGrab.Queue;
		}
		RenderQueue = renderQueue;
		Layer = layer;
		SortKey = DrawSortKey.Make(renderQueue, renderQueue > 2500, hostOrder, materialBinding.PipelineSortId, queueOrder);
		cachedPipeline = default(VkPipeline);
		cachedPipelineLayout = default(VkPipelineLayout);
		cachedPipelineVersion = -1;
		cachedInstancedMaterialSet = default(VkDescriptorSet);
		cachedInstancedResolvedDataVersion = -1L;
		cachedInstancedResourceGeneration = -1;
	}
}
