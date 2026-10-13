using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020039B5 RID: 14773
	public static class QuestGenUtility
	{
		// Token: 0x0601444D RID: 83021 RVA: 0x0060D184 File Offset: 0x0060B384
		public static string HardcodedSignalWithQuestID(string signal)
		{
			if (!QuestGen.Working)
			{
				return signal;
			}
			if (signal.NullOrEmpty())
			{
				return null;
			}
			if (signal.StartsWith("Quest") && signal.IndexOf('.') >= 0)
			{
				return signal;
			}
			if (signal.IndexOf('.') >= 0)
			{
				int num = signal.IndexOf('.');
				string text = signal.Substring(0, num);
				string str = signal.Substring(num + 1);
				if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
				{
					text = QuestGen.slate.CurrentPrefix + "/" + text;
				}
				text = QuestGenUtility.NormalizeVarPath(text);
				QuestGen.AddSlateQuestTagToAddWhenFinished(text);
				return QuestGen.GenerateNewSignal(text + "." + str, false);
			}
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				signal = QuestGen.slate.CurrentPrefix + "/" + signal;
			}
			signal = QuestGenUtility.NormalizeVarPath(signal);
			return QuestGen.GenerateNewSignal(signal, false);
		}

		// Token: 0x0601444E RID: 83022 RVA: 0x0060D264 File Offset: 0x0060B464
		public static string HardcodedTargetQuestTagWithQuestID(string questTag)
		{
			if (!QuestGen.Working)
			{
				return questTag;
			}
			if (questTag.NullOrEmpty())
			{
				return null;
			}
			if (questTag.StartsWith("Quest") && questTag.IndexOf('.') >= 0)
			{
				return questTag;
			}
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTag = QuestGen.slate.CurrentPrefix + "/" + questTag;
			}
			questTag = QuestGenUtility.NormalizeVarPath(questTag);
			return QuestGen.GenerateNewTargetQuestTag(questTag, false);
		}

		// Token: 0x0601444F RID: 83023 RVA: 0x0060D2D5 File Offset: 0x0060B4D5
		public static string QuestTagSignal(string questTag, string signal)
		{
			return questTag + "." + signal;
		}

		// Token: 0x06014450 RID: 83024 RVA: 0x0060D2E4 File Offset: 0x0060B4E4
		public static void RunInner(Action inner, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInner(inner, text);
		}

		// Token: 0x06014451 RID: 83025 RVA: 0x0060D310 File Offset: 0x0060B510
		public static void RunInner(Action inner, string innerNodeInSignal)
		{
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo("inSignal");
			QuestGen.slate.Set<string>("inSignal", innerNodeInSignal, false);
			try
			{
				inner();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x06014452 RID: 83026 RVA: 0x0060D364 File Offset: 0x0060B564
		public static void RunInnerNode(QuestNode node, QuestPartActivable outerQuestPart)
		{
			string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			outerQuestPart.outSignalsCompleted.Add(text);
			QuestGenUtility.RunInnerNode(node, text);
		}

		// Token: 0x06014453 RID: 83027 RVA: 0x0060D390 File Offset: 0x0060B590
		public static void RunInnerNode(QuestNode node, string innerNodeInSignal)
		{
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo("inSignal");
			QuestGen.slate.Set<string>("inSignal", innerNodeInSignal, false);
			try
			{
				node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x06014454 RID: 83028 RVA: 0x0060D3E4 File Offset: 0x0060B5E4
		public static void AddSlateVars(ref GrammarRequest req)
		{
			QuestGenUtility.tmpAddedSlateVars.Clear();
			List<Rule> rules = req.Rules;
			for (int i = 0; i < rules.Count; i++)
			{
				Rule_String rule_String = rules[i] as Rule_String;
				if (rule_String != null)
				{
					string text = rule_String.Generate();
					if (text != null)
					{
						bool flag = false;
						QuestGenUtility.tmpSymbol.Clear();
						for (int j = 0; j < text.Length; j++)
						{
							if (text[j] == '[')
							{
								flag = true;
							}
							else if (text[j] == ']')
							{
								QuestGenUtility.AddSlateVar(ref req, QuestGenUtility.tmpSymbol.ToString(), QuestGenUtility.tmpAddedSlateVars);
								QuestGenUtility.tmpSymbol.Clear();
								flag = false;
							}
							else if (flag)
							{
								QuestGenUtility.tmpSymbol.Append(text[j]);
							}
						}
					}
					if (rule_String.constantConstraints != null)
					{
						for (int k = 0; k < rule_String.constantConstraints.Count; k++)
						{
							string key = rule_String.constantConstraints[k].key;
							QuestGenUtility.AddSlateVar(ref req, key, QuestGenUtility.tmpAddedSlateVars);
						}
					}
				}
			}
		}

		// Token: 0x06014455 RID: 83029 RVA: 0x0060D4F8 File Offset: 0x0060B6F8
		private static void AddSlateVar(ref GrammarRequest req, string absoluteName, HashSet<string> added)
		{
			if (absoluteName == null)
			{
				return;
			}
			QuestGenUtility.tmpVarAbsoluteName.Clear();
			QuestGenUtility.tmpVarAbsoluteName.Append(absoluteName);
			while (QuestGenUtility.tmpVarAbsoluteName.Length > 0)
			{
				string text = QuestGenUtility.tmpVarAbsoluteName.ToString();
				if (added.Contains(text))
				{
					break;
				}
				object obj;
				if (QuestGen.slate.TryGet<object>(text, out obj, true))
				{
					QuestGenUtility.AddSlateVar(ref req, text, obj);
					added.Add(text);
					return;
				}
				if (char.IsNumber(QuestGenUtility.tmpVarAbsoluteName[QuestGenUtility.tmpVarAbsoluteName.Length - 1]))
				{
					while (char.IsNumber(QuestGenUtility.tmpVarAbsoluteName[QuestGenUtility.tmpVarAbsoluteName.Length - 1]))
					{
						StringBuilder stringBuilder = QuestGenUtility.tmpVarAbsoluteName;
						int length = stringBuilder.Length;
						stringBuilder.Length = length - 1;
					}
				}
				else
				{
					int num = text.LastIndexOf('_');
					if (num < 0)
					{
						break;
					}
					int num2 = text.LastIndexOf('/');
					if (num < num2)
					{
						break;
					}
					QuestGenUtility.tmpVarAbsoluteName.Length = num;
				}
			}
		}

		// Token: 0x06014456 RID: 83030 RVA: 0x0060D5E8 File Offset: 0x0060B7E8
		private static void AddSlateVar(ref GrammarRequest req, string absoluteName, object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is BodyPartRecord)
			{
				req.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord(absoluteName, (BodyPartRecord)obj));
			}
			else if (obj is Def)
			{
				req.Rules.AddRange(GrammarUtility.RulesForDef(absoluteName, (Def)obj));
			}
			else if (obj is Faction)
			{
				Faction faction = (Faction)obj;
				req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName, faction, req.Constants, true));
				if (faction.leader != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForPawn(absoluteName + "_leader", faction.leader, req.Constants, true, true));
				}
			}
			else if (obj is Pawn)
			{
				Pawn pawn = (Pawn)obj;
				req.Rules.AddRange(GrammarUtility.RulesForPawn(absoluteName, pawn, req.Constants, true, true));
				if (pawn.Faction != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForFaction(absoluteName + "_faction", pawn.Faction, req.Constants, true));
				}
			}
			else
			{
				Thing thing = obj as Thing;
				if (thing != null)
				{
					req.Rules.AddRange(GrammarUtility.RulesForThing(absoluteName, thing));
				}
				else if (obj is WorldObject)
				{
					req.Rules.AddRange(GrammarUtility.RulesForWorldObject(absoluteName, (WorldObject)obj, true));
				}
				else if (obj is Map)
				{
					req.Rules.AddRange(GrammarUtility.RulesForWorldObject(absoluteName, ((Map)obj).Parent, true));
				}
				else if (obj is IntVec2)
				{
					req.Rules.Add(new Rule_String(absoluteName, ((IntVec2)obj).ToStringCross()));
				}
				else
				{
					if (obj is IEnumerable && !(obj is string))
					{
						if (obj is IEnumerable<Thing>)
						{
							req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel(from x in (IEnumerable<Thing>)obj
							where x != null
							select x, "  - ")));
						}
						else if (obj is IEnumerable<Pawn>)
						{
							req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel((from x in (IEnumerable<Pawn>)obj
							where x != null
							select x).Cast<Thing>(), "  - ")));
						}
						else
						{
							if (obj is IEnumerable<object> && ((IEnumerable<object>)obj).Any<object>())
							{
								if (((IEnumerable<object>)obj).All((object x) => x is Thing))
								{
									req.Rules.Add(new Rule_String(absoluteName, GenLabel.ThingsLabel((from x in (IEnumerable<object>)obj
									where x != null
									select x).Cast<Thing>(), "  - ")));
									goto IL_3A0;
								}
							}
							IEnumerable<WorldObject> enumerable = obj as IEnumerable<WorldObject>;
							if (enumerable != null)
							{
								req.Rules.Add(new Rule_String(absoluteName, enumerable.Select(new Func<WorldObject, string>(QuestGenUtility.<AddSlateVar>g__PossiblyWithTags|13_4)).ToCommaList(true, false)));
							}
							else
							{
								List<string> list = new List<string>();
								foreach (object obj2 in ((IEnumerable)obj))
								{
									if (obj2 != null)
									{
										list.Add(obj2.ToString());
									}
								}
								req.Rules.Add(new Rule_String(absoluteName, list.ToCommaList(true, false)));
							}
						}
						IL_3A0:
						req.Rules.Add(new Rule_String(absoluteName + "_count", ((IEnumerable)obj).EnumerableCount().ToString()));
						int num = 0;
						using (IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj3 = enumerator.Current;
								QuestGenUtility.AddSlateVar(ref req, absoluteName + num.ToString(), obj3);
								num++;
							}
							goto IL_5A0;
						}
					}
					if (obj is Ideo)
					{
						req.Rules.AddRange(GrammarUtility.RulesForIdeo(absoluteName, (Ideo)obj));
					}
					else
					{
						Precept precept = obj as Precept;
						if (precept != null)
						{
							req.Rules.AddRange(GrammarUtility.RulesForPrecept(absoluteName, precept));
						}
						else
						{
							req.Rules.Add(new Rule_String(absoluteName, obj.ToString()));
							if (ConvertHelper.CanConvert<int>(obj))
							{
								req.Rules.Add(new Rule_String(absoluteName + "_duration", ConvertHelper.Convert<int>(obj).ToStringTicksToPeriod(true, false, true, false, false).Colorize(ColoredText.DateTimeColor)));
							}
							if (ConvertHelper.CanConvert<float>(obj))
							{
								req.Rules.Add(new Rule_String(absoluteName + "_money", ConvertHelper.Convert<float>(obj).ToStringMoney(null)));
							}
							if (ConvertHelper.CanConvert<float>(obj))
							{
								req.Rules.Add(new Rule_String(absoluteName + "_percent", ConvertHelper.Convert<float>(obj).ToStringPercent()));
							}
							if (ConvertHelper.CanConvert<FloatRange>(obj))
							{
								QuestGenUtility.AddSlateVar(ref req, absoluteName + "_average", ConvertHelper.Convert<FloatRange>(obj).Average);
							}
							if (ConvertHelper.CanConvert<FloatRange>(obj))
							{
								QuestGenUtility.AddSlateVar(ref req, absoluteName + "_min", ConvertHelper.Convert<FloatRange>(obj).min);
							}
							if (ConvertHelper.CanConvert<FloatRange>(obj))
							{
								QuestGenUtility.AddSlateVar(ref req, absoluteName + "_max", ConvertHelper.Convert<FloatRange>(obj).max);
							}
						}
					}
				}
			}
			IL_5A0:
			if (obj is Def)
			{
				if (!req.Constants.ContainsKey(absoluteName))
				{
					req.Constants.Add(absoluteName, ((Def)obj).defName);
				}
			}
			else if (obj is Faction)
			{
				if (!req.Constants.ContainsKey(absoluteName))
				{
					req.Constants.Add(absoluteName, ((Faction)obj).def.defName);
				}
			}
			else if ((obj.GetType().IsPrimitive || obj is string || obj.GetType().IsEnum) && !req.Constants.ContainsKey(absoluteName))
			{
				req.Constants.Add(absoluteName, obj.ToString());
			}
			if (obj is IEnumerable && !(obj is string))
			{
				string key = absoluteName + "_count";
				if (!req.Constants.ContainsKey(key))
				{
					req.Constants.Add(key, ((IEnumerable)obj).EnumerableCount().ToString());
				}
			}
		}

		// Token: 0x06014457 RID: 83031 RVA: 0x0060DCA8 File Offset: 0x0060BEA8
		public static string ResolveLocalTextWithDescriptionRules(RulePack localRules, string localRootKeyword = "root")
		{
			List<Rule> list = new List<Rule>();
			list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
			if (localRules != null)
			{
				list.AddRange(QuestGenUtility.AppendCurrentPrefix(localRules.Rules));
			}
			string text = localRootKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				text = QuestGen.slate.CurrentPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, text, false);
		}

		// Token: 0x06014458 RID: 83032 RVA: 0x0060DD17 File Offset: 0x0060BF17
		public static string ResolveLocalText(RulePack localRules, string localRootKeyword = "root")
		{
			return QuestGenUtility.ResolveLocalText((localRules != null) ? localRules.Rules : null, null, localRootKeyword, true);
		}

		// Token: 0x06014459 RID: 83033 RVA: 0x0060DD30 File Offset: 0x0060BF30
		public static string ResolveLocalText(List<Rule> localRules, Dictionary<string, string> localConstants = null, string localRootKeyword = "root", bool capitalizeFirstSentence = true)
		{
			string text = localRootKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				text = QuestGen.slate.CurrentPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return QuestGenUtility.ResolveAbsoluteText(QuestGenUtility.AppendCurrentPrefix(localRules), QuestGenUtility.AppendCurrentPrefix(localConstants), text, capitalizeFirstSentence);
		}

		// Token: 0x0601445A RID: 83034 RVA: 0x0060DD80 File Offset: 0x0060BF80
		public static string ResolveAbsoluteText(List<Rule> absoluteRules, Dictionary<string, string> absoluteConstants = null, string absoluteRootKeyword = "root", bool capitalizeFirstSentence = true)
		{
			GrammarRequest request = default(GrammarRequest);
			if (absoluteRules != null)
			{
				request.Rules.AddRange(absoluteRules);
			}
			if (absoluteConstants != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in absoluteConstants)
				{
					request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			QuestGenUtility.AddSlateVars(ref request);
			return GrammarResolver.Resolve(absoluteRootKeyword, request, null, false, null, null, null, capitalizeFirstSentence);
		}

		// Token: 0x0601445B RID: 83035 RVA: 0x0060DE10 File Offset: 0x0060C010
		public static List<Rule> AppendCurrentPrefix(List<Rule> rules)
		{
			if (rules == null)
			{
				return null;
			}
			List<Rule> list = new List<Rule>();
			string currentPrefix = QuestGen.slate.CurrentPrefix;
			for (int i = 0; i < rules.Count; i++)
			{
				Rule rule = rules[i].DeepCopy();
				if (!currentPrefix.NullOrEmpty())
				{
					rule.keyword = currentPrefix + "/" + rule.keyword;
				}
				rule.keyword = QuestGenUtility.NormalizeVarPath(rule.keyword);
				Rule_String rule_String = rule as Rule_String;
				if (rule_String != null && !rule_String.OutputNull)
				{
					rule_String.AppendPrefixToAllKeywords(currentPrefix);
				}
				list.Add(rule);
			}
			return list;
		}

		// Token: 0x0601445C RID: 83036 RVA: 0x0060DEA8 File Offset: 0x0060C0A8
		public static Dictionary<string, string> AppendCurrentPrefix(Dictionary<string, string> constants)
		{
			if (constants == null)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string currentPrefix = QuestGen.slate.CurrentPrefix;
			foreach (KeyValuePair<string, string> keyValuePair in constants)
			{
				string text = keyValuePair.Key;
				if (!currentPrefix.NullOrEmpty())
				{
					text = currentPrefix + "/" + text;
				}
				text = QuestGenUtility.NormalizeVarPath(text);
				dictionary.Add(text, keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x0601445D RID: 83037 RVA: 0x0060DF40 File Offset: 0x0060C140
		public static LookTargets ToLookTargets(SlateRef<IEnumerable<object>> objects, Slate slate)
		{
			if (objects.GetValue(slate) == null || !objects.GetValue(slate).Any<object>())
			{
				return LookTargets.Invalid;
			}
			return QuestGenUtility.ToLookTargets(objects.GetValue(slate));
		}

		// Token: 0x0601445E RID: 83038 RVA: 0x0060DF70 File Offset: 0x0060C170
		public static LookTargets ToLookTargets(IEnumerable<object> objects)
		{
			if (objects == null || !objects.Any<object>())
			{
				return LookTargets.Invalid;
			}
			LookTargets lookTargets = new LookTargets();
			foreach (object obj in objects)
			{
				if (obj is Thing)
				{
					lookTargets.targets.Add((Thing)obj);
				}
				else if (obj is WorldObject)
				{
					lookTargets.targets.Add((WorldObject)obj);
				}
				else if (obj is Map)
				{
					lookTargets.targets.Add(((Map)obj).Parent);
				}
				else if (obj is GlobalTargetInfo)
				{
					lookTargets.targets.Add((GlobalTargetInfo)obj);
				}
			}
			return lookTargets;
		}

		// Token: 0x0601445F RID: 83039 RVA: 0x0060E050 File Offset: 0x0060C250
		public static List<Rule> MergeRules(RulePack rules, string singleRule, string root)
		{
			List<Rule> list = new List<Rule>();
			if (rules != null)
			{
				list.AddRange(rules.Rules);
			}
			if (!singleRule.NullOrEmpty())
			{
				list.Add(new Rule_String(root, singleRule));
			}
			return list;
		}

		// Token: 0x06014460 RID: 83040 RVA: 0x0060E088 File Offset: 0x0060C288
		public static ChoiceLetter MakeLetter(string labelKeyword, string textKeyword, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			ChoiceLetter letter = LetterMaker.MakeLetter("error", "error", def, relatedFaction, quest);
			QuestGen.AddTextRequest(labelKeyword, delegate(string x)
			{
				letter.Label = x;
			}, null);
			QuestGen.AddTextRequest(textKeyword, delegate(string x)
			{
				letter.Text = x;
			}, null);
			return letter;
		}

		// Token: 0x06014461 RID: 83041 RVA: 0x0060E0EC File Offset: 0x0060C2EC
		public static ChoiceLetter MakeLetter(string labelKeyword, string textKeyword, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null)
		{
			ChoiceLetter letter = LetterMaker.MakeLetter("error", "error", def, lookTargets, relatedFaction, quest, null);
			QuestGen.AddTextRequest(labelKeyword, delegate(string x)
			{
				letter.Label = x;
			}, null);
			QuestGen.AddTextRequest(textKeyword, delegate(string x)
			{
				letter.Text = x;
			}, null);
			return letter;
		}

		// Token: 0x06014462 RID: 83042 RVA: 0x0060E154 File Offset: 0x0060C354
		public static void AddToOrMakeList(Slate slate, string name, object obj)
		{
			List<object> list;
			if (!slate.TryGet<List<object>>(name, out list, false))
			{
				list = new List<object>();
			}
			list.Add(obj);
			slate.Set<List<object>>(name, list, false);
		}

		// Token: 0x06014463 RID: 83043 RVA: 0x0060E184 File Offset: 0x0060C384
		public static void AddRangeToOrMakeList(Slate slate, string name, List<object> objs)
		{
			if (objs.NullOrEmpty<object>())
			{
				return;
			}
			List<object> list;
			if (!slate.TryGet<List<object>>(name, out list, false))
			{
				list = new List<object>();
			}
			list.AddRange(objs);
			slate.Set<List<object>>(name, list, false);
		}

		// Token: 0x06014464 RID: 83044 RVA: 0x0060E1BC File Offset: 0x0060C3BC
		public static bool IsInList(Slate slate, string name, object obj)
		{
			List<object> list;
			return slate.TryGet<List<object>>(name, out list, false) && list != null && list.Contains(obj);
		}

		// Token: 0x06014465 RID: 83045 RVA: 0x0060E1E4 File Offset: 0x0060C3E4
		public static List<Slate.VarRestoreInfo> SetVarsForPrefix(List<PrefixCapturedVar> capturedVars, string prefix, Slate slate)
		{
			if (capturedVars.NullOrEmpty<PrefixCapturedVar>())
			{
				return null;
			}
			if (prefix.NullOrEmpty())
			{
				List<Slate.VarRestoreInfo> list = new List<Slate.VarRestoreInfo>();
				for (int i = 0; i < capturedVars.Count; i++)
				{
					list.Add(slate.GetRestoreInfo(capturedVars[i].name));
				}
				for (int j = 0; j < capturedVars.Count; j++)
				{
					object obj;
					if (capturedVars[j].value.TryGetValue(slate, out obj))
					{
						if (capturedVars[j].name == "inSignal" && obj is string)
						{
							obj = QuestGenUtility.HardcodedSignalWithQuestID((string)obj);
						}
						slate.Set<object>(capturedVars[j].name, obj, false);
					}
				}
				return list;
			}
			for (int k = 0; k < capturedVars.Count; k++)
			{
				object obj2;
				if (capturedVars[k].value.TryGetValue(slate, out obj2))
				{
					if (capturedVars[k].name == "inSignal" && obj2 is string)
					{
						obj2 = QuestGenUtility.HardcodedSignalWithQuestID((string)obj2);
					}
					string name = prefix + "/" + capturedVars[k].name;
					slate.Set<object>(name, obj2, false);
				}
			}
			return null;
		}

		// Token: 0x06014466 RID: 83046 RVA: 0x0060E320 File Offset: 0x0060C520
		public static void RestoreVarsForPrefix(List<Slate.VarRestoreInfo> varsRestoreInfo, Slate slate)
		{
			if (varsRestoreInfo.NullOrEmpty<Slate.VarRestoreInfo>())
			{
				return;
			}
			for (int i = 0; i < varsRestoreInfo.Count; i++)
			{
				slate.Restore(varsRestoreInfo[i]);
			}
		}

		// Token: 0x06014467 RID: 83047 RVA: 0x0060E354 File Offset: 0x0060C554
		public static void GetReturnedVars(List<SlateRef<string>> varNames, string prefix, Slate slate)
		{
			if (varNames.NullOrEmpty<SlateRef<string>>())
			{
				return;
			}
			if (prefix.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < varNames.Count; i++)
			{
				string name = prefix + "/" + varNames[i].GetValue(slate);
				object var;
				if (slate.TryGet<object>(name, out var, false))
				{
					slate.Set<object>(varNames[i].GetValue(slate), var, false);
				}
			}
		}

		// Token: 0x06014468 RID: 83048 RVA: 0x0060E3C4 File Offset: 0x0060C5C4
		public static string NormalizeVarPath(string path)
		{
			if (path.NullOrEmpty())
			{
				return path;
			}
			if (!path.Contains(".."))
			{
				return path;
			}
			QuestGenUtility.tmpSb.Length = 0;
			QuestGenUtility.tmpPathParts.Clear();
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i] == '/')
				{
					QuestGenUtility.tmpPathParts.Add(QuestGenUtility.tmpSb.ToString());
					QuestGenUtility.tmpSb.Length = 0;
				}
				else
				{
					QuestGenUtility.tmpSb.Append(path[i]);
				}
			}
			if (QuestGenUtility.tmpSb.Length != 0)
			{
				QuestGenUtility.tmpPathParts.Add(QuestGenUtility.tmpSb.ToString());
			}
			for (int j = 0; j < QuestGenUtility.tmpPathParts.Count; j++)
			{
				while (j < QuestGenUtility.tmpPathParts.Count && QuestGenUtility.tmpPathParts[j] == "..")
				{
					if (j == 0)
					{
						QuestGenUtility.tmpPathParts.RemoveAt(0);
					}
					else
					{
						QuestGenUtility.tmpPathParts.RemoveAt(j);
						QuestGenUtility.tmpPathParts.RemoveAt(j - 1);
						j--;
					}
				}
			}
			QuestGenUtility.tmpSb.Length = 0;
			for (int k = 0; k < QuestGenUtility.tmpPathParts.Count; k++)
			{
				if (k != 0)
				{
					QuestGenUtility.tmpSb.Append('/');
				}
				QuestGenUtility.tmpSb.Append(QuestGenUtility.tmpPathParts[k]);
			}
			return QuestGenUtility.tmpSb.ToString();
		}

		// Token: 0x06014469 RID: 83049 RVA: 0x0060E523 File Offset: 0x0060C723
		public static void RunAdjustPointsForDistantFight()
		{
			QuestScriptDefOf.Util_AdjustPointsForDistantFight.root.Run();
		}

		// Token: 0x0601446A RID: 83050 RVA: 0x0060E534 File Offset: 0x0060C734
		public static void TestRunAdjustPointsForDistantFight(Slate slate)
		{
			QuestScriptDefOf.Util_AdjustPointsForDistantFight.root.TestRun(slate);
		}

		// Token: 0x0601446C RID: 83052 RVA: 0x0060E57C File Offset: 0x0060C77C
		[CompilerGenerated]
		internal static string <AddSlateVar>g__PossiblyWithTags|13_4(WorldObject w)
		{
			string text = Find.ActiveLanguageWorker.WithDefiniteArticle(w.Label, false, w.HasName);
			if (w.Faction == null || !w.HasName)
			{
				return text;
			}
			return text.ApplyTag(TagType.Settlement, w.Faction.GetUniqueLoadID()).Resolve();
		}

		// Token: 0x0400DFEA RID: 57322
		public const string OuterNodeCompletedSignal = "OuterNodeCompleted";

		// Token: 0x0400DFEB RID: 57323
		private static HashSet<string> tmpAddedSlateVars = new HashSet<string>();

		// Token: 0x0400DFEC RID: 57324
		private static StringBuilder tmpSymbol = new StringBuilder();

		// Token: 0x0400DFED RID: 57325
		private static StringBuilder tmpVarAbsoluteName = new StringBuilder();

		// Token: 0x0400DFEE RID: 57326
		private static List<string> tmpPathParts = new List<string>();

		// Token: 0x0400DFEF RID: 57327
		private static StringBuilder tmpSb = new StringBuilder();
	}
}
