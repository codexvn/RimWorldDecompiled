using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B4 RID: 12468
	[StaticConstructorOnStartup]
	public class Designator_PaintFloor : Designator_Paint
	{
		// Token: 0x17002D5B RID: 11611
		// (get) Token: 0x0601181E RID: 71710 RVA: 0x0050730C File Offset: 0x0050550C
		protected override Texture2D IconTopTex
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/Designators/PaintFloor_Top", true);
			}
		}

		// Token: 0x17002D5C RID: 11612
		// (get) Token: 0x0601181F RID: 71711 RVA: 0x00507319 File Offset: 0x00505519
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.PaintFloor;
			}
		}

		// Token: 0x06011820 RID: 71712 RVA: 0x00507320 File Offset: 0x00505520
		public Designator_PaintFloor()
		{
			this.defaultLabel = "DesignatorPaintFloor".Translate();
			this.defaultDesc = "DesignatorPaintFloorDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PaintFloor_Bottom", true);
		}

		// Token: 0x06011821 RID: 71713 RVA: 0x00507370 File Offset: 0x00505570
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (!base.Map.terrainGrid.TerrainAt(c).isPaintable)
			{
				return "MessageMustDesignatePaintableFloors".Translate(this.colorDef);
			}
			if (base.Map.terrainGrid.ColorAt(c) == this.colorDef)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011822 RID: 71714 RVA: 0x00507460 File Offset: 0x00505660
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.SetTerrainColor(c, this.colorDef);
				return;
			}
			base.Map.designationManager.TryRemoveDesignation(c, this.Designation);
			base.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.RemovePaintFloor);
			base.Map.designationManager.AddDesignation(new Designation(c, this.Designation, this.colorDef));
		}

		// Token: 0x06011823 RID: 71715 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06011824 RID: 71716 RVA: 0x0013CBB3 File Offset: 0x0013ADB3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x06011825 RID: 71717 RVA: 0x005074F5 File Offset: 0x005056F5
		protected override int NumHighlightedCells()
		{
			return Find.DesignatorManager.Dragger.DragCells.Count;
		}
	}
}
