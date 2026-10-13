using System;

namespace RimWorld
{
	// Token: 0x0200259D RID: 9629
	public class GoodwillSituationWorker_NaturalEnemy : GoodwillSituationWorker
	{
		// Token: 0x0600D30E RID: 54030 RVA: 0x003E2A2B File Offset: 0x003E0C2B
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!other.def.naturalEnemy)
			{
				return 0;
			}
			return -130;
		}
	}
}
