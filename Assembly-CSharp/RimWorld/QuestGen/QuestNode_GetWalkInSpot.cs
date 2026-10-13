using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A8C RID: 14988
	public class QuestNode_GetWalkInSpot : QuestNode
	{
		// Token: 0x06014784 RID: 83844 RVA: 0x0061D2D0 File Offset: 0x0061B4D0
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
			if (this.TryFindWalkInSpot(map, out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		// Token: 0x06014785 RID: 83845 RVA: 0x0061D334 File Offset: 0x0061B534
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
			if (this.TryFindWalkInSpot(map, out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x06014786 RID: 83846 RVA: 0x0061D39C File Offset: 0x0061B59C
		private bool TryFindWalkInSpot(Map map, out IntVec3 spawnSpot)
		{
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map) && map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => true, map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot);
		}

		// Token: 0x0400E29E RID: 58014
		[NoTranslate]
		public SlateRef<string> storeAs = "walkInSpot";
	}
}
