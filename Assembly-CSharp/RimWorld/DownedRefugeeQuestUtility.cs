using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B72 RID: 7026
	public static class DownedRefugeeQuestUtility
	{
		// Token: 0x06009CBE RID: 40126 RVA: 0x002D74C4 File Offset: 0x002D56C4
		public static Pawn GenerateRefugee(PlanetTile tile, PawnKindDef pawnKind = null, float chanceForFaction = 0.6f)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind ?? PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(chanceForFaction), PawnGenerationContext.NonPlayer, new PlanetTile?(tile), false, false, false, true, false, 20f, true, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, new float?(0.2f), null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			return pawn;
		}

		// Token: 0x06009CBF RID: 40127 RVA: 0x002D757C File Offset: 0x002D577C
		public static Faction GetRandomFactionForRefugee(float chanceForFaction = 0.6f)
		{
			Faction result;
			if (Rand.Chance(chanceForFaction) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04006CAE RID: 27822
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x04006CAF RID: 27823
		private const float ChanceToRedressWorldPawn = 0.2f;
	}
}
