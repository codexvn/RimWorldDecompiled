using System;

namespace Verse
{
	// Token: 0x02000390 RID: 912
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x06001761 RID: 5985 RVA: 0x00075852 File Offset: 0x00073A52
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "en " + str;
			}
			return "ett " + str;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0007587C File Offset: 0x00073A7C
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
			char ch = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (this.IsVowel(ch))
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (this.IsVowel(ch))
				{
					return str + "t";
				}
				return str + "et";
			}
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000758F1 File Offset: 0x00073AF1
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}
