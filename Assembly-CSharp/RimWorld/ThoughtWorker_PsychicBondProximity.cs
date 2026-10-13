using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E1 RID: 6369
	public class ThoughtWorker_PsychicBondProximity : ThoughtWorker
	{
		// Token: 0x06009104 RID: 37124 RVA: 0x002AAB5C File Offset: 0x002A8D5C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Hediff_PsychicBond hediff_PsychicBond = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (((hediff_PsychicBond != null) ? hediff_PsychicBond.target : null) == null)
			{
				return ThoughtState.Inactive;
			}
			if (ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(p, hediff_PsychicBond))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}

		// Token: 0x06009105 RID: 37125 RVA: 0x002AABBC File Offset: 0x002A8DBC
		public static bool NearPsychicBondedPerson(Pawn pawn, Hediff_PsychicBond bondHediff)
		{
			Thing thing = (bondHediff != null) ? bondHediff.target : null;
			Pawn bondedPawn = thing as Pawn;
			if (bondedPawn == null)
			{
				return false;
			}
			IThingHolder parentHolder = pawn.ParentHolder;
			IThingHolder parentHolder2 = bondedPawn.ParentHolder;
			return (parentHolder == null || !ThingOwnerUtility.ContentsSuspended(parentHolder)) && (parentHolder2 == null || !ThingOwnerUtility.ContentsSuspended(parentHolder2)) && ((parentHolder != null && parentHolder == parentHolder2) || QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true).FirstOrDefault((QuestPart_LendColonistsToFaction p) => p.LentColonistsListForReading.Contains(pawn) && p.LentColonistsListForReading.Contains(bondedPawn)) != null || pawn.MapHeld == bondedPawn.MapHeld);
		}
	}
}
