using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001710 RID: 5904
	public class LordToil_PanicFlee : LordToil
	{
		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06008B06 RID: 35590 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x06008B07 RID: 35591 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008B08 RID: 35592 RVA: 0x00296F74 File Offset: 0x00295174
		public override void Init()
		{
			base.Init();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.InAggroMentalState && (!this.HasFleeingDuty(pawn) || pawn.mindState.duty.def == DutyDefOf.ExitMapRandom))
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x00296FFC File Offset: 0x002951FC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				}
			}
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x00297054 File Offset: 0x00295254
		private bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty != null && (pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap);
		}
	}
}
