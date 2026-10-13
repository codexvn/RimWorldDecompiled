using System;

namespace Verse
{
	// Token: 0x020006D2 RID: 1746
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x000E27DF File Offset: 0x000E09DF
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x040022EB RID: 8939
		public float infectionChance = 0.5f;
	}
}
