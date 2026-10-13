using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020020ED RID: 8429
	public class TileMutatorWorker_IceCaves : TileMutatorWorker_Caves
	{
		// Token: 0x0600B81F RID: 47135 RVA: 0x0034EBAC File Offset: 0x0034CDAC
		public TileMutatorWorker_IceCaves(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B820 RID: 47136 RVA: 0x000026EA File Offset: 0x000008EA
		public override void GeneratePostElevationFertility(Map map)
		{
		}

		// Token: 0x0600B821 RID: 47137 RVA: 0x00351D10 File Offset: 0x0034FF10
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.GenerateCaves(map);
			MapGenFloatGrid caves = MapGenerator.Caves;
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (caves[intVec] > 0f)
				{
					Thing edifice = intVec.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
						list.Add(intVec);
					}
				}
			}
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
			base.GeneratePostTerrain(map);
		}

		// Token: 0x0600B822 RID: 47138 RVA: 0x00351DAC File Offset: 0x0034FFAC
		protected override bool ShouldCarve(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			if (c.InBounds(map))
			{
				Building edifice = c.GetEdifice(map);
				return ((edifice != null) ? edifice.def : null) == ThingDefOf.SolidIce;
			}
			return false;
		}
	}
}
