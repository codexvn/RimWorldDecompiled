using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141D RID: 5149
	public class JobGiver_AIJumpToJobRescueTarget : JobGiver_AIJumpToJobTarget
	{
		// Token: 0x06007D44 RID: 32068 RVA: 0x0025BA00 File Offset: 0x00259C00
		public override bool CanJumpToTarget(Pawn pawn, LocalTargetInfo target)
		{
			if (!base.CanJumpToTarget(pawn, target))
			{
				return false;
			}
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing is Pawn)
				{
					return true;
				}
				if (thing is Building_Bed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					return true;
				}
			}
			return false;
		}
	}
}
