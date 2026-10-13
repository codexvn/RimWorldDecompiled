using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200037A RID: 890
	public static class LanguageReportGenerator
	{
		// Token: 0x060016C0 RID: 5824 RVA: 0x000710D8 File Offset: 0x0006F2D8
		public static void SaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage && !defaultLanguage.anyError)
			{
				Messages.Message("Please activate a non-English language to scan.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			activeLanguage.LoadData();
			defaultLanguage.LoadData();
			LongEventHandler.QueueLongEvent(new Action(LanguageReportGenerator.DoSaveTranslationReport), "GeneratingTranslationReport", true, null, true, false, null);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00071138 File Offset: 0x0006F338
		private static void DoSaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			string str = "Translation report for ";
			LoadedLanguage loadedLanguage = activeLanguage;
			stringBuilder2.AppendLine(str + ((loadedLanguage != null) ? loadedLanguage.ToString() : null));
			if (activeLanguage.defInjections.Any((DefInjectionPackage x) => x.usedOldRepSyntax))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Consider using <Something.Field.Example.Etc>translation</Something.Field.Example.Etc> def-injection syntax instead of <rep>.");
			}
			try
			{
				LanguageReportGenerator.AppendGeneralLoadErrors(stringBuilder);
			}
			catch (Exception ex)
			{
				string str2 = "Error while generating translation report (general load errors): ";
				Exception ex2 = ex;
				Log.Error(str2 + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsLoadErros(stringBuilder);
			}
			catch (Exception ex3)
			{
				string str3 = "Error while generating translation report (def-injections load errors): ";
				Exception ex4 = ex3;
				Log.Error(str3 + ((ex4 != null) ? ex4.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendMissingKeyedTranslations(stringBuilder);
			}
			catch (Exception ex5)
			{
				string str4 = "Error while generating translation report (missing keyed translations): ";
				Exception ex6 = ex5;
				Log.Error(str4 + ((ex6 != null) ? ex6.ToString() : null));
			}
			List<string> list = new List<string>();
			try
			{
				LanguageReportGenerator.AppendMissingDefInjections(stringBuilder, list);
			}
			catch (Exception ex7)
			{
				string str5 = "Error while generating translation report (missing def-injections): ";
				Exception ex8 = ex7;
				Log.Error(str5 + ((ex8 != null) ? ex8.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendMissingBackstories(stringBuilder);
			}
			catch (Exception ex9)
			{
				string str6 = "Error while generating translation report (missing backstories): ";
				Exception ex10 = ex9;
				Log.Error(str6 + ((ex10 != null) ? ex10.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryDefInjections(stringBuilder, list);
			}
			catch (Exception ex11)
			{
				string str7 = "Error while generating translation report (unnecessary def-injections): ";
				Exception ex12 = ex11;
				Log.Error(str7 + ((ex12 != null) ? ex12.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendRenamedDefInjections(stringBuilder);
			}
			catch (Exception ex13)
			{
				string str8 = "Error while generating translation report (renamed def-injections): ";
				Exception ex14 = ex13;
				Log.Error(str8 + ((ex14 != null) ? ex14.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendArgumentCountMismatches(stringBuilder);
			}
			catch (Exception ex15)
			{
				string str9 = "Error while generating translation report (argument count mismatches): ";
				Exception ex16 = ex15;
				Log.Error(str9 + ((ex16 != null) ? ex16.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryKeyedTranslations(stringBuilder);
			}
			catch (Exception ex17)
			{
				string str10 = "Error while generating translation report (unnecessary keyed translations): ";
				Exception ex18 = ex17;
				Log.Error(str10 + ((ex18 != null) ? ex18.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendKeyedTranslationsMatchingEnglish(stringBuilder);
			}
			catch (Exception ex19)
			{
				string str11 = "Error while generating translation report (keyed translations matching English): ";
				Exception ex20 = ex19;
				Log.Error(str11 + ((ex20 != null) ? ex20.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesMatchingEnglish(stringBuilder);
			}
			catch (Exception ex21)
			{
				string str12 = "Error while generating translation report (backstories matching English): ";
				Exception ex22 = ex21;
				Log.Error(str12 + ((ex22 != null) ? ex22.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsSyntaxSuggestions(stringBuilder);
			}
			catch (Exception ex23)
			{
				string str13 = "Error while generating translation report (def-injections syntax suggestions): ";
				Exception ex24 = ex23;
				Log.Error(str13 + ((ex24 != null) ? ex24.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendTKeySystemErrors(stringBuilder);
			}
			catch (Exception ex25)
			{
				string str14 = "Error while generating translation report (TKeySystem errors): ";
				Exception ex26 = ex25;
				Log.Error(str14 + ((ex26 != null) ? ex26.ToString() : null));
			}
			try
			{
				LanguageReportGenerator.AppendObsoleteBackstoryTranslations(stringBuilder);
			}
			catch (Exception ex27)
			{
				string str15 = "Error while generating translation report (backstory syntax suggestions): ";
				Exception ex28 = ex27;
				Log.Error(str15 + ((ex28 != null) ? ex28.ToString() : null));
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (text.NullOrEmpty())
			{
				text = GenFilePaths.SaveDataFolderPath;
			}
			text = Path.Combine(text, "TranslationReport.txt");
			File.WriteAllText(text, stringBuilder.ToString());
			Messages.Message("MessageTranslationReportSaved".Translate(Path.GetFullPath(text)), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00071500 File Offset: 0x0006F700
		private static void AppendGeneralLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.loadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== General load errors (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00071590 File Offset: 0x0006F790
		private static void AppendDefInjectionsLoadErros(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadErrors)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations load errors (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x0007165C File Offset: 0x0006F85C
		private static void AppendMissingKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in defaultLanguage.keyedReplacements)
			{
				if (!activeLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					string text = string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (English file: ",
						defaultLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					});
					if (activeLanguage.HaveTextForKey(keyValuePair.Key, true))
					{
						text = text + " (placeholder exists in " + activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key) + ")";
					}
					num++;
					stringBuilder.AppendLine(text);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Missing keyed translations (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000717A4 File Offset: 0x0006F9A4
		private static void AppendMissingDefInjections(StringBuilder sb, List<string> outUnnecessaryDefInjections)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string str in defInjectionPackage.MissingInjections(outUnnecessaryDefInjections))
				{
					num++;
					stringBuilder.AppendLine(defInjectionPackage.defType.Name + ": " + str);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations missing (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x0007189C File Offset: 0x0006FA9C
		private static void AppendMissingBackstories(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			if (!BackstoryTranslationUtility.AnyLegacyBackstoryFiles(activeLanguage.AllDirectories))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations missing (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x0007194C File Offset: 0x0006FB4C
		private static void AppendUnnecessaryDefInjections(StringBuilder sb, List<string> unnecessaryDefInjections)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in unnecessaryDefInjections)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary def-injected translations (marked as NoTranslate) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000719D4 File Offset: 0x0006FBD4
		private static void AppendRenamedDefInjections(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in defInjectionPackage.injections)
				{
					if (!(keyValuePair.Value.path == keyValuePair.Value.nonBackCompatiblePath))
					{
						string text = keyValuePair.Value.nonBackCompatiblePath.Split('.', StringSplitOptions.None)[0];
						string text2 = keyValuePair.Value.path.Split('.', StringSplitOptions.None)[0];
						if (text != text2)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"Def has been renamed: ",
								text,
								" -> ",
								text2,
								", translation ",
								keyValuePair.Value.nonBackCompatiblePath,
								" should be renamed as well."
							}));
						}
						else
						{
							stringBuilder.AppendLine("Translation " + keyValuePair.Value.nonBackCompatiblePath + " should be renamed to " + keyValuePair.Value.path);
						}
						num++;
					}
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations using old, renamed defs (fixed automatically but can break in the next RimWorld version) (" + num.ToString() + ") =========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00071B94 File Offset: 0x0006FD94
		private static void AppendArgumentCountMismatches(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in defaultLanguage.keyedReplacements.Keys.Intersect(activeLanguage.keyedReplacements.Keys))
			{
				if (!activeLanguage.keyedReplacements[text].isPlaceholder && !LanguageReportGenerator.SameSimpleGrammarResolverSymbols(defaultLanguage.keyedReplacements[text].value, activeLanguage.keyedReplacements[text].value))
				{
					num++;
					stringBuilder.AppendLine(string.Format("{0} ({1})\n  - '{2}'\n  - '{3}'", new object[]
					{
						text,
						activeLanguage.GetKeySourceFileAndLine(text),
						defaultLanguage.keyedReplacements[text].value.Replace("\n", "\\n"),
						activeLanguage.keyedReplacements[text].value.Replace("\n", "\\n")
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Argument count mismatches (may or may not be incorrect) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00071CF8 File Offset: 0x0006FEF8
		private static void AppendUnnecessaryKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				if (!defaultLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary keyed translations (will never be used) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00071E04 File Offset: 0x00070004
		private static void AppendKeyedTranslationsMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				TaggedString taggedString;
				if (!keyValuePair.Value.isPlaceholder && defaultLanguage.TryGetTextFromKey(keyValuePair.Key, out taggedString) && keyValuePair.Value.value == taggedString)
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Keyed translations matching English (maybe ok) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00071F44 File Offset: 0x00070144
		private static void AppendBackstoriesMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			if (!BackstoryTranslationUtility.AnyLegacyBackstoryFiles(activeLanguage.AllDirectories))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.BackstoryTranslationsMatchingEnglish(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations matching English (maybe ok) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00071FF4 File Offset: 0x000701F4
		private static void AppendObsoleteBackstoryTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.ObsoleteBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstories translation using obsolete format (def injection is now enabled for backstories) (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00072094 File Offset: 0x00070294
		private static void AppendDefInjectionsSyntaxSuggestions(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadSyntaxSuggestions)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			if (num == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations syntax suggestions (" + num.ToString() + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00072164 File Offset: 0x00070364
		private static void AppendTKeySystemErrors(StringBuilder sb)
		{
			if (TKeySystem.loadErrors.Count == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== TKey system errors (" + TKeySystem.loadErrors.Count.ToString() + ") ==========");
			sb.Append(string.Join("\r\n", TKeySystem.loadErrors));
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x000721C4 File Offset: 0x000703C4
		public static bool SameSimpleGrammarResolverSymbols(string str1, string str2)
		{
			LanguageReportGenerator.tmpStr1Symbols.Clear();
			LanguageReportGenerator.tmpStr2Symbols.Clear();
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str1, LanguageReportGenerator.tmpStr1Symbols);
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str2, LanguageReportGenerator.tmpStr2Symbols);
			for (int i = 0; i < LanguageReportGenerator.tmpStr1Symbols.Count; i++)
			{
				if (!LanguageReportGenerator.tmpStr2Symbols.Contains(LanguageReportGenerator.tmpStr1Symbols[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x0007222C File Offset: 0x0007042C
		private static void CalculateSimpleGrammarResolverSymbols(string str, List<string> outSymbols)
		{
			outSymbols.Clear();
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '{')
				{
					LanguageReportGenerator.tmpSymbol.Length = 0;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					for (i++; i < str.Length; i++)
					{
						char c = str[i];
						if (c == '}')
						{
							flag = true;
							break;
						}
						if (c == '_')
						{
							flag2 = true;
						}
						else if (c == '?')
						{
							flag3 = true;
						}
						else if (!flag2 && !flag3)
						{
							LanguageReportGenerator.tmpSymbol.Append(c);
						}
					}
					if (flag)
					{
						outSymbols.Add(LanguageReportGenerator.tmpSymbol.ToString().Trim());
					}
				}
			}
		}

		// Token: 0x040014C1 RID: 5313
		private const string FileName = "TranslationReport.txt";

		// Token: 0x040014C2 RID: 5314
		private static List<string> tmpStr1Symbols = new List<string>();

		// Token: 0x040014C3 RID: 5315
		private static List<string> tmpStr2Symbols = new List<string>();

		// Token: 0x040014C4 RID: 5316
		private static StringBuilder tmpSymbol = new StringBuilder();
	}
}
