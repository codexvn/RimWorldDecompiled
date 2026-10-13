using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B7 RID: 6583
	public class MapPortalProperties
	{
		// Token: 0x040060A1 RID: 24737
		public MapGeneratorDef pocketMapGenerator;

		// Token: 0x040060A2 RID: 24738
		public ThingDef exitDef;

		// Token: 0x040060A3 RID: 24739
		public int pocketMapSize = 100;

		// Token: 0x040060A4 RID: 24740
		[MustTranslate]
		public string enteredLetterLabel;

		// Token: 0x040060A5 RID: 24741
		[MustTranslate]
		public string enteredLetterText;

		// Token: 0x040060A6 RID: 24742
		[MustTranslate]
		public LetterDef enteredLetterDef;

		// Token: 0x040060A7 RID: 24743
		public SoundDef traverseSound;
	}
}
