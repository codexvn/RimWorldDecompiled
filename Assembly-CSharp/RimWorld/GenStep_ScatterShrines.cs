using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002006 RID: 8198
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x0600B496 RID: 46230 RVA: 0x0033CF64 File Offset: 0x0033B164
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x0600B497 RID: 46231 RVA: 0x0033CF6C File Offset: 0x0033B16C
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			this.randomSize.x = GenStep_ScatterShrines.SizeRange.RandomInRange;
			this.randomSize.z = GenStep_ScatterShrines.SizeRange.RandomInRange;
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x0600B498 RID: 46232 RVA: 0x0033CFB1 File Offset: 0x0033B1B1
		protected override bool ShouldSkipMap(Map map)
		{
			return !map.IsStartingMap && Rand.Chance(0.75f);
		}

		// Token: 0x0600B499 RID: 46233 RVA: 0x0033CFCC File Offset: 0x0033B1CC
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && edifice.def.building.isNaturalRock)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B49A RID: 46234 RVA: 0x0033D02D File Offset: 0x0033B22D
		protected override CellRect EffectiveRectAt(IntVec3 c)
		{
			return CellRect.CenteredOn(c, this.randomSize.x, this.randomSize.z);
		}

		// Token: 0x0600B49B RID: 46235 RVA: 0x0033D04C File Offset: 0x0033B24C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect cellRect = this.EffectiveRectAt(loc);
			CellRect cellRect2 = cellRect.ClipInsideMap(map);
			if (cellRect2.Width != cellRect.Width || cellRect2.Height != cellRect.Height)
			{
				return;
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == ThingDefOf.AncientCryptosleepCasket)
					{
						return;
					}
				}
			}
			if (!base.CanPlaceAncientBuildingInRange(cellRect, map))
			{
				return;
			}
			if (orGenerateVar.Contains(cellRect))
			{
				return;
			}
			orGenerateVar.Add(cellRect);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.disableSinglePawn = new bool?(true);
			resolveParams.disableHives = new bool?(true);
			resolveParams.makeWarningLetter = new bool?(true);
			if (Find.Storyteller.difficulty.peacefulTemples)
			{
				resolveParams.podContentsType = new PodContentsType?(PodContentsType.AncientFriendly);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04007BD2 RID: 31698
		private IntVec2 randomSize;

		// Token: 0x04007BD3 RID: 31699
		private static readonly IntRange SizeRange = new IntRange(15, 20);

		// Token: 0x04007BD4 RID: 31700
		private const float SkipNonStartingMapChance = 0.75f;
	}
}
