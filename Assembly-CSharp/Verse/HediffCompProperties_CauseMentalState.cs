using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200068E RID: 1678
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x06002A54 RID: 10836 RVA: 0x000DEF4A File Offset: 0x000DD14A
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000DEF69 File Offset: 0x000DD169
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.endMentalStateOnCure && this.removeOnTriggered)
			{
				yield return "hediff comp cause mental state has both endMentalStateOnCure and removeOnTriggered enabled which are mutually exclusive. Did you forget to disable endMentalStateOnCure?";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400224D RID: 8781
		public MentalStateDef animalMentalState;

		// Token: 0x0400224E RID: 8782
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x0400224F RID: 8783
		public MentalStateDef humanMentalState;

		// Token: 0x04002250 RID: 8784
		[MustTranslate]
		public string overrideLetterLabel;

		// Token: 0x04002251 RID: 8785
		[MustTranslate]
		public string overrideLetterDesc;

		// Token: 0x04002252 RID: 8786
		public LetterDef letterDef;

		// Token: 0x04002253 RID: 8787
		public float mtbDaysToCauseMentalState;

		// Token: 0x04002254 RID: 8788
		public float minSeverity;

		// Token: 0x04002255 RID: 8789
		public bool endMentalStateOnCure = true;

		// Token: 0x04002256 RID: 8790
		public bool removeOnTriggered;

		// Token: 0x04002257 RID: 8791
		public bool forced;
	}
}
