using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000721 RID: 1825
	public class HediffComp_RandomizeSeverityPhases : HediffComp_Randomizer
	{
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000E673B File Offset: 0x000E493B
		public HediffCompProperties_RandomizeSeverityPhases Props
		{
			get
			{
				return (HediffCompProperties_RandomizeSeverityPhases)this.props;
			}
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000E6748 File Offset: 0x000E4948
		public HediffCompProperties_RandomizeSeverityPhases.Phase CurrentOption()
		{
			int? num = this.randomPhaseIndex;
			int num2 = 0;
			if (num.GetValueOrDefault() >= num2 & num != null)
			{
				num = this.randomPhaseIndex;
				List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases = this.Props.phases;
				int? num3 = (phases != null) ? new int?(phases.Count) : null;
				if (num.GetValueOrDefault() < num3.GetValueOrDefault() & (num != null & num3 != null))
				{
					return this.Props.phases[this.randomPhaseIndex.Value];
				}
			}
			return null;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000E67E0 File Offset: 0x000E49E0
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.Randomize();
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000E67F0 File Offset: 0x000E49F0
		public override void Randomize()
		{
			if (this.Props.phases == null || this.Props.phases.Count == 0)
			{
				return;
			}
			int num = Rand.Range(0, this.Props.phases.Count);
			if (this.randomPhaseIndex != null)
			{
				int num2 = num;
				int? num3 = this.randomPhaseIndex;
				if (!(num2 == num3.GetValueOrDefault() & num3 != null) && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
				{
					Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.Props.phases[this.randomPhaseIndex.Value].labelPrefix, this.Props.phases[num].labelPrefix), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			this.randomPhaseIndex = new int?(num);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000E6920 File Offset: 0x000E4B20
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int?>(ref this.randomPhaseIndex, "randomPhaseIndex", null, false);
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000E694D File Offset: 0x000E4B4D
		public override void CompPostTick(ref float severityAdjustment)
		{
			float num = severityAdjustment;
			HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
			severityAdjustment = num + ((phase != null) ? phase.severityPerDay : 0f) / 60000f;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000E6970 File Offset: 0x000E4B70
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "severity/day: ";
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				stringBuilder2.AppendLine(str + ((phase != null) ? phase.severityPerDay : 0f).ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000E69DC File Offset: 0x000E4BDC
		public override string CompLabelPrefix
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.labelPrefix;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002C7D RID: 11389 RVA: 0x000E69EF File Offset: 0x000E4BEF
		public override string CompDescriptionExtra
		{
			get
			{
				HediffCompProperties_RandomizeSeverityPhases.Phase phase = this.CurrentOption();
				if (phase == null)
				{
					return null;
				}
				return phase.descriptionExtra;
			}
		}

		// Token: 0x040023A2 RID: 9122
		private int? randomPhaseIndex;
	}
}
