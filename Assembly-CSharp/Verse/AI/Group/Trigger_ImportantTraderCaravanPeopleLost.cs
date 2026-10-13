using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010E2 RID: 4322
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x06006946 RID: 26950 RVA: 0x00212EB0 File Offset: 0x002110B0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.Killed))
			{
				if (signal.Pawn.GetTraderCaravanRole() == TraderCaravanRole.Trader || signal.Pawn.RaceProps.packAnimal)
				{
					return true;
				}
				if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
