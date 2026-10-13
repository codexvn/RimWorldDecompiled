using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030A1 RID: 12449
	public class Designator_Hunt : Designator
	{
		// Token: 0x17002D3C RID: 11580
		// (get) Token: 0x06011790 RID: 71568 RVA: 0x00504B9F File Offset: 0x00502D9F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x17002D3D RID: 11581
		// (get) Token: 0x06011791 RID: 71569 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011792 RID: 71570 RVA: 0x00504BA8 File Offset: 0x00502DA8
		public Designator_Hunt()
		{
			this.defaultLabel = "DesignatorHunt".Translate();
			this.defaultDesc = "DesignatorHuntDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x06011793 RID: 71571 RVA: 0x00504C34 File Offset: 0x00502E34
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.HuntablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateHuntable".Translate();
			}
			return true;
		}

		// Token: 0x06011794 RID: 71572 RVA: 0x00504C70 File Offset: 0x00502E70
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06011795 RID: 71573 RVA: 0x00504CC0 File Offset: 0x00502EC0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.AnimalOrWildMan() && !pawn.IsPrisonerInPrisonCell() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06011796 RID: 71574 RVA: 0x00504D28 File Offset: 0x00502F28
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x06011797 RID: 71575 RVA: 0x00504D7C File Offset: 0x00502F7C
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					Designator_Hunt.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
		}

		// Token: 0x06011798 RID: 71576 RVA: 0x00504E24 File Offset: 0x00503024
		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
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

		// Token: 0x06011799 RID: 71577 RVA: 0x00504E3C File Offset: 0x0050303C
		public static void ShowDesignationWarnings(Pawn pawn)
		{
			bool flag;
			bool flag2;
			Designator_Hunt.CheckHunters(pawn.MapHeld, out flag, out flag2);
			if (!flag)
			{
				Messages.Message("MessageNoHuntersAvailable".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			else if (!flag2)
			{
				Messages.Message("MessageNoHuntersWithProperWeapon".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			float manhunterOnDamageChance = pawn.RaceProps.manhunterOnDamageChance;
			float manhunterOnDamageChance2 = PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			if (manhunterOnDamageChance >= 0.015f)
			{
				Messages.Message("MessageAnimalsGoPsychoHunted".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnDamageChance2.ToStringPercent(), pawn.Named("ANIMAL")).CapitalizeFirst(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x0601179A RID: 71578 RVA: 0x00504F1C File Offset: 0x0050311C
		private static void CheckHunters(Map map, out bool anyAssignedHunting, out bool anyProperWeapon)
		{
			anyAssignedHunting = false;
			anyProperWeapon = false;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				bool flag = pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting);
				if (flag)
				{
					anyAssignedHunting = true;
				}
				if (!pawn.Downed && flag && WorkGiver_HunterHunt.HasHuntingWeapon(pawn))
				{
					anyProperWeapon = true;
					break;
				}
			}
		}

		// Token: 0x0400B4B2 RID: 46258
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
