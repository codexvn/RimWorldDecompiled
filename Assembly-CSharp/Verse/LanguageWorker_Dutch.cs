using System;

namespace Verse
{
	// Token: 0x02000383 RID: 899
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x06001716 RID: 5910 RVA: 0x00073641 File Offset: 0x00071841
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "een " + str;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00073659 File Offset: 0x00071859
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "de " + str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "de " + str;
			}
			return "het " + str;
		}
	}
}
