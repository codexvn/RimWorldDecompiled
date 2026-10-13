using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022AD RID: 8877
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		// Token: 0x17001D90 RID: 7568
		// (get) Token: 0x0600C125 RID: 49445 RVA: 0x0037ED70 File Offset: 0x0037CF70
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		// Token: 0x0600C126 RID: 49446 RVA: 0x0037ED80 File Offset: 0x0037CF80
		public float GetDurationSeconds(Pawn target)
		{
			if (this.Props.durationSecondsOverride != FloatRange.Zero)
			{
				return this.Props.durationSecondsOverride.RandomInRange;
			}
			float num = this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn);
			if (this.Props.durationMultiplier != null)
			{
				num *= target.GetStatValue(this.Props.durationMultiplier, true, -1);
			}
			return num;
		}
	}
}
