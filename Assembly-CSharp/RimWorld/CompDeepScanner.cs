using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F0A RID: 12042
	public class CompDeepScanner : CompScanner
	{
		// Token: 0x17002BE4 RID: 11236
		// (get) Token: 0x0601100C RID: 69644 RVA: 0x004E42D3 File Offset: 0x004E24D3
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		// Token: 0x17002BE5 RID: 11237
		// (get) Token: 0x0601100D RID: 69645 RVA: 0x004E42E0 File Offset: 0x004E24E0
		public override AcceptanceReport CanUseNow
		{
			get
			{
				if (!this.parent.Map.Biome.hasBedrock)
				{
					return "CannotUseScannerNoBedrock".Translate();
				}
				return base.CanUseNow;
			}
		}

		// Token: 0x0601100E RID: 69646 RVA: 0x004E4310 File Offset: 0x004E2510
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			if (!this.parent.Map.Biome.hasBedrock)
			{
				Messages.Message("MessageGroundPenetratingScannerNoBedrock".Translate(this.parent.Named("THING")), this.parent, MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x0601100F RID: 69647 RVA: 0x004E4374 File Offset: 0x004E2574
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.ShouldShowDeepResourceOverlay())
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x06011010 RID: 69648 RVA: 0x004E4393 File Offset: 0x004E2593
		public bool ShouldShowDeepResourceOverlay()
		{
			return this.powerComp != null && this.powerComp.PowerOn;
		}

		// Token: 0x06011011 RID: 69649 RVA: 0x004E43AC File Offset: 0x004E25AC
		protected override void DoFind(Pawn worker)
		{
			Map map = this.parent.Map;
			IntVec3 intVec;
			if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanScatterAt(x, map), map, out intVec))
			{
				Log.Error("Could not find a center cell for deep scanning lump generation!");
			}
			ThingDef thingDef = this.ChooseLumpThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, numCells, null))
			{
				if (this.CanScatterAt(intVec2, map) && !intVec2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(intVec2, thingDef, thingDef.deepCountPerCell);
				}
			}
			string key;
			if ("LetterDeepScannerFoundLump".CanTranslate())
			{
				key = "LetterDeepScannerFoundLump";
			}
			else if ("DeepScannerFoundLump".CanTranslate())
			{
				key = "DeepScannerFoundLump";
			}
			else
			{
				key = "LetterDeepScannerFoundLump";
			}
			Find.LetterStack.ReceiveLetter("LetterLabelDeepScannerFoundLump".Translate() + ": " + thingDef.LabelCap, key.Translate(thingDef.label, worker.Named("FINDER")), LetterDefOf.PositiveEvent, new LookTargets(intVec, map), null, null, null, null, 0, true);
		}

		// Token: 0x06011012 RID: 69650 RVA: 0x004E4528 File Offset: 0x004E2728
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int index = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.BaseTerrainAt(pos);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && pos.GetAffordances(map).Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(index);
		}

		// Token: 0x06011013 RID: 69651 RVA: 0x004E4591 File Offset: 0x004E2791
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
