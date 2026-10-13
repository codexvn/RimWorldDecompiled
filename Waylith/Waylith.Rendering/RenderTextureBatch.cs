using System;
using System.Collections.Generic;
using System.Numerics;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public sealed class RenderTextureBatch
{
	internal readonly struct BlitDraw
	{
		public GpuTextureAllocation Source { get; }

		public VkSampler Sampler { get; }

		public float4 SrcUvRect { get; }

		public VkRect2D DstRect { get; }

		public float4 Tint { get; }

		public BlitDraw(GpuTextureAllocation source, VkSampler sampler, float4 srcUvRect, VkRect2D dstRect, float4 tint)
		{
			Source = source;
			Sampler = sampler;
			SrcUvRect = srcUvRect;
			DstRect = dstRect;
			Tint = tint;
		}
	}

	internal readonly struct CopyOp
	{
		public GpuTextureAllocation Destination { get; }

		public VkOffset2D SrcOffset { get; }

		public VkOffset2D DstOffset { get; }

		public VkExtent2D Extent { get; }

		public CopyOp(GpuTextureAllocation destination, VkOffset2D srcOffset, VkOffset2D dstOffset, VkExtent2D extent)
		{
			Destination = destination;
			SrcOffset = srcOffset;
			DstOffset = dstOffset;
			Extent = extent;
		}
	}

	private static readonly float4 FullSource = new float4(0f, 0f, 1f, 1f);

	private static readonly float4 White = new float4(1f, 1f, 1f, 1f);

	private readonly RenderTextureDrawFeature feature;

	private readonly List<BlitDraw> draws = new List<BlitDraw>();

	private readonly List<CopyOp> copies = new List<CopyOp>();

	private readonly List<MeshDrawCommand> meshDraws = new List<MeshDrawCommand>();

	public IRenderTextureTarget Target { get; set; }

	public float4 ClearColor { get; set; }

	public RenderTextureBarrier FinalState { get; set; } = RenderTextureBarrier.Sampleable;

	public CameraView View { get; set; }

	internal IReadOnlyList<BlitDraw> Draws => draws;

	internal IReadOnlyList<CopyOp> Copies => copies;

	internal IReadOnlyList<MeshDrawCommand> MeshDraws => meshDraws;

	internal RenderTextureBatch(RenderTextureDrawFeature feature, IRenderTextureTarget target)
	{
		this.feature = feature;
		Target = target;
	}

	public void Reset()
	{
		draws.Clear();
		copies.Clear();
		meshDraws.Clear();
	}

	public void Blit(GpuTextureAllocation source, in VkRect2D dstRect)
	{
		Blit(source, in FullSource, in dstRect, in White);
	}

	public void Blit(GpuTextureAllocation source, in float4 srcUvRect, in VkRect2D dstRect, in float4 tint, VkSampler sampler = default(VkSampler))
	{
		ArgumentNullException.ThrowIfNull(source, "source");
		source.MarkUsed();
		draws.Add(new BlitDraw(source, sampler, srcUvRect, dstRect, tint));
	}

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submesh = 0, byte pass = 0)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		ArgumentNullException.ThrowIfNull(material, "material");
		meshDraws.Add(new MeshDrawCommand(mesh, material, default(DrawPropertyRange), matrix, submesh, pass));
	}

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, MaterialPropertyBlock? properties = null, int submesh = 0, byte pass = 0)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		ArgumentNullException.ThrowIfNull(material, "material");
		meshDraws.Add(new MeshDrawCommand(mesh, material, MeshDrawSubmission.CopyProperties(properties), matrix, submesh, pass, null, 0, 0, MeshDrawSubmission.CollectDrawStorage(properties)));
	}

	public void CopyTo(GpuTextureAllocation destination, in VkOffset2D dstOffset = default(VkOffset2D))
	{
		ArgumentNullException.ThrowIfNull(destination, "destination");
		destination.MarkWritten();
		copies.Add(new CopyOp(destination, default(VkOffset2D), dstOffset, Target.Extent));
	}

	public void ExecuteNow()
	{
		feature.ExecuteNow(this);
	}

	public void Submit()
	{
		feature.Submit(this);
	}
}
