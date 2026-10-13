using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E63 RID: 11875
	public class StudyNote
	{
		// Token: 0x0400AE4F RID: 44623
		public float threshold;

		// Token: 0x0400AE50 RID: 44624
		public FloatRange thresholdRange = FloatRange.Zero;

		// Token: 0x0400AE51 RID: 44625
		public float? studyKnowledgeAmount;

		// Token: 0x0400AE52 RID: 44626
		[MustTranslate]
		public string label;

		// Token: 0x0400AE53 RID: 44627
		[MustTranslate]
		public string text;
	}
}
