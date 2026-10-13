using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001818 RID: 6168
	public class ThoughtWorker_Precept_WieldingNobleOrDespisedWeapon : ThoughtWorker_Precept
	{
		// Token: 0x06008EEE RID: 36590 RVA: 0x002A4808 File Offset: 0x002A2A08
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return string.Empty;
			}
			return label.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06008EEF RID: 36591 RVA: 0x002A4848 File Offset: 0x002A2A48
		public override string PostProcessDescription(Pawn p, string description)
		{
			Pawn_EquipmentTracker equipment = p.equipment;
			ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
			if (thingWithComps == null)
			{
				return string.Empty;
			}
			return description.Formatted(thingWithComps.Named("WEAPON"));
		}

		// Token: 0x06008EF0 RID: 36592 RVA: 0x002A4888 File Offset: 0x002A2A88
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Ideo != null)
			{
				Pawn_EquipmentTracker equipment = p.equipment;
				if (((equipment != null) ? equipment.Primary : null) != null)
				{
					IdeoWeaponDisposition dispositionForWeapon = p.Ideo.GetDispositionForWeapon(p.equipment.Primary.def);
					if (dispositionForWeapon != IdeoWeaponDisposition.None)
					{
						if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
						{
							return ThoughtState.ActiveAtStage(0);
						}
						if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
						{
							return ThoughtState.ActiveAtStage(1);
						}
					}
					return ThoughtState.Inactive;
				}
			}
			return false;
		}
	}
}
