using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030BA RID: 12474
	public class Designator_RemoveFoundation : Designator_RemoveFloor
	{
		// Token: 0x0601183C RID: 71740 RVA: 0x00507B50 File Offset: 0x00505D50
		public Designator_RemoveFoundation()
		{
			this.defaultLabel = (ModsConfig.OdysseyActive ? "DesignatorRemoveFoundation".Translate() : "DesignatorRemoveBridge".Translate());
			this.defaultDesc = (ModsConfig.OdysseyActive ? "DesignatorRemoveFoundationDesc".Translate() : "DesignatorRemoveBridgeDesc".Translate());
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.soundSucceeded = SoundDefOf.Designate_RemoveBridge;
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x0601183D RID: 71741 RVA: 0x00507BDC File Offset: 0x00505DDC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFoundation) != null)
			{
				return false;
			}
			if (!base.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (WorkGiver_ConstructRemoveFoundation.AnyBuildingBlockingFoundationRemoval(c, base.Map))
			{
				return "MessageCannotRemoveSupportingFoundation".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0601183E RID: 71742 RVA: 0x00507C9B File Offset: 0x00505E9B
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.RemoveFoundation(c, false);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemoveFoundation, null));
		}
	}
}
