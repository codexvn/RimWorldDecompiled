using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020C4 RID: 8388
	public class TileMutatorWorker_AncientStructure : TileMutatorWorker
	{
		// Token: 0x0600B77A RID: 46970 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_AncientStructure(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B77B RID: 46971 RVA: 0x0034D4B0 File Offset: 0x0034B6B0
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			IntRange structureSizeRange = new IntRange(this.def.structureGenParms.structureSizeRange.min + this.def.structureGenParms.perimeterExpandBy * 2, this.def.structureGenParms.structureSizeRange.max + this.def.structureGenParms.perimeterExpandBy * 2);
			if (!MapGenUtility.TryGetStructureRect(map, structureSizeRange, out this.structurePerimeterRect, null))
			{
				this.structurePerimeterRect = map.Center.RectAbout(structureSizeRange.RandomInRange, structureSizeRange.RandomInRange);
			}
			orGenerateVar.Add(this.structurePerimeterRect);
			CellRect var = this.structurePerimeterRect.ContractedBy(this.def.structureGenParms.perimeterExpandBy);
			MapGenerator.SetVar<CellRect>("SpawnRect", var);
			StructureGenParams parms = new StructureGenParams
			{
				size = var.Size
			};
			LayoutWorker worker = this.def.structureGenParms.structureLayoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			worker.Spawn(layoutStructureSketch, map, var.Min, null, null, true, false, null);
			this.GeneratePerimeter(layoutStructureSketch, map);
		}

		// Token: 0x0600B77C RID: 46972 RVA: 0x0034D5E7 File Offset: 0x0034B7E7
		public override void GenerateNonCriticalStructures(Map map)
		{
			MapGenUtility.SpawnScatteredGroupPrefabs(map, this.structurePerimeterRect, this.def.structureGenParms.scatteredPrefabs);
		}

		// Token: 0x0600B77D RID: 46973 RVA: 0x0034D608 File Offset: 0x0034B808
		private void GeneratePerimeter(LayoutStructureSketch layoutStructureSketch, Map map)
		{
			TileMutatorWorker_AncientStructure.<>c__DisplayClass4_0 CS$<>8__locals1 = new TileMutatorWorker_AncientStructure.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.layoutStructureSketch = layoutStructureSketch;
			if (this.def.structureGenParms.perimeterWallDef == null && this.def.structureGenParms.perimeterTerrainDef == null)
			{
				return;
			}
			CellRect cellRect = CS$<>8__locals1.layoutStructureSketch.structureLayout.container.ExpandedBy(this.def.structureGenParms.perimeterExpandBy);
			if (this.def.structureGenParms.perimeterTerrainDef != null)
			{
				foreach (IntVec3 c3 in cellRect.Cells)
				{
					if (CS$<>8__locals1.<GeneratePerimeter>g__PerimeterValidator|0(c3, this.def.structureGenParms.perimeterTerrainDef))
					{
						CS$<>8__locals1.map.terrainGrid.SetTerrain(c3, this.def.structureGenParms.perimeterTerrainDef);
					}
				}
			}
			float num = (float)cellRect.Area / 100f;
			using (List<LayoutScatterTerrainParms>.Enumerator enumerator2 = this.def.structureGenParms.perimeterScatterTerrain.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LayoutScatterTerrainParms parms = enumerator2.Current;
					int num2;
					if (parms.groupCount != IntRange.Invalid)
					{
						num2 = parms.groupCount.RandomInRange;
					}
					else
					{
						num2 = Mathf.RoundToInt(parms.groupsPerHundredCells.RandomInRange * num);
					}
					num2 = Mathf.Max(num2, parms.minGroups);
					Predicate<IntVec3> <>9__1;
					Predicate<IntVec3> <>9__2;
					for (int i = 0; i < num2; i++)
					{
						int randomInRange = parms.groupDistRange.RandomInRange;
						int randomInRange2 = parms.itemsPerGroup.RandomInRange;
						int num3 = 0;
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 c) => CS$<>8__locals1.<GeneratePerimeter>g__PerimeterValidator|0(c, parms.def)));
						}
						IntVec3 intVec;
						cellRect.TryFindRandomCell(out intVec, validator);
						IntVec3 center = intVec;
						Map map2 = CS$<>8__locals1.map;
						int numCells = randomInRange;
						Predicate<IntVec3> validator2;
						if ((validator2 = <>9__2) == null)
						{
							validator2 = (<>9__2 = ((IntVec3 c) => CS$<>8__locals1.<GeneratePerimeter>g__PerimeterValidator|0(c, parms.def)));
						}
						List<IntVec3> list = GridShapeMaker.IrregularLump(center, map2, numCells, validator2);
						for (int j = 0; j < list.Count; j++)
						{
							IntVec3 c2 = list[j];
							if (CS$<>8__locals1.<GeneratePerimeter>g__PerimeterValidator|0(c2, parms.def) && Rand.DynamicChance(num3, randomInRange2, list.Count - j))
							{
								num3++;
								CS$<>8__locals1.map.terrainGrid.SetTerrain(c2, parms.def);
							}
						}
					}
				}
			}
			if (this.def.structureGenParms.perimeterWallDef != null)
			{
				foreach (IntVec3 intVec2 in cellRect.EdgeCells)
				{
					if (Rand.Chance(this.def.structureGenParms.perimeterWallChance) && !CS$<>8__locals1.layoutStructureSketch.AnyRoomContains(intVec2, null) && GenSpawn.CanSpawnAt(this.def.structureGenParms.perimeterWallDef, intVec2, CS$<>8__locals1.map, null, false))
					{
						GenSpawn.Spawn(this.def.structureGenParms.perimeterWallDef, intVec2, CS$<>8__locals1.map, WipeMode.Vanish);
					}
				}
			}
			if (!this.def.structureGenParms.perimeterScatteredThings.NullOrEmpty<AncientStructureGenParms.ScatteredThing>())
			{
				foreach (IntVec3 intVec3 in cellRect.Cells)
				{
					if (!CS$<>8__locals1.layoutStructureSketch.AnyRoomContains(intVec3, null))
					{
						AncientStructureGenParms.ScatteredThing scatteredThing = this.def.structureGenParms.perimeterScatteredThings.RandomElement<AncientStructureGenParms.ScatteredThing>();
						if (Rand.Chance(scatteredThing.chancePer100Cells / 100f))
						{
							Rot4 rot = scatteredThing.thingDef.rotatable ? Rot4.Random : scatteredThing.thingDef.defaultPlacingRot;
							if (GenSpawn.CanSpawnAt(scatteredThing.thingDef, intVec3, CS$<>8__locals1.map, new Rot4?(rot), false))
							{
								GenSpawn.Spawn(scatteredThing.thingDef, intVec3, CS$<>8__locals1.map, rot, WipeMode.Vanish);
							}
						}
					}
				}
			}
		}

		// Token: 0x04007E08 RID: 32264
		private CellRect structurePerimeterRect;
	}
}
