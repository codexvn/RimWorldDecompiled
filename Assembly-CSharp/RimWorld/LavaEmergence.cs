using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AA4 RID: 10916
	public class LavaEmergence : Thing
	{
		// Token: 0x17002606 RID: 9734
		// (get) Token: 0x0600F32E RID: 62254 RVA: 0x00469B0F File Offset: 0x00467D0F
		protected virtual IntRange PoolSizeRange
		{
			get
			{
				return new IntRange(450, 600);
			}
		}

		// Token: 0x17002607 RID: 9735
		// (get) Token: 0x0600F32F RID: 62255 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool FireLetter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600F330 RID: 62256 RVA: 0x00469B20 File Offset: 0x00467D20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.openCells, "openCells", LookMode.Value);
			Scribe_Collections.Look<IntVec3>(ref this.lavaCells, "lavaCells", LookMode.Value);
			Scribe_Values.Look<int>(ref this.poolSize, "poolSize", 0, false);
			Scribe_Values.Look<int>(ref this.coolDelay, "coolDelay", 0, false);
		}

		// Token: 0x0600F331 RID: 62257 RVA: 0x00469B7C File Offset: 0x00467D7C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.openCells.AddRange(this.GetInitialCells(map));
			if (this.openCells.Count == 0)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.poolSize = ((this.forcePoolSize > 0) ? this.forcePoolSize : this.PoolSizeRange.RandomInRange);
			this.coolDelay = ((this.forceCoolDelay > 0) ? this.forceCoolDelay : LavaEmergence.LavaBeginCoolingDelay.RandomInRange);
			if (this.FireLetter)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelLavaEmergence".Translate(), "LetterLavaEmergence".Translate(), LetterDefOf.ThreatSmall, new LookTargets(this.openCells.First<IntVec3>(), base.Map), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600F332 RID: 62258 RVA: 0x00469C4C File Offset: 0x00467E4C
		protected virtual IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			IntVec3 root;
			if (!CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.GetTerrain(map) == TerrainDefOf.LavaDeep && !c.Fogged(map), out root))
			{
				yield break;
			}
			HashSet<IntVec3> lavaDeepCells = new HashSet<IntVec3>();
			map.floodFiller.FloodFill(root, (IntVec3 c) => c.GetTerrain(map) == TerrainDefOf.LavaDeep, delegate(IntVec3 c)
			{
				lavaDeepCells.Add(c);
			}, int.MaxValue, false, null);
			foreach (IntVec3 intVec in lavaDeepCells.ToList<IntVec3>())
			{
				bool flag = false;
				foreach (IntVec3 b in GenAdj.CardinalDirections)
				{
					IntVec3 c2 = intVec + b;
					if (c2.InBounds(map) && c2.GetTerrain(map) != TerrainDefOf.LavaDeep)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					lavaDeepCells.Remove(intVec);
				}
			}
			int count = LavaEmergence.EmergencePointsCountRange.RandomInRange;
			int j;
			for (int i = 0; i < count; i = j + 1)
			{
				IntVec3 intVec2 = lavaDeepCells.RandomElement<IntVec3>();
				lavaDeepCells.Remove(intVec2);
				IntVec3 intVec3 = intVec2;
				foreach (IntVec3 b2 in GenAdj.CardinalDirections)
				{
					IntVec3 intVec4 = intVec2 + b2;
					if (intVec4.InBounds(map) && intVec4.GetTerrain(map) != TerrainDefOf.LavaDeep)
					{
						intVec3 = intVec4;
						break;
					}
				}
				yield return intVec3;
				j = i;
			}
			yield break;
		}

		// Token: 0x0600F333 RID: 62259 RVA: 0x00469C5C File Offset: 0x00467E5C
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(480))
			{
				return;
			}
			this.SpreadLava();
			if (this.lavaCells.Count >= this.poolSize || this.openCells.Count == 0)
			{
				this.BeginCooling();
			}
		}

		// Token: 0x0600F334 RID: 62260 RVA: 0x00469C98 File Offset: 0x00467E98
		protected void SpreadLava()
		{
			if (this.openCells.Count == 0)
			{
				return;
			}
			IntVec3 intVec;
			if (!this.openCells.TryRandomElementByWeight((IntVec3 c) => (float)(Mathf.Max(this.AdjacentLavaCells(c), 1) * 4), out intVec))
			{
				intVec = this.openCells.RandomElement<IntVec3>();
			}
			this.openCells.Remove(intVec);
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 intVec2 = intVec + b;
				if (!this.openCells.Contains(intVec2) && !this.lavaCells.Contains(intVec2) && this.CanLavaSpreadInto(intVec2))
				{
					this.openCells.Add(intVec2);
				}
			}
			base.Map.terrainGrid.SetTempTerrain(intVec, TerrainDefOf.LavaShallow);
			this.lavaCells.Add(intVec);
		}

		// Token: 0x0600F335 RID: 62261 RVA: 0x00469D68 File Offset: 0x00467F68
		protected void BeginCooling()
		{
			List<IntVec3> cells = this.lavaCells.ToList<IntVec3>();
			int num = 0;
			Func<IntVec3, float> <>9__0;
			while (!cells.Empty<IntVec3>())
			{
				IEnumerable<IntVec3> cells2 = cells;
				Func<IntVec3, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = delegate(IntVec3 c)
					{
						int num2 = 0;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							IntVec3 intVec2 = c + b;
							if (intVec2.InBounds(this.Map) && (cells.Contains(intVec2) || intVec2.GetTerrain(this.Map) == TerrainDefOf.LavaDeep))
							{
								num2++;
							}
						}
						return (float)(16 - num2 * 4);
					});
				}
				IntVec3 intVec;
				if (!cells2.TryRandomElementByWeight(weightSelector, out intVec))
				{
					intVec = cells.RandomElement<IntVec3>();
				}
				base.Map.tempTerrain.QueueRemoveTerrain(intVec, Find.TickManager.TicksGame + this.coolDelay + 400 * num);
				cells.Remove(intVec);
				num++;
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600F336 RID: 62262 RVA: 0x00469E24 File Offset: 0x00468024
		private bool CanLavaSpreadInto(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && !edifice.IsClearableFreeBuilding)
			{
				return false;
			}
			TerrainDef terrain = c.GetTerrain(base.Map);
			return terrain.natural && base.Map.terrainGrid.FoundationAt(c) == null && terrain != TerrainDefOf.LavaDeep;
		}

		// Token: 0x0600F337 RID: 62263 RVA: 0x00469E94 File Offset: 0x00468094
		private int AdjacentLavaCells(IntVec3 c)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(base.Map) && c2.GetTerrain(base.Map) == TerrainDefOf.LavaShallow)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0400A02F RID: 41007
		private static readonly IntRange EmergencePointsCountRange = new IntRange(2, 4);

		// Token: 0x0400A030 RID: 41008
		private static readonly IntRange LavaBeginCoolingDelay = new IntRange(300000, 360000);

		// Token: 0x0400A031 RID: 41009
		private const int LavaCoolInterval = 400;

		// Token: 0x0400A032 RID: 41010
		private const int ExpandIntervalTicks = 480;

		// Token: 0x0400A033 RID: 41011
		[Unsaved(false)]
		public int forcePoolSize = -1;

		// Token: 0x0400A034 RID: 41012
		[Unsaved(false)]
		public int forceCoolDelay = -1;

		// Token: 0x0400A035 RID: 41013
		protected HashSet<IntVec3> openCells = new HashSet<IntVec3>();

		// Token: 0x0400A036 RID: 41014
		protected HashSet<IntVec3> lavaCells = new HashSet<IntVec3>();

		// Token: 0x0400A037 RID: 41015
		protected int poolSize;

		// Token: 0x0400A038 RID: 41016
		private int coolDelay;
	}
}
