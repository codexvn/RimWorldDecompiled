using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PawnColumnDefGenerator
{
	[CompilerGenerated]
	private sealed class _003CImpliedPawnColumnDefs_003Ed__0 : IEnumerable<PawnColumnDef>, IEnumerable, IEnumerator<PawnColumnDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PawnColumnDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private PawnTableDef _003CanimalsTable_003E5__2;

		private PawnTableDef _003CworkTable_003E5__3;

		private bool _003CmoveWorkTypeLabelDown_003E5__4;

		private IEnumerator<TrainableDef> _003C_003E7__wrap4;

		private IEnumerator<WorkTypeDef> _003C_003E7__wrap5;

		PawnColumnDef IEnumerator<PawnColumnDef>.Current
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
		public _003CImpliedPawnColumnDefs_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CanimalsTable_003E5__2 = null;
			_003CworkTable_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = null;
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
					_003CanimalsTable_003E5__2 = PawnTableDefOf.Animals;
					_003C_003E7__wrap4 = DefDatabase<TrainableDef>.AllDefsListForReading.OrderByDescending((TrainableDef td) => td.listPriority).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_017a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_017a;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_017a:
					while (_003C_003E7__wrap4.MoveNext())
					{
						TrainableDef current = _003C_003E7__wrap4.Current;
						if (!current.specialTrainable)
						{
							string defName = "Trainable_" + current.defName;
							PawnColumnDef pawnColumnDef = (hotReload ? (DefDatabase<PawnColumnDef>.GetNamed(defName, errorOnFail: false) ?? new PawnColumnDef()) : new PawnColumnDef());
							pawnColumnDef.defName = defName;
							pawnColumnDef.trainable = current;
							pawnColumnDef.headerIcon = current.icon;
							pawnColumnDef.workerClass = typeof(PawnColumnWorker_Trainable);
							pawnColumnDef.sortable = true;
							pawnColumnDef.headerTip = current.LabelCap;
							pawnColumnDef.paintable = true;
							pawnColumnDef.modContentPack = current.modContentPack;
							_003CanimalsTable_003E5__2.columns.Insert(_003CanimalsTable_003E5__2.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, pawnColumnDef);
							_003C_003E2__current = pawnColumnDef;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					_003CworkTable_003E5__3 = PawnTableDefOf.Work;
					_003CmoveWorkTypeLabelDown_003E5__4 = false;
					_003C_003E7__wrap5 = WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder.Where((WorkTypeDef d) => d.visible).Reverse().GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap5.MoveNext())
				{
					WorkTypeDef current2 = _003C_003E7__wrap5.Current;
					_003CmoveWorkTypeLabelDown_003E5__4 = !_003CmoveWorkTypeLabelDown_003E5__4;
					string defName2 = "WorkPriority_" + current2.defName;
					PawnColumnDef pawnColumnDef2 = (hotReload ? (DefDatabase<PawnColumnDef>.GetNamed(defName2, errorOnFail: false) ?? new PawnColumnDef()) : new PawnColumnDef());
					pawnColumnDef2.defName = defName2;
					pawnColumnDef2.workType = current2;
					pawnColumnDef2.moveWorkTypeLabelDown = _003CmoveWorkTypeLabelDown_003E5__4;
					pawnColumnDef2.workerClass = typeof(PawnColumnWorker_WorkPriority);
					pawnColumnDef2.sortable = true;
					pawnColumnDef2.modContentPack = current2.modContentPack;
					_003CworkTable_003E5__3.columns.Insert(_003CworkTable_003E5__3.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, pawnColumnDef2);
					_003C_003E2__current = pawnColumnDef2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap5 = null;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PawnColumnDef> IEnumerable<PawnColumnDef>.GetEnumerator()
		{
			_003CImpliedPawnColumnDefs_003Ed__0 _003CImpliedPawnColumnDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedPawnColumnDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedPawnColumnDefs_003Ed__ = new _003CImpliedPawnColumnDefs_003Ed__0(0);
			}
			_003CImpliedPawnColumnDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedPawnColumnDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnColumnDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedPawnColumnDefs_003Ed__0))]
	public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedPawnColumnDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}
}
