using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D19 RID: 15641
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x17003513 RID: 13587
		// (get) Token: 0x060154A7 RID: 87207
		public abstract string Label { get; }

		// Token: 0x17003514 RID: 13588
		// (get) Token: 0x060154A8 RID: 87208
		public abstract string ReportString { get; }

		// Token: 0x060154A9 RID: 87209 RVA: 0x0065E5FB File Offset: 0x0065C7FB
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			return true;
		}

		// Token: 0x060154AA RID: 87210
		public abstract void Arrived(Caravan caravan);

		// Token: 0x060154AB RID: 87211 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}
	}
}
