using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA3 RID: 7075
	public class QuestPart_Filter_AnyColonistWithCharityPrecept : QuestPart_Filter
	{
		// Token: 0x06009DC3 RID: 40387 RVA: 0x002DB2D9 File Offset: 0x002D94D9
		protected override bool Pass(SignalArgs args)
		{
			return IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>();
		}
	}
}
