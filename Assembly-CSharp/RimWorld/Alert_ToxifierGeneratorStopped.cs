using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200304F RID: 12367
	public class Alert_ToxifierGeneratorStopped : Alert
	{
		// Token: 0x060115FE RID: 71166 RVA: 0x004FC09B File Offset: 0x004FA29B
		public Alert_ToxifierGeneratorStopped()
		{
			this.requireBiotech = true;
		}

		// Token: 0x060115FF RID: 71167 RVA: 0x004FC0B8 File Offset: 0x004FA2B8
		public override string GetLabel()
		{
			string result;
			if ((result = this.defaultLabel) == null)
			{
				result = (this.defaultLabel = "AlertToxifierGeneratorStopped".Translate(ThingDefOf.ToxifierGenerator.LabelCap));
			}
			return result;
		}

		// Token: 0x06011600 RID: 71168 RVA: 0x004FC0F8 File Offset: 0x004FA2F8
		public override TaggedString GetExplanation()
		{
			string str;
			if ((str = this.defaultExplanation) == null)
			{
				str = (this.defaultExplanation = "AlertToxifierGeneratorStoppedDesc".Translate(ThingDefOf.ToxifierGenerator.label));
			}
			return str;
		}

		// Token: 0x06011601 RID: 71169 RVA: 0x004FC13C File Offset: 0x004FA33C
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.ToxifierGenerator))
				{
					if (!building.TryGetComp<CompToxifier>().CanPolluteNow)
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x06011602 RID: 71170 RVA: 0x004FC1D8 File Offset: 0x004FA3D8
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3E3 RID: 46051
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
