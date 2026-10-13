using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020039B4 RID: 14772
	public static class QuestGen
	{
		// Token: 0x1700333A RID: 13114
		// (get) Token: 0x0601442F RID: 82991 RVA: 0x0060C953 File Offset: 0x0060AB53
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		// Token: 0x1700333B RID: 13115
		// (get) Token: 0x06014430 RID: 82992 RVA: 0x0060C95A File Offset: 0x0060AB5A
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		// Token: 0x1700333C RID: 13116
		// (get) Token: 0x06014431 RID: 82993 RVA: 0x0060C961 File Offset: 0x0060AB61
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		// Token: 0x1700333D RID: 13117
		// (get) Token: 0x06014432 RID: 82994 RVA: 0x0060C968 File Offset: 0x0060AB68
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		// Token: 0x1700333E RID: 13118
		// (get) Token: 0x06014433 RID: 82995 RVA: 0x0060C96F File Offset: 0x0060AB6F
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		// Token: 0x1700333F RID: 13119
		// (get) Token: 0x06014434 RID: 82996 RVA: 0x0060C976 File Offset: 0x0060AB76
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		// Token: 0x17003340 RID: 13120
		// (get) Token: 0x06014435 RID: 82997 RVA: 0x0060C97D File Offset: 0x0060AB7D
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		// Token: 0x17003341 RID: 13121
		// (get) Token: 0x06014436 RID: 82998 RVA: 0x0060C984 File Offset: 0x0060AB84
		public static List<Rule> QuestContentRulesReadOnly
		{
			get
			{
				return QuestGen.questContentRules;
			}
		}

		// Token: 0x06014437 RID: 82999 RVA: 0x0060C98C File Offset: 0x0060AB8C
		public static string GenerateNewSignal(string signalString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedSignals.TryGetValue(signalString, out num))
			{
				num = 0;
			}
			string result = string.Format("Quest{0}.{1}{2}", QuestGen.quest.id, signalString, (num == 0) ? "" : (num + 1).ToString());
			QuestGen.generatedSignals[signalString] = num + 1;
			return result;
		}

		// Token: 0x06014438 RID: 83000 RVA: 0x0060C9EC File Offset: 0x0060ABEC
		public static string GenerateNewTargetQuestTag(string targetString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedTargetQuestTags.TryGetValue(targetString, out num))
			{
				num = 0;
			}
			string result = string.Format("Quest{0}.{1}{2}", QuestGen.quest.id, targetString, (num == 0) ? "" : (num + 1).ToString());
			QuestGen.generatedTargetQuestTags[targetString] = num + 1;
			return result;
		}

		// Token: 0x06014439 RID: 83001 RVA: 0x0060CA4A File Offset: 0x0060AC4A
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		// Token: 0x0601443A RID: 83002 RVA: 0x0060CA60 File Offset: 0x0060AC60
		public static string GenerateResolvedQuestName(QuestScriptDef root, Slate initialVars)
		{
			if (QuestGen.working)
			{
				Log.Error("Cannot generated quest name while generating another quest.");
				return null;
			}
			QuestGen.InitializeQuestGen(root, initialVars);
			root.InitializeRules();
			try
			{
				QuestNode_ResolveQuestName.Resolve();
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Error while generating quest name: {0}", arg));
			}
			string name = QuestGen.quest.name;
			QuestGen.ClearQuestGenState();
			return name;
		}

		// Token: 0x0601443B RID: 83003 RVA: 0x0060CAC8 File Offset: 0x0060ACC8
		public static Quest Generate(QuestScriptDef root, Slate initialVars)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start("Generate quest");
			}
			Quest result = null;
			try
			{
				if (QuestGen.working)
				{
					throw new Exception("Called Generate() while already working.");
				}
				QuestGen.InitializeQuestGen(root, initialVars);
				root.Run();
				if (!root.everAcceptableInSpace && !root.autoAccept)
				{
					QuestGen.quest.AcceptanceRequirementNotSpace(null);
				}
				try
				{
					QuestNode_ResolveQuestName.Resolve();
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error while generating quest name: {0}", arg));
				}
				try
				{
					QuestNode_ResolveQuestDescription.Resolve();
				}
				catch (Exception arg2)
				{
					Log.Error(string.Format("Error while generating quest description: {0}", arg2));
				}
				try
				{
					QuestNode_ResolveTextRequests.Resolve();
				}
				catch (Exception arg3)
				{
					Log.Error(string.Format("Error while resolving text requests: {0}", arg3));
				}
				QuestGen.AddSlateQuestTags();
				bool flag = root.autoAccept;
				if (flag)
				{
					List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i].PreventsAutoAccept)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					QuestGen.quest.SetInitiallyAccepted();
				}
				Site site;
				if (QuestGen.slate.TryGet<Site>("site", out site, false) && site.MainSitePartDef.copyQuestName)
				{
					site.customLabel = QuestGen.quest.name;
				}
				result = QuestGen.quest;
			}
			catch (Exception arg4)
			{
				Log.Error(string.Format("Error in QuestGen: {0}", arg4));
			}
			finally
			{
				if (DeepProfiler.enabled)
				{
					DeepProfiler.End();
				}
				QuestGen.ClearQuestGenState();
			}
			return result;
		}

		// Token: 0x0601443C RID: 83004 RVA: 0x0060CCAC File Offset: 0x0060AEAC
		private static void InitializeQuestGen(QuestScriptDef root, Slate initialVars)
		{
			QuestGen.working = true;
			QuestGen.root = root;
			QuestGen.slate.Reset();
			QuestGen.slate.SetAll(initialVars);
			QuestGen.quest = Quest.MakeRaw();
			if (root.expireDaysRange.max > 0f)
			{
				QuestGen.quest.acceptanceExpireTick = GenTicks.TicksGame + (int)(root.expireDaysRange.RandomInRange * 60000f);
			}
			if (root.defaultChallengeRating > 0)
			{
				QuestGen.quest.challengeRating = root.defaultChallengeRating;
			}
			QuestGen.quest.root = root;
			QuestGen.quest.hidden = root.defaultHidden;
			QuestGen.quest.charity = root.defaultCharity;
			QuestGen.slate.SetIfNone<string>("inSignal", QuestGen.quest.InitiateSignal, false);
		}

		// Token: 0x0601443D RID: 83005 RVA: 0x0060CD78 File Offset: 0x0060AF78
		private static void ClearQuestGenState()
		{
			QuestGen.quest = null;
			QuestGen.root = null;
			QuestGen.working = false;
			QuestGen.generatedPawns.Clear();
			QuestGen.textRequests.Clear();
			QuestGen.slate.Reset();
			QuestGen.questDescriptionRules.Clear();
			QuestGen.questDescriptionConstants.Clear();
			QuestGen.questNameRules.Clear();
			QuestGen.questNameConstants.Clear();
			QuestGen.questContentRules.Clear();
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
			QuestGen.ResetIdCounters();
		}

		// Token: 0x0601443E RID: 83006 RVA: 0x0060CDF6 File Offset: 0x0060AFF6
		public static void AddToGeneratedPawns(Pawn pawn)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a pawn to generated pawns while not resolving any quest.");
				return;
			}
			if (!QuestGen.generatedPawns.Contains(pawn))
			{
				QuestGen.generatedPawns.Add(pawn);
			}
		}

		// Token: 0x0601443F RID: 83007 RVA: 0x0060CE22 File Offset: 0x0060B022
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		// Token: 0x06014440 RID: 83008 RVA: 0x0060CE38 File Offset: 0x0060B038
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		// Token: 0x06014441 RID: 83009 RVA: 0x0060CE45 File Offset: 0x0060B045
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.");
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x06014442 RID: 83010 RVA: 0x0060CE6C File Offset: 0x0060B06C
		public static void AddQuestDescriptionConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questDescriptionConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questDescriptionConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06014443 RID: 83011 RVA: 0x0060CEF8 File Offset: 0x0060B0F8
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		// Token: 0x06014444 RID: 83012 RVA: 0x0060CF05 File Offset: 0x0060B105
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.");
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x06014445 RID: 83013 RVA: 0x0060CF2C File Offset: 0x0060B12C
		public static void AddQuestNameConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questNameConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questNameConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06014446 RID: 83014 RVA: 0x0060CFB8 File Offset: 0x0060B1B8
		public static void AddQuestContentRules(RulePack rulePack)
		{
			QuestGen.AddQuestContentRules(rulePack.Rules);
		}

		// Token: 0x06014447 RID: 83015 RVA: 0x0060CFC5 File Offset: 0x0060B1C5
		public static void AddQuestContentRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest content rules while not resolving any quest.");
				return;
			}
			QuestGen.questContentRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x06014448 RID: 83016 RVA: 0x0060CFE9 File Offset: 0x0060B1E9
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		// Token: 0x06014449 RID: 83017 RVA: 0x0060D003 File Offset: 0x0060B203
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		// Token: 0x0601444A RID: 83018 RVA: 0x0060D018 File Offset: 0x0060B218
		public static void AddTextRequest(string localKeyword, Action<string> setter, List<Rule> extraLocalRules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a text request while not resolving any quest.");
				return;
			}
			QuestTextRequest questTextRequest = new QuestTextRequest();
			questTextRequest.keyword = localKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTextRequest.keyword = QuestGen.slate.CurrentPrefix + "/" + questTextRequest.keyword;
			}
			questTextRequest.keyword = QuestGenUtility.NormalizeVarPath(questTextRequest.keyword);
			questTextRequest.setter = setter;
			if (extraLocalRules != null)
			{
				questTextRequest.extraRules = QuestGenUtility.AppendCurrentPrefix(extraLocalRules);
			}
			QuestGen.textRequests.Add(questTextRequest);
		}

		// Token: 0x0601444B RID: 83019 RVA: 0x0060D0A8 File Offset: 0x0060B2A8
		private static void AddSlateQuestTags()
		{
			for (int i = 0; i < QuestGen.slateQuestTagsToAddWhenFinished.Count; i++)
			{
				object obj;
				if (QuestGen.slate.TryGet<object>(QuestGen.slateQuestTagsToAddWhenFinished[i], out obj, true))
				{
					string questTagToAdd = QuestGen.GenerateNewTargetQuestTag(QuestGen.slateQuestTagsToAddWhenFinished[i], false);
					QuestUtility.AddQuestTag(obj, questTagToAdd);
				}
			}
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
		}

		// Token: 0x0400DFDC RID: 57308
		public static Quest quest;

		// Token: 0x0400DFDD RID: 57309
		public static Slate slate = new Slate();

		// Token: 0x0400DFDE RID: 57310
		private static QuestScriptDef root;

		// Token: 0x0400DFDF RID: 57311
		private static bool working;

		// Token: 0x0400DFE0 RID: 57312
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		// Token: 0x0400DFE1 RID: 57313
		private static List<Pawn> generatedPawns = new List<Pawn>();

		// Token: 0x0400DFE2 RID: 57314
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		// Token: 0x0400DFE3 RID: 57315
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		// Token: 0x0400DFE4 RID: 57316
		private static List<Rule> questDescriptionRules = new List<Rule>();

		// Token: 0x0400DFE5 RID: 57317
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		// Token: 0x0400DFE6 RID: 57318
		private static List<Rule> questNameRules = new List<Rule>();

		// Token: 0x0400DFE7 RID: 57319
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		// Token: 0x0400DFE8 RID: 57320
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();

		// Token: 0x0400DFE9 RID: 57321
		private static List<Rule> questContentRules = new List<Rule>();
	}
}
