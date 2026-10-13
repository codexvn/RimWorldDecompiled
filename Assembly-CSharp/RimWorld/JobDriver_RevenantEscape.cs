using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200130A RID: 4874
	public class JobDriver_RevenantEscape : JobDriver
	{
		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06007798 RID: 30616 RVA: 0x00244A3E File Offset: 0x00242C3E
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x001DDCB7 File Offset: 0x001DBEB7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x002452D3 File Offset: 0x002434D3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Comp.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (Rand.MTBEventOccurs((float)JobDriver_RevenantEscape.SmearMTBTicks, 1f, (float)delta))
				{
					RevenantUtility.CreateRevenantSmear(this.pawn);
				}
				if (Rand.MTBEventOccurs((float)JobDriver_RevenantEscape.EscapedCheckInterval, 1f, (float)delta) && Find.TickManager.TicksGame > this.pawn.mindState.lastEngageTargetTick + JobDriver_RevenantEscape.MinEscapeTime && RevenantUtility.NearbyHumanlikePawnCount(this.pawn.Position, this.pawn.Map, 20f) == 0)
				{
					base.ReadyForNextToil();
				}
			}));
			yield return toil;
			Toil toil3 = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil4 = toil3;
			toil4.initAction = (Action)Delegate.Combine(toil4.initAction, new Action(delegate()
			{
				this.Comp.escapeSecondStageStartedTick = Find.TickManager.TicksGame;
			}));
			Toil toil5 = toil3;
			toil5.tickIntervalAction = (Action<int>)Delegate.Combine(toil5.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				AnalysisDetails analysisDetails;
				if (Find.AnalysisManager.TryGetAnalysisProgress(this.Comp.biosignature, out analysisDetails) && (analysisDetails.Satisfied || analysisDetails.timesDone >= 1) && Find.TickManager.TicksGame < this.Comp.escapeSecondStageStartedTick + JobDriver_RevenantEscape.SmearLeavingDuration && Rand.MTBEventOccurs((float)JobDriver_RevenantEscape.SmearMTBTicks, 1f, (float)delta))
				{
					RevenantUtility.CreateRevenantSmear(this.pawn);
				}
				if (Find.TickManager.TicksGame > this.Comp.escapeSecondStageStartedTick + JobDriver_RevenantEscape.MinEscapeTime * 2 && RevenantUtility.NearbyHumanlikePawnCount(this.pawn.Position, this.pawn.Map, 20f) == 0)
				{
					this.pawn.TryGetComp<CompRevenant>().revenantState = RevenantState.Sleep;
					base.EndJobWith(JobCondition.Succeeded);
				}
			}));
			yield return toil3;
			yield break;
		}

		// Token: 0x0400517C RID: 20860
		private static int MinEscapeTime = 300;

		// Token: 0x0400517D RID: 20861
		private static int EscapedCheckInterval = 120;

		// Token: 0x0400517E RID: 20862
		private static int SmearMTBTicks = 60;

		// Token: 0x0400517F RID: 20863
		private static readonly int SmearLeavingDuration = Mathf.FloorToInt((float)JobDriver_RevenantEscape.MinEscapeTime * 1.4f);
	}
}
