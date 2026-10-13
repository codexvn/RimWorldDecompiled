using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002016 RID: 8214
	public class GenStep_AncientStockpile : GenStep
	{
		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x0600B4D0 RID: 46288 RVA: 0x0033E2AA File Offset: 0x0033C4AA
		public override int SeedPart
		{
			get
			{
				return 928734;
			}
		}

		// Token: 0x0600B4D1 RID: 46289 RVA: 0x0033E2B4 File Offset: 0x0033C4B4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect = map.Center.RectAbout(new IntVec2(GenStep_AncientStockpile.SizeRange.RandomInRange, GenStep_AncientStockpile.SizeRange.RandomInRange));
			StructureGenParams parms2 = new StructureGenParams
			{
				size = cellRect.Size
			};
			LayoutDef layout = parms.layout;
			LayoutWorker layoutWorker = ((layout != null) ? layout.Worker : null) ?? LayoutDefOf.AncientStockpile.Worker;
			LayoutStructureSketch layoutStructureSketch = layoutWorker.GenerateStructureSketch(parms2);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			float? threatPoints = null;
			if (parms.sitePart != null)
			{
				threatPoints = new float?(parms.sitePart.parms.points);
			}
			layoutWorker.Spawn(layoutStructureSketch, map, cellRect.Min, threatPoints, null, true, false, null);
		}

		// Token: 0x04007BFC RID: 31740
		private static readonly IntRange SizeRange = new IntRange(40, 50);
	}
}
