using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02003273 RID: 12915
	public static class PageUtility
	{
		// Token: 0x060122BB RID: 74427 RVA: 0x005540E8 File Offset: 0x005522E8
		public static Page StitchedPages(IEnumerable<Page> pages)
		{
			List<Page> list = pages.ToList<Page>();
			if (list.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					list[i].prev = list[i - 1];
				}
				if (i < list.Count - 1)
				{
					list[i].next = list[i + 1];
				}
			}
			return list[0];
		}

		// Token: 0x060122BC RID: 74428 RVA: 0x00554157 File Offset: 0x00552357
		public static void InitGameStart()
		{
			LongEventHandler.QueueLongEvent(new Action(PageUtility.<InitGameStart>g__PreLoadAct|1_0), "Play", "GeneratingMap", true, null, true, false);
		}

		// Token: 0x060122BD RID: 74429 RVA: 0x00554178 File Offset: 0x00552378
		[CompilerGenerated]
		internal static void <InitGameStart>g__PreLoadAct|1_0()
		{
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}
	}
}
