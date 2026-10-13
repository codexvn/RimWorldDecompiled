using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003025 RID: 12325
	public class Alert_PokerTableNoChairs : Alert_JoyBuildingNoChairs
	{
		// Token: 0x17002CEE RID: 11502
		// (get) Token: 0x06011555 RID: 70997 RVA: 0x004F896D File Offset: 0x004F6B6D
		protected override JoyGiverDef JoyGiver
		{
			get
			{
				return JoyGiverDefOf.Play_Poker;
			}
		}

		// Token: 0x06011556 RID: 70998 RVA: 0x004F8974 File Offset: 0x004F6B74
		public Alert_PokerTableNoChairs()
		{
			this.defaultLabel = "PokerTablesNeedChairs".Translate();
			this.defaultExplanation = "PokerTablesNeedChairsDesc".Translate();
		}
	}
}
