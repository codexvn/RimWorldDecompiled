using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030BC RID: 12476
	public class Designator_Slaughter : Designator
	{
		// Token: 0x17002D63 RID: 11619
		// (get) Token: 0x06011848 RID: 71752 RVA: 0x00507F98 File Offset: 0x00506198
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x17002D64 RID: 11620
		// (get) Token: 0x06011849 RID: 71753 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0601184A RID: 71754 RVA: 0x00507FA0 File Offset: 0x005061A0
		public Designator_Slaughter()
		{
			this.defaultLabel = "DesignatorSlaughter".Translate();
			this.defaultDesc = "DesignatorSlaughterDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Slaughter;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x0601184B RID: 71755 RVA: 0x0050802C File Offset: 0x0050622C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.SlaughterablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateSlaughterable".Translate();
			}
			return true;
		}

		// Token: 0x0601184C RID: 71756 RVA: 0x00508068 File Offset: 0x00506268
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0601184D RID: 71757 RVA: 0x005080B8 File Offset: 0x005062B8
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null && !pawn.InAggroMentalState)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0601184E RID: 71758 RVA: 0x00508114 File Offset: 0x00506314
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
		}

		// Token: 0x0601184F RID: 71759 RVA: 0x00508180 File Offset: 0x00506380
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.ShowDesignationWarnings(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06011850 RID: 71760 RVA: 0x005081C6 File Offset: 0x005063C6
		private IEnumerable<Pawn> SlaughterablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return (Pawn)thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06011851 RID: 71761 RVA: 0x005081DD File Offset: 0x005063DD
		private void ShowDesignationWarnings(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x0400B4EA RID: 46314
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
