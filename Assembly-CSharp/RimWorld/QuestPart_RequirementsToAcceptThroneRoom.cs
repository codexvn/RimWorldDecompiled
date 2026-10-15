using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_RequirementsToAcceptThroneRoom : QuestPart_RequirementsToAccept
{
	[CompilerGenerated]
	private sealed class _003Cget_Hyperlinks_003Ed__4 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RequirementsToAcceptThroneRoom _003C_003E4__this;

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
		public _003Cget_Hyperlinks_003Ed__4(int _003C_003E1__state)
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
			QuestPart_RequirementsToAcceptThroneRoom questPart_RequirementsToAcceptThroneRoom = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_RequirementsToAcceptThroneRoom.forTitle, questPart_RequirementsToAcceptThroneRoom.faction);
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
			_003Cget_Hyperlinks_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Hyperlinks_003Ed__4(0)
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

	public RoyalTitleDef forTitle;

	public Faction faction;

	public Pawn forPawn;

	private List<string> acceptanceReportUnmetRequirements = new List<string>();

	public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_Hyperlinks_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Hyperlinks_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override AcceptanceReport CanAccept()
	{
		acceptanceReportUnmetRequirements.Clear();
		if (forTitle.throneRoomRequirements.NullOrEmpty())
		{
			return true;
		}
		Building_Throne assignedThrone = forPawn.ownership.AssignedThrone;
		if (assignedThrone == null)
		{
			return "QuestNoThroneRoom".Translate(forPawn.Named("PAWN"));
		}
		foreach (RoomRequirement throneRoomRequirement in forTitle.throneRoomRequirements)
		{
			if (!throneRoomRequirement.MetOrDisabled(assignedThrone.GetRoom(), forPawn))
			{
				acceptanceReportUnmetRequirements.Add(throneRoomRequirement.LabelCap(assignedThrone.GetRoom()));
			}
		}
		if (acceptanceReportUnmetRequirements.Count != 0)
		{
			return new AcceptanceReport("QuestThroneRoomRequirementsUnsatisfied".Translate(forPawn.Named("PAWN"), forTitle.GetLabelFor(forPawn).Named("TITLE")) + ":\n\n" + acceptanceReportUnmetRequirements.ToLineList("- "));
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref forTitle, "forTitle");
		Scribe_References.Look(ref faction, "faction");
		Scribe_References.Look(ref forPawn, "forPawn");
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		if (forPawn == replace)
		{
			forPawn = with;
		}
	}
}
