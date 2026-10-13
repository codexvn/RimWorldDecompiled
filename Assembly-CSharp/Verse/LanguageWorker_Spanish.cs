using System;

namespace Verse
{
	// Token: 0x0200038F RID: 911
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x0600175B RID: 5979 RVA: 0x00075689 File Offset: 0x00073889
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unas " : "unos ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000756C1 File Offset: 0x000738C1
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "las " : "los ") + str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000756F9 File Offset: 0x000738F9
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + ".º";
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0007570C File Offset: 0x0007390C
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			if (count != -1 && count < 2)
			{
				return str;
			}
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			if (this.IsVowel(c))
			{
				if (str == "sí")
				{
					return "síes";
				}
				if (c == 'í' || c == 'ú' || c == 'Í' || c == 'Ú')
				{
					return str + "es";
				}
				return str + "s";
			}
			else
			{
				if ((c == 'y' || c == 'Y') && this.IsVowel(c2))
				{
					return str + "es";
				}
				if ("lrndjsxLRNDJSX".IndexOf(c) >= 0 || (c == 'h' && c2 == 'c'))
				{
					return str + "es";
				}
				if (c == 'z' || c == 'Z')
				{
					return str.Substring(0, str.Length - 1) + ((c == 'z') ? 'c' : 'C').ToString() + "es";
				}
				return str + "s";
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0007583F File Offset: 0x00073A3F
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}
