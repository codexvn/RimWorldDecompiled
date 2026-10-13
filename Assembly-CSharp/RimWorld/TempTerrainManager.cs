using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002197 RID: 8599
	public class TempTerrainManager : IExposable
	{
		// Token: 0x0600BBD0 RID: 48080 RVA: 0x003668B4 File Offset: 0x00364AB4
		public TempTerrainManager(Map map)
		{
			this.map = map;
			if (ModsConfig.OdysseyActive)
			{
				this.freezeManager = new FreezeManager(map);
			}
		}

		// Token: 0x0600BBD1 RID: 48081 RVA: 0x003668E4 File Offset: 0x00364AE4
		public void Tick()
		{
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			using (new ProfilerBlock("Temporary Terrain Steady Effects"))
			{
				for (int i = 0; i < num; i++)
				{
					if (this.cycleIndex >= area)
					{
						this.cycleIndex = 0;
					}
					IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
					this.DoCellSteadyEffects(c);
					this.cycleIndex++;
				}
			}
			using (new ProfilerBlock("Temporary Terrain Removal"))
			{
				while (this.terrainToRemove.Count > 0 && this.terrainToRemove.Peek().tick <= Find.TickManager.TicksGame)
				{
					IntVec3 cell = this.terrainToRemove.Dequeue().cell;
					this.map.terrainGrid.RemoveTempTerrain(cell, false, false);
				}
			}
		}

		// Token: 0x0600BBD2 RID: 48082 RVA: 0x00366A08 File Offset: 0x00364C08
		private void DoCellSteadyEffects(IntVec3 c)
		{
			if (ModsConfig.OdysseyActive)
			{
				this.freezeManager.DoCellSteadyEffects(c);
			}
		}

		// Token: 0x0600BBD3 RID: 48083 RVA: 0x00366A20 File Offset: 0x00364C20
		public void QueueRemoveTerrain(IntVec3 c, int tick)
		{
			this.terrainToRemove.Enqueue(new TerrainRemoveTick
			{
				cell = c,
				tick = tick
			}, tick);
		}

		// Token: 0x0600BBD4 RID: 48084 RVA: 0x00366A54 File Offset: 0x00364C54
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Deep.Look<FreezeManager>(ref this.freezeManager, "freezeManager", Array.Empty<object>());
			List<IntVec3> list = new List<IntVec3>();
			List<int> list2 = new List<int>();
			while (this.terrainToRemove.Count > 0)
			{
				TerrainRemoveTick terrainRemoveTick = this.terrainToRemove.Dequeue();
				list.Add(terrainRemoveTick.cell);
				list2.Add(terrainRemoveTick.tick);
			}
			Scribe_Collections.Look<IntVec3>(ref list, "terrainToRemoveCells", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<int>(ref list2, "terrainToRemoveTicks", LookMode.Value, Array.Empty<object>());
			for (int i = 0; i < list.Count; i++)
			{
				this.terrainToRemove.Enqueue(new TerrainRemoveTick
				{
					cell = list[i],
					tick = list2[i]
				}, list2[i]);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.freezeManager == null)
			{
				this.freezeManager = new FreezeManager(this.map);
			}
		}

		// Token: 0x04008156 RID: 33110
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x04008157 RID: 33111
		private Map map;

		// Token: 0x04008158 RID: 33112
		private FreezeManager freezeManager;

		// Token: 0x04008159 RID: 33113
		private int cycleIndex;

		// Token: 0x0400815A RID: 33114
		private PriorityQueue<TerrainRemoveTick, int> terrainToRemove = new PriorityQueue<TerrainRemoveTick, int>();
	}
}
