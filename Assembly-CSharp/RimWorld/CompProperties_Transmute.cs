using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200229B RID: 8859
	public class CompProperties_Transmute : CompProperties_AbilityEffect
	{
		// Token: 0x0600C0CF RID: 49359 RVA: 0x0037D4CE File Offset: 0x0037B6CE
		public CompProperties_Transmute()
		{
			this.compClass = typeof(CompAbilityEffect_Transmute);
		}

		// Token: 0x040083F1 RID: 33777
		public List<CompProperties_Transmute.ElementRatio> elementRatios = new List<CompProperties_Transmute.ElementRatio>();

		// Token: 0x040083F2 RID: 33778
		public List<ThingDef> outcomeItems = new List<ThingDef>();

		// Token: 0x040083F3 RID: 33779
		public List<ThingDef> outcomeStuff = new List<ThingDef>();

		// Token: 0x040083F4 RID: 33780
		[MustTranslate]
		public string failedMessage;

		// Token: 0x0200229C RID: 8860
		public class ElementRatio
		{
			// Token: 0x040083F5 RID: 33781
			public ThingDef sourceStuff;

			// Token: 0x040083F6 RID: 33782
			public float ratio = 1f;
		}
	}
}
