using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003030 RID: 12336
	public class Alert_PredatorInPen : Alert
	{
		// Token: 0x0601157D RID: 71037 RVA: 0x004F98C4 File Offset: 0x004F7AC4
		public override string GetLabel()
		{
			return "AlertPredatorInAnimalPen".Translate(this.targets.First<GlobalTargetInfo>().Thing.Named("ANIMAL")).CapitalizeFirst();
		}

		// Token: 0x0601157E RID: 71038 RVA: 0x004F9908 File Offset: 0x004F7B08
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
					{
						CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
						if (!compAnimalPenMarker.PenState.Unenclosed)
						{
							foreach (Region region in compAnimalPenMarker.PenState.ConnectedRegions)
							{
								foreach (Thing thing2 in region.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn))
								{
									Pawn pawn = (Pawn)thing2;
									if (pawn.IsAnimal && pawn.RaceProps.predator && pawn.Faction == null && pawn.GetRegion(RegionType.Set_Passable) == region)
									{
										this.targets.Add(pawn);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0601157F RID: 71039 RVA: 0x004F9ACC File Offset: 0x004F7CCC
		public override TaggedString GetExplanation()
		{
			return "AlertPredatorInAnimalPenDesc".Translate(this.targets.First<GlobalTargetInfo>().Thing.Named("ANIMAL")).CapitalizeFirst();
		}

		// Token: 0x06011580 RID: 71040 RVA: 0x004F9B08 File Offset: 0x004F7D08
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3A0 RID: 45984
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
