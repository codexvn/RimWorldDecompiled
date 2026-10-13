using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020039B3 RID: 14771
	public class QuestTextRequest
	{
		// Token: 0x0400DFD9 RID: 57305
		public string keyword;

		// Token: 0x0400DFDA RID: 57306
		public Action<string> setter;

		// Token: 0x0400DFDB RID: 57307
		public List<Rule> extraRules;
	}
}
