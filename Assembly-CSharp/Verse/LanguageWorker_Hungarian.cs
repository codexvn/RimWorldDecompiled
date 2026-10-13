using System;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x06001735 RID: 5941 RVA: 0x000748D0 File Offset: 0x00072AD0
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000748E4 File Offset: 0x00072AE4
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
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "az " + str;
			}
			return "a " + str;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00074929 File Offset: 0x00072B29
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}
