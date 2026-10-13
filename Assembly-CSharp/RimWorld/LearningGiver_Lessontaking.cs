using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200151B RID: 5403
	public class LearningGiver_Lessontaking : LearningGiver
	{
		// Token: 0x060080B4 RID: 32948 RVA: 0x0026C6B0 File Offset: 0x0026A8B0
		public override bool CanDo(Pawn pawn)
		{
			if (!base.CanDo(pawn))
			{
				return false;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f))
			{
				return false;
			}
			Pawn pawn2 = SchoolUtility.FindTeacher(pawn);
			if (pawn2 == null)
			{
				return false;
			}
			Thing thing = SchoolUtility.ClosestSchoolDesk(pawn, pawn2);
			return thing != null && pawn.CanReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(thing), false);
		}

		// Token: 0x060080B5 RID: 32949 RVA: 0x0026C700 File Offset: 0x0026A900
		public override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = SchoolUtility.FindTeacher(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Thing thing = SchoolUtility.ClosestSchoolDesk(pawn, pawn2);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, thing);
		}
	}
}
