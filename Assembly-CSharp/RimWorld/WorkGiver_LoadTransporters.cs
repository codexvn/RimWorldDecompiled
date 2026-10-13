using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015E8 RID: 5608
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06008460 RID: 33888 RVA: 0x0027AD27 File Offset: 0x00278F27
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06008461 RID: 33889 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008462 RID: 33890 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06008463 RID: 33891 RVA: 0x0027AD30 File Offset: 0x00278F30
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x06008464 RID: 33892 RVA: 0x0027AD4C File Offset: 0x00278F4C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
