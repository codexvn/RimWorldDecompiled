using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FF8 RID: 12280
	public class Alert_NeedBabyCribs : Alert
	{
		// Token: 0x060114AB RID: 70827 RVA: 0x004F4CC3 File Offset: 0x004F2EC3
		public Alert_NeedBabyCribs()
		{
			this.defaultLabel = "AlertNeedBabyCribs".Translate();
			this.defaultExplanation = "AlertNeedBabyCribsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x060114AC RID: 70828 RVA: 0x004F4D04 File Offset: 0x004F2F04
		public override AlertReport GetReport()
		{
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					int num;
					int num2;
					int num3;
					Alert_NeedColonistBeds.AvailableColonistBeds(map, true, out num, out num2, out num3);
					if (num3 < 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
