using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class ChoiceLetter_AcceptVisitors : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__13 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_AcceptVisitors _003C_003E4__this;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__13(int _003C_003E1__state)
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
			ChoiceLetter_AcceptVisitors choiceLetter_AcceptVisitors = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!choiceLetter_AcceptVisitors.ArchivedOnly)
				{
					_003C_003E2__current = choiceLetter_AcceptVisitors.Option_Accept;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = choiceLetter_AcceptVisitors.Option_Close;
				_003C_003E1__state = 4;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = choiceLetter_AcceptVisitors.Option_RejectWithCharityConfirmation;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = choiceLetter_AcceptVisitors.Option_Postpone;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00b3;
			case 4:
				_003C_003E1__state = -1;
				goto IL_00b3;
			case 5:
				_003C_003E1__state = -1;
				goto IL_00dc;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00b3:
				if (choiceLetter_AcceptVisitors.lookTargets.IsValid())
				{
					_003C_003E2__current = choiceLetter_AcceptVisitors.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_00dc;
				IL_00dc:
				if (choiceLetter_AcceptVisitors.quest != null && !choiceLetter_AcceptVisitors.quest.hidden)
				{
					_003C_003E2__current = choiceLetter_AcceptVisitors.Option_ViewInQuestsTab("ViewRelatedQuest", postpone: true);
					_003C_003E1__state = 6;
					return true;
				}
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
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	public List<Pawn> pawns = new List<Pawn>();

	public string acceptedSignal;

	public string rejectedSignal;

	public bool charity;

	public override bool CanDismissWithRightClick => false;

	public override bool CanShowInLetterStack
	{
		get
		{
			if (!base.CanShowInLetterStack)
			{
				return false;
			}
			if (quest == null || quest.State != QuestState.Ongoing)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				if (CanStillAccept(pawns[i]))
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}

	private DiaOption Option_Accept
	{
		get
		{
			DiaOption diaOption = new DiaOption("AcceptButton".Translate());
			diaOption.action = delegate
			{
				pawns.RemoveAll((Pawn x) => !CanStillAccept(x));
				if (!acceptedSignal.NullOrEmpty())
				{
					foreach (Pawn pawn in pawns)
					{
						QuestUtility.SendQuestTargetSignals(pawn.questTags, "Recruited", pawn.Named("SUBJECT"));
					}
					object arg = ((pawns.Count == 1) ? ((object)pawns[0]) : ((object)pawns));
					Find.SignalManager.SendSignal(new Signal(acceptedSignal, arg.Named("SUBJECT")));
				}
				Find.LetterStack.RemoveLetter(this);
			};
			diaOption.resolveTree = true;
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				if (CanStillAccept(pawns[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}
	}

	private DiaOption Option_RejectWithCharityConfirmation => new DiaOption("RejectLetter".Translate())
	{
		action = delegate
		{
			Action action = delegate
			{
				if (!rejectedSignal.NullOrEmpty())
				{
					object arg = ((pawns.Count == 1) ? ((object)pawns[0]) : ((object)pawns));
					Find.SignalManager.SendSignal(new Signal(rejectedSignal, arg.Named("SUBJECT")));
				}
				Find.LetterStack.RemoveLetter(this);
			};
			if (!ModsConfig.IdeologyActive || !charity)
			{
				action();
			}
			else
			{
				IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
				if (source.Any())
				{
					string text = "";
					foreach (IGrouping<Ideo, Pawn> item in from c in source
						group c by c.Ideo)
					{
						text = text + "\n- " + "BelieversIn".Translate(item.Key.name.Colorize(item.Key.TextColor), item.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList()).Resolve();
					}
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmationCharityJoiner".Translate(text), action));
				}
				else
				{
					action();
				}
			}
		},
		resolveTree = true
	};

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__13))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__13(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Values.Look(ref acceptedSignal, "acceptedSignal");
		Scribe_Values.Look(ref rejectedSignal, "rejectedSignal");
		Scribe_Values.Look(ref charity, "charity", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}

	private bool CanStillAccept(Pawn p)
	{
		if (p.DestroyedOrNull() || !p.SpawnedOrAnyParentSpawned)
		{
			return false;
		}
		if (p.CurJob != null && p.CurJob.exitMapOnArrival)
		{
			return false;
		}
		if (p.Faction != null && p.Faction.HostileTo(Faction.OfPlayer))
		{
			return false;
		}
		Lord lord = p.GetLord();
		if (lord != null)
		{
			if (lord.CurLordToil is LordToil_ExitMap || lord.CurLordToil is LordToil_ExitMapRandom)
			{
				return false;
			}
			if (lord.LordJob is LordJob_VisitColony lordJob_VisitColony && lordJob_VisitColony.exitSubgraph.lordToils.Contains(lord.CurLordToil))
			{
				return false;
			}
		}
		return true;
	}
}
