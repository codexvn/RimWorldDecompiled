using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC5 RID: 6853
	public class PrisonerInteractionModeDef : Def
	{
		// Token: 0x040068C0 RID: 26816
		public int listOrder;

		// Token: 0x040068C1 RID: 26817
		public bool allowOnWildMan = true;

		// Token: 0x040068C2 RID: 26818
		public bool hideIfNoBloodfeeders;

		// Token: 0x040068C3 RID: 26819
		public bool hideOnHemogenicPawns;

		// Token: 0x040068C4 RID: 26820
		public bool mustBeAwake = true;

		// Token: 0x040068C5 RID: 26821
		public bool allowInClassicIdeoMode = true;

		// Token: 0x040068C6 RID: 26822
		public bool hideIfNotRecruitable;

		// Token: 0x040068C7 RID: 26823
		public bool hideIfNotStudiableAsPrisoner;

		// Token: 0x040068C8 RID: 26824
		public bool hideIfGrayFleshNotAppeared;

		// Token: 0x040068C9 RID: 26825
		public bool isNonExclusiveInteraction;

		// Token: 0x040068CA RID: 26826
		public bool isChildInteraction;
	}
}
