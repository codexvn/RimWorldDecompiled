using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002194 RID: 8596
	public class StrengthWatcher
	{
		// Token: 0x17001C87 RID: 7303
		// (get) Token: 0x0600BBC3 RID: 48067 RVA: 0x00366128 File Offset: 0x00364328
		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= pawn.health.summaryHealth.SummaryHealthPercent;
					if (pawn.Downed)
					{
						num2 *= 0.3f;
					}
					num += num2;
				}
				foreach (Building building in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (building.def.building != null && building.def.building.IsTurret)
					{
						num += 0.3f;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BBC4 RID: 48068 RVA: 0x00366224 File Offset: 0x00364424
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x04008148 RID: 33096
		private Map map;
	}
}
