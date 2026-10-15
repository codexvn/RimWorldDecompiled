using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class ScenPart_IncidentBase : ScenPart
{
	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__9 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_IncidentBase _003C_003E4__this;

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
		public _003CGetSummaryListEntries_003Ed__9(int _003C_003E1__state)
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
			ScenPart_IncidentBase scenPart_IncidentBase = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == scenPart_IncidentBase.IncidentTag)
				{
					_003C_003E2__current = scenPart_IncidentBase.incident.LabelCap;
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
			_003CGetSummaryListEntries_003Ed__9 _003CGetSummaryListEntries_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSummaryListEntries_003Ed__ = this;
			}
			else
			{
				_003CGetSummaryListEntries_003Ed__ = new _003CGetSummaryListEntries_003Ed__9(0)
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

	protected IncidentDef incident;

	public IncidentDef Incident => incident;

	protected abstract string IncidentTag { get; }

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref incident, "incident");
		if (Scribe.mode == LoadSaveMode.PostLoadInit && incident == null)
		{
			incident = RandomizableIncidents().FirstOrDefault();
			Log.Error("ScenPart has null incident after loading. Changing to " + incident.ToStringSafe());
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((incident != null) ? incident.GetHashCode() : 0);
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
		DoIncidentEditInterface(scenPartRect);
	}

	public override string Summary(Scenario scen)
	{
		string key = "ScenPart_" + IncidentTag;
		return ScenSummaryList.SummaryWithList(scen, IncidentTag, key.Translate());
	}

	[IteratorStateMachine(typeof(_003CGetSummaryListEntries_003Ed__9))]
	public override IEnumerable<string> GetSummaryListEntries(string tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSummaryListEntries_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}

	public override void Randomize()
	{
		incident = RandomizableIncidents().RandomElement();
	}

	public override bool TryMerge(ScenPart other)
	{
		if (other is ScenPart_IncidentBase scenPart_IncidentBase)
		{
			return scenPart_IncidentBase.Incident == incident;
		}
		return false;
	}

	public override bool CanCoexistWith(ScenPart other)
	{
		if (other is ScenPart_IncidentBase scenPart_IncidentBase)
		{
			return scenPart_IncidentBase.Incident != incident;
		}
		return true;
	}

	protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
	{
		return Enumerable.Empty<IncidentDef>();
	}

	protected void DoIncidentEditInterface(Rect rect)
	{
		if (!Widgets.ButtonText(rect, incident.LabelCap, drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			return;
		}
		FloatMenuUtility.MakeMenu(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate
		{
			incident = id;
		});
	}
}
