using System;

namespace RimWorld
{
	// Token: 0x020023AA RID: 9130
	public static class PawnPostureUtility
	{
		// Token: 0x0600C5CC RID: 50636 RVA: 0x001ACE80 File Offset: 0x001AB080
		public static bool Laying(this PawnPosture posture)
		{
			return (posture & PawnPosture.LayingOnGroundNormal) > PawnPosture.Standing;
		}

		// Token: 0x0600C5CD RID: 50637 RVA: 0x001ACE90 File Offset: 0x001AB090
		public static bool InBed(this PawnPosture posture)
		{
			return (posture & PawnPosture.InBedMask) > PawnPosture.Standing;
		}

		// Token: 0x0600C5CE RID: 50638 RVA: 0x001ACE88 File Offset: 0x001AB088
		public static bool FaceUp(this PawnPosture posture)
		{
			return (posture & PawnPosture.FaceUpMask) > PawnPosture.Standing;
		}
	}
}
