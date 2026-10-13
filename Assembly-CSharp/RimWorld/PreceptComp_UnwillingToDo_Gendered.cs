using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002646 RID: 9798
	public class PreceptComp_UnwillingToDo_Gendered : PreceptComp_UnwillingToDo
	{
		// Token: 0x0600D6AE RID: 54958 RVA: 0x003FAE1C File Offset: 0x003F901C
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			Pawn pawn;
			return !ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn) || pawn.gender != this.gender || base.MemberWillingToDo(ev);
		}

		// Token: 0x0600D6AF RID: 54959 RVA: 0x003FAE55 File Offset: 0x003F9055
		public override string GetProhibitionText()
		{
			if (!this.displayDescription)
			{
				return null;
			}
			return base.GetProhibitionText() + " (" + this.gender.GetLabel(false) + ")";
		}

		// Token: 0x040091E8 RID: 37352
		public Gender gender;

		// Token: 0x040091E9 RID: 37353
		public bool displayDescription = true;
	}
}
