using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002292 RID: 8850
	public class CompAbilityEffect_Stun : CompAbilityEffect_WithDuration
	{
		// Token: 0x0600C0AD RID: 49325 RVA: 0x0037CBE0 File Offset: 0x0037ADE0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					pawn.stances.stunner.StunFor(base.GetDurationSeconds(pawn).SecondsToTicks(), this.parent.pawn, false, true, false);
				}
			}
		}
	}
}
