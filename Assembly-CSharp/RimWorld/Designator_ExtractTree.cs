using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200309B RID: 12443
	public class Designator_ExtractTree : Designator
	{
		// Token: 0x17002D36 RID: 11574
		// (get) Token: 0x0601176C RID: 71532 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0601176D RID: 71533 RVA: 0x00504258 File Offset: 0x00502458
		public Designator_ExtractTree()
		{
			this.defaultLabel = "DesignatorExtractTree".Translate();
			this.defaultDesc = "DesignatorExtractTreeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ExtractTree", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ExtractTree;
			this.hotKey = KeyBindingDefOf.Misc12;
			this.tutorTag = "ExtractTree";
		}

		// Token: 0x0601176E RID: 71534 RVA: 0x005042E4 File Offset: 0x005024E4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601176F RID: 71535 RVA: 0x00504370 File Offset: 0x00502570
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Plant plant = t as Plant;
			if (plant != null && plant.def.Minifiable && plant.def.plant.IsTree && base.Map.designationManager.DesignationOn(plant, DesignationDefOf.ExtractTree) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06011770 RID: 71536 RVA: 0x005043CC File Offset: 0x005025CC
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					this.DesignateThing(t);
				}
			}
		}

		// Token: 0x06011771 RID: 71537 RVA: 0x00504438 File Offset: 0x00502638
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation((Plant)t, DesignationDefOf.ExtractTree, null));
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.CutPlant);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
			designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.HarvestPlant);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
		}
	}
}
