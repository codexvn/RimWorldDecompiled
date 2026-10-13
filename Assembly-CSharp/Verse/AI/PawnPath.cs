using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000FB9 RID: 4025
	public class PawnPath : IDisposable
	{
		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060062E2 RID: 25314 RVA: 0x001F2AB6 File Offset: 0x001F0CB6
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060062E3 RID: 25315 RVA: 0x001F2AC8 File Offset: 0x001F0CC8
		public bool Finished
		{
			get
			{
				return this.NodesLeftCount <= 0;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060062E4 RID: 25316 RVA: 0x001F2AD6 File Offset: 0x001F0CD6
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060062E5 RID: 25317 RVA: 0x001F2ADE File Offset: 0x001F0CDE
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060062E6 RID: 25318 RVA: 0x001F2AE8 File Offset: 0x001F0CE8
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060062E7 RID: 25319 RVA: 0x001F2AFC File Offset: 0x001F0CFC
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060062E8 RID: 25320 RVA: 0x001F2B04 File Offset: 0x001F0D04
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060062E9 RID: 25321 RVA: 0x001F2B0C File Offset: 0x001F0D0C
		public IntVec3 FirstNode
		{
			get
			{
				List<IntVec3> list = this.nodes;
				return list[list.Count - 1];
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060062EA RID: 25322 RVA: 0x001F2B21 File Offset: 0x001F0D21
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060062EB RID: 25323 RVA: 0x001F2B2F File Offset: 0x001F0D2F
		public static PawnPath NotFound
		{
			get
			{
				return new PawnPath
				{
					totalCostInt = -1f
				};
			}
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x001F2B44 File Offset: 0x001F0D44
		internal void Initialize(NativeList<IntVec3> points, int cost)
		{
			if (!this.Found)
			{
				Log.Warning("Calling initialize on invalid path");
				return;
			}
			this.inUse = true;
			for (int i = 0; i < points.Length; i++)
			{
				this.nodes.Add(points[i]);
			}
			this.curNodeIndex = this.nodes.Count - 1;
			this.totalCostInt = (float)cost;
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x001F2BAB File Offset: 0x001F0DAB
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x001F2BB9 File Offset: 0x001F0DB9
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x001F2BC4 File Offset: 0x001F0DC4
		public void ReleaseToPool()
		{
			if (!this.Found)
			{
				return;
			}
			this.totalCostInt = 0f;
			this.usedRegionHeuristics = false;
			this.nodes.Clear();
			this.inUse = false;
			if (this.pool != null)
			{
				this.pool.ReturnPath(this);
			}
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x001F2C12 File Offset: 0x001F0E12
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x001F2C29 File Offset: 0x001F0E29
		public bool BacktrackNode()
		{
			if (this.curNodeIndex >= this.nodes.Count - 1)
			{
				return false;
			}
			this.curNodeIndex++;
			return true;
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x001F2C51 File Offset: 0x001F0E51
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x001F2C68 File Offset: 0x001F0E68
		public override string ToString()
		{
			if (!this.Found)
			{
				return "PawnPath(not found)";
			}
			if (!this.inUse)
			{
				return "PawnPath(not in use)";
			}
			return string.Concat(new string[]
			{
				"PawnPath(nodeCount= ",
				this.nodes.Count.ToString(),
				(this.nodes.Count > 0) ? string.Format(" first={0} last={1}", this.FirstNode, this.LastNode) : "",
				" cost=",
				this.totalCostInt.ToString(),
				" )"
			});
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x001F2D10 File Offset: 0x001F0F10
		public void DrawPath(Pawn pathingPawn)
		{
			if (!this.Found)
			{
				return;
			}
			float y = AltitudeLayer.Item.AltitudeFor();
			if (this.NodesLeftCount > 0)
			{
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = this.Peek(i).ToVector3Shifted();
					a.y = y;
					Vector3 b = this.Peek(i + 1).ToVector3Shifted();
					b.y = y;
					GenDraw.DrawLineBetween(a, b);
				}
				if (pathingPawn != null)
				{
					Vector3 drawPos = pathingPawn.DrawPos;
					drawPos.y = y;
					Vector3 b2 = this.Peek(0).ToVector3Shifted();
					b2.y = y;
					if ((drawPos - b2).sqrMagnitude > 0.01f)
					{
						GenDraw.DrawLineBetween(drawPos, b2);
					}
				}
			}
		}

		// Token: 0x060062F5 RID: 25333 RVA: 0x001F2DD7 File Offset: 0x001F0FD7
		public List<IntVec3> PeekNextCells(int count, int curWaypointIndexOffset = 0)
		{
			this.PeekNextCells(count, PawnPath.tmpPeekCells, curWaypointIndexOffset);
			return PawnPath.tmpPeekCells;
		}

		// Token: 0x060062F6 RID: 25334 RVA: 0x001F2DEC File Offset: 0x001F0FEC
		public void PeekNextCells(int count, List<IntVec3> outCells, int offset = 1)
		{
			outCells.Clear();
			if (this.Finished)
			{
				return;
			}
			int num = Math.Max(this.curNodeIndex - offset - count, 0);
			for (int i = this.curNodeIndex - offset; i >= num; i--)
			{
				outCells.Add(this.nodes[i]);
			}
		}

		// Token: 0x040046A3 RID: 18083
		private readonly List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x040046A4 RID: 18084
		private float totalCostInt;

		// Token: 0x040046A5 RID: 18085
		private int curNodeIndex;

		// Token: 0x040046A6 RID: 18086
		private bool usedRegionHeuristics;

		// Token: 0x040046A7 RID: 18087
		private bool inUse;

		// Token: 0x040046A8 RID: 18088
		internal PawnPathPool pool;

		// Token: 0x040046A9 RID: 18089
		private static readonly List<IntVec3> tmpPeekCells = new List<IntVec3>();
	}
}
