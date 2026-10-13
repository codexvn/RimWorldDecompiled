using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC8 RID: 7368
	public class QuestPart_TransporterPawns_Tend : QuestPart_TransporterPawns
	{
		// Token: 0x0600A426 RID: 42022 RVA: 0x002F1084 File Offset: 0x002EF284
		public override void Process(Pawn pawn)
		{
			int num = 0;
			while (pawn.health.HasHediffsNeedingTend(false))
			{
				num++;
				if (num > 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				this.DoTend(pawn);
			}
		}

		// Token: 0x0600A427 RID: 42023 RVA: 0x002F10C1 File Offset: 0x002EF2C1
		protected virtual void DoTend(Pawn pawn)
		{
			TendUtility.DoTend(null, pawn, null);
		}
	}
}
