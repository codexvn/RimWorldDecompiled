using System;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.PostFx;

namespace RimWorldWaylith.Rendering;

public sealed class WaterDepthTargets : IDisposable
{
	private const VkFormat ColorFormat = VkFormat.R16SFloat;

	private readonly DeviceEx device;

	private readonly VkRenderPass renderPass;

	private readonly Texture2D output = new Texture2D(1, 1)
	{
		Name = "WaterDepthOutput",
		filterMode = FilterMode.Bilinear,
		wrapMode = TextureWrapMode.Clamp
	};

	private RenderTexture? target;

	private PassAttachment? attachment;

	private int width;

	private int height;

	private int framesInFlight;

	public VkRenderPass RenderPass => renderPass;

	public PassAttachment? Attachment => attachment;

	public Texture Output => output;

	public WaterDepthTargets(GraphicsDevice graphicsDevice)
	{
		device = graphicsDevice.Device;
		renderPass = PostFxPassHelpers.CreateColorRenderPass(device, VkFormat.R16SFloat, VkFormat.Undefined, VkAttachmentLoadOp.Clear);
	}

	public void EnsureSized(int framesInFlight, int w, int h)
	{
		w = Math.Max(1, w);
		h = Math.Max(1, h);
		if (target == null || width != w || height != h || this.framesInFlight != framesInFlight)
		{
			width = w;
			height = h;
			this.framesInFlight = framesInFlight;
			if (target == null)
			{
				target = new RenderTexture(w, h, VkFormat.R16SFloat, VkFormat.Undefined, RenderTextureFilter.Bilinear, RenderTextureWrap.Clamp, randomWrite: false, "WaterDepth");
			}
			else
			{
				target.Resize(w, h);
			}
			output.width = w;
			output.height = h;
			output.GpuResource = target.Color;
			RebuildAttachment();
		}
	}

	private void RebuildAttachment()
	{
		VkFramebuffer framebuffer = target.GetFramebuffer(renderPass);
		int num = Math.Max(1, framesInFlight);
		VkFramebuffer[] array = new VkFramebuffer[num];
		VkImageView[] depthViews = new VkImageView[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = framebuffer;
		}
		attachment = new PassAttachment(clearValues: new VkClearValue[1]
		{
			new VkClearColorValue
			{
				Float32 = new float4(0f, 0f, 0f, 0f)
			}
		}, renderPass: renderPass, framebuffers: array, depthViews: depthViews, extent: new VkExtent2D(width, height), sampleCount: VkSampleCountFlags._1Bit, colorFormat: VkFormat.R16SFloat, depthFormat: VkFormat.Undefined);
	}

	public void MarkSampleable()
	{
		if (target != null)
		{
			target.Color.CurrentLayout = VkImageLayout.ShaderReadOnlyOptimal;
		}
	}

	public void Dispose()
	{
		output.GpuResource = null;
		target?.Dispose();
		target = null;
		device.DestroyRenderPass(renderPass, device.VkAllocator);
	}
}
