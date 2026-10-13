using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020006D0 RID: 1744
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x000E2687 File Offset: 0x000E0887
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000E2694 File Offset: 0x000E0894
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000E26A2 File Offset: 0x000E08A2
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000E26B9 File Offset: 0x000E08B9
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			this.ticksToHeal -= delta;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(base.Pawn, this.parent.LabelCap);
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000E26F0 File Offset: 0x000E08F0
		public static void TryHealRandomPermanentWound(Pawn pawn, string cause)
		{
			Hediff hediff;
			if (!(from hd in pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.Cure(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, hediff.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000E2791 File Offset: 0x000E0991
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000E27A5 File Offset: 0x000E09A5
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal.ToString();
		}

		// Token: 0x040022E8 RID: 8936
		private int ticksToHeal;
	}
}
