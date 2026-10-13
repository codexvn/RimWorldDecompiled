using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003029 RID: 12329
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		// Token: 0x17002CF2 RID: 11506
		// (get) Token: 0x06011560 RID: 71008 RVA: 0x004F8D00 File Offset: 0x004F6F00
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
					for (int j = 0; j < list.Count; j++)
					{
						CompShipLandingBeacon compShipLandingBeacon = list[j].TryGetComp<CompShipLandingBeacon>();
						if (compShipLandingBeacon != null && compShipLandingBeacon.Active && !compShipLandingBeacon.LandingAreas.Any<ShipLandingArea>())
						{
							this.targets.Add(list[j]);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06011561 RID: 71009 RVA: 0x004F8DA0 File Offset: 0x004F6FA0
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x06011562 RID: 71010 RVA: 0x004F8DEF File Offset: 0x004F6FEF
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400B390 RID: 45968
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
