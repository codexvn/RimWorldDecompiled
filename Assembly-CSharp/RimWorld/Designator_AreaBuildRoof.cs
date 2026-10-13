using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003080 RID: 12416
	public class Designator_AreaBuildRoof : Designator_Cells
	{
		// Token: 0x17002D1A RID: 11546
		// (get) Token: 0x060116DD RID: 71389 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D1B RID: 11547
		// (get) Token: 0x060116DE RID: 71390 RVA: 0x00501D53 File Offset: 0x004FFF53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x060116DF RID: 71391 RVA: 0x00501FB8 File Offset: 0x005001B8
		public Designator_AreaBuildRoof()
		{
			this.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc9;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Roof;
			this.useMouseIcon = true;
			this.tutorTag = "AreaBuildRoofExpand";
		}

		// Token: 0x060116E0 RID: 71392 RVA: 0x00502044 File Offset: 0x00500244
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			return !base.Map.areaManager.BuildRoof[c];
		}

		// Token: 0x060116E1 RID: 71393 RVA: 0x00502099 File Offset: 0x00500299
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x060116E2 RID: 71394 RVA: 0x005020CC File Offset: 0x005002CC
		public override bool ShowWarningForCell(IntVec3 c)
		{
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
			{
				if (thing.def.plant != null && thing.def.plant.interferesWithRoof)
				{
					Messages.Message("MessageRoofIncompatibleWithPlant".Translate(thing), MessageTypeDefOf.CautionInput, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060116E3 RID: 71395 RVA: 0x00502164 File Offset: 0x00500364
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
