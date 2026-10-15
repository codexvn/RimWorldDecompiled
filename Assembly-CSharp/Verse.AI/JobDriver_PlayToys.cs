using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_PlayToys : JobDriver_BabyPlay
{
	[CompilerGenerated]
	private sealed class _003CPlay_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlayToys _003C_003E4__this;

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
		public _003CPlay_003Ed__13(int _003C_003E1__state)
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
			JobDriver_PlayToys jobDriver_PlayToys = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_PlayToys.FailOnDestroyedNullOrForbidden(TargetIndex.B);
				jobDriver_PlayToys.FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E2__current = Toils_Goto.Goto(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_PlayToys.DropBaby();
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_PlayToys.PlayToil();
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
			_003CPlay_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlay_003Ed__13(0)
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

	private const TargetIndex ToyBoxInd = TargetIndex.B;

	private const int ToysCount = 5;

	private const float ToyDistanceFactor = 0.5f;

	private static readonly FloatRange ToyRandomAngleOffset = new FloatRange(-5f, 5f);

	private const int InteractionTicks = 600;

	private Mote[] motesToMaintain;

	private Thing ToyBox => base.TargetThingB;

	private Building_Bed Bed => (Building_Bed)base.TargetThingC;

	protected override StartingConditions StartingCondition => StartingConditions.PickupBaby;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!base.TryMakePreToilReservations(errorOnFailed))
		{
			return false;
		}
		if (Bed != null && !pawn.Reserve(Bed, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CPlay_003Ed__13))]
	protected override IEnumerable<Toil> Play()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlay_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil PlayToil()
	{
		Toil toil = ToilMaker.MakeToil("PlayToil");
		toil.defaultCompleteMode = ToilCompleteMode.Never;
		toil.handlingFacing = true;
		toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
		toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
		{
			if (motesToMaintain.NullOrEmpty())
			{
				Vector3 vector = base.Baby.TrueCenter();
				Vector3 v = IntVec3.North.ToVector3();
				float num = 72f;
				motesToMaintain = new Mote[5];
				for (int i = 0; i < 5; i++)
				{
					Vector3 loc = vector + v.RotatedBy(num * (float)i + ToyRandomAngleOffset.RandomInRange) * 0.5f;
					motesToMaintain[i] = MoteMaker.MakeStaticMote(loc, base.Map, ThingDefOf.Mote_Toy);
				}
			}
			pawn.rotationTracker.FaceTarget(base.Baby);
			for (int j = 0; j < motesToMaintain.Length; j++)
			{
				motesToMaintain[j]?.Maintain();
			}
			if (pawn.IsHashIntervalTick(600, delta))
			{
				pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
			}
			if (roomPlayGainFactor < 0f)
			{
				roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
			}
			if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, pawn, roomPlayGainFactor, delta, ToyBox))
			{
				pawn.jobs.curDriver.ReadyForNextToil();
			}
		});
		ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
		return toil;
	}

	private Toil DropBaby()
	{
		Toil toil = ToilMaker.MakeToil("DropBaby");
		toil.initAction = delegate
		{
			pawn.carryTracker.innerContainer.TryDrop(base.Baby, ThingPlaceMode.Near, 1, out var _, null, (IntVec3 c) => !ToyBox.OccupiedRect().Contains(c));
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}
}
