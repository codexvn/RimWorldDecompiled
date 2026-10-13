using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013B8 RID: 5048
	public class JobDriver_ExecuteSlave : JobDriver
	{
		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06007B92 RID: 31634 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007B93 RID: 31635 RVA: 0x00253F0A File Offset: 0x0025210A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007B94 RID: 31636 RVA: 0x00253F2D File Offset: 0x0025212D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Execute slave"))
			{
				yield break;
			}
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Interpersonal.GotoSlave(this.pawn, this.Victim).FailOn(() => !this.Victim.IsSlaveOfColony);
			Toil execute = ToilMaker.MakeToil("MakeNewToils");
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim, 8, true);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, execute.actor, PawnExecutionKind.GenericBrutal);
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			execute.activeSkill = (() => SkillDefOf.Melee);
			yield return execute;
			yield break;
		}
	}
}
