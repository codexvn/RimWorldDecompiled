using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8C RID: 8076
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x0600B265 RID: 45669 RVA: 0x00331750 File Offset: 0x0032F950
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlagDefOf.PowerGrid;
		}

		// Token: 0x0600B266 RID: 45670 RVA: 0x00331770 File Offset: 0x0032F970
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x0600B267 RID: 45671 RVA: 0x00331780 File Offset: 0x0032F980
		protected override void TakePrintFrom(Thing t)
		{
			if (t.Faction != null && t.Faction != Faction.OfPlayer)
			{
				return;
			}
			Building building = t as Building;
			if (building != null)
			{
				building.PrintForPowerGrid(this);
			}
		}
	}
}
