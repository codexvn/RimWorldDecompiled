using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001626 RID: 5670
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x060085AF RID: 34223 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060085B0 RID: 34224 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x0027EB79 File Offset: 0x0027CD79
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.designationsByDef[DesignationDefOf.Flick])
			{
				yield return designation.target.Thing;
			}
			List<Designation>.Enumerator enumerator = default(List<Designation>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060085B2 RID: 34226 RVA: 0x0027EB89 File Offset: 0x0027CD89
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x060085B3 RID: 34227 RVA: 0x0027EBA3 File Offset: 0x0027CDA3
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060085B4 RID: 34228 RVA: 0x0027EBD4 File Offset: 0x0027CDD4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Flick, t);
		}
	}
}
