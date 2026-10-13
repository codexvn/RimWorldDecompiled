using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200228F RID: 8847
	public class CompProperties_AbilityStopMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x17001D71 RID: 7537
		// (get) Token: 0x0600C095 RID: 49301 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool OverridesPsyfocusCost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C096 RID: 49302 RVA: 0x0037C573 File Offset: 0x0037A773
		public CompProperties_AbilityStopMentalState()
		{
			this.compClass = typeof(CompAbilityEffect_StopMentalState);
		}

		// Token: 0x17001D72 RID: 7538
		// (get) Token: 0x0600C097 RID: 49303 RVA: 0x0037C5AC File Offset: 0x0037A7AC
		public override string PsyfocusCostExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("PsyfocusCostPerMentalBreakIntensity".Translate() + ":");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMinor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMinor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMajor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMajor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityExtreme".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForExtreme.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001D73 RID: 7539
		// (get) Token: 0x0600C098 RID: 49304 RVA: 0x0037C6A9 File Offset: 0x0037A8A9
		public override FloatRange PsyfocusCostRange
		{
			get
			{
				return new FloatRange(this.psyfocusCostForMinor, this.psyfocusCostForExtreme);
			}
		}

		// Token: 0x0600C099 RID: 49305 RVA: 0x0037C6BC File Offset: 0x0037A8BC
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.psyfocusCostForMinor < 0f)
			{
				yield return "psyfocusCostForMinor must be defined ";
			}
			if (this.psyfocusCostForMajor < 0f)
			{
				yield return "psyfocusCostForMajor must be defined ";
			}
			if (this.psyfocusCostForExtreme < 0f)
			{
				yield return "psyfocusCostForExtreme must be defined ";
			}
			yield break;
			yield break;
		}

		// Token: 0x040083DC RID: 33756
		public List<MentalStateDef> exceptions;

		// Token: 0x040083DD RID: 33757
		public float psyfocusCostForMinor = -1f;

		// Token: 0x040083DE RID: 33758
		public float psyfocusCostForMajor = -1f;

		// Token: 0x040083DF RID: 33759
		public float psyfocusCostForExtreme = -1f;
	}
}
