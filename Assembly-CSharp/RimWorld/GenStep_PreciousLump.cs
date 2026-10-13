using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002078 RID: 8312
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x0600B641 RID: 46657 RVA: 0x00345EBE File Offset: 0x003440BE
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x0600B642 RID: 46658 RVA: 0x00345EC8 File Offset: 0x003440C8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (parms.sitePart != null && parms.sitePart.parms.preciousLumpResources != null)
			{
				this.forcedDefToScatter = parms.sitePart.parms.preciousLumpResources;
			}
			else
			{
				this.forcedDefToScatter = this.mineables.RandomElement<ThingDef>();
			}
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map, parms);
		}

		// Token: 0x0600B643 RID: 46659 RVA: 0x00345F70 File Offset: 0x00344170
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) || !list.Any((CellRect x) => x.Contains(c))) && (!map.wasSpawnedViaGravShipLanding || !CellRect.CenteredOn(MapGenerator.PlayerStartSpot, Find.CurrentGravship.Bounds.Size).ExpandedBy(10).Contains(c)) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
		}

		// Token: 0x0600B644 RID: 46660 RVA: 0x0034600C File Offset: 0x0034420C
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			base.ScatterAt(c, map, parms, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}

		// Token: 0x04007D0E RID: 32014
		public List<ThingDef> mineables;

		// Token: 0x04007D0F RID: 32015
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);
	}
}
