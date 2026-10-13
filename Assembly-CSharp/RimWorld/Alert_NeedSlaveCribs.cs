using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FFA RID: 12282
	public class Alert_NeedSlaveCribs : Alert
	{
		// Token: 0x060114B0 RID: 70832 RVA: 0x004F4F6C File Offset: 0x004F316C
		public Alert_NeedSlaveCribs()
		{
			this.defaultLabel = "AlertNeedSlaveCribs".Translate();
			this.defaultExplanation = "AlertNeedSlaveCribsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
			this.requireBiotech = true;
		}

		// Token: 0x060114B1 RID: 70833 RVA: 0x004F4FC0 File Offset: 0x004F31C0
		public override AlertReport GetReport()
		{
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					bool flag;
					bool flag2;
					Alert_NeedSlaveBeds.CheckSlaveBeds(map, out flag, out flag2);
					if (!flag2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
