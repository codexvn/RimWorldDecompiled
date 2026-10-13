using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200152E RID: 5422
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x060080EE RID: 33006 RVA: 0x0026D338 File Offset: 0x0026B538
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			if (!SocialInteractionUtility.BestInteractableCell(pawn, mentalState_InsultingSpree.target).IsValid)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
