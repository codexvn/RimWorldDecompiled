using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200069D RID: 1693
	public class HediffComp_DamageBrain : HediffComp
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000DFF8B File Offset: 0x000DE18B
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000DFF98 File Offset: 0x000DE198
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Props.mtbDaysPerStage[this.parent.CurStageIndex] <= 0f && !base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
			{
				BodyPartRecord brain = base.Pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int randomInRange = this.Props.damageAmount.RandomInRange;
				base.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)randomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				Messages.Message("MessageReceivedBrainDamageFromHediff".Translate(base.Pawn.Named("PAWN"), randomInRange, this.parent.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
