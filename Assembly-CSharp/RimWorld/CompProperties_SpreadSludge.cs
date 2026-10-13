using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E51 RID: 11857
	public class CompProperties_SpreadSludge : CompProperties
	{
		// Token: 0x06010B0E RID: 68366 RVA: 0x004CFC5C File Offset: 0x004CDE5C
		public CompProperties_SpreadSludge()
		{
			this.compClass = typeof(CompSpreadSludge);
		}

		// Token: 0x0400ADFF RID: 44543
		public int mtbTicks = 2500;

		// Token: 0x0400AE00 RID: 44544
		public AbilityDef abilityDef;

		// Token: 0x0400AE01 RID: 44545
		public bool tryAvoidSludge;
	}
}
