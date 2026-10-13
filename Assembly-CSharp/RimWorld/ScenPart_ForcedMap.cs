using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028B4 RID: 10420
	public class ScenPart_ForcedMap : ScenPart
	{
		// Token: 0x0600E402 RID: 58370 RVA: 0x0042A087 File Offset: 0x00428287
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<MapGeneratorDef>(ref this.mapGenerator, "mapGenerator");
			Scribe_Defs.Look<PlanetLayerDef>(ref this.layerDef, "layerDef");
		}

		// Token: 0x0600E403 RID: 58371 RVA: 0x0042A0B0 File Offset: 0x004282B0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_ForcedMapPlanetLayer".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(scenPartRect, this.mapGenerator.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (MapGeneratorDef localFd3 in from d in DefDatabase<MapGeneratorDef>.AllDefs
				where d.validScenarioMap
				select d)
				{
					MapGeneratorDef localFd = localFd3;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.mapGenerator = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			scenPartRect.y += ScenPart.RowHeight;
			if (Widgets.ButtonText(scenPartRect, this.layerDef.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (PlanetLayerDef localFd2 in DefDatabase<PlanetLayerDef>.AllDefs)
				{
					PlanetLayerDef localFd = localFd2;
					list2.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.layerDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
		}

		// Token: 0x0600E404 RID: 58372 RVA: 0x0042A2FC File Offset: 0x004284FC
		public override void PostWorldGenerate()
		{
			PlanetTile planetTile = TileFinder.RandomStartingTile();
			PlanetLayer planetLayer = Find.WorldGrid.FirstLayerOfDef(this.layerDef);
			if (this.layerDef != PlanetLayerDefOf.Surface && planetLayer != null)
			{
				planetTile = planetLayer.GetClosestTile(planetTile);
			}
			Find.GameInitData.startingTile = planetTile;
			Find.GameInitData.mapGeneratorDef = this.mapGenerator;
		}

		// Token: 0x040098A3 RID: 39075
		public MapGeneratorDef mapGenerator;

		// Token: 0x040098A4 RID: 39076
		public PlanetLayerDef layerDef;
	}
}
