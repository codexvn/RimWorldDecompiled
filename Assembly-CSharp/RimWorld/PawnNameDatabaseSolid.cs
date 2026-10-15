using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class PawnNameDatabaseSolid
{
	[CompilerGenerated]
	private sealed class _003CAllNames_003Ed__6 : IEnumerable<NameTriple>, IEnumerable, IEnumerator<NameTriple>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NameTriple _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Dictionary<GenderPossibility, List<NameTriple>>.Enumerator _003C_003E7__wrap1;

		private List<NameTriple>.Enumerator _003C_003E7__wrap2;

		NameTriple IEnumerator<NameTriple>.Current
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
		public _003CAllNames_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
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
			_003C_003E7__wrap1 = default(Dictionary<GenderPossibility, List<NameTriple>>.Enumerator);
			_003C_003E7__wrap2 = default(List<NameTriple>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_0084;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = solidNames.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00a3;
				IL_0084:
				if (_003C_003E7__wrap2.MoveNext())
				{
					NameTriple current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<NameTriple>.Enumerator);
				goto IL_00a3;
				IL_00a3:
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003E7__wrap2 = _003C_003E7__wrap1.Current.Value.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0084;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<GenderPossibility, List<NameTriple>>.Enumerator);
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<NameTriple> IEnumerable<NameTriple>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllNames_003Ed__6(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NameTriple>)this).GetEnumerator();
		}
	}

	private static Dictionary<GenderPossibility, List<NameTriple>> solidNames;

	private const float PreferredNameChance = 0.5f;

	static PawnNameDatabaseSolid()
	{
		solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();
		foreach (GenderPossibility value in Enum.GetValues(typeof(GenderPossibility)))
		{
			solidNames.Add(value, new List<NameTriple>());
		}
	}

	public static void Clear()
	{
		foreach (GenderPossibility value in Enum.GetValues(typeof(GenderPossibility)))
		{
			solidNames[value].Clear();
		}
	}

	public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
	{
		solidNames[genderPos].Add(newName);
	}

	public static List<NameTriple> GetListForGender(GenderPossibility gp)
	{
		return solidNames[gp];
	}

	[IteratorStateMachine(typeof(_003CAllNames_003Ed__6))]
	public static IEnumerable<NameTriple> AllNames()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllNames_003Ed__6(-2);
	}
}
