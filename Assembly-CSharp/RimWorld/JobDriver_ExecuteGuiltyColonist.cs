using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200126C RID: 4716
	public class JobDriver_ExecuteGuiltyColonist : JobDriver
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x0600735D RID: 29533 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600735E RID: 29534 RVA: 0x00237637 File Offset: 0x00235837
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x0023765A File Offset: 0x0023585A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoGuiltyColonist(this.pawn, this.Victim).FailOn(() => !this.Victim.IsColonist || !this.Victim.guilt.IsGuilty || !this.Victim.guilt.awaitingExecution);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Pawn victim = this.Victim;
				ExecutionUtility.DoExecutionByCut(this.pawn, victim, 8, true);
				ThoughtUtility.GiveThoughtsForPawnExecuted(victim, this.pawn, PawnExecutionKind.GenericBrutal);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.activeSkill = (() => SkillDefOf.Melee);
			yield return toil;
			yield break;
		}
	}
}
