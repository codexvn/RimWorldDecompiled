using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Brutal.VulkanApi.Vma;
using Waylith.Graphics;
using Waylith.Math;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering.Culling;

internal static class OcclusionQueryCompute
{
	internal sealed class Pyramid
	{
		public readonly int size;

		public readonly int levelCount;

		public readonly int totalCells;

		public readonly int[] offsets;

		public int ReducePasses => (levelCount - 1 + 3) / 4;

		public int Dispatches => 2 + ReducePasses + 1;

		public Pyramid(int size)
		{
			this.size = size;
			for (int num = size; num >= 4; num >>= 1)
			{
				levelCount++;
			}
			offsets = new int[levelCount];
			int num2 = size;
			for (int i = 0; i < levelCount; i++)
			{
				offsets[i] = totalCells;
				totalCells += num2 * num2;
				num2 >>= 1;
			}
		}

		public int MidSizeFor(uint srcWidth, uint srcHeight)
		{
			uint num = System.Math.Max(srcWidth, srcHeight);
			int num2;
			for (num2 = 1; num2 < num; num2 <<= 1)
			{
			}
			return System.Math.Clamp(num2 / 4, size, 1024);
		}
	}

	internal sealed class Frame
	{
		public GpuComputeBuffer? boxes;

		public GpuComputeBuffer? ids;

		public GpuComputeBuffer? mid;

		public readonly GpuComputeBuffer?[] hzb = new GpuComputeBuffer[2];

		public readonly GpuComputeBuffer?[] verdicts = new GpuComputeBuffer[2];

		public readonly byte[][] inFrustum = new byte[2][]
		{
			Array.Empty<byte>(),
			Array.Empty<byte>()
		};

		public int boxCount;
	}

	private struct CullPush
	{
		public Matrix4x4 ViewProjection;

		public Vector4 CameraPos;

		public uint BoxCount;

		public uint PyramidSize;

		public uint MipCount;

		public uint LinearDepth;
	}

	private struct HzbBasePush
	{
		public uint DepthWidth;

		public uint DepthHeight;

		public uint DstSize;

		public uint LinearDepth;
	}

	private struct HzbMidPush
	{
		public uint SrcSize;

		public uint DstSize;

		public uint DstOffset;

		public uint Padding;
	}

	private struct HzbReducePush
	{
		public uint SrcOffset;

		public uint SrcSize;

		public uint Dst0Offset;

		public uint Dst0Size;

		public uint Dst1Offset;

		public uint Dst1Size;

		public uint Dst2Offset;

		public uint Dst2Size;

		public uint Dst3Offset;

		public uint Dst3Size;

		public uint LevelCount;
	}

	private const string CullKernelPath = "Compute/OcclusionCull.comp.glsl";

	private const string HzbBaseKernelPath = "Compute/OcclusionHzbBase.comp.glsl";

	private const string HzbMidKernelPath = "Compute/OcclusionHzbMid.comp.glsl";

	private const string HzbReduceKernelPath = "Compute/OcclusionHzbReduce.comp.glsl";

	private const int CullGroupSize = 64;

	private const int HzbGroupSize = 8;

	private const int MidRatio = 4;

	private const int MaxMidSize = 1024;

	private const int MinLevelSize = 4;

	private const int Submissions = 2;

	private static readonly ComputeBinding[] HzbBaseBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.CombinedImageSampler),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] HzbMidBindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] HzbReduceBindings = new ComputeBinding[1]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer)
	};

	private static readonly ComputeBinding[] CullBindings = new ComputeBinding[4]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer),
		new ComputeBinding(2, ComputeBindingType.StorageBuffer),
		new ComputeBinding(3, ComputeBindingType.StorageBuffer)
	};

	private static ComputeKernel? cullKernel;

	private static ComputeKernel? hzbBaseKernel;

	private static ComputeKernel? hzbMidKernel;

	private static ComputeKernel? hzbReduceKernel;

	private static ComputeSubmissionRing<Frame>? ring;

	private static ComputeSubmissionRing<Frame>.Slot? recordingSlot;

	private static int slotCapacity;

	private static ComputeDispatch[] dispatchChain = Array.Empty<ComputeDispatch>();

	private static int chainCount;

	private static readonly Action<CommandBuffer> DepthBarrier = RecordDepthReadBarrier;

	private static VkImage barrierImage;

	private static void RecordDepthReadBarrier(CommandBuffer cmd)
	{
		VkImageMemoryBarrier vkImageMemoryBarrier = new VkImageMemoryBarrier();
		vkImageMemoryBarrier.SrcAccessMask = VkAccessFlags.ColorAttachmentWriteBit;
		vkImageMemoryBarrier.DstAccessMask = VkAccessFlags.ShaderReadBit;
		vkImageMemoryBarrier.OldLayout = VkImageLayout.ShaderReadOnlyOptimal;
		vkImageMemoryBarrier.NewLayout = VkImageLayout.ShaderReadOnlyOptimal;
		vkImageMemoryBarrier.SrcQueueFamilyIndex = -1;
		vkImageMemoryBarrier.DstQueueFamilyIndex = -1;
		vkImageMemoryBarrier.Image = barrierImage;
		vkImageMemoryBarrier.SubresourceRange = new VkImageSubresourceRange
		{
			AspectMask = VkImageAspectFlags.ColorBit,
			BaseMipLevel = 0,
			LevelCount = 1,
			BaseArrayLayer = 0,
			LayerCount = 1
		};
		VkImageMemoryBarrier vkImageMemoryBarrier2 = vkImageMemoryBarrier;
		VkImageMemoryBarrier reference = vkImageMemoryBarrier2;
		cmd.PipelineBarrier(VkPipelineStageFlags.ColorAttachmentOutputBit, VkPipelineStageFlags.ComputeShaderBit, VkDependencyFlags.None, default(ReadOnlySpan<VkMemoryBarrier>), default(ReadOnlySpan<VkBufferMemoryBarrier>), new ReadOnlySpan<VkImageMemoryBarrier>(in reference));
	}

	internal static Frame? BeginRecord(GraphicsDevice graphicsDevice, OcclusionQuery.BoxSnapshot snapshot)
	{
		EnsureResources(graphicsDevice, snapshot.count);
		ring.Advance();
		ComputeSubmissionRing<Frame>.Slot current = ring.Current;
		if (current.InFlight)
		{
			return null;
		}
		recordingSlot = current;
		chainCount = 0;
		Frame data = current.Data;
		if (snapshot.dispatchCount > 0)
		{
			data.boxes.Upload<OcclusionQuery.GpuBox>(snapshot.boxes.AsSpan(0, snapshot.dispatchCount), 0L);
			data.ids.Upload<uint>(snapshot.ids.AsSpan(0, snapshot.dispatchCount), 0L);
		}
		OcclusionQuery.CullTargetState[] targets = OcclusionQuery.Targets;
		foreach (OcclusionQuery.CullTargetState cullTargetState in targets)
		{
			if (data.inFrustum[cullTargetState.index].Length < snapshot.count)
			{
				data.inFrustum[cullTargetState.index] = new byte[snapshot.count];
			}
			snapshot.inFrustum[cullTargetState.index].AsSpan(0, snapshot.count).CopyTo(data.inFrustum[cullTargetState.index]);
		}
		data.boxCount = snapshot.count;
		return data;
	}

	internal static void FillChain(OcclusionQuery.CullTargetState cull, Frame frame, int dispatchCount, VkImageView srcView, uint srcWidth, uint srcHeight, bool linearDepth)
	{
		Pyramid pyramid = cull.pyramid;
		GpuComputeBuffer buffer = frame.hzb[cull.index];
		int num = pyramid.MidSizeFor(srcWidth, srcHeight);
		int num2 = (num + 8 - 1) / 8;
		ComputeDispatch computeDispatch = ComputeDispatch.Rent(hzbBaseKernel, num2, num2);
		computeDispatch.SetSampledView(0, srcView, ComputeDispatchQueue.PointClampSampler, linearDepth ? VkImageLayout.ShaderReadOnlyOptimal : VkImageLayout.DepthStencilReadOnlyOptimal);
		computeDispatch.SetStorageBuffer(1, frame.mid);
		computeDispatch.SetPushConstants<HzbBasePush>(new HzbBasePush
		{
			DepthWidth = srcWidth,
			DepthHeight = srcHeight,
			DstSize = (uint)num,
			LinearDepth = (linearDepth ? 1u : 0u)
		});
		dispatchChain[chainCount++] = computeDispatch;
		int num3 = (pyramid.size + 8 - 1) / 8;
		ComputeDispatch computeDispatch2 = ComputeDispatch.Rent(hzbMidKernel, num3, num3);
		computeDispatch2.SetStorageBuffer(0, frame.mid);
		computeDispatch2.SetStorageBuffer(1, buffer);
		computeDispatch2.SetPushConstants<HzbMidPush>(new HzbMidPush
		{
			SrcSize = (uint)num,
			DstSize = (uint)pyramid.size,
			DstOffset = (uint)pyramid.offsets[0]
		});
		dispatchChain[chainCount++] = computeDispatch2;
		for (int i = 1; i < pyramid.levelCount; i += 4)
		{
			int levelCount = System.Math.Min(4, pyramid.levelCount - i);
			int num4 = ((pyramid.size >> i) + 8 - 1) / 8;
			ComputeDispatch computeDispatch3 = ComputeDispatch.Rent(hzbReduceKernel, num4, num4);
			computeDispatch3.SetStorageBuffer(0, buffer);
			HzbReducePush hzbReducePush = default(HzbReducePush);
			hzbReducePush.SrcOffset = (uint)pyramid.offsets[i - 1];
			hzbReducePush.SrcSize = (uint)(pyramid.size >> i - 1);
			hzbReducePush.LevelCount = (uint)levelCount;
			HzbReducePush push = hzbReducePush;
			SetReduceLevel(ref push, pyramid, i, 0);
			SetReduceLevel(ref push, pyramid, i, 1);
			SetReduceLevel(ref push, pyramid, i, 2);
			SetReduceLevel(ref push, pyramid, i, 3);
			computeDispatch3.SetPushConstants(in push);
			dispatchChain[chainCount++] = computeDispatch3;
		}
		if (dispatchCount != 0)
		{
			CameraView cullView = cull.cullView;
			Matrix4x4 result;
			Vector3 value = (Matrix4x4.Invert(cullView.View, out result) ? result.GetPosition() : Vector3.Zero);
			ComputeDispatch computeDispatch4 = ComputeDispatch.Rent(cullKernel, (dispatchCount + 64 - 1) / 64, 1);
			computeDispatch4.SetStorageBuffer(0, buffer);
			computeDispatch4.SetStorageBuffer(1, frame.boxes);
			computeDispatch4.SetStorageBuffer(2, frame.verdicts[cull.index]);
			computeDispatch4.SetStorageBuffer(3, frame.ids);
			computeDispatch4.SetPushConstants<CullPush>(new CullPush
			{
				ViewProjection = cullView.ViewProjection,
				CameraPos = new Vector4(value, 0.002f),
				BoxCount = (uint)dispatchCount,
				PyramidSize = (uint)pyramid.size,
				MipCount = (uint)pyramid.levelCount,
				LinearDepth = (linearDepth ? 1u : 0u)
			});
			dispatchChain[chainCount++] = computeDispatch4;
		}
	}

	private static void SetReduceLevel(ref HzbReducePush push, Pyramid p, int first, int slot)
	{
		int num = System.Math.Min(first + slot, p.levelCount - 1);
		uint num2 = (uint)p.offsets[num];
		uint num3 = (uint)(p.size >> num);
		switch (slot)
		{
		case 0:
			push.Dst0Offset = num2;
			push.Dst0Size = num3;
			break;
		case 1:
			push.Dst1Offset = num2;
			push.Dst1Size = num3;
			break;
		case 2:
			push.Dst2Offset = num2;
			push.Dst2Size = num3;
			break;
		default:
			push.Dst3Offset = num2;
			push.Dst3Size = num3;
			break;
		}
	}

	internal static void Submit(GpuTextureAllocation source)
	{
		barrierImage = source.Image;
		ring.Submit(recordingSlot, dispatchChain.AsSpan(0, chainCount), DepthBarrier);
		recordingSlot = null;
	}

	internal static int ReadVerdicts()
	{
		if (ring == null)
		{
			return 0;
		}
		Frame frame = null;
		long num = long.MinValue;
		for (int i = 0; i < ring.Count; i++)
		{
			ComputeSubmissionRing<Frame>.Slot slot = ring[i];
			if (ring.IsSignalled(slot) && slot.Sequence > num)
			{
				num = slot.Sequence;
				frame = slot.Data;
			}
		}
		int num2 = frame?.boxCount ?? 0;
		OcclusionQuery.CullTargetState[] targets = OcclusionQuery.Targets;
		foreach (OcclusionQuery.CullTargetState cullTargetState in targets)
		{
			if (num2 == 0)
			{
				break;
			}
			if (cullTargetState.gpuVisible.Length < num2)
			{
				Array.Resize(ref cullTargetState.gpuVisible, num2);
				Array.Resize(ref cullTargetState.inFrustumWhenTested, num2);
			}
			frame.verdicts[cullTargetState.index].Read(MemoryMarshal.AsBytes(cullTargetState.gpuVisible.AsSpan(0, num2)), 0L);
			frame.inFrustum[cullTargetState.index].AsSpan(0, num2).CopyTo(cullTargetState.inFrustumWhenTested);
		}
		for (int k = 0; k < ring.Count; k++)
		{
			if (ring.IsSignalled(ring[k]))
			{
				ring.Release(ring[k]);
			}
		}
		return num2;
	}

	private static void EnsureResources(GraphicsDevice graphicsDevice, int boxCount)
	{
		DeviceEx device = graphicsDevice.Device;
		VmaAllocator allocator = graphicsDevice.Allocator;
		if (cullKernel == null)
		{
			cullKernel = ComputeKernelCache.Load("Compute/OcclusionCull.comp.glsl", "main", CullBindings, Marshal.SizeOf<CullPush>());
		}
		if (hzbBaseKernel == null)
		{
			hzbBaseKernel = ComputeKernelCache.Load("Compute/OcclusionHzbBase.comp.glsl", "main", HzbBaseBindings, Marshal.SizeOf<HzbBasePush>());
		}
		if (hzbMidKernel == null)
		{
			hzbMidKernel = ComputeKernelCache.Load("Compute/OcclusionHzbMid.comp.glsl", "main", HzbMidBindings, Marshal.SizeOf<HzbMidPush>());
		}
		if (hzbReduceKernel == null)
		{
			hzbReduceKernel = ComputeKernelCache.Load("Compute/OcclusionHzbReduce.comp.glsl", "main", HzbReduceBindings, Marshal.SizeOf<HzbReducePush>());
		}
		if (ring == null)
		{
			int num = 0;
			OcclusionQuery.CullTargetState[] targets = OcclusionQuery.Targets;
			foreach (OcclusionQuery.CullTargetState cullTargetState in targets)
			{
				num += cullTargetState.pyramid.Dispatches;
			}
			dispatchChain = new ComputeDispatch[num];
			ring = new ComputeSubmissionRing<Frame>(graphicsDevice, 2, dispatchChain.Length);
			for (int j = 0; j < ring.Count; j++)
			{
				Frame data = ring[j].Data;
				targets = OcclusionQuery.Targets;
				foreach (OcclusionQuery.CullTargetState cullTargetState2 in targets)
				{
					data.hzb[cullTargetState2.index] = GpuComputeBuffer.Create(device, allocator, "Occlusion." + cullTargetState2.name + ".Hzb", (long)cullTargetState2.pyramid.totalCells * 4L, VkBufferUsageFlags.None, deviceLocal: true);
				}
				data.mid = GpuComputeBuffer.Create(device, allocator, "Occlusion.Hzb.Mid", 4194304L, VkBufferUsageFlags.None, deviceLocal: true);
			}
		}
		if (slotCapacity >= boxCount)
		{
			return;
		}
		slotCapacity = System.Math.Max(1024, boxCount * 2);
		for (int k = 0; k < ring.Count; k++)
		{
			Frame data2 = ring[k].Data;
			data2.boxes?.Dispose();
			data2.ids?.Dispose();
			data2.boxes = GpuComputeBuffer.Create(device, allocator, "Occlusion.Boxes", (long)slotCapacity * (long)Marshal.SizeOf<OcclusionQuery.GpuBox>());
			data2.ids = GpuComputeBuffer.Create(device, allocator, "Occlusion.Ids", (long)slotCapacity * 4L);
			OcclusionQuery.CullTargetState[] targets = OcclusionQuery.Targets;
			foreach (OcclusionQuery.CullTargetState cullTargetState3 in targets)
			{
				data2.verdicts[cullTargetState3.index]?.Dispose();
				data2.verdicts[cullTargetState3.index] = GpuComputeBuffer.Create(device, allocator, "Occlusion." + cullTargetState3.name + ".Verdicts", (long)slotCapacity * 4L);
			}
			data2.boxCount = 0;
		}
	}

	internal static void Shutdown()
	{
		if (ring != null)
		{
			for (int i = 0; i < ring.Count; i++)
			{
				Frame data = ring[i].Data;
				data.boxes?.Dispose();
				data.ids?.Dispose();
				data.mid?.Dispose();
				OcclusionQuery.CullTargetState[] targets = OcclusionQuery.Targets;
				foreach (OcclusionQuery.CullTargetState cullTargetState in targets)
				{
					data.verdicts[cullTargetState.index]?.Dispose();
					data.hzb[cullTargetState.index]?.Dispose();
				}
			}
			ring.Dispose();
			ring = null;
		}
		recordingSlot = null;
		slotCapacity = 0;
		chainCount = 0;
	}
}
