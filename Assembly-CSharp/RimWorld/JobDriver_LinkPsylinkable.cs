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

public class JobDriver_LinkPsylinkable : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__11 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_LinkPsylinkable _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__11(int _003C_003E1__state)
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
			JobDriver_LinkPsylinkable CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckRoyalty("Psylinkable"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals12.AddFailCondition(() => !CS_0024_003C_003E8__locals12.Psylinkable.CanPsylink(CS_0024_003C_003E8__locals12.pawn, CS_0024_003C_003E8__locals12.LinkSpot).Accepted);
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(15000);
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals12.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals12.PsylinkableThing);
					if (CS_0024_003C_003E8__locals12.pawn.IsHashIntervalTick(720, delta))
					{
						Vector3 vector = CS_0024_003C_003E8__locals12.pawn.TrueCenter();
						vector += (CS_0024_003C_003E8__locals12.PsylinkableThing.TrueCenter() - vector) * Rand.Value;
						FleckMaker.Static(vector, CS_0024_003C_003E8__locals12.pawn.Map, FleckDefOf.PsycastAreaEffect, 0.5f);
						CS_0024_003C_003E8__locals12.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(CS_0024_003C_003E8__locals12.PsylinkableThing)));
					}
				};
				toil.handlingFacing = false;
				toil.socialMode = RandomSocialMode.Off;
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
			_003CMakeNewToils_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__11(0)
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

	public const int LinkTimeTicks = 15000;

	public const int EffectsTickInterval = 720;

	protected const TargetIndex PsylinkableInd = TargetIndex.A;

	protected const TargetIndex LinkSpotInd = TargetIndex.B;

	private Thing PsylinkableThing => base.TargetA.Thing;

	private CompPsylinkable Psylinkable => PsylinkableThing.TryGetComp<CompPsylinkable>();

	private LocalTargetInfo LinkSpot => job.targetB;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(PsylinkableThing, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(LinkSpot, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__11))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}
}
