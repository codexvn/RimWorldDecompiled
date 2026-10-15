using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptComp_UnwillingToDo_WithDef : PreceptComp_UnwillingToDo
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptions_003Ed__2 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp_UnwillingToDo_WithDef _003C_003E4__this;

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
		public _003CGetDescriptions_003Ed__2(int _003C_003E1__state)
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
			PreceptComp_UnwillingToDo_WithDef preceptComp_UnwillingToDo_WithDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = "UnwillingToDoIdeoAction".Translate() + ": " + "BuildThing".Translate(preceptComp_UnwillingToDo_WithDef.buildingDef);
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
			_003CGetDescriptions_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptions_003Ed__2(0)
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

	public ThingDef buildingDef;

	public override bool MemberWillingToDo(HistoryEvent ev)
	{
		if (eventDef != null && ev.def != eventDef)
		{
			return true;
		}
		ev.args.TryGetArg(HistoryEventArgsNames.Doer, out Pawn arg);
		ev.args.TryGetArg(HistoryEventArgsNames.Building, out ThingDef arg2);
		if (arg2 == null)
		{
			return true;
		}
		if (nullifyingTraits != null && arg != null && arg.story != null)
		{
			if (!preceptDef.enabledForNPCFactions && !arg.Faction.IsPlayer)
			{
				return true;
			}
			for (int i = 0; i < nullifyingTraits.Count; i++)
			{
				if (nullifyingTraits[i].HasTrait(arg))
				{
					return true;
				}
			}
		}
		return arg2 != buildingDef;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptions_003Ed__2))]
	public override IEnumerable<string> GetDescriptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptions_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
