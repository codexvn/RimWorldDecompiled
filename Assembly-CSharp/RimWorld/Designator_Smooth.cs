using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C0 RID: 12480
	public abstract class Designator_Smooth : Designator_Cells
	{
		// Token: 0x17002D67 RID: 11623
		// (get) Token: 0x0601185F RID: 71775 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D68 RID: 11624
		// (get) Token: 0x06011860 RID: 71776 RVA: 0x00503150 File Offset: 0x00501350
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Orders;
			}
		}

		// Token: 0x06011861 RID: 71777 RVA: 0x005084E0 File Offset: 0x005066E0
		protected Designator_Smooth()
		{
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_SmoothSurface;
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06011862 RID: 71778 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06011863 RID: 71779 RVA: 0x0050851C File Offset: 0x0050671C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
