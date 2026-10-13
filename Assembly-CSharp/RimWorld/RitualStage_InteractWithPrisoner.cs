using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B7 RID: 9911
	public class RitualStage_InteractWithPrisoner : RitualStage
	{
		// Token: 0x0600D957 RID: 55639 RVA: 0x00405AD2 File Offset: 0x00403CD2
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.IsPrisonerOfColony);
		}
	}
}
