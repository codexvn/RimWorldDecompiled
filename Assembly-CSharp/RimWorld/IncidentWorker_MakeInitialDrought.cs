using System;

namespace RimWorld
{
	// Token: 0x02001DB2 RID: 7602
	public class IncidentWorker_MakeInitialDrought : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600A9DB RID: 43483 RVA: 0x00309A3C File Offset: 0x00307C3C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.GetGameConditionDef(parms));
			gameConditionManager.RegisterCondition(cond);
			return true;
		}
	}
}
