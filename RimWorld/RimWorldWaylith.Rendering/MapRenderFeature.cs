using System;
using System.Collections.Generic;
using System.Numerics;
using Brutal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using RimWorld.Planet;
using UnityEngine;
using UnityStubs.Rendering;
using Verse;
using Waylith.Graphics;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

public sealed class MapRenderFeature : IRenderFeature, IDisposable
{
	private sealed class MapMainPass : IRenderPass, IDisposable
	{
		private readonly MeshBatchRenderer renderer;

		private readonly MeshBatchRenderer waterRenderer;

		private readonly RenderToMainThreadDispatcher renderToMainThread;

		private readonly SceneColorTargets sceneTargets;

		private readonly SceneGrabTargets sceneGrabTargets;

		private readonly WaterDepthTargets waterDepthTargets;

		public byte PassId { get; } = RenderPassIds.MapMain;

		public PassAttachment? Attachment => null;

		public bool PreBackbuffer => true;

		public MapMainPass(MeshBatchRenderer renderer, MeshBatchRenderer waterRenderer, RenderToMainThreadDispatcher renderToMainThread, SceneColorTargets sceneTargets, SceneGrabTargets sceneGrabTargets, WaterDepthTargets waterDepthTargets)
		{
			this.renderer = renderer;
			this.waterRenderer = waterRenderer;
			this.renderToMainThread = renderToMainThread;
			this.sceneTargets = sceneTargets;
			this.sceneGrabTargets = sceneGrabTargets;
			this.waterDepthTargets = waterDepthTargets;
		}

		public void Record(in FrameContext ctx, object? featurePayload)
		{
			GpuTextureService.FlushPendingTextureCommands();
			if (featurePayload is MeshDrawFrame meshFrame)
			{
				RecordWaterDepthPrePass(in ctx, meshFrame);
				RecordScene(in ctx, meshFrame);
				sceneTargets.RecordSampleBarrier(ctx.CommandBuffer, ctx.SubmissionFrameIndex);
			}
		}

		private unsafe void RecordWaterDepthPrePass(in FrameContext ctx, MeshDrawFrame meshFrame)
		{
			PassAttachment attachment = waterDepthTargets.Attachment;
			if (attachment != null)
			{
				FrameContext frameContext = attachment.MakeFrameContext(in ctx);
				CommandBuffer commandBuffer = frameContext.CommandBuffer;
				VkClearValue[] clearValues = attachment.ClearValues;
				fixed (VkClearValue* clearValues2 = clearValues)
				{
					VkRenderPassBeginInfo pRenderPassBegin = new VkRenderPassBeginInfo
					{
						RenderPass = frameContext.RenderPass,
						Framebuffer = frameContext.Framebuffer,
						RenderArea = new VkRect2D(frameContext.Extent),
						ClearValueCount = clearValues.Length,
						ClearValues = clearValues2
					};
					commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
				}
				SetFullViewport(commandBuffer, frameContext.Extent);
				waterRenderer.RecordPass(in frameContext, meshFrame, RenderPassIds.WaterDepth, null, null, int.MinValue, int.MaxValue, 1);
				commandBuffer.EndRenderPass();
				waterDepthTargets.MarkSampleable();
			}
		}

		private void RecordScene(in FrameContext ctx, MeshDrawFrame meshFrame)
		{
			PassAttachment passAttachment = ((meshFrame.PreserveSceneColor && sceneTargets.BackdropAttachment != null) ? sceneTargets.BackdropAttachment : sceneTargets.Attachment);
			if (passAttachment == null)
			{
				return;
			}
			IReadOnlyList<SceneGrabPoint> sceneGrabs = meshFrame.SceneGrabs;
			if (sceneGrabs == null || sceneGrabs.Count == 0 || sceneTargets.LoadAttachment == null)
			{
				RecordSlice(in ctx, meshFrame, passAttachment, clear: true, int.MinValue, int.MaxValue);
				return;
			}
			int submissionFrameIndex = ctx.SubmissionFrameIndex;
			int num = int.MinValue;
			int num2 = 0;
			while (true)
			{
				bool flag = num == int.MinValue;
				int num3 = ((num2 < sceneGrabs.Count) ? sceneGrabs[num2].Queue : int.MaxValue);
				RecordSlice(in ctx, meshFrame, flag ? passAttachment : sceneTargets.LoadAttachment, flag, num, num3);
				if (num2 < sceneGrabs.Count)
				{
					VkImage image = sceneTargets.Current(submissionFrameIndex).Color.Image;
					do
					{
						sceneGrabTargets.RecordGrab(ctx.CommandBuffer, sceneGrabs[num2], image);
						num2++;
					}
					while (num2 < sceneGrabs.Count && sceneGrabs[num2].Queue == num3);
					if (Platform.IsMacOS)
					{
						RecordTransparentDepthLoadBarrier(ctx.CommandBuffer, submissionFrameIndex);
					}
					num = num3;
					continue;
				}
				break;
			}
		}

		private unsafe void RecordSlice(in FrameContext ctx, MeshDrawFrame meshFrame, PassAttachment attachment, bool clear, int queueMin, int queueMax)
		{
			FrameContext frameContext = attachment.MakeFrameContext(in ctx);
			CommandBuffer commandBuffer = frameContext.CommandBuffer;
			VkRenderPassBeginInfo vkRenderPassBeginInfo = new VkRenderPassBeginInfo();
			vkRenderPassBeginInfo.RenderPass = frameContext.RenderPass;
			vkRenderPassBeginInfo.Framebuffer = frameContext.Framebuffer;
			vkRenderPassBeginInfo.RenderArea = new VkRect2D(frameContext.Extent);
			VkRenderPassBeginInfo pRenderPassBegin = vkRenderPassBeginInfo;
			if (clear)
			{
				System.Numerics.Vector4 backgroundColor = meshFrame.BackgroundColor;
				VkClearValue* ptr = stackalloc VkClearValue[2];
				*ptr = new VkClearColorValue
				{
					Float32 = new float4(backgroundColor.X, backgroundColor.Y, backgroundColor.Z, backgroundColor.W)
				};
				ptr[1] = new VkClearDepthStencilValue
				{
					Depth = 0f,
					Stencil = 0
				};
				pRenderPassBegin.ClearValueCount = 2;
				pRenderPassBegin.ClearValues = ptr;
				commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
			}
			else
			{
				commandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
			}
			SetFullViewport(commandBuffer, frameContext.Extent);
			renderer.RecordPass(in frameContext, meshFrame, PassId, null, null, queueMin, queueMax, -1, mergeBySortKey: true);
			commandBuffer.EndRenderPass();
		}

		private void RecordTransparentDepthLoadBarrier(CommandBuffer cmd, int frame)
		{
			VkImageMemoryBarrier[] array = new VkImageMemoryBarrier[1]
			{
				new VkImageMemoryBarrier
				{
					SrcAccessMask = VkAccessFlags.DepthStencilAttachmentWriteBit,
					DstAccessMask = (VkAccessFlags.DepthStencilAttachmentReadBit | VkAccessFlags.DepthStencilAttachmentWriteBit),
					OldLayout = VkImageLayout.DepthStencilAttachmentOptimal,
					NewLayout = VkImageLayout.DepthStencilAttachmentOptimal,
					SrcQueueFamilyIndex = -1,
					DstQueueFamilyIndex = -1,
					Image = sceneTargets.Current(frame).Depth.Image,
					SubresourceRange = new VkImageSubresourceRange
					{
						AspectMask = (VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit),
						BaseMipLevel = 0,
						LevelCount = 1,
						BaseArrayLayer = 0,
						LayerCount = 1
					}
				}
			};
			cmd.PipelineBarrier(VkPipelineStageFlags.LateFragmentTestsBit, VkPipelineStageFlags.EarlyFragmentTestsBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), array);
		}

		private static void SetFullViewport(CommandBuffer cmd, VkExtent2D extent)
		{
			VkViewport reference = new VkViewport
			{
				X = 0f,
				Y = 0f,
				Width = extent.Width,
				Height = extent.Height,
				MinDepth = 0f,
				MaxDepth = 1f
			};
			cmd.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
			VkRect2D reference2 = new VkRect2D(extent);
			cmd.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		}

		public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
		{
			if (renderTarget is IRenderContext renderContext)
			{
				renderer.OnRenderContextRebuilt(renderContext);
				waterRenderer.OnRenderContextRebuilt(renderContext);
			}
		}

		public void Dispose()
		{
			renderer.Dispose();
			waterRenderer.Dispose();
		}

		void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
		{
			Record(in ctx, featurePayload);
		}
	}

	private static readonly int WaterOutputTexId = UnityEngine.Shader.PropertyToID("_WaterOutputTex");

	private static readonly UnityEngine.Color MapBackgroundSkyMultiplier = new UnityEngine.Color(0.13f, 0.13f, 0.13f, 1f);

	private readonly GraphicsDevice graphicsDevice;

	private readonly SceneColorTargets sceneTargets;

	private readonly SceneGrabTargets sceneGrabTargets;

	private readonly WaterDepthTargets waterDepthTargets;

	private readonly MapMainPass mainPass;

	private readonly IRenderPass[] passes;

	private readonly TransparentDepthOrder depthOrder = new TransparentDepthOrder();

	private bool isActive;

	public bool IsActive => isActive;

	public IReadOnlyList<IRenderPass> Passes => passes;

	public MapRenderFeature(GraphicsDevice graphicsDevice, IRenderContext renderContext, RenderToMainThreadDispatcher renderToMainThread, SceneColorTargets sceneTargets)
	{
		this.graphicsDevice = graphicsDevice;
		this.sceneTargets = sceneTargets;
		sceneGrabTargets = new SceneGrabTargets(renderContext.ColorFormat);
		waterDepthTargets = new WaterDepthTargets(graphicsDevice);
		MeshBatchRenderer renderer = new MeshBatchRenderer(graphicsDevice, renderContext);
		MeshBatchRenderer waterRenderer = new MeshBatchRenderer(graphicsDevice, renderContext);
		mainPass = new MapMainPass(renderer, waterRenderer, renderToMainThread, sceneTargets, sceneGrabTargets, waterDepthTargets);
		passes = new IRenderPass[1] { mainPass };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		isActive = !LongEventState.EventWithoutStandardWindowRunning && WorldRendererUtility.DrawingMap && MapGenerator.mapBeingGenerated == null;
		if (!isActive)
		{
			return null;
		}
		MeshDrawSubmissionSnapshot meshDrawSubmissionSnapshot = MeshDrawSubmission.CaptureFlushedSnapshot(graphicsDevice);
		string globalKeywordKey = UnityEngine.Shader.GlobalKeywordKey;
		Camera camera = Current.CameraDriver?.MyCamera;
		if (camera == null)
		{
			isActive = false;
			return null;
		}
		CameraView view = CameraViewFactory.GetView(camera);
		MeshDrawCommand[] commands = meshDrawSubmissionSnapshot.Commands;
		List<FinalDrawItem> list = MeshDrawFrame.RentList();
		List<FinalDrawItem> list2 = MeshDrawFrame.RentList();
		List<SceneGrabPoint> sceneGrabs = null;
		GlobalShaderState.SetTexture(WaterOutputTexId, waterDepthTargets.Output);
		UnityBuiltinGlobals.Publish(camera, view.View, view.ViewProjection);
		DrawItemFactory drawItemMaker = MeshDrawSubmission.DrawItemMaker;
		if (drawItemMaker == null)
		{
			return null;
		}
		depthOrder.Compute(commands, meshDrawSubmissionSnapshot.CommandCount, view.View);
		for (int i = 0; i < meshDrawSubmissionSnapshot.CommandCount; i++)
		{
			MeshDrawCommand command = commands[i];
			FinalDrawItem finalDrawItem = drawItemMaker.TryMake(in command, meshDrawSubmissionSnapshot.PropertyArena, globalKeywordKey, i, null, depthOrder[i]);
			if (finalDrawItem != null)
			{
				list.Add(finalDrawItem);
				CollectSceneGrab(finalDrawItem, ref sceneGrabs);
			}
		}
		for (int j = 0; j < meshDrawSubmissionSnapshot.PersistentSources.Count; j++)
		{
			IReadOnlyList<FinalDrawItem> drawItemsReadOnly = meshDrawSubmissionSnapshot.PersistentSources[j].DrawItemsReadOnly;
			for (int k = 0; k < drawItemsReadOnly.Count; k++)
			{
				list2.Add(drawItemsReadOnly[k]);
				CollectSceneGrab(drawItemsReadOnly[k], ref sceneGrabs);
			}
		}
		list2.Sort(DrawSortKeyComparer.Instance);
		sceneGrabs?.Sort((SceneGrabPoint a, SceneGrabPoint b) => a.Queue.CompareTo(b.Queue));
		System.Numerics.Vector3 sunDirection = System.Numerics.Vector3.UnitY;
		if (UnityEngine.Shader.TryGetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, out var value))
		{
			sunDirection = new System.Numerics.Vector3(value.x, value.y, value.z);
		}
		System.Numerics.Vector4 backgroundColor = System.Numerics.Vector4.Zero;
		SkyManager skyManager = Find.CurrentMap?.skyManager;
		if (skyManager != null)
		{
			UnityEngine.Color color = skyManager.CurSky.colors.sky * MapBackgroundSkyMultiplier;
			backgroundColor = new System.Numerics.Vector4(color.r, color.g, color.b, color.a);
		}
		return new MeshDrawFrame(timing.FrameNumber, view, list, list2, sunDirection)
		{
			BackgroundColor = backgroundColor,
			SceneGrabs = sceneGrabs,
			PreserveSceneColor = (Find.World?.renderer != null && WorldRendererUtility.WorldBackgroundNow && Find.WorldCamera != null)
		};
	}

	private static void CollectSceneGrab(FinalDrawItem draw, ref List<SceneGrabPoint>? sceneGrabs)
	{
		SceneGrabPoint sceneGrab = draw.Binding.Shader.SceneGrab;
		if (sceneGrab != null)
		{
			if (sceneGrabs == null)
			{
				sceneGrabs = new List<SceneGrabPoint>();
			}
			if (!sceneGrabs.Contains(sceneGrab))
			{
				sceneGrabs.Add(sceneGrab);
			}
		}
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
		if (renderTarget is IRenderContext renderContext)
		{
			VkExtent2D extent = renderContext.GetExtent();
			sceneTargets.EnsureSized(renderContext.FramesInFlight, extent.Width, extent.Height);
			sceneGrabTargets.SetExtent(extent.Width, extent.Height);
			waterDepthTargets.EnsureSized(renderContext.FramesInFlight, extent.Width, extent.Height);
		}
		mainPass.OnRenderTargetRebuilt(renderTarget);
	}

	public void Dispose()
	{
		mainPass.Dispose();
		sceneGrabTargets.Dispose();
		waterDepthTargets.Dispose();
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}
}
