using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x02003E9E RID: 16030
	public class StringTrieSet : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06015F46 RID: 89926 RVA: 0x00690027 File Offset: 0x0068E227
		public StringTrieSet() : this(EqualityComparer<char>.Default)
		{
		}

		// Token: 0x06015F47 RID: 89927 RVA: 0x00690034 File Offset: 0x0068E234
		public StringTrieSet(IEqualityComparer<char> comparer)
		{
			this._trie = new TrieSet<char>(comparer);
		}

		// Token: 0x170036F6 RID: 14070
		// (get) Token: 0x06015F48 RID: 89928 RVA: 0x00690048 File Offset: 0x0068E248
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170036F7 RID: 14071
		// (get) Token: 0x06015F49 RID: 89929 RVA: 0x00002501 File Offset: 0x00000701
		bool ICollection<string>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015F4A RID: 89930 RVA: 0x00690055 File Offset: 0x0068E255
		public IEnumerable<string> GetByPrefix(string prefix)
		{
			return from c in this._trie.GetByPrefix(prefix)
			select new string(c.ToArray<char>());
		}

		// Token: 0x06015F4B RID: 89931 RVA: 0x00690087 File Offset: 0x0068E287
		public IEnumerator<string> GetEnumerator()
		{
			return (from c in this._trie
			select new string(c.ToArray<char>())).GetEnumerator();
		}

		// Token: 0x06015F4C RID: 89932 RVA: 0x006900B8 File Offset: 0x0068E2B8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06015F4D RID: 89933 RVA: 0x006900C0 File Offset: 0x0068E2C0
		public void Add(string item)
		{
			this._trie.Add(item);
		}

		// Token: 0x06015F4E RID: 89934 RVA: 0x006900CE File Offset: 0x0068E2CE
		public void AddRange(IEnumerable<string> item)
		{
			this._trie.AddRange(item);
		}

		// Token: 0x06015F4F RID: 89935 RVA: 0x006900DC File Offset: 0x0068E2DC
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x06015F50 RID: 89936 RVA: 0x006900E9 File Offset: 0x0068E2E9
		public bool Contains(string item)
		{
			return this._trie.Contains(item);
		}

		// Token: 0x06015F51 RID: 89937 RVA: 0x006900F7 File Offset: 0x0068E2F7
		public void CopyTo(string[] array, int arrayIndex)
		{
			Array.Copy((from c in this._trie
			select new string(c.ToArray<char>())).ToArray<string>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x06015F52 RID: 89938 RVA: 0x00690136 File Offset: 0x0068E336
		public bool Remove(string item)
		{
			return this._trie.Remove(item);
		}

		// Token: 0x0400F355 RID: 62293
		private readonly TrieSet<char> _trie;
	}
}
