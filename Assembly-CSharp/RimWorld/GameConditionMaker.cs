using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D28 RID: 7464
	public static class GameConditionMaker
	{
		// Token: 0x0600A720 RID: 42784 RVA: 0x002FE85A File Offset: 0x002FCA5A
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		// Token: 0x0600A721 RID: 42785 RVA: 0x002FE87C File Offset: 0x002FCA7C
		public static GameCondition MakeCondition(GameConditionDef def, int duration = -1)
		{
			GameCondition gameCondition = (GameCondition)Activator.CreateInstance(def.conditionClass);
			gameCondition.startTick = Find.TickManager.TicksGame;
			gameCondition.def = def;
			gameCondition.Duration = duration;
			gameCondition.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
			gameCondition.PostMake();
			return gameCondition;
		}
	}
}
