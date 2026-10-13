using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003851 RID: 14417
	public class HediffComp_DestroyOrgan : HediffComp
	{
		// Token: 0x170032E9 RID: 13033
		// (get) Token: 0x06013F05 RID: 81669 RVA: 0x005EA737 File Offset: 0x005E8937
		public HediffCompProperties_DestroyOrgan Props
		{
			get
			{
				return (HediffCompProperties_DestroyOrgan)this.props;
			}
		}

		// Token: 0x06013F06 RID: 81670 RVA: 0x005EA744 File Offset: 0x005E8944
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity >= base.Def.maxSeverity)
			{
				this.DoDecayEffect();
			}
		}

		// Token: 0x06013F07 RID: 81671 RVA: 0x005EA764 File Offset: 0x005E8964
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (culprit != null && culprit.def == HediffDefOf.OrganDecay && culprit.Part == this.parent.Part)
			{
				this.DoDecayEffect();
			}
		}

		// Token: 0x06013F08 RID: 81672 RVA: 0x005EA790 File Offset: 0x005E8990
		private void DoDecayEffect()
		{
			base.Pawn.health.RemoveHediff(this.parent);
			if (base.Pawn.Dead)
			{
				base.Pawn.health.AddHediff(HediffDefOf.Decayed, this.parent.Part, null, null);
			}
			else
			{
				DamageInfo dinfo = new DamageInfo(this.Props.damageType ?? DamageDefOf.Rotting, 99999f, 999f, -1f, null, this.parent.Part, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetAllowDamagePropagation(false);
				base.Pawn.TakeDamage(dinfo);
			}
			if (base.Pawn.Faction == Faction.OfPlayer && !string.IsNullOrEmpty(this.Props.messageText))
			{
				Messages.Message(this.Props.messageText.Formatted(base.Pawn.Named("PAWN"), this.parent.Part.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
