using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003099 RID: 12441
	public class Designator_ExtractSkull : Designator
	{
		// Token: 0x17002D34 RID: 11572
		// (get) Token: 0x06011762 RID: 71522 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x17002D35 RID: 11573
		// (get) Token: 0x06011763 RID: 71523 RVA: 0x00503FC7 File Offset: 0x005021C7
		public override bool Visible
		{
			get
			{
				return ModsConfig.IdeologyActive && WorkGiver_ExtractSkull.CanPlayerExtractSkull();
			}
		}

		// Token: 0x06011764 RID: 71524 RVA: 0x00503FDC File Offset: 0x005021DC
		public Designator_ExtractSkull()
		{
			this.defaultLabel = "DesignatorExtractSkull".Translate();
			this.defaultDesc = "DesignatorExtractSkullDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ExtractSkull", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ExtractSkull;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ExtractSkull";
		}

		// Token: 0x06011765 RID: 71525 RVA: 0x00504068 File Offset: 0x00502268
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

		// Token: 0x06011766 RID: 71526 RVA: 0x005040F4 File Offset: 0x005022F4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!this.Visible)
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			if (corpse == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Head))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) != null)
			{
				return false;
			}
			if (t.SpawnedParentOrMe is Building)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011767 RID: 71527 RVA: 0x005041BC File Offset: 0x005023BC
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

		// Token: 0x06011768 RID: 71528 RVA: 0x00504228 File Offset: 0x00502428
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.ExtractSkull, null));
		}
	}
}
