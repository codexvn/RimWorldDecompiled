using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010D2 RID: 4306
	public class Trigger_OnPlayerMechHarmAnything : Trigger
	{
		// Token: 0x06006924 RID: 26916 RVA: 0x0021276E File Offset: 0x0021096E
		public Trigger_OnPlayerMechHarmAnything(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x00212780 File Offset: 0x00210980
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (ModsConfig.BiotechActive && signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null)
			{
				Pawn pawn = signal.dinfo.Instigator as Pawn;
				if (pawn != null && pawn.IsColonyMech && this.things.Contains(signal.thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004A70 RID: 19056
		private List<Thing> things;
	}
}
