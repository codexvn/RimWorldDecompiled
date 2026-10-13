using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020020FF RID: 8447
	public class TileMutatorWorker_LavaLake : TileMutatorWorker_Lake
	{
		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x0600B868 RID: 47208 RVA: 0x0034FBF4 File Offset: 0x0034DDF4
		protected override float LakeRadius
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x0600B869 RID: 47209 RVA: 0x0034E5E7 File Offset: 0x0034C7E7
		public TileMutatorWorker_LavaLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B86A RID: 47210 RVA: 0x00352F04 File Offset: 0x00351104
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec, map) > 0.25f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B86B RID: 47211 RVA: 0x00352F6C File Offset: 0x0035116C
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			CellRect mapRect = CellRect.WholeMap(map);
			HashSet<IntVec3> visited = new HashSet<IntVec3>();
			HashSet<IntVec3> island = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!visited.Contains(intVec) && intVec.GetTerrain(map) != TerrainDefOf.LavaDeep)
				{
					island.Clear();
					bool touchesMapEdge = false;
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => x.GetTerrain(map) != TerrainDefOf.LavaDeep));
					}
					floodFiller.FloodFill(root, passCheck, delegate(IntVec3 x)
					{
						visited.Add(x);
						island.Add(x);
						if (mapRect.IsOnEdge(x))
						{
							touchesMapEdge = true;
						}
					}, int.MaxValue, false, null);
					if (!touchesMapEdge || island.Count < hashSet.Count)
					{
						using (HashSet<IntVec3>.Enumerator enumerator2 = island.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								IntVec3 c = enumerator2.Current;
								map.terrainGrid.SetTerrain(c, TerrainDefOf.LavaDeep);
							}
							continue;
						}
					}
					if (island.Count > hashSet.Count)
					{
						foreach (IntVec3 c2 in hashSet)
						{
							map.terrainGrid.SetTerrain(c2, TerrainDefOf.LavaDeep);
						}
						hashSet.Clear();
						hashSet.AddRange(island);
					}
				}
			}
		}

		// Token: 0x0600B86C RID: 47212 RVA: 0x003531EC File Offset: 0x003513EC
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (valAt > 0.4f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.LavaDeep);
				return;
			}
			if (valAt > 0.25f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.VolcanicRock);
			}
		}

		// Token: 0x04007F18 RID: 32536
		private const float LavaThreshold = 0.4f;

		// Token: 0x04007F19 RID: 32537
		private const float VolcanicRockThreshold = 0.25f;
	}
}
