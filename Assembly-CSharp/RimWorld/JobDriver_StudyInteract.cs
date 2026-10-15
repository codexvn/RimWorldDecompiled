using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_StudyInteract : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_StudyInteract _003C_003E4__this;

		private bool _003CtargetIsPawn_003E5__2;

		private Toil _003CfindAdjacentCell_003E5__3;

		private Toil _003CgoToAdjacentCell_003E5__4;

		private Toil _003CstudyToil_003E5__5;

		private Toil _003CfinishInteraction_003E5__6;

		private int _003Cinteractions_003E5__7;

		private int _003Ci_003E5__8;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CfindAdjacentCell_003E5__3 = null;
			_003CgoToAdjacentCell_003E5__4 = null;
			_003CstudyToil_003E5__5 = null;
			_003CfinishInteraction_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_StudyInteract CS_0024_003C_003E8__locals32 = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003CtargetIsPawn_003E5__2 = CS_0024_003C_003E8__locals32.TargetThingA is Pawn;
				CS_0024_003C_003E8__locals32.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals32.FailOn(() => (CS_0024_003C_003E8__locals32.StudyComp == null || !CS_0024_003C_003E8__locals32.StudyComp.CurrentlyStudiable()) ? true : false);
				CS_0024_003C_003E8__locals32.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals32.StudyComp != null)
					{
						CS_0024_003C_003E8__locals32.StudyComp.studyInteractions += CS_0024_003C_003E8__locals32.studyInteractions;
					}
				});
				StatDef stat = ((CS_0024_003C_003E8__locals32.Platform != null) ? StatDefOf.EntityStudyRate : StatDefOf.ResearchSpeed);
				int num2 = Mathf.RoundToInt(600f / CS_0024_003C_003E8__locals32.pawn.GetStatValue(stat));
				if (_003CtargetIsPawn_003E5__2 || CS_0024_003C_003E8__locals32.Platform != null)
				{
					num2 *= 2;
				}
				_003CfindAdjacentCell_003E5__3 = Toils_General.Do(delegate
				{
					IntVec3 adjacentInteractionCell = SocialInteractionUtility.GetAdjacentInteractionCell(CS_0024_003C_003E8__locals32.pawn, CS_0024_003C_003E8__locals32.job.GetTarget(TargetIndex.A).Thing, CS_0024_003C_003E8__locals32.job.playerForced);
					CS_0024_003C_003E8__locals32.pawn.Map.pawnDestinationReservationManager.Reserve(CS_0024_003C_003E8__locals32.pawn, CS_0024_003C_003E8__locals32.job, adjacentInteractionCell);
					CS_0024_003C_003E8__locals32.job.targetB = adjacentInteractionCell;
				});
				_003CgoToAdjacentCell_003E5__4 = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				if (_003CtargetIsPawn_003E5__2)
				{
					_003CstudyToil_003E5__5 = CS_0024_003C_003E8__locals32.StudyPawn(TargetIndex.A, num2);
				}
				else
				{
					_003CstudyToil_003E5__5 = Toils_General.WaitWith(TargetIndex.A, num2, useProgressBar: true, maintainPosture: false, maintainSleep: false, TargetIndex.A);
					_003CstudyToil_003E5__5.AddPreTickIntervalAction(delegate(int delta)
					{
						CS_0024_003C_003E8__locals32.ThingToStudy.TryGetComp<CompObelisk>()?.Notify_InteractedTick(CS_0024_003C_003E8__locals32.pawn, delta);
					});
				}
				_003CstudyToil_003E5__5.AddPreTickIntervalAction(delegate(int delta)
				{
					CS_0024_003C_003E8__locals32.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta);
				});
				_003CstudyToil_003E5__5.activeSkill = () => SkillDefOf.Intellectual;
				if (CS_0024_003C_003E8__locals32.StudyComp.KnowledgeCategory != null)
				{
					_003CstudyToil_003E5__5.WithEffect(() => EffecterDefOf.StudyHoraxian, CS_0024_003C_003E8__locals32.TargetThingA, null);
				}
				_003CfinishInteraction_003E5__6 = ToilMaker.MakeToil("Interaction finish");
				_003CfinishInteraction_003E5__6.initAction = CS_0024_003C_003E8__locals32.DoStudyInteraction;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Cinteractions_003E5__7 = 5 - CS_0024_003C_003E8__locals32.StudyComp.studyInteractions;
				_003Ci_003E5__8 = 0;
				goto IL_02a2;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToAdjacentCell_003E5__4;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0258;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CfinishInteraction_003E5__6;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003Ci_003E5__8++;
				goto IL_02a2;
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_02a2:
				if (_003Ci_003E5__8 < _003Cinteractions_003E5__7)
				{
					if (!_003CtargetIsPawn_003E5__2 && _003Ci_003E5__8 > 0)
					{
						_003C_003E2__current = _003CfindAdjacentCell_003E5__3;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0258;
				}
				toil = ToilMaker.MakeToil("Study finish");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals32.studyInteractions = 0;
					CS_0024_003C_003E8__locals32.StudyComp.studyInteractions = 0;
					CS_0024_003C_003E8__locals32.StudyComp.lastStudiedTick = Find.TickManager.TicksGame;
					if (ModsConfig.AnomalyActive && CS_0024_003C_003E8__locals32.ThingToStudy is Pawn pawn && (!pawn.RaceProps.Humanlike || pawn.IsMutant))
					{
						TaleRecorder.RecordTale(TaleDefOf.StudiedEntity, CS_0024_003C_003E8__locals32.pawn, pawn);
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 6;
				return true;
				IL_0258:
				_003C_003E2__current = _003CstudyToil_003E5__5;
				_003C_003E1__state = 4;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private const TargetIndex ThingToStudyIndex = TargetIndex.A;

	private const TargetIndex AdjacentCellIndex = TargetIndex.B;

	private const int BaseDurationTicks = 600;

	private const int PawnTargetStudyDurationFactor = 2;

	public const float ElectroharvesterFactor = 0.5f;

	private int studyInteractions;

	private Building_HoldingPlatform Platform => base.TargetThingA as Building_HoldingPlatform;

	private Thing ThingToStudy => Platform?.HeldPawn ?? base.TargetThingA;

	private CompStudiable StudyComp => ThingToStudy?.TryGetComp<CompStudiable>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (Platform != null)
		{
			if (pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed))
			{
				return pawn.Reserve(ThingToStudy, job, 1, -1, null, errorOnFailed);
			}
			return false;
		}
		return pawn.Reserve(base.TargetThingA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil StudyPawn(TargetIndex pawnIndex, int duration)
	{
		Toil study = ToilMaker.MakeToil("Study pawn");
		study.initAction = delegate
		{
			Pawn actor = study.actor;
			actor.pather.StopDead();
			if (actor.CurJob.GetTarget(pawnIndex).Thing is Pawn recipient)
			{
				PawnUtility.ForceWait(recipient, duration, study.actor);
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.PrisonerStudyAnomaly);
			}
		};
		study.tickIntervalAction = delegate
		{
			study.actor.rotationTracker.FaceTarget(study.actor.CurJob.GetTarget(pawnIndex));
		};
		study.handlingFacing = true;
		study.defaultCompleteMode = ToilCompleteMode.Delay;
		study.defaultDuration = duration;
		study.WithProgressBarToilDelay(pawnIndex);
		return study;
	}

	private void DoStudyInteraction()
	{
		float anomalyKnowledgeAmount = 0f;
		if (ModsConfig.AnomalyActive)
		{
			anomalyKnowledgeAmount = StudyComp.AdjustedAnomalyKnowledgePerStudy * base.pawn.GetStatValue(StatDefOf.StudyEfficiency);
		}
		StudyComp.Study(base.pawn, 0.87f, anomalyKnowledgeAmount);
		studyInteractions++;
		if (ModsConfig.AnomalyActive && ThingToStudy is Pawn pawn)
		{
			pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
			pawn.mindState.interactionsToday++;
		}
	}

	public override bool? IsSameJobAs(Job j)
	{
		return j.targetA == base.TargetThingA;
	}

	protected override string ReportStringProcessed(string str)
	{
		return JobUtility.GetResolvedJobReport(str, ThingToStudy);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref studyInteractions, "studyInteractions", 0);
	}
}
