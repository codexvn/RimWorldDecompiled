using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public static class Toils_JobTransforms
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public IntVec3 interactCell;

		public Func<IntVec3, int> _003C_003E9__0;

		internal int _003CIngredientPlaceCellsInOrder_003Eb__0(IntVec3 c)
		{
			return (c - interactCell).LengthHorizontalSquared;
		}
	}

	[CompilerGenerated]
	private sealed class _003CIngredientPlaceCellsInOrder_003Ed__4 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing destination;

		public Thing _003C_003E3__destination;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CIngredientPlaceCellsInOrder_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					yieldedIngPlaceCells.Clear();
					_003C_003E1__state = -3;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass4_0();
					_003C_003E8__1.interactCell = destination.Position;
					if (destination is IBillGiver billGiver)
					{
						_003C_003E8__1.interactCell = ((Thing)billGiver).InteractionCell;
						_003C_003E7__wrap1 = billGiver.IngredientStackCells.OrderBy((IntVec3 c) => (c - _003C_003E8__1.interactCell).LengthHorizontalSquared).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0107;
					}
					goto IL_0121;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0107;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_01ad;
					}
					IL_01ad:
					_003Ci_003E5__3++;
					goto IL_01bf;
					IL_01bf:
					if (_003Ci_003E5__3 < 200)
					{
						IntVec3 intVec = _003C_003E8__1.interactCell + GenRadial.RadialPattern[_003Ci_003E5__3];
						if (!yieldedIngPlaceCells.Contains(intVec))
						{
							Building edifice = intVec.GetEdifice(destination.Map);
							if (edifice == null || edifice.def.passability != Traversability.Impassable || edifice.def.surfaceType != 0)
							{
								_003C_003E2__current = intVec;
								_003C_003E1__state = 2;
								return true;
							}
						}
						goto IL_01ad;
					}
					_003C_003E8__1 = null;
					_003C_003Em__Finally1();
					return false;
					IL_0121:
					_003Ci_003E5__3 = 0;
					goto IL_01bf;
					IL_0107:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap1.Current;
						yieldedIngPlaceCells.Add(current);
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0121;
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
			yieldedIngPlaceCells.Clear();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CIngredientPlaceCellsInOrder_003Ed__4 _003CIngredientPlaceCellsInOrder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CIngredientPlaceCellsInOrder_003Ed__ = this;
			}
			else
			{
				_003CIngredientPlaceCellsInOrder_003Ed__ = new _003CIngredientPlaceCellsInOrder_003Ed__4(0);
			}
			_003CIngredientPlaceCellsInOrder_003Ed__.destination = _003C_003E3__destination;
			return _003CIngredientPlaceCellsInOrder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

	public static Toil ExtractNextTargetFromQueue(TargetIndex ind, bool failIfCountFromQueueTooBig = true)
	{
		Toil toil = ToilMaker.MakeToil("ExtractNextTargetFromQueue");
		toil.initAction = delegate
		{
			Pawn actor = toil.actor;
			Job curJob = actor.jobs.curJob;
			List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
			if (!targetQueue.NullOrEmpty())
			{
				if (failIfCountFromQueueTooBig && !curJob.countQueue.NullOrEmpty() && targetQueue[0].HasThing && curJob.countQueue[0] > targetQueue[0].Thing.stackCount)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					curJob.SetTarget(ind, targetQueue[0]);
					targetQueue.RemoveAt(0);
					if (!curJob.countQueue.NullOrEmpty())
					{
						curJob.count = curJob.countQueue[0];
						curJob.countQueue.RemoveAt(0);
					}
				}
			}
		};
		return toil;
	}

	public static Toil ClearQueue(TargetIndex ind)
	{
		Toil toil = ToilMaker.MakeToil("ClearQueue");
		toil.initAction = delegate
		{
			List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
			if (!targetQueue.NullOrEmpty())
			{
				targetQueue.Clear();
			}
		};
		return toil;
	}

	public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind, Func<Thing, bool> validator = null)
	{
		Toil toil = ToilMaker.MakeToil("ClearDespawnedNullOrForbiddenQueuedTargets");
		toil.initAction = delegate
		{
			Pawn actor = toil.actor;
			actor.jobs.curJob.GetTargetQueue(ind).RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor) || (validator != null && !validator(ta.Thing)));
		};
		return toil;
	}

	[IteratorStateMachine(typeof(_003CIngredientPlaceCellsInOrder_003Ed__4))]
	private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(Thing destination)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CIngredientPlaceCellsInOrder_003Ed__4(-2)
		{
			_003C_003E3__destination = destination
		};
	}

	public static Toil SetTargetToIngredientPlaceCell(TargetIndex facilityInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
	{
		Toil toil = ToilMaker.MakeToil("SetTargetToIngredientPlaceCell");
		toil.initAction = delegate
		{
			Pawn actor = toil.actor;
			Job curJob = actor.jobs.curJob;
			Thing thing = curJob.GetTarget(carryItemInd).Thing;
			IntVec3 intVec = IntVec3.Invalid;
			foreach (IntVec3 item in IngredientPlaceCellsInOrder(curJob.GetTarget(facilityInd).Thing))
			{
				if (GenSpawn.CanSpawnAt(thing.def, item, actor.Map, null))
				{
					if (!intVec.IsValid)
					{
						intVec = item;
					}
					bool flag = false;
					List<Thing> list = actor.Map.thingGrid.ThingsListAt(item);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].def.category == ThingCategory.Item && (!list[i].CanStackWith(thing) || list[i].stackCount == list[i].def.stackLimit))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						curJob.SetTarget(cellTargetInd, item);
						return;
					}
				}
			}
			curJob.SetTarget(cellTargetInd, intVec);
		};
		return toil;
	}

	public static Toil MoveCurrentTargetIntoQueue(TargetIndex ind)
	{
		Toil toil = ToilMaker.MakeToil("MoveCurrentTargetIntoQueue");
		toil.initAction = delegate
		{
			Job curJob = toil.actor.CurJob;
			LocalTargetInfo target = curJob.GetTarget(ind);
			if (target.IsValid)
			{
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue == null)
				{
					curJob.AddQueuedTarget(ind, target);
				}
				else
				{
					targetQueue.Insert(0, target);
				}
				curJob.SetTarget(ind, null);
			}
		};
		return toil;
	}

	public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
	{
		Toil toil = ToilMaker.MakeToil("SucceedOnNoTargetInQueue");
		toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
		return toil;
	}
}
