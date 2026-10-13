using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FD RID: 4861
	public static class MechRepairUtility
	{
		// Token: 0x06007744 RID: 30532 RVA: 0x00243EA0 File Offset: 0x002420A0
		private static Hediff GetHediffToHeal(Pawn mech)
		{
			Hediff hediff = null;
			float num = float.PositiveInfinity;
			foreach (Hediff hediff2 in mech.health.hediffSet.hediffs)
			{
				if (hediff2 is Hediff_Injury && hediff2.Severity < num)
				{
					num = hediff2.Severity;
					hediff = hediff2;
				}
			}
			if (hediff != null)
			{
				return hediff;
			}
			foreach (Hediff hediff3 in mech.health.hediffSet.hediffs)
			{
				if (hediff3 is Hediff_MissingPart)
				{
					return hediff3;
				}
			}
			return null;
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x00243F78 File Offset: 0x00242178
		public static bool CanRepair(Pawn mech)
		{
			return mech.TryGetComp<CompMechRepairable>() != null && (MechRepairUtility.GetHediffToHeal(mech) != null || MechRepairUtility.IsMissingWeapon(mech));
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x00243F94 File Offset: 0x00242194
		public static void RepairTick(Pawn mech, int delta)
		{
			Hediff hediffToHeal = MechRepairUtility.GetHediffToHeal(mech);
			if (hediffToHeal == null)
			{
				MechRepairUtility.GenerateWeapon(mech);
				return;
			}
			Hediff_MissingPart hediff_MissingPart = hediffToHeal as Hediff_MissingPart;
			if (hediff_MissingPart != null)
			{
				mech.health.RemoveHediff(hediff_MissingPart);
				return;
			}
			hediffToHeal.Heal((float)delta);
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x00243FD4 File Offset: 0x002421D4
		public static bool IsMissingWeapon(Pawn mech)
		{
			List<string> weaponTags = mech.kindDef.weaponTags;
			if (weaponTags.NullOrEmpty<string>())
			{
				return false;
			}
			List<ThingWithComps> allEquipmentListForReading = mech.equipment.AllEquipmentListForReading;
			Predicate<string> <>9__0;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				List<string> weaponTags2 = allEquipmentListForReading[i].def.weaponTags;
				Predicate<string> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string t) => weaponTags.Contains(t)));
				}
				if (weaponTags2.Any(predicate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x00244060 File Offset: 0x00242260
		public static void GenerateWeapon(Pawn mech)
		{
			if (!MechRepairUtility.IsMissingWeapon(mech))
			{
				return;
			}
			PawnWeaponGenerator.TryGenerateWeaponFor(mech, default(PawnGenerationRequest));
		}
	}
}
