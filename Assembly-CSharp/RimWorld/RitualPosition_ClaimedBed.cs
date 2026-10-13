using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002791 RID: 10129
	public class RitualPosition_ClaimedBed : RitualPosition
	{
		// Token: 0x0600DDEE RID: 56814 RVA: 0x004150D4 File Offset: 0x004132D4
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Pawn pawn = (this.ofPawn == null) ? p : ritual.assignments.FirstAssignedPawn(this.ofPawn);
			if (pawn.ownership != null && pawn.ownership.OwnedBed != null)
			{
				return new PawnStagePosition(pawn.ownership.OwnedBed.Position, null, Rot4.Invalid, this.highlight);
			}
			if (this.tryClaimIfNoneClaimed)
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(pawn);
				if (building_Bed != null)
				{
					pawn.ownership.ClaimBedIfNonMedical(building_Bed);
					return new PawnStagePosition(building_Bed.Position, null, Rot4.Invalid, this.highlight);
				}
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x0600DDEF RID: 56815 RVA: 0x00415182 File Offset: 0x00413382
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.ofPawn, "ofPawn", null, false);
			Scribe_Values.Look<bool>(ref this.tryClaimIfNoneClaimed, "tryClaimIfNoneClaimed", false, false);
		}

		// Token: 0x040095A4 RID: 38308
		[NoTranslate]
		public string ofPawn;

		// Token: 0x040095A5 RID: 38309
		public bool tryClaimIfNoneClaimed;
	}
}
