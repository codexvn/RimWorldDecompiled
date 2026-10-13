using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001623 RID: 5667
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x060085A0 RID: 34208 RVA: 0x0027EA00 File Offset: 0x0027CC00
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x060085A1 RID: 34209 RVA: 0x0026F78A File Offset: 0x0026D98A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x060085A2 RID: 34210 RVA: 0x0027EA10 File Offset: 0x0027CC10
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate(), null);
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
