using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PredatorHunt : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public JobDriver_PredatorHunt _003C_003E4__this;

		public Toil prepareToEatCorpse;

		internal void _003CMakeNewToils_003Eb__0(JobCondition jobCondition)
		{
			_003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E4__this.pawn);
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Pawn actor = prepareToEatCorpse.actor;
			Corpse corpse = _003C_003E4__this.Corpse;
			if (corpse == null)
			{
				Pawn prey = _003C_003E4__this.Prey;
				if (prey == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable);
					return;
				}
				corpse = prey.Corpse;
				if (corpse == null || !corpse.Spawned)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable);
					return;
				}
			}
			if (actor.Faction == Faction.OfPlayer)
			{
				corpse.SetForbidden(value: false, warnOnFail: false);
			}
			else
			{
				corpse.SetForbidden(value: true, warnOnFail: false);
			}
			actor.CurJob.SetTarget(TargetIndex.A, corpse);
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			_003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E4__this.pawn);
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			Pawn prey = _003C_003E4__this.Prey;
			bool surpriseAttack = _003C_003E4__this.firstHit && !prey.IsColonist;
			if (_003C_003E4__this.pawn.meleeVerbs.TryMeleeAttack(prey, _003C_003E4__this.job.verbToUse, surpriseAttack))
			{
				if (!_003C_003E4__this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
				{
					_003C_003E4__this.notifiedPlayerAttacked = true;
					Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, _003C_003E4__this.pawn.LabelIndefinite(), prey.Named("PREY"), _003C_003E4__this.pawn.Named("PREDATOR")).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall);
				}
				_003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E4__this.pawn);
				_003C_003E4__this.firstHit = false;
			}
		}

		internal bool _003CMakeNewToils_003Eb__4()
		{
			return _003C_003E4__this.Corpse != null;
		}

		internal bool _003CMakeNewToils_003Eb__5()
		{
			if (Find.TickManager.TicksGame > _003C_003E4__this.startTick + 5000)
			{
				return (float)(_003C_003E4__this.job.GetTarget(TargetIndex.A).Cell - _003C_003E4__this.pawn.Position).LengthHorizontalSquared > 4f;
			}
			return false;
		}

		internal bool _003CMakeNewToils_003Eb__6()
		{
			return _003C_003E4__this.pawn.needs.food.CurLevelPercentage < 0.9f;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PredatorHunt _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		private Toil _003CgotoCorpse_003E5__2;

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
			_003C_003E8__1 = null;
			_003CgotoCorpse_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PredatorHunt jobDriver_PredatorHunt = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_PredatorHunt.AddFinishAction(delegate
				{
					_003C_003E8__1._003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E8__1._003C_003E4__this.pawn);
				});
				_003C_003E8__1.prepareToEatCorpse = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.prepareToEatCorpse.initAction = delegate
				{
					Pawn actor = _003C_003E8__1.prepareToEatCorpse.actor;
					Corpse corpse = _003C_003E8__1._003C_003E4__this.Corpse;
					if (corpse == null)
					{
						Pawn prey2 = _003C_003E8__1._003C_003E4__this.Prey;
						if (prey2 == null)
						{
							actor.jobs.EndCurrentJob(JobCondition.Incompletable);
							return;
						}
						corpse = prey2.Corpse;
						if (corpse == null || !corpse.Spawned)
						{
							actor.jobs.EndCurrentJob(JobCondition.Incompletable);
							return;
						}
					}
					if (actor.Faction == Faction.OfPlayer)
					{
						corpse.SetForbidden(value: false, warnOnFail: false);
					}
					else
					{
						corpse.SetForbidden(value: true, warnOnFail: false);
					}
					actor.CurJob.SetTarget(TargetIndex.A, corpse);
				};
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					_003C_003E8__1._003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E8__1._003C_003E4__this.pawn);
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Action hitAction = delegate
				{
					Pawn prey = _003C_003E8__1._003C_003E4__this.Prey;
					bool surpriseAttack = _003C_003E8__1._003C_003E4__this.firstHit && !prey.IsColonist;
					if (_003C_003E8__1._003C_003E4__this.pawn.meleeVerbs.TryMeleeAttack(prey, _003C_003E8__1._003C_003E4__this.job.verbToUse, surpriseAttack))
					{
						if (!_003C_003E8__1._003C_003E4__this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
						{
							_003C_003E8__1._003C_003E4__this.notifiedPlayerAttacked = true;
							Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, _003C_003E8__1._003C_003E4__this.pawn.LabelIndefinite(), prey.Named("PREY"), _003C_003E8__1._003C_003E4__this.pawn.Named("PREDATOR")).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall);
						}
						_003C_003E8__1._003C_003E4__this.Map.attackTargetsCache.UpdateTarget(_003C_003E8__1._003C_003E4__this.pawn);
						_003C_003E8__1._003C_003E4__this.firstHit = false;
					}
				};
				Toil toil = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, hitAction).JumpIfDespawnedOrNull(TargetIndex.A, _003C_003E8__1.prepareToEatCorpse).JumpIf(() => _003C_003E8__1._003C_003E4__this.Corpse != null, _003C_003E8__1.prepareToEatCorpse)
					.FailOn(() => Find.TickManager.TicksGame > _003C_003E8__1._003C_003E4__this.startTick + 5000 && (float)(_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Cell - _003C_003E8__1._003C_003E4__this.pawn.Position).LengthHorizontalSquared > 4f);
				toil.AddPreTickIntervalAction(jobDriver_PredatorHunt.CheckWarnPlayerInterval);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003C_003E8__1.prepareToEatCorpse;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003CgotoCorpse_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E2__current = _003CgotoCorpse_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				float durationMultiplier = 1f / jobDriver_PredatorHunt.pawn.GetStatValue(StatDefOf.EatingSpeed);
				_003C_003E2__current = Toils_Ingest.ChewIngestible(jobDriver_PredatorHunt.pawn, durationMultiplier, TargetIndex.A).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.FinalizeIngest(jobDriver_PredatorHunt.pawn, TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgotoCorpse_003E5__2, () => _003C_003E8__1._003C_003E4__this.pawn.needs.food.CurLevelPercentage < 0.9f);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				return false;
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

	private bool notifiedPlayerAttacked;

	private bool notifiedPlayerAttacking;

	private bool firstHit = true;

	public const TargetIndex PreyInd = TargetIndex.A;

	private const TargetIndex CorpseInd = TargetIndex.A;

	private const int MaxHuntTicks = 5000;

	public Pawn Prey
	{
		get
		{
			Corpse corpse = Corpse;
			if (corpse != null)
			{
				return corpse.InnerPawn;
			}
			return (Pawn)job.GetTarget(TargetIndex.A).Thing;
		}
	}

	private Corpse Corpse => job.GetTarget(TargetIndex.A).Thing as Corpse;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref firstHit, "firstHit", defaultValue: false);
		Scribe_Values.Look(ref notifiedPlayerAttacking, "notifiedPlayerAttacking", defaultValue: false);
	}

	public override string GetReport()
	{
		if (Corpse != null)
		{
			return ReportStringProcessed(JobDefOf.Ingest.reportString);
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
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

	public override void Notify_DamageTaken(DamageInfo dinfo)
	{
		base.Notify_DamageTaken(dinfo);
		if (dinfo.Def.ExternalViolenceFor(pawn) && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != Prey && !pawn.InMentalState && !pawn.Downed)
		{
			pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
		}
	}

	private void CheckWarnPlayerInterval(int delta)
	{
		if (notifiedPlayerAttacking)
		{
			return;
		}
		Pawn prey = Prey;
		if (prey.Spawned && prey.Faction == Faction.OfPlayer && Find.TickManager.TicksGame > pawn.mindState.lastPredatorHuntingPlayerNotificationTick + 2500 && prey.Position.InHorDistOf(pawn.Position, 60f))
		{
			if (prey.RaceProps.Humanlike)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPredatorHuntingColonist".Translate(pawn.LabelShort, prey.LabelDefinite(), pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), "LetterPredatorHuntingColonist".Translate(pawn.LabelIndefinite(), prey.LabelDefinite(), pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), LetterDefOf.ThreatBig, pawn);
			}
			else
			{
				Messages.Message((prey.Name.Numerical ? "LetterPredatorHuntingColonist" : "MessagePredatorHuntingPlayerAnimal").Translate(pawn.Named("PREDATOR"), prey.Named("PREY")), pawn, MessageTypeDefOf.ThreatBig);
			}
			pawn.mindState.Notify_PredatorHuntingPlayerNotification();
			notifiedPlayerAttacking = true;
		}
	}
}
