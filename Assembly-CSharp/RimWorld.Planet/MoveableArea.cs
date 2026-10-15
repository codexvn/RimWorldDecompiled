using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class MoveableArea : IExposable
{
	[CompilerGenerated]
	private sealed class _003Cget_RelativeCells_003Ed__11 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MoveableArea _003C_003E4__this;

		private List<IntVec3>.Enumerator _003C_003E7__wrap1;

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
		public _003Cget_RelativeCells_003Ed__11(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<IntVec3>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MoveableArea moveableArea = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = moveableArea.relativeCells.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = PrefabUtility.GetAdjustedLocalPosition(current, moveableArea.gravship.Rotation);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<IntVec3>.Enumerator);
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003Cget_RelativeCells_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RelativeCells_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	protected List<IntVec3> relativeCells = new List<IntVec3>();

	protected string label;

	protected string renamableLabel;

	protected Color color;

	protected int id;

	private Gravship gravship;

	public IEnumerable<IntVec3> RelativeCells
	{
		[IteratorStateMachine(typeof(_003Cget_RelativeCells_003Ed__11))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RelativeCells_003Ed__11(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public MoveableArea()
	{
	}

	public MoveableArea(Gravship gravship, string label, string renamableLabel, Color color, int id)
	{
		this.gravship = gravship;
		this.label = label;
		this.renamableLabel = renamableLabel;
		this.color = color;
		this.id = id;
	}

	public virtual void ExposeData()
	{
		Scribe_Collections.Look(ref relativeCells, "relativeCells", LookMode.Value);
		Scribe_Values.Look(ref label, "label");
		Scribe_Values.Look(ref renamableLabel, "renamableLabel");
		Scribe_Values.Look(ref color, "color");
		Scribe_Values.Look(ref id, "id", 0);
		Scribe_References.Look(ref gravship, "gravship");
	}

	public void Add(IntVec3 relativeCell)
	{
		relativeCells.Add(relativeCell);
	}
}
