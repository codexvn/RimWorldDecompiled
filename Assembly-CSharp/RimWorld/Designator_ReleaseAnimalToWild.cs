using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B5 RID: 12469
	public class Designator_ReleaseAnimalToWild : Designator
	{
		// Token: 0x06011826 RID: 71718 RVA: 0x0050750C File Offset: 0x0050570C
		public Designator_ReleaseAnimalToWild()
		{
			this.defaultLabel = "DesignatorReleaseAnimalToWild".Translate();
			this.defaultDesc = "DesignatorReleaseAnimalToWildDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ReleaseToTheWild", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ReleaseToWild;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ReleaseAnimalToWild";
		}

		// Token: 0x06011827 RID: 71719 RVA: 0x00507598 File Offset: 0x00505798
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

		// Token: 0x06011828 RID: 71720 RVA: 0x00507624 File Offset: 0x00505824
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null && !pawn.Dead && pawn.RaceProps.canReleaseToWild;
		}

		// Token: 0x06011829 RID: 71721 RVA: 0x00507684 File Offset: 0x00505884
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

		// Token: 0x0601182A RID: 71722 RVA: 0x005076F0 File Offset: 0x005058F0
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation((Pawn)t, DesignationDefOf.ReleaseAnimalToWild, null));
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal((Pawn)t);
		}
	}
}
