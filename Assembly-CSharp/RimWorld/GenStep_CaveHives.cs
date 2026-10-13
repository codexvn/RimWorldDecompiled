using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA3 RID: 8099
	public class GenStep_CaveHives : GenStep
	{
		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x0600B2DC RID: 45788 RVA: 0x003339E1 File Offset: 0x00331BE1
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x0600B2DD RID: 45789 RVA: 0x003339E8 File Offset: 0x00331BE8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.Storyteller.difficulty.allowCaveHives)
			{
				return;
			}
			if (Faction.OfInsects == null)
			{
				return;
			}
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			float num = 0.7f;
			int num2 = 0;
			this.rockCells.Clear();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (elevation[intVec] > num)
				{
					this.rockCells.Add(intVec);
				}
				if (caves[intVec] > 0f)
				{
					num2++;
				}
			}
			List<IntVec3> list = (from c in map.AllCells
			where map.thingGrid.ThingsAt(c).Any((Thing thing) => thing.Faction != null)
			select c).ToList<IntVec3>();
			GenMorphology.Dilate(list, 50, map, null);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>(list);
			int num3 = GenMath.RoundRandom((float)num2 / 1000f);
			GenMorphology.Erode(this.rockCells, 10, map, null);
			this.possibleSpawnCells.Clear();
			for (int i = 0; i < this.rockCells.Count; i++)
			{
				if (caves[this.rockCells[i]] > 0f && !hashSet.Contains(this.rockCells[i]))
				{
					this.possibleSpawnCells.Add(this.rockCells[i]);
				}
			}
			this.spawnedHives.Clear();
			for (int j = 0; j < num3; j++)
			{
				this.TrySpawnHive(map);
			}
			this.spawnedHives.Clear();
		}

		// Token: 0x0600B2DE RID: 45790 RVA: 0x00333BA8 File Offset: 0x00331DA8
		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec;
			if (!this.TryFindHiveSpawnCell(map, out intVec))
			{
				return;
			}
			this.possibleSpawnCells.Remove(intVec);
			Hive item = HiveUtility.SpawnHive(intVec, map, WipeMode.VanishOrMoveAside, true, false, false, false, false, false, true);
			this.spawnedHives.Add(item);
		}

		// Token: 0x0600B2DF RID: 45791 RVA: 0x00333BEC File Offset: 0x00331DEC
		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			Func<IntVec3, bool> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IEnumerable<IntVec3> source = this.possibleSpawnCells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null));
				}
				IntVec3 intVec2;
				if (!source.Where(predicate).TryRandomElement(out intVec2))
				{
					break;
				}
				float num2 = -1f;
				for (int j = 0; j < this.spawnedHives.Count; j++)
				{
					float num3 = (float)intVec2.DistanceToSquared(this.spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!intVec.IsValid || num2 > num)
				{
					intVec = intVec2;
					num = num2;
				}
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}

		// Token: 0x04007A77 RID: 31351
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x04007A78 RID: 31352
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x04007A79 RID: 31353
		private List<Hive> spawnedHives = new List<Hive>();

		// Token: 0x04007A7A RID: 31354
		private const int MinDistToOpenSpace = 10;

		// Token: 0x04007A7B RID: 31355
		private const int MinDistFromFactionBase = 50;

		// Token: 0x04007A7C RID: 31356
		private const float CaveCellsPerHive = 1000f;
	}
}
