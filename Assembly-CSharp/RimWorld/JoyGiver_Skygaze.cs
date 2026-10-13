using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014F3 RID: 5363
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x06007FED RID: 32749 RVA: 0x00268184 File Offset: 0x00266384
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x002681B4 File Offset: 0x002663B4
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null) || pawn.Map.weatherManager.curWeather.rainRate > 0.1f || pawn.Map.weatherManager.curWeather.preventSkygaze || (ModsConfig.AnomalyActive && pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness)))
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
