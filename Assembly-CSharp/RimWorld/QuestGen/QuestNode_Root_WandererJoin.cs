using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BBE RID: 15294
	public abstract class QuestNode_Root_WandererJoin : QuestNode
	{
		// Token: 0x17003381 RID: 13185
		// (get) Token: 0x06014C56 RID: 85078 RVA: 0x00638D93 File Offset: 0x00636F93
		protected virtual int AllowKilledBeforeTicks
		{
			get
			{
				return 15000;
			}
		}

		// Token: 0x17003382 RID: 13186
		// (get) Token: 0x06014C57 RID: 85079 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool CanBeSpace
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06014C58 RID: 85080
		public abstract Pawn GeneratePawn();

		// Token: 0x06014C59 RID: 85081
		public abstract void SendLetter(Quest quest, Pawn pawn);

		// Token: 0x06014C5A RID: 85082 RVA: 0x00638D9C File Offset: 0x00636F9C
		protected virtual void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			quest.DropPods(map.Parent, Gen.YieldSingle<Pawn>(pawn), null, null, null, null, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, null);
		}

		// Token: 0x06014C5B RID: 85083 RVA: 0x00638DDC File Offset: 0x00636FDC
		protected virtual void AddLeftMapQuestParts(Quest quest, Pawn pawn)
		{
			Action <>9__2;
			quest.AnyPawnUnhealthy(Gen.YieldSingle<Pawn>(pawn), delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, delegate
			{
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate()
					{
						quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererLeftHealthy".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
					});
				}
				quest2.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014C5C RID: 85084 RVA: 0x00638E34 File Offset: 0x00637034
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map;
			if (!slate.TryGet<Map>("map", out map, false))
			{
				bool mustBeInfestable = false;
				bool canBeSpace = this.CanBeSpace;
				map = QuestGen_Get.GetMap(mustBeInfestable, null, canBeSpace);
			}
			if (!this.CanBeSpace)
			{
				quest.AcceptanceRequirementNotSpace(map.Parent);
			}
			Pawn pawn = this.GeneratePawn();
			this.AddSpawnPawnQuestParts(quest, map, pawn);
			slate.Set<Pawn>("pawn", pawn, false);
			this.SendLetter(quest, pawn);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftBehind");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.PlayerTended");
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
			string inSignal5 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");
			quest.End(QuestEndOutcome.Success, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			Action <>9__6;
			Action <>9__4;
			Action <>9__5;
			quest.Signal(inSignal, delegate
			{
				Quest quest = quest;
				int allowKilledBeforeTicks = this.AllowKilledBeforeTicks;
				Action action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate()
					{
						Quest quest2 = quest;
						Action action2;
						if ((action2 = <>9__6) == null)
						{
							action2 = (<>9__6 = delegate()
							{
								quest.Message("MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftToDie".Translate(pawn), MessageTypeDefOf.NegativeEvent, false, null, pawn, null);
							});
						}
						quest2.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				Action elseAction;
				if ((elseAction = <>9__5) == null)
				{
					elseAction = (<>9__5 = delegate()
					{
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest.AcceptedAfterTicks(allowKilledBeforeTicks, action, elseAction, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			Action <>9__7;
			quest.Signal(inSignal2, delegate
			{
				Quest quest = quest;
				Action action;
				if ((action = <>9__7) == null)
				{
					action = (<>9__7 = delegate()
					{
						quest.Message("MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftBehind".Translate(pawn), MessageTypeDefOf.NegativeEvent, false, null, pawn, null);
					});
				}
				quest.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererRecruited".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
			}, null, inSignal5, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal5, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Signal(inSignal4, delegate
			{
				this.AddLeftMapQuestParts(quest, pawn);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014C5D RID: 85085 RVA: 0x00638FB4 File Offset: 0x006371B4
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (!slate.TryGet<Map>("map", out map, false))
			{
				bool mustBeInfestable = false;
				bool canBeSpace = this.CanBeSpace;
				return QuestGen_Get.GetMap(mustBeInfestable, null, canBeSpace) != null;
			}
			if (!this.CanBeSpace)
			{
				return Find.AnyPlayerHomeMap != null;
			}
			return Find.RandomSurfacePlayerHomeMap != null;
		}
	}
}
