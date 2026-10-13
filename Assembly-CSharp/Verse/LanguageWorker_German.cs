using System;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000386 RID: 902
	public class LanguageWorker_German : LanguageWorker
	{
		// Token: 0x0600172A RID: 5930 RVA: 0x000745D8 File Offset: 0x000727D8
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "ein " + str;
			case Gender.Male:
				return "ein " + str;
			case Gender.Female:
				return "eine " + str;
			default:
				return str;
			}
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00074624 File Offset: 0x00072824
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			switch (gender)
			{
			case Gender.None:
				return "das " + str;
			case Gender.Male:
				return "der " + str;
			case Gender.Female:
				return "die " + str;
			default:
				return str;
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00074670 File Offset: 0x00072870
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			return Regex.Replace(str, "(.)(.)(\\(/Name\\)|</color>)?'s(?=\\s|$)", delegate(Match m)
			{
				string value = m.Groups[1].Value;
				string value2 = m.Groups[2].Value;
				string value3 = m.Groups[3].Value;
				if (value2 == "s" || value2 == "ß" || value2 == "z" || value2 == "x" || (value == "c" && value2 == "e"))
				{
					return value + value2 + value3 + "'";
				}
				return value + value2 + value3 + "s";
			}, RegexOptions.Compiled);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x000746A6 File Offset: 0x000728A6
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + ".";
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000746B9 File Offset: 0x000728B9
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			return LanguageWorker_German.englishWorker.Pluralize(str, gender, count);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x000746C8 File Offset: 0x000728C8
		public override string PostProcessThingLabelForRelic(string thingLabel)
		{
			char[] anyOf = new char[]
			{
				' ',
				'-'
			};
			int num = thingLabel.LastIndexOfAny(anyOf);
			if (num != -1)
			{
				thingLabel = thingLabel.Substring(num + 1);
			}
			foreach (string text in new string[]
			{
				"Horn",
				"Lanze",
				"Pulser",
				"Werfer",
				"Axt",
				"Flinte",
				"Bogen",
				"Revolver",
				"Gewehr",
				"Stoßzahn",
				"Stab",
				"Hammer",
				"Schwert",
				"Pistole",
				"Dolch",
				"Büchse",
				"Kanone",
				"Granaten",
				"Granate",
				"Keule",
				"Säbel",
				"Messer",
				"Rapier",
				"Klinge",
				"Sense",
				"Speer"
			})
			{
				if (thingLabel.EndsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					thingLabel = text;
					break;
				}
			}
			return thingLabel;
		}

		// Token: 0x040014E5 RID: 5349
		private static readonly LanguageWorker_English englishWorker = new LanguageWorker_English();
	}
}
