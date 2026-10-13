using System;

namespace Verse.AI.Group
{
	// Token: 0x020010BF RID: 4287
	public abstract class TriggerFilter
	{
		// Token: 0x060068FB RID: 26875
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
