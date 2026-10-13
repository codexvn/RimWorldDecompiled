using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001613 RID: 5651
	public abstract class WorkGiver_StudyBase : WorkGiver_Scanner
	{
		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06008558 RID: 34136 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x0027DFD7 File Offset: 0x0027C1D7
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return Find.StudyManager.GetStudiableThingsAndPlatforms(pawn.Map);
		}

		// Token: 0x0600855A RID: 34138 RVA: 0x0027DFEC File Offset: 0x0027C1EC
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			Thing thing = t.Thing;
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
				}
			}
			if (thing == null)
			{
				return 0f;
			}
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			return (float)(Find.TickManager.TicksGame - compStudiable.lastStudiedTick);
		}
	}
}
