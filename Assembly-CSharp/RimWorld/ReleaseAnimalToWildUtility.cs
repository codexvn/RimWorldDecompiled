using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030B6 RID: 12470
	public static class ReleaseAnimalToWildUtility
	{
		// Token: 0x0601182B RID: 71723 RVA: 0x0050775C File Offset: 0x0050595C
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageReleaseBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0601182C RID: 71724 RVA: 0x005077F8 File Offset: 0x005059F8
		public static void DoReleaseAnimal(Pawn animal, Pawn releasedBy)
		{
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(animal, null, PawnDiedOrDownedThoughtsKind.ReleasedToWild);
			releasedBy.interactions.TryInteractWith(animal, InteractionDefOf.ReleaseToWild);
			animal.Map.designationManager.RemoveDesignation(animal.Map.designationManager.DesignationOn(animal, DesignationDefOf.ReleaseAnimalToWild));
			animal.SetFaction(null, null);
			Pawn_Ownership ownership = animal.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			Messages.Message("MessageAnimalReturnedWildReleased".Translate(animal.LabelShort, animal), releasedBy, MessageTypeDefOf.NeutralEvent, true);
		}
	}
}
