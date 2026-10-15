using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
{
	[CompilerGenerated]
	private sealed class _003Cget_Hyperlinks_003Ed__8 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RequirementsToAcceptColonistWithTitle _003C_003E4__this;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003Cget_Hyperlinks_003Ed__8(int _003C_003E1__state)
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
			QuestPart_RequirementsToAcceptColonistWithTitle questPart_RequirementsToAcceptColonistWithTitle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_RequirementsToAcceptColonistWithTitle.minimumTitle, questPart_RequirementsToAcceptColonistWithTitle.faction);
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003Cget_Hyperlinks_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Hyperlinks_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public RoyalTitleDef minimumTitle;

	public Faction faction;

	public override bool RequiresAccepter => true;

	public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_Hyperlinks_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Hyperlinks_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override AcceptanceReport CanAccept()
	{
		foreach (Pawn allMapsCaravansAndTravellingTransporters_Alive_Colonist in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
		{
			if (CanPawnAccept(allMapsCaravansAndTravellingTransporters_Alive_Colonist))
			{
				return true;
			}
		}
		return new AcceptanceReport("QuestNoColonistWithTitle".Translate(minimumTitle.GetLabelCapForBothGenders()));
	}

	public override bool CanPawnAccept(Pawn p)
	{
		if (p.royalty == null)
		{
			return false;
		}
		RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(faction);
		if (currentTitle == null)
		{
			return false;
		}
		return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minimumTitle);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref minimumTitle, "minimumTitle");
		Scribe_References.Look(ref faction, "faction");
	}
}
