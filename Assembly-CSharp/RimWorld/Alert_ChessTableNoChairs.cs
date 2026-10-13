using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003024 RID: 12324
	public class Alert_ChessTableNoChairs : Alert_JoyBuildingNoChairs
	{
		// Token: 0x17002CED RID: 11501
		// (get) Token: 0x06011553 RID: 70995 RVA: 0x004F8934 File Offset: 0x004F6B34
		protected override JoyGiverDef JoyGiver
		{
			get
			{
				return JoyGiverDefOf.Play_Chess;
			}
		}

		// Token: 0x06011554 RID: 70996 RVA: 0x004F893B File Offset: 0x004F6B3B
		public Alert_ChessTableNoChairs()
		{
			this.defaultLabel = "ChessTablesNeedChairs".Translate();
			this.defaultExplanation = "ChessTablesNeedChairsDesc".Translate();
		}
	}
}
