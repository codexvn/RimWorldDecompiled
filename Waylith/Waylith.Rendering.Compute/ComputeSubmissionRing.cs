using System;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;

namespace Waylith.Rendering.Compute;

public sealed class ComputeSubmissionRing<T> : IDisposable where T : class, new()
{
	public sealed class Slot
	{
		public readonly T Data = new T();

		public volatile bool InFlight;

		public long Sequence;

		internal CommandBuffer Cmd;

		internal VkFence Fence;

		internal VkDescriptorPool Pool;
	}

	private readonly GraphicsDevice graphicsDevice;

	private readonly DeviceEx device;

	private readonly VkCommandPool commandPool;

	private readonly Slot[] slots;

	private int cursor;

	private long sequence;

	public int Count => slots.Length;

	public Slot this[int index] => slots[index];

	public Slot Current => slots[cursor];

	public ComputeSubmissionRing(GraphicsDevice graphicsDevice, int slotCount, int maxDispatchesPerSubmission)
	{
		this.graphicsDevice = graphicsDevice;
		device = graphicsDevice.Device;
		DeviceEx handle = device;
		VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
		{
			QueueFamilyIndex = graphicsDevice.GraphicsQueue.Family,
			Flags = (VkCommandPoolCreateFlags.TransientBit | VkCommandPoolCreateFlags.ResetCommandBufferBit)
		};
		commandPool = handle.CreateCommandPool(in pCreateInfo, device.VkAllocator);
		slots = new Slot[slotCount];
		for (int i = 0; i < slotCount; i++)
		{
			slots[i] = new Slot
			{
				Cmd = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
				{
					CommandPool = commandPool,
					Level = VkCommandBufferLevel.Primary
				}),
				Fence = device.CreateFence(new VkFenceCreateInfo(), device.VkAllocator),
				Pool = ComputeDispatchQueue.CreateDescriptorPool(maxDispatchesPerSubmission)
			};
		}
	}

	public Slot Advance()
	{
		cursor = (cursor + 1) % slots.Length;
		return slots[(cursor + 1) % slots.Length];
	}

	public bool IsSignalled(Slot slot)
	{
		if (slot.InFlight)
		{
			return device.GetFenceStatus(slot.Fence) == VkResult.Success;
		}
		return false;
	}

	public void Release(Slot slot)
	{
		DeviceEx handle = device;
		VkFence reference = slot.Fence;
		handle.ResetFences(new ReadOnlySpan<VkFence>(in reference));
		slot.InFlight = false;
	}

	public void Submit(Slot slot, ReadOnlySpan<ComputeDispatch> chain, Action<CommandBuffer>? prologue = null)
	{
		device.ResetDescriptorPool(slot.Pool, 0);
		CommandBuffer cmd = slot.Cmd;
		cmd.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
		prologue?.Invoke(cmd);
		for (int i = 0; i < chain.Length; i++)
		{
			if (i > 0)
			{
				ComputeDispatchQueue.RecordComputeChainBarrier(cmd);
			}
			ComputeDispatchQueue.RecordDispatch(cmd, device, slot.Pool, chain[i]);
			ComputeDispatch.Return(chain[i]);
		}
		VkMemoryBarrier vkMemoryBarrier = new VkMemoryBarrier();
		vkMemoryBarrier.SrcAccessMask = VkAccessFlags.ShaderWriteBit;
		vkMemoryBarrier.DstAccessMask = VkAccessFlags.HostReadBit;
		VkMemoryBarrier reference = vkMemoryBarrier;
		Span<VkMemoryBarrier> span = new Span<VkMemoryBarrier>(ref reference);
		cmd.PipelineBarrier(VkPipelineStageFlags.ComputeShaderBit, VkPipelineStageFlags.HostBit, VkDependencyFlags.None, span, Array.Empty<VkBufferMemoryBarrier>(), default(Span<VkImageMemoryBarrier>));
		cmd.End();
		GraphicsDevice obj = graphicsDevice;
		CommandBuffer reference2 = cmd;
		obj.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference2), default(Span<VkSemaphore>), slot.Fence);
		slot.Sequence = ++sequence;
		slot.InFlight = true;
	}

	public void Dispose()
	{
		Slot[] array = slots;
		foreach (Slot slot in array)
		{
			if (slot.InFlight)
			{
				device.WaitForFence(slot.Fence, -1);
			}
			device.DestroyFence(slot.Fence, device.VkAllocator);
			device.DestroyDescriptorPool(slot.Pool, device.VkAllocator);
		}
		device.DestroyCommandPool(commandPool, device.VkAllocator);
	}
}
