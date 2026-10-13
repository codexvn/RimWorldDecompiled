using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
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

public sealed class WorldRenderFeature : IRenderFeature, IDisposable
{
	private readonly record struct CameraSlot(MeshDrawFrame Frame, CameraClearFlags ClearFlags, MeshBatchRenderer Renderer);

	private sealed class WorldFrame(CameraSlot[] slots, bool hdr) : IDisposable
	{
		public CameraSlot[] Slots { get; } = slots;

		public bool Hdr { get; } = hdr;

		public void Dispose()
		{
			CameraSlot[] slots = Slots;
			foreach (CameraSlot cameraSlot in slots)
			{
				cameraSlot.Frame.Dispose();
			}
		}
	}

	private sealed class WorldMainPass : IRenderPass, IDisposable
	{
		private readonly GraphicsDevice graphicsDevice;

		private readonly List<MeshBatchRenderer> renderers = new List<MeshBatchRenderer>();

		private readonly RenderToMainThreadDispatcher renderToMainThread;

		private readonly SceneColorTargets sceneTargets;

		private readonly SceneColorTargets hdrTargets;

		private readonly SceneGrabTargets grabTargets;

		private readonly SceneGrabTargets hdrGrabTargets;

		private IRenderContext renderContext;

		public byte PassId { get; } = RenderPassIds.WorldMain;

		public PassAttachment? Attachment => sceneTargets.Attachment;

		public bool PreBackbuffer => true;

		public IReadOnlyList<MeshBatchRenderer> Renderers => renderers;

		public WorldMainPass(GraphicsDevice graphicsDevice, IRenderContext renderContext, RenderToMainThreadDispatcher renderToMainThread, SceneColorTargets sceneTargets, SceneColorTargets hdrTargets, SceneGrabTargets grabTargets, SceneGrabTargets hdrGrabTargets)
		{
			this.graphicsDevice = graphicsDevice;
			this.renderContext = renderContext;
			this.renderToMainThread = renderToMainThread;
			this.sceneTargets = sceneTargets;
			this.hdrTargets = hdrTargets;
			this.grabTargets = grabTargets;
			this.hdrGrabTargets = hdrGrabTargets;
			EnsureRenderers(2);
		}

		public void EnsureRenderers(int count)
		{
			while (renderers.Count < count)
			{
				renderers.Add(new MeshBatchRenderer(graphicsDevice, renderContext));
			}
		}

		public void Record(in FrameContext ctx, object? featurePayload)
		{
			GpuTextureService.FlushPendingTextureCommands();
			if (!(featurePayload is WorldFrame worldFrame))
			{
				return;
			}
			SceneColorTargets sceneColorTargets = (worldFrame.Hdr ? hdrTargets : sceneTargets);
			if (sceneColorTargets.Attachment != null && sceneColorTargets.LoadAttachment != null)
			{
				RecordCameras(in ctx, sceneColorTargets, worldFrame.Hdr ? hdrGrabTargets : grabTargets, worldFrame.Slots);
				if (worldFrame.Hdr)
				{
					sceneTargets.RecordResolveFrom(ctx.CommandBuffer, ctx.SubmissionFrameIndex, hdrTargets);
				}
				else
				{
					sceneTargets.RecordSampleBarrier(ctx.CommandBuffer, ctx.SubmissionFrameIndex);
				}
			}
		}

		private void RecordCameras(in FrameContext ctx, SceneColorTargets targets, SceneGrabTargets grabs, CameraSlot[] slots)
		{
			int submissionFrameIndex = ctx.SubmissionFrameIndex;
			bool flag = true;
			for (int i = 0; i < slots.Length; i++)
			{
				CameraSlot slot = slots[i];
				IReadOnlyList<SceneGrabPoint> sceneGrabs = slot.Frame.SceneGrabs;
				int num = int.MinValue;
				int num2 = 0;
				while (true)
				{
					int num3 = ((sceneGrabs != null && num2 < sceneGrabs.Count) ? sceneGrabs[num2].Queue : int.MaxValue);
					FrameContext ctx2 = (flag ? targets.Attachment : targets.LoadAttachment).MakeFrameContext(in ctx);
					BeginSlice(in ctx2, slot, flag);
					if (num == int.MinValue && !flag)
					{
						Clear(ctx2.CommandBuffer, ctx2.Extent, slot);
					}
					MeshBatchRenderer renderer = slot.Renderer;
					MeshDrawFrame frame = slot.Frame;
					byte passId = PassId;
					int queueMin = num;
					int queueMax = num3;
					renderer.RecordPass(in ctx2, frame, passId, null, null, queueMin, queueMax);
					ctx2.CommandBuffer.EndRenderPass();
					flag = false;
					if (sceneGrabs == null || num2 >= sceneGrabs.Count)
					{
						break;
					}
					VkImage image = targets.Current(submissionFrameIndex).Color.Image;
					do
					{
						grabs.RecordGrab(ctx.CommandBuffer, sceneGrabs[num2], image);
						num2++;
					}
					while (num2 < sceneGrabs.Count && sceneGrabs[num2].Queue == num3);
					if (Platform.IsMacOS)
					{
						RecordDepthLoadBarrier(ctx.CommandBuffer, targets.Current(submissionFrameIndex).Depth.Image);
					}
					num = num3;
				}
			}
		}

		private unsafe static void BeginSlice(in FrameContext ctx, CameraSlot slot, bool clear)
		{
			VkRenderPassBeginInfo vkRenderPassBeginInfo = new VkRenderPassBeginInfo();
			vkRenderPassBeginInfo.RenderPass = ctx.RenderPass;
			vkRenderPassBeginInfo.Framebuffer = ctx.Framebuffer;
			vkRenderPassBeginInfo.RenderArea = new VkRect2D(ctx.Extent);
			VkRenderPassBeginInfo pRenderPassBegin = vkRenderPassBeginInfo;
			if (clear)
			{
				System.Numerics.Vector4 backgroundColor = slot.Frame.BackgroundColor;
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
			}
			ctx.CommandBuffer.BeginRenderPass(in pRenderPassBegin, VkSubpassContents.Inline);
			VkViewport vkViewport = new VkViewport();
			vkViewport.X = 0f;
			vkViewport.Y = 0f;
			vkViewport.Width = ctx.Extent.Width;
			vkViewport.Height = ctx.Extent.Height;
			vkViewport.MinDepth = 0f;
			vkViewport.MaxDepth = 1f;
			VkViewport vkViewport2 = vkViewport;
			CommandBuffer commandBuffer = ctx.CommandBuffer;
			VkViewport reference = vkViewport2;
			commandBuffer.SetViewport(0, new ReadOnlySpan<VkViewport>(in reference));
			CommandBuffer commandBuffer2 = ctx.CommandBuffer;
			VkRect2D reference2 = new VkRect2D(ctx.Extent);
			commandBuffer2.SetScissor(0, new ReadOnlySpan<VkRect2D>(in reference2));
		}

		private static void RecordDepthLoadBarrier(CommandBuffer cmd, VkImage depthImage)
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
					Image = depthImage,
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

		private static void Clear(CommandBuffer cmd, VkExtent2D extent, CameraSlot slot)
		{
			if (slot.ClearFlags != CameraClearFlags.Nothing)
			{
				VkClearAttachment vkClearAttachment = new VkClearAttachment();
				vkClearAttachment.AspectMask = VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit;
				VkClearAttachment vkClearAttachment2 = vkClearAttachment;
				vkClearAttachment2.ClearValue.DepthStencil = new VkClearDepthStencilValue
				{
					Depth = 0f,
					Stencil = 0
				};
				VkClearRect vkClearRect = new VkClearRect();
				vkClearRect.Rect = new VkRect2D(extent);
				vkClearRect.LayerCount = 1;
				VkClearRect vkClearRect2 = vkClearRect;
				if (slot.ClearFlags != CameraClearFlags.Color)
				{
					VkClearAttachment reference = vkClearAttachment2;
					ReadOnlySpan<VkClearAttachment> pAttachments = new ReadOnlySpan<VkClearAttachment>(in reference);
					VkClearRect reference2 = vkClearRect2;
					cmd.ClearAttachments(pAttachments, new ReadOnlySpan<VkClearRect>(in reference2));
					return;
				}
				System.Numerics.Vector4 backgroundColor = slot.Frame.BackgroundColor;
				vkClearAttachment = new VkClearAttachment();
				vkClearAttachment.AspectMask = VkImageAspectFlags.ColorBit;
				vkClearAttachment.ColorAttachment = 0;
				VkClearAttachment vkClearAttachment3 = vkClearAttachment;
				vkClearAttachment3.ClearValue.Color = new VkClearColorValue
				{
					Float32 = new float4(backgroundColor.X, backgroundColor.Y, backgroundColor.Z, backgroundColor.W)
				};
				InlineArray2<VkClearAttachment> buffer = default(InlineArray2<VkClearAttachment>);
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkClearAttachment>, VkClearAttachment>(ref buffer, 0) = vkClearAttachment3;
				global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<VkClearAttachment>, VkClearAttachment>(ref buffer, 1) = vkClearAttachment2;
				ReadOnlySpan<VkClearAttachment> pAttachments2 = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<VkClearAttachment>, VkClearAttachment>(in buffer, 2);
				VkClearRect reference3 = vkClearRect2;
				cmd.ClearAttachments(pAttachments2, new ReadOnlySpan<VkClearRect>(in reference3));
			}
		}

		public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
		{
			if (!(renderTarget is IRenderContext renderContext))
			{
				return;
			}
			this.renderContext = renderContext;
			VkExtent2D extent = renderContext.GetExtent();
			hdrTargets.EnsureSized(renderContext.FramesInFlight, extent.Width, extent.Height);
			grabTargets.SetExtent(extent.Width, extent.Height);
			hdrGrabTargets.SetExtent(extent.Width, extent.Height);
			foreach (MeshBatchRenderer renderer in renderers)
			{
				renderer.OnRenderContextRebuilt(renderContext);
			}
		}

		public void Dispose()
		{
			foreach (MeshBatchRenderer renderer in renderers)
			{
				renderer.Dispose();
			}
			hdrTargets.Dispose();
			grabTargets.Dispose();
			hdrGrabTargets.Dispose();
		}

		void IRenderPass.Record(in FrameContext ctx, object? featurePayload)
		{
			Record(in ctx, featurePayload);
		}
	}

	private static readonly UnityEngine.Color DefaultSkyColor = new UnityEngine.Color(0.0627451f, 0.09019608f, 0.11764706f);

	private readonly GraphicsDevice graphicsDevice;

	private readonly WorldMainPass mainPass;

	private readonly IRenderPass[] passes;

	private readonly TransparentDepthOrder depthOrder = new TransparentDepthOrder();

	private readonly List<Camera> cameras = new List<Camera>();

	private readonly int worldTextMask = UnityEngine.LayerMask.GetMask("WorldText");

	private bool isActive;

	public bool IsActive => isActive;

	public IReadOnlyList<IRenderPass> Passes => passes;

	public WorldRenderFeature(GraphicsDevice graphicsDevice, IRenderContext renderContext, RenderToMainThreadDispatcher renderToMainThread, SceneColorTargets sceneTargets)
	{
		this.graphicsDevice = graphicsDevice;
		SceneColorTargets sceneColorTargets = new SceneColorTargets(graphicsDevice, VkFormat.R16G16B16A16SFloat, renderContext.DepthFormat, "WorldHdrColor");
		sceneColorTargets.EnsureSized(renderContext.FramesInFlight, renderContext.GetExtent().Width, renderContext.GetExtent().Height);
		SceneGrabTargets sceneGrabTargets = new SceneGrabTargets(renderContext.ColorFormat);
		SceneGrabTargets sceneGrabTargets2 = new SceneGrabTargets(VkFormat.R16G16B16A16SFloat);
		VkExtent2D extent = renderContext.GetExtent();
		sceneGrabTargets.SetExtent(extent.Width, extent.Height);
		sceneGrabTargets2.SetExtent(extent.Width, extent.Height);
		mainPass = new WorldMainPass(graphicsDevice, renderContext, renderToMainThread, sceneTargets, sceneColorTargets, sceneGrabTargets, sceneGrabTargets2);
		passes = new IRenderPass[1] { mainPass };
	}

	public object? CaptureFrame(in FrameTiming timing)
	{
		isActive = !LongEventState.EventWithoutStandardWindowRunning && Find.World?.renderer != null && WorldRendererUtility.WorldRendered;
		if (!isActive)
		{
			return null;
		}
		DrawItemFactory drawItemMaker = MeshDrawSubmission.DrawItemMaker;
		CollectCameras();
		if (cameras.Count == 0 || drawItemMaker == null)
		{
			isActive = false;
			return null;
		}
		MeshDrawSubmissionSnapshot meshDrawSubmissionSnapshot = MeshDrawSubmission.CaptureFlushedSnapshot(graphicsDevice);
		string globalKeywordKey = UnityEngine.Shader.GlobalKeywordKey;
		MeshDrawCommand[] commands = meshDrawSubmissionSnapshot.Commands;
		System.Numerics.Vector3 sunDirection = ResolveSunDirection();
		Camera worldCamera = Find.WorldCamera;
		CameraSlot[] array = new CameraSlot[cameras.Count];
		bool flag = false;
		mainPass.EnsureRenderers(cameras.Count);
		for (int i = 0; i < cameras.Count; i++)
		{
			Camera camera = cameras[i];
			flag |= camera.allowHDR;
			CameraView view = CameraViewFactory.GetView(camera);
			UnityBuiltinGlobals.Publish(camera, view.View, view.ViewProjection);
			int num = ((camera == worldCamera) ? (camera.cullingMask | worldTextMask) : camera.cullingMask);
			List<FinalDrawItem> list = new List<FinalDrawItem>();
			List<FinalDrawItem> list2 = new List<FinalDrawItem>();
			List<SceneGrabPoint> sceneGrabs = null;
			depthOrder.Compute(commands, meshDrawSubmissionSnapshot.CommandCount, view.View);
			for (int j = 0; j < meshDrawSubmissionSnapshot.CommandCount; j++)
			{
				MeshDrawCommand command = commands[j];
				if (((1 << command.Layer) & num) != 0 && command.PassId != RenderPassIds.WaterDepth && command.PassId != RenderPassIds.CaptureOnly)
				{
					if (command.PassId != RenderPassIds.WorldMain)
					{
						command = new MeshDrawCommand(command.Mesh, command.Material, command.PropertyRange, command.Matrix, command.SubmeshIndex, RenderPassIds.WorldMain, command.Instancing, command.RenderQueueOverride, command.Layer, command.Storage);
					}
					FinalDrawItem finalDrawItem = drawItemMaker.TryMake(in command, meshDrawSubmissionSnapshot.PropertyArena, globalKeywordKey, j, null, depthOrder[j]);
					if (finalDrawItem != null)
					{
						list.Add(finalDrawItem);
						CollectSceneGrab(finalDrawItem, ref sceneGrabs);
					}
				}
			}
			if (camera == worldCamera)
			{
				for (int k = 0; k < meshDrawSubmissionSnapshot.PersistentSources.Count; k++)
				{
					IReadOnlyList<FinalDrawItem> drawItemsReadOnly = meshDrawSubmissionSnapshot.PersistentSources[k].DrawItemsReadOnly;
					for (int l = 0; l < drawItemsReadOnly.Count; l++)
					{
						list2.Add(drawItemsReadOnly[l]);
						CollectSceneGrab(drawItemsReadOnly[l], ref sceneGrabs);
					}
				}
			}
			sceneGrabs?.Sort((SceneGrabPoint a, SceneGrabPoint b) => a.Queue.CompareTo(b.Queue));
			CameraClearFlags cameraClearFlags = ((camera.clearFlags == CameraClearFlags.Skybox) ? CameraClearFlags.Color : camera.clearFlags);
			UnityEngine.Color color = ((cameraClearFlags == CameraClearFlags.Color) ? camera.backgroundColor : DefaultSkyColor);
			MeshDrawFrame frame = new MeshDrawFrame(timing.FrameNumber, view, list, list2, sunDirection)
			{
				BackgroundColor = new System.Numerics.Vector4(color.r, color.g, color.b, 1f),
				SceneGrabs = sceneGrabs
			};
			array[i] = new CameraSlot(frame, cameraClearFlags, mainPass.Renderers[i]);
		}
		return new WorldFrame(array, flag);
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

	private void CollectCameras()
	{
		cameras.Clear();
		Camera camera = Find.Camera;
		Camera[] array = UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		foreach (Camera camera2 in array)
		{
			if (camera2.isActiveAndEnabled && camera2.targetTexture == null && (camera == null || !camera2.transform.IsChildOf(camera.transform)))
			{
				cameras.Add(camera2);
			}
		}
		cameras.Sort((Camera a, Camera b) => a.depth.CompareTo(b.depth));
	}

	private static System.Numerics.Vector3 ResolveSunDirection()
	{
		if ((!UnityEngine.Shader.TryGetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, out var value) || value > 0.5f) && UnityEngine.Shader.TryGetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, out var value2))
		{
			System.Numerics.Vector3 value3 = new System.Numerics.Vector3(value2.x, value2.y, value2.z);
			if (value3.LengthSquared() > 1E-06f)
			{
				return System.Numerics.Vector3.Normalize(value3);
			}
		}
		return System.Numerics.Vector3.Zero;
	}

	public void OnRenderTargetRebuilt(IRenderTarget renderTarget)
	{
		mainPass.OnRenderTargetRebuilt(renderTarget);
	}

	public void Dispose()
	{
		mainPass.Dispose();
	}

	object? IRenderFeature.CaptureFrame(in FrameTiming timing)
	{
		return CaptureFrame(in timing);
	}
}
