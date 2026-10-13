using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001371 RID: 4977
	public class JobDriver_Execute : JobDriver
	{
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06007A1D RID: 31261 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x0024D3FF File Offset: 0x0024B5FF
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x0024D422 File Offset: 0x0024B622
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
			Toil execute = ToilMaker.MakeToil("MakeNewToils");
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim, 8, true);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, execute.actor, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					this.Victim
				});
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			execute.activeSkill = (() => SkillDefOf.Melee);
			yield return execute;
			yield break;
		}
	}
}
