using System;

namespace Verse
{
	// Token: 0x02000226 RID: 550
	public class CreepJoinerAggressiveDef : CreepJoinerBaseDef
	{
		// Token: 0x0400090A RID: 2314
		public bool hasMessage;

		// Token: 0x0400090B RID: 2315
		[MustTranslate]
		public string message;

		// Token: 0x0400090C RID: 2316
		public bool hasLetter;

		// Token: 0x0400090D RID: 2317
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400090E RID: 2318
		[MustTranslate]
		public string letterDesc;

		// Token: 0x0400090F RID: 2319
		public LetterDef letterDef;

		// Token: 0x04000910 RID: 2320
		public Type workerType;
	}
}
