using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010DA RID: 4314
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x06006936 RID: 26934 RVA: 0x00212ACC File Offset: 0x00210CCC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn_NeedsTracker needs = lord.ownedPawns[i].needs;
					bool flag;
					if (needs == null)
					{
						flag = false;
					}
					else
					{
						Need_Food food = needs.food;
						HungerCategory? hungerCategory = (food != null) ? new HungerCategory?(food.CurCategory) : null;
						HungerCategory hungerCategory2 = HungerCategory.UrgentlyHungry;
						flag = (hungerCategory.GetValueOrDefault() >= hungerCategory2 & hungerCategory != null);
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
