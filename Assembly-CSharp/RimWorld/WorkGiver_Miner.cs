using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015EC RID: 5612
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06008471 RID: 33905 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008472 RID: 33906 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06008473 RID: 33907 RVA: 0x0027B3E0 File Offset: 0x002795E0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return MineAIUtility.PotentialMineables(pawn);
		}

		// Token: 0x06008474 RID: 33908 RVA: 0x0027B3E8 File Offset: 0x002795E8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Mine) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.MineVein);
		}

		// Token: 0x06008475 RID: 33909 RVA: 0x0027B41B File Offset: 0x0027961B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return MineAIUtility.JobOnThing(pawn, t, forced);
		}
	}
}
