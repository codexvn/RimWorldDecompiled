using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200131B RID: 4891
	public class JobDriver_StudyInteract : JobDriver
	{
		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x0600780C RID: 30732 RVA: 0x0023071F File Offset: 0x0022E91F
		private Building_HoldingPlatform Platform
		{
			get
			{
				return base.TargetThingA as Building_HoldingPlatform;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x0600780D RID: 30733 RVA: 0x00246583 File Offset: 0x00244783
		private Thing ThingToStudy
		{
			get
			{
				Building_HoldingPlatform platform = this.Platform;
				return ((platform != null) ? platform.HeldPawn : null) ?? base.TargetThingA;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x0600780E RID: 30734 RVA: 0x002465A1 File Offset: 0x002447A1
		private CompStudiable StudyComp
		{
			get
			{
				Thing thingToStudy = this.ThingToStudy;
				if (thingToStudy == null)
				{
					return null;
				}
				return thingToStudy.TryGetComp<CompStudiable>();
			}
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x002465B4 File Offset: 0x002447B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Platform != null)
			{
				return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.ThingToStudy, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(base.TargetThingA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x00246631 File Offset: 0x00244831
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool targetIsPawn = base.TargetThingA is Pawn;
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.StudyComp == null || !this.StudyComp.CurrentlyStudiable());
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (this.StudyComp != null)
				{
					this.StudyComp.studyInteractions += this.studyInteractions;
				}
			});
			StatDef stat = (this.Platform != null) ? StatDefOf.EntityStudyRate : StatDefOf.ResearchSpeed;
			int num = Mathf.RoundToInt(600f / this.pawn.GetStatValue(stat, true, -1));
			if (targetIsPawn || this.Platform != null)
			{
				num *= 2;
			}
			Toil findAdjacentCell = Toils_General.Do(delegate
			{
				IntVec3 adjacentInteractionCell = SocialInteractionUtility.GetAdjacentInteractionCell(this.pawn, this.job.GetTarget(TargetIndex.A).Thing, this.job.playerForced);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, adjacentInteractionCell);
				this.job.targetB = adjacentInteractionCell;
			});
			Toil goToAdjacentCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil studyToil;
			if (targetIsPawn)
			{
				studyToil = this.StudyPawn(TargetIndex.A, num);
			}
			else
			{
				studyToil = Toils_General.WaitWith(TargetIndex.A, num, true, false, false, TargetIndex.A, PathEndMode.Touch);
				studyToil.AddPreTickIntervalAction(delegate(int delta)
				{
					CompObelisk compObelisk = this.ThingToStudy.TryGetComp<CompObelisk>();
					if (compObelisk == null)
					{
						return;
					}
					compObelisk.Notify_InteractedTick(this.pawn, delta);
				});
			}
			studyToil.AddPreTickIntervalAction(delegate(int delta)
			{
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta, false, false);
			});
			studyToil.activeSkill = (() => SkillDefOf.Intellectual);
			if (this.StudyComp.KnowledgeCategory != null)
			{
				studyToil.WithEffect(() => EffecterDefOf.StudyHoraxian, base.TargetThingA, null);
			}
			Toil finishInteraction = ToilMaker.MakeToil("Interaction finish");
			finishInteraction.initAction = new Action(this.DoStudyInteraction);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			int interactions = 5 - this.StudyComp.studyInteractions;
			int num2;
			for (int i = 0; i < interactions; i = num2 + 1)
			{
				if (!targetIsPawn && i > 0)
				{
					yield return findAdjacentCell;
					yield return goToAdjacentCell;
				}
				yield return studyToil;
				yield return finishInteraction;
				num2 = i;
			}
			Toil toil = ToilMaker.MakeToil("Study finish");
			toil.initAction = delegate()
			{
				this.studyInteractions = 0;
				this.StudyComp.studyInteractions = 0;
				this.StudyComp.lastStudiedTick = Find.TickManager.TicksGame;
				if (ModsConfig.AnomalyActive)
				{
					Pawn pawn = this.ThingToStudy as Pawn;
					if (pawn != null && (!pawn.RaceProps.Humanlike || pawn.IsMutant))
					{
						TaleRecorder.RecordTale(TaleDefOf.StudiedEntity, new object[]
						{
							this.pawn,
							pawn
						});
					}
				}
			};
			yield return toil;
			yield break;
		}

		// Token: 0x06007811 RID: 30737 RVA: 0x00246644 File Offset: 0x00244844
		private Toil StudyPawn(TargetIndex pawnIndex, int duration)
		{
			Toil study = ToilMaker.MakeToil("Study pawn");
			study.initAction = delegate()
			{
				Pawn actor = study.actor;
				actor.pather.StopDead();
				Pawn pawn = actor.CurJob.GetTarget(pawnIndex).Thing as Pawn;
				if (pawn != null)
				{
					PawnUtility.ForceWait(pawn, duration, study.actor, false, false);
					actor.interactions.TryInteractWith(pawn, InteractionDefOf.PrisonerStudyAnomaly);
				}
			};
			study.tickIntervalAction = delegate(int delta)
			{
				study.actor.rotationTracker.FaceTarget(study.actor.CurJob.GetTarget(pawnIndex));
			};
			study.handlingFacing = true;
			study.defaultCompleteMode = ToilCompleteMode.Delay;
			study.defaultDuration = duration;
			study.WithProgressBarToilDelay(pawnIndex, false, -0.5f);
			return study;
		}

		// Token: 0x06007812 RID: 30738 RVA: 0x002466EC File Offset: 0x002448EC
		private void DoStudyInteraction()
		{
			float anomalyKnowledgeAmount = 0f;
			if (ModsConfig.AnomalyActive)
			{
				anomalyKnowledgeAmount = this.StudyComp.AdjustedAnomalyKnowledgePerStudy * this.pawn.GetStatValue(StatDefOf.StudyEfficiency, true, -1);
			}
			this.StudyComp.Study(this.pawn, 0.87f, anomalyKnowledgeAmount);
			this.studyInteractions++;
			if (ModsConfig.AnomalyActive)
			{
				Pawn pawn = this.ThingToStudy as Pawn;
				if (pawn != null)
				{
					pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
					pawn.mindState.interactionsToday++;
				}
			}
		}

		// Token: 0x06007813 RID: 30739 RVA: 0x00246788 File Offset: 0x00244988
		public override bool? IsSameJobAs(Job j)
		{
			return new bool?(j.targetA == base.TargetThingA);
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x002467A5 File Offset: 0x002449A5
		protected override string ReportStringProcessed(string str)
		{
			return JobUtility.GetResolvedJobReport(str, this.ThingToStudy);
		}

		// Token: 0x06007815 RID: 30741 RVA: 0x002467B8 File Offset: 0x002449B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.studyInteractions, "studyInteractions", 0, false);
		}

		// Token: 0x040051AB RID: 20907
		private const TargetIndex ThingToStudyIndex = TargetIndex.A;

		// Token: 0x040051AC RID: 20908
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;

		// Token: 0x040051AD RID: 20909
		private const int BaseDurationTicks = 600;

		// Token: 0x040051AE RID: 20910
		private const int PawnTargetStudyDurationFactor = 2;

		// Token: 0x040051AF RID: 20911
		public const float ElectroharvesterFactor = 0.5f;

		// Token: 0x040051B0 RID: 20912
		private int studyInteractions;
	}
}
