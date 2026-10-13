using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021FB RID: 8699
	public class CompAbilityEffect_BrainDamageChance : CompAbilityEffect
	{
		// Token: 0x17001D10 RID: 7440
		// (get) Token: 0x0600BE7C RID: 48764 RVA: 0x00373B4E File Offset: 0x00371D4E
		public new CompProperties_AbilityBrainDamageChance Props
		{
			get
			{
				return (CompProperties_AbilityBrainDamageChance)this.props;
			}
		}

		// Token: 0x0600BE7D RID: 48765 RVA: 0x00373B5C File Offset: 0x00371D5C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.Dead)
				{
					return;
				}
				if (Rand.Value <= this.Props.brainDamageChance)
				{
					BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
					if (brain == null)
					{
						return;
					}
					int num = Rand.RangeInclusive(1, 5);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.parent.pawn, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}
	}
}
