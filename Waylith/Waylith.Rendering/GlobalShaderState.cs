using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Waylith.Math;

namespace Waylith.Rendering;

public static class GlobalShaderState
{
	private sealed class GlobalArray
	{
		public byte[] Bytes = Array.Empty<byte>();

		public int Stride;

		public int Generation;

		public byte[]? Snapshot;

		public int SnapshotGeneration;
	}

	private static readonly ReaderWriterLockSlim Sync = new ReaderWriterLockSlim();

	private static readonly Dictionary<int, float> Floats = new Dictionary<int, float>();

	private static readonly Dictionary<int, Vector4> Vectors = new Dictionary<int, Vector4>();

	private static readonly Dictionary<int, Matrix4x4> Matrices = new Dictionary<int, Matrix4x4>();

	private static readonly Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();

	private static readonly HashSet<string> Keywords = new HashSet<string>(StringComparer.Ordinal);

	private static string keywordKeyCache = string.Empty;

	private static bool keywordsDirty;

	private static long version;

	private static long registrationVersion;

	private static readonly Dictionary<int, GlobalArray> Arrays = new Dictionary<int, GlobalArray>();

	private static int arrayCount;

	public static string GlobalKeywordKey
	{
		get
		{
			if (!keywordsDirty)
			{
				return keywordKeyCache;
			}
			string[] array = new string[Keywords.Count];
			Keywords.CopyTo(array);
			Array.Sort(array, (IComparer<string>?)StringComparer.Ordinal);
			keywordKeyCache = string.Join(";", array);
			keywordsDirty = false;
			return keywordKeyCache;
		}
	}

	public static long Version => Interlocked.Read(in version);

	public static long RegistrationVersion => Interlocked.Read(in registrationVersion);

	public static bool HasArrays => Volatile.Read(in arrayCount) > 0;

	public static void EnableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword) && Keywords.Add(keyword))
		{
			keywordsDirty = true;
		}
	}

	public static void DisableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword) && Keywords.Remove(keyword))
		{
			keywordsDirty = true;
		}
	}

	public static void SetFloat(int id, float value)
	{
		Sync.EnterWriteLock();
		try
		{
			if (Floats.TryGetValue(id, out var value2))
			{
				if (value2.Equals(value))
				{
					return;
				}
			}
			else
			{
				Interlocked.Increment(ref registrationVersion);
			}
			Floats[id] = value;
			Interlocked.Increment(ref version);
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	public static void SetVector(int id, Vector4 value)
	{
		Sync.EnterWriteLock();
		try
		{
			if (Vectors.TryGetValue(id, out var value2))
			{
				if (value2.Equals(value))
				{
					return;
				}
			}
			else
			{
				Interlocked.Increment(ref registrationVersion);
			}
			Vectors[id] = value;
			Interlocked.Increment(ref version);
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	public static void SetMatrix(int id, Matrix4x4 value)
	{
		Sync.EnterWriteLock();
		try
		{
			if (Matrices.TryGetValue(id, out var value2))
			{
				if (value2.Equals(value))
				{
					return;
				}
			}
			else
			{
				Interlocked.Increment(ref registrationVersion);
			}
			Matrices[id] = value;
			Interlocked.Increment(ref version);
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	public static void SetTexture(int id, Texture value)
	{
		Sync.EnterWriteLock();
		try
		{
			if (Textures.TryGetValue(id, out Texture value2))
			{
				if (value2 == value)
				{
					return;
				}
			}
			else
			{
				Interlocked.Increment(ref registrationVersion);
			}
			Textures[id] = value;
			Interlocked.Increment(ref version);
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	public static bool TryGetFloat(int id, out float value)
	{
		Sync.EnterReadLock();
		try
		{
			return Floats.TryGetValue(id, out value);
		}
		finally
		{
			Sync.ExitReadLock();
		}
	}

	public static bool TryGetVector(int id, out Vector4 value)
	{
		Sync.EnterReadLock();
		try
		{
			return Vectors.TryGetValue(id, out value);
		}
		finally
		{
			Sync.ExitReadLock();
		}
	}

	public static bool TryGetMatrix(int id, out Matrix4x4 value)
	{
		Sync.EnterReadLock();
		try
		{
			return Matrices.TryGetValue(id, out value);
		}
		finally
		{
			Sync.ExitReadLock();
		}
	}

	public static bool TryGetTexture(int id, out Texture? value)
	{
		Sync.EnterReadLock();
		try
		{
			return Textures.TryGetValue(id, out value);
		}
		finally
		{
			Sync.ExitReadLock();
		}
	}

	public static void SetFloatArray(int id, ReadOnlySpan<float> values)
	{
		SetArray(id, MemoryMarshal.AsBytes(values), 4);
	}

	public static void SetVectorArray(int id, ReadOnlySpan<Vector4> values)
	{
		SetArray(id, MemoryMarshal.AsBytes(values), 16);
	}

	public static void SetMatrixArray(int id, ReadOnlySpan<Matrix4x4> values)
	{
		SetArray(id, MemoryMarshal.AsBytes(values), 64);
	}

	private static void SetArray(int id, ReadOnlySpan<byte> bytes, int stride)
	{
		if (bytes.IsEmpty)
		{
			return;
		}
		Sync.EnterWriteLock();
		try
		{
			if (!Arrays.TryGetValue(id, out GlobalArray value))
			{
				value = (Arrays[id] = new GlobalArray());
				Volatile.Write(ref arrayCount, Arrays.Count);
				Interlocked.Increment(ref registrationVersion);
			}
			else if (value.Stride == stride && bytes.SequenceEqual(value.Bytes))
			{
				return;
			}
			if (value.Bytes.Length != bytes.Length)
			{
				value.Bytes = new byte[bytes.Length];
			}
			bytes.CopyTo(value.Bytes);
			value.Stride = stride;
			value.Generation++;
			Interlocked.Increment(ref version);
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	internal static bool TryCopyArray(int id, Span<byte> destination, int destinationStride, int elementBytes, int length)
	{
		Sync.EnterReadLock();
		try
		{
			if (!Arrays.TryGetValue(id, out GlobalArray value))
			{
				return false;
			}
			CopyArrayElements(value.Bytes, value.Stride, destination, destinationStride, elementBytes, length);
			return true;
		}
		finally
		{
			Sync.ExitReadLock();
		}
	}

	internal static void CopyArrayElements(ReadOnlySpan<byte> source, int sourceStride, Span<byte> destination, int destinationStride, int elementBytes, int length)
	{
		int num = System.Math.Min(elementBytes, sourceStride);
		int num2 = System.Math.Min(length, source.Length / sourceStride);
		for (int i = 0; i < num2 && i * destinationStride + num <= destination.Length; i++)
		{
			source.Slice(i * sourceStride, num).CopyTo(destination.Slice(i * destinationStride));
		}
	}

	internal static bool TryGetArraySnapshot(int id, out byte[] bytes, out int generation)
	{
		Sync.EnterWriteLock();
		try
		{
			if (!Arrays.TryGetValue(id, out GlobalArray value))
			{
				bytes = Array.Empty<byte>();
				generation = 0;
				return false;
			}
			if (value.Snapshot == null || value.SnapshotGeneration != value.Generation)
			{
				value.Snapshot = (byte[])value.Bytes.Clone();
				value.SnapshotGeneration = value.Generation;
			}
			bytes = value.Snapshot;
			generation = value.Generation;
			return true;
		}
		finally
		{
			Sync.ExitWriteLock();
		}
	}

	public static void SetFloat(string name, float value)
	{
		SetFloat(ShaderPropertyId.GetOrCreate(name), value);
	}

	public static void SetVector(string name, Vector4 value)
	{
		SetVector(ShaderPropertyId.GetOrCreate(name), value);
	}

	public static void SetMatrix(string name, Matrix4x4 value)
	{
		SetMatrix(ShaderPropertyId.GetOrCreate(name), value);
	}

	public static void SetTexture(string name, Texture value)
	{
		SetTexture(ShaderPropertyId.GetOrCreate(name), value);
	}

	public static void SetColor(int id, Color value)
	{
		SetVector(id, value);
	}

	public static void SetColor(string name, Color value)
	{
		SetColor(ShaderPropertyId.GetOrCreate(name), value);
	}
}
