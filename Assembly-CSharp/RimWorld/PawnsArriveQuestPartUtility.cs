using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class PawnsArriveQuestPartUtility
{
	[CompilerGenerated]
	private sealed class _003CGetQuestLookTargets_003Ed__0 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<Pawn> pawns;

		public IEnumerable<Pawn> _003C_003E3__pawns;

		private IEnumerator<Pawn> _003C_003E7__wrap1;

		private Pawn _003Cp_003E5__3;

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
		public _003CGetQuestLookTargets_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 2) <= 1u)
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
			_003Cp_003E5__3 = null;
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
					if (pawns.Count() == 1)
					{
						_003C_003E2__current = pawns.First();
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = pawns.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0118;
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00d8;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_0111;
					}
					IL_0111:
					_003Cp_003E5__3 = null;
					goto IL_0118;
					IL_0118:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cp_003E5__3 = _003C_003E7__wrap1.Current;
						if (_003Cp_003E5__3.Faction == Faction.OfPlayer || _003Cp_003E5__3.HostFaction == Faction.OfPlayer)
						{
							_003C_003E2__current = _003Cp_003E5__3;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_00d8;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_00d8:
					if (_003Cp_003E5__3.Faction == null && _003Cp_003E5__3.Downed)
					{
						_003C_003E2__current = _003Cp_003E5__3;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0111;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CGetQuestLookTargets_003Ed__0 _003CGetQuestLookTargets_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetQuestLookTargets_003Ed__ = this;
			}
			else
			{
				_003CGetQuestLookTargets_003Ed__ = new _003CGetQuestLookTargets_003Ed__0(0);
			}
			_003CGetQuestLookTargets_003Ed__.pawns = _003C_003E3__pawns;
			return _003CGetQuestLookTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetQuestLookTargets_003Ed__0))]
	public static IEnumerable<Pawn> GetQuestLookTargets(IEnumerable<Pawn> pawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetQuestLookTargets_003Ed__0(-2)
		{
			_003C_003E3__pawns = pawns
		};
	}

	public static bool IncreasesPopulation(IEnumerable<Pawn> pawns, bool joinPlayer, bool makePrisoners)
	{
		foreach (Pawn pawn in pawns)
		{
			if (pawn.RaceProps.Humanlike && !pawn.Destroyed && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || pawn.Downed || joinPlayer || makePrisoners))
			{
				return true;
			}
		}
		return false;
	}
}
