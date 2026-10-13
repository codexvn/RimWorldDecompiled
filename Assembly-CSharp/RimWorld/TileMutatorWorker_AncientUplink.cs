using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020020C7 RID: 8391
	public class TileMutatorWorker_AncientUplink : TileMutatorWorker
	{
		// Token: 0x0600B783 RID: 46979 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_AncientUplink(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B784 RID: 46980 RVA: 0x0034DB38 File Offset: 0x0034BD38
		public override void GeneratePostFog(Map map)
		{
			TileMutatorWorker_AncientUplink.<>c__DisplayClass1_0 CS$<>8__locals1 = new TileMutatorWorker_AncientUplink.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CS$<>8__locals1.prefab = PrefabDefOf.AncientUplink;
			CellRect cellRect;
			if (!MapGenUtility.TryGetRandomClearRect(CS$<>8__locals1.prefab.size.x, CS$<>8__locals1.prefab.size.z, out cellRect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GeneratePostFog>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => base.<GeneratePostFog>g__Validator|0(CellRect.CenteredOn(c, CS$<>8__locals1.prefab.size)), out center))
				{
					return;
				}
				cellRect = CellRect.CenteredOn(center, CS$<>8__locals1.prefab.size);
			}
			foreach (IntVec3 c2 in cellRect)
			{
				CS$<>8__locals1.map.terrainGrid.SetTerrain(c2, TerrainDefOf.AncientTile);
			}
			PrefabUtility.SpawnPrefab(CS$<>8__locals1.prefab, CS$<>8__locals1.map, this.GetPrefabRoot(cellRect), Rot4.North, null, null, null, null, false);
			CS$<>8__locals1.usedRects.Add(cellRect);
		}

		// Token: 0x0600B785 RID: 46981 RVA: 0x0034DC70 File Offset: 0x0034BE70
		private IntVec3 GetPrefabRoot(CellRect rect)
		{
			IntVec3 centerCell = rect.CenterCell;
			if (rect.Width % 2 == 0)
			{
				centerCell.x--;
			}
			if (rect.Height % 2 == 0)
			{
				centerCell.z--;
			}
			return centerCell;
		}
	}
}
