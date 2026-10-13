using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B65 RID: 11109
	public class ThingSetMaker_SubTree : ThingSetMaker
	{
		// Token: 0x0600F863 RID: 63587 RVA: 0x00483B5B File Offset: 0x00481D5B
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return this.def.root.CanGenerate(parms);
		}

		// Token: 0x0600F864 RID: 63588 RVA: 0x00483B6E File Offset: 0x00481D6E
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			outThings.AddRange(this.def.root.Generate(parms));
		}

		// Token: 0x0600F865 RID: 63589 RVA: 0x00483B87 File Offset: 0x00481D87
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return this.def.root.AllGeneratableThingsDebug(parms);
		}

		// Token: 0x0400A358 RID: 41816
		public ThingSetMakerDef def;
	}
}
