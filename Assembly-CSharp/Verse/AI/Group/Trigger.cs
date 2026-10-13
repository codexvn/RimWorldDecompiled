using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010BD RID: 4285
	public abstract class Trigger
	{
		// Token: 0x060068F5 RID: 26869
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x060068F6 RID: 26870 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x00212105 File Offset: 0x00210305
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04004A5A RID: 19034
		public TriggerData data;

		// Token: 0x04004A5B RID: 19035
		public List<TriggerFilter> filters;
	}
}
