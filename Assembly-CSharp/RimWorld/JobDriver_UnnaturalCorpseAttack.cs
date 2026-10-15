using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_UnnaturalCorpseAttack : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public JobDriver_UnnaturalCorpseAttack _003C_003E4__this;

		public int killTicks;

		public UnnaturalCorpseTracker tracker;

		internal void _003CMakeNewToils_003Eb__0()
		{
			JobDriver curDriver = _003C_003E4__this.pawn.jobs.curDriver;
			int ticks = killTicks + 60;
			Find.Anomaly.Hypnotize(_003C_003E4__this.Victim, _003C_003E4__this.pawn, ticks);
			curDriver.ReadyForNextToil();
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			Messages.Message("MessageAwokenAttacking".Translate(_003C_003E4__this.Victim.Named("PAWN")), _003C_003E4__this.Victim, MessageTypeDefOf.NegativeEvent);
			tracker.Notify_AwokenAttackStarting();
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			_003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(_003C_003E4__this.pawn);
			_003C_003E4__this.Victim.health.GetOrAddHediff(HediffDefOf.AwokenHypnosis, null, null).TryGetComp<HediffComp_Disappears>().ticksToDisappear++;
			if (_003C_003E4__this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.UnnaturalCorpseAwokenKilling)
			{
				_003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.UnnaturalCorpseAwokenKilling);
			}
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			if (_003C_003E4__this.pawn.Drawer.renderer.CurAnimation == AnimationDefOf.UnnaturalCorpseAwokenKilling)
			{
				_003C_003E4__this.pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			Pawn pawn = _003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			if (brain != null)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_UnnaturalCorpseAttack, _003C_003E4__this.pawn));
				DamageDef psychic = DamageDefOf.Psychic;
				BodyPartRecord hitPart = brain;
				pawn.TakeDamage(new DamageInfo(psychic, 99999f, 99999f, -1f, _003C_003E4__this.pawn, hitPart));
				Find.Anomaly.Notify_PawnKilledViaAwoken(pawn);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_UnnaturalCorpseAttack _003C_003E4__this;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__4(int _003C_003E1__state)
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
			JobDriver_UnnaturalCorpseAttack jobDriver_UnnaturalCorpseAttack = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass4_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_UnnaturalCorpseAttack.FailOnDespawnedOrNull(TargetIndex.A);
				Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(jobDriver_UnnaturalCorpseAttack.Victim, out _003C_003E8__1.tracker);
				_003C_003E8__1.killTicks = _003C_003E8__1.tracker.TicksToKill;
				_003C_003E2__current = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate
				{
					JobDriver curDriver = _003C_003E8__1._003C_003E4__this.pawn.jobs.curDriver;
					int ticks = _003C_003E8__1.killTicks + 60;
					Find.Anomaly.Hypnotize(_003C_003E8__1._003C_003E4__this.Victim, _003C_003E8__1._003C_003E4__this.pawn, ticks);
					curDriver.ReadyForNextToil();
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = (Action)Delegate.Combine(toil2.initAction, (Action)delegate
				{
					Messages.Message("MessageAwokenAttacking".Translate(_003C_003E8__1._003C_003E4__this.Victim.Named("PAWN")), _003C_003E8__1._003C_003E4__this.Victim, MessageTypeDefOf.NegativeEvent);
					_003C_003E8__1.tracker.Notify_AwokenAttackStarting();
				});
				toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, (Action)delegate
				{
					_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.pawn);
					_003C_003E8__1._003C_003E4__this.Victim.health.GetOrAddHediff(HediffDefOf.AwokenHypnosis, null, null).TryGetComp<HediffComp_Disappears>().ticksToDisappear++;
					if (_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.CurAnimation != AnimationDefOf.UnnaturalCorpseAwokenKilling)
					{
						_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.SetAnimation(AnimationDefOf.UnnaturalCorpseAwokenKilling);
					}
				});
				toil2.defaultDuration = _003C_003E8__1.killTicks;
				toil2.defaultCompleteMode = ToilCompleteMode.Delay;
				toil2.PlaySustainerOrSound(SoundDefOf.Pawn_Awoken_Hypnotize);
				toil2.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.CurAnimation == AnimationDefOf.UnnaturalCorpseAwokenKilling)
					{
						_003C_003E8__1._003C_003E4__this.pawn.Drawer.renderer.SetAnimation(null);
					}
				});
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Pawn pawn = _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Pawn;
					BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
					if (brain != null)
					{
						Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_UnnaturalCorpseAttack, _003C_003E8__1._003C_003E4__this.pawn));
						DamageDef psychic = DamageDefOf.Psychic;
						BodyPartRecord hitPart = brain;
						pawn.TakeDamage(new DamageInfo(psychic, 99999f, 99999f, -1f, _003C_003E8__1._003C_003E4__this.pawn, hitPart));
						Find.Anomaly.Notify_PawnKilledViaAwoken(pawn);
					}
				};
				toil.PlaySustainerOrSound(SoundDefOf.CocoonDestroyed);
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
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
			_003CMakeNewToils_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__4(0)
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

	private const TargetIndex VictimIndex = TargetIndex.A;

	private Pawn Victim => base.TargetPawnA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__4))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}
}
