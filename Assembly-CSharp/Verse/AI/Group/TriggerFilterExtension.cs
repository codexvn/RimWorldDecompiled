using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010C2 RID: 4290
	public static class TriggerFilterExtension
	{
		// Token: 0x06006901 RID: 26881 RVA: 0x002121FC File Offset: 0x002103FC
		public static Trigger WithFilter(this Trigger t, TriggerFilter f)
		{
			if (t.filters == null)
			{
				t.filters = new List<TriggerFilter>();
			}
			t.filters.Add(f);
			return t;
		}
	}
}
