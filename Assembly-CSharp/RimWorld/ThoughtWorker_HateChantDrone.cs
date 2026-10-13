using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189B RID: 6299
	public class ThoughtWorker_HateChantDrone : ThoughtWorker
	{
		// Token: 0x06009027 RID: 36903 RVA: 0x002A85EC File Offset: 0x002A67EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			HateChantDroneLevel hateChantDroneLevel = HateChantDroneLevel.None;
			Map mapHeld = p.MapHeld;
			if (mapHeld != null)
			{
				GameCondition_HateChantDrone activeCondition = mapHeld.gameConditionManager.GetActiveCondition<GameCondition_HateChantDrone>();
				if (activeCondition == null || (ModsConfig.AnomalyActive && p.Faction == Faction.OfHoraxCult))
				{
					return false;
				}
				if (activeCondition.level > hateChantDroneLevel)
				{
					hateChantDroneLevel = activeCondition.level;
				}
			}
			switch (hateChantDroneLevel)
			{
			case HateChantDroneLevel.None:
				return false;
			case HateChantDroneLevel.VeryLow:
				return ThoughtState.ActiveAtStage(0);
			case HateChantDroneLevel.Low:
				return ThoughtState.ActiveAtStage(1);
			case HateChantDroneLevel.Medium:
				return ThoughtState.ActiveAtStage(2);
			case HateChantDroneLevel.High:
				return ThoughtState.ActiveAtStage(3);
			case HateChantDroneLevel.Extreme:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
