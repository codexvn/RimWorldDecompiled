using System;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005C1 RID: 1473
	[BurstCompile(CompileSynchronously = true)]
	public struct PathFinderJob : IJob
	{
		// Token: 0x060025E8 RID: 9704 RVA: 0x000C7460 File Offset: 0x000C5660
		[BurstCompile]
		public void Execute()
		{
			this.calcGrid.Clear<PathFinderJob.CalcNode>();
			this.frontier.Clear();
			this.path.Clear();
			this.result.Value = default(PathFinderJob.ResultData);
			int num = this.indices.CellToIndex(this.destCell);
			bool flag = this.destRect.Area == 1;
			int num2 = 0;
			this.InitalizeSearch();
			while (this.frontier.Count > 0)
			{
				int num3;
				float a;
				this.frontier.Dequeue(out num3, out a);
				if (Mathf.Approximately(a, this.calcGrid[num3].fCost) && this.calcGrid[num3].status != PathFinderJob.CalcNode.Status.Closed)
				{
					IntVec3 intVec = this.indices.IndexToCell(num3);
					if (flag)
					{
						if (num3 == num)
						{
							this.FinalizePath(num3);
							return;
						}
					}
					else if (this.destRect.Contains(intVec) && !this.excludedRectIndices.Contains(num3))
					{
						this.FinalizePath(num3);
						return;
					}
					if (num2 >= this.indices.NumGridCells)
					{
						return;
					}
					CellConnection connections = this.connectivity[num3];
					int num4 = connections.BitLoopEndIndex();
					for (int i = 0; i < num4; i++)
					{
						CellConnection cellConnection = CellConnectionExtensions.FlagFromBitIndex[i];
						if (connections.HasBit(cellConnection))
						{
							IntVec3 intVec2 = intVec + CellConnectionExtensions.OffsetFromBitIndex[i];
							int num5 = this.indices.CellToIndex(intVec2);
							if (this.calcGrid[num5].status != PathFinderJob.CalcNode.Status.Closed)
							{
								int num6 = this.IndexCost(num5);
								if (num6 < 10000)
								{
									if (cellConnection.Diagonal())
									{
										int num7 = this.indices.CellToIndex(new IntVec3(intVec2.x, 0, intVec.z));
										int num8 = this.indices.CellToIndex(new IntVec3(intVec.x, 0, intVec2.z));
										if (this.IndexCost(num7) >= 10000 || this.IndexCost(num8) >= 10000 || this.buildings.IsSet(num7) || (!this.traverseParams.canBashFences && this.fences.IsSet(num7)) || this.buildings.IsSet(num8) || (!this.traverseParams.canBashFences && this.fences.IsSet(num8)))
										{
											goto IL_3F7;
										}
									}
									if (cellConnection.Diagonal())
									{
										num6 += this.moveTicksDiagonal;
									}
									else
									{
										num6 += this.moveTicksCardinal;
									}
									int num9 = num6 + this.calcGrid[num3].gCost;
									PathFinderJob.CalcNode.Status status = this.calcGrid[num5].status;
									if (status != PathFinderJob.CalcNode.Status.None)
									{
										int num10 = 0;
										if (status == PathFinderJob.CalcNode.Status.Closed)
										{
											num10 = Mathf.CeilToInt((float)this.moveTicksCardinal * 0.8f);
										}
										if (this.calcGrid[num5].gCost <= num9 + num10)
										{
											goto IL_3F7;
										}
									}
									if (status == PathFinderJob.CalcNode.Status.None)
									{
										IntVec3 intVec3 = this.destCell - intVec2;
										if (intVec3.x != 0 || intVec3.z != 0)
										{
											int num11 = math.abs(intVec3.x);
											int num12 = math.abs(intVec3.z);
											float num13 = 1f * (float)(num11 + num12) + -0.58579004f * (float)math.min(num11, num12);
											PathFinderJob.CalcNode value = this.calcGrid[num5];
											value.hCost = num13 * 13f * this.heuristicStrength;
											this.calcGrid[num5] = value;
										}
									}
									float num14 = (float)num9 + this.calcGrid[num5].hCost;
									if (num14 < 0f)
									{
										num14 = 0f;
									}
									PathFinderJob.CalcNode value2 = this.calcGrid[num5];
									value2.gCost = num9;
									value2.fCost = num14;
									value2.parentIndex = num3;
									value2.status = PathFinderJob.CalcNode.Status.Open;
									this.calcGrid[num5] = value2;
									this.frontier.Enqueue(num5, num14);
								}
							}
						}
						IL_3F7:;
					}
					num2++;
					PathFinderJob.CalcNode value3 = this.calcGrid[num3];
					value3.status = PathFinderJob.CalcNode.Status.Closed;
					this.calcGrid[num3] = value3;
				}
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000C78A4 File Offset: 0x000C5AA4
		private int IndexCost(int index)
		{
			if (this.providerCost[index] == 65535)
			{
				return 10000;
			}
			return math.max(this.grid[index], (int)this.providerCost[index]) + (this.blocked[index] ? 175 : 0);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000C7900 File Offset: 0x000C5B00
		private void InitalizeSearch()
		{
			int num = this.indices.CellToIndex(this.start);
			PathFinderJob.CalcNode value = this.calcGrid[num];
			value.parentIndex = num;
			value.status = PathFinderJob.CalcNode.Status.Open;
			this.calcGrid[num] = value;
			this.frontier.Enqueue(num, 0f);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000C795C File Offset: 0x000C5B5C
		private void FinalizePath(int finalIndex)
		{
			int num = finalIndex;
			for (;;)
			{
				IntVec3 intVec = this.indices.IndexToCell(num);
				this.path.Add(intVec);
				int parentIndex = this.calcGrid[num].parentIndex;
				if (num == parentIndex)
				{
					break;
				}
				num = parentIndex;
			}
			PathFinderJob.ResultData value;
			value.pathCost = this.calcGrid[finalIndex].gCost;
			this.result.Value = value;
		}

		// Token: 0x04001F36 RID: 7990
		private const int Cost_Blocked = 175;

		// Token: 0x04001F37 RID: 7991
		public CellIndices indices;

		// Token: 0x04001F38 RID: 7992
		public float heuristicStrength;

		// Token: 0x04001F39 RID: 7993
		public int moveTicksCardinal;

		// Token: 0x04001F3A RID: 7994
		public int moveTicksDiagonal;

		// Token: 0x04001F3B RID: 7995
		public IntVec3 start;

		// Token: 0x04001F3C RID: 7996
		public IntVec3 destCell;

		// Token: 0x04001F3D RID: 7997
		public CellRect destRect;

		// Token: 0x04001F3E RID: 7998
		public PathFinder.UnmanagedGridTraverseParams traverseParams;

		// Token: 0x04001F3F RID: 7999
		public NativeArray<PathFinderJob.CalcNode> calcGrid;

		// Token: 0x04001F40 RID: 8000
		public NativePriorityQueue<int, float, FloatMinComparer> frontier;

		// Token: 0x04001F41 RID: 8001
		public NativeList<IntVec3> path;

		// Token: 0x04001F42 RID: 8002
		public NativeReference<PathFinderJob.ResultData> result;

		// Token: 0x04001F43 RID: 8003
		[ReadOnly]
		public NativeArray<int>.ReadOnly grid;

		// Token: 0x04001F44 RID: 8004
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly providerCost;

		// Token: 0x04001F45 RID: 8005
		[ReadOnly]
		public NativeArray<bool>.ReadOnly blocked;

		// Token: 0x04001F46 RID: 8006
		[ReadOnly]
		public NativeList<int> excludedRectIndices;

		// Token: 0x04001F47 RID: 8007
		[ReadOnly]
		public NativeArray<CellConnection>.ReadOnly connectivity;

		// Token: 0x04001F48 RID: 8008
		[ReadOnly]
		public NativeBitArray.ReadOnly fences;

		// Token: 0x04001F49 RID: 8009
		[ReadOnly]
		public NativeBitArray.ReadOnly buildings;

		// Token: 0x04001F4A RID: 8010
		private bool passAllDestroyableThings;

		// Token: 0x04001F4B RID: 8011
		private bool passWater;

		// Token: 0x020005C2 RID: 1474
		public struct CalcNode
		{
			// Token: 0x04001F4C RID: 8012
			public int gCost;

			// Token: 0x04001F4D RID: 8013
			public float hCost;

			// Token: 0x04001F4E RID: 8014
			public float fCost;

			// Token: 0x04001F4F RID: 8015
			public int parentIndex;

			// Token: 0x04001F50 RID: 8016
			public PathFinderJob.CalcNode.Status status;

			// Token: 0x020005C3 RID: 1475
			public enum Status : byte
			{
				// Token: 0x04001F52 RID: 8018
				None,
				// Token: 0x04001F53 RID: 8019
				Open,
				// Token: 0x04001F54 RID: 8020
				Closed
			}
		}

		// Token: 0x020005C4 RID: 1476
		public struct ResultData
		{
			// Token: 0x04001F55 RID: 8021
			public int pathCost;
		}
	}
}
