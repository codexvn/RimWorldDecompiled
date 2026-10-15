using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
{
	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_ScatterThingsAnywhere _003C_003E4__this;

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
		public _003CGetSummaryListEntries_003Ed__4(int _003C_003E1__state)
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
			ScenPart_ScatterThingsAnywhere scenPart_ScatterThingsAnywhere = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "MapScatteredWith")
				{
					_003C_003E2__current = GenLabel.ThingLabel(scenPart_ScatterThingsAnywhere.thingDef, scenPart_ScatterThingsAnywhere.stuff, scenPart_ScatterThingsAnywhere.count).CapitalizeFirst();
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetSummaryListEntries_003Ed__4 _003CGetSummaryListEntries_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSummaryListEntries_003Ed__ = this;
			}
			else
			{
				_003CGetSummaryListEntries_003Ed__ = new _003CGetSummaryListEntries_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetSummaryListEntries_003Ed__.tag = _003C_003E3__tag;
			return _003CGetSummaryListEntries_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public const string MapScatteredWithTag = "MapScatteredWith";

	protected override bool NearPlayerStart => false;

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
	}

	[IteratorStateMachine(typeof(_003CGetSummaryListEntries_003Ed__4))]
	public override IEnumerable<string> GetSummaryListEntries(string tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSummaryListEntries_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}
}
