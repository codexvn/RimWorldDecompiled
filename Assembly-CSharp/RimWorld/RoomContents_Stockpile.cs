using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1E RID: 7966
	public class RoomContents_Stockpile : RoomContentsWorker
	{
		// Token: 0x0600B020 RID: 45088 RVA: 0x00322F68 File Offset: 0x00321168
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.GenerateRows(ThingDefOf.Shelf, room, map, ThingDefOf.Steel);
			PocketMapExit pocketMapExit = map.listerThings.ThingsOfDef(ThingDefOf.AncientHatchExit).FirstOrDefault<Thing>() as PocketMapExit;
			AncientHatch ancientHatch = (((pocketMapExit != null) ? pocketMapExit.entrance : null) ?? PocketMapUtility.currentlyGeneratingPortal) as AncientHatch;
			if (ancientHatch != null)
			{
				switch (ancientHatch.stockpileType)
				{
				case TileMutatorWorker_Stockpile.StockpileType.Medicine:
					this.GenerateItems(map, room, ThingDefOf.MedicineUltratech, new IntRange(5, 10));
					this.GenerateItems(map, room, ThingDefOf.MedicineIndustrial, new IntRange(30, 60));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Chemfuel:
					this.GenerateItems(map, room, ThingDefOf.Chemfuel, new IntRange(750, 1200));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Component:
					this.GenerateItems(map, room, ThingDefOf.ComponentIndustrial, new IntRange(25, 50));
					this.GenerateItems(map, room, ThingDefOf.ComponentSpacer, new IntRange(2, 3));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Weapons:
					this.GenerateWeapons(map, room, new IntRange(2, 3));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Gravcore:
					this.GenerateItems(map, room, ThingDefOf.Gravcore, IntRange.One);
					this.GenerateItems(map, room, ThingDefOf.GravlitePanel, new IntRange(150, 200));
					break;
				case TileMutatorWorker_Stockpile.StockpileType.Drugs:
					this.GenerateItems(map, room, ThingDefOf.SmokeleafJoint, new IntRange(5, 20));
					this.GenerateItems(map, room, ThingDefOf.Flake, new IntRange(25, 100));
					this.GenerateItems(map, room, ThingDefOf.Yayo, new IntRange(25, 60));
					this.GenerateItems(map, room, ThingDefOf.GoJuice, new IntRange(-10, 10));
					this.GenerateItems(map, room, ThingDefOf.WakeUp, new IntRange(-10, 10));
					this.GenerateItems(map, room, ThingDefOf.Luciferium, new IntRange(5, 15));
					break;
				}
			}
			LayoutRoomDef layoutRoomDef = room.defs.FirstOrDefault((LayoutRoomDef x) => x.thingSetMakerDef != null);
			ThingSetMakerDef thingSetMakerDef = (layoutRoomDef != null) ? layoutRoomDef.thingSetMakerDef : null;
			if (thingSetMakerDef != null)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(2200f));
				List<Thing> items = thingSetMakerDef.root.Generate(parms);
				this.GenerateItems(map, room, items);
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600B021 RID: 45089 RVA: 0x003231B0 File Offset: 0x003213B0
		private void GenerateItems(Map map, LayoutRoom room, List<Thing> items)
		{
			int num = 999;
			while (items.Count > 0 && num-- > 0)
			{
				Thing itemToSpawn = items.Last<Thing>();
				items.Remove(itemToSpawn);
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map, out loc, 0, 0, (IntVec3 c) => this.ShelfValidator(map, c, itemToSpawn.def), true))
				{
					break;
				}
				GenSpawn.Spawn(itemToSpawn, loc, map, WipeMode.Vanish).SetForbidden(true, true);
			}
		}

		// Token: 0x0600B022 RID: 45090 RVA: 0x00323254 File Offset: 0x00321454
		private void GenerateItems(Map map, LayoutRoom room, ThingDef itemDef, IntRange countRange)
		{
			int num = countRange.RandomInRange;
			int num2 = 99;
			Func<IntVec3, bool> <>9__0;
			while (num > 0 && num2-- > 0)
			{
				Map map2 = map;
				int contractedBy = 0;
				int padding = 0;
				Func<IntVec3, bool> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.ShelfValidator(map, c, itemDef)));
				}
				IntVec3 loc;
				if (!room.TryGetRandomCellInRoom(map2, out loc, contractedBy, padding, validator, true))
				{
					break;
				}
				int num3 = Rand.Range(1, Mathf.Min(countRange.max / 4, itemDef.stackLimit));
				num3 = Mathf.Min(num3, num);
				Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(itemDef, null), loc, map, WipeMode.Vanish);
				thing.stackCount = num3;
				thing.SetForbidden(true, true);
				num -= num3;
			}
		}

		// Token: 0x0600B023 RID: 45091 RVA: 0x00323328 File Offset: 0x00321528
		private void GenerateWeapons(Map map, LayoutRoom room, IntRange countRange)
		{
			int num = countRange.RandomInRange;
			int num2 = 999;
			while (num > 0 && num2-- > 0)
			{
				Thing weapon;
				if (EquipmentUtility.TryGenerateWeaponByTag(RoomContents_Stockpile.WeaponTags.RandomElement<string>(), out weapon, 99999f) && !weapon.HasThingCategory(ThingCategoryDefOf.WeaponsUnique))
				{
					CompQuality compQuality;
					if (weapon.TryGetComp(out compQuality))
					{
						compQuality.SetQuality(QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Excellent), new ArtGenerationContext?(ArtGenerationContext.Outsider));
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

		// Token: 0x0600B024 RID: 45092 RVA: 0x0032341C File Offset: 0x0032161C
		private bool ShelfValidator(Map map, IntVec3 c, ThingDef itemDef)
		{
			Building_Storage building_Storage = c.GetFirstThing(map, ThingDefOf.Shelf) as Building_Storage;
			return building_Storage != null && building_Storage.SpaceRemainingFor(itemDef) != 0;
		}

		// Token: 0x040078B3 RID: 30899
		private static readonly List<string> WeaponTags = new List<string>
		{
			"IndustrialGunAdvanced",
			"SpacerGun",
			"RangedHeavy"
		};
	}
}
