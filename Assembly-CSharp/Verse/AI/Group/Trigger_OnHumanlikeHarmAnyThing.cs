using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010D1 RID: 4305
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		// Token: 0x06006922 RID: 26914 RVA: 0x002126E2 File Offset: 0x002108E2
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x002126F4 File Offset: 0x002108F4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null)
			{
				Pawn pawn = signal.dinfo.Instigator as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004A6F RID: 19055
		private List<Thing> things;
	}
}
