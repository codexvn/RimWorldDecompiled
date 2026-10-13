using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BB RID: 14011
	public class SpecialThingFilterWorker_CorpsesUnnatural : SpecialThingFilterWorker
	{
		// Token: 0x06013759 RID: 79705 RVA: 0x005C8B08 File Offset: 0x005C6D08
		public override bool Matches(Thing t)
		{
			return t is UnnaturalCorpse;
		}
	}
}
