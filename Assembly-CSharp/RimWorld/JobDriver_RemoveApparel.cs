using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RemoveApparel : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RemoveApparel _003C_003E4__this;

		private Toil _003CcarryToCell_003E5__2;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcarryToCell_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RemoveApparel CS_0024_003C_003E8__locals20 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals20.FailOnDestroyedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_General.Wait(CS_0024_003C_003E8__locals20.duration).PlaySustainerOrSound(CS_0024_003C_003E8__locals20.Apparel.def.apparel.soundRemove).WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					Apparel resultingAp;
					if (!CS_0024_003C_003E8__locals20.pawn.apparel.WornApparel.Contains(CS_0024_003C_003E8__locals20.Apparel))
					{
						CS_0024_003C_003E8__locals20.EndJobWith(JobCondition.Incompletable);
					}
					else if (!CS_0024_003C_003E8__locals20.pawn.apparel.TryDrop(CS_0024_003C_003E8__locals20.Apparel, out resultingAp))
					{
						CS_0024_003C_003E8__locals20.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						CS_0024_003C_003E8__locals20.job.targetA = resultingAp;
						if (CS_0024_003C_003E8__locals20.job.haulDroppedApparel)
						{
							resultingAp.SetForbidden(value: false, warnOnFail: false);
							StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(resultingAp);
							if (StoreUtility.TryFindBestBetterStoreCellFor(resultingAp, CS_0024_003C_003E8__locals20.pawn, CS_0024_003C_003E8__locals20.Map, currentPriority, CS_0024_003C_003E8__locals20.pawn.Faction, out var foundCell))
							{
								CS_0024_003C_003E8__locals20.job.count = resultingAp.stackCount;
								CS_0024_003C_003E8__locals20.job.targetB = foundCell;
							}
							else
							{
								CS_0024_003C_003E8__locals20.EndJobWith(JobCondition.Incompletable);
							}
						}
						else
						{
							CS_0024_003C_003E8__locals20.EndJobWith(JobCondition.Succeeded);
						}
					}
				});
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals20.job.haulDroppedApparel)
				{
					return false;
				}
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A).FailOn(() => !CS_0024_003C_003E8__locals20.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				_003C_003E2__current = _003CcarryToCell_003E5__2;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, _003CcarryToCell_003E5__2, storageMode: true);
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	private int duration;

	private const TargetIndex ApparelInd = TargetIndex.A;

	private Apparel Apparel => (Apparel)job.GetTarget(TargetIndex.A).Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref duration, "duration", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		duration = (int)(Apparel.GetStatValue(StatDefOf.EquipDelay) * 60f);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
