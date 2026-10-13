using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020D1 RID: 8401
	public class TileMutatorWorker_ArcheanTrees : TileMutatorWorker
	{
		// Token: 0x0600B7A2 RID: 47010 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_ArcheanTrees(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B7A3 RID: 47011 RVA: 0x0034E2D0 File Offset: 0x0034C4D0
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.radius = ThingDefOf.Plant_TreeArchean.GetCompProperties<CompProperties_Terraformer>().radius;
			int randomInRange = TileMutatorWorker_ArcheanTrees.TreeRange.RandomInRange;
			int num = 0;
			foreach (IntVec3 intVec in map.AllCells.InRandomOrder(null))
			{
				if (this.ValidPosition(intVec, map))
				{
					this.SpawnTree(intVec, map);
					if (++num >= randomInRange)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600B7A4 RID: 47012 RVA: 0x0034E368 File Offset: 0x0034C568
		private void SpawnTree(IntVec3 cell, Map map)
		{
			WildPlantSpawner.SpawnPlant(ThingDefOf.Plant_TreeArchean, map, cell, false).Growth = 1f;
			int num = GenRadial.NumCellsInRadius(this.radius + 1.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				float magnitude = (intVec - cell).Magnitude;
				if (intVec.InBounds(map) && CompTerraformer.CanEverConvertCell(intVec, map, null) && (magnitude <= this.radius || Rand.Chance(0.33f)))
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
				}
			}
			MapGenerator.UsedRects.Add(cell.RectAbout((int)this.radius / 2, (int)this.radius / 2));
		}

		// Token: 0x0600B7A5 RID: 47013 RVA: 0x0034E428 File Offset: 0x0034C628
		private bool ValidPosition(IntVec3 center, Map map)
		{
			if (!CompTerraformer.CanEverConvertCell(center, map, null))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(this.radius);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (CompTerraformer.CanEverConvertCell(center + GenRadial.RadialPattern[i], map, null))
				{
					num2++;
				}
				if (num2 >= 5)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04007E22 RID: 32290
		private float radius;

		// Token: 0x04007E23 RID: 32291
		private const int MinCellsAround = 5;

		// Token: 0x04007E24 RID: 32292
		private const float ExtraRange = 1.9f;

		// Token: 0x04007E25 RID: 32293
		private const float ExtraChance = 0.33f;

		// Token: 0x04007E26 RID: 32294
		private static readonly IntRange TreeRange = new IntRange(10, 15);
	}
}
