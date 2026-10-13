using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DFE RID: 15870
	public abstract class TransportersArrivalAction : IExposable
	{
		// Token: 0x170035F1 RID: 13809
		// (get) Token: 0x06015A61 RID: 88673
		public abstract bool GeneratesMap { get; }

		// Token: 0x06015A62 RID: 88674 RVA: 0x0065E5FB File Offset: 0x0065C7FB
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			return true;
		}

		// Token: 0x06015A63 RID: 88675 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return false;
		}

		// Token: 0x06015A64 RID: 88676
		public abstract void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile);

		// Token: 0x06015A65 RID: 88677 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}
	}
}
