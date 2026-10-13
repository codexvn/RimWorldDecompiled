using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200240E RID: 9230
	public static class LovePartnerRelationUtility
	{
		// Token: 0x0600C7E2 RID: 51170 RVA: 0x003A5E4F File Offset: 0x003A404F
		public static bool HasAnyLovePartner(Pawn pawn, bool allowDead = true)
		{
			return LovePartnerRelationUtility.ExistingLovePartner(pawn, allowDead) != null;
		}

		// Token: 0x0600C7E3 RID: 51171 RVA: 0x003A5E5B File Offset: 0x003A405B
		public static bool IsLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Spouse;
		}

		// Token: 0x0600C7E4 RID: 51172 RVA: 0x003A5E77 File Offset: 0x003A4077
		public static bool IsExLovePartnerRelation(PawnRelationDef relation)
		{
			return relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse;
		}

		// Token: 0x0600C7E5 RID: 51173 RVA: 0x003A5E8C File Offset: 0x003A408C
		public static bool HasAnyLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x0600C7E6 RID: 51174 RVA: 0x003A5ECC File Offset: 0x003A40CC
		public static bool HasAnyExLovePartnerOfTheSameGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender == pawn.gender) != null;
		}

		// Token: 0x0600C7E7 RID: 51175 RVA: 0x003A5F0C File Offset: 0x003A410C
		public static bool HasAnyLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x0600C7E8 RID: 51176 RVA: 0x003A5F4C File Offset: 0x003A414C
		public static bool HasAnyExLovePartnerOfTheOppositeGender(Pawn pawn)
		{
			return pawn.relations.DirectRelations.Find((DirectPawnRelation x) => LovePartnerRelationUtility.IsExLovePartnerRelation(x.def) && x.otherPawn.gender != pawn.gender) != null;
		}

		// Token: 0x0600C7E9 RID: 51177 RVA: 0x003A5F8C File Offset: 0x003A418C
		public static Pawn ExistingLovePartner(Pawn pawn, bool allowDead = true)
		{
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					return directRelations[i].otherPawn;
				}
			}
			return null;
		}

		// Token: 0x0600C7EA RID: 51178 RVA: 0x003A5FF0 File Offset: 0x003A41F0
		public static List<DirectPawnRelation> ExistingLovePartners(Pawn pawn, bool allowDead = true)
		{
			LovePartnerRelationUtility.tmpExistingLovePartners.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					LovePartnerRelationUtility.tmpExistingLovePartners.Add(directRelations[i]);
				}
			}
			return LovePartnerRelationUtility.tmpExistingLovePartners;
		}

		// Token: 0x0600C7EB RID: 51179 RVA: 0x003A6068 File Offset: 0x003A4268
		public static DirectPawnRelation ExistingLoveRealtionshipBetween(Pawn pawn, Pawn other, bool allowDead = true)
		{
			return (from r in LovePartnerRelationUtility.ExistingLovePartners(pawn, allowDead)
			where r.otherPawn == other
			select r).FirstOrDefault<DirectPawnRelation>();
		}

		// Token: 0x0600C7EC RID: 51180 RVA: 0x003A60A0 File Offset: 0x003A42A0
		public static int ExistingLovePartnersCount(Pawn pawn, bool allowDead = true)
		{
			int num = 0;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (!directRelations[i].otherPawn.Destroyed || allowDead))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600C7ED RID: 51181 RVA: 0x003A60FC File Offset: 0x003A42FC
		public static bool LovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.Lover, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Fiance, second) || first.relations.DirectRelationExists(PawnRelationDefOf.Spouse, second);
		}

		// Token: 0x0600C7EE RID: 51182 RVA: 0x003A6137 File Offset: 0x003A4337
		public static bool ExLovePartnerRelationExists(Pawn first, Pawn second)
		{
			return first.relations.DirectRelationExists(PawnRelationDefOf.ExSpouse, second) || first.relations.DirectRelationExists(PawnRelationDefOf.ExLover, second);
		}

		// Token: 0x0600C7EF RID: 51183 RVA: 0x003A6160 File Offset: 0x003A4360
		public static void GiveRandomExLoverOrExSpouseRelation(Pawn first, Pawn second)
		{
			PawnRelationDef def;
			if (Rand.Value < 0.5f)
			{
				def = PawnRelationDefOf.ExLover;
			}
			else
			{
				def = PawnRelationDefOf.ExSpouse;
			}
			first.relations.AddDirectRelation(def, second);
		}

		// Token: 0x0600C7F0 RID: 51184 RVA: 0x003A6194 File Offset: 0x003A4394
		public static Pawn GetPartnerInMyBed(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			if (building_Bed == null)
			{
				return null;
			}
			if (building_Bed.SleepingSlotsCount <= 1)
			{
				return null;
			}
			if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn, true))
			{
				return null;
			}
			foreach (Pawn pawn2 in building_Bed.CurOccupants)
			{
				if (pawn2 != pawn && LovePartnerRelationUtility.LovePartnerRelationExists(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x0600C7F1 RID: 51185 RVA: 0x003A6210 File Offset: 0x003A4410
		public static Pawn ExistingLeastLikedPawnWithRelation(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingLeastLikedRel(p, validator);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x0600C7F2 RID: 51186 RVA: 0x003A6230 File Offset: 0x003A4430
		public static DirectPawnRelation ExistingLeastLikedRel(Pawn p, Func<DirectPawnRelation, bool> validator)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MaxValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (validator(directRelations[i]))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 < num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600C7F3 RID: 51187 RVA: 0x003A62B0 File Offset: 0x003A44B0
		public static Pawn ExistingMostLikedLovePartner(Pawn p, bool allowDead)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, allowDead);
			if (directPawnRelation != null)
			{
				return directPawnRelation.otherPawn;
			}
			return null;
		}

		// Token: 0x0600C7F4 RID: 51188 RVA: 0x003A62D0 File Offset: 0x003A44D0
		public static DirectPawnRelation ExistingMostLikedLovePartnerRel(Pawn p, bool allowDead)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if ((allowDead || !directRelations[i].otherPawn.Dead) && LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def))
				{
					int num2 = p.relations.OpinionOf(directRelations[i].otherPawn);
					if (directPawnRelation == null || num2 > num)
					{
						directPawnRelation = directRelations[i];
						num = num2;
					}
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600C7F5 RID: 51189 RVA: 0x003A6368 File Offset: 0x003A4568
		public static HistoryEventDef GetHistoryEventLoveRelationCount(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false, false).Count;
			if (count <= 1)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count <= 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count <= 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count <= 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x0600C7F6 RID: 51190 RVA: 0x003A63B0 File Offset: 0x003A45B0
		public static HistoryEventDef GetHistoryEventForLoveRelationCountPlusOne(this Pawn pawn)
		{
			int count = pawn.GetLoveRelations(false, false).Count;
			if (count == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (count < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (count < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (count < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x0600C7F7 RID: 51191 RVA: 0x003A63F8 File Offset: 0x003A45F8
		public static float GetLovinMtbHours(Pawn pawn, Pawn partner)
		{
			if (pawn.Dead || partner.Dead)
			{
				return -1f;
			}
			if (DebugSettings.alwaysDoLovin)
			{
				return 0.1f;
			}
			if (pawn.needs.food.Starving || partner.needs.food.Starving)
			{
				return -1f;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
			{
				return -1f;
			}
			if (pawn.health.hediffSet.InLabor(true) || partner.health.hediffSet.InLabor(true))
			{
				return -1f;
			}
			float num = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(pawn);
			if (num <= 0f)
			{
				return -1f;
			}
			float num2 = LovePartnerRelationUtility.LovinMtbSinglePawnFactor(partner);
			if (num2 <= 0f)
			{
				return -1f;
			}
			float num3 = 12f;
			num3 *= num;
			num3 *= num2;
			num3 /= Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f);
			num3 /= Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f);
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)pawn.relations.OpinionOf(partner));
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, (float)partner.relations.OpinionOf(pawn));
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicLove, false))
			{
				num3 /= 4f;
			}
			return num3;
		}

		// Token: 0x0600C7F8 RID: 51192 RVA: 0x003A6590 File Offset: 0x003A4790
		private static float LovinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			return num / GenMath.FlatHill(0f, 14f, 16f, 25f, 80f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x0600C7F9 RID: 51193 RVA: 0x003A6616 File Offset: 0x003A4816
		public static void TryToShareBed(Pawn first, Pawn second)
		{
			if (LovePartnerRelationUtility.TryToShareBed_Int(first, second))
			{
				return;
			}
			LovePartnerRelationUtility.TryToShareBed_Int(second, first);
		}

		// Token: 0x0600C7FA RID: 51194 RVA: 0x003A662C File Offset: 0x003A482C
		private static bool TryToShareBed_Int(Pawn bedOwner, Pawn otherPawn)
		{
			Building_Bed ownedBed = bedOwner.ownership.OwnedBed;
			if (ownedBed != null && ownedBed.AnyUnownedSleepingSlot && BedUtility.WillingToShareBed(bedOwner, otherPawn))
			{
				otherPawn.ownership.ClaimBedIfNonMedical(ownedBed);
				return true;
			}
			return false;
		}

		// Token: 0x0600C7FB RID: 51195 RVA: 0x003A666C File Offset: 0x003A486C
		public static float LovePartnerRelationGenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request, bool ex)
		{
			if (generated.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.ageTracker.AgeBiologicalYearsFloat < 14f)
			{
				return 0f;
			}
			if (other.story == null)
			{
				return 0f;
			}
			if (generated.gender == other.gender && (!other.story.traits.HasTrait(TraitDefOf.Gay) || !request.AllowGay))
			{
				return 0f;
			}
			if (generated.gender != other.gender && other.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return 0f;
			}
			if (ModsConfig.BiotechActive)
			{
				if (((generated != null) ? generated.records : null) != null && generated.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
				if (((other != null) ? other.records : null) != null && other.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
			}
			float num = 1f;
			if (ex)
			{
				int num2 = 0;
				List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (LovePartnerRelationUtility.IsExLovePartnerRelation(directRelations[i].def))
					{
						num2++;
					}
				}
				num = Mathf.Pow(0.2f, (float)num2);
			}
			else if (LovePartnerRelationUtility.HasAnyLovePartner(other, true))
			{
				return 0f;
			}
			float num3 = (generated.gender == other.gender) ? 0.01f : 1f;
			float generationChanceAgeFactor = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(generated);
			float generationChanceAgeFactor2 = LovePartnerRelationUtility.GetGenerationChanceAgeFactor(other);
			float generationChanceAgeGapFactor = LovePartnerRelationUtility.GetGenerationChanceAgeGapFactor(generated, other, ex);
			float num4 = 1f;
			if (generated.GetRelations(other).Any((PawnRelationDef x) => x.familyByBloodRelation))
			{
				num4 = 0.01f;
			}
			return num * generationChanceAgeFactor * generationChanceAgeFactor2 * generationChanceAgeGapFactor * num3 * num4;
		}

		// Token: 0x0600C7FC RID: 51196 RVA: 0x003A6857 File Offset: 0x003A4A57
		private static float GetGenerationChanceAgeFactor(Pawn p)
		{
			return Mathf.Clamp(GenMath.LerpDouble(14f, 27f, 0f, 1f, p.ageTracker.AgeBiologicalYearsFloat), 0f, 1f);
		}

		// Token: 0x0600C7FD RID: 51197 RVA: 0x003A688C File Offset: 0x003A4A8C
		private static float GetGenerationChanceAgeGapFactor(Pawn p1, Pawn p2, bool ex)
		{
			float num = Mathf.Abs(p1.ageTracker.AgeBiologicalYearsFloat - p2.ageTracker.AgeBiologicalYearsFloat);
			if (ex)
			{
				float num2 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p1, p2);
				if (num2 >= 0f)
				{
					num = Mathf.Min(num, num2);
				}
				float num3 = LovePartnerRelationUtility.MinPossibleAgeGapAtMinAgeToGenerateAsLovers(p2, p1);
				if (num3 >= 0f)
				{
					num = Mathf.Min(num, num3);
				}
			}
			if (num > 40f)
			{
				return 0f;
			}
			return Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 1f, 0.001f, num), 0.001f, 1f);
		}

		// Token: 0x0600C7FE RID: 51198 RVA: 0x003A6920 File Offset: 0x003A4B20
		private static float MinPossibleAgeGapAtMinAgeToGenerateAsLovers(Pawn p1, Pawn p2)
		{
			float num = p1.ageTracker.AgeChronologicalYearsFloat - 14f;
			if (num < 0f)
			{
				return 0f;
			}
			float num2 = PawnRelationUtility.MaxPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, p2.ageTracker.AgeChronologicalYearsFloat, num);
			float num3 = PawnRelationUtility.MinPossibleBioAgeAt(p2.ageTracker.AgeBiologicalYearsFloat, num);
			if (num2 < 0f)
			{
				return -1f;
			}
			if (num2 < 14f)
			{
				return -1f;
			}
			if (num3 <= 14f)
			{
				return 0f;
			}
			return num3 - 14f;
		}

		// Token: 0x0600C7FF RID: 51199 RVA: 0x003A69B0 File Offset: 0x003A4BB0
		public static void TryToShareChildrenForGeneratedLovePartner(Pawn generated, Pawn other, PawnGenerationRequest request, float extraChanceFactor)
		{
			if (generated.gender == other.gender)
			{
				return;
			}
			List<Pawn> list = other.relations.Children.ToList<Pawn>();
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				float num = 1f;
				if (generated.gender == Gender.Male)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, generated, other, null, new PawnGenerationRequest?(request), null);
				}
				else if (generated.gender == Gender.Female)
				{
					num = ChildRelationUtility.ChanceOfBecomingChildOf(pawn, other, generated, null, null, new PawnGenerationRequest?(request));
				}
				num *= extraChanceFactor;
				if (Rand.Value < num)
				{
					if (generated.gender == Gender.Male)
					{
						pawn.SetFather(generated);
					}
					else if (generated.gender == Gender.Female)
					{
						pawn.SetMother(generated);
					}
				}
			}
		}

		// Token: 0x0600C800 RID: 51200 RVA: 0x003A6A8C File Offset: 0x003A4C8C
		public static void ChangeSpouseRelationsToExSpouse(Pawn pawn)
		{
			List<Pawn> spouses = pawn.GetSpouses(true);
			for (int i = spouses.Count - 1; i >= 0; i--)
			{
				HistoryEvent ev = new HistoryEvent(pawn.GetHistoryEventForSpouseCountPlusOne(), pawn.Named(HistoryEventArgsNames.Doer));
				if (spouses[i].Dead || !ev.DoerWillingToDo())
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, spouses[i]);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, spouses[i]);
				}
			}
		}

		// Token: 0x0600C801 RID: 51201 RVA: 0x003A6B10 File Offset: 0x003A4D10
		public static Pawn GetMostDislikedNonPartnerBedOwner(Pawn p)
		{
			Building_Bed ownedBed = p.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return null;
			}
			Pawn pawn = null;
			int num = 0;
			for (int i = 0; i < ownedBed.OwnersForReading.Count; i++)
			{
				if (ownedBed.OwnersForReading[i] != p && !LovePartnerRelationUtility.LovePartnerRelationExists(p, ownedBed.OwnersForReading[i]))
				{
					int num2 = p.relations.OpinionOf(ownedBed.OwnersForReading[i]);
					if (pawn == null || num2 < num)
					{
						pawn = ownedBed.OwnersForReading[i];
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600C802 RID: 51202 RVA: 0x003A6BA0 File Offset: 0x003A4DA0
		public static float IncestOpinionOffsetFor(Pawn other, Pawn pawn)
		{
			float num = 0f;
			List<DirectPawnRelation> directRelations = other.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && directRelations[i].otherPawn != pawn && !directRelations[i].otherPawn.Dead)
				{
					foreach (PawnRelationDef pawnRelationDef in other.GetRelations(directRelations[i].otherPawn))
					{
						float incestOpinionOffset = pawnRelationDef.incestOpinionOffset;
						if (incestOpinionOffset < num)
						{
							num = incestOpinionOffset;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600C803 RID: 51203 RVA: 0x003A6C5C File Offset: 0x003A4E5C
		public static bool AreNearEachOther(Pawn p1, Pawn p2)
		{
			return !p1.DestroyedOrNull() && !p2.DestroyedOrNull() && ((p1.MapHeld != null && p1.MapHeld == p2.MapHeld) || (p1.GetCaravan() != null && p1.GetCaravan() == p2.GetCaravan()) || (p1.ParentHolder != null && p1.ParentHolder == p2.ParentHolder));
		}

		// Token: 0x0400885A RID: 34906
		private const float MinAgeToGenerateWithLovePartnerRelation = 14f;

		// Token: 0x0400885B RID: 34907
		private static List<DirectPawnRelation> tmpExistingLovePartners = new List<DirectPawnRelation>();
	}
}
