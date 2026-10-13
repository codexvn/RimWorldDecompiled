using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFC RID: 8188
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x0600B464 RID: 46180 RVA: 0x0033C0D4 File Offset: 0x0033A2D4
		public override int SeedPart
		{
			get
			{
				return 920906419;
			}
		}

		// Token: 0x0600B465 RID: 46181 RVA: 0x0033C0DC File Offset: 0x0033A2DC
		public override void Generate(Map map, GenStepParams parms)
		{
			this.minSpacing = 5f;
			this.warnOnFail = false;
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x0600B466 RID: 46182 RVA: 0x0033C13C File Offset: 0x0033A33C
		protected override int CalculateFinalCount(Map map)
		{
			if (this.count >= 0)
			{
				return Mathf.RoundToInt((float)this.count * this.GetPlacementFactor(map));
			}
			float num = this.countPer10kCellsRange.RandomInRange;
			if (!map.IsStartingMap && this.useNomadicMineables)
			{
				num *= Current.Game.storyteller.difficulty.nomadicMineableResourcesFactor;
			}
			return Mathf.RoundToInt((float)GenStep_Scatterer.CountFromPer10kCells(num, map, -1) * this.GetPlacementFactor(map));
		}

		// Token: 0x0600B467 RID: 46183 RVA: 0x0033C1B0 File Offset: 0x0033A3B0
		protected virtual ThingDef ChooseThingDef()
		{
			if (this.forcedDefToScatter != null)
			{
				return this.forcedDefToScatter;
			}
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeightWithFallback(delegate(ThingDef d)
			{
				if (d.building == null)
				{
					return 0f;
				}
				if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
				{
					return 0f;
				}
				return d.building.mineableScatterCommonality;
			}, null);
		}

		// Token: 0x0600B468 RID: 46184 RVA: 0x0033C1D8 File Offset: 0x0033A3D8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (base.NearUsedSpot(c, this.CalculateFinalMinSpacing(map)))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x0600B469 RID: 46185 RVA: 0x0033C218 File Offset: 0x0033A418
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			GenStep_ScatterLumpsMineable.<>c__DisplayClass11_0 CS$<>8__locals1 = new GenStep_ScatterLumpsMineable.<>c__DisplayClass11_0();
			if (Current.ProgramState == ProgramState.MapInitializing && Find.Storyteller.def.tutorialMode)
			{
				return;
			}
			ThingDef thingDef = this.ChooseThingDef();
			if (thingDef == null)
			{
				return;
			}
			int numCells = (this.forcedLumpSize > 0) ? this.forcedLumpSize : thingDef.building.mineableScatterLumpSizeRange.RandomInRange;
			this.recentLumpCells.Clear();
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(c, map, numCells, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__Validator|0)))
			{
				GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
				this.recentLumpCells.Add(intVec);
			}
		}

		// Token: 0x04007BB0 RID: 31664
		public ThingDef forcedDefToScatter;

		// Token: 0x04007BB1 RID: 31665
		public int forcedLumpSize;

		// Token: 0x04007BB2 RID: 31666
		public float maxValue = float.MaxValue;

		// Token: 0x04007BB3 RID: 31667
		public bool useNomadicMineables;

		// Token: 0x04007BB4 RID: 31668
		[Unsaved(false)]
		protected readonly List<IntVec3> recentLumpCells = new List<IntVec3>();
	}
}
