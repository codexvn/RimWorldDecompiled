using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200211A RID: 8474
	public class TileMutatorWorker_RiverIsland : TileMutatorWorker_River
	{
		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x0600B8E6 RID: 47334 RVA: 0x00356058 File Offset: 0x00354258
		protected override float GetCurveAmplitude
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x0600B8E7 RID: 47335 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B8E8 RID: 47336 RVA: 0x003550D5 File Offset: 0x003532D5
		public TileMutatorWorker_RiverIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8E9 RID: 47337 RVA: 0x00356060 File Offset: 0x00354260
		public override void Init(Map map)
		{
			base.Init(map);
			this.riverIslandAmplitude = TileMutatorWorker_RiverIsland.RiverIslandAmplitudeRange.RandomInRange;
			this.riverIslandCentrality = TileMutatorWorker_RiverIsland.RiverIslandCentralityRange.RandomInRange;
			this.widthFrac = TileMutatorWorker_RiverIsland.RiverWidthFracRange.RandomInRange;
		}

		// Token: 0x0600B8EA RID: 47338 RVA: 0x003560B0 File Offset: 0x003542B0
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
			List<SurfaceTile.RiverLink> source = (from rl in surfaceTile.Rivers
			orderby -((SurfaceTile)rl.neighbor.Tile).riverDist
			select rl).ToList<SurfaceTile.RiverLink>();
			WorldGrid worldGrid = Find.WorldGrid;
			SurfaceTile.RiverLink riverLink = source.First<SurfaceTile.RiverLink>();
			PlanetTile tile = riverLink.neighbor.Tile.tile;
			riverLink = source.Last<SurfaceTile.RiverLink>();
			float headingFromTo = worldGrid.GetHeadingFromTo(tile, riverLink.neighbor.Tile.tile);
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, headingFromTo);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			if (base.IsFlowingAToB(item, item2, headingFromTo))
			{
				this.nodeA = new RiverNode
				{
					start = item,
					end = item2,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
				this.nodeB = new RiverNode
				{
					start = item,
					end = item2,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
			}
			else
			{
				this.nodeA = new RiverNode
				{
					start = item2,
					end = item,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
				this.nodeB = new RiverNode
				{
					start = item2,
					end = item,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
			}
			map.waterInfo.riverGraph.Add(this.nodeA);
			map.waterInfo.riverGraph.Add(this.nodeB);
		}

		// Token: 0x0600B8EB RID: 47339 RVA: 0x00356254 File Offset: 0x00354454
		protected override Vector2 GetDisplacedPoint(RiverNode riverNode, float t)
		{
			Vector3 vector = riverNode.end - riverNode.start;
			Vector3 a = riverNode.start + t * vector;
			float d = -4f * Mathf.Pow(t, 2f) + 4f * t;
			float num = t * Vector3.Distance(riverNode.start, riverNode.end);
			Vector3 normalized = new Vector3(-vector.z, 0f, vector.x).normalized;
			float num2 = Mathf.InverseLerp(this.riverIslandCentrality, 1f - this.riverIslandCentrality, t);
			float num3 = (float)this.riverBendNoise.GetValue((double)(num * this.GetWidthNoiseFactor(riverNode)), (double)(riverNode.start.x + riverNode.start.z + num2 * 100f), (double)(riverNode.end.x + riverNode.end.z + num2 * 100f));
			float num4 = -4f * Mathf.Pow(num2, 2f) + 4f * num2;
			if (riverNode == this.nodeA)
			{
				num3 += this.riverIslandAmplitude * num4;
			}
			else
			{
				num3 -= this.riverIslandAmplitude * num4;
			}
			Vector3 vector2 = a + num3 * this.GetCurveAmplitude * normalized * d;
			return new Vector2(vector2.x, vector2.z);
		}

		// Token: 0x0600B8EC RID: 47340 RVA: 0x003563BC File Offset: 0x003545BC
		protected override float GetRiverWidthAt(RiverNode riverNode, Vector2 cell)
		{
			float tvalue = base.GetTValue(riverNode, cell);
			float num = (riverNode == this.nodeA) ? this.widthFrac : (1f - this.widthFrac);
			float num2 = Mathf.InverseLerp(this.riverIslandCentrality, 1f - this.riverIslandCentrality, tvalue);
			float num3 = -4f * Mathf.Pow(num2, 2f) + 4f * num2;
			float num4 = 1f - num * num3;
			return Mathf.Max(riverNode.width / 2f * num4 * (1f + this.riverWidthNoise.GetValue(cell) * 0.15f), 1.5f);
		}

		// Token: 0x04007F7F RID: 32639
		private const float MinWidth = 1.5f;

		// Token: 0x04007F80 RID: 32640
		private const float RiverCurveAmplitude = 20f;

		// Token: 0x04007F81 RID: 32641
		private static readonly FloatRange RiverIslandAmplitudeRange = new FloatRange(1f, 2f);

		// Token: 0x04007F82 RID: 32642
		private static readonly FloatRange RiverIslandCentralityRange = new FloatRange(0f, 0.3f);

		// Token: 0x04007F83 RID: 32643
		private static readonly FloatRange RiverWidthFracRange = new FloatRange(0.2f, 0.8f);

		// Token: 0x04007F84 RID: 32644
		private RiverNode nodeA;

		// Token: 0x04007F85 RID: 32645
		private RiverNode nodeB;

		// Token: 0x04007F86 RID: 32646
		private float widthFrac;

		// Token: 0x04007F87 RID: 32647
		private float riverIslandAmplitude;

		// Token: 0x04007F88 RID: 32648
		private float riverIslandCentrality;
	}
}
