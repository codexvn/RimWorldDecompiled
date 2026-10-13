using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200242A RID: 9258
	public class PawnRelationWorker_Overseer : PawnRelationWorker
	{
		// Token: 0x0600C861 RID: 51297 RVA: 0x003A82B1 File Offset: 0x003A64B1
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			base.OnRelationCreated(firstPawn, secondPawn);
			if (MechanitorUtility.IsMechanitor(firstPawn))
			{
				firstPawn.mechanitor.AssignPawnControlGroup(secondPawn, null);
				return;
			}
			if (MechanitorUtility.IsMechanitor(secondPawn))
			{
				secondPawn.mechanitor.AssignPawnControlGroup(firstPawn, null);
			}
		}

		// Token: 0x0600C862 RID: 51298 RVA: 0x003A82E6 File Offset: 0x003A64E6
		public override void OnRelationRemoved(Pawn firstPawn, Pawn secondPawn)
		{
			base.OnRelationRemoved(firstPawn, secondPawn);
			if (MechanitorUtility.IsMechanitor(firstPawn))
			{
				firstPawn.mechanitor.UnassignPawnFromAnyControlGroup(secondPawn);
				return;
			}
			if (MechanitorUtility.IsMechanitor(secondPawn))
			{
				secondPawn.mechanitor.UnassignPawnFromAnyControlGroup(firstPawn);
			}
		}

		// Token: 0x0600C863 RID: 51299 RVA: 0x003A831C File Offset: 0x003A651C
		public override void Notify_PostRemovedByDeath(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn = MechanitorUtility.IsMechanitor(firstPawn) ? firstPawn : secondPawn;
			Pawn pawn2 = (firstPawn == pawn) ? secondPawn : firstPawn;
			if (!pawn2.Dead)
			{
				CompOverseerSubject overseerSubject = pawn2.OverseerSubject;
				if (overseerSubject != null)
				{
					overseerSubject.Notify_DisconnectedFromOverseer();
				}
			}
			if (pawn != null && !pawn.Dead)
			{
				Messages.Message(Find.ActiveLanguageWorker.PostProcessed("MessageMechanitorLostControlOfMech".Translate(pawn, pawn2) + ": " + pawn2.LabelShortCap), new LookTargets(new Thing[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600C864 RID: 51300 RVA: 0x003A83BC File Offset: 0x003A65BC
		public override void Notify_PostRemovedLeftBehind(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn = MechanitorUtility.IsMechanitor(firstPawn) ? firstPawn : secondPawn;
			Pawn pawn2 = (firstPawn == pawn) ? secondPawn : firstPawn;
			if (!pawn2.Dead)
			{
				CompOverseerSubject overseerSubject = pawn2.OverseerSubject;
				if (overseerSubject != null)
				{
					overseerSubject.Notify_DisconnectedFromOverseer();
				}
			}
			if (pawn != null && !pawn.Dead)
			{
				Messages.Message(Find.ActiveLanguageWorker.PostProcessed("MessageMechanitorLostControlOfMech".Translate(pawn, pawn2) + ": " + pawn2.LabelShortCap), new LookTargets(new Thing[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
			}
		}
	}
}
