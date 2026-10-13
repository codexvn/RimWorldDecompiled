using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using Verse.Glow;

namespace Verse
{
	// Token: 0x0200041B RID: 1051
	[BurstCompile(CompileSynchronously = true)]
	public sealed class GlowGrid : IDisposable
	{
		// Token: 0x06001A8B RID: 6795 RVA: 0x0008663C File Offset: 0x0008483C
		public GlowGrid(Map map)
		{
			this.map = map;
			this.indices = map.cellIndices;
			this.lights = new NativeList<GlowLight>(4096, Allocator.Persistent);
			this.lightBlockers = new NativeBitArray(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.dirtyCells = new UnsafeBitArray(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.accumulatedGlow = new NativeArray<Color32>(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.accumulatedGlowNoCavePlants = new NativeArray<Color32>(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.glowPool = new GlowGrid.GlowPool();
			GlowGrid.FloodFillerPool.EnsureAllocated();
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0008670F File Offset: 0x0008490F
		private Color32 GetAccumulatedGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			return this.GetAccumulatedGlowAt(this.map.cellIndices.CellToIndex(c), ignoreCavePlants);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0008672C File Offset: 0x0008492C
		private Color32 GetAccumulatedGlowAt(int index, bool ignoreCavePlants = false)
		{
			if (!this.accumulatedGlow.IsCreated)
			{
				return default(Color32);
			}
			if (this.map.terrainGrid.TerrainAt(index).exposesToVacuum)
			{
				return default(Color32);
			}
			return (ignoreCavePlants ? this.accumulatedGlowNoCavePlants : this.accumulatedGlow)[index];
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0008678C File Offset: 0x0008498C
		public Color32 VisualGlowAt(int index)
		{
			return this.GetAccumulatedGlowAt(index, false);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00086796 File Offset: 0x00084996
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.GetAccumulatedGlowAt(c, false);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000867A0 File Offset: 0x000849A0
		public float GroundGlowAt(IntVec3 c, bool ignoreCavePlants = false, bool ignoreSky = false)
		{
			float num = 0f;
			if (!ignoreSky && !this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num >= 1f)
				{
					return num;
				}
			}
			Color32 accumulatedGlowAt = this.GetAccumulatedGlowAt(c, ignoreCavePlants);
			if (accumulatedGlowAt.a == 1)
			{
				return 1f;
			}
			float b = (float)Mathf.Max(Mathf.Max((int)accumulatedGlowAt.r, (int)accumulatedGlowAt.g), (int)accumulatedGlowAt.b) / 255f * 3.6f;
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00086839 File Offset: 0x00084A39
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GroundGlowAt(c, false, false));
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00086849 File Offset: 0x00084A49
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.9f)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.3f)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00086860 File Offset: 0x00084A60
		public void RegisterGlower(CompGlower newGlow)
		{
			if (!this.litGlowers.Add(newGlow))
			{
				return;
			}
			GlowLight glowLight = new GlowLight(newGlow, this.glowPool.Take());
			this.lights.Add(glowLight);
			this.anyDirtyLight = true;
			glowLight = this.lights[this.lights.Length - 1];
			foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell);
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00086910 File Offset: 0x00084B10
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			if (!this.litGlowers.Remove(oldGlow))
			{
				return;
			}
			if (!this.lights.IsCreated)
			{
				return;
			}
			int glowerIndex = this.GetGlowerIndex(oldGlow);
			GlowLight glowLight = this.lights[glowerIndex];
			foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell);
			}
			this.glowPool.ReturnSet(glowLight.localGlowPoolIndex);
			this.lights.RemoveAt(glowerIndex);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000869C8 File Offset: 0x00084BC8
		[BurstCompile]
		private int GetGlowerIndex(CompGlower glower)
		{
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (!glowLight.isTerrain && glowLight.id == glower.parent.thingIDNumber)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00086A18 File Offset: 0x00084C18
		public void RegisterTerrain(IntVec3 cell)
		{
			if (!this.litTerrain.Add(cell))
			{
				return;
			}
			GlowLight glowLight = new GlowLight(cell, this.map, this.glowPool.Take());
			this.lights.Add(glowLight);
			this.anyDirtyLight = true;
			glowLight = this.lights[this.lights.Length - 1];
			foreach (IntVec3 cell2 in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell2);
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00086ACC File Offset: 0x00084CCC
		public void DeregisterTerrain(IntVec3 cell)
		{
			if (!this.litTerrain.Remove(cell))
			{
				return;
			}
			if (!this.lights.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (glowLight.isTerrain && !(glowLight.position != cell))
				{
					foreach (IntVec3 cell2 in glowLight.AffectedRect.ClipInsideMap(this.map))
					{
						this.DirtyCell(cell2);
					}
					this.glowPool.ReturnSet(glowLight.localGlowPoolIndex);
					this.lights.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00086BB0 File Offset: 0x00084DB0
		public void GlowGridUpdate_First()
		{
			if (!this.accumulatedGlow.IsCreated || !this.accumulatedGlowNoCavePlants.IsCreated)
			{
				return;
			}
			try
			{
				if (this.anyDirtyLight)
				{
					using (new ProfilerBlock("ComputeGlowGridsJob()"))
					{
						new ComputeGlowGridsJob
						{
							lights = this.lights,
							indices = this.indices,
							lightBlockers = this.lightBlockers,
							statePool = GlowGrid.FloodFillerPool.pool,
							glowPool = this.glowPool.pool,
							mapSize = this.map.Size
						}.Schedule(this.lights.Length, UnityData.GetIdealBatchCount(this.lights.Length), default(JobHandle)).Complete();
						this.anyDirtyLight = false;
					}
				}
				if (this.anyDirtyCell)
				{
					using (new ProfilerBlock("CombineColorsJob()"))
					{
						new GlowGrid.CombineColorsJob
						{
							lights = this.lights,
							dirtyCells = this.dirtyCells,
							indices = this.indices,
							glow = this.accumulatedGlow,
							glowNoCavePlants = this.accumulatedGlowNoCavePlants,
							glowPool = this.glowPool.pool,
							allDirty = !this.hasRunInitially
						}.Schedule(this.indices.NumGridCells, UnityData.GetIdealBatchCount(this.indices.NumGridCells), default(JobHandle)).Complete();
						this.anyDirtyCell = false;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw;
			}
			if (!this.hasRunInitially)
			{
				this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.GroundGlow);
			}
			this.hasRunInitially = true;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00086DE4 File Offset: 0x00084FE4
		public void DirtyCell(IntVec3 cell)
		{
			if (!this.dirtyCells.IsCreated || !this.hasRunInitially)
			{
				return;
			}
			this.dirtyCells.Set(this.indices.CellToIndex(cell), true);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Roofs);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.GroundGlow);
			this.anyDirtyCell = true;
			this.map.events.Notify_GlowChanged(cell);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00086E6D File Offset: 0x0008506D
		public void LightBlockerAdded(IntVec3 cell)
		{
			this.lightBlockers.Set(this.indices.CellToIndex(cell), true);
			this.DirtyCell(cell);
			this.DirtyLightsAround(cell);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00086E95 File Offset: 0x00085095
		public void LightBlockerRemoved(IntVec3 cell)
		{
			if (!this.lightBlockers.IsCreated)
			{
				return;
			}
			this.lightBlockers.Set(this.indices.CellToIndex(cell), false);
			this.DirtyCell(cell);
			this.DirtyLightsAround(cell);
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00086ECC File Offset: 0x000850CC
		[BurstCompile]
		private void DirtyLightsAround(IntVec3 cell)
		{
			if (!this.hasRunInitially)
			{
				return;
			}
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (!glowLight.dirty && glowLight.AffectedRect.Contains(cell))
				{
					glowLight.dirty = true;
					this.lights[i] = glowLight;
					foreach (IntVec3 intVec in glowLight.AffectedRect)
					{
						if (intVec.InBounds(this.map) && this.indices.Contains(intVec))
						{
							this.DirtyCell(intVec);
						}
					}
					this.anyDirtyLight = true;
				}
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00086FB0 File Offset: 0x000851B0
		public void DevPrintLightIdsAffectingCell(IntVec3 cell)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("Lights affecting {0}\n", cell));
			for (int i = 0; i < this.lights.Length; i++)
			{
				if (this.lights[i].AffectedRect.Contains(cell))
				{
					stringBuilder.AppendLine(string.Format("   [{0}] - {1}", i, this.lights[i].ToString()));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x0008704C File Offset: 0x0008524C
		public void DevDirtyRect(CellRect rect)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				this.dirtyCells.Set(this.indices.CellToIndex(c), true);
			}
			this.anyDirtyCell = true;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000870B4 File Offset: 0x000852B4
		public void Dispose()
		{
			this.glowPool.Dispose();
			this.lights.Dispose();
			this.dirtyCells.Dispose();
			this.lightBlockers.Dispose();
			this.accumulatedGlow.Dispose();
			this.accumulatedGlowNoCavePlants.Dispose();
		}

		// Token: 0x0400176C RID: 5996
		private readonly Map map;

		// Token: 0x0400176D RID: 5997
		private readonly CellIndices indices;

		// Token: 0x0400176E RID: 5998
		private readonly GlowGrid.GlowPool glowPool;

		// Token: 0x0400176F RID: 5999
		private NativeList<GlowLight> lights;

		// Token: 0x04001770 RID: 6000
		private NativeBitArray lightBlockers;

		// Token: 0x04001771 RID: 6001
		private UnsafeBitArray dirtyCells;

		// Token: 0x04001772 RID: 6002
		private NativeArray<Color32> accumulatedGlow;

		// Token: 0x04001773 RID: 6003
		private NativeArray<Color32> accumulatedGlowNoCavePlants;

		// Token: 0x04001774 RID: 6004
		private readonly HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04001775 RID: 6005
		private readonly HashSet<IntVec3> litTerrain = new HashSet<IntVec3>();

		// Token: 0x04001776 RID: 6006
		private bool anyDirtyCell = true;

		// Token: 0x04001777 RID: 6007
		private bool anyDirtyLight;

		// Token: 0x04001778 RID: 6008
		private bool hasRunInitially;

		// Token: 0x04001779 RID: 6009
		private const int AlphaOfOverlit = 1;

		// Token: 0x0400177A RID: 6010
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x0400177B RID: 6011
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x0400177C RID: 6012
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x0400177D RID: 6013
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;

		// Token: 0x0400177E RID: 6014
		public const int MaxLightRadius = 40;

		// Token: 0x0400177F RID: 6015
		public const int MaxLightDiameter = 81;

		// Token: 0x04001780 RID: 6016
		public const int MaxLightCells = 6561;

		// Token: 0x0200041C RID: 1052
		private static class FloodFillerPool
		{
			// Token: 0x06001AA0 RID: 6816 RVA: 0x00087104 File Offset: 0x00085304
			public static void EnsureAllocated()
			{
				if (GlowGrid.FloodFillerPool.allocated)
				{
					return;
				}
				GlowGrid.FloodFillerPool.allocated = true;
				UnityData.DisposeStatic += GlowGrid.FloodFillerPool.Dispose;
				GlowGrid.FloodFillerPool.pool = new NativeArray<GlowUniqueState>(UnityData.MaxJobWorkerThreadCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < GlowGrid.FloodFillerPool.pool.Length; i++)
				{
					GlowGrid.FloodFillerPool.pool[i] = GlowUniqueState.AllocateNew();
				}
			}

			// Token: 0x06001AA1 RID: 6817 RVA: 0x00087168 File Offset: 0x00085368
			private static void Dispose()
			{
				if (!GlowGrid.FloodFillerPool.allocated)
				{
					return;
				}
				for (int i = 0; i < GlowGrid.FloodFillerPool.pool.Length; i++)
				{
					GlowGrid.FloodFillerPool.pool[i].Dispose();
				}
				GlowGrid.FloodFillerPool.pool.Dispose();
				GlowGrid.FloodFillerPool.allocated = false;
			}

			// Token: 0x04001781 RID: 6017
			public static NativeArray<GlowUniqueState> pool;

			// Token: 0x04001782 RID: 6018
			private static bool allocated;
		}

		// Token: 0x0200041D RID: 1053
		private class GlowPool : IDisposable
		{
			// Token: 0x06001AA2 RID: 6818 RVA: 0x000871B8 File Offset: 0x000853B8
			public int Take()
			{
				for (int i = 0; i < this.pool.Length; i++)
				{
					LocalGlowArea localGlowArea = this.pool[i];
					if (!localGlowArea.inUse)
					{
						localGlowArea.Take();
						this.pool[i] = localGlowArea;
						return i;
					}
				}
				LocalGlowArea localGlowArea2 = LocalGlowArea.AllocateNew();
				localGlowArea2.Take();
				this.pool.Add(localGlowArea2);
				return this.pool.Length - 1;
			}

			// Token: 0x06001AA3 RID: 6819 RVA: 0x00087230 File Offset: 0x00085430
			public void ReturnSet(int index)
			{
				LocalGlowArea value = this.pool[index];
				value.Return();
				this.pool[index] = value;
			}

			// Token: 0x06001AA4 RID: 6820 RVA: 0x00087260 File Offset: 0x00085460
			public void Dispose()
			{
				for (int i = 0; i < this.pool.Length; i++)
				{
					this.pool[i].Dispose();
				}
				this.pool.Dispose();
			}

			// Token: 0x04001783 RID: 6019
			public NativeList<LocalGlowArea> pool = new NativeList<LocalGlowArea>(4096, Allocator.Persistent);
		}

		// Token: 0x0200041E RID: 1054
		[BurstCompile]
		private struct CombineColorsJob : IJobParallelFor
		{
			// Token: 0x06001AA6 RID: 6822 RVA: 0x000872C0 File Offset: 0x000854C0
			[BurstCompile]
			public void Execute(int i)
			{
				if (!this.dirtyCells.IsSet(i) && !this.allDirty)
				{
					return;
				}
				this.dirtyCells.Set(i, false);
				IntVec3 c = this.indices.IndexToCell(i);
				Color32 value = default(Color32);
				Color32 value2 = default(Color32);
				for (int j = 0; j < this.lights.Length; j++)
				{
					GlowLight glowLight = this.lights[j];
					if (glowLight.AffectedRect.Contains(c))
					{
						int index = glowLight.WorldToLocalIndex(c);
						Color32 color = this.glowPool[glowLight.localGlowPoolIndex].colors[index];
						this.AddColors(ref value, color, glowLight.overlightRadius);
						if (!glowLight.isCavePlant)
						{
							this.AddColors(ref value2, color, glowLight.overlightRadius);
						}
					}
				}
				this.glow[i] = value;
				this.glowNoCavePlants[i] = value2;
			}

			// Token: 0x06001AA7 RID: 6823 RVA: 0x000873BC File Offset: 0x000855BC
			[BurstCompile]
			private void AddColors(ref Color32 existingSum, in Color32 toAdd, float overlightRadius)
			{
				float num = (float)toAdd.a;
				ColorInt colorInt = toAdd.AsColorInt();
				colorInt.ClampToNonNegative();
				colorInt.a = 0;
				if (colorInt.r <= 0 && colorInt.g <= 0 && colorInt.b <= 0)
				{
					return;
				}
				ColorInt colA = existingSum.AsColorInt();
				colA += colorInt;
				if (num < overlightRadius)
				{
					colA.a = 1;
				}
				Color32 color;
				colA.ProjectToColor32Fast(out color);
				existingSum = color;
			}

			// Token: 0x04001784 RID: 6020
			[ReadOnly]
			public NativeList<GlowLight> lights;

			// Token: 0x04001785 RID: 6021
			public UnsafeBitArray dirtyCells;

			// Token: 0x04001786 RID: 6022
			public CellIndices indices;

			// Token: 0x04001787 RID: 6023
			public bool allDirty;

			// Token: 0x04001788 RID: 6024
			[NativeDisableParallelForRestriction]
			public NativeArray<Color32> glow;

			// Token: 0x04001789 RID: 6025
			[NativeDisableParallelForRestriction]
			public NativeArray<Color32> glowNoCavePlants;

			// Token: 0x0400178A RID: 6026
			[NativeDisableParallelForRestriction]
			public NativeList<LocalGlowArea> glowPool;
		}
	}
}
