using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Waylith.Collections;

public sealed class ConcurrentAppendList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly ConcurrentAppendList<T> list;

		private readonly int snapshotCount;

		private int index;

		public T Current { get; private set; }

		object? IEnumerator.Current => Current;

		internal Enumerator(ConcurrentAppendList<T> list, int snapshotCount)
		{
			this.list = list;
			this.snapshotCount = snapshotCount;
			index = -1;
			Current = default(T);
		}

		public bool MoveNext()
		{
			int num = index + 1;
			if (num >= snapshotCount)
			{
				return false;
			}
			index = num;
			Current = list[num];
			return true;
		}

		public void Reset()
		{
			index = -1;
			Current = default(T);
		}

		public void Dispose()
		{
		}
	}

	private const int DefaultChunkSize = 1024;

	private readonly int chunkShift;

	private readonly int chunkMask;

	private readonly Lock writeLock = new Lock();

	private volatile T[]?[] chunks;

	private int count;

	public int Count => Volatile.Read(in count);

	public T this[int index]
	{
		get
		{
			if (!TryGet(index, out var value))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return value;
		}
	}

	public ConcurrentAppendList(int chunkSize = 1024, int initialChunkCapacity = 4)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(chunkSize, 1, "chunkSize");
		ArgumentOutOfRangeException.ThrowIfLessThan(initialChunkCapacity, 1, "initialChunkCapacity");
		int num = (int)BitOperations.RoundUpToPowerOf2((uint)chunkSize);
		chunkShift = BitOperations.TrailingZeroCount(num);
		chunkMask = num - 1;
		chunks = new T[initialChunkCapacity][];
	}

	public int Add(T item)
	{
		using (writeLock.EnterScope())
		{
			int num = count;
			int num2 = num >> chunkShift;
			int num3 = num & chunkMask;
			T[][] array = chunks;
			if (num2 >= array.Length)
			{
				T[][] destinationArray = new T[array.Length * 2][];
				Array.Copy(array, destinationArray, array.Length);
				array = (chunks = destinationArray);
			}
			T[] array2 = array[num2];
			if (array2 == null)
			{
				array2 = (array[num2] = new T[chunkMask + 1]);
			}
			array2[num3] = item;
			Volatile.Write(ref count, num + 1);
			return num;
		}
	}

	public bool TryGet(int index, out T value)
	{
		if ((uint)index >= (uint)Volatile.Read(in count))
		{
			value = default(T);
			return false;
		}
		value = chunks[index >> chunkShift][index & chunkMask];
		return true;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this, Count);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
