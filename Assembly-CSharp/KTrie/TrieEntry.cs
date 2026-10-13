using System;
using System.Collections.Generic;

namespace KTrie
{
	// Token: 0x02003EA3 RID: 16035
	public struct TrieEntry<TKey, TValue>
	{
		// Token: 0x06015F78 RID: 89976 RVA: 0x0069047D File Offset: 0x0068E67D
		public TrieEntry(IEnumerable<TKey> key, TValue value)
		{
			this.Key = key;
			this.Value = value;
		}

		// Token: 0x170036FF RID: 14079
		// (get) Token: 0x06015F79 RID: 89977 RVA: 0x0069048D File Offset: 0x0068E68D
		public readonly IEnumerable<TKey> Key { get; }

		// Token: 0x17003700 RID: 14080
		// (get) Token: 0x06015F7A RID: 89978 RVA: 0x00690495 File Offset: 0x0068E695
		public readonly TValue Value { get; }
	}
}
