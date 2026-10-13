using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000293 RID: 659
	public class IdeoStoryPatternDef : Def
	{
		// Token: 0x06001077 RID: 4215 RVA: 0x0004E4BB File Offset: 0x0004C6BB
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.segments.Any<string>())
			{
				yield return "Pattern must have at least one segment";
			}
			yield break;
		}

		// Token: 0x04000D8C RID: 3468
		[NoTranslate]
		public List<string> segments = new List<string>();

		// Token: 0x04000D8D RID: 3469
		public List<string> noCapitalizeFirstSentence = new List<string>();

		// Token: 0x04000D8E RID: 3470
		public RulePack rules;
	}
}
