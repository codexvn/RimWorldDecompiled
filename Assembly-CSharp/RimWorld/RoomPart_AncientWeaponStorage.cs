using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4A RID: 8010
	public class RoomPart_AncientWeaponStorage : RoomPartWorker
	{
		// Token: 0x0600B128 RID: 45352 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_AncientWeaponStorage(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B129 RID: 45353 RVA: 0x00329E38 File Offset: 0x00328038
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.GenerateRows(ThingDefOf.Shelf, room, map, ThingDefOf.Steel);
			int num = RoomPart_AncientWeaponStorage.WeaponCountRange.RandomInRange;
			int num2 = 999;
			while (num > 0 && num2-- > 0)
			{
				Thing weapon;
				if (EquipmentUtility.TryGenerateWeaponByTag(RoomPart_AncientWeaponStorage.WeaponTags.RandomElement<string>(), out weapon, 99999f) && !weapon.HasThingCategory(ThingCategoryDefOf.WeaponsUnique))
				{
					CompQuality compQuality;
					if (weapon.TryGetComp(out compQuality))
					{
						compQuality.SetQuality(QualityUtility.GenerateQuality(QualityGenerator.BaseGen), new ArtGenerationContext?(ArtGenerationContext.Outsider));
					}
					IntVec3 loc;
					if (room.TryGetRandomCellInRoom(map, out loc, 0, 0, (IntVec3 c) => this.ShelfValidator(map, c, weapon.def), true))
					{
						GenSpawn.Spawn(weapon, loc, map, WipeMode.Vanish).SetForbidden(true, true);
						num--;
					}
				}
			}
		}

		// Token: 0x0600B12A RID: 45354 RVA: 0x00329F48 File Offset: 0x00328148
		private bool ShelfValidator(Map map, IntVec3 c, ThingDef itemDef)
		{
			Building_Storage building_Storage = c.GetFirstThing(map, ThingDefOf.Shelf) as Building_Storage;
			return building_Storage != null && building_Storage.SpaceRemainingFor(itemDef) != 0;
		}

		// Token: 0x0400794B RID: 31051
		private static readonly List<string> WeaponTags = new List<string>
		{
			"IndustrialGunAdvanced",
			"SimpleGun",
			"RangedHeavy"
		};

		// Token: 0x0400794C RID: 31052
		private static readonly IntRange WeaponCountRange = new IntRange(6, 12);
	}
}
