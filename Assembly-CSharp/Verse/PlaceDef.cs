using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020002C6 RID: 710
	public class PlaceDef : Def
	{
		// Token: 0x04000F78 RID: 3960
		public RulePack placeRules;

		// Token: 0x04000F79 RID: 3961
		[NoTranslate]
		public List<string> tags = new List<string>();
	}
}
