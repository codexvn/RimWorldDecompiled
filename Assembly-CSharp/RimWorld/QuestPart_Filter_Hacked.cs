using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB9 RID: 7097
	public class QuestPart_Filter_Hacked : QuestPart_Filter
	{
		// Token: 0x06009E07 RID: 40455 RVA: 0x002DBCF8 File Offset: 0x002D9EF8
		protected override bool Pass(SignalArgs args)
		{
			Thing thing;
			if (args.TryGetArg<Thing>("SUBJECT", out thing))
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null)
				{
					return compHackable.IsHacked;
				}
			}
			return false;
		}
	}
}
