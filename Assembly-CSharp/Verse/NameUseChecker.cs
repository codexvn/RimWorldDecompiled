using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public static class NameUseChecker
{
	[CompilerGenerated]
	private sealed class _003Cget_AllPawnsNamesEverUsed_003Ed__1 : IEnumerable<Name>, IEnumerable, IEnumerator<Name>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Name _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Name IEnumerator<Name>.Current
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
		public _003Cget_AllPawnsNamesEverUsed_003Ed__1(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (current.Name != null)
					{
						_003C_003E2__current = current.Name;
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
		IEnumerator<Name> IEnumerable<Name>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_AllPawnsNamesEverUsed_003Ed__1(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Name>)this).GetEnumerator();
		}
	}

	public static IEnumerable<Name> AllPawnsNamesEverUsed
	{
		[IteratorStateMachine(typeof(_003Cget_AllPawnsNamesEverUsed_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllPawnsNamesEverUsed_003Ed__1(-2);
		}
	}

	public static bool NameWordIsUsed(string singleName)
	{
		foreach (Name item in AllPawnsNamesEverUsed)
		{
			if (item is NameTriple nameTriple && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
			{
				return true;
			}
			if (item is NameSingle nameSingle && nameSingle.Name == singleName)
			{
				return true;
			}
		}
		return false;
	}

	public static bool NameSingleIsUsed(string candidate)
	{
		foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
		{
			if (item.Name is NameSingle nameSingle && nameSingle.Name == candidate)
			{
				return true;
			}
		}
		return false;
	}

	public static bool XenotypeNameIsUsed(string candidate)
	{
		foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
		{
			if (item.genes != null && item.genes.UniqueXenotype)
			{
				string xenotypeName = item.genes.xenotypeName;
				if (candidate == xenotypeName)
				{
					return true;
				}
			}
		}
		return false;
	}
}
