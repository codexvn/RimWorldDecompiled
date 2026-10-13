using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001611 RID: 5649
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		// Token: 0x06008548 RID: 34120 RVA: 0x0027DDBC File Offset: 0x0027BFBC
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Strip))
			{
				if (!designation.target.HasThing)
				{
					Log.ErrorOnce("Strip designation has no target.", 63126);
				}
				else
				{
					yield return designation.target.Thing;
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06008549 RID: 34121 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600854A RID: 34122 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600854B RID: 34123 RVA: 0x0027DDCC File Offset: 0x0027BFCC
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Strip);
		}

		// Token: 0x0600854C RID: 34124 RVA: 0x0027DDE6 File Offset: 0x0027BFE6
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) != null && pawn.CanReserve(t, 1, -1, null, forced) && StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x0600854D RID: 34125 RVA: 0x0027DE21 File Offset: 0x0027C021
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Strip, t);
		}
	}
}
