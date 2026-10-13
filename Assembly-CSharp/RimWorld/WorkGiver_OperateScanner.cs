using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015EF RID: 5615
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06008486 RID: 33926 RVA: 0x0027B613 File Offset: 0x00279813
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06008487 RID: 33927 RVA: 0x0027B620 File Offset: 0x00279820
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06008488 RID: 33928 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06008489 RID: 33929 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600848A RID: 33930 RVA: 0x0027B630 File Offset: 0x00279830
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!pawn.CanReserve(building, 1, -1, null, forced))
			{
				return false;
			}
			AcceptanceReport canUseNow = building.TryGetComp<CompScanner>().CanUseNow;
			if (!canUseNow)
			{
				if (!canUseNow.Reason.NullOrEmpty())
				{
					JobFailReason.Is(canUseNow.Reason, null);
				}
				return false;
			}
			return !building.IsBurning();
		}

		// Token: 0x0600848B RID: 33931 RVA: 0x0027B6A9 File Offset: 0x002798A9
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
