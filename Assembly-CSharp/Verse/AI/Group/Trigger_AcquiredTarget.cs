using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010E7 RID: 4327
	public class Trigger_AcquiredTarget : Trigger
	{
		// Token: 0x06006950 RID: 26960 RVA: 0x00213178 File Offset: 0x00211378
		public Trigger_AcquiredTarget(Faction requireTargetWithSpecificFaction)
		{
			this.faction = requireTargetWithSpecificFaction;
		}

		// Token: 0x06006951 RID: 26961 RVA: 0x00213187 File Offset: 0x00211387
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type != TriggerSignalType.AcquiredTarget)
			{
				return false;
			}
			if (this.faction != null)
			{
				Thing otherThing = signal.otherThing;
				if (((otherThing != null) ? otherThing.Faction : null) != this.faction)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004A81 RID: 19073
		private Faction faction;
	}
}
