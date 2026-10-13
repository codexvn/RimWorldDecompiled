using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Waylith.Collections;

public class DenseDictionary<TKey, TValue> where TKey : IRefEquatable<TKey>
{
	private struct Bucket
	{
		public uint distAndFingerprint;

		public int entryIndex;
	}

	public struct Enumerator
	{
		private readonly DenseDictionary<TKey, TValue> dict;

		private int index;

		public EntryRef Current => new EntryRef(dict, index);

		internal Enumerator(DenseDictionary<TKey, TValue> dict)
		{
			this.dict = dict;
			index = -1;
		}

		public bool MoveNext()
		{
			return ++index < dict.count;
		}
	}

	public readonly struct EntryRef
	{
		private readonly DenseDictionary<TKey, TValue> dict;

		private readonly int index;

		public ref readonly TKey Key => ref dict.keys[index];

		public ref readonly TValue Value => ref dict.values[index];

		internal EntryRef(DenseDictionary<TKey, TValue> dict, int index)
		{
			this.dict = dict;
			this.index = index;
		}
	}

	private const uint DistInc = 256u;

	private const uint FingerprintMask = 255u;

	private const int MinBuckets = 16;

	private Bucket[] buckets;

	private TKey[] keys;

	private TValue[] values;

	private int count;

	private int shift;

	public int Count => count;

	public TValue this[in TKey key]
	{
		get
		{
			if (!TryGetValue(in key, out var value))
			{
				throw new KeyNotFoundException();
			}
			return value;
		}
		set
		{
			if (count == keys.Length)
			{
				Grow();
			}
			uint num = MixHash(in key);
			uint num2 = 0x100 | (num & 0xFF);
			int num3 = (int)(num >> shift);
			while (true)
			{
				ref Bucket reference = ref buckets[num3];
				if (reference.distAndFingerprint == num2 && keys[reference.entryIndex].Equals(in key))
				{
					values[reference.entryIndex] = value;
					return;
				}
				if (reference.distAndFingerprint < num2)
				{
					break;
				}
				num2 += 256;
				num3 = Next(num3);
			}
			keys[count] = key;
			values[count] = value;
			PlaceAndShiftUp(new Bucket
			{
				distAndFingerprint = num2,
				entryIndex = count
			}, num3);
			count++;
		}
	}

	public DenseDictionary(int capacity = 0)
	{
		int num = 16;
		while (MaxEntriesFor(num) < capacity)
		{
			num <<= 1;
		}
		AllocateBuckets(num);
		keys = new TKey[MaxEntriesFor(num)];
		values = new TValue[keys.Length];
	}

	private static int MaxEntriesFor(int numBuckets)
	{
		return numBuckets * 4 / 5;
	}

	[MemberNotNull("buckets")]
	private void AllocateBuckets(int numBuckets)
	{
		buckets = new Bucket[numBuckets];
		shift = 32;
		for (int num = numBuckets; num > 1; num >>= 1)
		{
			shift--;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int Next(int bucketIdx)
	{
		if (bucketIdx + 1 != buckets.Length)
		{
			return bucketIdx + 1;
		}
		return 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint MixHash(in TKey key)
	{
		return MixBits((uint)key.GetHashCode());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint MixBits(uint h)
	{
		h ^= h >> 16;
		h *= 2146121005;
		h ^= h >> 15;
		h *= 2221713035u;
		h ^= h >> 16;
		return h;
	}

	public bool TryGetValueBy<TLookup>(in TLookup lookup, out TValue value) where TLookup : IRefLookup<TKey>
	{
		uint num = MixBits((uint)lookup.KeyHash());
		uint num2 = 0x100 | (num & 0xFF);
		int num3 = (int)(num >> shift);
		while (true)
		{
			ref Bucket reference = ref buckets[num3];
			if (reference.distAndFingerprint == num2 && lookup.KeyEquals(in keys[reference.entryIndex]))
			{
				value = values[reference.entryIndex];
				return true;
			}
			if (reference.distAndFingerprint < num2)
			{
				break;
			}
			num2 += 256;
			num3 = Next(num3);
		}
		value = default(TValue);
		return false;
	}

	public void SetBy<TLookup>(in TLookup lookup, TValue value) where TLookup : IRefLookup<TKey>
	{
		if (count == keys.Length)
		{
			Grow();
		}
		uint num = MixBits((uint)lookup.KeyHash());
		uint num2 = 0x100 | (num & 0xFF);
		int num3 = (int)(num >> shift);
		while (true)
		{
			ref Bucket reference = ref buckets[num3];
			if (reference.distAndFingerprint == num2 && lookup.KeyEquals(in keys[reference.entryIndex]))
			{
				values[reference.entryIndex] = value;
				return;
			}
			if (reference.distAndFingerprint < num2)
			{
				break;
			}
			num2 += 256;
			num3 = Next(num3);
		}
		keys[count] = lookup.ToKey();
		values[count] = value;
		PlaceAndShiftUp(new Bucket
		{
			distAndFingerprint = num2,
			entryIndex = count
		}, num3);
		count++;
	}

	public bool TryGetValue(in TKey key, out TValue value)
	{
		uint num = MixHash(in key);
		uint num2 = 0x100 | (num & 0xFF);
		int num3 = (int)(num >> shift);
		while (true)
		{
			ref Bucket reference = ref buckets[num3];
			if (reference.distAndFingerprint == num2 && keys[reference.entryIndex].Equals(in key))
			{
				value = values[reference.entryIndex];
				return true;
			}
			if (reference.distAndFingerprint < num2)
			{
				break;
			}
			num2 += 256;
			num3 = Next(num3);
		}
		value = default(TValue);
		return false;
	}

	public bool ContainsKey(in TKey key)
	{
		TValue value;
		return TryGetValue(in key, out value);
	}

	private void PlaceAndShiftUp(Bucket b, int idx)
	{
		while (buckets[idx].distAndFingerprint != 0)
		{
			ref Bucket reference = ref buckets[idx];
			Bucket bucket = buckets[idx];
			Bucket bucket2 = b;
			b = bucket;
			reference = bucket2;
			b.distAndFingerprint += 256u;
			idx = Next(idx);
		}
		buckets[idx] = b;
	}

	private void Grow()
	{
		TKey[] sourceArray = keys;
		TValue[] sourceArray2 = values;
		AllocateBuckets(buckets.Length << 1);
		keys = new TKey[MaxEntriesFor(buckets.Length)];
		values = new TValue[keys.Length];
		Array.Copy(sourceArray, keys, count);
		Array.Copy(sourceArray2, values, count);
		for (int i = 0; i < count; i++)
		{
			uint num = MixHash(in keys[i]);
			uint num2 = 0x100 | (num & 0xFF);
			int num3 = (int)(num >> shift);
			while (num2 <= buckets[num3].distAndFingerprint)
			{
				num2 += 256;
				num3 = Next(num3);
			}
			PlaceAndShiftUp(new Bucket
			{
				distAndFingerprint = num2,
				entryIndex = i
			}, num3);
		}
	}

	public bool Remove(in TKey key)
	{
		uint num = MixHash(in key);
		uint num2 = 0x100 | (num & 0xFF);
		int num3 = (int)(num >> shift);
		while (true)
		{
			ref Bucket reference = ref buckets[num3];
			if (reference.distAndFingerprint == num2 && keys[reference.entryIndex].Equals(in key))
			{
				break;
			}
			if (reference.distAndFingerprint < num2)
			{
				return false;
			}
			num2 += 256;
			num3 = Next(num3);
		}
		RemoveBucketAndEntry(num3);
		return true;
	}

	public ref readonly TKey KeyAt(int denseIndex)
	{
		return ref keys[denseIndex];
	}

	public ref readonly TValue ValueAt(int denseIndex)
	{
		return ref values[denseIndex];
	}

	public void RemoveAt(int denseIndex)
	{
		if ((uint)denseIndex >= (uint)count)
		{
			throw new ArgumentOutOfRangeException("denseIndex");
		}
		int num = (int)(MixHash(in keys[denseIndex]) >> shift);
		while (buckets[num].entryIndex != denseIndex)
		{
			num = Next(num);
		}
		RemoveBucketAndEntry(num);
	}

	private void RemoveBucketAndEntry(int idx)
	{
		int entryIndex = buckets[idx].entryIndex;
		int num = Next(idx);
		while (buckets[num].distAndFingerprint >= 512)
		{
			buckets[idx] = new Bucket
			{
				distAndFingerprint = buckets[num].distAndFingerprint - 256,
				entryIndex = buckets[num].entryIndex
			};
			idx = num;
			num = Next(idx);
		}
		buckets[idx] = default(Bucket);
		int num2 = count - 1;
		if (entryIndex != num2)
		{
			keys[entryIndex] = keys[num2];
			values[entryIndex] = values[num2];
			int num3 = (int)(MixHash(in keys[entryIndex]) >> shift);
			while (buckets[num3].entryIndex != num2)
			{
				num3 = Next(num3);
			}
			buckets[num3].entryIndex = entryIndex;
		}
		keys[num2] = default(TKey);
		values[num2] = default(TValue);
		count--;
	}

	public void Clear()
	{
		if (count != 0)
		{
			Array.Clear(buckets, 0, buckets.Length);
			Array.Clear(keys, 0, count);
			Array.Clear(values, 0, count);
			count = 0;
		}
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}
}
