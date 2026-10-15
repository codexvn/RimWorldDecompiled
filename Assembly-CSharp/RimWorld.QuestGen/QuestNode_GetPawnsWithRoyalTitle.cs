using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.QuestGen;

public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
{
	[CompilerGenerated]
	private sealed class _003CGetFilteredPawns_003Ed__6 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Pawn> pawns;

		public List<Pawn> _003C_003E3__pawns;

		private int _003Ci_003E5__2;

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
		public _003CGetFilteredPawns_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0087;
			}
			_003C_003E1__state = -1;
			_ = QuestGen.slate;
			_003Ci_003E5__2 = 0;
			goto IL_0097;
			IL_0087:
			_003Ci_003E5__2++;
			goto IL_0097;
			IL_0097:
			if (_003Ci_003E5__2 < pawns.Count)
			{
				if (pawns[_003Ci_003E5__2].royalty != null && pawns[_003Ci_003E5__2].royalty.AllTitlesInEffectForReading.Any())
				{
					_003C_003E2__current = pawns[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0087;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CGetFilteredPawns_003Ed__6 _003CGetFilteredPawns_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFilteredPawns_003Ed__ = this;
			}
			else
			{
				_003CGetFilteredPawns_003Ed__ = new _003CGetFilteredPawns_003Ed__6(0);
			}
			_003CGetFilteredPawns_003Ed__.pawns = _003C_003E3__pawns;
			return _003CGetFilteredPawns_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public SlateRef<List<Pawn>> pawns;

	[NoTranslate]
	public SlateRef<string> storeAs;

	[NoTranslate]
	public SlateRef<string> storeCountAs;

	[NoTranslate]
	public SlateRef<string> storePawnsLabelAs;

	protected override bool TestRunInt(Slate slate)
	{
		return true;
	}

	protected override void RunInt()
	{
		Slate slate = QuestGen.slate;
		if (pawns.GetValue(slate) == null)
		{
			return;
		}
		IEnumerable<Pawn> filteredPawns = GetFilteredPawns(pawns.GetValue(slate));
		slate.Set(storeAs.GetValue(slate), filteredPawns);
		if (storeCountAs.GetValue(slate) != null)
		{
			slate.Set(storeCountAs.GetValue(slate), filteredPawns.Count());
		}
		if (storePawnsLabelAs.GetValue(slate) != null)
		{
			slate.Set(storePawnsLabelAs.GetValue(slate), filteredPawns.Select((Pawn p) => p.LabelNoCountColored.Resolve()).ToCommaList(useAnd: true));
		}
	}

	[IteratorStateMachine(typeof(_003CGetFilteredPawns_003Ed__6))]
	private IEnumerable<Pawn> GetFilteredPawns(List<Pawn> pawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFilteredPawns_003Ed__6(-2)
		{
			_003C_003E3__pawns = pawns
		};
	}
}
