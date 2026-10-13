using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200208C RID: 8332
	public class GenStep_WorkSiteStash : GenStep_Scatterer
	{
		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600B684 RID: 46724 RVA: 0x00344CD0 File Offset: 0x00342ED0
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x0600B685 RID: 46725 RVA: 0x003472A4 File Offset: 0x003454A4
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 10, 10);
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => x.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B686 RID: 46726 RVA: 0x00347380 File Offset: 0x00345580
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			resolveParams.workSitePoints = parms.sitePart.site.desiredThreatPoints;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push(this.symbolName, resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			orGenerateVar.Add(cellRect);
		}

		// Token: 0x04007D35 RID: 32053
		public string symbolName;

		// Token: 0x04007D36 RID: 32054
		private const int Size = 10;
	}
}
