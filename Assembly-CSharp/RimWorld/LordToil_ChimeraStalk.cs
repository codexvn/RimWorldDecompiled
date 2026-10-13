using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016EB RID: 5867
	public class LordToil_ChimeraStalk : LordToil
	{
		// Token: 0x06008A74 RID: 35444 RVA: 0x00294A90 File Offset: 0x00292C90
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = this.lord.ownedPawns[i].mindState.duty;
				if (((duty != null) ? duty.def : null) != DutyDefOf.ChimeraStalkFlee)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ChimeraStalkFlee);
					this.lord.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06008A75 RID: 35445 RVA: 0x00294B30 File Offset: 0x00292D30
		public override void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
			base.Notify_PawnJobDone(p, condition);
			PawnDuty duty = p.mindState.duty;
			if (((duty != null) ? duty.def : null) == DutyDefOf.ChimeraStalkFlee && p.CurJobDef == JobDefOf.Wait_Wander)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.ChimeraStalkWander);
			}
		}
	}
}
