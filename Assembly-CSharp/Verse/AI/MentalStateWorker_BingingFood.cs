using System;

namespace Verse.AI
{
	// Token: 0x02000F9C RID: 3996
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		// Token: 0x0600627C RID: 25212 RVA: 0x001F128E File Offset: 0x001EF48E
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.needs.food != null && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
