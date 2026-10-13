using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015ED RID: 5613
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x06008477 RID: 33911 RVA: 0x0027B425 File Offset: 0x00279625
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Open))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x0027B435 File Offset: 0x00279635
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Open);
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06008479 RID: 33913 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600847A RID: 33914 RVA: 0x0027B44F File Offset: 0x0027964F
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x0600847B RID: 33915 RVA: 0x0027B480 File Offset: 0x00279680
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Open, t);
		}
	}
}
