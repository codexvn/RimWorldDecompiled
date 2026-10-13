using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C14 RID: 15380
	public class WorldPath : IDisposable
	{
		// Token: 0x17003397 RID: 13207
		// (get) Token: 0x06014DB8 RID: 85432 RVA: 0x0063EE6C File Offset: 0x0063D06C
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x17003398 RID: 13208
		// (get) Token: 0x06014DB9 RID: 85433 RVA: 0x0063EE7E File Offset: 0x0063D07E
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x17003399 RID: 13209
		// (get) Token: 0x06014DBA RID: 85434 RVA: 0x0063EE86 File Offset: 0x0063D086
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x1700339A RID: 13210
		// (get) Token: 0x06014DBB RID: 85435 RVA: 0x0063EE90 File Offset: 0x0063D090
		public List<PlanetTile> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700339B RID: 13211
		// (get) Token: 0x06014DBC RID: 85436 RVA: 0x0063EE98 File Offset: 0x0063D098
		public PlanetTile FirstNode
		{
			get
			{
				List<PlanetTile> list = this.nodes;
				return list[list.Count - 1];
			}
		}

		// Token: 0x1700339C RID: 13212
		// (get) Token: 0x06014DBD RID: 85437 RVA: 0x0063EEAD File Offset: 0x0063D0AD
		public PlanetTile LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x1700339D RID: 13213
		// (get) Token: 0x06014DBE RID: 85438 RVA: 0x0063EEBB File Offset: 0x0063D0BB
		public PlanetLayer Layer
		{
			get
			{
				return this.layer;
			}
		}

		// Token: 0x1700339E RID: 13214
		// (get) Token: 0x06014DBF RID: 85439 RVA: 0x0063EEC3 File Offset: 0x0063D0C3
		public int NodeCount
		{
			get
			{
				return this.nodes.Count;
			}
		}

		// Token: 0x1700339F RID: 13215
		// (get) Token: 0x06014DC0 RID: 85440 RVA: 0x0063EED0 File Offset: 0x0063D0D0
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x06014DC1 RID: 85441 RVA: 0x0063EED7 File Offset: 0x0063D0D7
		public void AddNodeAtStart(PlanetTile tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x06014DC2 RID: 85442 RVA: 0x0063EEE8 File Offset: 0x0063D0E8
		public void SetupFound(float totalCost, PlanetLayer planetLayer)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning(string.Format("Calling SetupFound with totalCost={0} on WorldPath.NotFound", totalCost));
				return;
			}
			this.layer = planetLayer;
			this.totalCostInt = totalCost;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x06014DC3 RID: 85443 RVA: 0x0063EF34 File Offset: 0x0063D134
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06014DC4 RID: 85444 RVA: 0x0063EF3C File Offset: 0x0063D13C
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
				this.layer = null;
			}
		}

		// Token: 0x06014DC5 RID: 85445 RVA: 0x0063EF6A File Offset: 0x0063D16A
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06014DC6 RID: 85446 RVA: 0x0063EF7C File Offset: 0x0063D17C
		public PlanetTile ConsumeNextNode()
		{
			PlanetTile result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06014DC7 RID: 85447 RVA: 0x0063EF93 File Offset: 0x0063D193
		public PlanetTile Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06014DC8 RID: 85448 RVA: 0x0063EFA8 File Offset: 0x0063D1A8
		public override string ToString()
		{
			if (!this.Found)
			{
				return "WorldPath(not found)";
			}
			if (!this.inUse)
			{
				return "WorldPath(not in use)";
			}
			return string.Concat(new string[]
			{
				"WorldPath(nodeCount= ",
				this.nodes.Count.ToString(),
				(this.nodes.Count > 0) ? (" first=" + this.FirstNode.ToString() + " last=" + this.LastNode.ToString()) : "",
				" cost=",
				this.totalCostInt.ToString(),
				" )"
			});
		}

		// Token: 0x06014DC9 RID: 85449 RVA: 0x0063F068 File Offset: 0x0063D268
		public void DrawPath(Caravan pathingCaravan)
		{
			if (!this.Found)
			{
				return;
			}
			if (this.NodesLeftCount > 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				float d = 0.08f;
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = worldGrid.GetTileCenter(this.Peek(i));
					Vector3 vector = worldGrid.GetTileCenter(this.Peek(i + 1));
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector, 1f);
				}
				if (pathingCaravan != null)
				{
					Vector3 a2 = pathingCaravan.DrawPos;
					Vector3 vector2 = worldGrid.GetTileCenter(this.Peek(0));
					a2 += a2.normalized * d;
					vector2 += vector2.normalized * d;
					if ((a2 - vector2).sqrMagnitude > 0.005f)
					{
						GenDraw.DrawWorldLineBetween(a2, vector2, 1f);
					}
				}
			}
		}

		// Token: 0x0400E881 RID: 59521
		private readonly List<PlanetTile> nodes = new List<PlanetTile>(128);

		// Token: 0x0400E882 RID: 59522
		private float totalCostInt;

		// Token: 0x0400E883 RID: 59523
		private int curNodeIndex;

		// Token: 0x0400E884 RID: 59524
		private PlanetLayer layer;

		// Token: 0x0400E885 RID: 59525
		public bool inUse;
	}
}
