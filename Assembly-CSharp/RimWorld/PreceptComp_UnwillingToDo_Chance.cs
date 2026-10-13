using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002647 RID: 9799
	public class PreceptComp_UnwillingToDo_Chance : PreceptComp_UnwillingToDo
	{
		// Token: 0x0600D6B1 RID: 54961 RVA: 0x003FAE91 File Offset: 0x003F9091
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			return Rand.Value >= this.chance || base.MemberWillingToDo(ev);
		}

		// Token: 0x0600D6B2 RID: 54962 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string GetProhibitionText()
		{
			return null;
		}

		// Token: 0x040091EA RID: 37354
		public float chance;
	}
}
