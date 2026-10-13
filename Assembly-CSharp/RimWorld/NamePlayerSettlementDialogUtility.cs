using System;
using RimWorld.Planet;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200319E RID: 12702
	public static class NamePlayerSettlementDialogUtility
	{
		// Token: 0x06011DFA RID: 73210 RVA: 0x0053069E File Offset: 0x0052E89E
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x06011DFB RID: 73211 RVA: 0x005306C1 File Offset: 0x0052E8C1
		public static void Named(Settlement factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}

		// Token: 0x0400B8D3 RID: 47315
		public const int CharacterLimit = 64;
	}
}
