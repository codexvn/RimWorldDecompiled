using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PutApparelOnOutfitStand : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PutApparelOnOutfitStand _003C_003E4__this;

		private bool _003CusingSource_003E5__2;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PutApparelOnOutfitStand CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals14.FailOnBurningImmobile(TargetIndex.A);
				CS_0024_003C_003E8__locals14.FailOnBurningImmobile(TargetIndex.B);
				_003CusingSource_003E5__2 = CS_0024_003C_003E8__locals14.TargetIsOnApparelSource;
				if (_003CusingSource_003E5__2)
				{
					_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.C);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00a6;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a6;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0111;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0111;
			case 5:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals14.pawn.carryTracker.CarriedThing is Apparel apparel)
					{
						if (!CS_0024_003C_003E8__locals14.DestOutfitStand.HasRoomForApparelOfDef(apparel.def) && !CS_0024_003C_003E8__locals14.DestOutfitStand.TryDropThingsToMakeRoomForThingOfDef(apparel.def))
						{
							CS_0024_003C_003E8__locals14.EndJobWith(JobCondition.Errored);
						}
						else
						{
							CS_0024_003C_003E8__locals14.pawn.carryTracker.innerContainer.Remove(apparel);
							CS_0024_003C_003E8__locals14.DestOutfitStand.AddApparel(apparel);
						}
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 6;
				return true;
			}
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0111:
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
				IL_00a6:
				if (_003CusingSource_003E5__2)
				{
					Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
					toil2.initAction = delegate
					{
						CS_0024_003C_003E8__locals14.ApparelSource.RemoveApparel(CS_0024_003C_003E8__locals14.Apparel);
						CS_0024_003C_003E8__locals14.pawn.carryTracker.TryStartCarry(CS_0024_003C_003E8__locals14.Apparel);
					};
					_003C_003E2__current = toil2;
					_003C_003E1__state = 3;
					return true;
				}
				CS_0024_003C_003E8__locals14.job.count = 1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
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

	private const TargetIndex ApparelInd = TargetIndex.A;

	private const TargetIndex OutfitStandInd = TargetIndex.B;

	private const TargetIndex ApparelSourceIndex = TargetIndex.C;

	private Apparel Apparel => job.GetTarget(TargetIndex.A).Thing as Apparel;

	private Building_OutfitStand DestOutfitStand => job.GetTarget(TargetIndex.B).Thing as Building_OutfitStand;

	private bool TargetIsOnApparelSource
	{
		get
		{
			Apparel apparel = Apparel;
			if (apparel != null && !apparel.Spawned && apparel.ParentHolder is IApparelSource apparelSource)
			{
				return apparelSource is Thing;
			}
			return false;
		}
	}

	private IApparelSource ApparelSource => (IApparelSource)job.GetTarget(TargetIndex.C).Thing;

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (TargetIsOnApparelSource)
		{
			job.targetC = (Thing)Apparel.ParentHolder;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (TargetIsOnApparelSource)
		{
			if (pawn.Reserve((Thing)Apparel.ParentHolder, job, 1, -1, null, errorOnFailed))
			{
				return pawn.Reserve(DestOutfitStand, job, 1, -1, null, errorOnFailed);
			}
			return false;
		}
		if (pawn.Reserve(Apparel, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DestOutfitStand, job, 1, -1, null, errorOnFailed);
		}
		return false;
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
}
