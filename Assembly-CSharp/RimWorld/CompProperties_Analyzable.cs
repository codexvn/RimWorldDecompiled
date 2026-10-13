using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA9 RID: 11177
	public abstract class CompProperties_Analyzable : CompProperties_Interactable
	{
		// Token: 0x0600F9DF RID: 63967 RVA: 0x0048AB81 File Offset: 0x00488D81
		public CompProperties_Analyzable()
		{
			this.compClass = typeof(CompAnalyzable);
		}

		// Token: 0x0600F9E0 RID: 63968 RVA: 0x0048ABBC File Offset: 0x00488DBC
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!string.IsNullOrEmpty(this.progressedLetterLabel) && !this.progressedLetters.NullOrEmpty<string>() && this.progressedLetterDef == null)
			{
				yield return "Letter def is null (progressedLetterDef) for analyzable component";
			}
			if ((!string.IsNullOrEmpty(this.completedLetterLabel) || !string.IsNullOrEmpty(this.completedLetter)) && this.completedLetterDef == null)
			{
				yield return "Letter def is null (completedLetterDef) for analyzable component";
			}
			if ((!string.IsNullOrEmpty(this.repeatCompletedLetterLabel) || !string.IsNullOrEmpty(this.repeatCompletedLetter)) && this.repeatCompletedLetterDef == null)
			{
				yield return "Letter def is null (repeatCompletedLetterDef) for analyzable component";
			}
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400A445 RID: 42053
		public IntRange analysisRequiredRange = new IntRange(1, 1);

		// Token: 0x0400A446 RID: 42054
		public bool destroyedOnAnalyzed;

		// Token: 0x0400A447 RID: 42055
		public float analysisDurationHours = 1.5f;

		// Token: 0x0400A448 RID: 42056
		public bool showProgress;

		// Token: 0x0400A449 RID: 42057
		public bool allowRepeatAnalysis;

		// Token: 0x0400A44A RID: 42058
		[MustTranslate]
		public string progressedLetterLabel;

		// Token: 0x0400A44B RID: 42059
		[MustTranslate]
		public List<string> progressedLetters = new List<string>();

		// Token: 0x0400A44C RID: 42060
		public LetterDef progressedLetterDef;

		// Token: 0x0400A44D RID: 42061
		[MustTranslate]
		public string completedLetterLabel;

		// Token: 0x0400A44E RID: 42062
		[MustTranslate]
		public string completedLetter;

		// Token: 0x0400A44F RID: 42063
		public LetterDef completedLetterDef;

		// Token: 0x0400A450 RID: 42064
		[MustTranslate]
		public string repeatCompletedLetterLabel;

		// Token: 0x0400A451 RID: 42065
		[MustTranslate]
		public string repeatCompletedLetter;

		// Token: 0x0400A452 RID: 42066
		public LetterDef repeatCompletedLetterDef;

		// Token: 0x0400A453 RID: 42067
		public bool canStudyInPlace;
	}
}
