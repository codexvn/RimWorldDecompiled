using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Brutal;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Graphics;
using Waylith.Rendering;
using Waylith.Rendering.Compute;

namespace Waylith.Hardware;

public static class GpuProbe
{
	private struct PushConstants
	{
		public uint Iterations;

		public uint ElementCount;

		public uint ThreadStride;

		public uint Pad;
	}

	private sealed class Context
	{
		public required DeviceEx Device;

		public required GraphicsDevice GraphicsDevice;

		public required VkCommandPool CommandPool;

		public required CommandBuffer CommandBuffer;

		public VkQueryPool TimestampPool;

		public double TimestampPeriodNs;

		public bool TimestampsSupported;
	}

	public const int ProbeVersion = 4;

	private const string KernelPath = "Compute/GpuProbe.comp.hlsl";

	private const int ThreadsPerGroup = 64;

	private const int PushConstantSize = 16;

	private const int MaxGroups = 65535;

	private const int FlopsPerIterationPerThread = 16;

	private const int WarmupRuns = 4;

	private const int MeasureRepeats = 5;

	private const int BandwidthMeasureRepeats = 8;

	private const double TargetDispatchMs = 4.0;

	private const double TotalBudgetMs = 1500.0;

	private const long BandwidthBufferBytes = 268435456L;

	private static readonly int[] SweepGroups = new int[6] { 64, 256, 1024, 4096, 16384, 65535 };

	private static readonly ComputeBinding[] Bindings = new ComputeBinding[2]
	{
		new ComputeBinding(0, ComputeBindingType.StorageBuffer),
		new ComputeBinding(1, ComputeBindingType.StorageBuffer)
	};

	public static GpuPerformanceProfile Run(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		DeviceEx device = graphicsDevice.Device;
		VkCommandPool commandPool = default(VkCommandPool);
		VkDescriptorPool vkDescriptorPool = default(VkDescriptorPool);
		VkQueryPool vkQueryPool = default(VkQueryPool);
		GpuComputeBuffer gpuComputeBuffer = null;
		GpuComputeBuffer gpuComputeBuffer2 = null;
		bool shouldAppend;
		try
		{
			ComputeKernel kernel = ComputeKernelCache.Load("Compute/GpuProbe.comp.hlsl", "AluProbeCS", Bindings, 16, ShaderSourceLanguage.Hlsl);
			ComputeKernel kernel2 = ComputeKernelCache.Load("Compute/GpuProbe.comp.hlsl", "BandwidthProbeCS", Bindings, 16, ShaderSourceLanguage.Hlsl);
			VkCommandPoolCreateInfo pCreateInfo = new VkCommandPoolCreateInfo
			{
				QueueFamilyIndex = graphicsDevice.GraphicsFamilyIndex,
				Flags = (VkCommandPoolCreateFlags.TransientBit | VkCommandPoolCreateFlags.ResetCommandBufferBit)
			};
			commandPool = device.CreateCommandPool(in pCreateInfo, device.VkAllocator);
			vkDescriptorPool = CreateDescriptorPool(device, 2);
			CommandBuffer commandBuffer = device.AllocateCommandBuffer(new VkCommandBufferAllocateInfo
			{
				CommandPool = commandPool,
				Level = VkCommandBufferLevel.Primary
			});
			VkPhysicalDeviceLimits limits = graphicsDevice.PhysicalDevice.GetProperties().Limits;
			bool flag = (bool)limits.TimestampComputeAndGraphics && limits.TimestampPeriod > 0f;
			if (flag)
			{
				VkQueryPoolCreateInfo pCreateInfo2 = new VkQueryPoolCreateInfo
				{
					QueryType = VkQueryType.Timestamp,
					QueryCount = 2
				};
				vkQueryPool = device.CreateQueryPool(in pCreateInfo2, device.VkAllocator);
			}
			Context ctx = new Context
			{
				Device = device,
				GraphicsDevice = graphicsDevice,
				CommandPool = commandPool,
				CommandBuffer = commandBuffer,
				TimestampPool = vkQueryPool,
				TimestampPeriodNs = limits.TimestampPeriod,
				TimestampsSupported = flag
			};
			long sizeBytes = 16776960L;
			gpuComputeBuffer = GpuComputeBuffer.Create(device, graphicsDevice.Allocator, "GpuProbe.Out", sizeBytes, VkBufferUsageFlags.None, deviceLocal: true);
			gpuComputeBuffer2 = GpuComputeBuffer.Create(device, graphicsDevice.Allocator, "GpuProbe.In", 268435456L, VkBufferUsageFlags.None, deviceLocal: true);
			VkDescriptorSet set = WriteDescriptorSet(device, vkDescriptorPool, kernel, gpuComputeBuffer, gpuComputeBuffer2);
			VkDescriptorSet set2 = WriteDescriptorSet(device, vkDescriptorPool, kernel2, gpuComputeBuffer, gpuComputeBuffer2);
			Stopwatch stopwatch = Stopwatch.StartNew();
			(float gflops, int saturationThreads) tuple = MeasureAlu(ctx, kernel, set, stopwatch);
			float item = tuple.gflops;
			int item2 = tuple.saturationThreads;
			float num = MeasureBandwidth(ctx, kernel2, set2, stopwatch);
			LogCategory log;
			if (!(item > 0f) || !float.IsFinite(item) || !(num > 0f) || !float.IsFinite(num))
			{
				log = RenderLog.Log;
				LogCategory logCategory = log;
				LogString<Warning> message = new LogString<Warning>(115, 3, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("GpuProbe: incomplete after ");
					message.AppendFormatted(stopwatch.Elapsed.TotalMilliseconds, 0, "0");
					message.AppendLiteral(" ms (");
					message.AppendFormatted(item, 0, "0");
					message.AppendLiteral(" GFLOP/s, ");
					message.AppendFormatted(num, 0, "0.0");
					message.AppendLiteral(" GB/s), using conservative (low) tier this boot and re-probing next boot.");
				}
				logCategory.Warning(message, "Run", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuProbe.cs", 131);
				return GpuPerformanceProfile.Conservative;
			}
			GpuPerformanceProfile gpuPerformanceProfile = default(GpuPerformanceProfile);
			gpuPerformanceProfile.AluGFlopsPerSecond = item;
			gpuPerformanceProfile.MemoryBandwidthGBPerSecond = num;
			gpuPerformanceProfile.AluSaturationThreads = item2;
			gpuPerformanceProfile.Measured = true;
			GpuPerformanceProfile gpuPerformanceProfile2 = gpuPerformanceProfile;
			log = RenderLog.Log;
			LogCategory logCategory2 = log;
			LogString<Information> message2 = new LogString<Information>(28, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("GpuProbe: completed in ");
				message2.AppendFormatted(stopwatch.Elapsed.TotalMilliseconds, 0, "0");
				message2.AppendLiteral(" ms. ");
				message2.AppendFormatted(gpuPerformanceProfile2);
			}
			logCategory2.Info(message2, "Run", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuProbe.cs", 142);
			return gpuPerformanceProfile2;
		}
		catch (Exception value)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory3 = log;
			LogString<Error> message3 = new LogString<Error>(59, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message3.AppendLiteral("GpuProbe: failed, falling back to conservative (low) tier: ");
				message3.AppendFormatted(value);
			}
			logCategory3.Error(message3, "Run", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuProbe.cs", 147);
			return GpuPerformanceProfile.Conservative;
		}
		finally
		{
			gpuComputeBuffer2?.Dispose();
			gpuComputeBuffer?.Dispose();
			if (!vkQueryPool.IsNull())
			{
				device.DestroyQueryPool(vkQueryPool, device.VkAllocator);
			}
			if (!vkDescriptorPool.IsNull())
			{
				device.DestroyDescriptorPool(vkDescriptorPool, device.VkAllocator);
			}
			if (!commandPool.IsNull())
			{
				device.DestroyCommandPool(commandPool, device.VkAllocator);
			}
		}
	}

	private static (float gflops, int saturationThreads) MeasureAlu(Context ctx, ComputeKernel kernel, VkDescriptorSet set, Stopwatch total)
	{
		int num = CalibrateIterations(ctx, kernel, set);
		double num2 = 0.0;
		double[] array = new double[SweepGroups.Length];
		for (int i = 0; i < SweepGroups.Length; i++)
		{
			if (total.Elapsed.TotalMilliseconds > 1500.0)
			{
				break;
			}
			int num3 = SweepGroups[i];
			double num4 = MeasuredDispatchSeconds(ctx, kernel, set, num3, MakeAluPush(num), 5);
			double num5 = (array[i] = (double)num3 * 64.0 * (double)num * 16.0 / num4);
			if (num5 > num2)
			{
				num2 = num5;
			}
		}
		int item = 4194240;
		for (int j = 0; j < SweepGroups.Length; j++)
		{
			if (array[j] >= 0.95 * num2 && array[j] > 0.0)
			{
				item = SweepGroups[j] * 64;
				break;
			}
		}
		return (gflops: (float)(num2 / 1000000000.0), saturationThreads: item);
	}

	private static float MeasureBandwidth(Context ctx, ComputeKernel kernel, VkDescriptorSet set, Stopwatch total)
	{
		uint elementCount = 67108864u;
		PushConstants pushConstants = default(PushConstants);
		pushConstants.ElementCount = elementCount;
		pushConstants.ThreadStride = 4194240u;
		PushConstants push = pushConstants;
		MinDispatchSeconds(ctx, kernel, set, 65535, push, 2);
		if (total.Elapsed.TotalMilliseconds > 1500.0)
		{
			return 0f;
		}
		double num = MeasuredDispatchSeconds(ctx, kernel, set, 65535, push, 8);
		if (!(num > 0.0))
		{
			return 0f;
		}
		return (float)(268435456.0 / num / 1000000000.0);
	}

	private static int CalibrateIterations(Context ctx, ComputeKernel kernel, VkDescriptorSet set)
	{
		int num = 512;
		for (int i = 0; i < 4; i++)
		{
			MinDispatchSeconds(ctx, kernel, set, 65535, MakeAluPush(num), 1);
		}
		double num2 = MeasuredDispatchSeconds(ctx, kernel, set, 65535, MakeAluPush(num), 2);
		if (num2 <= 0.0 && ctx.TimestampsSupported)
		{
			RenderLog.Log.Warning("GpuProbe: GPU timestamps did not advance across a dispatch, timing with the wall clock instead.", "CalibrateIterations", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Hardware\\GpuProbe.cs", 229);
			ctx.TimestampsSupported = false;
			num2 = MinDispatchSeconds(ctx, kernel, set, 65535, MakeAluPush(num), 2);
		}
		double num3 = 0.004 / System.Math.Max(num2, 1E-06);
		return System.Math.Clamp((int)((double)num * num3), 256, 2000000);
	}

	private static PushConstants MakeAluPush(int iterations)
	{
		PushConstants result = default(PushConstants);
		result.Iterations = (uint)iterations;
		return result;
	}

	private static double MeasuredDispatchSeconds(Context ctx, ComputeKernel kernel, VkDescriptorSet set, int groups, PushConstants push, int repeats)
	{
		if (!ctx.TimestampsSupported)
		{
			return MinDispatchSeconds(ctx, kernel, set, groups, push, repeats);
		}
		return MinDispatchGpuSeconds(ctx, kernel, set, groups, push, repeats);
	}

	private static double MinDispatchSeconds(Context ctx, ComputeKernel kernel, VkDescriptorSet set, int groups, PushConstants push, int repeats)
	{
		Span<byte> span = stackalloc byte[16];
		MemoryMarshal.Write(span, in push);
		double num = double.MaxValue;
		for (int i = 0; i < repeats; i++)
		{
			CommandBuffer commandBuffer = ctx.CommandBuffer;
			commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
			commandBuffer.BindPipeline(VkPipelineBindPoint.Compute, kernel.Pipeline);
			commandBuffer.BindDescriptorSet(VkPipelineBindPoint.Compute, kernel.PipelineLayout, 0, set, default(Span<ByteSize32>));
			commandBuffer.PushConstants<byte>(kernel.PipelineLayout, VkShaderStageFlags.ComputeBit, ByteSize.Zero, span);
			commandBuffer.Dispatch(groups, 1, 1);
			commandBuffer.End();
			VkFence vkFence = ctx.Device.CreateFence(new VkFenceCreateInfo(), ctx.Device.VkAllocator);
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				GraphicsDevice graphicsDevice = ctx.GraphicsDevice;
				CommandBuffer reference = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), vkFence);
				ctx.Device.WaitForFence(vkFence, -1);
				stopwatch.Stop();
				double totalSeconds = stopwatch.Elapsed.TotalSeconds;
				if (totalSeconds < num)
				{
					num = totalSeconds;
				}
			}
			finally
			{
				ctx.Device.DestroyFence(vkFence, ctx.Device.VkAllocator);
			}
		}
		return num;
	}

	private static double MinDispatchGpuSeconds(Context ctx, ComputeKernel kernel, VkDescriptorSet set, int groups, PushConstants push, int repeats)
	{
		Span<byte> span = stackalloc byte[16];
		MemoryMarshal.Write(span, in push);
		Span<byte> span2 = stackalloc byte[16];
		double num = double.MaxValue;
		for (int i = 0; i < repeats; i++)
		{
			CommandBuffer commandBuffer = ctx.CommandBuffer;
			commandBuffer.Begin(VkCommandBufferUsageFlags.OneTimeSubmitBit);
			commandBuffer.ResetQueryPool(ctx.TimestampPool, 0, 2);
			commandBuffer.WriteTimestamp(VkPipelineStageFlags.TopOfPipeBit, ctx.TimestampPool, 0);
			commandBuffer.BindPipeline(VkPipelineBindPoint.Compute, kernel.Pipeline);
			commandBuffer.BindDescriptorSet(VkPipelineBindPoint.Compute, kernel.PipelineLayout, 0, set, default(Span<ByteSize32>));
			commandBuffer.PushConstants<byte>(kernel.PipelineLayout, VkShaderStageFlags.ComputeBit, ByteSize.Zero, span);
			commandBuffer.Dispatch(groups, 1, 1);
			commandBuffer.WriteTimestamp(VkPipelineStageFlags.BottomOfPipeBit, ctx.TimestampPool, 1);
			commandBuffer.End();
			VkFence vkFence = ctx.Device.CreateFence(new VkFenceCreateInfo(), ctx.Device.VkAllocator);
			try
			{
				GraphicsDevice graphicsDevice = ctx.GraphicsDevice;
				CommandBuffer reference = commandBuffer;
				graphicsDevice.SubmitGraphics(default(Span<VkSemaphore>), default(Span<VkPipelineStageFlags>), new Span<CommandBuffer>(ref reference), default(Span<VkSemaphore>), vkFence);
				ctx.Device.WaitForFence(vkFence, -1);
				ctx.Device.GetQueryPoolResults(ctx.TimestampPool, 0, 2, span2, (ByteSize64)8, VkQueryResultFlags._64Bit | VkQueryResultFlags.WaitBit);
				ulong num2 = MemoryMarshal.Read<ulong>(span2);
				ulong num3 = MemoryMarshal.Read<ulong>(span2.Slice(8));
				if (num3 > num2)
				{
					double num4 = (double)(num3 - num2) * ctx.TimestampPeriodNs * 1E-09;
					if (num4 > 0.0 && num4 < num)
					{
						num = num4;
					}
				}
			}
			finally
			{
				ctx.Device.DestroyFence(vkFence, ctx.Device.VkAllocator);
			}
		}
		if (num != double.MaxValue)
		{
			return num;
		}
		return 0.0;
	}

	private unsafe static VkDescriptorSet WriteDescriptorSet(DeviceEx device, VkDescriptorPool pool, ComputeKernel kernel, GpuComputeBuffer outBuffer, GpuComputeBuffer inBuffer)
	{
		VkDescriptorSet vkDescriptorSet = DescriptorSetWriter.AllocateDescriptorSet(device, pool, kernel.DescriptorSetLayout);
		VkDescriptorBufferInfo* ptr = stackalloc VkDescriptorBufferInfo[2];
		*ptr = new VkDescriptorBufferInfo
		{
			Buffer = outBuffer.Buffer,
			Offset = ByteSize.Zero,
			Range = outBuffer.Size
		};
		ptr[1] = new VkDescriptorBufferInfo
		{
			Buffer = inBuffer.Buffer,
			Offset = ByteSize.Zero,
			Range = inBuffer.Size
		};
		VkWriteDescriptorSet* ptr2 = stackalloc VkWriteDescriptorSet[2];
		*ptr2 = new VkWriteDescriptorSet
		{
			DstSet = vkDescriptorSet,
			DstBinding = 0,
			DescriptorCount = 1,
			DescriptorType = VkDescriptorType.StorageBuffer,
			BufferInfo = ptr
		};
		ptr2[1] = new VkWriteDescriptorSet
		{
			DstSet = vkDescriptorSet,
			DstBinding = 1,
			DescriptorCount = 1,
			DescriptorType = VkDescriptorType.StorageBuffer,
			BufferInfo = ptr + 1
		};
		device.UpdateDescriptorSets(new ReadOnlySpan<VkWriteDescriptorSet>(ptr2, 2), default(ReadOnlySpan<VkCopyDescriptorSet>));
		return vkDescriptorSet;
	}

	private unsafe static VkDescriptorPool CreateDescriptorPool(DeviceEx device, int maxSets)
	{
		VkDescriptorPoolSize* ptr = stackalloc VkDescriptorPoolSize[1];
		*ptr = new VkDescriptorPoolSize
		{
			Type = VkDescriptorType.StorageBuffer,
			DescriptorCount = maxSets * 2
		};
		VkDescriptorPoolCreateInfo pCreateInfo = new VkDescriptorPoolCreateInfo
		{
			MaxSets = maxSets,
			PoolSizeCount = 1,
			PoolSizes = ptr
		};
		return device.CreateDescriptorPool(in pCreateInfo, device.VkAllocator);
	}
}
