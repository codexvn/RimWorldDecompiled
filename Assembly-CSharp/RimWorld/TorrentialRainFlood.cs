using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A9B RID: 10907
	public class TorrentialRainFlood : Flood
	{
		// Token: 0x170025F3 RID: 9715
		// (get) Token: 0x0600F2E2 RID: 62178 RVA: 0x0046861B File Offset: 0x0046681B
		protected override int StartDelayTicks
		{
			get
			{
				return 4000;
			}
		}

		// Token: 0x0600F2E3 RID: 62179 RVA: 0x00468624 File Offset: 0x00466824
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3, int>(ref this.floodedCells, "floodedCells", LookMode.Value, LookMode.Value, ref this.tmpKeys, ref this.tmpValues, true, false, false);
		}

		// Token: 0x0600F2E4 RID: 62180 RVA: 0x00468658 File Offset: 0x00466858
		protected override IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				yield break;
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (WaterBody waterBody in map.waterBodyTracker.Bodies)
			{
				if (waterBody.waterBodyType != WaterBodyType.Saltwater)
				{
					bool flag = false;
					using (HashSet<IntVec3>.Enumerator enumerator2 = waterBody.cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (!enumerator2.Current.Roofed(map))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						list.AddRange(waterBody.cells);
					}
				}
			}
			if (list.Count == 0)
			{
				yield break;
			}
			foreach (IntVec3 intVec in list.ToList<IntVec3>())
			{
				bool flag2 = false;
				foreach (IntVec3 b in GenAdj.CardinalDirections)
				{
					IntVec3 c = intVec + b;
					if (c.InBounds(base.Map) && !c.GetTerrain(map).IsWater)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list.Remove(intVec);
				}
			}
			list.Shuffle<IntVec3>();
			foreach (IntVec3 item in list)
			{
				yield return new ValueTuple<IntVec3, int>(item, Flood.FloodWidthRange.RandomInRange);
			}
			List<IntVec3>.Enumerator enumerator4 = default(List<IntVec3>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600F2E5 RID: 62181 RVA: 0x00468670 File Offset: 0x00466870
		protected override void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain)
		{
			int value = (this.estimatedFloodedTiles - this.floodedTileCount) * 10;
			base.Map.terrainGrid.SetTempTerrain(cell, sourceTerrain.floodTerrain);
			this.floodedCells[cell] = value;
		}

		// Token: 0x0600F2E6 RID: 62182 RVA: 0x004686B4 File Offset: 0x004668B4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.floodedCells.Any<KeyValuePair<IntVec3, int>>())
			{
				int num = this.floodedCells.Values.Min();
				foreach (KeyValuePair<IntVec3, int> keyValuePair in this.floodedCells)
				{
					IntVec3 intVec;
					int num2;
					keyValuePair.Deconstruct(out intVec, out num2);
					IntVec3 c = intVec;
					int num3 = num2;
					base.Map.tempTerrain.QueueRemoveTerrain(c, Find.TickManager.TicksGame + 2000 + num3 - num);
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x04009FFC RID: 40956
		private const int RecedeIntervalTicks = 10;

		// Token: 0x04009FFD RID: 40957
		private Dictionary<IntVec3, int> floodedCells = new Dictionary<IntVec3, int>();

		// Token: 0x04009FFE RID: 40958
		private const int EndDelayTicks = 2000;

		// Token: 0x04009FFF RID: 40959
		private List<IntVec3> tmpKeys;

		// Token: 0x0400A000 RID: 40960
		private List<int> tmpValues;
	}
}
