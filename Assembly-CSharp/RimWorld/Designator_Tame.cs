using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C7 RID: 12487
	public class Designator_Tame : Designator
	{
		// Token: 0x17002D6D RID: 11629
		// (get) Token: 0x06011885 RID: 71813 RVA: 0x00508D5F File Offset: 0x00506F5F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x17002D6E RID: 11630
		// (get) Token: 0x06011886 RID: 71814 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011887 RID: 71815 RVA: 0x00508D68 File Offset: 0x00506F68
		public Designator_Tame()
		{
			this.defaultLabel = "DesignatorTame".Translate();
			this.defaultDesc = "DesignatorTameDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Tame;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.tutorTag = "Tame";
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x06011888 RID: 71816 RVA: 0x00508E06 File Offset: 0x00507006
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.TameablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateTameable".Translate();
			}
			return true;
		}

		// Token: 0x06011889 RID: 71817 RVA: 0x00508E44 File Offset: 0x00507044
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0601188A RID: 71818 RVA: 0x00508E94 File Offset: 0x00507094
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!TameUtility.CanTame(pawn))
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return "CantTameScaria".Translate();
			}
			return base.Map.designationManager.DesignationOn(pawn, this.Designation) == null;
		}

		// Token: 0x0601188B RID: 71819 RVA: 0x00508F08 File Offset: 0x00507108
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					TameUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind), true);
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		// Token: 0x0601188C RID: 71820 RVA: 0x00508FBC File Offset: 0x005071BC
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x0601188D RID: 71821 RVA: 0x0050900E File Offset: 0x0050720E
		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
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

		// Token: 0x0400B4FD RID: 46333
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
