using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A83 RID: 10883
	public static class BillDialogUtility
	{
		// Token: 0x0600F24D RID: 62029 RVA: 0x0046587B File Offset: 0x00463A7B
		public static IEnumerable<Widgets.DropdownMenuElement<Pawn>> GetPawnRestrictionOptionsForBill(Bill bill, Func<Pawn, bool> pawnValidator = null)
		{
			bool workSkill = bill.recipe.workSkill != null;
			IEnumerable<Pawn> enumerable = PawnsFinder.AllMaps_FreeColonists;
			enumerable = from p in enumerable
			where pawnValidator == null || pawnValidator(p)
			select p into pawn
			orderby pawn.LabelShortCap
			select pawn;
			if (workSkill)
			{
				enumerable = from pawn in enumerable
				orderby pawn.skills.GetSkill(bill.recipe.workSkill).Level descending
				select pawn;
			}
			WorkGiverDef workGiver = bill.billStack.billGiver.GetWorkgiver();
			if (workGiver == null)
			{
				Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148);
				yield break;
			}
			enumerable = from pawn in enumerable
			orderby pawn.workSettings.WorkIsActive(workGiver.workType) descending
			select pawn;
			enumerable = from pawn in enumerable
			orderby pawn.WorkTypeIsDisabled(workGiver.workType)
			select pawn;
			using (IEnumerator<Pawn> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					if (pawn.WorkTypeIsDisabled(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.label)), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
							{
								pawn.LabelShortCap,
								pawn.skills.GetSkill(bill.recipe.workSkill).Level,
								bill.recipe.workSkill.label,
								"NotAssigned".Translate()
							}), delegate()
							{
								bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (!pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate()
							{
								bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else if (bill.recipe.workSkill != null)
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(bill.recipe.workSkill).Level, bill.recipe.workSkill.label), delegate()
							{
								bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate()
							{
								bill.SetPawnRestriction(pawn);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
							payload = pawn
						};
					}
				}
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}
	}
}
