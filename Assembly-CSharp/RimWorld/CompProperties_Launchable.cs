using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D0B RID: 11531
	public class CompProperties_Launchable : CompProperties
	{
		// Token: 0x06010338 RID: 66360 RVA: 0x004AF17B File Offset: 0x004AD37B
		public CompProperties_Launchable()
		{
			this.compClass = typeof(CompLaunchable);
		}

		// Token: 0x0400A92B RID: 43307
		public int fixedLaunchDistanceMax = -1;

		// Token: 0x0400A92C RID: 43308
		public float fuelPerTile = 2.25f;

		// Token: 0x0400A92D RID: 43309
		public float minFuelCost = -1f;

		// Token: 0x0400A92E RID: 43310
		public ThingDef skyfallerLeaving;

		// Token: 0x0400A92F RID: 43311
		public ThingDef activeTransporterDef;

		// Token: 0x0400A930 RID: 43312
		public WorldObjectDef worldObjectDef;

		// Token: 0x0400A931 RID: 43313
		public int cooldownTicks;

		// Token: 0x0400A932 RID: 43314
		[MustTranslate]
		public string cooldownEndedMessage;
	}
}
