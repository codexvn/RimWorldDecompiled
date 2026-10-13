using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200115B RID: 4443
	public static class SchoolUtility
	{
		// Token: 0x06006CBA RID: 27834 RVA: 0x00222668 File Offset: 0x00220868
		public static Pawn FindTeacher(Pawn child)
		{
			foreach (Pawn pawn in ChildcareUtility.SpawnedColonistChildcarers(child.Map))
			{
				if (SchoolUtility.CanTeachNow(pawn) && pawn.CanReach(child, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06006CBB RID: 27835 RVA: 0x002226DC File Offset: 0x002208DC
		public static bool CanTeachNow(Pawn teacher)
		{
			return !teacher.DevelopmentalStage.Juvenile() && !teacher.Downed && !teacher.Drafted && !teacher.WorkTypeIsDisabled(WorkTypeDefOf.Childcare) && teacher.workSettings.WorkIsActive(WorkTypeDefOf.Childcare) && teacher.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && teacher.Awake() && !teacher.IsBurning() && !PawnUtility.WillSoonHaveBasicNeed(teacher, 0.1f) && !teacher.InMentalState && teacher.GetLord() == null;
		}

		// Token: 0x06006CBC RID: 27836 RVA: 0x0022276C File Offset: 0x0022096C
		public static bool NeedsTeacher(Pawn student)
		{
			if (student.CurJobDef != JobDefOf.Lessontaking)
			{
				return false;
			}
			Pawn pawn = student.CurJob.GetTarget(TargetIndex.B).Pawn;
			return pawn == null || pawn.CurJobDef != JobDefOf.Lessongiving || !(pawn.CurJob.GetTarget(TargetIndex.B) == student);
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x002227C9 File Offset: 0x002209C9
		public static IntVec3 DeskSpotTeacher(Thing desk)
		{
			if (desk.def != ThingDefOf.SchoolDesk)
			{
				return IntVec3.Invalid;
			}
			return desk.InteractionCells[1];
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x002227EA File Offset: 0x002209EA
		public static IntVec3 DeskSpotStudent(Thing desk)
		{
			if (desk.def != ThingDefOf.SchoolDesk)
			{
				return IntVec3.Invalid;
			}
			return desk.InteractionCells[0];
		}

		// Token: 0x06006CBF RID: 27839 RVA: 0x0022280C File Offset: 0x00220A0C
		public static Thing ClosestSchoolDesk(Pawn child, Pawn teacher)
		{
			if (teacher == null)
			{
				return null;
			}
			return GenClosest.ClosestThingReachable(child.Position, child.Map, ThingRequest.ForDef(ThingDefOf.SchoolDesk), PathEndMode.InteractionCell, TraverseParms.For(child, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing d) => child.CanReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(d), false) && teacher.CanReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(d), false) && !d.IsForbidden(child) && !d.IsForbidden(teacher), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x04004C55 RID: 19541
		public const TargetIndex DeskIndex = TargetIndex.A;

		// Token: 0x04004C56 RID: 19542
		public const TargetIndex TeacherIndex = TargetIndex.B;

		// Token: 0x04004C57 RID: 19543
		public const TargetIndex StudentIndex = TargetIndex.B;

		// Token: 0x04004C58 RID: 19544
		private const int DeskStudentInteractionIndex = 0;

		// Token: 0x04004C59 RID: 19545
		private const int DeskTeacherInteractionIndex = 1;
	}
}
