using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006FB RID: 1787
	public class HediffCompProperties_ReplaceHediff : HediffCompProperties
	{
		// Token: 0x06002BDC RID: 11228 RVA: 0x000E46A0 File Offset: 0x000E28A0
		public HediffCompProperties_ReplaceHediff()
		{
			this.compClass = typeof(HediffComp_ReplaceHediff);
		}

		// Token: 0x04002343 RID: 9027
		public float severity = 1f;

		// Token: 0x04002344 RID: 9028
		public bool manuallyTriggered;

		// Token: 0x04002345 RID: 9029
		[MustTranslate]
		public string message;

		// Token: 0x04002346 RID: 9030
		public MessageTypeDef messageDef;

		// Token: 0x04002347 RID: 9031
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04002348 RID: 9032
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04002349 RID: 9033
		public LetterDef letterDef;

		// Token: 0x0400234A RID: 9034
		public List<HediffCompProperties_ReplaceHediff.TriggeredHediff> hediffs = new List<HediffCompProperties_ReplaceHediff.TriggeredHediff>();

		// Token: 0x020006FC RID: 1788
		public class TriggeredHediff
		{
			// Token: 0x06002BDD RID: 11229 RVA: 0x000E46D0 File Offset: 0x000E28D0
			public void ApplyTo(Pawn pawn, List<Hediff> outAddedHediffs = null)
			{
				List<Hediff> list = new List<Hediff>();
				HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, false, this.countRange.RandomInRange, list, false);
				if (this.severityRange != FloatRange.Zero)
				{
					foreach (Hediff hediff in list)
					{
						hediff.Severity = this.severityRange.RandomInRange;
					}
				}
				if (outAddedHediffs != null)
				{
					outAddedHediffs.AddRange(list);
				}
			}

			// Token: 0x0400234B RID: 9035
			public HediffDef hediff;

			// Token: 0x0400234C RID: 9036
			public IntRange countRange = new IntRange(1, 1);

			// Token: 0x0400234D RID: 9037
			public List<BodyPartDef> partsToAffect;

			// Token: 0x0400234E RID: 9038
			public FloatRange severityRange = FloatRange.Zero;
		}
	}
}
