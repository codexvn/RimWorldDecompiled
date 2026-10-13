using System;

namespace Verse.AI.Group
{
	// Token: 0x020010DF RID: 4319
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x06006940 RID: 26944 RVA: 0x00212C76 File Offset: 0x00210E76
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x00212C98 File Offset: 0x00210E98
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x04004A78 RID: 19064
		private Thing thing;

		// Token: 0x04004A79 RID: 19065
		private float damageFraction = 0.5f;
	}
}
