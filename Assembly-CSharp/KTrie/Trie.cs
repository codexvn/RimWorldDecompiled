using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x02003EA0 RID: 16032
	public class Trie<TKey, TValue> : IDictionary<IEnumerable<TKey>, TValue>, ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>, IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>>, IEnumerable
	{
		// Token: 0x06015F58 RID: 89944 RVA: 0x00690150 File Offset: 0x0068E350
		public Trie() : this(EqualityComparer<TKey>.Default)
		{
		}

		// Token: 0x06015F59 RID: 89945 RVA: 0x0069015D File Offset: 0x0068E35D
		public Trie(IEqualityComparer<TKey> comparer)
		{
			this._trie = new TrieSet<TKey>(comparer);
		}

		// Token: 0x170036F8 RID: 14072
		// (get) Token: 0x06015F5A RID: 89946 RVA: 0x00690171 File Offset: 0x0068E371
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170036F9 RID: 14073
		// (get) Token: 0x06015F5B RID: 89947 RVA: 0x00002501 File Offset: 0x00000701
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170036FA RID: 14074
		// (get) Token: 0x06015F5C RID: 89948 RVA: 0x0069017E File Offset: 0x0068E37E
		public ICollection<IEnumerable<TKey>> Keys
		{
			get
			{
				return this._trie.ToList<IEnumerable<TKey>>();
			}
		}

		// Token: 0x170036FB RID: 14075
		// (get) Token: 0x06015F5D RID: 89949 RVA: 0x0069018B File Offset: 0x0068E38B
		public ICollection<TValue> Values
		{
			get
			{
				return (from Trie<TKey, TValue>.TrieEntryPrivate te in this._trie
				select te.Value).ToArray<TValue>();
			}
		}

		// Token: 0x170036FC RID: 14076
		public TValue this[IEnumerable<TKey> key]
		{
			get
			{
				TValue result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException("The given key was not present in the trie.");
			}
			set
			{
				IEnumerable<TKey> enumerable;
				if (this._trie.TryGetItem(key, out enumerable))
				{
					((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value = value;
					return;
				}
				this.Add(key, value);
			}
		}

		// Token: 0x06015F60 RID: 89952 RVA: 0x0069021A File Offset: 0x0068E41A
		public IEnumerable<TrieEntry<TKey, TValue>> GetByPrefix(IEnumerable<TKey> prefix)
		{
			return from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie.GetByPrefix(prefix)
			select new TrieEntry<TKey, TValue>(i, i.Value);
		}

		// Token: 0x06015F61 RID: 89953 RVA: 0x00690251 File Offset: 0x0068E451
		public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
		{
			return (from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie
			select new KeyValuePair<IEnumerable<TKey>, TValue>(i, i.Value)).GetEnumerator();
		}

		// Token: 0x06015F62 RID: 89954 RVA: 0x00690287 File Offset: 0x0068E487
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06015F63 RID: 89955 RVA: 0x0069028F File Offset: 0x0068E48F
		void ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Add(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06015F64 RID: 89956 RVA: 0x006902A5 File Offset: 0x0068E4A5
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x06015F65 RID: 89957 RVA: 0x006902B4 File Offset: 0x0068E4B4
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Contains(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			IEnumerable<TKey> enumerable;
			if (this._trie.TryGetItem(item.Key, out enumerable))
			{
				TValue value = ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value;
				if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015F66 RID: 89958 RVA: 0x006902FC File Offset: 0x0068E4FC
		public void CopyTo(KeyValuePair<IEnumerable<TKey>, TValue>[] array, int arrayIndex)
		{
			Array.Copy((from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie
			select new KeyValuePair<IEnumerable<TKey>, TValue>(i, i.Value)).ToArray<KeyValuePair<IEnumerable<TKey>, TValue>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06015F67 RID: 89959 RVA: 0x0069034C File Offset: 0x0068E54C
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Remove(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			IEnumerable<TKey> enumerable;
			if (this._trie.TryGetItem(item.Key, out enumerable))
			{
				TValue value = ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value;
				if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
				{
					return this.Remove(item.Key);
				}
			}
			return false;
		}

		// Token: 0x06015F68 RID: 89960 RVA: 0x0069039E File Offset: 0x0068E59E
		public bool ContainsKey(IEnumerable<TKey> key)
		{
			return this._trie.Contains(key);
		}

		// Token: 0x06015F69 RID: 89961 RVA: 0x006903AC File Offset: 0x0068E5AC
		public void Add(IEnumerable<TKey> key, TValue value)
		{
			this._trie.Add(new Trie<TKey, TValue>.TrieEntryPrivate(key)
			{
				Value = value
			});
		}

		// Token: 0x06015F6A RID: 89962 RVA: 0x006903C6 File Offset: 0x0068E5C6
		public bool Remove(IEnumerable<TKey> key)
		{
			return this._trie.Remove(key);
		}

		// Token: 0x06015F6B RID: 89963 RVA: 0x006903D4 File Offset: 0x0068E5D4
		public bool TryGetValue(IEnumerable<TKey> key, out TValue value)
		{
			IEnumerable<TKey> enumerable;
			bool flag = this._trie.TryGetItem(key, out enumerable);
			value = (flag ? ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value : default(TValue));
			return flag;
		}

		// Token: 0x0400F35A RID: 62298
		private readonly TrieSet<TKey> _trie;

		// Token: 0x02003EA1 RID: 16033
		private sealed class TrieEntryPrivate : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x06015F6C RID: 89964 RVA: 0x00690410 File Offset: 0x0068E610
			public TrieEntryPrivate(IEnumerable<TKey> key)
			{
				this.Key = key;
			}

			// Token: 0x170036FD RID: 14077
			// (get) Token: 0x06015F6D RID: 89965 RVA: 0x0069041F File Offset: 0x0068E61F
			private IEnumerable<TKey> Key { get; }

			// Token: 0x170036FE RID: 14078
			// (get) Token: 0x06015F6E RID: 89966 RVA: 0x00690427 File Offset: 0x0068E627
			// (set) Token: 0x06015F6F RID: 89967 RVA: 0x0069042F File Offset: 0x0068E62F
			public TValue Value { get; set; }

			// Token: 0x06015F70 RID: 89968 RVA: 0x00690438 File Offset: 0x0068E638
			public IEnumerator<TKey> GetEnumerator()
			{
				return this.Key.GetEnumerator();
			}

			// Token: 0x06015F71 RID: 89969 RVA: 0x00690445 File Offset: 0x0068E645
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
