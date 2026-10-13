using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014DD RID: 5341
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x06007F95 RID: 32661 RVA: 0x00266460 File Offset: 0x00264660
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				return false;
			}
			if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				return WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			return true;
		}

		// Token: 0x06007F96 RID: 32662 RVA: 0x00266490 File Offset: 0x00264690
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c;
			Building t2;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t, c, t2);
		}
	}
}
