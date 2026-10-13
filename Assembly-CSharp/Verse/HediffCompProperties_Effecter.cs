using System;

namespace Verse
{
	// Token: 0x020006B0 RID: 1712
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x06002AEB RID: 10987 RVA: 0x000E10CC File Offset: 0x000DF2CC
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x040022A7 RID: 8871
		public EffecterDef stateEffecter;

		// Token: 0x040022A8 RID: 8872
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
