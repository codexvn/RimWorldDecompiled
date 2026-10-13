using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D42 RID: 11586
	public class CompProperties_MechRelay : CompProperties
	{
		// Token: 0x0601046C RID: 66668 RVA: 0x004B4598 File Offset: 0x004B2798
		public CompProperties_MechRelay()
		{
			this.compClass = typeof(CompMechRelay);
		}

		// Token: 0x0400AA03 RID: 43523
		[MustTranslate]
		public string activeInspectString;

		// Token: 0x0400AA04 RID: 43524
		[MustTranslate]
		public string destabilizationInspectString;

		// Token: 0x0400AA05 RID: 43525
		[MustTranslate]
		public string destabilizationMessage;

		// Token: 0x0400AA06 RID: 43526
		[MustTranslate]
		public string crashedLetterLabel;

		// Token: 0x0400AA07 RID: 43527
		[MustTranslate]
		public string crashedLetterText;

		// Token: 0x0400AA08 RID: 43528
		public ThingDef crashedThingDef;
	}
}
