using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public static class MineAIUtility
{
	[CompilerGenerated]
	private sealed class _003CPotentialMineables_003Ed__4 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Designation>.Enumerator _003C_003E7__wrap1;

		Thing IEnumerator<Thing>.Current
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
		public _003CPotentialMineables_003Ed__4(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
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
					tmpDesignations.Clear();
					tmpDesignations.AddRange(pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine));
					tmpDesignations.AddRange(pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.MineVein));
					_003C_003E7__wrap1 = tmpDesignations.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					bool flag = false;
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c = current.target.Cell + GenAdj.AdjacentCells[i];
						if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						Mineable firstMineable = current.target.Cell.GetFirstMineable(pawn.Map);
						if (firstMineable != null)
						{
							_003C_003E2__current = firstMineable;
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Designation>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPotentialMineables_003Ed__4 _003CPotentialMineables_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialMineables_003Ed__ = this;
			}
			else
			{
				_003CPotentialMineables_003Ed__ = new _003CPotentialMineables_003Ed__4(0);
			}
			_003CPotentialMineables_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialMineables_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private static string NoPathTrans;

	private const int MiningJobTicks = 20000;

	private static List<Designation> tmpDesignations = new List<Designation>();

	public static void ResetStaticData()
	{
		NoPathTrans = "NoPath".Translate();
	}

	[IteratorStateMachine(typeof(_003CPotentialMineables_003Ed__4))]
	public static IEnumerable<Thing> PotentialMineables(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialMineables_003Ed__4(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
	{
		if (!t.def.mineable)
		{
			return null;
		}
		if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null && pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) == null)
		{
			return null;
		}
		if (!pawn.CanReserve(t, 1, -1, null, forced))
		{
			return null;
		}
		if (!new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
		{
			return null;
		}
		bool flag = false;
		for (int i = 0; i < 8; i++)
		{
			IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
			if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 8; j++)
			{
				IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
				if (!intVec2.InBounds(t.Map) || !ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) || !intVec2.WalkableBy(t.Map, pawn) || intVec2.Standable(t.Map))
				{
					continue;
				}
				List<Thing> thingList = intVec2.GetThingList(t.Map);
				for (int k = 0; k < thingList.Count; k++)
				{
					if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
					{
						Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[k]);
						if (job != null)
						{
							return job;
						}
					}
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				JobFailReason.Is(NoPathTrans);
				return null;
			}
		}
		return JobMaker.MakeJob(JobDefOf.Mine, t, 20000, checkOverrideOnExpiry: true);
	}
}
