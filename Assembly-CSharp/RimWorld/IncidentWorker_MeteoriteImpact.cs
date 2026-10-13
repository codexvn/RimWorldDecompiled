using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB7 RID: 7607
	public class IncidentWorker_MeteoriteImpact : IncidentWorker
	{
		// Token: 0x0600A9EC RID: 43500 RVA: 0x00309D14 File Offset: 0x00307F14
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return IncidentWorker_MeteoriteImpact.TryFindCell(out intVec, map);
		}

		// Token: 0x0600A9ED RID: 43501 RVA: 0x00309D38 File Offset: 0x00307F38
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			List<Thing> contents;
			Skyfaller skyfaller = this.SpawnMeteoriteIncoming(parms, out contents);
			if (skyfaller == null)
			{
				return false;
			}
			skyfaller.impactLetter = this.MakeLetter(skyfaller, contents);
			return true;
		}

		// Token: 0x0600A9EE RID: 43502 RVA: 0x00309D63 File Offset: 0x00307F63
		protected virtual List<Thing> GenerateMeteorContents(IncidentParms parms)
		{
			return ThingSetMakerDefOf.Meteorite.root.Generate();
		}

		// Token: 0x0600A9EF RID: 43503 RVA: 0x00309D74 File Offset: 0x00307F74
		protected Skyfaller SpawnMeteoriteIncoming(IncidentParms parms, out List<Thing> contents)
		{
			Map map = (Map)parms.target;
			IntVec3 pos;
			if (!IncidentWorker_MeteoriteImpact.TryFindCell(out pos, map))
			{
				contents = null;
				return null;
			}
			contents = this.GenerateMeteorContents(parms);
			return SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, contents, pos, map);
		}

		// Token: 0x0600A9F0 RID: 43504 RVA: 0x00309DB4 File Offset: 0x00307FB4
		protected virtual Letter MakeLetter(Skyfaller meteorite, List<Thing> contents)
		{
			Thing thing = (contents != null) ? contents[0] : null;
			if (thing == null)
			{
				return null;
			}
			LetterDef def = thing.def.building.isResourceRock ? LetterDefOf.PositiveEvent : LetterDefOf.NeutralEvent;
			string str = this.def.letterText.Formatted(thing.def.label).CapitalizeFirst();
			return LetterMaker.MakeLetter(this.def.letterLabel + ": " + thing.def.LabelCap, str, def, new TargetInfo(meteorite.Position, meteorite.Map, false), null, null, null);
		}

		// Token: 0x0600A9F1 RID: 43505 RVA: 0x00309E6C File Offset: 0x0030806C
		private static bool TryFindCell(out IntVec3 cell, Map map)
		{
			int maxMineables = ThingSetMaker_Meteorite.MineablesCountRange.max;
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, TerrainAffordanceDefOf.Light, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)maxMineables)) + 2;
				CellRect other = CellRect.CenteredOn(x, num, num);
				int num2 = 0;
				foreach (IntVec3 c in other)
				{
					if (c.InBounds(map) && c.Standable(map))
					{
						num2++;
					}
				}
				if (ModsConfig.RoyaltyActive)
				{
					foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker))
					{
						MonumentMarker monumentMarker = thing as MonumentMarker;
						if (monumentMarker.AllDone && monumentMarker.sketch.OccupiedRect.ExpandedBy(3).MovedBy(monumentMarker.Position).Overlaps(other))
						{
							return false;
						}
					}
				}
				return num2 >= maxMineables;
			});
		}
	}
}
