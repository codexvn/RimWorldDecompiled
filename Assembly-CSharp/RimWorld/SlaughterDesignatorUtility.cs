using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030BE RID: 12478
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x0601185A RID: 71770 RVA: 0x0050833C File Offset: 0x0050653C
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0601185B RID: 71771 RVA: 0x005083D8 File Offset: 0x005065D8
		public static void CheckWarnAboutVeneratedAnimal(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (pawn.MapHeld.mapPawns.FreeColonistsSpawned.Count == 0)
			{
				return;
			}
			bool flag = true;
			foreach (Pawn pawn2 in pawn.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn2.WorkTypeIsDisabled(WorkTypeDefOf.Hunting) && (pawn2.Ideo == null || !pawn2.Ideo.IsVeneratedAnimal(pawn)))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalIsVeneratedForAllColonists".Translate(pawn.GetKindLabelPlural(-1).CapitalizeFirst().Named("PAWNKINDLABELPLURAL"), Faction.OfPlayer.def.pawnsPlural.Named("PAWNS")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
