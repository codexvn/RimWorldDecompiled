using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000379 RID: 889
	public class LanguageInfo
	{
		// Token: 0x040014BB RID: 5307
		public string friendlyNameNative;

		// Token: 0x040014BC RID: 5308
		public string friendlyNameEnglish;

		// Token: 0x040014BD RID: 5309
		public bool canBeTiny = true;

		// Token: 0x040014BE RID: 5310
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x040014BF RID: 5311
		public Type languageWorkerClass = typeof(LanguageWorker_Default);

		// Token: 0x040014C0 RID: 5312
		public int? totalNumCaseCount;
	}
}
