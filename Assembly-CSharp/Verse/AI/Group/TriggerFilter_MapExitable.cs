using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C1 RID: 4289
	public class TriggerFilter_MapExitable : TriggerFilter
	{
		// Token: 0x060068FF RID: 26879 RVA: 0x002121EF File Offset: 0x002103EF
		public override bool AllowActivation(Lord lord, TriggerSignal signal)
		{
			return lord.Map.CanEverExit;
		}
	}
}
