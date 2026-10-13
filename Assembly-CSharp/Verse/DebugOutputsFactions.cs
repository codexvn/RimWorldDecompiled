using System;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000A89 RID: 2697
	public class DebugOutputsFactions
	{
		// Token: 0x060043D3 RID: 17363 RVA: 0x0015EB97 File Offset: 0x0015CD97
		[DebugOutput("Factions", false)]
		public static void AllFactions()
		{
			Find.FactionManager.LogAllFactions();
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0015EBA3 File Offset: 0x0015CDA3
		[DebugOutput("Factions", false)]
		public static void AllFactionsToRemove()
		{
			Find.FactionManager.LogFactionsToRemove();
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0015EBAF File Offset: 0x0015CDAF
		[DebugOutput("Factions", false)]
		public static void AllFactionsFromPawns()
		{
			Find.FactionManager.LogFactionsOnPawns();
		}
	}
}
