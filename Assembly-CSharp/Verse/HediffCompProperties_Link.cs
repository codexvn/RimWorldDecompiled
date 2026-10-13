using System;

namespace Verse
{
	// Token: 0x020006DC RID: 1756
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x06002B81 RID: 11137 RVA: 0x000E3559 File Offset: 0x000E1759
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x04002304 RID: 8964
		public bool showName = true;

		// Token: 0x04002305 RID: 8965
		public float maxDistance = -1f;

		// Token: 0x04002306 RID: 8966
		public bool requireLinkOnOtherPawn = true;

		// Token: 0x04002307 RID: 8967
		public ThingDef customMote;
	}
}
