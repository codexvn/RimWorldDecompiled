using System;

namespace Verse
{
	// Token: 0x02000726 RID: 1830
	public class HediffComp_SeverityPerSecond : HediffComp
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000E6D3F File Offset: 0x000E4F3F
		private HediffCompProperties_SeverityPerSecond Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerSecond)this.props;
			}
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000E6D4C File Offset: 0x000E4F4C
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerSecond = this.Props.CalculateSeverityPerSecond();
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000E6D66 File Offset: 0x000E4F66
		public override void CompPostTick(ref float severityAdjustment)
		{
			float num = severityAdjustment;
			float num2 = this.severityPerSecond / 60f;
			HediffStage curStage = this.parent.CurStage;
			severityAdjustment = num + num2 * ((curStage != null) ? curStage.severityGainFactor : 1f);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000E6D98 File Offset: 0x000E4F98
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerSecond, "severityPerDay", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.severityPerSecond == 0f && this.Props.severityPerSecond != 0f && this.Props.severityPerSecondRange == FloatRange.Zero)
			{
				this.severityPerSecond = this.Props.CalculateSeverityPerSecond();
				Log.Warning("Hediff " + this.parent.Label + " had severityPerSecond not matching parent.");
			}
		}

		// Token: 0x040023AE RID: 9134
		protected float severityPerSecond;
	}
}
