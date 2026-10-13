using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B8 RID: 12472
	public class Designator_RemoveFloor : Designator_Cells
	{
		// Token: 0x17002D5D RID: 11613
		// (get) Token: 0x06011830 RID: 71728 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D5E RID: 11614
		// (get) Token: 0x06011831 RID: 71729 RVA: 0x005078A3 File Offset: 0x00505AA3
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Floors;
			}
		}

		// Token: 0x06011832 RID: 71730 RVA: 0x005078AC File Offset: 0x00505AAC
		public Designator_RemoveFloor()
		{
			this.defaultLabel = "DesignatorRemoveFloor".Translate();
			this.defaultDesc = "DesignatorRemoveFloorDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveFloor", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_RemoveFloor;
			this.hotKey = KeyBindingDefOf.Misc1;
		}

		// Token: 0x06011833 RID: 71731 RVA: 0x00507930 File Offset: 0x00505B30
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (!base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return "TerrainMustBeRemovable".Translate();
			}
			if (WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, base.Map))
			{
				return false;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06011834 RID: 71732 RVA: 0x005079EF File Offset: 0x00505BEF
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.RemoveTopLayer(c, true);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemoveFloor, null));
		}

		// Token: 0x06011835 RID: 71733 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
