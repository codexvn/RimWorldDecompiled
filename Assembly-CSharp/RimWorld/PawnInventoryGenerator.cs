using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002364 RID: 9060
	public static class PawnInventoryGenerator
	{
		// Token: 0x0600C47E RID: 50302 RVA: 0x003921D4 File Offset: 0x003903D4
		public static void GenerateInventoryFor(Pawn p, PawnGenerationRequest request)
		{
			p.inventory.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < p.kindDef.fixedInventory.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = p.kindDef.fixedInventory[i];
				Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, thingDefCountClass.stuff);
				thing.stackCount = thingDefCountClass.count;
				CompQuality compQuality;
				if (thing.TryGetComp(out compQuality))
				{
					compQuality.SetQuality(thingDefCountClass.quality, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
				CompColorable compColorable;
				if (thingDefCountClass.color != null && thing.TryGetComp(out compColorable))
				{
					compColorable.SetColor(thingDefCountClass.color.Value);
				}
				if (thing.HasComp<CompEquippable>())
				{
					p.equipment.AddEquipment((ThingWithComps)thing);
				}
				else
				{
					Apparel apparel = thing as Apparel;
					if (apparel != null)
					{
						p.apparel.Wear(apparel, true, false);
					}
					else
					{
						p.inventory.innerContainer.TryAdd(thing, true);
					}
				}
			}
			if (p.kindDef.inventoryOptions != null)
			{
				foreach (Thing item in p.kindDef.inventoryOptions.GenerateThings())
				{
					p.inventory.innerContainer.TryAdd(item, true);
				}
			}
			if (request.AllowFood)
			{
				PawnInventoryGenerator.GiveRandomFood(p);
			}
			PawnInventoryGenerator.GiveDrugsIfAddicted(p);
			PawnInventoryGenerator.GiveCombatEnhancingDrugs(p);
		}

		// Token: 0x0600C47F RID: 50303 RVA: 0x00392354 File Offset: 0x00390554
		public static void GiveRandomFood(Pawn p)
		{
			if (p.kindDef.invNutrition > 0.001f)
			{
				ThingDef def;
				if (p.kindDef.invFoodDef != null)
				{
					def = p.kindDef.invFoodDef;
				}
				else
				{
					float value = Rand.Value;
					if (value < 0.5f)
					{
						def = ThingDefOf.MealSimple;
					}
					else if ((double)value < 0.75)
					{
						def = ThingDefOf.MealFine;
					}
					else
					{
						def = ThingDefOf.MealSurvivalPack;
					}
				}
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = GenMath.RoundRandom(p.kindDef.invNutrition / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
				p.inventory.TryAddItemNotForSale(thing);
			}
		}

		// Token: 0x0600C480 RID: 50304 RVA: 0x003923FC File Offset: 0x003905FC
		private static void GiveDrugsIfAddicted(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			p.health.hediffSet.GetHediffs<Hediff_Addiction>(ref PawnInventoryGenerator.tmpAddictionHediffs, null);
			using (List<Hediff_Addiction>.Enumerator enumerator = PawnInventoryGenerator.tmpAddictionHediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hediff_Addiction addiction = enumerator.Current;
					ThingDef def;
					if (DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
					{
						if (x.category != ThingCategory.Item)
						{
							return false;
						}
						if (p.Faction != null && x.techLevel > p.Faction.def.techLevel)
						{
							return false;
						}
						CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
						return compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == addiction.def;
					}).TryRandomElement(out def))
					{
						int stackCount = Rand.RangeInclusive(2, 5);
						Thing thing = ThingMaker.MakeThing(def, null);
						thing.stackCount = stackCount;
						p.inventory.TryAddItemNotForSale(thing);
					}
				}
			}
		}

		// Token: 0x0600C481 RID: 50305 RVA: 0x003924E8 File Offset: 0x003906E8
		private static void GiveCombatEnhancingDrugs(Pawn pawn)
		{
			if (Rand.Value >= pawn.kindDef.combatEnhancingDrugsChance)
			{
				return;
			}
			if (pawn.IsTeetotaler())
			{
				return;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				CompDrug compDrug = pawn.inventory.innerContainer[i].TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					return;
				}
			}
			int randomInRange = pawn.kindDef.combatEnhancingDrugsCount.RandomInRange;
			if (randomInRange <= 0)
			{
				return;
			}
			IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
			{
				if (x.category != ThingCategory.Item)
				{
					return false;
				}
				if (pawn.Faction != null && x.techLevel > pawn.Faction.def.techLevel)
				{
					return false;
				}
				CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.isCombatEnhancingDrug;
			});
			int num = 0;
			ThingDef def;
			while (num < randomInRange && source.TryRandomElement(out def))
			{
				pawn.inventory.innerContainer.TryAdd(ThingMaker.MakeThing(def, null), true);
				num++;
			}
		}

		// Token: 0x0400860D RID: 34317
		private static List<Hediff_Addiction> tmpAddictionHediffs = new List<Hediff_Addiction>();
	}
}
