using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030FA RID: 12538
	public abstract class Designator_Zone : Designator_Cells
	{
		// Token: 0x17002DC3 RID: 11715
		// (get) Token: 0x060119E2 RID: 72162 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002DC4 RID: 11716
		// (get) Token: 0x060119E3 RID: 72163 RVA: 0x0050E78D File Offset: 0x0050C98D
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Zones;
			}
		}

		// Token: 0x060119E4 RID: 72164 RVA: 0x0050E794 File Offset: 0x0050C994
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			OverlayDrawHandler.DrawZonesThisFrame();
			if (Find.Selector.SelectedZone != null)
			{
				GenDraw.DrawFieldEdges(Find.Selector.SelectedZone.Cells, 2900);
			}
			GenDraw.DrawNoZoneEdgeLines();
		}
	}
}
