using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

[StaticConstructorOnStartup]
public class BookOutcomeDoer_GiveQuest : BookOutcomeDoer
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BookOutcomeDoer_GiveQuest _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__17(int _003C_003E1__state)
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
			BookOutcomeDoer_GiveQuest CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals4.QuestGiven)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandViewQuest".Translate(CS_0024_003C_003E8__locals4.quest.name),
						defaultDesc = "CommandViewQuestDesc".Translate(),
						icon = ViewQuestCommandTex,
						action = delegate
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(CS_0024_003C_003E8__locals4.quest);
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				if (DebugSettings.godMode)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Give quest",
						icon = TexUI.Placeholder,
						action = delegate
						{
							CS_0024_003C_003E8__locals4.giveNext = true;
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTopicRulePacks_003Ed__18 : IEnumerable<RulePack>, IEnumerable, IEnumerator<RulePack>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RulePack _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BookOutcomeDoer_GiveQuest _003C_003E4__this;

		RulePack IEnumerator<RulePack>.Current
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
		public _003CGetTopicRulePacks_003Ed__18(int _003C_003E1__state)
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
			BookOutcomeDoer_GiveQuest bookOutcomeDoer_GiveQuest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!bookOutcomeDoer_GiveQuest.hasQuest)
				{
					return false;
				}
				_003C_003E2__current = bookOutcomeDoer_GiveQuest.questDef.questSubjectRules;
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
		IEnumerator<RulePack> IEnumerable<RulePack>.GetEnumerator()
		{
			_003CGetTopicRulePacks_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTopicRulePacks_003Ed__18(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RulePack>)this).GetEnumerator();
		}
	}

	private bool hasQuest;

	private QuestScriptDef questDef;

	private Quest quest;

	private bool giveNext;

	private const int ReceiveQuestMTBTicks = 12500;

	private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest");

	public new BookOutcomeProperties_GiveQuest Props => (BookOutcomeProperties_GiveQuest)props;

	private bool QuestGiven => quest != null;

	public override bool BenefitDetailsCanChange(Pawn reader = null)
	{
		if (!QuestGiven)
		{
			return hasQuest;
		}
		return false;
	}

	public override string GetBenefitsString(Pawn reader = null)
	{
		if (!hasQuest)
		{
			return null;
		}
		TaggedString ts = " - " + "BookHasQuest".Translate();
		if (QuestGiven)
		{
			ts += $" ({"DiscoveredQuest".Translate()})";
			ts = ts.Colorize(Color.gray);
		}
		return ts.Resolve();
	}

	public override bool DoesProvidesOutcome(Pawn reader)
	{
		if (hasQuest)
		{
			return !QuestGiven;
		}
		return false;
	}

	public override void OnBookGenerated(Pawn author = null)
	{
		if (ModsConfig.OdysseyActive)
		{
			hasQuest = Rand.Chance(Props.questChance);
			if (hasQuest)
			{
				questDef = GetQuestDef();
			}
			if (questDef == null)
			{
				hasQuest = false;
			}
		}
	}

	private QuestScriptDef GetQuestDef()
	{
		IEnumerable<QuestScriptDef> giverQuests = QuestUtility.GetGiverQuests(QuestGiverTag.Reading);
		if (giverQuests.EnumerableNullOrEmpty())
		{
			return null;
		}
		return giverQuests.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight);
	}

	public override void OnReadingTick(Pawn reader, float factor)
	{
		if (hasQuest && !QuestGiven && (Rand.MTBEventOccurs(12500f, 1f, 1f) || giveNext))
		{
			GenerateQuest(reader);
		}
	}

	private void GenerateQuest(Pawn reader)
	{
		Slate slate = new Slate();
		slate.Set("points", StorytellerUtility.DefaultThreatPointsNow(reader.Map));
		slate.Set("discoveryMethod", "QuestDiscoveredFromBook".Translate(base.Book.Named("BOOK"), reader.Named("READER")));
		if (questDef == null)
		{
			questDef = GetQuestDef();
		}
		if (questDef != null)
		{
			quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, slate);
			Messages.Message("MessageBookGaveQuest".Translate(quest.name, base.Book.Named("BOOK"), reader.Named("READER")), MessageTypeDefOf.PositiveEvent);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod"));
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__17))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetTopicRulePacks_003Ed__18))]
	public override IEnumerable<RulePack> GetTopicRulePacks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTopicRulePacks_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref hasQuest, "hasQuest", defaultValue: false);
		Scribe_Defs.Look(ref questDef, "questDef");
		Scribe_References.Look(ref quest, "quest");
	}
}
