using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F2D RID: 12077
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17002C0E RID: 11278
		// (get) Token: 0x060110C9 RID: 69833 RVA: 0x004E67BF File Offset: 0x004E49BF
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x060110CA RID: 69834 RVA: 0x004E67CC File Offset: 0x004E49CC
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (Rand.Value <= this.PropsBrainDamageChance.brainDamageChance)
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int num = Rand.RangeInclusive(1, 5);
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}
	}
}
