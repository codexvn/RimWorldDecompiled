using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039D9 RID: 14809
	public static class QuestGen_Signal
	{
		// Token: 0x06014523 RID: 83235 RVA: 0x00612E30 File Offset: 0x00611030
		public static void Signal(this Quest quest, string inSignal = null, Action action = null, IEnumerable<string> outSignals = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			Slate slate = QuestGen.slate;
			int num = ((outSignals != null) ? outSignals.Count<string>() : 0) + ((action != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal);
				if (action != null)
				{
					questPart_Pass.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInner(action, questPart_Pass.outSignal);
				}
				else
				{
					questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(outSignals.First<string>());
				}
				questPart_Pass.signalListenMode = signalListenMode;
				quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal);
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutMany.outSignals.Add(text);
				QuestGenUtility.RunInner(action, text);
			}
			foreach (string signal in outSignals)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			questPart_PassOutMany.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x06014524 RID: 83236 RVA: 0x00612F44 File Offset: 0x00611144
		public static void AnySignal(this Quest quest, IEnumerable<string> inSignals = null, Action action = null, IEnumerable<string> outSignals = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			Slate slate = QuestGen.slate;
			int num = ((outSignals != null) ? outSignals.Count<string>() : 0) + ((action != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_PassAny questPart_PassAny = new QuestPart_PassAny();
				foreach (string signal in inSignals)
				{
					questPart_PassAny.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (action != null)
				{
					questPart_PassAny.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInner(action, questPart_PassAny.outSignal);
				}
				else
				{
					questPart_PassAny.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(outSignals.First<string>());
				}
				questPart_PassAny.signalListenMode = signalListenMode;
				quest.AddPart(questPart_PassAny);
				return;
			}
			QuestPart_PassAnyOutMany questPart_PassAnyOutMany = new QuestPart_PassAnyOutMany();
			foreach (string signal2 in inSignals)
			{
				questPart_PassAnyOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAnyOutMany.outSignals.Add(text);
				QuestGenUtility.RunInner(action, text);
			}
			foreach (string signal3 in outSignals)
			{
				questPart_PassAnyOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAnyOutMany.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassAnyOutMany);
		}

		// Token: 0x06014525 RID: 83237 RVA: 0x006130C8 File Offset: 0x006112C8
		public static void SignalRandom(this Quest quest, IEnumerable<Action> actions, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_PassOutRandom questPart_PassOutRandom = new QuestPart_PassOutRandom();
			questPart_PassOutRandom.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassOutRandom.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassOutRandom);
			foreach (Action inner in actions)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutRandom.outSignals.Add(text);
				QuestGenUtility.RunInner(inner, text);
			}
		}

		// Token: 0x06014526 RID: 83238 RVA: 0x0061315C File Offset: 0x0061135C
		public static void SendSignals(this Quest quest, IEnumerable<string> outSignals, string outSignalsFormat = "", int outSignalsFormattedCount = 0)
		{
			Slate slate = QuestGen.slate;
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			if (outSignals != null)
			{
				enumerable = enumerable.Concat(outSignals);
			}
			if (outSignalsFormattedCount > 0)
			{
				for (int i = 0; i < outSignalsFormattedCount; i++)
				{
					enumerable = enumerable.Concat(Gen.YieldSingle<string>(outSignalsFormat.Formatted(i.Named("INDEX")).ToString()));
				}
			}
			if (enumerable.EnumerableNullOrEmpty<string>())
			{
				return;
			}
			if (enumerable.Count<string>() == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(enumerable.First<string>());
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (string signal in enumerable)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x06014527 RID: 83239 RVA: 0x00613284 File Offset: 0x00611484
		public static void SignalPassOutMany(this Quest quest, Action action = null, string inSignal = null, IEnumerable<string> outSignals = null)
		{
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (action != null)
			{
				string innerNodeInSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, innerNodeInSignal);
			}
			if (outSignals != null)
			{
				foreach (string item in outSignals)
				{
					questPart_PassOutMany.outSignals.Add(item);
				}
			}
			quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x06014528 RID: 83240 RVA: 0x00613314 File Offset: 0x00611514
		public static QuestPart_PassActivable SignalPassActivable(this Quest quest, Action action = null, string inSignalEnable = null, string inSignal = null, string outSignalCompleted = null, IEnumerable<string> outSignalsCompleted = null, string inSignalDisable = null, bool reactivatable = false)
		{
			QuestPart_PassActivable questPart_PassActivable = new QuestPart_PassActivable();
			questPart_PassActivable.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassActivable.inSignalDisable = inSignalDisable;
			questPart_PassActivable.inSignal = inSignal;
			questPart_PassActivable.reactivatable = reactivatable;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, text);
				questPart_PassActivable.outSignalsCompleted.Add(text);
			}
			if (outSignalsCompleted != null)
			{
				questPart_PassActivable.outSignalsCompleted.AddRange(outSignalsCompleted);
			}
			if (!outSignalCompleted.NullOrEmpty())
			{
				questPart_PassActivable.outSignalsCompleted.Add(outSignalCompleted);
			}
			quest.AddPart(questPart_PassActivable);
			return questPart_PassActivable;
		}

		// Token: 0x06014529 RID: 83241 RVA: 0x006133AC File Offset: 0x006115AC
		public static void SignalPass(this Quest quest, Action action = null, string inSignal = null, string outSignal = null)
		{
			QuestPart_Pass questPart_Pass = new QuestPart_Pass();
			questPart_Pass.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_Pass.outSignal = outSignal;
			quest.AddPart(questPart_Pass);
		}

		// Token: 0x0601452A RID: 83242 RVA: 0x00613408 File Offset: 0x00611608
		public static void SignalPassAll(this Quest quest, Action action = null, List<string> inSignals = null, string outSignal = null)
		{
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			QuestPart_PassAll questPart_PassAll2 = questPart_PassAll;
			List<string> inSignals2 = inSignals;
			if (inSignals == null)
			{
				(inSignals2 = new List<string>()).Add(QuestGen.slate.Get<string>("inSignal", null, false));
			}
			questPart_PassAll2.inSignals = inSignals2;
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_PassAll.outSignal = outSignal;
			quest.AddPart(questPart_PassAll);
		}

		// Token: 0x0601452B RID: 83243 RVA: 0x0061346C File Offset: 0x0061166C
		public static void SignalPassAny(this Quest quest, Action action = null, List<string> inSignals = null, string outSignal = null)
		{
			QuestPart_PassAny questPart_PassAny = new QuestPart_PassAny();
			QuestPart_PassAny questPart_PassAny2 = questPart_PassAny;
			List<string> inSignals2 = inSignals;
			if (inSignals == null)
			{
				(inSignals2 = new List<string>()).Add(QuestGen.slate.Get<string>("inSignal", null, false));
			}
			questPart_PassAny2.inSignals = inSignals2;
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_PassAny.outSignal = outSignal;
			quest.AddPart(questPart_PassAny);
		}

		// Token: 0x0601452C RID: 83244 RVA: 0x006134D0 File Offset: 0x006116D0
		public static void SignalPassAllSequence(this Quest quest, Action action = null, List<string> inSignals = null, string outSignal = null)
		{
			QuestPart_PassAllSequence questPart_PassAllSequence = new QuestPart_PassAllSequence();
			QuestPart_PassAllSequence questPart_PassAllSequence2 = questPart_PassAllSequence;
			List<string> inSignals2 = inSignals;
			if (inSignals == null)
			{
				(inSignals2 = new List<string>()).Add(QuestGen.slate.Get<string>("inSignal", null, false));
			}
			questPart_PassAllSequence2.inSignals = inSignals2;
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_PassAllSequence.outSignal = outSignal;
			quest.AddPart(questPart_PassAllSequence);
		}

		// Token: 0x0601452D RID: 83245 RVA: 0x00613534 File Offset: 0x00611734
		public static void SignalPassWithFaction(this Quest quest, Faction faction, Action action = null, Action outAction = null, string inSignal = null, string outSignal = null)
		{
			QuestPart_PassWithFactionArg questPart_PassWithFactionArg = new QuestPart_PassWithFactionArg();
			questPart_PassWithFactionArg.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassWithFactionArg.faction = faction;
			questPart_PassWithFactionArg.outSignal = outSignal;
			if (action != null)
			{
				string innerNodeInSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, innerNodeInSignal);
			}
			if (outAction != null)
			{
				if (questPart_PassWithFactionArg.outSignal == null)
				{
					questPart_PassWithFactionArg.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				}
				QuestGenUtility.RunInner(outAction, questPart_PassWithFactionArg.outSignal);
			}
			quest.AddPart(questPart_PassWithFactionArg);
		}
	}
}
