using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EnterTransporter : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EnterTransporter _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
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
			JobDriver_EnterTransporter CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals16.FailOnDespawnedOrNull(CS_0024_003C_003E8__locals16.TransporterInd);
				CS_0024_003C_003E8__locals16.FailOn(() => CS_0024_003C_003E8__locals16.Shuttle != null && !CS_0024_003C_003E8__locals16.Shuttle.IsAllowed(CS_0024_003C_003E8__locals16.pawn));
				_003C_003E2__current = Toils_Goto.GotoThing(CS_0024_003C_003E8__locals16.TransporterInd, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals16.job.playerForced || !LoadTransportersJobUtility.HasJobOnTransporter(CS_0024_003C_003E8__locals16.pawn, CS_0024_003C_003E8__locals16.Transporter))
					{
						if (!CS_0024_003C_003E8__locals16.Transporter.LoadingInProgressOrReadyToLaunch)
						{
							TransporterUtility.InitiateLoading(Gen.YieldSingle(CS_0024_003C_003E8__locals16.Transporter));
						}
						CompTransporter transporter = CS_0024_003C_003E8__locals16.Transporter;
						bool flag = CS_0024_003C_003E8__locals16.pawn.DeSpawnOrDeselect();
						transporter.GetDirectlyHeldThings().TryAdd(CS_0024_003C_003E8__locals16.pawn);
						if (flag)
						{
							Find.Selector.Select(CS_0024_003C_003E8__locals16.pawn, playSound: false, forceDesignatorDeselect: false);
						}
					}
				};
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private TargetIndex TransporterInd = TargetIndex.A;

	public CompTransporter Transporter => job.GetTarget(TransporterInd).Thing?.TryGetComp<CompTransporter>();

	public CompShuttle Shuttle => job.GetTarget(TransporterInd).Thing?.TryGetComp<CompShuttle>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
