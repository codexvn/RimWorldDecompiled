using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Math;

namespace Waylith.Rendering.Culling;

public static class OcclusionQuery
{
	internal sealed class CullTargetState
	{
		public readonly int index;

		public readonly string name;

		public readonly OcclusionQueryCompute.Pyramid pyramid;

		public uint[] verdicts = Array.Empty<uint>();

		public byte[] inFrustum = Array.Empty<byte>();

		public byte[] wasDrawn = Array.Empty<byte>();

		public uint[] gpuVisible = Array.Empty<uint>();

		public byte[] inFrustumWhenTested = Array.Empty<byte>();

		public CameraView[] views = Array.Empty<CameraView>();

		public CameraView cullView;

		public CullTargetState(int index, string name, int pyramidSize)
		{
			this.index = index;
			this.name = name;
			pyramid = new OcclusionQueryCompute.Pyramid(pyramidSize);
		}

		public void Resize(int capacity)
		{
			Array.Resize(ref verdicts, capacity);
			Array.Resize(ref inFrustum, capacity);
			Array.Resize(ref wasDrawn, capacity);
			Array.Resize(ref gpuVisible, capacity);
			Array.Resize(ref inFrustumWhenTested, capacity);
		}
	}

	public struct GpuBox
	{
		public Vector4 Min;

		public Vector4 Max;
	}

	public sealed class BoxSnapshot
	{
		internal GpuBox[] boxes = Array.Empty<GpuBox>();

		internal uint[] ids = Array.Empty<uint>();

		internal byte[][] inFrustum = new byte[2][]
		{
			Array.Empty<byte>(),
			Array.Empty<byte>()
		};

		internal int count;

		internal int dispatchCount;
	}

	public static bool Enabled = true;

	public const float DepthBias = 0.002f;

	private const int CameraPyramidSize = 256;

	private const int ShadowPyramidSize = 256;

	internal const int Main = 0;

	internal const int Shadows = 1;

	internal const int TargetCount = 2;

	internal static readonly CullTargetState[] Targets = new CullTargetState[2]
	{
		new CullTargetState(0, "Camera", 256),
		new CullTargetState(1, "Shadow", 256)
	};

	public static bool Frozen;

	private static GpuBox[] boxes = Array.Empty<GpuBox>();

	private static readonly Stack<int> FreeSlots = new Stack<int>();

	private static readonly ConcurrentStack<BoxSnapshot> CullBoxSnapshotPool = new ConcurrentStack<BoxSnapshot>();

	private static bool[] targetViewValid = Array.Empty<bool>();

	public static int RegisteredCount { get; private set; }

	public static int CulledCount { get; private set; }

	public static int Register()
	{
		CullTargetState[] targets;
		if (FreeSlots.TryPop(out var result))
		{
			targets = Targets;
			foreach (CullTargetState obj in targets)
			{
				obj.verdicts[result] = 1u;
				obj.inFrustum[result] = 1;
			}
			return result;
		}
		if (RegisteredCount == boxes.Length)
		{
			int num = System.Math.Max(1024, boxes.Length * 2);
			Array.Resize(ref boxes, num);
			targets = Targets;
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i].Resize(num);
			}
		}
		targets = Targets;
		foreach (CullTargetState obj2 in targets)
		{
			obj2.verdicts[RegisteredCount] = 1u;
			obj2.inFrustum[RegisteredCount] = 1;
		}
		return RegisteredCount++;
	}

	public static void Release(int handle)
	{
		boxes[handle] = default(GpuBox);
		CullTargetState[] targets = Targets;
		foreach (CullTargetState obj in targets)
		{
			obj.verdicts[handle] = 1u;
			obj.inFrustum[handle] = 0;
			obj.wasDrawn[handle] = 0;
		}
		FreeSlots.Push(handle);
	}

	public static void SetBounds(int handle, in Bounds bounds)
	{
		boxes[handle] = new GpuBox
		{
			Min = new Vector4(bounds.min, 0f),
			Max = new Vector4(bounds.max, 0f)
		};
	}

	public static bool IsVisible(int handle)
	{
		if (Enabled)
		{
			return Targets[0].verdicts[handle] != 0;
		}
		return true;
	}

	public static bool IsShadowVisible(int handle)
	{
		if (Enabled)
		{
			return Targets[1].verdicts[handle] != 0;
		}
		return true;
	}

	public static void SetOnScreen(int handle, bool visible)
	{
		Targets[0].inFrustum[handle] = (visible ? ((byte)1) : ((byte)0));
	}

	public static void SetInShadow(int handle, bool visible)
	{
		Targets[1].inFrustum[handle] = (visible ? ((byte)1) : ((byte)0));
	}

	public static void BeginFrame()
	{
		if (!Enabled || Frozen)
		{
			return;
		}
		System.Math.Min(OcclusionQueryCompute.ReadVerdicts(), RegisteredCount);
		int num = 0;
		CullTargetState[] targets = Targets;
		foreach (CullTargetState cullTargetState in targets)
		{
			for (int j = 0; j < RegisteredCount; j++)
			{
				if (cullTargetState.inFrustumWhenTested[j] != 0)
				{
					cullTargetState.verdicts[j] = cullTargetState.gpuVisible[j];
				}
				else if (cullTargetState.wasDrawn[j] == 0)
				{
					cullTargetState.verdicts[j] = cullTargetState.inFrustum[j];
				}
				bool flag = cullTargetState.verdicts[j] != 0 && cullTargetState.inFrustum[j] != 0;
				cullTargetState.wasDrawn[j] = (flag ? ((byte)1) : ((byte)0));
				if (cullTargetState.index == 0 && !flag)
				{
					num++;
				}
			}
		}
		CulledCount = num;
	}

	public static BoxSnapshot? CaptureBoxes()
	{
		if (!Enabled || Frozen || RegisteredCount == 0)
		{
			return null;
		}
		if (!CullBoxSnapshotPool.TryPop(out BoxSnapshot result))
		{
			result = new BoxSnapshot();
		}
		CullTargetState[] targets;
		if (result.boxes.Length < boxes.Length)
		{
			result.boxes = new GpuBox[boxes.Length];
			result.ids = new uint[boxes.Length];
			targets = Targets;
			foreach (CullTargetState cullTargetState in targets)
			{
				result.inFrustum[cullTargetState.index] = new byte[boxes.Length];
			}
		}
		int num = 0;
		for (int j = 0; j < RegisteredCount; j++)
		{
			bool flag = false;
			targets = Targets;
			foreach (CullTargetState cullTargetState2 in targets)
			{
				flag |= cullTargetState2.inFrustum[j] != 0;
			}
			if (flag)
			{
				result.boxes[num] = boxes[j];
				result.ids[num] = (uint)j;
				num++;
			}
		}
		result.dispatchCount = num;
		targets = Targets;
		foreach (CullTargetState cullTargetState3 in targets)
		{
			cullTargetState3.inFrustum.AsSpan(0, RegisteredCount).CopyTo(result.inFrustum[cullTargetState3.index]);
		}
		result.count = RegisteredCount;
		return result;
	}

	public static void Recycle(BoxSnapshot? snapshot)
	{
		if (snapshot != null)
		{
			CullBoxSnapshotPool.Push(snapshot);
		}
	}

	public static void RecordFrame(GraphicsDevice graphicsDevice, BoxSnapshot? snapshot, RenderTexture? target, int depthIndex, int writeIndex, in CameraView view, in CameraView lightView)
	{
		GpuTextureAllocation gpuTextureAllocation = target?.Color;
		int num = System.Math.Max(depthIndex, writeIndex);
		CullTargetState[] targets;
		if (targetViewValid.Length <= num)
		{
			Array.Resize(ref targetViewValid, num + 1);
			targets = Targets;
			for (int i = 0; i < targets.Length; i++)
			{
				Array.Resize(ref targets[i].views, num + 1);
			}
		}
		bool flag = targetViewValid[depthIndex];
		targets = Targets;
		foreach (CullTargetState cullTargetState in targets)
		{
			cullTargetState.cullView = cullTargetState.views[depthIndex];
			cullTargetState.views[writeIndex] = ((cullTargetState.index == 0) ? view : lightView);
		}
		targetViewValid[writeIndex] = true;
		if (snapshot == null || gpuTextureAllocation == null || !flag)
		{
			return;
		}
		OcclusionQueryCompute.Frame frame = OcclusionQueryCompute.BeginRecord(graphicsDevice, snapshot);
		if (frame == null)
		{
			return;
		}
		VkExtent2D extent = target.Extent;
		uint num2 = (uint)(ShadowMapGlobal.GetDepthSize?.Invoke() ?? 0);
		targets = Targets;
		foreach (CullTargetState cullTargetState2 in targets)
		{
			uint srcHeight;
			uint num5;
			VkImageView srcView;
			if (cullTargetState2.index != 0)
			{
				VkImageView obj = ShadowMapGlobal.GetDepthView?.Invoke(depthIndex) ?? default(VkImageView);
				uint num3 = num2;
				uint num4 = num2;
				srcHeight = num4;
				num5 = num3;
				srcView = obj;
			}
			else
			{
				VkImageView colorView = target.ColorView;
				uint num4 = (uint)extent.Width;
				uint num3 = (uint)extent.Height;
				srcHeight = num3;
				num5 = num4;
				srcView = colorView;
			}
			if (!srcView.IsNull() && num5 != 0)
			{
				OcclusionQueryCompute.FillChain(cullTargetState2, frame, snapshot.dispatchCount, srcView, num5, srcHeight, cullTargetState2.index == 0);
			}
		}
		OcclusionQueryCompute.Submit(gpuTextureAllocation);
	}

	public static void InvalidateDepth()
	{
		Array.Clear(targetViewValid);
	}

	public static void Shutdown()
	{
		OcclusionQueryCompute.Shutdown();
		Array.Clear(targetViewValid);
	}
}
