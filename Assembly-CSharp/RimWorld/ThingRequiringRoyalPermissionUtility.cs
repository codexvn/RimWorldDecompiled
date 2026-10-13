using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200385C RID: 14428
	public static class ThingRequiringRoyalPermissionUtility
	{
		// Token: 0x06013F29 RID: 81705 RVA: 0x005EB1E0 File Offset: 0x005E93E0
		public static bool IsViolatingRulesOf(Def implantOrWeapon, Pawn pawn, Faction faction, int implantLevel = 0)
		{
			if (faction.def.royalImplantRules == null || faction.def.royalImplantRules.Count == 0)
			{
				return false;
			}
			RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, faction, implantLevel);
			if (minTitleToUse == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			if (currentTitle == null)
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num < 0)
			{
				return false;
			}
			int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleToUse);
			return num < num2;
		}

		// Token: 0x06013F2A RID: 81706 RVA: 0x005EB25C File Offset: 0x005E945C
		public static bool IsViolatingRulesOfAnyFaction(Def implantOrWeapon, Pawn pawn, int implantLevel = 0)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(implantOrWeapon, pawn, faction, implantLevel))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013F2B RID: 81707 RVA: 0x005EB2B8 File Offset: 0x005E94B8
		public static RoyalTitleDef GetMinTitleToUse(Def implantOrWeapon, Faction faction, int implantLevel = 0)
		{
			HediffDef hediffDef = implantOrWeapon as HediffDef;
			if (hediffDef != null)
			{
				return faction.GetMinTitleForImplant(hediffDef, implantLevel);
			}
			return null;
		}

		// Token: 0x06013F2C RID: 81708 RVA: 0x00048A14 File Offset: 0x00046C14
		[Obsolete("Will be removed in the future")]
		public static TaggedString GetEquipWeaponConfirmationDialogText(Thing weapon, Pawn pawn)
		{
			return null;
		}
	}
}
