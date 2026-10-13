using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015EB RID: 5611
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		// Token: 0x0600846C RID: 33900 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600846D RID: 33901 RVA: 0x0027B1F6 File Offset: 0x002793F6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		// Token: 0x0600846E RID: 33902 RVA: 0x0027B208 File Offset: 0x00279408
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging().Count == 0;
		}

		// Token: 0x0600846F RID: 33903 RVA: 0x0027B224 File Offset: 0x00279424
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.stackCount == t.def.stackLimit)
			{
				return null;
			}
			if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				return null;
			}
			ISlotGroup slotGroup = t.GetSlotGroup();
			ISlotGroup storageGroup = slotGroup.StorageGroup;
			slotGroup = (storageGroup ?? slotGroup);
			if (!pawn.CanReserve(t.Position, 1, -1, null, forced))
			{
				return null;
			}
			foreach (Thing thing in slotGroup.HeldThings)
			{
				if (thing != t && thing.CanStackWith(t) && (forced || thing.stackCount >= t.stackCount) && thing.stackCount < thing.def.stackLimit && !thing.IsForbidden(pawn.Faction) && thing.Position.InAllowedArea(pawn) && pawn.CanReserve(thing.Position, 1, -1, null, forced) && pawn.CanReserve(thing, 1, -1, null, false) && thing.Position.IsValidStorageFor(thing.Map, t) && !thing.Position.ContainsStaticFire(thing.Map))
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, thing.Position);
					job.count = Mathf.Min(thing.def.stackLimit - thing.stackCount, t.stackCount);
					job.haulMode = HaulMode.ToCellStorage;
					return job;
				}
			}
			JobFailReason.Is("NoMergeTarget".Translate(), null);
			return null;
		}
	}
}
