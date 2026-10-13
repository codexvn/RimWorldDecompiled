using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002115 RID: 8469
	public class TileMutatorWorker_RiverConfluence : TileMutatorWorker_River
	{
		// Token: 0x0600B8CC RID: 47308 RVA: 0x003550D5 File Offset: 0x003532D5
		public TileMutatorWorker_RiverConfluence(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8CD RID: 47309 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B8CE RID: 47310 RVA: 0x00355464 File Offset: 0x00353664
		protected override void GenerateRiverGraph(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return;
			}
			List<SurfaceTile.RiverLink> list = surfaceTile.Rivers.ToList<SurfaceTile.RiverLink>();
			SurfaceTile.RiverLink item = (from rl in list
			orderby ((SurfaceTile)rl.neighbor.Tile).riverDist
			select rl).First<SurfaceTile.RiverLink>();
			list.Remove(item);
			SurfaceTile.RiverLink riverLink = (from rl in list
			where !rl.neighbor.Tile.WaterCovered
			select rl).MaxBy((SurfaceTile.RiverLink rl) => rl.river.widthOnMap);
			list.Remove(riverLink);
			List<SurfaceTile.RiverLink> list2 = (from rl in list
			where !rl.neighbor.Tile.WaterCovered
			select rl).ToList<SurfaceTile.RiverLink>();
			List<SurfaceTile.RiverLink> list3 = (from rl in list
			where rl.neighbor.Tile.WaterCovered
			select rl).ToList<SurfaceTile.RiverLink>();
			float angle = Find.WorldGrid.GetHeadingFromTo(riverLink.neighbor.Tile.tile, item.neighbor.Tile.tile);
			Rot4 a = Find.World.CoastDirectionAt(map.Tile);
			if (a != Rot4.Invalid)
			{
				angle = a.AsAngle;
			}
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, angle);
			Vector3 item2 = mapEdgeNodes.Item1;
			Vector3 item3 = mapEdgeNodes.Item2;
			RiverNode riverNode;
			if (base.IsFlowingAToB(item2, item3, angle))
			{
				riverNode = new RiverNode
				{
					start = item2,
					end = item3,
					width = riverLink.river.widthOnMap
				};
			}
			else
			{
				riverNode = new RiverNode
				{
					start = item3,
					end = item2,
					width = riverLink.river.widthOnMap
				};
			}
			map.waterInfo.riverGraph.Add(riverNode);
			float tvalue = base.GetTValue(riverNode, new Vector2((float)this.riverCenter.x, (float)this.riverCenter.z));
			Vector3 vector = this.GetDisplacedPoint(riverNode, tvalue).ToVector3();
			foreach (SurfaceTile.RiverLink riverLink2 in list2)
			{
				float headingFromTo = Find.WorldGrid.GetHeadingFromTo(riverLink2.neighbor.Tile.tile, surfaceTile.tile);
				ValueTuple<Vector3, Vector3> mapEdgeNodes2 = base.GetMapEdgeNodes(map, headingFromTo);
				Vector3 item4 = mapEdgeNodes2.Item1;
				Vector3 item5 = mapEdgeNodes2.Item2;
				if (base.IsFlowingAToB(item4, item5, headingFromTo))
				{
					RiverNode item6 = new RiverNode
					{
						start = item4,
						end = vector,
						width = riverLink2.river.widthOnMap
					};
					map.waterInfo.riverGraph.Add(item6);
				}
				else
				{
					RiverNode item7 = new RiverNode
					{
						start = item5,
						end = vector,
						width = riverLink2.river.widthOnMap
					};
					map.waterInfo.riverGraph.Add(item7);
				}
			}
			foreach (SurfaceTile.RiverLink riverLink3 in list3)
			{
				float headingFromTo2 = Find.WorldGrid.GetHeadingFromTo(surfaceTile.tile, riverLink3.neighbor.Tile.tile);
				ValueTuple<Vector3, Vector3> mapEdgeNodes3 = base.GetMapEdgeNodes(map, headingFromTo2);
				Vector3 item8 = mapEdgeNodes3.Item1;
				Vector3 item9 = mapEdgeNodes3.Item2;
				if (base.IsFlowingAToB(item8, item9, headingFromTo2))
				{
					RiverNode item10 = new RiverNode
					{
						start = vector,
						end = item9,
						width = riverLink3.river.widthOnMap
					};
					map.waterInfo.riverGraph.Add(item10);
				}
				else
				{
					RiverNode item11 = new RiverNode
					{
						start = vector,
						end = item8,
						width = riverLink3.river.widthOnMap
					};
					map.waterInfo.riverGraph.Add(item11);
				}
			}
		}
	}
}
