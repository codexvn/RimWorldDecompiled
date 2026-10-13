using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F4 RID: 8692
	public class CompAbilityEffect_AnimalRoar : CompAbilityEffect
	{
		// Token: 0x17001D0B RID: 7435
		// (get) Token: 0x0600BE61 RID: 48737 RVA: 0x00373485 File Offset: 0x00371685
		public new CompProperties_AbilityAnimalRoar Props
		{
			get
			{
				return (CompProperties_AbilityAnimalRoar)this.props;
			}
		}

		// Token: 0x0600BE62 RID: 48738 RVA: 0x00373492 File Offset: 0x00371692
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return target.Pawn != null && Rand.Chance(this.Props.chanceFromHearingCurve.Evaluate(target.Pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing)));
		}

		// Token: 0x0600BE63 RID: 48739 RVA: 0x003734CF File Offset: 0x003716CF
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			Pawn_CallTracker caller = this.parent.pawn.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall(true);
		}
	}
}
