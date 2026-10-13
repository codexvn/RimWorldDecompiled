using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200076F RID: 1903
	public abstract class HediffGiver
	{
		// Token: 0x06002EB5 RID: 11957 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000F0449 File Offset: 0x000EE649
		public virtual float ChanceFactor(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && this.hediff == HediffDefOf.Carcinoma)
			{
				return pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
			}
			return 1f;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000F0474 File Offset: 0x000EE674
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (this.allowOnLodgers || !pawn.IsQuestLodger()) && (this.allowOnQuestRewardPawns || !pawn.IsWorldPawn() || !pawn.IsQuestReward()) && (this.allowOnQuestReservedPawns || !pawn.IsWorldPawn() || Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.ReservedByQuest) && (!ModsConfig.IdeologyActive || this.allowOnBeggars || pawn.kindDef != PawnKindDefOf.Beggar) && (pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeBaby || !Find.Storyteller.difficulty.babiesAreHealthy) && (pawn.genes == null || pawn.genes.HediffGiversCanGive(this.hediff)) && (!pawn.IsMutant || pawn.mutant.HediffGiversCanGive(this.hediff)) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs, true);
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000F056C File Offset: 0x000EE76C
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShortCap, this.hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000F06AC File Offset: 0x000EE8AC
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.hediff == null)
			{
				yield return "hediff is null";
			}
			yield break;
		}

		// Token: 0x04002488 RID: 9352
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x04002489 RID: 9353
		public List<BodyPartDef> partsToAffect;

		// Token: 0x0400248A RID: 9354
		public bool canAffectAnyLivePart;

		// Token: 0x0400248B RID: 9355
		public bool allowOnLodgers = true;

		// Token: 0x0400248C RID: 9356
		public bool allowOnQuestRewardPawns = true;

		// Token: 0x0400248D RID: 9357
		public bool allowOnQuestReservedPawns = true;

		// Token: 0x0400248E RID: 9358
		public bool allowOnBeggars = true;

		// Token: 0x0400248F RID: 9359
		public int countToAffect = 1;
	}
}
