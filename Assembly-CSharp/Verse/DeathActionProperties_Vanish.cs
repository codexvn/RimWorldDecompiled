using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000194 RID: 404
	public class DeathActionProperties_Vanish : DeathActionProperties
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x00036165 File Offset: 0x00034365
		public DeathActionProperties_Vanish()
		{
			this.workerClass = typeof(DeathActionWorker_Vanish);
		}

		// Token: 0x04000688 RID: 1672
		public FleckDef fleck;

		// Token: 0x04000689 RID: 1673
		public ThingDef filth;

		// Token: 0x0400068A RID: 1674
		public IntRange filthCountRange = IntRange.One;

		// Token: 0x0400068B RID: 1675
		public FleshbeastUtility.MeatExplosionSize? meatExplosionSize;
	}
}
