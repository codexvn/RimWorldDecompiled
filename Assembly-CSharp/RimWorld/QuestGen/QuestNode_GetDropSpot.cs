using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A3F RID: 14911
	public class QuestNode_GetDropSpot : QuestNode
	{
		// Token: 0x06014677 RID: 83575 RVA: 0x00618618 File Offset: 0x00616818
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return true;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 var;
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		// Token: 0x06014678 RID: 83576 RVA: 0x00618688 File Offset: 0x00616888
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return;
			}
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			IntVec3 var;
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x06014679 RID: 83577 RVA: 0x006186FC File Offset: 0x006168FC
		private bool TryFindDropSpot(Map map, float minDistFromEdge, out IntVec3 spawnSpot)
		{
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map) && (float)x.DistanceToEdge(map) >= minDistFromEdge && map.reachability.CanReachColony(x), map, 1000, out spawnSpot);
		}

		// Token: 0x0400E189 RID: 57737
		[NoTranslate]
		public SlateRef<string> storeAs = "dropSpot";

		// Token: 0x0400E18A RID: 57738
		public SlateRef<float> minDistanceFromEdge;
	}
}
