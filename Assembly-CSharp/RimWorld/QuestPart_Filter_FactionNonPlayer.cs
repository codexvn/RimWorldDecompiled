using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB7 RID: 7095
	public class QuestPart_Filter_FactionNonPlayer : QuestPart_Filter
	{
		// Token: 0x06009E03 RID: 40451 RVA: 0x002DBC88 File Offset: 0x002D9E88
		protected override bool Pass(SignalArgs args)
		{
			Faction faction;
			if (args.TryGetArg<Faction>("FACTION", out faction))
			{
				return faction != Faction.OfPlayer;
			}
			Thing thing;
			return args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Faction != Faction.OfPlayer;
		}
	}
}
