using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002117 RID: 8471
	public class TileMutatorWorker_RiverDelta : TileMutatorWorker_River
	{
		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x0600B8D6 RID: 47318 RVA: 0x003558E6 File Offset: 0x00353AE6
		private static FloatRange SegmentLengthRange
		{
			get
			{
				return new FloatRange(30f, 70f);
			}
		}

		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x0600B8D7 RID: 47319 RVA: 0x003558F7 File Offset: 0x00353AF7
		private static FloatRange SplitAngleRange
		{
			get
			{
				return new FloatRange(15f, 30f);
			}
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x0600B8D8 RID: 47320 RVA: 0x00355908 File Offset: 0x00353B08
		private static FloatRange InitialLengthRange
		{
			get
			{
				return new FloatRange(0.2f, 0.5f);
			}
		}

		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x0600B8D9 RID: 47321 RVA: 0x0034EE1A File Offset: 0x0034D01A
		protected override float GetCurveFrequency
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x0600B8DA RID: 47322 RVA: 0x0019D0E8 File Offset: 0x0019B2E8
		protected override float GetCurveAmplitude
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x0600B8DB RID: 47323 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected override float GetWidthNoiseFactor(RiverNode riverNode)
		{
			return 1f;
		}

		// Token: 0x0600B8DC RID: 47324 RVA: 0x003550D5 File Offset: 0x003532D5
		public TileMutatorWorker_RiverDelta(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B8DD RID: 47325 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B8DE RID: 47326 RVA: 0x0035591C File Offset: 0x00353B1C
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
			SurfaceTile.RiverLink riverLink = (from rl in surfaceTile.Rivers
			where !rl.neighbor.Tile.WaterCovered
			select rl).MaxBy((SurfaceTile.RiverLink rl) => rl.river.widthOnMap);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = (((num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake)).Value + 180f) % 360f;
			float angle = (450f - num2) % 360f;
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, angle);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			RiverNode riverNode;
			if (base.IsFlowingAToB(item, item2, angle))
			{
				riverNode = new RiverNode
				{
					start = item,
					end = item + (item2 - item) * TileMutatorWorker_RiverDelta.InitialLengthRange.RandomInRange,
					width = riverLink.river.widthOnMap
				};
			}
			else
			{
				riverNode = new RiverNode
				{
					start = item2,
					end = item2 + (item - item2) * TileMutatorWorker_RiverDelta.InitialLengthRange.RandomInRange,
					width = riverLink.river.widthOnMap
				};
			}
			map.waterInfo.riverGraph.Add(riverNode);
			this.SplitRiver(map, riverNode, num2);
			this.MergeNearbyNodes(map, riverNode);
		}

		// Token: 0x0600B8DF RID: 47327 RVA: 0x00355ACC File Offset: 0x00353CCC
		private void SplitRiver(Map map, RiverNode prevSegment, float baseAngle)
		{
			if (!prevSegment.end.InBounds(map) || map.terrainGrid.TerrainAt(prevSegment.end.ToIntVec3()).IsOcean)
			{
				return;
			}
			Vector3 normalized = (prevSegment.end - prevSegment.start).normalized;
			Vector3 a = new Vector3(-normalized.z, 0f, normalized.x);
			float num = 5f * (prevSegment.width / 10f);
			if (!Rand.Chance(0.9f))
			{
				float f = (baseAngle + TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange - TileMutatorWorker_RiverDelta.SplitAngleRange.Average) * 0.017453292f;
				float randomInRange = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
				float width = prevSegment.width;
				Vector3 start = prevSegment.end - normalized * width / 4f;
				Vector3 end = new Vector3(prevSegment.end.x + randomInRange * Mathf.Cos(f), 0f, prevSegment.end.z + randomInRange * Mathf.Sin(f));
				RiverNode riverNode = new RiverNode
				{
					start = start,
					end = end,
					width = width
				};
				map.waterInfo.riverGraph.Add(riverNode);
				prevSegment.childNodes.Add(riverNode);
				this.SplitRiver(map, riverNode, baseAngle);
				return;
			}
			float f2 = (baseAngle - TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange - num) * 0.017453292f;
			float randomInRange2 = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
			float num2 = Mathf.Max(prevSegment.width * Rand.Range(0.33f, 0.67f), 1.5f);
			Vector3 start2 = prevSegment.end - normalized * num2 / 4f - a * (prevSegment.width / 2f - num2 / 2f);
			Vector3 end2 = new Vector3(prevSegment.end.x + randomInRange2 * Mathf.Cos(f2), 0f, prevSegment.end.z + randomInRange2 * Mathf.Sin(f2));
			RiverNode riverNode2 = new RiverNode
			{
				start = start2,
				end = end2,
				width = num2 * 1.1f
			};
			map.waterInfo.riverGraph.Add(riverNode2);
			prevSegment.childNodes.Add(riverNode2);
			this.SplitRiver(map, riverNode2, baseAngle);
			f2 = (baseAngle + TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange + num) * 0.017453292f;
			randomInRange2 = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
			num2 = Mathf.Max(prevSegment.width - num2, 1.5f) + 1f;
			Vector3 start3 = prevSegment.end - normalized * num2 / 4f + a * (prevSegment.width / 2f - num2 / 2f);
			Vector3 end3 = new Vector3(prevSegment.end.x + randomInRange2 * Mathf.Cos(f2), 0f, prevSegment.end.z + randomInRange2 * Mathf.Sin(f2));
			RiverNode riverNode3 = new RiverNode
			{
				start = start3,
				end = end3,
				width = num2 * 1.1f
			};
			map.waterInfo.riverGraph.Add(riverNode3);
			prevSegment.childNodes.Add(riverNode3);
			this.SplitRiver(map, riverNode3, baseAngle);
		}

		// Token: 0x0600B8E0 RID: 47328 RVA: 0x00355E54 File Offset: 0x00354054
		private void MergeNearbyNodes(Map map, RiverNode root)
		{
			TileMutatorWorker_RiverDelta.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.markedForRemoval = new HashSet<RiverNode>();
			List<Vector3> list = new List<Vector3>();
			Queue<RiverNode> queue = new Queue<RiverNode>();
			queue.Enqueue(root);
			while (!queue.Empty<RiverNode>())
			{
				RiverNode riverNode = queue.Dequeue();
				bool flag = false;
				foreach (Vector3 vector in list)
				{
					if ((riverNode.end - vector).magnitude < 25f)
					{
						riverNode.end = vector;
						foreach (RiverNode node in riverNode.childNodes)
						{
							TileMutatorWorker_RiverDelta.<MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(node, ref CS$<>8__locals1);
						}
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(riverNode.end);
					foreach (RiverNode item in riverNode.childNodes.InRandomOrder(null))
					{
						queue.Enqueue(item);
					}
				}
			}
			foreach (RiverNode item2 in CS$<>8__locals1.markedForRemoval)
			{
				map.waterInfo.riverGraph.Remove(item2);
			}
		}

		// Token: 0x0600B8E1 RID: 47329 RVA: 0x00355FEC File Offset: 0x003541EC
		[CompilerGenerated]
		internal static void <MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(RiverNode node, ref TileMutatorWorker_RiverDelta.<>c__DisplayClass27_0 A_1)
		{
			A_1.markedForRemoval.Add(node);
			foreach (RiverNode node2 in node.childNodes)
			{
				TileMutatorWorker_RiverDelta.<MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(node2, ref A_1);
			}
		}

		// Token: 0x04007F6F RID: 32623
		private const float MinWidth = 1.5f;

		// Token: 0x04007F70 RID: 32624
		private const float DeltaCurveFrequency = 0.02f;

		// Token: 0x04007F71 RID: 32625
		private const float DeltaCurveAmplitude = 10f;

		// Token: 0x04007F72 RID: 32626
		private const float DeltaMergeDist = 25f;

		// Token: 0x04007F73 RID: 32627
		private const float DeltaSplitChance = 0.9f;

		// Token: 0x04007F74 RID: 32628
		private const float MinSegmentLength = 30f;

		// Token: 0x04007F75 RID: 32629
		private const float MaxSegmentLength = 70f;

		// Token: 0x04007F76 RID: 32630
		private const float MinSplitAngle = 15f;

		// Token: 0x04007F77 RID: 32631
		private const float MaxSplitAngle = 30f;

		// Token: 0x04007F78 RID: 32632
		private const float MinInitialLength = 0.2f;

		// Token: 0x04007F79 RID: 32633
		private const float MaxInitialLength = 0.5f;

		// Token: 0x04007F7A RID: 32634
		private const float ExtraWidthAngle = 5f;
	}
}
