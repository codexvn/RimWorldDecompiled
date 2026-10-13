using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAB RID: 7083
	public class QuestPart_Filter_AnyPawnInCombatShape : QuestPart_Filter_AnyPawn
	{
		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06009DDB RID: 40411 RVA: 0x002DB620 File Offset: 0x002D9820
		protected override int PawnsCount
		{
			get
			{
				if (this.pawns.NullOrEmpty<Pawn>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed && !this.pawns[i].Downed)
					{
						num++;
					}
				}
				return num;
			}
		}
	}
}
