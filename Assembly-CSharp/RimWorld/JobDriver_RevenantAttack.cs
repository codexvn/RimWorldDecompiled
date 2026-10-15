using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RevenantAttack : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass15_0
	{
		public JobDriver_RevenantAttack _003C_003E4__this;

		public Toil stalk;

		internal void _003CMakeNewToils_003Eb__0()
		{
			if (!_003C_003E4__this.pawn.stances.FullBodyBusy)
			{
				Pawn pawn = (Pawn)_003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
				JobDriver curDriver = _003C_003E4__this.pawn.jobs.curDriver;
				Find.Anomaly.Hypnotize(pawn, _003C_003E4__this.pawn, LongStunTicks);
				curDriver.ReadyForNextToil();
			}
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			_003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
		}

		internal void _003CMakeNewToils_003Eb__5(int delta)
		{
			Job curJob = stalk.actor.jobs.curJob;
			if (Rand.MTBEventOccurs(180f, 1f, delta))
			{
				curJob.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(_003C_003E4__this.pawn, 10f) ?? curJob.GetTarget(TargetIndex.A).Pawn);
				_003C_003E4__this.pawn.mindState.enemyTarget = curJob.GetTarget(TargetIndex.A).Pawn;
			}
			if (curJob.targetA == null)
			{
				_003C_003E4__this.EndJobWith(JobCondition.Incompletable);
				return;
			}
			Pawn pawn = curJob.targetA.Pawn;
			if (_003C_003E4__this.pawn.Position.InHorDistOf(pawn.Position, _003C_003E4__this.RevealRange))
			{
				if (!_003C_003E4__this.Comp.Invisibility.PsychologicallyVisible)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRevenantRevealed".Translate(), _003C_003E4__this.Comp.everRevealed ? "LetterRevenantRevealed".Translate() : "LetterRevenantRevealedFirst".Translate(), LetterDefOf.ThreatBig, _003C_003E4__this.pawn, null, null, null, null, 60);
				}
				_003C_003E4__this.Comp.Invisibility.BecomeVisible();
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			if (_003C_003E4__this.Comp.Invisibility.PsychologicallyVisible && Rand.MTBEventOccurs(60f, 1f, delta))
			{
				RevenantUtility.CreateRevenantSmear(_003C_003E4__this.pawn);
			}
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Pawn pawn = _003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
			if (pawn == null || !pawn.Spawned || pawn.Map != _003C_003E4__this.pawn.Map)
			{
				if (pawn != null)
				{
					Find.Anomaly.EndHypnotize(pawn);
				}
				_003C_003E4__this.job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(_003C_003E4__this.pawn, 20f));
				_003C_003E4__this.pawn.mindState.enemyTarget = _003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
				if (_003C_003E4__this.pawn.mindState.enemyTarget == null)
				{
					_003C_003E4__this.Comp.revenantState = RevenantState.Wander;
				}
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			_003C_003E4__this.hypnotizeEndTick = Find.TickManager.TicksGame + _003C_003E4__this.HypnotizeDurationTicks;
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			Pawn pawn = _003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
			Find.Anomaly.EndHypnotize(pawn);
			_003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
		}

		internal void _003CMakeNewToils_003Eb__6()
		{
			_003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(_003C_003E4__this.pawn);
			if (_003C_003E4__this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.RevenantHypnotise)
			{
				_003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantHypnotise);
			}
			if (!_003C_003E4__this.pawn.stances.stunner.Stunned && Find.TickManager.TicksGame >= _003C_003E4__this.hypnotizeEndTick)
			{
				Pawn pawn = _003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
				_003C_003E4__this.Comp.Hypnotize(pawn);
				if (_003C_003E4__this.Comp.injuredWhileAttacking)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantFleshChunk);
					thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = _003C_003E4__this.Comp.biosignature;
					Thing thing2 = GenSpawn.Spawn(thing, _003C_003E4__this.pawn.PositionHeld, _003C_003E4__this.pawn.Map);
					Find.LetterStack.ReceiveLetter("LetterRevenantFleshChunkLabel".Translate(), "LetterRevenantFleshChunkText".Translate(), LetterDefOf.NeutralEvent, thing2, null, null, null, null, 600);
					_003C_003E4__this.Comp.injuredWhileAttacking = false;
				}
				_003C_003E4__this.Comp.revenantState = RevenantState.Escape;
				_003C_003E4__this.EndJobWith(JobCondition.Succeeded);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RevenantAttack _003C_003E4__this;

		private _003C_003Ec__DisplayClass15_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RevenantAttack f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass15_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.stalk = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate
				{
					if (!_003C_003E8__1._003C_003E4__this.pawn.stances.FullBodyBusy)
					{
						Pawn pawn3 = (Pawn)_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
						JobDriver curDriver = _003C_003E8__1._003C_003E4__this.pawn.jobs.curDriver;
						Find.Anomaly.Hypnotize(pawn3, _003C_003E8__1._003C_003E4__this.pawn, LongStunTicks);
						curDriver.ReadyForNextToil();
					}
				});
				Toil toil2 = _003C_003E8__1.stalk;
				toil2.initAction = (Action)Delegate.Combine(toil2.initAction, (Action)delegate
				{
					_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
				});
				Toil toil3 = _003C_003E8__1.stalk;
				toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					Job curJob = _003C_003E8__1.stalk.actor.jobs.curJob;
					if (Rand.MTBEventOccurs(180f, 1f, delta))
					{
						curJob.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(_003C_003E8__1._003C_003E4__this.pawn, 10f) ?? curJob.GetTarget(TargetIndex.A).Pawn);
						_003C_003E8__1._003C_003E4__this.pawn.mindState.enemyTarget = curJob.GetTarget(TargetIndex.A).Pawn;
					}
					if (curJob.targetA == null)
					{
						_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						Pawn pawn4 = curJob.targetA.Pawn;
						if (_003C_003E8__1._003C_003E4__this.pawn.Position.InHorDistOf(pawn4.Position, _003C_003E8__1._003C_003E4__this.RevealRange))
						{
							if (!_003C_003E8__1._003C_003E4__this.Comp.Invisibility.PsychologicallyVisible)
							{
								Find.LetterStack.ReceiveLetter("LetterLabelRevenantRevealed".Translate(), _003C_003E8__1._003C_003E4__this.Comp.everRevealed ? "LetterRevenantRevealed".Translate() : "LetterRevenantRevealedFirst".Translate(), LetterDefOf.ThreatBig, _003C_003E8__1._003C_003E4__this.pawn, null, null, null, null, 60);
							}
							_003C_003E8__1._003C_003E4__this.Comp.Invisibility.BecomeVisible();
							Find.TickManager.slower.SignalForceNormalSpeed();
						}
						if (_003C_003E8__1._003C_003E4__this.Comp.Invisibility.PsychologicallyVisible && Rand.MTBEventOccurs(60f, 1f, delta))
						{
							RevenantUtility.CreateRevenantSmear(_003C_003E8__1._003C_003E4__this.pawn);
						}
					}
				});
				_003C_003E8__1.stalk.AddFinishAction(delegate
				{
					Pawn pawn5 = _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
					if (pawn5 == null || !pawn5.Spawned || pawn5.Map != _003C_003E8__1._003C_003E4__this.pawn.Map)
					{
						if (pawn5 != null)
						{
							Find.Anomaly.EndHypnotize(pawn5);
						}
						_003C_003E8__1._003C_003E4__this.job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(_003C_003E8__1._003C_003E4__this.pawn, 20f));
						_003C_003E8__1._003C_003E4__this.pawn.mindState.enemyTarget = _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
						if (_003C_003E8__1._003C_003E4__this.pawn.mindState.enemyTarget == null)
						{
							_003C_003E8__1._003C_003E4__this.Comp.revenantState = RevenantState.Wander;
						}
					}
				});
				_003C_003E2__current = _003C_003E8__1.stalk;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				toil.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.hypnotizeEndTick = Find.TickManager.TicksGame + _003C_003E8__1._003C_003E4__this.HypnotizeDurationTicks;
				};
				toil.AddFinishAction(delegate
				{
					Pawn pawn = _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
					Find.Anomaly.EndHypnotize(pawn);
					_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
				});
				toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
				{
					_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.pawn);
					if (_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.RevenantHypnotise)
					{
						_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantHypnotise);
					}
					if (!_003C_003E8__1._003C_003E4__this.pawn.stances.stunner.Stunned && Find.TickManager.TicksGame >= _003C_003E8__1._003C_003E4__this.hypnotizeEndTick)
					{
						Pawn pawn2 = _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
						_003C_003E8__1._003C_003E4__this.Comp.Hypnotize(pawn2);
						if (_003C_003E8__1._003C_003E4__this.Comp.injuredWhileAttacking)
						{
							Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantFleshChunk);
							thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = _003C_003E8__1._003C_003E4__this.Comp.biosignature;
							Thing thing2 = GenSpawn.Spawn(thing, _003C_003E8__1._003C_003E4__this.pawn.PositionHeld, _003C_003E8__1._003C_003E4__this.pawn.Map);
							Find.LetterStack.ReceiveLetter("LetterRevenantFleshChunkLabel".Translate(), "LetterRevenantFleshChunkText".Translate(), LetterDefOf.NeutralEvent, thing2, null, null, null, null, 600);
							_003C_003E8__1._003C_003E4__this.Comp.injuredWhileAttacking = false;
						}
						_003C_003E8__1._003C_003E4__this.Comp.revenantState = RevenantState.Escape;
						_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Succeeded);
					}
				});
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.PlaySustainerOrSound(SoundDefOf.Pawn_Revenant_Hypnotize);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
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
			_003CMakeNewToils_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__15(0)
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

	private const TargetIndex VictimInd = TargetIndex.A;

	private const float NewTargetScanRadius = 20f;

	private const int SmearMTBTicks = 60;

	private const int CheckForCloserTargetMTB = 180;

	private static readonly int LongStunTicks = 2500;

	private int hypnotizeEndTick;

	private int HypnotizeDurationTicks => RevenantUtility.HypnotizeDurationSecondsFromNumColonistsCurve.Evaluate(RevenantUtility.NumSpawnedUnhypnotizedColonists(pawn.Map)).SecondsToTicks();

	private float RevealRange => RevenantUtility.RevealRangeFromNumColonistsCurve.Evaluate(RevenantUtility.NumSpawnedUnhypnotizedColonists(pawn.Map));

	private CompRevenant Comp => pawn.TryGetComp<CompRevenant>();

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref hypnotizeEndTick, "hypnotizeEndTick", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (pawn.mindState.enemyTarget != null)
		{
			job.SetTarget(TargetIndex.A, pawn.mindState.enemyTarget);
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__15))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}

	public override void Notify_PatherFailed()
	{
		Pawn pawn = job.GetTarget(TargetIndex.A).Pawn;
		if (pawn != null)
		{
			Find.Anomaly.EndHypnotize(pawn);
		}
		job.SetTarget(TargetIndex.A, RevenantUtility.GetClosestTargetInRadius(base.pawn, 20f));
		base.pawn.mindState.enemyTarget = job.GetTarget(TargetIndex.A).Pawn;
		if (base.pawn.mindState.enemyTarget == null)
		{
			Comp.revenantState = RevenantState.Wander;
		}
		base.Notify_PatherFailed();
	}
}
