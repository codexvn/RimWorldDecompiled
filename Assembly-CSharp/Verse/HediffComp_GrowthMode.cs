using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020006CE RID: 1742
	public class HediffComp_GrowthMode : HediffComp_SeverityModifierBase
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000E2292 File Offset: 0x000E0492
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06002B3C RID: 11068 RVA: 0x000E229F File Offset: 0x000E049F
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000E22AC File Offset: 0x000E04AC
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000E2300 File Offset: 0x000E0500
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000E235F File Offset: 0x000E055F
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			base.CompPostTickInterval(ref severityAdjustment, delta);
			if (base.Pawn.IsHashIntervalTick(5000, delta) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000E2398 File Offset: 0x000E0598
		public override float SeverityChangePerDay()
		{
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
			{
				float num = this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
				if (ModsConfig.BiotechActive && this.parent.def == HediffDefOf.Carcinoma)
				{
					num *= base.Pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
				}
				return num;
			}
			case HediffGrowthMode.Stable:
				return 0f;
			case HediffGrowthMode.Remission:
				return this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
			default:
				throw new NotImplementedException("GrowthMode");
			}
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000E2428 File Offset: 0x000E0628
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				switch (this.growthMode)
				{
				case HediffGrowthMode.Growing:
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					return;
				case HediffGrowthMode.Stable:
					Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					return;
				case HediffGrowthMode.Remission:
					Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000E2590 File Offset: 0x000E0790
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x040022E3 RID: 8931
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x040022E4 RID: 8932
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x040022E5 RID: 8933
		public HediffGrowthMode growthMode;

		// Token: 0x040022E6 RID: 8934
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x040022E7 RID: 8935
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
