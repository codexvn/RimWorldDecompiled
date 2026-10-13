using System;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000437 RID: 1079
	public sealed class SandGrid : IExposable, IDisposable
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x0008B0A3 File Offset: 0x000892A3
		internal NativeArray<float> DepthGrid_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x0008B0AB File Offset: 0x000892AB
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0008B0B4 File Offset: 0x000892B4
		public SandGrid(Map map)
		{
			if (!ModLister.CheckOdyssey("sand"))
			{
				return;
			}
			this.map = map;
			this.depthGrid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0008B0E8 File Offset: 0x000892E8
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SandGrid.SandFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SandGrid.SandShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0008B112 File Offset: 0x00089312
		private static ushort SandFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0008B136 File Offset: 0x00089336
		private static float SandShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0008B140 File Offset: 0x00089340
		private bool CanHaveSand(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SandGrid.CanCoexistWithSand(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || (terrainDef.holdSnowOrSand && terrainDef != TerrainDefOf.Sand && terrainDef != TerrainDefOf.SoftSand);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0008B1A3 File Offset: 0x000893A3
		public static bool CanCoexistWithSand(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0008B1BC File Offset: 0x000893BC
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 <= 0f && depthToAdd < 0f)
			{
				return;
			}
			if (num2 >= 0.999f && depthToAdd > 1f)
			{
				return;
			}
			if (!this.CanHaveSand(num))
			{
				this.depthGrid[num] = 0f;
				return;
			}
			float num3 = num2 + depthToAdd;
			num3 = Mathf.Clamp(num3, 0f, 1f);
			float num4 = num3 - num2;
			this.totalDepth += (double)num4;
			if (Mathf.Abs(num4) > 0.0001f)
			{
				this.depthGrid[num] = num3;
				this.CheckVisualOrPathCostChange(c, num2, num3);
			}
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0008B280 File Offset: 0x00089480
		public void SetDepth(IntVec3 c, float newDepth)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newDepth > 0f && !this.CanHaveSand(num))
			{
				newDepth = 0f;
			}
			newDepth = Mathf.Clamp(newDepth, 0f, 1f);
			float num2 = this.depthGrid[num];
			this.depthGrid[num] = newDepth;
			float num3 = newDepth - num2;
			this.totalDepth += (double)num3;
			this.CheckVisualOrPathCostChange(c, num2, newDepth);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0008B310 File Offset: 0x00089510
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
				}
				float num = 0.4f;
				if (c.IsPolluted(this.map) && ((oldDepth > num && newDepth < num) || (oldDepth < num && newDepth > num)))
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Terrain, true, false);
				}
				if (WeatherBuildupUtility.GetBuildupCategory(oldDepth) != WeatherBuildupUtility.GetBuildupCategory(newDepth))
				{
					this.map.pathing.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0008B405 File Offset: 0x00089605
		public void MakeMeshDirty(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Sand, true, false);
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0008B424 File Offset: 0x00089624
		public float GetDepth(IntVec3 c)
		{
			if (!this.depthGrid.IsCreated || !c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0008B463 File Offset: 0x00089663
		public WeatherBuildupCategory GetCategory(IntVec3 c)
		{
			return WeatherBuildupUtility.GetBuildupCategory(this.GetDepth(c));
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0008B471 File Offset: 0x00089671
		public void Dispose()
		{
			this.depthGrid.Dispose();
		}

		// Token: 0x040017FF RID: 6143
		private Map map;

		// Token: 0x04001800 RID: 6144
		private NativeArray<float> depthGrid;

		// Token: 0x04001801 RID: 6145
		private double totalDepth;

		// Token: 0x04001802 RID: 6146
		public const float MaxDepth = 1f;
	}
}
