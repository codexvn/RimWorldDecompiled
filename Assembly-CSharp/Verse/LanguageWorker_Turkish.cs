using System;

namespace Verse
{
	// Token: 0x02000391 RID: 913
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x06001765 RID: 5989 RVA: 0x00075904 File Offset: 0x00073B04
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "bir " + str;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00009C2B File Offset: 0x00007E2B
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
