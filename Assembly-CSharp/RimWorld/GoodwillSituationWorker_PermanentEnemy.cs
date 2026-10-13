using System;

namespace RimWorld
{
	// Token: 0x0200259E RID: 9630
	public class GoodwillSituationWorker_PermanentEnemy : GoodwillSituationWorker
	{
		// Token: 0x0600D310 RID: 54032 RVA: 0x003E2A41 File Offset: 0x003E0C41
		public override int GetMaxGoodwill(Faction other)
		{
			if (Faction.OfPlayerSilentFail == null)
			{
				return 100;
			}
			if (!GoodwillSituationWorker_PermanentEnemy.ArePermanentEnemies(Faction.OfPlayer, other))
			{
				return 100;
			}
			return -100;
		}

		// Token: 0x0600D311 RID: 54033 RVA: 0x003E2A60 File Offset: 0x003E0C60
		public static bool ArePermanentEnemies(Faction a, Faction b)
		{
			return a.def.permanentEnemy || b.def.permanentEnemy || (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer) || (b.def.permanentEnemyToEveryoneExceptPlayer && !a.IsPlayer) || (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def)) || (b.def.permanentEnemyToEveryoneExcept != null && !b.def.permanentEnemyToEveryoneExcept.Contains(a.def));
		}
	}
}
