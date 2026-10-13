using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001593 RID: 5523
	public class WorkGiver_Warden_SuppressActivity : WorkGiver_Scanner
	{
		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x060082C4 RID: 33476 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060082C5 RID: 33477 RVA: 0x002747D7 File Offset: 0x002729D7
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Suppressable);
		}

		// Token: 0x060082C6 RID: 33478 RVA: 0x002747EC File Offset: 0x002729EC
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			Thing thingToSuppress = this.GetThingToSuppress(t.Thing, false);
			if (thingToSuppress == null)
			{
				return 0f;
			}
			CompActivity compActivity = thingToSuppress.TryGetComp<CompActivity>();
			if (compActivity == null)
			{
				return 0f;
			}
			return compActivity.ActivityLevel;
		}

		// Token: 0x060082C7 RID: 33479 RVA: 0x00274828 File Offset: 0x00272A28
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			Thing thingToSuppress = this.GetThingToSuppress(t, forced);
			CompActivity compActivity = thingToSuppress.TryGetComp<CompActivity>();
			if (thingToSuppress == null)
			{
				return null;
			}
			if (StatDefOf.ActivitySuppressionRate.Worker.IsDisabledFor(pawn))
			{
				return null;
			}
			if (StatDefOf.ActivitySuppressionRate.Worker.GetValue(pawn, true, -1) <= 0f)
			{
				JobFailReason.Is("ZeroSuppressionRate".Translate(), null);
				return null;
			}
			if (!ActivitySuppressionUtility.CanBeSuppressed(thingToSuppress, true, forced))
			{
				return null;
			}
			if (!forced && compActivity.ActivityLevel < compActivity.suppressIfAbove)
			{
				return null;
			}
			if (!pawn.CanReserve(thingToSuppress, 1, -1, null, forced))
			{
				return null;
			}
			Building_HoldingPlatform building_HoldingPlatform = thingToSuppress.ParentHolder as Building_HoldingPlatform;
			if (building_HoldingPlatform != null && !pawn.CanReserve(building_HoldingPlatform, 1, -1, null, forced))
			{
				return null;
			}
			IntVec3 intVec;
			if (!SocialInteractionUtility.TryGetAdjacentInteractionCell(pawn, t, forced, out intVec))
			{
				JobFailReason.Is("CannotStandNear".Translate(), null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ActivitySuppression, t);
			job.playerForced = forced;
			return job;
		}

		// Token: 0x060082C8 RID: 33480 RVA: 0x00274928 File Offset: 0x00272B28
		private Thing GetThingToSuppress(Thing thing, bool playerForced)
		{
			Thing thing2 = thing;
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform != null)
			{
				thing2 = building_HoldingPlatform.HeldPawn;
			}
			if (thing2 == null || !ActivitySuppressionUtility.CanBeSuppressed(thing2, true, playerForced))
			{
				return null;
			}
			return thing2;
		}
	}
}
