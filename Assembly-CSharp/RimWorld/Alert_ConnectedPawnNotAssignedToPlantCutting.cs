using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003040 RID: 12352
	public class Alert_ConnectedPawnNotAssignedToPlantCutting : Alert
	{
		// Token: 0x060115BD RID: 71101 RVA: 0x004FAE51 File Offset: 0x004F9051
		public Alert_ConnectedPawnNotAssignedToPlantCutting()
		{
			this.defaultLabel = "AlertConnectedPawnNotAssignedToPlantCuttingLabel".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x060115BE RID: 71102 RVA: 0x004FAE80 File Offset: 0x004F9080
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x060115BF RID: 71103 RVA: 0x004FAE94 File Offset: 0x004F9094
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
						if (compTreeConnection != null && compTreeConnection.Connected && compTreeConnection.DesiredConnectionStrength > 0f && compTreeConnection.ConnectedPawn.workSettings.GetPriority(WorkTypeDefOf.PlantCutting) == 0)
						{
							this.targets.Add(compTreeConnection.ConnectedPawn);
						}
					}
				}
			}
		}

		// Token: 0x060115C0 RID: 71104 RVA: 0x004FAF54 File Offset: 0x004F9154
		public override TaggedString GetExplanation()
		{
			return "AlertConnectedPawnNotAssignedToPlantCuttingDesc".Translate() + ":\n" + (from x in this.targets
			select ((Pawn)((Thing)x)).NameFullColored.Resolve()).ToLineList("  - ", true);
		}

		// Token: 0x0400B3C3 RID: 46019
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
