using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001457 RID: 5207
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		// Token: 0x06007DEB RID: 32235 RVA: 0x0025EBC1 File Offset: 0x0025CDC1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		// Token: 0x06007DEC RID: 32236 RVA: 0x0025EBDC File Offset: 0x0025CDDC
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = GenClosest.ClosestThingReachable(pawn.GetLord().CurLordToil.FlagLoc, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.maxDistFromPoint, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing);
			}
			return null;
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x0025EC45 File Offset: 0x0025CE45
		public static Predicate<Thing> FireValidator(Pawn pawn)
		{
			return (Thing t) => !(((AttachableThing)t).parent is Pawn) && pawn.CanReserve(t, 1, -1, null, false) && !pawn.WorkTagIsDisabled(WorkTags.Firefighting);
		}

		// Token: 0x0400548C RID: 21644
		public float maxDistFromPoint = -1f;
	}
}
