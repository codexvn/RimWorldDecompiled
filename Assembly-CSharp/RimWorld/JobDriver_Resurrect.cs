using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_Resurrect : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Resurrect _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Resurrect CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(600);
				toil.WithProgressBarToilDelay(TargetIndex.A);
				toil.FailOnDespawnedOrNull(TargetIndex.A);
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				toil.tickAction = delegate
				{
					CompUsable compUsable = CS_0024_003C_003E8__locals6.Item.TryGetComp<CompUsable>();
					if (compUsable != null && CS_0024_003C_003E8__locals6.warmupMote == null && compUsable.Props.warmupMote != null)
					{
						CS_0024_003C_003E8__locals6.warmupMote = MoteMaker.MakeAttachedOverlay(CS_0024_003C_003E8__locals6.Corpse, compUsable.Props.warmupMote, Vector3.zero);
					}
					CS_0024_003C_003E8__locals6.warmupMote?.Maintain();
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(CS_0024_003C_003E8__locals6.Resurrect);
				_003C_003E1__state = 5;
				return true;
			case 5:
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const TargetIndex CorpseInd = TargetIndex.A;

	private const TargetIndex ItemInd = TargetIndex.B;

	private const int DurationTicks = 600;

	private Mote warmupMote;

	private Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

	private Thing Item => job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Corpse, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void Resurrect()
	{
		Pawn innerPawn = Corpse.InnerPawn;
		CompTargetEffect_Resurrect compTargetEffect_Resurrect = Item.TryGetComp<CompTargetEffect_Resurrect>();
		bool flag = true;
		if (!compTargetEffect_Resurrect.Props.withSideEffects)
		{
			if (!ResurrectionUtility.TryResurrect(innerPawn))
			{
				flag = false;
			}
		}
		else if (!ResurrectionUtility.TryResurrectWithSideEffects(innerPawn))
		{
			flag = false;
		}
		if (flag)
		{
			SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(innerPawn));
			Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent);
			if (compTargetEffect_Resurrect.Props.moteDef != null)
			{
				MoteMaker.MakeAttachedOverlay(innerPawn, compTargetEffect_Resurrect.Props.moteDef, Vector3.zero);
			}
			if (compTargetEffect_Resurrect.Props.addsHediff != null)
			{
				innerPawn.health.AddHediff(compTargetEffect_Resurrect.Props.addsHediff, null, null);
			}
		}
		Item.SplitOff(1).Destroy();
	}
}
