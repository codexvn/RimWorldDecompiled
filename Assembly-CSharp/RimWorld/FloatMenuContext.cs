using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class FloatMenuContext
{
	[CompilerGenerated]
	private sealed class _003Cget_ValidSelectedPawns_003Ed__23 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuContext _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_ValidSelectedPawns_003Ed__23(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FloatMenuContext floatMenuContext = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = floatMenuContext.allSelectedPawns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (FloatMenuMakerMap.currentProvider == null || FloatMenuMakerMap.currentProvider.SelectedPawnValid(current, floatMenuContext))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_ValidSelectedPawns_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ValidSelectedPawns_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public List<Pawn> allSelectedPawns;

	public Vector3 clickPosition;

	public Map map;

	private IntVec3 cachedClickedCell;

	private List<Thing> cachedClickedThings;

	private List<Pawn> cachedClickedPawns;

	private Room cachedClickedRoom;

	private Zone cachedClickedZone;

	public IntVec3 ClickedCell => cachedClickedCell;

	public Room ClickedRoom => cachedClickedRoom;

	public Zone ClickedZone => cachedClickedZone;

	public List<Thing> ClickedThings => cachedClickedThings;

	public List<Pawn> ClickedPawns => cachedClickedPawns;

	public bool IsMultiselect => allSelectedPawns.Count > 1;

	public Pawn FirstSelectedPawn
	{
		get
		{
			foreach (Pawn allSelectedPawn in allSelectedPawns)
			{
				if (FloatMenuMakerMap.currentProvider == null || FloatMenuMakerMap.currentProvider.SelectedPawnValid(allSelectedPawn, this))
				{
					return allSelectedPawn;
				}
			}
			return null;
		}
	}

	public IEnumerable<Pawn> ValidSelectedPawns
	{
		[IteratorStateMachine(typeof(_003Cget_ValidSelectedPawns_003Ed__23))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ValidSelectedPawns_003Ed__23(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public FloatMenuContext(List<Pawn> selectedPawns, Vector3 clickPosition, Map map)
	{
		allSelectedPawns = selectedPawns;
		this.clickPosition = clickPosition;
		this.map = map;
		cachedClickedCell = IntVec3.FromVector3(clickPosition);
		cachedClickedRoom = cachedClickedCell.GetRoom(map);
		cachedClickedZone = cachedClickedCell.GetZone(map);
		cachedClickedThings = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForThing());
		cachedClickedPawns = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForPawns()).OfType<Pawn>().ToList();
		selectedPawns.RemoveAll((Pawn pawn) => !pawn.CanTakeOrder);
	}
}
