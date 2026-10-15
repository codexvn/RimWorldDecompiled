using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptComp_UnwillingToDo : PreceptComp
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptions_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp_UnwillingToDo _003C_003E4__this;

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
		public _003CGetDescriptions_003Ed__7(int _003C_003E1__state)
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
			PreceptComp_UnwillingToDo preceptComp_UnwillingToDo = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = "UnwillingToDoIdeoAction".Translate() + ": " + preceptComp_UnwillingToDo.eventDef.LabelCap;
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
			_003CGetDescriptions_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptions_003Ed__7(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_TraitsAffecting_003Ed__4 : IEnumerable<TraitRequirement>, IEnumerable, IEnumerator<TraitRequirement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TraitRequirement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp_UnwillingToDo _003C_003E4__this;

		private int _003Ci_003E5__2;

		TraitRequirement IEnumerator<TraitRequirement>.Current
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
		public _003Cget_TraitsAffecting_003Ed__4(int _003C_003E1__state)
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
			PreceptComp_UnwillingToDo preceptComp_UnwillingToDo = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
			}
			else
			{
				_003C_003E1__state = -1;
				if (preceptComp_UnwillingToDo.nullifyingTraits == null)
				{
					goto IL_0079;
				}
				_003Ci_003E5__2 = 0;
			}
			if (_003Ci_003E5__2 < preceptComp_UnwillingToDo.nullifyingTraits.Count)
			{
				_003C_003E2__current = preceptComp_UnwillingToDo.nullifyingTraits[_003Ci_003E5__2];
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_0079;
			IL_0079:
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
		IEnumerator<TraitRequirement> IEnumerable<TraitRequirement>.GetEnumerator()
		{
			_003Cget_TraitsAffecting_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_TraitsAffecting_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TraitRequirement>)this).GetEnumerator();
		}
	}

	public HistoryEventDef eventDef;

	public List<TraitRequirement> nullifyingTraits;

	public List<HediffDef> nullifyingHediffs;

	public override IEnumerable<TraitRequirement> TraitsAffecting
	{
		[IteratorStateMachine(typeof(_003Cget_TraitsAffecting_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_TraitsAffecting_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool MemberWillingToDo(HistoryEvent ev)
	{
		if (eventDef != null && ev.def != eventDef)
		{
			return true;
		}
		if (!ev.args.TryGetArg(HistoryEventArgsNames.Doer, out Pawn arg))
		{
			return false;
		}
		if (!preceptDef.enabledForNPCFactions && arg != null && !arg.Faction.IsPlayer)
		{
			return true;
		}
		if (nullifyingTraits != null && arg?.story != null)
		{
			for (int i = 0; i < nullifyingTraits.Count; i++)
			{
				if (nullifyingTraits[i].HasTrait(arg))
				{
					return true;
				}
			}
		}
		if (nullifyingHediffs != null && arg?.health?.hediffSet != null)
		{
			for (int j = 0; j < nullifyingHediffs.Count; j++)
			{
				if (!arg.health.hediffSet.HasHediff(nullifyingHediffs[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public virtual string GetProhibitionText()
	{
		return description ?? ((string)eventDef.LabelCap);
	}

	[IteratorStateMachine(typeof(_003CGetDescriptions_003Ed__7))]
	public override IEnumerable<string> GetDescriptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptions_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
