using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Vomit : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Vomit _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Vomit CS_0024_003C_003E8__locals17 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals17.ticksLeft = Rand.Range(300, 900);
					int num2 = 0;
					IntVec3 intVec;
					do
					{
						intVec = CS_0024_003C_003E8__locals17.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
						num2++;
						if (num2 > 12)
						{
							intVec = CS_0024_003C_003E8__locals17.pawn.Position;
							break;
						}
					}
					while (!intVec.InBounds(CS_0024_003C_003E8__locals17.pawn.Map) || !intVec.Standable(CS_0024_003C_003E8__locals17.pawn.Map));
					CS_0024_003C_003E8__locals17.job.targetA = intVec;
					CS_0024_003C_003E8__locals17.pawn.pather.StopDead();
				};
				toil.tickIntervalAction = delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals17.pawn.IsHashIntervalTick(150, delta))
					{
						FilthMaker.TryMakeFilth(CS_0024_003C_003E8__locals17.job.targetA.Cell, CS_0024_003C_003E8__locals17.Map, ThingDefOf.Filth_Vomit, CS_0024_003C_003E8__locals17.pawn.LabelIndefinite());
						if (CS_0024_003C_003E8__locals17.pawn.needs != null && CS_0024_003C_003E8__locals17.pawn.needs.TryGetNeed(out Need_Food need) && need.CurLevelPercentage > 0.1f)
						{
							need.CurLevel -= need.MaxLevel * 0.04f;
						}
					}
					CS_0024_003C_003E8__locals17.ticksLeft -= delta;
					if (CS_0024_003C_003E8__locals17.ticksLeft <= 0)
					{
						CS_0024_003C_003E8__locals17.ReadyForNextToil();
						TaleRecorder.RecordTale(TaleDefOf.Vomited, CS_0024_003C_003E8__locals17.pawn);
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.WithEffect(EffecterDefOf.Vomit, TargetIndex.A, null);
				toil.PlaySustainerOrSound(() => SoundDefOf.Vomit);
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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

	private int ticksLeft;

	public override void SetInitialPosture()
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksLeft, "ticksLeft", 0);
	}

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
}
