using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000697 RID: 1687
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x06002A7E RID: 10878 RVA: 0x000DFADB File Offset: 0x000DDCDB
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x04002270 RID: 8816
		public NeedDef needDef;

		// Token: 0x04002271 RID: 8817
		public float percentPerDay;
	}
}
