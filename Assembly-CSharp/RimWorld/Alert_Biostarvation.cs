using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003005 RID: 12293
	public class Alert_Biostarvation : Alert
	{
		// Token: 0x17002CD8 RID: 11480
		// (get) Token: 0x060114DB RID: 70875 RVA: 0x004F5E84 File Offset: 0x004F4084
		public List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Building_GrowthVat building_GrowthVat in maps[i].listerBuildings.AllBuildingsColonistOfClass<Building_GrowthVat>())
					{
						if (building_GrowthVat.Working && building_GrowthVat.BiostarvationDailyOffset > 0f)
						{
							this.targets.Add(building_GrowthVat);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x060114DC RID: 70876 RVA: 0x004F5F24 File Offset: 0x004F4124
		public Alert_Biostarvation()
		{
			this.defaultLabel = "Biostarvation".Translate().CapitalizeFirst();
			this.defaultExplanation = "BiostarvationExplanation".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x060114DD RID: 70877 RVA: 0x004F5F82 File Offset: 0x004F4182
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400B359 RID: 45913
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
