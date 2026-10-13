using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001724 RID: 5924
	public class LordToil_SightstealerAssault : LordToil
	{
		// Token: 0x06008B73 RID: 35699 RVA: 0x00298C24 File Offset: 0x00296E24
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.SightstealerAssault);
			}
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x00298C88 File Offset: 0x00296E88
		public override void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (!victim.HostileTo(dinfo.Instigator))
			{
				return;
			}
			this.lord.Notify_PawnLost(victim, PawnLostCondition.LeftVoluntarily, null);
			victim.mindState.enemyTarget = dinfo.Instigator;
			victim.mindState.Notify_EngagedTarget();
			victim.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}
	}
}
