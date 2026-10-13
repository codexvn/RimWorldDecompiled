using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A3D RID: 6717
	public class GatheringWorker_MarriageCeremony : GatheringWorker
	{
		// Token: 0x0600974B RID: 38731 RVA: 0x002C323E File Offset: 0x002C143E
		private static void FindFiancees(Pawn organizer, out Pawn firstFiance, out Pawn secondFiance)
		{
			firstFiance = organizer;
			secondFiance = organizer.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
		}

		// Token: 0x0600974C RID: 38732 RVA: 0x002C3258 File Offset: 0x002C1458
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			Pawn firstPawn;
			Pawn secondPawn;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstPawn, out secondPawn);
			return new LordJob_Joinable_MarriageCeremony(firstPawn, secondPawn, spot);
		}

		// Token: 0x0600974D RID: 38733 RVA: 0x002C3278 File Offset: 0x002C1478
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Pawn firstFiance;
			Pawn secondFiance;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstFiance, out secondFiance);
			return RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out spot);
		}

		// Token: 0x0600974E RID: 38734 RVA: 0x002C3298 File Offset: 0x002C1498
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Pawn pawn;
			Pawn pawn2;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
			Messages.Message("MessageNewMarriageCeremony".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("PAWN1"), pawn2.Named("PAWN2")), new TargetInfo(spot, pawn.Map, false), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600974F RID: 38735 RVA: 0x002C3308 File Offset: 0x002C1508
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer != null)
			{
				Pawn pawn;
				Pawn pawn2;
				GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
				if (!GatheringsUtility.PawnCanStartOrContinueGathering(pawn) || !GatheringsUtility.PawnCanStartOrContinueGathering(pawn2))
				{
					return false;
				}
			}
			return base.CanExecute(map, organizer);
		}
	}
}
