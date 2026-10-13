using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x02003E9C RID: 16028
	public class StringTrie<TValue> : IDictionary<string, TValue>, ICollection<KeyValuePair<string, TValue>>, IEnumerable<KeyValuePair<string, TValue>>, IEnumerable
	{
		// Token: 0x06015F2B RID: 89899 RVA: 0x0068FD8C File Offset: 0x0068DF8C
		public StringTrie(IEqualityComparer<char> comparer)
		{
			this._trie = new Trie<char, TValue>(comparer);
		}

		// Token: 0x06015F2C RID: 89900 RVA: 0x0068FDA0 File Offset: 0x0068DFA0
		public StringTrie() : this(EqualityComparer<char>.Default)
		{
		}

		// Token: 0x170036F1 RID: 14065
		// (get) Token: 0x06015F2D RID: 89901 RVA: 0x0068FDAD File Offset: 0x0068DFAD
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170036F2 RID: 14066
		// (get) Token: 0x06015F2E RID: 89902 RVA: 0x0068FDBA File Offset: 0x0068DFBA
		public ICollection<string> Keys
		{
			get
			{
				return (from i in this._trie.Keys
				select new string(i.ToArray<char>())).ToArray<string>();
			}
		}

		// Token: 0x170036F3 RID: 14067
		// (get) Token: 0x06015F2F RID: 89903 RVA: 0x0068FDF0 File Offset: 0x0068DFF0
		public ICollection<TValue> Values
		{
			get
			{
				return this._trie.Values.ToArray<TValue>();
			}
		}

		// Token: 0x170036F4 RID: 14068
		// (get) Token: 0x06015F30 RID: 89904 RVA: 0x00002501 File Offset: 0x00000701
		bool ICollection<KeyValuePair<string, !0>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170036F5 RID: 14069
		public TValue this[string key]
		{
			get
			{
				return this._trie[key];
			}
			set
			{
				this._trie[key] = value;
			}
		}

		// Token: 0x06015F33 RID: 89907 RVA: 0x0068FE1F File Offset: 0x0068E01F
		public void Add(string key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._trie.Add(key, value);
		}

		// Token: 0x06015F34 RID: 89908 RVA: 0x0068FE3C File Offset: 0x0068E03C
		public void AddRange(IEnumerable<StringEntry<TValue>> collection)
		{
			foreach (StringEntry<TValue> stringEntry in collection)
			{
				this._trie.Add(stringEntry.Key, stringEntry.Value);
			}
		}

		// Token: 0x06015F35 RID: 89909 RVA: 0x0068FE98 File Offset: 0x0068E098
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x06015F36 RID: 89910 RVA: 0x0068FEA5 File Offset: 0x0068E0A5
		public bool ContainsKey(string key)
		{
			return this._trie.ContainsKey(key);
		}

		// Token: 0x06015F37 RID: 89911 RVA: 0x0068FEB3 File Offset: 0x0068E0B3
		public IEnumerable<StringEntry<TValue>> GetByPrefix(string prefix)
		{
			return from i in this._trie.GetByPrefix(prefix)
			select new StringEntry<TValue>(new string(i.Key.ToArray<char>()), i.Value);
		}

		// Token: 0x06015F38 RID: 89912 RVA: 0x0068FEE5 File Offset: 0x0068E0E5
		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return (from i in this._trie
			select new KeyValuePair<string, TValue>(new string(i.Key.ToArray<char>()), i.Value)).GetEnumerator();
		}

		// Token: 0x06015F39 RID: 89913 RVA: 0x0068FF16 File Offset: 0x0068E116
		public bool Remove(string key)
		{
			return this._trie.Remove(key);
		}

		// Token: 0x06015F3A RID: 89914 RVA: 0x0068FF24 File Offset: 0x0068E124
		public bool TryGetValue(string key, out TValue value)
		{
			return this._trie.TryGetValue(key, out value);
		}

		// Token: 0x06015F3B RID: 89915 RVA: 0x0068FF33 File Offset: 0x0068E133
		void ICollection<KeyValuePair<string, !0>>.Add(KeyValuePair<string, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06015F3C RID: 89916 RVA: 0x0068FF49 File Offset: 0x0068E149
		bool ICollection<KeyValuePair<string, !0>>.Contains(KeyValuePair<string, TValue> item)
		{
			return ((ICollection<KeyValuePair<IEnumerable<char>, TValue>>)this._trie).Contains(new KeyValuePair<IEnumerable<char>, TValue>(item.Key, item.Value));
		}

		// Token: 0x06015F3D RID: 89917 RVA: 0x0068FF69 File Offset: 0x0068E169
		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
		{
			Array.Copy((from i in this._trie
			select new KeyValuePair<string, TValue>(new string(i.Key.ToArray<char>()), i.Value)).ToArray<KeyValuePair<string, TValue>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06015F3E RID: 89918 RVA: 0x0068FFA8 File Offset: 0x0068E1A8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06015F3F RID: 89919 RVA: 0x0068FFB0 File Offset: 0x0068E1B0
		bool ICollection<KeyValuePair<string, !0>>.Remove(KeyValuePair<string, TValue> item)
		{
			return ((ICollection<KeyValuePair<IEnumerable<char>, TValue>>)this._trie).Remove(new KeyValuePair<IEnumerable<char>, TValue>(item.Key, item.Value));
		}

		// Token: 0x0400F34F RID: 62287
		private readonly Trie<char, TValue> _trie;
	}
}
