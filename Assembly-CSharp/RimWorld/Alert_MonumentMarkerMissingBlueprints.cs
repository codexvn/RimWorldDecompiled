using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003028 RID: 12328
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		// Token: 0x17002CF1 RID: 11505
		// (get) Token: 0x0601155D RID: 71005 RVA: 0x004F8BF4 File Offset: 0x004F6DF4
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (!monumentMarker.complete)
						{
							SketchEntity firstEntityWithMissingBlueprint = monumentMarker.FirstEntityWithMissingBlueprint;
							if (firstEntityWithMissingBlueprint != null)
							{
								this.targets.Add(new GlobalTargetInfo(firstEntityWithMissingBlueprint.pos + monumentMarker.Position, maps[i], false));
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0601155E RID: 71006 RVA: 0x004F8CA4 File Offset: 0x004F6EA4
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0601155F RID: 71007 RVA: 0x004F8CF3 File Offset: 0x004F6EF3
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400B38F RID: 45967
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
