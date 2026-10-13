using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E9E RID: 3742
	public class JobDriver_CastAbilityMelee : JobDriver_CastAbility
	{
		// Token: 0x06005E4D RID: 24141 RVA: 0x001E038F File Offset: 0x001DE58F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.job.ability.CanCast && !this.job.ability.Casting);
			Ability ability = ((Verb_CastAbility)this.job.verbToUse).ability;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOn(() => !ability.CanApplyOn(this.job.targetA));
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x001E039F File Offset: 0x001DE59F
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
