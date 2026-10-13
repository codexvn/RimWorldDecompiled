using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023F2 RID: 9202
	public class ConversionTuning
	{
		// Token: 0x04008811 RID: 34833
		public const float ConvertAttempt_BaseSelectionWeight = 0.04f;

		// Token: 0x04008812 RID: 34834
		public const float ConvertAttempt_Colonist = 1f;

		// Token: 0x04008813 RID: 34835
		public const float ConvertAttempt_Slave = 0.5f;

		// Token: 0x04008814 RID: 34836
		public const float ConvertAttempt_NPCFreeVsColonist = 0.5f;

		// Token: 0x04008815 RID: 34837
		public const float ConvertAttempt_NPCFreeVsNPCFree = 0.25f;

		// Token: 0x04008816 RID: 34838
		public const float ConvertAttempt_NPCFreeVsPrisoner = 0.25f;

		// Token: 0x04008817 RID: 34839
		public const float ConvertAttempt_NPCFreeVsSlave = 0.5f;

		// Token: 0x04008818 RID: 34840
		public const float ConvertAttempt_PrisonerVsColonist = 0.25f;

		// Token: 0x04008819 RID: 34841
		public const float ConvertAttempt_PrisonerVsNPCFree = 0.25f;

		// Token: 0x0400881A RID: 34842
		public const float ConvertAttempt_PrisonerVsPrisoner = 0.5f;

		// Token: 0x0400881B RID: 34843
		public const float ConvertAttempt_PrisonerVsSlave = 0.5f;

		// Token: 0x0400881C RID: 34844
		public const float ConvertAttempt_FailOutcomeWeight_Nothing = 0.78f;

		// Token: 0x0400881D RID: 34845
		public const float ConvertAttempt_FailOutcomeWeight_Resentment = 0.2f;

		// Token: 0x0400881E RID: 34846
		public const float ConvertAttempt_FailOutcomeWeight_SocialFight = 0.02f;

		// Token: 0x0400881F RID: 34847
		public const float ConvertAttempt_BaseCertaintyReduction = 0.06f;

		// Token: 0x04008820 RID: 34848
		public static readonly SimpleCurve CertaintyPerDayByMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.02f),
				true
			},
			{
				new CurvePoint(0.8f, 0.03f),
				true
			}
		};

		// Token: 0x04008821 RID: 34849
		public const float PostConversionCertainty = 0.5f;

		// Token: 0x04008822 RID: 34850
		public static readonly FloatRange InitialCertaintyRange = new FloatRange(0.6f, 1f);

		// Token: 0x04008823 RID: 34851
		public const float ConversionPowerFactor_AgreeWithMeme = 0.2f;

		// Token: 0x04008824 RID: 34852
		public const float ConversionPowerFactor_DisagreeWithMeme = -0.2f;

		// Token: 0x04008825 RID: 34853
		public const float ConversionPowerFactor_Min = -0.4f;

		// Token: 0x04008826 RID: 34854
		public const float ChildCertaintyChangeFactor = 2f;
	}
}
