using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB8 RID: 8120
	public class GenStep_GeothermalVents : GenStep_ScatterThings
	{
		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x0600B319 RID: 45849 RVA: 0x003351AB File Offset: 0x003333AB
		public override int SeedPart
		{
			get
			{
				return 2140896;
			}
		}

		// Token: 0x0600B31A RID: 45850 RVA: 0x003351B4 File Offset: 0x003333B4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Generate(map, parms);
			float x = this.densityByDistance.MaxBy((CurvePoint p) => p.x).x;
			ThingGrid thingGrid = map.thingGrid;
			foreach (IntVec3 intVec in this.geothermalVents)
			{
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(intVec, x, false))
				{
					if (thingGrid.ThingAt(intVec2, ThingDefOf.GeothermalVent) == null)
					{
						float lengthHorizontal = (intVec2 - intVec).LengthHorizontal;
						if (Rand.Chance(this.densityByDistance.Evaluate(lengthHorizontal)))
						{
							this.DoPlaceRandomFloraAt(intVec2, map);
						}
					}
				}
			}
			this.geothermalVents.Clear();
		}

		// Token: 0x0600B31B RID: 45851 RVA: 0x003352D8 File Offset: 0x003334D8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			this.geothermalVents.Add(loc);
			base.ScatterAt(loc, map, parms, stackCount);
		}

		// Token: 0x0600B31C RID: 45852 RVA: 0x003352F4 File Offset: 0x003334F4
		private void DoPlaceRandomFloraAt(IntVec3 pos, Map map)
		{
			ThingDef plant = this.floraToScatter.RandomElementByWeight((BiomePlantRecord f) => f.commonality).plant;
			Thing thing;
			if (!plant.CanEverPlantAt(pos, map, out thing, false, true, true))
			{
				return;
			}
			Plant plant2 = (Plant)ThingMaker.MakeThing(plant, null);
			plant2.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
			if (plant2.def.plant.LimitedLifespan)
			{
				plant2.Age = Rand.Range(0, Mathf.Max(plant2.def.plant.LifespanTicks - 50, 0));
			}
			GenSpawn.Spawn(plant2, pos, map, WipeMode.Vanish);
		}

		// Token: 0x04007AB6 RID: 31414
		public List<BiomePlantRecord> floraToScatter;

		// Token: 0x04007AB7 RID: 31415
		public SimpleCurve densityByDistance;

		// Token: 0x04007AB8 RID: 31416
		[Unsaved(false)]
		private List<IntVec3> geothermalVents = new List<IntVec3>();
	}
}
