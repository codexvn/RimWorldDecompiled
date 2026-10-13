using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A98 RID: 10904
	public class SeasonalFlood : Flood
	{
		// Token: 0x170025F0 RID: 9712
		// (get) Token: 0x0600F2CF RID: 62159 RVA: 0x00468132 File Offset: 0x00466332
		protected override int MaxFloodDurationTicks
		{
			get
			{
				return 240000;
			}
		}

		// Token: 0x0600F2D0 RID: 62160 RVA: 0x00468139 File Offset: 0x00466339
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.remainFloodedTicks, "floodedTicks", 0, false);
		}

		// Token: 0x0600F2D1 RID: 62161 RVA: 0x00468154 File Offset: 0x00466354
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.remainFloodedTicks = SeasonalFlood.FloodedTicksRange.RandomInRange;
			IntVec3 c2;
			CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.GetTerrain(map).IsRiver && !c.Fogged(map), out c2);
			Find.LetterStack.ReceiveLetter("LetterLabelSeasonalFlooding".Translate(), "LetterSeasonalFlooding".Translate(), LetterDefOf.ThreatSmall, new LookTargets(c2, map), null, null, null, null, 0, true);
		}

		// Token: 0x0600F2D2 RID: 62162 RVA: 0x004681EC File Offset: 0x004663EC
		protected override void Tick()
		{
			if (base.Map.mapTemperature.OutdoorTemp < -7f)
			{
				Messages.Message("MessageSeasonalFloodingFrozen".Translate(), MessageTypeDefOf.NeutralEvent, true);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			if (this.remainFloodedTicks > 0 && Find.TickManager.TicksGame > this.spawnedTick + base.FloodingTicks + this.remainFloodedTicks)
			{
				Messages.Message("MessageFloodingSubsiding".Translate(), MessageTypeDefOf.NeutralEvent, true);
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F2D3 RID: 62163 RVA: 0x0046828B File Offset: 0x0046648B
		protected override IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				yield break;
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetTerrain(map).IsRiver)
				{
					list.Add(intVec);
				}
			}
			foreach (IntVec3 intVec2 in list.ToList<IntVec3>())
			{
				bool flag = false;
				foreach (IntVec3 b in GenAdj.CardinalDirections)
				{
					IntVec3 c = intVec2 + b;
					if (c.InBounds(base.Map) && !c.GetTerrain(map).IsRiver)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Remove(intVec2);
				}
			}
			list.Shuffle<IntVec3>();
			foreach (IntVec3 item in list)
			{
				yield return new ValueTuple<IntVec3, int>(item, Flood.FloodWidthRange.RandomInRange);
			}
			List<IntVec3>.Enumerator enumerator3 = default(List<IntVec3>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600F2D4 RID: 62164 RVA: 0x004682A4 File Offset: 0x004664A4
		protected override void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain)
		{
			int num = this.estimatedFloodedTiles - this.floodedTileCount;
			base.Map.terrainGrid.SetTempTerrain(cell, TerrainDefOf.ShallowFloodwater);
			base.Map.tempTerrain.QueueRemoveTerrain(cell, this.spawnedTick + base.FloodingTicks + this.remainFloodedTicks + num);
		}

		// Token: 0x04009FF1 RID: 40945
		private const int RecedeIntervalTicks = 1;

		// Token: 0x04009FF2 RID: 40946
		private static readonly IntRange FloodedTicksRange = new IntRange(240000, 360000);

		// Token: 0x04009FF3 RID: 40947
		private int remainFloodedTicks;
	}
}
