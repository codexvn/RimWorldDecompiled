using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026DF RID: 9951
	public class RitualObligationTargetFilterDef : Def
	{
		// Token: 0x0600DA1F RID: 55839 RVA: 0x004087CE File Offset: 0x004069CE
		public RitualObligationTargetFilter GetInstance()
		{
			return (RitualObligationTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x040093A8 RID: 37800
		public Type workerClass;

		// Token: 0x040093A9 RID: 37801
		public bool colonistThingsOnly = true;

		// Token: 0x040093AA RID: 37802
		public List<ThingDef> thingDefs;

		// Token: 0x040093AB RID: 37803
		public int minUnroofedCells;

		// Token: 0x040093AC RID: 37804
		public int unroofedCellSearchRadius;

		// Token: 0x040093AD RID: 37805
		public int woodPerParticipant;

		// Token: 0x040093AE RID: 37806
		public int maxSpeakerDistance;

		// Token: 0x040093AF RID: 37807
		public int maxDrumDistance;
	}
}
