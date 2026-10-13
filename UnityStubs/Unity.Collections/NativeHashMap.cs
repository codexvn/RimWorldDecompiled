using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

namespace Unity.Collections;

public struct NativeHashMap<TKey, TValue> : INativeDisposable, IDisposable, IEnumerable<KVPair<TKey, TValue>>, IEnumerable
{
	public struct Enumerator : IEnumerator<KVPair<TKey, TValue>>, IEnumerator, IDisposable
	{
		private Dictionary<TKey, TValue>.Enumerator _inner;

		public KVPair<TKey, TValue> Current
		{
			get
			{
				KeyValuePair<TKey, TValue> current = _inner.Current;
				return new KVPair<TKey, TValue>(current.Key, current.Value);
			}
		}

		object IEnumerator.Current => Current;

		internal Enumerator(Dictionary<TKey, TValue> dict)
		{
			_inner = dict.GetEnumerator();
		}

		public bool MoveNext()
		{
			return _inner.MoveNext();
		}

		public void Reset()
		{
			((IEnumerator)_inner).Reset();
		}

		public void Dispose()
		{
			_inner.Dispose();
		}
	}

	private static readonly Dictionary<TKey, TValue> Empty = new Dictionary<TKey, TValue>();

	private Dictionary<TKey, TValue> _dict;

	public const int MaxCapacity = 536870912;

	public bool IsCreated => _dict != null;

	public bool IsEmpty
	{
		get
		{
			if (_dict != null)
			{
				return _dict.Count == 0;
			}
			return true;
		}
	}

	public int Count => _dict?.Count ?? 0;

	public TValue this[TKey key]
	{
		get
		{
			return _dict[key];
		}
		set
		{
			_dict[key] = value;
		}
	}

	public NativeHashMap(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
	{
		_dict = new Dictionary<TKey, TValue>(Math.Max(0, initialCapacity));
	}

	public void Add(TKey key, TValue item)
	{
		_dict.Add(key, item);
	}

	public bool TryAdd(TKey key, TValue item)
	{
		return _dict.TryAdd(key, item);
	}

	public bool Remove(TKey key)
	{
		if (_dict != null)
		{
			return _dict.Remove(key);
		}
		return false;
	}

	public bool ContainsKey(TKey key)
	{
		if (_dict != null)
		{
			return _dict.ContainsKey(key);
		}
		return false;
	}

	public void Clear()
	{
		_dict?.Clear();
	}

	public bool TryGetValue(TKey key, out TValue item)
	{
		if (_dict != null)
		{
			return _dict.TryGetValue(key, out item);
		}
		item = default(TValue);
		return false;
	}

	public JobHandle Dispose(JobHandle inputDeps)
	{
		Dispose();
		return inputDeps;
	}

	public void Dispose()
	{
		_dict?.Clear();
		_dict = null;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(_dict ?? Empty);
	}

	IEnumerator<KVPair<TKey, TValue>> IEnumerable<KVPair<TKey, TValue>>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
