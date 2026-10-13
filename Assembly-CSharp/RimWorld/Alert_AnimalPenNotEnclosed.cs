using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200302F RID: 12335
	public class Alert_AnimalPenNotEnclosed : Alert
	{
		// Token: 0x0601157A RID: 71034 RVA: 0x004F9799 File Offset: 0x004F7999
		public Alert_AnimalPenNotEnclosed()
		{
			this.defaultLabel = "AlertAnimalPenNotEnclosed".Translate();
			this.defaultExplanation = "AlertAnimalPenNotEnclosedDesc".Translate();
		}

		// Token: 0x0601157B RID: 71035 RVA: 0x004F97D8 File Offset: 0x004F79D8
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
					{
						CompAnimalPenMarker compAnimalPenMarker = building.TryGetComp<CompAnimalPenMarker>();
						if (!building.IsForbidden(Faction.OfPlayer) && compAnimalPenMarker.PenState.Unenclosed)
						{
							this.targets.Add(building);
						}
					}
				}
			}
		}

		// Token: 0x0601157C RID: 71036 RVA: 0x004F98B0 File Offset: 0x004F7AB0
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B39F RID: 45983
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
