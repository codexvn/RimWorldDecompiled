using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000724 RID: 1828
	public class HediffComp_SeverityPerDay : HediffComp_SeverityModifierBase
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x000E6AFC File Offset: 0x000E4CFC
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x000E6B09 File Offset: 0x000E4D09
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDay = this.Props.CalculateSeverityPerDay();
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000E6B24 File Offset: 0x000E4D24
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDay, "severityPerDay", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.severityPerDay == 0f && this.Props.severityPerDay != 0f && this.Props.severityPerDayRange == FloatRange.Zero)
			{
				this.severityPerDay = this.Props.CalculateSeverityPerDay();
				Log.Warning("Hediff " + this.parent.Label + " had severityPerDay not matching parent.");
			}
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000E6BBC File Offset: 0x000E4DBC
		public override float SeverityChangePerDay()
		{
			if (base.Pawn.ageTracker.AgeBiologicalYearsFloat < this.Props.minAge)
			{
				return 0f;
			}
			float num = this.severityPerDay;
			HediffStage curStage = this.parent.CurStage;
			float num2 = num * ((curStage != null) ? curStage.severityGainFactor : 1f);
			if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(base.Pawn))
			{
				num2 *= this.Props.mechanitorFactor;
			}
			return num2;
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000E6C34 File Offset: 0x000E4E34
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.Props.showHoursToRecover && this.SeverityChangePerDay() < 0f)
				{
					return Mathf.RoundToInt(this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay()) * 24f).ToString() + "LetterHour".Translate();
				}
				return null;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x000E6C9C File Offset: 0x000E4E9C
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Props.showDaysToRecover && this.SeverityChangePerDay() < 0f)
				{
					return "DaysToRecover".Translate((this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay())).ToString("0.0")).Resolve();
				}
				return null;
			}
		}

		// Token: 0x040023AB RID: 9131
		public float severityPerDay;
	}
}
