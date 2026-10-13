using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001548 RID: 5448
	public abstract class WorkGiver
	{
		// Token: 0x0600814C RID: 33100 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x0600814D RID: 33101 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600814E RID: 33102 RVA: 0x0026F72C File Offset: 0x0026D92C
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04005639 RID: 22073
		public WorkGiverDef def;
	}
}
