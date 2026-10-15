using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkCellsGlobal_003Ed__2 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Designation> _003C_003E7__wrap1;

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
		public _003CPotentialWorkCellsGlobal_003Ed__2(int _003C_003E1__state)
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
					_003C_003E7__wrap1 = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Designation current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.target.Cell;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			_003CPotentialWorkCellsGlobal_003Ed__2 _003CPotentialWorkCellsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkCellsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkCellsGlobal_003Ed__ = new _003CPotentialWorkCellsGlobal_003Ed__2(0);
			}
			_003CPotentialWorkCellsGlobal_003Ed__.pawn = _003C_003E3__pawn;
			return _003CPotentialWorkCellsGlobal_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	public override PathEndMode PathEndMode => PathEndMode.Touch;

	[IteratorStateMachine(typeof(_003CPotentialWorkCellsGlobal_003Ed__2))]
	public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkCellsGlobal_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool ShouldSkip(Pawn pawn, bool forced = false)
	{
		return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.SmoothWall);
	}

	public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
	{
		if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
		{
			return false;
		}
		Building edifice = c.GetEdifice(pawn.Map);
		if (edifice == null || !edifice.def.IsSmoothable)
		{
			Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176);
			pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
			return false;
		}
		if (!pawn.CanReserve(edifice, 1, -1, null, forced) || !pawn.CanReserve(c, 1, -1, null, forced))
		{
			return false;
		}
		return true;
	}

	public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
	{
		return JobMaker.MakeJob(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
	}
}
