using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011B3 RID: 4531
	public class JobDriver_Sacrifice : JobDriver
	{
		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06006ED1 RID: 28369 RVA: 0x00228924 File Offset: 0x00226B24
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06006ED2 RID: 28370 RVA: 0x0022894A File Offset: 0x00226B4A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006ED3 RID: 28371 RVA: 0x0022896D File Offset: 0x00226B6D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Sacrifice"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Pawn victim = this.Victim;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Toil execute = ToilMaker.MakeToil("MakeNewToils");
			execute.initAction = delegate()
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.pawnsDeathIgnored.Add(victim);
					}
				}
				ExecutionUtility.DoExecutionByCut(this.pawn, victim, 0, false);
				ThoughtUtility.GiveThoughtsForPawnExecuted(victim, this.pawn, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					victim
				});
				victim.health.killedByRitual = true;
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_Reserve.Release(TargetIndex.A);
			yield return execute;
			yield break;
		}

		// Token: 0x04004D4D RID: 19789
		private const TargetIndex VictimIndex = TargetIndex.A;

		// Token: 0x04004D4E RID: 19790
		private const TargetIndex StandingIndex = TargetIndex.B;
	}
}
