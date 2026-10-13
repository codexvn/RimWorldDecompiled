using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001499 RID: 5273
	public class JobGiver_GetHemogen : ThinkNode_JobGiver
	{
		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06007EAA RID: 32426 RVA: 0x00261E60 File Offset: 0x00260060
		public static float HemogenPackHemogenGain
		{
			get
			{
				if (JobGiver_GetHemogen.cachedHemogenPackHemogenGain == null)
				{
					if (!ModsConfig.BiotechActive)
					{
						JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(0f);
					}
					else
					{
						IngestibleProperties ingestible = ThingDefOf.HemogenPack.ingestible;
						object obj;
						if (ingestible == null)
						{
							obj = null;
						}
						else
						{
							List<IngestionOutcomeDoer> outcomeDoers = ingestible.outcomeDoers;
							if (outcomeDoers == null)
							{
								obj = null;
							}
							else
							{
								obj = outcomeDoers.FirstOrDefault((IngestionOutcomeDoer x) => x is IngestionOutcomeDoer_OffsetHemogen);
							}
						}
						IngestionOutcomeDoer_OffsetHemogen ingestionOutcomeDoer_OffsetHemogen = obj as IngestionOutcomeDoer_OffsetHemogen;
						if (ingestionOutcomeDoer_OffsetHemogen == null)
						{
							JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(0f);
						}
						else
						{
							JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(ingestionOutcomeDoer_OffsetHemogen.offset);
						}
					}
				}
				return JobGiver_GetHemogen.cachedHemogenPackHemogenGain.Value;
			}
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x00261F09 File Offset: 0x00260109
		public static void ResetStaticData()
		{
			JobGiver_GetHemogen.cachedHemogenPackHemogenGain = null;
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x00261F16 File Offset: 0x00260116
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) == null)
			{
				return 0f;
			}
			return 9.1f;
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x00261F44 File Offset: 0x00260144
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				return null;
			}
			if (!gene_Hemogen.ShouldConsumeHemogenNow())
			{
				return null;
			}
			if (pawn.IsBloodfeeder())
			{
				Pawn prisoner = this.GetPrisoner(pawn);
				if (prisoner != null)
				{
					return JobMaker.MakeJob(JobDefOf.PrisonerBloodfeed, prisoner);
				}
			}
			if (gene_Hemogen.hemogenPacksAllowed)
			{
				int num = Mathf.FloorToInt((gene_Hemogen.Max - gene_Hemogen.Value) / JobGiver_GetHemogen.HemogenPackHemogenGain);
				if (num > 0)
				{
					Thing hemogenPack = this.GetHemogenPack(pawn);
					if (hemogenPack != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.Ingest, hemogenPack);
						job.count = Mathf.Min(hemogenPack.stackCount, num);
						job.ingestTotalCount = true;
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x00261FFC File Offset: 0x002601FC
		private Thing GetHemogenPack(Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing != null && carriedThing.def == ThingDefOf.HemogenPack)
			{
				return carriedThing;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				if (pawn.inventory.innerContainer[i].def == ThingDefOf.HemogenPack)
				{
					return pawn.inventory.innerContainer[i];
				}
			}
			return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(pawn), null, false);
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x002620EC File Offset: 0x002602EC
		public static AcceptanceReport CanFeedOnPrisoner(Pawn bloodfeeder, Pawn prisoner)
		{
			if (prisoner.WouldDieFromAdditionalBloodLoss(0.4499f))
			{
				return "CannotFeedOnWouldKill".Translate(prisoner.Named("PAWN"));
			}
			if (!prisoner.IsPrisonerOfColony || !prisoner.guest.PrisonerIsSecure || prisoner.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Bloodfeed) || prisoner.IsForbidden(bloodfeeder) || !bloodfeeder.CanReserveAndReach(prisoner, PathEndMode.OnCell, bloodfeeder.NormalMaxDanger(), 1, -1, null, false) || prisoner.InAggroMentalState)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x00262180 File Offset: 0x00260380
		private Pawn GetPrisoner(Pawn pawn)
		{
			return (Pawn)GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.mapPawns.PrisonersOfColonySpawned, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Pawn pawn2 = t as Pawn;
				return pawn2 != null && JobGiver_GetHemogen.CanFeedOnPrisoner(pawn, pawn2).Accepted;
			}, null, false);
		}

		// Token: 0x040054EF RID: 21743
		private static float? cachedHemogenPackHemogenGain;
	}
}
