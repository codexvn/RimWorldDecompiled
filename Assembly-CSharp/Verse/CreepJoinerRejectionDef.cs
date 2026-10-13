using System;

namespace Verse
{
	// Token: 0x02000227 RID: 551
	public class CreepJoinerRejectionDef : CreepJoinerBaseDef
	{
		// Token: 0x04000911 RID: 2321
		public bool hasLetter;

		// Token: 0x04000912 RID: 2322
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000913 RID: 2323
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04000914 RID: 2324
		public LetterDef letterDef;

		// Token: 0x04000915 RID: 2325
		public Type workerType;
	}
}
