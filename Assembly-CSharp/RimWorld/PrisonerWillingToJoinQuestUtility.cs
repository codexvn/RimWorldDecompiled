using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B76 RID: 7030
	public static class PrisonerWillingToJoinQuestUtility
	{
		// Token: 0x06009CCB RID: 40139 RVA: 0x002D791C File Offset: 0x002D5B1C
		public static Pawn GeneratePrisoner(PlanetTile tile, Faction hostFaction)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Slave, hostFaction, PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 75f, true, true, false, true, true, false, false, true, true, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.guest.SetGuestStatus(hostFaction, GuestStatus.Prisoner);
			return pawn;
		}

		// Token: 0x04006CB4 RID: 27828
		private const float RelationWithColonistWeight = 75f;
	}
}
