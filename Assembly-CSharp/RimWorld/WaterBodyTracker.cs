using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200219B RID: 8603
	public class WaterBodyTracker : IExposable
	{
		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x0600BBE6 RID: 48102 RVA: 0x00366E42 File Offset: 0x00365042
		public List<WaterBody> Bodies
		{
			get
			{
				return this.bodies;
			}
		}

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x0600BBE7 RID: 48103 RVA: 0x00366E4A File Offset: 0x0036504A
		public bool AnyBodyContainsFish
		{
			get
			{
				return this.anyBodyContainsFish;
			}
		}

		// Token: 0x0600BBE8 RID: 48104 RVA: 0x00366E52 File Offset: 0x00365052
		public WaterBodyTracker(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BBE9 RID: 48105 RVA: 0x00366E6C File Offset: 0x0036506C
		public void ConstructBodies()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.bodiesByCell.Clear();
			if (this.bodies == null)
			{
				this.bodies = new List<WaterBody>();
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					IntVec3 c = this.map.cellIndices.IndexToCell(i);
					WaterBodyType waterBodyType = c.GetWaterBodyType(this.map);
					WaterBody waterBody;
					if (waterBodyType != WaterBodyType.None && !this.TryGetWaterBodyAt(c, out waterBody))
					{
						this.TryCreateBodyOrAddToExisting(c, waterBodyType);
					}
				}
				for (int j = 0; j < this.bodies.Count; j++)
				{
					this.bodies[j].Initialize();
					if (this.bodies[j].HasFish)
					{
						this.anyBodyContainsFish = true;
					}
				}
				return;
			}
			for (int k = 0; k < this.bodies.Count; k++)
			{
				WaterBody waterBody2 = this.bodies[k];
				waterBody2.CellCount = 0;
				waterBody2.cells.Clear();
				this.FloodFillBody(waterBody2.rootCell, this.bodies[k]);
				waterBody2.RecacheState();
				if (waterBody2.HasFish)
				{
					this.anyBodyContainsFish = true;
				}
			}
			for (int l = 0; l < this.map.cellIndices.NumGridCells; l++)
			{
				IntVec3 c2 = this.map.cellIndices.IndexToCell(l);
				WaterBodyType waterBodyType2 = c2.GetWaterBodyType(this.map);
				WaterBody waterBody;
				if (waterBodyType2 != WaterBodyType.None && !this.TryGetWaterBodyAt(c2, out waterBody))
				{
					this.TryCreateBodyOrAddToExisting(c2, waterBodyType2);
				}
			}
		}

		// Token: 0x0600BBEA RID: 48106 RVA: 0x00367004 File Offset: 0x00365204
		public void Tick()
		{
			if (this.map.TileInfo.MaxFishPopulation <= 0f)
			{
				return;
			}
			if (this.map.IsHashIntervalTick(2500))
			{
				GameCondition gameCondition;
				float num = this.map.gameConditionManager.FishPopulationOffsetFactorPerDay(this.map, out gameCondition);
				for (int i = 0; i < this.bodies.Count; i++)
				{
					if (this.bodies[i] != null)
					{
						this.bodies[i].Population += this.bodies[i].MaxPopulation * num / 24f;
					}
				}
			}
		}

		// Token: 0x0600BBEB RID: 48107 RVA: 0x003670AC File Offset: 0x003652AC
		public void Notify_Fished(IntVec3 c, float amount)
		{
			WaterBody waterBody;
			if (this.TryGetWaterBodyAt(c, out waterBody))
			{
				waterBody.Population -= amount;
			}
		}

		// Token: 0x0600BBEC RID: 48108 RVA: 0x003670D4 File Offset: 0x003652D4
		public bool AnyFishPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			return this.TryGetWaterBodyAt(c, out waterBody) && waterBody.HasFish;
		}

		// Token: 0x0600BBED RID: 48109 RVA: 0x003670F4 File Offset: 0x003652F4
		public float FishPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			if (!this.TryGetWaterBodyAt(c, out waterBody))
			{
				return 0f;
			}
			return waterBody.Population;
		}

		// Token: 0x0600BBEE RID: 48110 RVA: 0x00367118 File Offset: 0x00365318
		public float MaxPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			if (!this.TryGetWaterBodyAt(c, out waterBody))
			{
				return 0f;
			}
			return waterBody.MaxPopulation;
		}

		// Token: 0x0600BBEF RID: 48111 RVA: 0x0036713C File Offset: 0x0036533C
		public float PopulationPercentAt(IntVec3 c)
		{
			float num = this.MaxPopulationAt(c);
			if (num <= 0f)
			{
				return 0f;
			}
			return this.FishPopulationAt(c) / num;
		}

		// Token: 0x0600BBF0 RID: 48112 RVA: 0x00367168 File Offset: 0x00365368
		public WaterBody WaterBodyAt(IntVec3 c)
		{
			WaterBody result;
			if (!this.TryGetWaterBodyAt(c, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600BBF1 RID: 48113 RVA: 0x00367183 File Offset: 0x00365383
		public bool TryGetWaterBodyAt(IntVec3 c, out WaterBody body)
		{
			if (!c.InBounds(this.map))
			{
				body = null;
				return false;
			}
			return this.bodiesByCell.TryGetValue(c, out body) && body != null;
		}

		// Token: 0x0600BBF2 RID: 48114 RVA: 0x003671B0 File Offset: 0x003653B0
		public void Notify_TerrainChanged(IntVec3 cell, TerrainDef oldTerr, TerrainDef newTerr)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			WaterBodyType? waterBodyType = (newTerr != null) ? new WaterBodyType?(newTerr.waterBodyType) : null;
			WaterBodyType? waterBodyType2 = (oldTerr != null) ? new WaterBodyType?(oldTerr.waterBodyType) : null;
			if (waterBodyType.GetValueOrDefault() == waterBodyType2.GetValueOrDefault() & waterBodyType != null == (waterBodyType2 != null))
			{
				return;
			}
			WaterBodyType waterBodyType3 = cell.GetWaterBodyType(this.map);
			if (waterBodyType3 != WaterBodyType.None)
			{
				this.TryCreateBodyOrAddToExisting(cell, waterBodyType3);
			}
			else
			{
				this.RemoveCellFromExistingBody(cell);
			}
			WaterBody waterBody = cell.GetWaterBody(this.map);
			if (waterBody == null)
			{
				return;
			}
			waterBody.Notify_TerrainChanged(cell, oldTerr, newTerr);
		}

		// Token: 0x0600BBF3 RID: 48115 RVA: 0x00367258 File Offset: 0x00365458
		public void Notify_PollutionChanged(IntVec3 cell, bool isPolluted)
		{
			if (this.WaterBodyAt(cell) == null)
			{
				return;
			}
			WaterBody waterBody = cell.GetWaterBody(this.map);
			if (waterBody == null)
			{
				return;
			}
			waterBody.Notify_PollutionChanged(cell, isPolluted);
		}

		// Token: 0x0600BBF4 RID: 48116 RVA: 0x0036727C File Offset: 0x0036547C
		private void TryCreateBodyOrAddToExisting(IntVec3 c, WaterBodyType waterBodyType)
		{
			WaterBody waterBody;
			if (this.TryGetWaterBodyAt(c, out waterBody) && waterBody.waterBodyType == waterBodyType)
			{
				return;
			}
			WaterBody waterBody2 = null;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				WaterBody waterBody3;
				if (c2.InBounds(this.map) && c2.GetWaterBodyType(this.map) == waterBodyType && this.TryGetWaterBodyAt(c2, out waterBody3) && (waterBody2 == null || waterBody3.HasFish))
				{
					waterBody2 = waterBody3;
				}
			}
			if (waterBody2 != null)
			{
				this.FloodFillBody(c, waterBody2);
				return;
			}
			WaterBody waterBody4 = new WaterBody(this.map, c);
			this.bodies.Add(waterBody4);
			waterBody4.Initialize();
			this.bodiesByCell.SetOrAdd(c, waterBody4);
			waterBody4.cells.Add(c);
			if (waterBody4.MaxPopulation > 0f)
			{
				this.anyBodyContainsFish = true;
			}
		}

		// Token: 0x0600BBF5 RID: 48117 RVA: 0x00367354 File Offset: 0x00365554
		private void RemoveCellFromExistingBody(IntVec3 c)
		{
			Zone_Fishing zone_Fishing = c.GetZone(this.map) as Zone_Fishing;
			if (zone_Fishing != null)
			{
				zone_Fishing.RemoveCell(c);
			}
			if (c.GetWaterBodyType(this.map) != WaterBodyType.None)
			{
				return;
			}
			WaterBody body;
			if (this.TryGetWaterBodyAt(c, out body))
			{
				this.bodiesByCell.Remove(c);
				body.cells.Remove(c);
				WaterBody body2 = body;
				int cellCount = body2.CellCount;
				body2.CellCount = cellCount - 1;
				if (body.CellCount == 0)
				{
					this.bodies.Remove(body);
					return;
				}
				if (body.rootCell == c)
				{
					body.rootCell = this.bodiesByCell.Keys.First((IntVec3 x) => this.bodiesByCell[x] == body);
				}
			}
		}

		// Token: 0x0600BBF6 RID: 48118 RVA: 0x0036743C File Offset: 0x0036563C
		private void FloodFillBody(IntVec3 c, WaterBody body)
		{
			this.map.floodFiller.FloodFill(c, delegate(IntVec3 x)
			{
				WaterBody waterBody;
				return (!this.TryGetWaterBodyAt(x, out waterBody) || waterBody != body) && x.GetWaterBodyType(this.map) == body.waterBodyType;
			}, delegate(IntVec3 x)
			{
				WaterBody waterBody;
				if (this.TryGetWaterBodyAt(x, out waterBody) && waterBody != body)
				{
					this.bodies.Remove(waterBody);
				}
				WaterBody body2 = body;
				int cellCount = body2.CellCount;
				body2.CellCount = cellCount + 1;
				this.bodiesByCell.SetOrAdd(x, body);
				body.cells.Add(x);
			}, int.MaxValue, false, null);
		}

		// Token: 0x0600BBF7 RID: 48119 RVA: 0x00367490 File Offset: 0x00365690
		public void ExposeData()
		{
			Scribe_Collections.Look<WaterBody>(ref this.bodies, "waterBodies", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastRareCatchTick, "lastRareCatchTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastNegativeCatchTick, "lastNegativeCatchTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.bodies != null)
			{
				foreach (WaterBody waterBody in this.bodies)
				{
					waterBody.map = this.map;
				}
			}
		}

		// Token: 0x0600BBF8 RID: 48120 RVA: 0x00367530 File Offset: 0x00365730
		public void DebugDraw()
		{
			if (DebugViewSettings.drawWaterBodies)
			{
				foreach (WaterBody waterBody in this.bodies)
				{
					Rand.PushState(waterBody.GetHashCode());
					Color col = new Color(Rand.Range(0f, 1f), Rand.Range(0f, 1f), Rand.Range(0f, 1f), 0.5f);
					foreach (IntVec3 c in waterBody.cells)
					{
						CellRenderer.RenderCell(c, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
					}
					Rand.PopState();
				}
			}
		}

		// Token: 0x04008166 RID: 33126
		private readonly Map map;

		// Token: 0x04008167 RID: 33127
		private List<WaterBody> bodies;

		// Token: 0x04008168 RID: 33128
		public int lastRareCatchTick;

		// Token: 0x04008169 RID: 33129
		public int lastNegativeCatchTick;

		// Token: 0x0400816A RID: 33130
		[Unsaved(false)]
		private bool anyBodyContainsFish;

		// Token: 0x0400816B RID: 33131
		[Unsaved(false)]
		private Dictionary<IntVec3, WaterBody> bodiesByCell = new Dictionary<IntVec3, WaterBody>();

		// Token: 0x0400816C RID: 33132
		private const int TickInterval = 2500;
	}
}
