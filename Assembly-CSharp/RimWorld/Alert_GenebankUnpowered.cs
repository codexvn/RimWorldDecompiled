using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003054 RID: 12372
	public class Alert_GenebankUnpowered : Alert
	{
		// Token: 0x06011611 RID: 71185 RVA: 0x004FC49C File Offset: 0x004FA69C
		public Alert_GenebankUnpowered()
		{
			this.defaultLabel = "AlertGenebankUnpowered".Translate();
			this.defaultExplanation = "AlertGenebankUnpoweredDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x06011612 RID: 71186 RVA: 0x004FC4EB File Offset: 0x004FA6EB
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06011613 RID: 71187 RVA: 0x004FC500 File Offset: 0x004FA700
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GeneBank))
				{
					CompGenepackContainer compGenepackContainer = building.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null && compGenepackContainer.innerContainer.Any<Thing>() && !compGenepackContainer.PowerOn)
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x0400B3EA RID: 46058
		public List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
