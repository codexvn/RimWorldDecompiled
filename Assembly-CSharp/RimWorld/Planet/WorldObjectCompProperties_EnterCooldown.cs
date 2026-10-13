using System;

namespace RimWorld.Planet
{
	// Token: 0x02003C0F RID: 15375
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x06014DA8 RID: 85416 RVA: 0x0063ED6B File Offset: 0x0063CF6B
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x0400E87F RID: 59519
		public bool autoStartOnMapRemoved = true;

		// Token: 0x0400E880 RID: 59520
		public float durationDays = 1f;
	}
}
