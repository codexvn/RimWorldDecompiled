using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002069 RID: 8297
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		// Token: 0x17001BD1 RID: 7121
		// (get) Token: 0x0600B616 RID: 46614 RVA: 0x00344CD0 File Offset: 0x00342ED0
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x0600B617 RID: 46615 RVA: 0x00344CD8 File Offset: 0x00342ED8
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
			CellRect rect = CellRect.CenteredOn(c, 7, 7);
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

		// Token: 0x0600B618 RID: 46616 RVA: 0x00344D94 File Offset: 0x00342F94
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			else
			{
				ItemStashContentsComp component = map.Parent.GetComponent<ItemStashContentsComp>();
				if (component != null && component.contents.Any)
				{
					resolveParams.stockpileConcreteContents = component.contents;
				}
				else
				{
					resolveParams.thingSetMakerDef = (this.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile);
				}
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("storage", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			orGenerateVar.Add(cellRect);
		}

		// Token: 0x04007CE5 RID: 31973
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04007CE6 RID: 31974
		private const int Size = 7;
	}
}
