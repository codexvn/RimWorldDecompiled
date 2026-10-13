using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C03 RID: 15363
	public class QuestPart_Filter_DecreeNotPossible : QuestPart_Filter
	{
		// Token: 0x06014D65 RID: 85349 RVA: 0x0063E0D8 File Offset: 0x0063C2D8
		protected override bool Pass(SignalArgs args)
		{
			Pawn pawn;
			return args.TryGetArg<Pawn>("SUBJECT", out pawn) && (pawn.royalty == null || !pawn.royalty.PossibleDecreeQuests.Contains(this.quest.root));
		}
	}
}
