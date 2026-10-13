using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003631 RID: 13873
	public static class ExpectationsUtility
	{
		// Token: 0x0601343C RID: 78908 RVA: 0x005B72F0 File Offset: 0x005B54F0
		public static void Reset()
		{
			ExpectationsUtility.wealthExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.WealthTriggered
			orderby ed.order
			select ed).ToList<ExpectationDef>();
			ExpectationsUtility.roleExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.forRoles
			orderby ed.order
			select ed).ToList<ExpectationDef>();
		}

		// Token: 0x0601343D RID: 78909 RVA: 0x005B73AC File Offset: 0x005B55AC
		public static ExpectationDef CurrentExpectationFor(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (p.Faction != Faction.OfPlayer && !p.IsPrisonerOfColony)
			{
				return ExpectationDefOf.ExtremelyLow;
			}
			if (p.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (p.royalty != null && p.MapHeld.IsPlayerHome)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						RoyalTitle currentTitleInFaction = p.royalty.GetCurrentTitleInFaction(faction);
						if (currentTitleInFaction != null && currentTitleInFaction.conceited && currentTitleInFaction.def.minExpectation != null && currentTitleInFaction.def.minExpectation.order > expectationDef.order)
						{
							expectationDef = currentTitleInFaction.def.minExpectation;
						}
					}
				}
				if (ModsConfig.IdeologyActive)
				{
					Ideo ideo = p.Ideo;
					Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
					if (precept_Role != null && precept_Role.def.expectationsOffset != 0 && !MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
					{
						ExpectationDef expectationDef2 = ExpectationsUtility.ExpectationForOrder(Math.Max(expectationDef.order + precept_Role.def.expectationsOffset, 0), true);
						if (expectationDef2 != null)
						{
							expectationDef = expectationDef2;
						}
					}
				}
				return expectationDef;
			}
			return ExpectationDefOf.VeryLow;
		}

		// Token: 0x0601343E RID: 78910 RVA: 0x005B7500 File Offset: 0x005B5700
		public static ExpectationDef CurrentExpectationFor(Map m)
		{
			float wealthTotal = m.wealthWatcher.WealthTotal;
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (wealthTotal < expectationDef.maxMapWealth)
				{
					return expectationDef;
				}
			}
			List<ExpectationDef> list = ExpectationsUtility.wealthExpectationsInOrder;
			return list[list.Count - 1];
		}

		// Token: 0x0601343F RID: 78911 RVA: 0x005B7558 File Offset: 0x005B5758
		public static ExpectationDef ExpectationForOrder(int order, bool forRole = false)
		{
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (order == expectationDef.order)
				{
					return expectationDef;
				}
			}
			if (forRole)
			{
				for (int j = 0; j < ExpectationsUtility.roleExpectationsInOrder.Count; j++)
				{
					ExpectationDef expectationDef2 = ExpectationsUtility.roleExpectationsInOrder[j];
					if (order == expectationDef2.order)
					{
						return expectationDef2;
					}
				}
			}
			return null;
		}

		// Token: 0x06013440 RID: 78912 RVA: 0x005B75C4 File Offset: 0x005B57C4
		public static bool OffsetByRole(Pawn p)
		{
			if (ModsConfig.IdeologyActive && p.ideo != null)
			{
				Precept_Role role = p.Ideo.GetRole(p);
				if (role != null && role.def.expectationsOffset != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400D5CA RID: 54730
		private static List<ExpectationDef> wealthExpectationsInOrder;

		// Token: 0x0400D5CB RID: 54731
		private static List<ExpectationDef> roleExpectationsInOrder;
	}
}
