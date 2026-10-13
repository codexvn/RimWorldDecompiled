using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001A94 RID: 6804
	public class IdeoDescriptionMaker
	{
		// Token: 0x04006676 RID: 26230
		public List<IdeoDescriptionMaker.PatternEntry> patterns;

		// Token: 0x04006677 RID: 26231
		public RulePack rules;

		// Token: 0x04006678 RID: 26232
		public Dictionary<string, string> constants;

		// Token: 0x02001A95 RID: 6805
		public class PatternEntry
		{
			// Token: 0x04006679 RID: 26233
			public IdeoStoryPatternDef def;

			// Token: 0x0400667A RID: 26234
			public float weight = 1f;
		}
	}
}
