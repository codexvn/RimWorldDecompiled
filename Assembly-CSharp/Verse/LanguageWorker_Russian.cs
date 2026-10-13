using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200038E RID: 910
	public class LanguageWorker_Russian : LanguageWorker
	{
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00075162 File Offset: 0x00073362
		public override int TotalNumCaseCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x00073B24 File Offset: 0x00071D24
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00075168 File Offset: 0x00073368
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty() || (count != -1 && count < 2))
			{
				return str;
			}
			string text;
			if (!this.TryLookupPluralForm(str, gender, out text, count))
			{
				text = this.PluralizeFallback(str, gender, count);
			}
			if (count == -1)
			{
				return text;
			}
			string formSeveral;
			string formMany;
			if (this.TryLookUp("Case", str, 1, out formSeveral, null) && this.TryLookUp("Case", text, 1, out formMany, null))
			{
				return this.GetFormForNumber(count, str, formSeveral, formMany);
			}
			return text;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x000751D4 File Offset: 0x000733D4
		public override bool TryLookUp(string tableName, string key, int index, out string result, string fullStringForReference = null)
		{
			Dictionary<string, string[]> lookupTable = LanguageDatabase.activeLanguage.WordInfo.GetLookupTable(tableName);
			if (lookupTable == null)
			{
				result = null;
				return false;
			}
			key = key.ToLower().Trim();
			if (key.NullOrEmpty())
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Tried to lookup a bad key '",
						key,
						"' in table '",
						tableName,
						"'."
					}));
				}
				result = key;
				return true;
			}
			bool result2;
			try
			{
				int i = key.Length;
				while (i > 0)
				{
					string key2 = key.Substring(0, i);
					string str = key.Substring(i);
					string[] array = lookupTable.TryGetValue(key2, null);
					if (array != null)
					{
						if (index < 0 || array.Length <= index)
						{
							if (DebugSettings.logTranslationLookupErrors)
							{
								Log.Warning(string.Format("Tried a lookup an out-of-bounds index '{0}' for key '{1}' in table '{2}'.", index, key, tableName));
							}
							result = key;
							return true;
						}
						result = array[index] + str;
						return true;
					}
					else
					{
						i = key.LastIndexOf(' ', i - 1);
					}
				}
				if (DebugSettings.logTranslationLookupErrors)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Tried a lookup for key '",
						key,
						"' in table '",
						tableName,
						"', which doesn't exist."
					}));
				}
				result = key;
				result2 = true;
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("Exception while looking up in tableName: {0}, key: {1}, index: {2}, fullStringForReference: {3}: {4}", new object[]
				{
					tableName,
					key,
					index,
					fullStringForReference,
					ex.Message
				}));
				result = key;
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00075364 File Offset: 0x00073564
		private string PluralizeFallback(string str, Gender gender, int count = -1)
		{
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			switch (gender)
			{
			case Gender.None:
			{
				if (c <= 'O')
				{
					if (c != 'E')
					{
						if (c != 'O')
						{
							break;
						}
						return str.Substring(0, str.Length - 1) + "A";
					}
				}
				else if (c != 'e')
				{
					if (c == 'o')
					{
						return str.Substring(0, str.Length - 1) + "a";
					}
					break;
				}
				char value = char.ToUpper(c2);
				if ("ГКХЖЧШЩЦ".IndexOf(value) >= 0)
				{
					if (c == 'E')
					{
						return str.Substring(0, str.Length - 1) + "A";
					}
					if (c == 'e')
					{
						return str.Substring(0, str.Length - 1) + "a";
					}
				}
				else
				{
					if (c == 'E')
					{
						return str.Substring(0, str.Length - 1) + "Я";
					}
					if (c == 'e')
					{
						return str.Substring(0, str.Length - 1) + "я";
					}
				}
				break;
			}
			case Gender.Male:
				if (LanguageWorker_Russian.IsConsonant(c))
				{
					return str + "ы";
				}
				if (c <= 'Ь')
				{
					if (c == 'Й')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'Ь')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
				}
				else
				{
					if (c == 'й')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
				}
				break;
			case Gender.Female:
				if (c <= 'Ь')
				{
					if (c != 'A' && c != 'a')
					{
						if (c == 'Ь')
						{
							return str.Substring(0, str.Length - 1) + "И";
						}
					}
					else
					{
						char value2 = char.ToUpper(c2);
						if ("ГКХЖЧШЩ".IndexOf(value2) >= 0)
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "и";
							}
							return str.Substring(0, str.Length - 1) + "И";
						}
						else
						{
							if (c == 'a')
							{
								return str.Substring(0, str.Length - 1) + "ы";
							}
							return str.Substring(0, str.Length - 1) + "Ы";
						}
					}
				}
				else
				{
					if (c == 'Я')
					{
						return str.Substring(0, str.Length - 1) + "И";
					}
					if (c == 'ь')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					if (c == 'я')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
				}
				break;
			}
			return str;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00075676 File Offset: 0x00073876
		private static bool IsConsonant(char ch)
		{
			return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
		}
	}
}
