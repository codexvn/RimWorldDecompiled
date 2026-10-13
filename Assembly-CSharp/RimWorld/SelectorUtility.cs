using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020033E9 RID: 13289
	public static class SelectorUtility
	{
		// Token: 0x06012AF6 RID: 76534 RVA: 0x00588150 File Offset: 0x00586350
		public static void SortInColonistBarOrder(List<Thing> things)
		{
			SelectorUtility.tmp_thingsToSort.Clear();
			SelectorUtility.tmp_thingsToSort.AddRange(things);
			things.Clear();
			foreach (Pawn item in Find.ColonistBar.GetColonistsInOrder())
			{
				int num = SelectorUtility.tmp_thingsToSort.IndexOf(item);
				if (num != -1)
				{
					things.Add(item);
					SelectorUtility.tmp_thingsToSort.RemoveAt(num);
				}
			}
			things.AddRange(SelectorUtility.tmp_thingsToSort);
			SelectorUtility.tmp_thingsToSort.Clear();
		}

		// Token: 0x06012AF7 RID: 76535 RVA: 0x005881F4 File Offset: 0x005863F4
		public static bool IsEquivalentRace(Pawn a, Pawn b)
		{
			return (ModsConfig.AnomalyActive && ((a.def == ThingDefOf.Human && b.def == ThingDefOf.CreepJoiner) || (b.def == ThingDefOf.Human && a.def == ThingDefOf.CreepJoiner))) || a.RaceProps == b.RaceProps;
		}

		// Token: 0x0400C210 RID: 49680
		private static readonly List<Thing> tmp_thingsToSort = new List<Thing>();
	}
}
