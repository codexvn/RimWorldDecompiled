using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030CE RID: 12494
	public class Designator_Uninstall : Designator
	{
		// Token: 0x17002D72 RID: 11634
		// (get) Token: 0x060118AA RID: 71850 RVA: 0x0022523F File Offset: 0x0022343F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17002D73 RID: 11635
		// (get) Token: 0x060118AB RID: 71851 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x060118AC RID: 71852 RVA: 0x00509624 File Offset: 0x00507824
		public Designator_Uninstall()
		{
			this.defaultLabel = "DesignatorUninstall".Translate();
			this.defaultDesc = "DesignatorUninstallDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Uninstall", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x060118AD RID: 71853 RVA: 0x005096A8 File Offset: 0x005078A8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				return false;
			}
			if (this.TopUninstallableInCell(c) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060118AE RID: 71854 RVA: 0x005096FC File Offset: 0x005078FC
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		// Token: 0x060118AF RID: 71855 RVA: 0x0050970C File Offset: 0x0050790C
		private Thing TopUninstallableInCell(IntVec3 loc)
		{
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(thing).Accepted)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x060118B0 RID: 71856 RVA: 0x0050979C File Offset: 0x0050799C
		public override void DesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				t.SetFaction(Faction.OfPlayer, null);
			}
			if (DebugSettings.godMode || t.GetStatValue(StatDefOf.WorkToBuild, true, -1) == 0f || t.def.IsFrame)
			{
				t.Uninstall();
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
				{
					Messages.Message("MessageOccupiedHoldingPlatformUninstalled".Translate(), building_HoldingPlatform, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x060118B1 RID: 71857 RVA: 0x00509850 File Offset: 0x00507A50
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!building.def.Minifiable)
			{
				return false;
			}
			if (!DebugSettings.godMode && building.Faction != Faction.OfPlayer && !building.def.building.alwaysUninstallable)
			{
				if (building.Faction != null)
				{
					return false;
				}
				if (!building.ClaimableBy(Faction.OfPlayer))
				{
					return false;
				}
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060118B2 RID: 71858 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
