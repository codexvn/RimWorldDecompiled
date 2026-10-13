using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003909 RID: 14601
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x0601421F RID: 82463 RVA: 0x005FF0DC File Offset: 0x005FD2DC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x06014220 RID: 82464 RVA: 0x005FF104 File Offset: 0x005FD304
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef == null)
			{
				return;
			}
			float growth = rp.fixedCulativedPlantGrowth ?? Rand.Range(0.2f, 1f);
			int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
			List<Thing> list = new List<Thing>();
			using (CellRect.Enumerator enumerator = rp.rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					if (c.InBounds(map) && !list.Any((Thing p) => p.def.plant.blockAdjacentSow && c.IsAdjacentToCardinalOrInside(p.OccupiedRect())))
					{
						float num = map.fertilityGrid.FertilityAt(c);
						if ((thingDef.plant.completelyIgnoreFertility || num >= thingDef.plant.fertilityMin) && this.TryDestroyBlockingThingsAt(c))
						{
							Plant plant = (Plant)GenSpawn.Spawn(thingDef, c, map, WipeMode.Vanish);
							list.Add(plant);
							plant.Growth = growth;
							if (plant.def.plant.LimitedLifespan)
							{
								plant.Age = age;
							}
						}
					}
				}
			}
		}

		// Token: 0x06014221 RID: 82465 RVA: 0x005FF298 File Offset: 0x005FD498
		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			float minFertility = float.MaxValue;
			bool flag = false;
			foreach (IntVec3 loc in rect)
			{
				float num = map.fertilityGrid.FertilityAt(loc);
				if (num > 0f)
				{
					flag = true;
					minFertility = Mathf.Min(minFertility, num);
				}
			}
			if (!flag)
			{
				return null;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && !x.plant.cavePlant && !x.plant.diesToLight && (x.plant.completelyIgnoreFertility || x.plant.fertilityMin <= minFertility) && x.plant.minGrowthTemperature <= map.mapTemperature.OutdoorTemp && x.plant.maxGrowthTemperature >= map.mapTemperature.OutdoorTemp && x.plant.Harvestable
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06014222 RID: 82466 RVA: 0x005FF360 File Offset: 0x005FD560
		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn) && !SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
				{
					SymbolResolver_CultivatedPlants.tmpThings.Clear();
					return false;
				}
			}
			for (int j = 0; j < SymbolResolver_CultivatedPlants.tmpThings.Count; j++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[j] is Pawn))
				{
					SymbolResolver_CultivatedPlants.tmpThings[j].Destroy(DestroyMode.Vanish);
				}
			}
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			return true;
		}

		// Token: 0x0400DEBB RID: 57019
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x0400DEBC RID: 57020
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
