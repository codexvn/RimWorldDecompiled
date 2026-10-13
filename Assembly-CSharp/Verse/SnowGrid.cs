using System;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000438 RID: 1080
	public sealed class SnowGrid : IExposable, IDisposable
	{
		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x0008B4B0 File Offset: 0x000896B0
		internal NativeArray<float> DepthGrid_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x0008B4B8 File Offset: 0x000896B8
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x0008B4C1 File Offset: 0x000896C1
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x0008B4E8 File Offset: 0x000896E8
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x0008B112 File Offset: 0x00089312
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x0008B136 File Offset: 0x00089336
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0008B514 File Offset: 0x00089714
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				return false;
			}
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
			return terrainDef == null || terrainDef.holdSnowOrSand;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0008B1A3 File Offset: 0x000893A3
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0008B568 File Offset: 0x00089768
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
			if (!this.CanHaveSnow(num))
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

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0008B62C File Offset: 0x0008982C
		public void SetDepth(IntVec3 c, float newDepth)
		{
			if (!this.depthGrid.IsCreated)
			{
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newDepth > 0f && !this.CanHaveSnow(num))
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

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0008B6BC File Offset: 0x000898BC
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (Mathf.Approximately(oldDepth, newDepth))
			{
				return;
			}
			if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things, true, false);
			}
			else if (newDepth == 0f)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
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

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0008B7AF File Offset: 0x000899AF
		public void MakeMeshDirty(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Snow, true, false);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0008B7CE File Offset: 0x000899CE
		public float GetDepth(IntVec3 c)
		{
			if (!this.depthGrid.IsCreated || !c.InBounds(this.map))
			{
				return 0f;
			}
			return this.depthGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0008B80D File Offset: 0x00089A0D
		public WeatherBuildupCategory GetCategory(IntVec3 c)
		{
			return WeatherBuildupUtility.GetBuildupCategory(this.GetDepth(c));
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0008B81B File Offset: 0x00089A1B
		public void Dispose()
		{
			this.depthGrid.Dispose();
		}

		// Token: 0x04001803 RID: 6147
		private Map map;

		// Token: 0x04001804 RID: 6148
		private NativeArray<float> depthGrid;

		// Token: 0x04001805 RID: 6149
		private double totalDepth;

		// Token: 0x04001806 RID: 6150
		public const float MaxDepth = 1f;
	}
}
