using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010E0 RID: 4320
	public class Trigger_AnyThingDamageTaken : Trigger
	{
		// Token: 0x06006942 RID: 26946 RVA: 0x00212CE6 File Offset: 0x00210EE6
		public Trigger_AnyThingDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x00212D08 File Offset: 0x00210F08
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				foreach (Thing thing in this.things)
				{
					if (thing.DestroyedOrNull() || (float)thing.HitPoints < (1f - this.damageFraction) * (float)thing.MaxHitPoints)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04004A7A RID: 19066
		private List<Thing> things;

		// Token: 0x04004A7B RID: 19067
		private float damageFraction = 0.5f;
	}
}
