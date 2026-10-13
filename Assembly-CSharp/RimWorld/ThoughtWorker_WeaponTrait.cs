using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AE RID: 6318
	public class ThoughtWorker_WeaponTrait : ThoughtWorker
	{
		// Token: 0x06009068 RID: 36968 RVA: 0x002A935B File Offset: 0x002A755B
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x06009069 RID: 36969 RVA: 0x002A9380 File Offset: 0x002A7580
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label.Formatted(this.WeaponName(p).Named("WEAPON")));
		}

		// Token: 0x0600906A RID: 36970 RVA: 0x002A93A8 File Offset: 0x002A75A8
		protected string WeaponName(Pawn pawn)
		{
			if (pawn.equipment.bondedWeapon == null)
			{
				return string.Empty;
			}
			if (pawn.equipment.bondedWeapon.StyleSourcePrecept != null)
			{
				return pawn.equipment.bondedWeapon.StyleSourcePrecept.Label;
			}
			CompGeneratedNames compGeneratedNames = pawn.equipment.bondedWeapon.TryGetComp<CompGeneratedNames>();
			if (compGeneratedNames != null)
			{
				return compGeneratedNames.Name;
			}
			return pawn.equipment.bondedWeapon.LabelNoCount;
		}

		// Token: 0x0600906B RID: 36971 RVA: 0x002A941C File Offset: 0x002A761C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			if (((equipment != null) ? equipment.bondedWeapon : null) == null || p.equipment.bondedWeapon.Destroyed)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon == null || compBladelinkWeapon.TraitsListForReading.NullOrEmpty<WeaponTraitDef>())
			{
				return ThoughtState.Inactive;
			}
			return true;
		}
	}
}
