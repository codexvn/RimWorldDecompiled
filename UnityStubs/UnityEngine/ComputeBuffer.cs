using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Waylith.Rendering;

namespace UnityEngine;

public class ComputeBuffer : IDisposable
{
	private readonly InstanceDataBuffer data = new InstanceDataBuffer();

	private int lastGeneration = -1;

	private readonly List<(WeakReference<Material> material, int nameID)> boundMaterials = new List<(WeakReference<Material>, int)>();

	public int stride { get; }

	public int count { get; }

	public ComputeBufferType type { get; }

	internal InstanceDataBuffer Data => data;

	internal int LastGeneration => lastGeneration;

	internal ReadOnlySpan<byte> CurrentBytes
	{
		get
		{
			if (lastGeneration >= 0)
			{
				return data.GetBytes(lastGeneration);
			}
			return ReadOnlySpan<byte>.Empty;
		}
	}

	public ComputeBuffer(int count, int stride)
	{
		this.count = count;
		this.stride = stride;
	}

	public ComputeBuffer(int count, int stride, ComputeBufferType type)
	{
		this.count = count;
		this.stride = stride;
		this.type = type;
	}

	public unsafe void SetData<T0>(NativeArray<T0> data)
	{
		int num = UnsafeUtility.SizeOf<T0>();
		ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(data.GetUnsafeReadOnlyPtrInternal(), data.Length * num);
		lastGeneration = this.data.Write(bytes, num, data.Length);
		UploadToBoundMaterials();
	}

	public unsafe void SetData<T0>(NativeArray<T0> data, int nativeBufferStartIndex, int computeBufferStartIndex, int count)
	{
		int num = UnsafeUtility.SizeOf<T0>();
		byte* pointer = (byte*)data.GetUnsafeReadOnlyPtrInternal() + (long)nativeBufferStartIndex * (long)num;
		WriteRange(new ReadOnlySpan<byte>(pointer, count * num), num, computeBufferStartIndex, count);
	}

	public void SetData(Array data)
	{
		SetData(data, 0, 0, data.Length);
	}

	public unsafe void SetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
	{
		int num = RuntimeHelpers.SizeOf(data.GetType().GetElementType().TypeHandle);
		fixed (byte* arrayDataReference = &MemoryMarshal.GetArrayDataReference(data))
		{
			ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(arrayDataReference + (long)managedBufferStartIndex * (long)num, count * num);
			WriteRange(bytes, num, computeBufferStartIndex, count);
		}
	}

	public void GetData(Array data)
	{
		GetData(data, 0, 0, data?.Length ?? 0);
	}

	public unsafe void GetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count)
	{
		ArgumentNullException.ThrowIfNull(data, "data");
		int num = RuntimeHelpers.SizeOf(data.GetType().GetElementType().TypeHandle);
		ReadOnlySpan<byte> readOnlySpan = ((lastGeneration < 0) ? ReadOnlySpan<byte>.Empty : this.data.GetBytes(lastGeneration));
		int num2 = computeBufferStartIndex * num;
		int num3 = Math.Clamp(readOnlySpan.Length - num2, 0, count * num);
		fixed (byte* arrayDataReference = &MemoryMarshal.GetArrayDataReference(data))
		{
			Span<byte> destination = new Span<byte>(arrayDataReference + (long)managedBufferStartIndex * (long)num, count * num);
			destination.Clear();
			if (num3 > 0)
			{
				readOnlySpan.Slice(num2, num3).CopyTo(destination);
			}
		}
	}

	private void WriteRange(ReadOnlySpan<byte> bytes, int elementSize, int computeBufferStartIndex, int count)
	{
		if (computeBufferStartIndex <= 0)
		{
			lastGeneration = data.Write(bytes, elementSize, count);
		}
		else
		{
			ReadOnlySpan<byte> bytes2 = data.GetBytes(lastGeneration);
			int num = computeBufferStartIndex * elementSize;
			byte[] array = new byte[Math.Max(bytes2.Length, num + bytes.Length)];
			bytes2.CopyTo(array);
			bytes.CopyTo(array.AsSpan(num));
			lastGeneration = data.Write(array, elementSize, array.Length / elementSize);
		}
		UploadToBoundMaterials();
	}

	internal void Bind(Material material, int nameID)
	{
		boundMaterials.Add((new WeakReference<Material>(material), nameID));
		material.UploadBuffer(nameID, data.GetBytes(lastGeneration));
	}

	internal void Unbind(Material material, int nameID)
	{
		boundMaterials.RemoveAll(((WeakReference<Material> material, int nameID) b) => b.nameID == nameID && (!b.material.TryGetTarget(out var target) || (object)target == material));
	}

	private void UploadToBoundMaterials()
	{
		if (boundMaterials.Count == 0)
		{
			return;
		}
		ReadOnlySpan<byte> bytes = data.GetBytes(lastGeneration);
		for (int num = boundMaterials.Count - 1; num >= 0; num--)
		{
			if (boundMaterials[num].material.TryGetTarget(out var target))
			{
				target.UploadBuffer(boundMaterials[num].nameID, bytes);
			}
			else
			{
				boundMaterials.RemoveAt(num);
			}
		}
	}

	public void Release()
	{
	}

	public void Dispose()
	{
	}
}
