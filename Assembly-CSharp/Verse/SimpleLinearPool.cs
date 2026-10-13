using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000168 RID: 360
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x00031F24 File Offset: 0x00030124
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00031F70 File Offset: 0x00030170
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x0400058F RID: 1423
		private readonly List<T> items = new List<T>();

		// Token: 0x04000590 RID: 1424
		private int readIndex;
	}
}
