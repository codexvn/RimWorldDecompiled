using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD7 RID: 7639
	public class IncidentWorker_SpecialTreeSpawn : IncidentWorker
	{
		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600AA5D RID: 43613 RVA: 0x0030BAAD File Offset: 0x00309CAD
		protected GenStep_SpecialTrees GenStep
		{
			get
			{
				if (this.genStep == null)
				{
					this.genStep = (GenStep_SpecialTrees)this.def.treeGenStepDef.genStep;
				}
				return this.genStep;
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x0600AA5E RID: 43614 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int MaxTreesPerIncident
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x0600AA5F RID: 43615 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool SendLetter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AA60 RID: 43616 RVA: 0x0030BAD8 File Offset: 0x00309CD8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			if (map == null)
			{
				return false;
			}
			if (!parms.bypassStorytellerSettings && map.Biome.isExtremeBiome)
			{
				return false;
			}
			int num = this.GenStep.DesiredTreeCountForMap(map);
			IntVec3 intVec;
			return map.listerThings.ThingsOfDef(this.def.treeDef).Count < num && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x0600AA61 RID: 43617 RVA: 0x0030BB50 File Offset: 0x00309D50
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = Math.Min(this.MaxTreesPerIncident, this.GenStep.DesiredTreeCountForMap(map) - map.listerThings.ThingsOfDef(this.def.treeDef).Count);
			List<Thing> list = new List<Thing>();
			int num2 = 0;
			IntVec3 cell;
			Thing item;
			while (num2 < num && this.TryFindRootCell(map, out cell) && this.GenStep.TrySpawnAt(cell, map, this.def.treeGrowth, out item))
			{
				list.Add(item);
				num2++;
			}
			if (list.NullOrEmpty<Thing>())
			{
				return false;
			}
			if (this.SendLetter)
			{
				base.SendStandardLetter(this.def.letterLabel, (list.Count > 1) ? this.def.letterTextPlural : this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			}
			return true;
		}

		// Token: 0x0600AA62 RID: 43618 RVA: 0x0030BC48 File Offset: 0x00309E48
		protected virtual bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 40, 0, 0f, 22, 10, 0, -1, 15), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 10, 0, 0f, 18, 20, 0, -1, 15), map, out cell);
		}

		// Token: 0x04007583 RID: 30083
		private GenStep_SpecialTrees genStep;
	}
}
