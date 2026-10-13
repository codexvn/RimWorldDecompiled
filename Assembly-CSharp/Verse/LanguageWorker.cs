using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public abstract class LanguageWorker
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x00002501 File Offset: 0x00000701
		public virtual int TotalNumCaseCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x000726C4 File Offset: 0x000708C4
		public virtual string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("IndefiniteForm".CanTranslate())
			{
				return "IndefiniteForm".Translate(str);
			}
			return "IndefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00072726 File Offset: 0x00070926
		public string WithIndefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithIndefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x0007273E File Offset: 0x0007093E
		public string WithIndefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00072751 File Offset: 0x00070951
		public string WithIndefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str, plural, name));
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00072764 File Offset: 0x00070964
		public virtual string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if ("DefiniteForm".CanTranslate())
			{
				return "DefiniteForm".Translate(str);
			}
			return "DefiniteArticle".Translate() + " " + str;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000727C6 File Offset: 0x000709C6
		public string WithDefiniteArticle(string str, bool plural = false, bool name = false)
		{
			return this.WithDefiniteArticle(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), plural, name);
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x000727DE File Offset: 0x000709DE
		public string WithDefiniteArticlePostProcessed(string str, Gender gender, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, gender, plural, name));
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x000727F1 File Offset: 0x000709F1
		public string WithDefiniteArticlePostProcessed(string str, bool plural = false, bool name = false)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str, plural, name));
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00072802 File Offset: 0x00070A02
		public virtual string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0007280B File Offset: 0x00070A0B
		public virtual string PostProcessed(string str)
		{
			return str.MergeMultipleSpaces(true);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00072814 File Offset: 0x00070A14
		public virtual string ToTitleCase(string str)
		{
			return str.CapitalizeFirst();
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x0007281C File Offset: 0x00070A1C
		public virtual string Pluralize(string str, Gender gender, int count = -1)
		{
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			return str;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00072839 File Offset: 0x00070A39
		public string Pluralize(string str, int count = -1)
		{
			return this.Pluralize(str, LanguageDatabase.activeLanguage.ResolveGender(str, null, Gender.Male), count);
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00072850 File Offset: 0x00070A50
		public virtual bool TryLookupPluralForm(string str, Gender gender, out string plural, int count = -1)
		{
			plural = null;
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return false;
			}
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable("plural");
			if (lookupTable == null)
			{
				return false;
			}
			string key = str.ToLower();
			if (!lookupTable.ContainsKey(key))
			{
				return false;
			}
			string[] array = lookupTable[key];
			if (array.Length < 2)
			{
				return false;
			}
			plural = array[1];
			if (str.Length != 0 && char.IsUpper(str[0]))
			{
				plural = plural.CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000728D8 File Offset: 0x00070AD8
		public virtual bool TryLookUp(string tableName, string keyName, int index, out string result, string fullStringForReference = null)
		{
			result = null;
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				return false;
			}
			if (keyName.NullOrEmpty())
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning("Tried to lookup an empty key in table '" + tableName + "'.");
				}
				result = keyName;
				return true;
			}
			string text = keyName.ToLower();
			if (!lookupTable.ContainsKey(text))
			{
				LanguageWorker.ParenthesisRegex.Replace(text, "");
				text = text.Trim();
				if (!lookupTable.ContainsKey(text))
				{
					if (DebugSettings.logTranslationLookupErrors)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Tried a lookup for key '",
							keyName,
							"' in table '",
							tableName,
							"', which doesn't exist."
						}));
					}
					result = keyName;
					return true;
				}
			}
			string[] array = lookupTable[text];
			if (array.Length < index + 1)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning(string.Format("Tried a lookup an out-of-bounds index '{0}' for key '{1}' in table '{2}'.", index, keyName, tableName));
				}
				result = keyName;
				return true;
			}
			result = array[index];
			return true;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x000729D2 File Offset: 0x00070BD2
		public virtual string PostProcessThingLabelForRelic(string thingLabel)
		{
			if (thingLabel.IndexOf(' ') != -1)
			{
				return null;
			}
			return thingLabel;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000729E4 File Offset: 0x00070BE4
		public virtual string ResolveNumCase(float number, List<string> args)
		{
			string formOne = args[0].Trim('\'');
			string text = args[1].Trim('\'');
			string formMany = args[2].Trim('\'');
			if (number - Mathf.Floor(number) > 1E-45f)
			{
				return number.ToString() + " " + text;
			}
			return number.ToString() + " " + this.GetFormForNumber((int)number, formOne, text, formMany);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00072A5C File Offset: 0x00070C5C
		protected virtual string GetFormForNumber(int num, string formOne, string formSeveral, string formMany)
		{
			int num2 = num % 10;
			if (num / 10 % 10 == 1)
			{
				return formMany;
			}
			if (num2 == 1)
			{
				return formOne;
			}
			if (num2 - 2 > 2)
			{
				return formMany;
			}
			return formSeveral;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00072A90 File Offset: 0x00070C90
		public virtual string ResolveReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker.replaceArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
			}
			return text;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00072B2D File Offset: 0x00070D2D
		public virtual string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			if (functionName == "lookup")
			{
				return this.ResolveLookup(args, fullStringForReference);
			}
			if (functionName == "replace")
			{
				return this.ResolveReplace(args);
			}
			return "";
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00072B60 File Offset: 0x00070D60
		protected string ResolveLookup(List<string> args, string fullStringForReference)
		{
			if (args.Count != 2 && args.Count != 3)
			{
				Log.Error("Invalid argument number for 'lookup' function, expected 2 or 3. A key to lookup, table name and optional index if there's more than 1 entry per key. Full string: " + fullStringForReference);
				return "";
			}
			string text = args[1];
			int index = 1;
			if (args.Count == 3 && !int.TryParse(args[2], out index))
			{
				Log.Error("Invalid lookup index value: '" + args[2] + "' Full string: " + fullStringForReference);
				return "";
			}
			string result;
			if (this.TryLookUp(text.ToLower(), args[0], index, out result, fullStringForReference))
			{
				return result;
			}
			return "";
		}

		// Token: 0x040014CF RID: 5327
		private static Regex ParenthesisRegex = new Regex("\\((.*?)\\)");

		// Token: 0x040014D0 RID: 5328
		private static readonly Regex replaceArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);
	}
}
