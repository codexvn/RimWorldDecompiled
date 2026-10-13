using System;

namespace Verse
{
	// Token: 0x02000384 RID: 900
	public class LanguageWorker_English : LanguageWorker
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x0007368F File Offset: 0x0007188F
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "a " + str;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x000736B5 File Offset: 0x000718B5
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return "";
			}
			if (name)
			{
				return str;
			}
			return "the " + str;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x000736D8 File Offset: 0x000718D8
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			if (str.StartsWith("a ", StringComparison.OrdinalIgnoreCase) && str.Length >= 3)
			{
				bool flag = str.Substring(2) == "hour";
				if (flag || str[2] == 'a' || str[2] == 'e' || str[2] == 'i' || str[2] == 'o' || str[2] == 'u')
				{
					str = str.Insert(1, "n");
				}
				if (flag || str[2] == 'A' || str[2] == 'E' || str[2] == 'I' || str[2] == 'O' || str[2] == 'U')
				{
					str = str.Insert(1, "n");
				}
			}
			if (str.StartsWith("an unique ", StringComparison.OrdinalIgnoreCase))
			{
				str = str.Remove(1, 1);
			}
			str = str.Replace(" a a", " an a");
			str = str.Replace(" a e", " an e");
			str = str.Replace(" a i", " an i");
			str = str.Replace(" a o", " an o");
			str = str.Replace(" a u", " an u");
			str = str.Replace(" a hour", " an hour");
			str = str.Replace(" an unique", " a unique");
			str = str.Replace(" A a", " An a");
			str = str.Replace(" A e", " An e");
			str = str.Replace(" A i", " An i");
			str = str.Replace(" A o", " An o");
			str = str.Replace(" A u", " An u");
			str = str.Replace(" A hour", " An hour");
			str = str.Replace(" An unique", " A unique");
			str = str.Replace("\na a", "\nan a");
			str = str.Replace("\na e", "\nan e");
			str = str.Replace("\na i", "\nan i");
			str = str.Replace("\na o", "\nan o");
			str = str.Replace("\na u", "\nan u");
			str = str.Replace("\na hour", "\nan hour");
			str = str.Replace("\nan unique", "\na unique");
			str = str.Replace("\nA a", "\nAn a");
			str = str.Replace("\nA e", "\nAn e");
			str = str.Replace("\nA i", "\nAn i");
			str = str.Replace("\nA o", "\nAn o");
			str = str.Replace("\nA u", "\nAn u");
			str = str.Replace("\nA hour", "\nAn hour");
			str = str.Replace("\nAn unique", "\nA unique");
			str = str.Replace(" a A", " an A");
			str = str.Replace(" a E", " an E");
			str = str.Replace(" a I", " an I");
			str = str.Replace(" a O", " an O");
			str = str.Replace(" a U", " an U");
			str = str.Replace(" A A", " An A");
			str = str.Replace(" A E", " An E");
			str = str.Replace(" A I", " An I");
			str = str.Replace(" A O", " An O");
			str = str.Replace(" A U", " An U");
			str = str.Replace("\na A", "\nan A");
			str = str.Replace("\na E", "\nan E");
			str = str.Replace("\na I", "\nan I");
			str = str.Replace("\na O", "\nan O");
			str = str.Replace("\na U", "\nan U");
			str = str.Replace("\nA A", "\nAn A");
			str = str.Replace("\nA E", "\nAn E");
			str = str.Replace("\nA I", "\nAn I");
			str = str.Replace("\nA O", "\nAn O");
			str = str.Replace("\nA U", "\nAn U");
			return str;
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00073B24 File Offset: 0x00071D24
		public override string ToTitleCase(string str)
		{
			return GenText.ToTitleCaseSmart(str);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00073B2C File Offset: 0x00071D2C
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			int num = number % 10;
			if (number / 10 % 10 != 1)
			{
				if (num == 1)
				{
					return number.ToString() + "st";
				}
				if (num == 2)
				{
					return number.ToString() + "nd";
				}
				if (num == 3)
				{
					return number.ToString() + "rd";
				}
			}
			return number.ToString() + "th";
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00073B9C File Offset: 0x00071D9C
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
			if (str[str.Length - 1] == 's' || (count != -1 && count < 2))
			{
				return str;
			}
			int num = (int)str[str.Length - 1];
			char c = (str.Length == 1) ? '\0' : str[str.Length - 2];
			bool flag = char.IsLetter(c) && "oaieuyOAIEUY".IndexOf(c) >= 0;
			bool flag2 = char.IsLetter(c) && !flag;
			if (num == 121 && flag2)
			{
				return str.Substring(0, str.Length - 1) + "ies";
			}
			return str + "s";
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00073C64 File Offset: 0x00071E64
		public override string PostProcessThingLabelForRelic(string thingLabel)
		{
			int num = thingLabel.LastIndexOf(' ');
			if (num != -1)
			{
				return thingLabel.Substring(num + 1);
			}
			return thingLabel;
		}
	}
}
