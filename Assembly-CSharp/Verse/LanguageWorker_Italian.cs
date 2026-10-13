using System;

namespace Verse
{
	// Token: 0x02000389 RID: 905
	public class LanguageWorker_Italian : LanguageWorker
	{
		// Token: 0x06001739 RID: 5945 RVA: 0x0007493C File Offset: 0x00072B3C
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			char c = str[0];
			char c2 = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "un'" + str;
				}
				return "una " + str;
			}
			else
			{
				char c3 = char.ToLower(c);
				char c4 = char.ToLower(c2);
				if ((c == 's' || c == 'S') && !this.IsVowel(c2))
				{
					return "uno " + str;
				}
				if ((c3 == 'p' && c4 == 's') || (c3 == 'p' && c4 == 'n') || c3 == 'z' || c3 == 'x' || c3 == 'y' || (c3 == 'g' && c4 == 'n'))
				{
					return "uno " + str;
				}
				return "un " + str;
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00074A04 File Offset: 0x00072C04
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char c = str[0];
			char ch = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "la " + str;
			}
			else
			{
				if (c == 'z' || c == 'Z')
				{
					return "lo " + str;
				}
				if ((c == 's' || c == 'S') && !this.IsVowel(ch))
				{
					return "lo " + str;
				}
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "il " + str;
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00074AB7 File Offset: 0x00072CB7
		public bool IsVowel(char ch)
		{
			return "aeiouAEIOU".IndexOf(ch) >= 0;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00074ACA File Offset: 0x00072CCA
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + "°";
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00074AE0 File Offset: 0x00072CE0
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
			char ch = str[str.Length - 1];
			if (!this.IsVowel(ch))
			{
				return str;
			}
			if (gender == Gender.Female)
			{
				return str.Substring(0, str.Length - 1) + "e";
			}
			return str.Substring(0, str.Length - 1) + "i";
		}
	}
}
