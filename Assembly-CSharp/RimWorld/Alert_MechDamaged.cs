using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003052 RID: 12370
	public class Alert_MechDamaged : Alert
	{
		// Token: 0x0601160A RID: 71178 RVA: 0x004FC354 File Offset: 0x004FA554
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonyMech && MechRepairUtility.CanRepair(list[j]))
					{
						this.targets.Add(list[j]);
					}
				}
			}
		}

		// Token: 0x0601160B RID: 71179 RVA: 0x004FC3D9 File Offset: 0x004FA5D9
		public Alert_MechDamaged()
		{
			this.defaultLabel = "AlertMechNeedsRepair".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0601160C RID: 71180 RVA: 0x004FC408 File Offset: 0x004FA608
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0601160D RID: 71181 RVA: 0x004FC41C File Offset: 0x004FA61C
		public override TaggedString GetExplanation()
		{
			return "AlertMechNeedsRepairDescPrefix".Translate() + ":\n" + (from p in this.targets
			select p.LabelCap).ToLineList("  - ", false) + "\n\n" + "AlertMechNeedsRepairDesc".Translate();
		}

		// Token: 0x0400B3E7 RID: 46055
		private List<Pawn> targets = new List<Pawn>();
	}
}
