using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030BB RID: 12475
	public class Designator_RemovePaint : Designator_Cells
	{
		// Token: 0x17002D61 RID: 11617
		// (get) Token: 0x0601183F RID: 71743 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D62 RID: 11618
		// (get) Token: 0x06011840 RID: 71744 RVA: 0x00506A3C File Offset: 0x00504C3C
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x06011841 RID: 71745 RVA: 0x00507CD8 File Offset: 0x00505ED8
		public Designator_RemovePaint()
		{
			this.defaultLabel = "DesignatorRemovePaint".Translate();
			this.defaultDesc = "DesignatorRemovePaintDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemovePaint", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_RemovePaint;
			this.hotKey = KeyBindingDefOf.Misc7;
			this.tutorTag = "RemovePaint";
		}

		// Token: 0x06011842 RID: 71746 RVA: 0x00507D64 File Offset: 0x00505F64
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if ((from t in c.GetThingList(base.Map)
			where this.CanDesignateThing(t).Accepted
			select t).Any<Thing>())
			{
				return AcceptanceReport.WasAccepted;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			if (base.Map.terrainGrid.ColorAt(c) == null)
			{
				return "MessageMustDesignatePainted".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06011843 RID: 71747 RVA: 0x00507E24 File Offset: 0x00506024
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
					return;
				}
			}
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemovePaintFloor, null));
		}

		// Token: 0x06011844 RID: 71748 RVA: 0x00507EB0 File Offset: 0x005060B0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && building.PaintColorDef != null;
		}

		// Token: 0x06011845 RID: 71749 RVA: 0x00507F0C File Offset: 0x0050610C
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.PaintBuilding);
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.RemovePaintBuilding);
			if (DebugSettings.godMode)
			{
				((Building)t).ChangePaint(null);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.RemovePaintBuilding, null));
		}

		// Token: 0x06011846 RID: 71750 RVA: 0x0013CBB3 File Offset: 0x0013ADB3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
