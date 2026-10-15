using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class PreceptComp
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptions_003Ed__10 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CGetDescriptions_003Ed__10(int _003C_003E1__state)
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
			PreceptComp preceptComp = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = preceptComp.description.CapitalizeFirst();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetDescriptions_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptions_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[MustTranslate]
	public string description;

	public PreceptDef preceptDef;

	public virtual IEnumerable<TraitRequirement> TraitsAffecting => Enumerable.Empty<TraitRequirement>();

	public virtual void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
	{
	}

	public virtual void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
	{
	}

	public virtual void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
	{
	}

	public virtual void Notify_MemberGenerated(Pawn pawn, Precept precept, bool newborn, bool ignoreApparel = false)
	{
	}

	public virtual void Notify_AddBedThoughts(Pawn pawn, Precept precept)
	{
	}

	public virtual bool MemberWillingToDo(HistoryEvent ev)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptions_003Ed__10))]
	public virtual IEnumerable<string> GetDescriptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptions_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual IEnumerable<string> ConfigErrors(PreceptDef parent)
	{
		return Enumerable.Empty<string>();
	}
}
