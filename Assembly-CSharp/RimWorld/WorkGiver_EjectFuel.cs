using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015B7 RID: 5559
	public class WorkGiver_EjectFuel : WorkGiver_Scanner
	{
		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x0600836F RID: 33647 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06008370 RID: 33648 RVA: 0x0027780E File Offset: 0x00275A0E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.EjectFuel))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x0027781E File Offset: 0x00275A1E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.EjectFuel);
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x00277838 File Offset: 0x00275A38
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.EjectFuel) == null)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompRefuelable compRefuelable;
			if (!t.TryGetComp(out compRefuelable))
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compRefuelable.CanEjectFuel();
			if (!acceptanceReport.Accepted)
			{
				JobFailReason.Is(acceptanceReport.Reason, null);
				return false;
			}
			return true;
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x0027789F File Offset: 0x00275A9F
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.EjectFuel, t);
		}
	}
}
