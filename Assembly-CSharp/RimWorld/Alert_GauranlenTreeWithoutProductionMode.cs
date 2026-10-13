using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200303F RID: 12351
	public class Alert_GauranlenTreeWithoutProductionMode : Alert
	{
		// Token: 0x060115BA RID: 71098 RVA: 0x004FAD40 File Offset: 0x004F8F40
		public Alert_GauranlenTreeWithoutProductionMode()
		{
			this.defaultLabel = "AlertGauranlenTreeWithoutDryadTypeLabel".Translate();
			this.defaultExplanation = "AlertGauranlenTreeWithoutDryadTypeDesc".Translate("ChangeMode".Translate());
			this.requireIdeology = true;
		}

		// Token: 0x060115BB RID: 71099 RVA: 0x004FAD9E File Offset: 0x004F8F9E
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x060115BC RID: 71100 RVA: 0x004FADB4 File Offset: 0x004F8FB4
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.DryadSpawner);
					for (int j = 0; j < list.Count; j++)
					{
						CompTreeConnection compTreeConnection = list[j].TryGetComp<CompTreeConnection>();
						if (compTreeConnection != null && compTreeConnection.Connected && !compTreeConnection.HasProductionMode)
						{
							this.targets.Add(list[j]);
						}
					}
				}
			}
		}

		// Token: 0x0400B3C2 RID: 46018
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
