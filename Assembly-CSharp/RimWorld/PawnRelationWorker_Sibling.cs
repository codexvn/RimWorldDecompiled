using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200242E RID: 9262
	public class PawnRelationWorker_Sibling : PawnRelationWorker
	{
		// Token: 0x0600C86E RID: 51310 RVA: 0x003A8707 File Offset: 0x003A6907
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && me.HasSameMother(other) && me.HasSameFather(other);
		}

		// Token: 0x0600C86F RID: 51311 RVA: 0x003A8724 File Offset: 0x003A6924
		public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (other.GetFather() != null || other.GetMother() != null)
			{
				num = ChildRelationUtility.ChanceOfBecomingChildOf(generated, other.GetFather(), other.GetMother(), new PawnGenerationRequest?(request), null, null);
			}
			if (!ChildRelationUtility.XenotypesCompatible(generated, other))
			{
				return 0f;
			}
			float num2 = Mathf.Abs(generated.ageTracker.AgeChronologicalYearsFloat - other.ageTracker.AgeChronologicalYearsFloat);
			float num3 = 1f;
			if (num2 > 40f)
			{
				num3 = 0.2f;
			}
			else if (num2 > 10f)
			{
				num3 = 0.65f;
			}
			return num * num3 * base.BaseGenerationChanceFactor(generated, other, request);
		}

		// Token: 0x0600C870 RID: 51312 RVA: 0x003A87D0 File Offset: 0x003A69D0
		public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			bool flag = other.GetMother() != null;
			bool flag2 = other.GetFather() != null;
			bool flag3 = Rand.Value < 0.85f;
			if (flag && LovePartnerRelationUtility.HasAnyLovePartner(other.GetMother(), true))
			{
				flag3 = false;
			}
			if (flag2 && LovePartnerRelationUtility.HasAnyLovePartner(other.GetFather(), true))
			{
				flag3 = false;
			}
			if (!flag)
			{
				Pawn newMother = PawnRelationWorker_Sibling.GenerateParent(generated, other, Gender.Female, request, flag3);
				other.SetMother(newMother);
			}
			generated.SetMother(other.GetMother());
			if (!flag2)
			{
				Pawn newFather = PawnRelationWorker_Sibling.GenerateParent(generated, other, Gender.Male, request, flag3);
				other.SetFather(newFather);
			}
			generated.SetFather(other.GetFather());
			if (!flag || !flag2)
			{
				if (other.GetMother().story.traits.HasTrait(TraitDefOf.Gay) || other.GetFather().story.traits.HasTrait(TraitDefOf.Gay))
				{
					other.GetFather().relations.AddDirectRelation(PawnRelationDefOf.ExLover, other.GetMother());
				}
				else if (flag3)
				{
					Pawn mother = other.GetMother();
					Pawn father = other.GetFather();
					NameTriple nameTriple = mother.Name as NameTriple;
					father.relations.AddDirectRelation(PawnRelationDefOf.Spouse, mother);
					if (nameTriple != null)
					{
						PawnGenerationRequest pawnGenerationRequest = default(PawnGenerationRequest);
						SpouseRelationUtility.ResolveNameForSpouseOnGeneration(ref pawnGenerationRequest, mother);
						string b = nameTriple.Last;
						string text = null;
						if (pawnGenerationRequest.FixedLastName != null)
						{
							b = pawnGenerationRequest.FixedLastName;
						}
						if (pawnGenerationRequest.FixedBirthName != null)
						{
							text = pawnGenerationRequest.FixedBirthName;
						}
						if (mother.story != null && (nameTriple.Last != b || mother.story.birthLastName != text))
						{
							mother.story.birthLastName = text;
						}
					}
				}
				else
				{
					LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation(other.GetFather(), other.GetMother());
				}
			}
			PawnRelationWorker_Sibling.ResolveMyName(ref request, generated);
		}

		// Token: 0x0600C871 RID: 51313 RVA: 0x003A89A8 File Offset: 0x003A6BA8
		private static Pawn GenerateParent(Pawn generatedChild, Pawn existingChild, Gender genderToGenerate, PawnGenerationRequest childRequest, bool newlyGeneratedParentsWillBeSpousesIfNotGay)
		{
			float ageChronologicalYearsFloat = generatedChild.ageTracker.AgeChronologicalYearsFloat;
			float ageChronologicalYearsFloat2 = existingChild.ageTracker.AgeChronologicalYearsFloat;
			float num = (genderToGenerate == Gender.Male) ? 14f : 16f;
			float num2 = (genderToGenerate == Gender.Male) ? 50f : 45f;
			float num3 = (genderToGenerate == Gender.Male) ? 30f : 27f;
			float num4 = Mathf.Max(ageChronologicalYearsFloat, ageChronologicalYearsFloat2) + num;
			float maxChronologicalAge = num4 + (num2 - num);
			float midChronologicalAge = num4 + (num3 - num);
			float value;
			float value2;
			string text;
			XenotypeDef xenotypeDef;
			PawnRelationWorker_Sibling.GenerateParentParams(num4, maxChronologicalAge, midChronologicalAge, num, generatedChild, existingChild, childRequest, out value, out value2, out text, out xenotypeDef);
			bool flag = true;
			Faction faction = existingChild.Faction;
			if (faction == null || faction.IsPlayer || !faction.def.humanlikeFaction)
			{
				bool tryMedievalOrBetter = faction != null && faction.def.techLevel >= TechLevel.Medieval;
				if (!Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					faction = Faction.OfAncients;
				}
			}
			PawnKindDef kindDef = existingChild.kindDef;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			bool allowGay = flag;
			float? fixedBiologicalAge = new float?(value);
			float? fixedChronologicalAge = new float?(value2);
			Gender? fixedGender = new Gender?(genderToGenerate);
			string fixedLastName = text;
			XenotypeDef forcedXenotype = xenotypeDef;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction2, context, null, true, true, true, false, false, 1f, false, allowGay, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, fixedGender, fixedLastName, null, null, null, false, false, false, false, null, null, forcedXenotype, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600C872 RID: 51314 RVA: 0x003A8B54 File Offset: 0x003A6D54
		private static void GenerateParentParams(float minChronologicalAge, float maxChronologicalAge, float midChronologicalAge, float minBioAgeToHaveChildren, Pawn generatedChild, Pawn existingChild, PawnGenerationRequest childRequest, out float biologicalAge, out float chronologicalAge, out string lastName, out XenotypeDef xenotype)
		{
			chronologicalAge = Rand.GaussianAsymmetric(midChronologicalAge, (midChronologicalAge - minChronologicalAge) / 2f, (maxChronologicalAge - midChronologicalAge) / 2f);
			chronologicalAge = Mathf.Clamp(chronologicalAge, minChronologicalAge, maxChronologicalAge);
			biologicalAge = Rand.Range(minBioAgeToHaveChildren, Mathf.Min(existingChild.RaceProps.lifeExpectancy, chronologicalAge));
			lastName = null;
			xenotype = null;
			if (ModsConfig.BiotechActive)
			{
				if (existingChild.genes.Xenotype.inheritable)
				{
					xenotype = existingChild.genes.Xenotype;
				}
				else
				{
					xenotype = XenotypeDefOf.Baseliner;
				}
			}
			if (!ChildRelationUtility.DefinitelyHasNotBirthName(existingChild))
			{
				NameTriple nameTriple = existingChild.Name as NameTriple;
				if (nameTriple != null && ChildRelationUtility.ChildWantsNameOfAnyParent(existingChild))
				{
					if (existingChild.GetMother() == null && existingChild.GetFather() == null)
					{
						if (Rand.Value < 0.5f)
						{
							lastName = nameTriple.Last;
							return;
						}
					}
					else
					{
						string last = nameTriple.Last;
						string b = null;
						if (existingChild.GetMother() != null)
						{
							NameTriple nameTriple2 = existingChild.GetMother().Name as NameTriple;
							if (nameTriple2 != null)
							{
								b = nameTriple2.Last;
								goto IL_12B;
							}
						}
						if (existingChild.GetFather() != null)
						{
							NameTriple nameTriple3 = existingChild.GetFather().Name as NameTriple;
							if (nameTriple3 != null)
							{
								b = nameTriple3.Last;
							}
						}
						IL_12B:
						if (last != b)
						{
							lastName = last;
						}
					}
				}
			}
		}

		// Token: 0x0600C873 RID: 51315 RVA: 0x003A8C9C File Offset: 0x003A6E9C
		private static void ResolveMyName(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			if (ChildRelationUtility.ChildWantsNameOfAnyParent(generated))
			{
				if (Rand.Value < 0.5f)
				{
					NameTriple nameTriple = generated.GetFather().Name as NameTriple;
					if (nameTriple != null)
					{
						request.SetFixedLastName(nameTriple.Last);
						return;
					}
				}
				NameTriple nameTriple2 = generated.GetMother().Name as NameTriple;
				if (nameTriple2 != null)
				{
					request.SetFixedLastName(nameTriple2.Last);
				}
			}
		}
	}
}
