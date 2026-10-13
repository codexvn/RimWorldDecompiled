using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8D RID: 3213
	public static class ThingCountUtility
	{
		// Token: 0x0600528A RID: 21130 RVA: 0x001AA9E4 File Offset: 0x001A8BE4
		public static int CountOf(List<ThingCount> list, Thing thing)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					num += list[i].Count;
				}
			}
			return num;
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001AAA2C File Offset: 0x001A8C2C
		public static void AddToList(List<ThingCount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					list[i] = list[i].WithCount(list[i].Count + countToAdd);
					return;
				}
			}
			list.Add(new ThingCount(thing, countToAdd, false));
		}
	}
}
