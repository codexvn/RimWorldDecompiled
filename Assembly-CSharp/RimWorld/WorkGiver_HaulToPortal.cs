using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015E4 RID: 5604
	public class WorkGiver_HaulToPortal : WorkGiver_Scanner
	{
		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06008444 RID: 33860 RVA: 0x0027A801 File Offset: 0x00278A01
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.MapPortal);
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06008445 RID: 33861 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008446 RID: 33862 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06008447 RID: 33863 RVA: 0x0027A80C File Offset: 0x00278A0C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			MapPortal portal = t as MapPortal;
			return EnterPortalUtility.HasJobOnPortal(pawn, portal);
		}

		// Token: 0x06008448 RID: 33864 RVA: 0x0027A828 File Offset: 0x00278A28
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			MapPortal portal = t as MapPortal;
			return EnterPortalUtility.JobOnPortal(pawn, portal);
		}
	}
}
