using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006AE RID: 1710
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x06002AE4 RID: 10980 RVA: 0x000E0FF4 File Offset: 0x000DF1F4
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x040022A5 RID: 8869
		public ChemicalDef chemical;
	}
}
