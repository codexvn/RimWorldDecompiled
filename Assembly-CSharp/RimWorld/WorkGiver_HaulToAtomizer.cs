using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015DC RID: 5596
	public class WorkGiver_HaulToAtomizer : WorkGiver_Scanner
	{
		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06008423 RID: 33827 RVA: 0x00279FE7 File Offset: 0x002781E7
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Atomizer);
			}
		}

		// Token: 0x06008424 RID: 33828 RVA: 0x00279FF0 File Offset: 0x002781F0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul to atomizer"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompAtomizer compAtomizer = t.TryGetComp<CompAtomizer>();
			if (compAtomizer.Full)
			{
				JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans, null);
				return false;
			}
			if (!forced && !compAtomizer.AutoLoad)
			{
				return false;
			}
			if (!forced && compAtomizer.FillPercent > 0.5f)
			{
				return false;
			}
			if (HaulAIUtility.FindFixedIngredientCount(pawn, compAtomizer.Props.thingDef, compAtomizer.SpaceLeft).NullOrEmpty<Thing>())
			{
				JobFailReason.Is("NoIngredient".Translate(compAtomizer.Props.thingDef), null);
				return false;
			}
			return true;
		}

		// Token: 0x06008425 RID: 33829 RVA: 0x0027A0A0 File Offset: 0x002782A0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompAtomizer compAtomizer = t.TryGetComp<CompAtomizer>();
			if (compAtomizer == null)
			{
				return null;
			}
			if (compAtomizer.Full)
			{
				JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans, null);
				return null;
			}
			if (!forced && !compAtomizer.AutoLoad)
			{
				return null;
			}
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, compAtomizer.Props.thingDef, compAtomizer.SpaceLeft);
			if (list.NullOrEmpty<Thing>())
			{
				JobFailReason.Is("NoIngredient".Translate(compAtomizer.Props.thingDef), null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToAtomizer, t);
			job.targetQueueB = (from f in list
			select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
			job.count = compAtomizer.SpaceLeft;
			return job;
		}

		// Token: 0x0400570F RID: 22287
		private const float MaxFillPercent = 0.5f;
	}
}
