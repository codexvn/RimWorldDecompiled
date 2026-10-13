using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC7 RID: 7367
	public class QuestPart_TransporterPawns_Feed : QuestPart_TransporterPawns
	{
		// Token: 0x0600A424 RID: 42020 RVA: 0x002F1046 File Offset: 0x002EF246
		public override void Process(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) != null)
			{
				pawn.needs.food.CurLevel = pawn.needs.food.MaxLevel;
			}
		}
	}
}
