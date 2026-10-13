using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A4B RID: 14923
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		// Token: 0x0601469F RID: 83615 RVA: 0x00618E4C File Offset: 0x0061704C
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		// Token: 0x060146A0 RID: 83616 RVA: 0x00618E88 File Offset: 0x00617088
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		// Token: 0x060146A1 RID: 83617 RVA: 0x00618EB8 File Offset: 0x006170B8
		private int GetLargestSize(Slate slate)
		{
			Map mapResolved = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			if (mapResolved == null)
			{
				return 0;
			}
			int value = this.max.GetValue(slate);
			CellRect cellRect = LargestAreaFinder.FindLargestRect(mapResolved, (IntVec3 x) => this.IsClear(x, mapResolved), value);
			return Mathf.Min(new int[]
			{
				cellRect.Width,
				cellRect.Height,
				value
			});
		}

		// Token: 0x060146A2 RID: 83618 RVA: 0x00618F48 File Offset: 0x00617148
		private bool IsClear(IntVec3 c, Map map)
		{
			if (!c.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial && thingList[i].Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (thingList[i].def.mineable)
				{
					bool flag = false;
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[j];
						if (c2.InBounds(map) && c2.GetFirstMineable(map) == null)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400E1B2 RID: 57778
		public SlateRef<Map> map;

		// Token: 0x0400E1B3 RID: 57779
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1B4 RID: 57780
		public SlateRef<int> failIfSmaller;

		// Token: 0x0400E1B5 RID: 57781
		public SlateRef<int> max;
	}
}
