using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001604 RID: 5636
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x06008503 RID: 34051 RVA: 0x0027D31E File Offset: 0x0027B51E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x06008504 RID: 34052 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x06008505 RID: 34053 RVA: 0x0027D327 File Offset: 0x0027B527
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x06008506 RID: 34054 RVA: 0x0027D32E File Offset: 0x0027B52E
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x0027D335 File Offset: 0x0027B535
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x06008508 RID: 34056 RVA: 0x0027D343 File Offset: 0x0027B543
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x06008509 RID: 34057 RVA: 0x0027D358 File Offset: 0x0027B558
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
