using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EnterCryptosleepCasket : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public JobDriver_EnterCryptosleepCasket _003C_003E4__this;

		public Toil enter;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003Ec__DisplayClass1_1 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass1_1();
			CS_0024_003C_003E8__locals10.actor = enter.actor;
			CS_0024_003C_003E8__locals10.pod = (Building_CryptosleepCasket)CS_0024_003C_003E8__locals10.actor.CurJob.targetA.Thing;
			Action action = delegate
			{
				bool flag = CS_0024_003C_003E8__locals10.actor.DeSpawnOrDeselect();
				if (CS_0024_003C_003E8__locals10.pod.TryAcceptThing(CS_0024_003C_003E8__locals10.actor) && flag)
				{
					Find.Selector.Select(CS_0024_003C_003E8__locals10.actor, playSound: false, forceDesignatorDeselect: false);
				}
			};
			if (!CS_0024_003C_003E8__locals10.pod.def.building.isPlayerEjectable)
			{
				if (_003C_003E4__this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(CS_0024_003C_003E8__locals10.actor.Named("PAWN")).AdjustedFor(CS_0024_003C_003E8__locals10.actor), action));
				}
				else
				{
					action();
				}
			}
			else
			{
				action();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_1
	{
		public Pawn actor;

		public Building_CryptosleepCasket pod;

		internal void _003CMakeNewToils_003Eb__1()
		{
			bool flag = actor.DeSpawnOrDeselect();
			if (pod.TryAcceptThing(actor) && flag)
			{
				Find.Selector.Select(actor, playSound: false, forceDesignatorDeselect: false);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EnterCryptosleepCasket _003C_003E4__this;

		private _003C_003Ec__DisplayClass1_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__1(int _003C_003E1__state)
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
			JobDriver_EnterCryptosleepCasket f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(500);
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				toil.WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E8__1.enter = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.enter.initAction = delegate
				{
					_003C_003Ec__DisplayClass1_1 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass1_1();
					CS_0024_003C_003E8__locals10.actor = _003C_003E8__1.enter.actor;
					CS_0024_003C_003E8__locals10.pod = (Building_CryptosleepCasket)CS_0024_003C_003E8__locals10.actor.CurJob.targetA.Thing;
					Action action = delegate
					{
						bool flag = CS_0024_003C_003E8__locals10.actor.DeSpawnOrDeselect();
						if (CS_0024_003C_003E8__locals10.pod.TryAcceptThing(CS_0024_003C_003E8__locals10.actor) && flag)
						{
							Find.Selector.Select(CS_0024_003C_003E8__locals10.actor, playSound: false, forceDesignatorDeselect: false);
						}
					};
					if (!CS_0024_003C_003E8__locals10.pod.def.building.isPlayerEjectable)
					{
						if (_003C_003E8__1._003C_003E4__this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(CS_0024_003C_003E8__locals10.actor.Named("PAWN")).AdjustedFor(CS_0024_003C_003E8__locals10.actor), action));
						}
						else
						{
							action();
						}
					}
					else
					{
						action();
					}
				};
				_003C_003E8__1.enter.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = _003C_003E8__1.enter;
				_003C_003E1__state = 3;
				return true;
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
			_003CMakeNewToils_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__1(0)
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

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__1))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}
}
