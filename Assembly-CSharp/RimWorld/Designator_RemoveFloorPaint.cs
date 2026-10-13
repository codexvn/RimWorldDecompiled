using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B9 RID: 12473
	public class Designator_RemoveFloorPaint : Designator_Cells
	{
		// Token: 0x17002D5F RID: 11615
		// (get) Token: 0x06011836 RID: 71734 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D60 RID: 11616
		// (get) Token: 0x06011837 RID: 71735 RVA: 0x00506A3C File Offset: 0x00504C3C
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x06011838 RID: 71736 RVA: 0x00507A2C File Offset: 0x00505C2C
		public Designator_RemoveFloorPaint()
		{
			this.defaultLabel = "DesignatorRemoveFloorPaint".Translate();
			this.defaultDesc = "DesignatorRemoveFloorPaintDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemovePaint", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_RemovePaint;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "RemovePaint";
		}

		// Token: 0x06011839 RID: 71737 RVA: 0x00507AB8 File Offset: 0x00505CB8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.terrainGrid.ColorAt(c) == null)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0601183A RID: 71738 RVA: 0x00507B10 File Offset: 0x00505D10
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemovePaintFloor, null));
		}

		// Token: 0x0601183B RID: 71739 RVA: 0x0013CBB3 File Offset: 0x0013ADB3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
