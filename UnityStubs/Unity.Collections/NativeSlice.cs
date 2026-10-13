using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.Collections;

public struct NativeSlice<T> : IEnumerable<T>, IEnumerable, IEquatable<NativeSlice<T>>
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private NativeSlice<T> _slice;

		private int _index;

		public T Current => _slice[_index];

		object IEnumerator.Current => Current;

		internal Enumerator(NativeSlice<T> slice)
		{
			_slice = slice;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return ++_index < _slice.Length;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	private NativeArray<T> _array;

	private int _start;

	private int _length;

	public int Length => _length;

	public T this[int index]
	{
		get
		{
			CheckIndex(index);
			return _array[_start + index];
		}
		set
		{
			CheckIndex(index);
			_array[_start + index] = value;
		}
	}

	public NativeSlice(NativeArray<T> array, int start, int length)
	{
		if (start < 0 || length < 0 || start + length > array.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		_array = array;
		_start = start;
		_length = length;
	}

	public NativeSlice(NativeArray<T> array, int start)
		: this(array, start, array.Length - start)
	{
	}

	public NativeSlice(NativeArray<T> array)
		: this(array, 0, array.Length)
	{
	}

	public NativeSlice<T> Slice(int start, int length)
	{
		if (start < 0 || length < 0 || start + length > _length)
		{
			throw new ArgumentOutOfRangeException();
		}
		return new NativeSlice<T>(_array, _start + start, length);
	}

	public NativeSlice<T> Slice(int start)
	{
		if ((uint)start > (uint)_length)
		{
			throw new ArgumentOutOfRangeException();
		}
		return new NativeSlice<T>(_array, _start + start, _length - start);
	}

	public void CopyFrom(NativeSlice<T> slice)
	{
		if (slice.Length != _length)
		{
			throw new ArgumentException("Slice length does not match destination length.", "slice");
		}
		for (int i = 0; i < _length; i++)
		{
			this[i] = slice[i];
		}
	}

	public void CopyFrom(T[] array)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		if (array.Length != _length)
		{
			throw new ArgumentException("Source length does not match slice length.", "array");
		}
		for (int i = 0; i < _length; i++)
		{
			this[i] = array[i];
		}
	}

	public void CopyTo(T[] array)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		if (array.Length != _length)
		{
			throw new ArgumentException("Destination length does not match slice length.", "array");
		}
		for (int i = 0; i < _length; i++)
		{
			array[i] = this[i];
		}
	}

	public T[] ToArray()
	{
		T[] array = new T[_length];
		CopyTo(array);
		return array;
	}

	public bool Equals(NativeSlice<T> other)
	{
		if (_array.Equals(other._array) && _start == other._start)
		{
			return _length == other._length;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is NativeSlice<T> other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_array, _start, _length);
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void CheckIndex(int index)
	{
		if ((uint)index >= (uint)_length)
		{
			throw new IndexOutOfRangeException();
		}
	}
}
