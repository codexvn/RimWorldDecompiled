using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CBB RID: 11451
	public class CompProperties_GoldenCube : CompProperties_Interactable
	{
		// Token: 0x06010122 RID: 65826 RVA: 0x004A670C File Offset: 0x004A490C
		public CompProperties_GoldenCube()
		{
			this.compClass = typeof(CompGoldenCube);
		}

		// Token: 0x0400A7EC RID: 42988
		[MustTranslate]
		public string letterDeactivatedLabel;

		// Token: 0x0400A7ED RID: 42989
		[MustTranslate]
		public string letterDeactivatedDesc;

		// Token: 0x0400A7EE RID: 42990
		[MustTranslate]
		public string letterDeactivatedAppend;
	}
}
