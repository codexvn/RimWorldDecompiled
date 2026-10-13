using System;

namespace RimWorld
{
	// Token: 0x0200271F RID: 10015
	public abstract class RitualObligationTriggerProperties
	{
		// Token: 0x0600DBDF RID: 56287 RVA: 0x0040CEAB File Offset: 0x0040B0AB
		public virtual RitualObligationTrigger GetInstance(Precept_Ritual parent)
		{
			RitualObligationTrigger ritualObligationTrigger = (RitualObligationTrigger)Activator.CreateInstance(this.triggerClass);
			ritualObligationTrigger.ritual = parent;
			return ritualObligationTrigger;
		}

		// Token: 0x0400949C RID: 38044
		public Type triggerClass;

		// Token: 0x0400949D RID: 38045
		public bool mustBePlayerIdeo;
	}
}
