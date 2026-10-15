using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RecolorApparel : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RecolorApparel _003C_003E4__this;

		private IEnumerator<Toil> _003C_003E7__wrap1;

		private Toil _003Cwork_003E5__3;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003Cwork_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_RecolorApparel CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModLister.CheckIdeology("Apparel recoloring"))
					{
						return false;
					}
					CS_0024_003C_003E8__locals6.FailOnDespawnedOrNull(TargetIndex.C);
					_003C_003E7__wrap1 = JobDriver_DoBill.CollectIngredientsToils(TargetIndex.A, TargetIndex.C, TargetIndex.B, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: false).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a2;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a2;
				case 2:
					_003C_003E1__state = -1;
					_003Cwork_003E5__3 = Toils_General.WaitWith(TargetIndex.C, 1000, useProgressBar: true, maintainPosture: false, maintainSleep: false, TargetIndex.C);
					_003Cwork_003E5__3.PlaySustainerOrSound(SoundDefOf.Interact_RecolorApparel);
					_003C_003E2__current = _003Cwork_003E5__3;
					_003C_003E1__state = 3;
					return true;
				case 3:
				{
					_003C_003E1__state = -1;
					Toil toil = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
					_003C_003E2__current = toil;
					_003C_003E1__state = 4;
					return true;
				}
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_General.Do(delegate
					{
						ThingCountClass thingCountClass = CS_0024_003C_003E8__locals6.job.placedThings[0];
						if (thingCountClass.thing.stackCount == 1)
						{
							thingCountClass.thing.Destroy();
							CS_0024_003C_003E8__locals6.job.placedThings.RemoveAt(0);
						}
						else if (!thingCountClass.thing.Destroyed)
						{
							thingCountClass.thing.SplitOff(1).Destroy();
						}
						CS_0024_003C_003E8__locals6.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompColorable>().Recolor();
						CS_0024_003C_003E8__locals6.job.count = CS_0024_003C_003E8__locals6.job.GetTargetQueue(TargetIndex.B)?.Count ?? 0;
					});
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, _003Cwork_003E5__3);
					_003C_003E1__state = 6;
					return true;
				case 6:
					{
						_003C_003E1__state = -1;
						_003Cwork_003E5__3 = null;
						return false;
					}
					IL_00a2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell);
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	public const TargetIndex DyeInd = TargetIndex.A;

	public const TargetIndex ApparelInd = TargetIndex.B;

	public const TargetIndex StylingStationInd = TargetIndex.C;

	public const int WorkTimeTicks = 1000;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(job.GetTarget(TargetIndex.C), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		Thing thing = job.GetTarget(TargetIndex.C).Thing;
		if (thing != null && thing.def.hasInteractionCell && !pawn.ReserveSittableOrSpot(thing.InteractionCell, job, errorOnFailed))
		{
			return false;
		}
		int num = job.GetTargetQueue(TargetIndex.B).Count;
		foreach (LocalTargetInfo item in job.GetTargetQueue(TargetIndex.A))
		{
			int num2 = Mathf.Min(num, item.Thing.stackCount);
			if (!pawn.Reserve(item, job, 1, num2, null, errorOnFailed))
			{
				return false;
			}
			num -= num2;
			if (num2 <= 0)
			{
				break;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
