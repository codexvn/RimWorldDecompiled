using System;

namespace Verse
{
	// Token: 0x0200038C RID: 908
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x0600174D RID: 5965 RVA: 0x00075025 File Offset: 0x00073225
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "umas " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "uma " : "um ") + str;
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0007505D File Offset: 0x0007325D
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "as " : "os ") + str;
			}
			return ((gender == Gender.Female) ? "a " : "o ") + str;
		}
	}
}
