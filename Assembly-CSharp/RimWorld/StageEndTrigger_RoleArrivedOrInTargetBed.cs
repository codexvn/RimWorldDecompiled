using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200269C RID: 9884
	public class StageEndTrigger_RoleArrivedOrInTargetBed : StageEndTrigger_RolesArrived
	{
		// Token: 0x0600D904 RID: 55556 RVA: 0x00404B70 File Offset: 0x00402D70
		protected override bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.PawnWithRole(r);
			return (pawn != null && pawn.CurrentBed() == ritual.selectedTarget.Thing) || base.ArrivedCheck(r, ritual);
		}
	}
}
