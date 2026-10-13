using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E64 RID: 11876
	public class CompProperties_StudyUnlocks : CompProperties
	{
		// Token: 0x06010B95 RID: 68501 RVA: 0x004D2709 File Offset: 0x004D0909
		public CompProperties_StudyUnlocks()
		{
			this.compClass = typeof(CompStudyUnlocks);
		}

		// Token: 0x06010B96 RID: 68502 RVA: 0x004D272C File Offset: 0x004D092C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			float num = 0f;
			int num2;
			for (int i = 0; i < this.studyNotes.Count; i = num2 + 1)
			{
				StudyNote note = this.studyNotes[i];
				if (num != 0f)
				{
					if (note.threshold != 0f && note.threshold <= num)
					{
						yield return string.Format("Threshold for note at index {0} had a threshold value ({1}) lower than the previous maximum ({2})", i, note.threshold, num);
					}
					else if (note.threshold == 0f && note.thresholdRange.min <= num)
					{
						yield return string.Format("Threshold for note at index {0} had a min threshold value ({1}) lower than the previous maximum ({2})", i, note.thresholdRange.min, num);
					}
				}
				num = ((note.threshold != 0f) ? note.threshold : note.thresholdRange.max);
				note = null;
				num2 = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AE54 RID: 44628
		public List<StudyNote> studyNotes = new List<StudyNote>();

		// Token: 0x0400AE55 RID: 44629
		public float? defaultStudyAmount;

		// Token: 0x0400AE56 RID: 44630
		public KnowledgeCategoryDef defaultCategoryOverride;
	}
}
