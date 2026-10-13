using System;

namespace KTrie
{
	// Token: 0x02003E9B RID: 16027
	public struct StringEntry<TValue>
	{
		// Token: 0x06015F28 RID: 89896 RVA: 0x0068FD6C File Offset: 0x0068DF6C
		public StringEntry(string key, TValue value)
		{
			this.Key = key;
			this.Value = value;
		}

		// Token: 0x170036EF RID: 14063
		// (get) Token: 0x06015F29 RID: 89897 RVA: 0x0068FD7C File Offset: 0x0068DF7C
		public readonly string Key { get; }

		// Token: 0x170036F0 RID: 14064
		// (get) Token: 0x06015F2A RID: 89898 RVA: 0x0068FD84 File Offset: 0x0068DF84
		public readonly TValue Value { get; }
	}
}
