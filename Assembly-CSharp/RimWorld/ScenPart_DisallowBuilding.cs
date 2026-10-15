using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ScenPart_DisallowBuilding : ScenPart_Rule
{
	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_DisallowBuilding _003C_003E4__this;

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
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "DisallowBuilding")
				{
					_003C_003E2__current = scenPart_DisallowBuilding.building.LabelCap;
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

	[CompilerGenerated]
	private sealed class _003CRandomizableBuildingDefs_003Ed__10 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CRandomizableBuildingDefs_003Ed__10(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Wall;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Turret_MiniTurret;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.OrbitalTradeBeacon;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Battery;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.TrapSpike;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Cooler;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Heater;
				_003C_003E1__state = 7;
				return true;
			case 7:
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CRandomizableBuildingDefs_003Ed__10(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private ThingDef building;

	private const string DisallowBuildingTag = "DisallowBuilding";

	protected override void ApplyRule()
	{
		Current.Game.Rules.SetAllowBuilding(building, allowed: false);
	}

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
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

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref building, "building");
	}

	public override void Randomize()
	{
		building = RandomizableBuildingDefs().RandomElement();
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		if (!Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), building.LabelCap, drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			return;
		}
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		foreach (ThingDef item in from t in PossibleBuildingDefs()
			orderby t.label
			select t)
		{
			ThingDef localTd = item;
			list.Add(new FloatMenuOption(localTd.LabelCap, delegate
			{
				building = localTd;
			}));
		}
		Find.WindowStack.Add(new FloatMenu(list));
	}

	public override bool TryMerge(ScenPart other)
	{
		if (other is ScenPart_DisallowBuilding scenPart_DisallowBuilding && scenPart_DisallowBuilding.building == building)
		{
			return true;
		}
		return false;
	}

	protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
	{
		return DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Building && d.BuildableByPlayer);
	}

	[IteratorStateMachine(typeof(_003CRandomizableBuildingDefs_003Ed__10))]
	private IEnumerable<ThingDef> RandomizableBuildingDefs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomizableBuildingDefs_003Ed__10(-2);
	}

	public override bool HasNullDefs()
	{
		if (!base.HasNullDefs())
		{
			return building == null;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((building != null) ? building.GetHashCode() : 0);
	}
}
