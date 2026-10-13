using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAA RID: 7082
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter_AnyPawn
	{
		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x06009DD9 RID: 40409 RVA: 0x002DB5CC File Offset: 0x002D97CC
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
					if (!this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}
	}
}
