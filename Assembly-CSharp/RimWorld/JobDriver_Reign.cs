using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200139E RID: 5022
	public class JobDriver_Reign : JobDriver_Meditate
	{
		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06007B2B RID: 31531 RVA: 0x00251463 File Offset: 0x0024F663
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		// Token: 0x06007B2C RID: 31532 RVA: 0x00251470 File Offset: 0x0024F670
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x00251494 File Offset: 0x0024F694
		public override string GetReport()
		{
			return string.Concat(new string[]
			{
				this.ReportStringProcessed(this.job.def.reportString),
				": ",
				this.Throne.LabelShort.CapitalizeFirst(),
				".",
				base.PsyfocusPerDayReport()
			});
		}

		// Token: 0x06007B2E RID: 31534 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x002514F1 File Offset: 0x0024F6F1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, this.Throne.InteractionCell + this.Throne.Rotation.FacingCell);
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_All)) != null);
			toil.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.tickAction = delegate()
			{
				if (this.pawn.mindState.applyThroneThoughtsTick == 0)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 10000;
				}
				else if (this.pawn.mindState.applyThroneThoughtsTick <= Find.TickManager.TicksGame)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 60000;
					ThoughtDef thoughtDef = null;
					if (this.Throne.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.ThroneRoom)
					{
						thoughtDef = ThoughtDefOf.ReignedInThroneroom;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(this.Throne.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
						}
					}
				}
				this.rotateToFace = TargetIndex.B;
				base.MeditationTick();
			};
			yield return toil;
			yield break;
		}

		// Token: 0x04005344 RID: 21316
		protected const TargetIndex FacingInd = TargetIndex.B;

		// Token: 0x04005345 RID: 21317
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
