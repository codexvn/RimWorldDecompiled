using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ScenPart_PermaGameCondition : ScenPart
{
	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__9 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_PermaGameCondition _003C_003E4__this;

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
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "PermaGameCondition")
				{
					_003C_003E2__current = scenPart_PermaGameCondition.gameCondition.LabelCap + ": " + scenPart_PermaGameCondition.gameCondition.description.CapitalizeFirst();
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

	private GameConditionDef gameCondition;

	public const string PermaGameConditionTag = "PermaGameCondition";

	public override string Label => "Permanent".Translate().CapitalizeFirst() + ": " + gameCondition.label.CapitalizeFirst();

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		if (!Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), gameCondition.LabelCap, drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			return;
		}
		FloatMenuUtility.MakeMenu(AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate
		{
			gameCondition = d;
		});
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref gameCondition, "gameCondition");
	}

	public override void Randomize()
	{
		gameCondition = AllowedGameConditions().RandomElement();
	}

	private IEnumerable<GameConditionDef> AllowedGameConditions()
	{
		return DefDatabase<GameConditionDef>.AllDefs.Where((GameConditionDef d) => d.canBePermanent);
	}

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
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

	public override void GenerateIntoMap(Map map)
	{
		GameCondition cond = GameConditionMaker.MakeConditionPermanent(gameCondition);
		map.gameConditionManager.RegisterCondition(cond);
	}

	public override bool CanCoexistWith(ScenPart other)
	{
		if (gameCondition == null)
		{
			return true;
		}
		if (other is ScenPart_PermaGameCondition scenPart_PermaGameCondition && !gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition))
		{
			return false;
		}
		return true;
	}

	public override bool HasNullDefs()
	{
		if (!base.HasNullDefs())
		{
			return gameCondition == null;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((gameCondition != null) ? gameCondition.GetHashCode() : 0);
	}
}
