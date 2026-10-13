using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001418 RID: 5144
	public class JobGiver_AITrashDutyFocus : ThinkNode_JobGiver
	{
		// Token: 0x06007D34 RID: 32052 RVA: 0x0025B408 File Offset: 0x00259608
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			return (JobGiver_AITrashDutyFocus)base.DeepCopy(resolve);
		}

		// Token: 0x06007D35 RID: 32053 RVA: 0x0025B418 File Offset: 0x00259618
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null || !pawn.mindState.duty.focus.IsValid)
			{
				return null;
			}
			LocalTargetInfo focus = pawn.mindState.duty.focus;
			if (!focus.Thing.Spawned || focus.ThingDestroyed || !pawn.HostileTo(focus.Thing) || !pawn.CanReach(focus, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = TrashUtility.TrashJob(pawn, focus.Thing, false, true);
			if (job != null)
			{
				return job;
			}
			return null;
		}
	}
}
