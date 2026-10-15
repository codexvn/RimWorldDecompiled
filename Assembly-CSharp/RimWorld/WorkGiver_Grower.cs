using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class WorkGiver_Grower : WorkGiver_Scanner
{
	[CompilerGenerated]
	private sealed class _003CPotentialWorkCellsGlobal_003Ed__4 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		public WorkGiver_Grower _003C_003E4__this;

		private Danger _003CmaxDanger_003E5__2;

		private List<Building> _003CbList_003E5__3;

		private List<Zone> _003CzonesList_003E5__4;

		private int _003Ci_003E5__5;

		private CellRect.Enumerator _003C_003E7__wrap5;

		private Zone_Growing _003CgrowZone_003E5__7;

		private int _003Cj_003E5__8;

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
		public _003CPotentialWorkCellsGlobal_003Ed__4(int _003C_003E1__state)
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
			_003CbList_003E5__3 = null;
			_003CzonesList_003E5__4 = null;
			_003CgrowZone_003E5__7 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorkGiver_Grower workGiver_Grower = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CmaxDanger_003E5__2 = pawn.NormalMaxDanger();
					_003CbList_003E5__3 = pawn.Map.listerBuildings.allBuildingsColonist;
					_003Ci_003E5__5 = 0;
					goto IL_0140;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0115;
				case 2:
					{
						_003C_003E1__state = -1;
						_003Cj_003E5__8++;
						goto IL_0284;
					}
					IL_0140:
					if (_003Ci_003E5__5 < _003CbList_003E5__3.Count)
					{
						if (_003CbList_003E5__3[_003Ci_003E5__5] is Building_PlantGrower building_PlantGrower && workGiver_Grower.ExtraRequirements(building_PlantGrower, pawn) && !building_PlantGrower.IsForbidden(pawn) && pawn.CanReach(building_PlantGrower, PathEndMode.OnCell, _003CmaxDanger_003E5__2) && !building_PlantGrower.IsBurning())
						{
							_003C_003E7__wrap5 = building_PlantGrower.OccupiedRect().GetEnumerator();
							_003C_003E1__state = -3;
							goto IL_0115;
						}
						goto IL_012e;
					}
					wantedPlantDef = null;
					_003CzonesList_003E5__4 = pawn.Map.zoneManager.AllZones;
					_003Ci_003E5__5 = 0;
					goto IL_02bb;
					IL_0115:
					if (_003C_003E7__wrap5.MoveNext())
					{
						IntVec3 current = _003C_003E7__wrap5.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					wantedPlantDef = null;
					goto IL_012e;
					IL_02bb:
					if (_003Ci_003E5__5 < _003CzonesList_003E5__4.Count)
					{
						_003CgrowZone_003E5__7 = _003CzonesList_003E5__4[_003Ci_003E5__5] as Zone_Growing;
						if (_003CgrowZone_003E5__7 != null)
						{
							if (_003CgrowZone_003E5__7.cells.Count == 0)
							{
								Log.ErrorOnce("Grow zone has 0 cells: " + _003CgrowZone_003E5__7, -563487);
							}
							else if (workGiver_Grower.ExtraRequirements(_003CgrowZone_003E5__7, pawn) && !_003CgrowZone_003E5__7.ContainsStaticFire && pawn.CanReach(_003CgrowZone_003E5__7.Cells[0], PathEndMode.OnCell, _003CmaxDanger_003E5__2))
							{
								_003Cj_003E5__8 = 0;
								goto IL_0284;
							}
						}
						goto IL_02a9;
					}
					wantedPlantDef = null;
					return false;
					IL_0284:
					if (_003Cj_003E5__8 < _003CgrowZone_003E5__7.cells.Count)
					{
						_003C_003E2__current = _003CgrowZone_003E5__7.cells[_003Cj_003E5__8];
						_003C_003E1__state = 2;
						return true;
					}
					wantedPlantDef = null;
					_003CgrowZone_003E5__7 = null;
					goto IL_02a9;
					IL_012e:
					_003Ci_003E5__5++;
					goto IL_0140;
					IL_02a9:
					_003Ci_003E5__5++;
					goto IL_02bb;
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
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CPotentialWorkCellsGlobal_003Ed__4 _003CPotentialWorkCellsGlobal_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPotentialWorkCellsGlobal_003Ed__ = this;
			}
			else
			{
				_003CPotentialWorkCellsGlobal_003Ed__ = new _003CPotentialWorkCellsGlobal_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
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

	protected static ThingDef wantedPlantDef;

	public override bool AllowUnreachable => true;

	protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CPotentialWorkCellsGlobal_003Ed__4))]
	public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPotentialWorkCellsGlobal_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
	{
		return c.GetPlantToGrowSettable(map)?.GetPlantDefToGrow();
	}
}
