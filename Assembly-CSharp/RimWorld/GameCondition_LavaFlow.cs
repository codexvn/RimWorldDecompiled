using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D44 RID: 7492
	public class GameCondition_LavaFlow : GameCondition
	{
		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600A7D8 RID: 42968 RVA: 0x00301127 File Offset: 0x002FF327
		private int FlowDurationTicks
		{
			get
			{
				return this.maxFlow * 5;
			}
		}

		// Token: 0x0600A7D9 RID: 42969 RVA: 0x00301131 File Offset: 0x002FF331
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.openCells, "openCells", LookMode.Value);
			Scribe_Values.Look<int>(ref this.maxFlow, "maxFlow", 0, false);
		}

		// Token: 0x0600A7DA RID: 42970 RVA: 0x0030115C File Offset: 0x002FF35C
		public override void Init()
		{
			base.Init();
			this.maxFlow = GameCondition_LavaFlow.MaxFlowRange.RandomInRange;
			foreach (IntVec3 intVec in this.GetInitialCells(base.SingleMap))
			{
				base.SingleMap.terrainGrid.SetTempTerrain(intVec, TerrainDefOf.LavaShallow);
				base.SingleMap.tempTerrain.QueueRemoveTerrain(intVec, Find.TickManager.TicksGame + this.FlowDurationTicks + 60000);
				this.openCells.Add(intVec);
			}
			if (this.openCells.Count == 0)
			{
				this.End();
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelLavaFlow".Translate(), "LetterLavaFlow".Translate(), LetterDefOf.ThreatSmall, new LookTargets(this.openCells.First<IntVec3>(), base.SingleMap), null, null, null, null, 0, true);
		}

		// Token: 0x0600A7DB RID: 42971 RVA: 0x00301260 File Offset: 0x002FF460
		private IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			Rot4 edge = Rot4.Random;
			int count = GameCondition_LavaFlow.StartPositionCountRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => (c.GetTerrain(map) == TerrainDefOf.VolcanicRock || c.GetTerrain(map) == TerrainDefOf.CooledLava) && !c.Fogged(map) && c.GetEdifice(map) == null));
				}
				IntVec3 intVec;
				if (CellFinder.TryFindRandomEdgeCellWith(validator, map, edge, 0f, out intVec))
				{
					yield return intVec;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600A7DC RID: 42972 RVA: 0x00301270 File Offset: 0x002FF470
		public override void GameConditionTick()
		{
			if (!base.SingleMap.IsHashIntervalTick(5))
			{
				return;
			}
			this.SpreadLava();
			if (Find.TickManager.TicksGame > this.startTick + this.FlowDurationTicks || this.openCells.Count == 0)
			{
				this.End();
			}
		}

		// Token: 0x0600A7DD RID: 42973 RVA: 0x003012C0 File Offset: 0x002FF4C0
		private void SpreadLava()
		{
			IntVec3 intVec;
			if (!this.openCells.TryRandomElementByWeight((IntVec3 c) => (float)(Mathf.Max(this.AdjacentLavaCells(c), 1) * 4), out intVec))
			{
				intVec = this.openCells.RandomElement<IntVec3>();
			}
			if (this.tempAdjCells == null)
			{
				this.tempAdjCells = GenAdj.CardinalDirections.ToList<IntVec3>();
			}
			this.tempAdjCells.Shuffle<IntVec3>();
			bool flag = true;
			foreach (IntVec3 b in this.tempAdjCells)
			{
				IntVec3 intVec2 = intVec + b;
				if (this.CanLavaPotentiallySpreadInto(intVec2))
				{
					flag = false;
				}
				if (this.CanLavaSpreadInto(intVec2))
				{
					base.SingleMap.terrainGrid.SetTempTerrain(intVec2, TerrainDefOf.LavaShallow);
					base.SingleMap.tempTerrain.QueueRemoveTerrain(intVec2, Find.TickManager.TicksGame + this.FlowDurationTicks + 60000);
					this.openCells.Add(intVec2);
					break;
				}
			}
			if (flag)
			{
				this.openCells.Remove(intVec);
			}
		}

		// Token: 0x0600A7DE RID: 42974 RVA: 0x003013D8 File Offset: 0x002FF5D8
		private bool CanLavaPotentiallySpreadInto(IntVec3 c)
		{
			return c.InBounds(base.SingleMap) && !this.openCells.Contains(c) && base.SingleMap.terrainGrid.FoundationAt(c) == null && (base.SingleMap.terrainGrid.UnderTerrainAt(c) ?? base.SingleMap.terrainGrid.TopTerrainAt(c)) == TerrainDefOf.VolcanicRock && base.SingleMap.terrainGrid.TerrainAt(c) != TerrainDefOf.LavaShallow;
		}

		// Token: 0x0600A7DF RID: 42975 RVA: 0x00301464 File Offset: 0x002FF664
		private bool CanLavaSpreadInto(IntVec3 c)
		{
			if (!this.CanLavaPotentiallySpreadInto(c))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.SingleMap);
			return edifice == null || edifice.IsClearableFreeBuilding;
		}

		// Token: 0x0600A7E0 RID: 42976 RVA: 0x00301498 File Offset: 0x002FF698
		private int AdjacentLavaCells(IntVec3 c)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(base.SingleMap) && c2.GetTerrain(base.SingleMap) == TerrainDefOf.LavaShallow)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04007430 RID: 29744
		private HashSet<IntVec3> openCells = new HashSet<IntVec3>();

		// Token: 0x04007431 RID: 29745
		private int maxFlow;

		// Token: 0x04007432 RID: 29746
		private const int FlowRateTicks = 5;

		// Token: 0x04007433 RID: 29747
		private const int CoolDurationTicks = 60000;

		// Token: 0x04007434 RID: 29748
		private static readonly IntRange StartPositionCountRange = new IntRange(3, 6);

		// Token: 0x04007435 RID: 29749
		private static readonly IntRange MaxFlowRange = new IntRange(20000, 32000);

		// Token: 0x04007436 RID: 29750
		private List<IntVec3> tempAdjCells;
	}
}
