using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023F1 RID: 9201
	public static class ChildRelationUtility
	{
		// Token: 0x0600C787 RID: 51079 RVA: 0x003A26F8 File Offset: 0x003A08F8
		public static float ChanceOfBecomingChildOf(Pawn child, Pawn father, Pawn mother, PawnGenerationRequest? childGenerationRequest, PawnGenerationRequest? fatherGenerationRequest, PawnGenerationRequest? motherGenerationRequest)
		{
			if (child.IsDuplicate)
			{
				return 0f;
			}
			if (father != null && father.gender != Gender.Male)
			{
				Log.Warning("Tried to calculate chance for father with gender \"" + father.gender.ToString() + "\".");
				return 0f;
			}
			if (mother != null && mother.gender != Gender.Female)
			{
				Log.Warning("Tried to calculate chance for mother with gender \"" + mother.gender.ToString() + "\".");
				return 0f;
			}
			if (father != null && child.GetFather() != null && child.GetFather() != father)
			{
				return 0f;
			}
			if (mother != null && child.GetMother() != null && child.GetMother() != mother)
			{
				return 0f;
			}
			if (mother != null && father != null && !LovePartnerRelationUtility.LovePartnerRelationExists(mother, father) && !LovePartnerRelationUtility.ExLovePartnerRelationExists(mother, father))
			{
				return 0f;
			}
			if (mother != null && !ChildRelationUtility.XenotypesCompatible(child, mother))
			{
				return 0f;
			}
			if (father != null && !ChildRelationUtility.XenotypesCompatible(child, father))
			{
				return 0f;
			}
			if (ModsConfig.BiotechActive)
			{
				if (((father != null) ? father.records : null) != null && father.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
				if (((mother != null) ? mother.records : null) != null && mother.records.GetValue(RecordDefOf.TimeAsChildInColony) > 0f)
				{
					return 0f;
				}
			}
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			if (father != null && child.GetFather() == null)
			{
				num = ChildRelationUtility.GetParentAgeFactor(father, child, 14f, 30f, 50f);
				if (num == 0f)
				{
					return 0f;
				}
				if (father.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num4 = 0.1f;
				}
			}
			if (mother != null && child.GetMother() == null)
			{
				num2 = ChildRelationUtility.GetParentAgeFactor(mother, child, 16f, 27f, 45f);
				if (num2 == 0f)
				{
					return 0f;
				}
				int num5 = ChildRelationUtility.NumberOfChildrenFemaleWantsEver(mother);
				if (mother.relations.ChildrenCount >= num5)
				{
					return 0f;
				}
				num3 = 1f - (float)mother.relations.ChildrenCount / (float)num5;
				if (mother.story.traits.HasTrait(TraitDefOf.Gay))
				{
					num4 = 0.1f;
				}
			}
			float num6 = 1f;
			if (mother != null)
			{
				Pawn firstDirectRelationPawn = mother.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn != null && firstDirectRelationPawn != father)
				{
					num6 *= 0.15f;
				}
			}
			if (father != null)
			{
				Pawn firstDirectRelationPawn2 = father.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn2 != null && firstDirectRelationPawn2 != mother)
				{
					num6 *= 0.15f;
				}
			}
			return num * num2 * num3 * num6 * num4;
		}

		// Token: 0x0600C788 RID: 51080 RVA: 0x003A29A0 File Offset: 0x003A0BA0
		private static float GetParentAgeFactor(Pawn parent, Pawn child, float minAgeToHaveChildren, float usualAgeToHaveChildren, float maxAgeToHaveChildren)
		{
			float num = PawnRelationUtility.MaxPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, parent.ageTracker.AgeChronologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			float num2 = PawnRelationUtility.MinPossibleBioAgeAt(parent.ageTracker.AgeBiologicalYearsFloat, child.ageTracker.AgeChronologicalYearsFloat);
			if (num <= 0f)
			{
				return 0f;
			}
			if (num2 > num)
			{
				return 0f;
			}
			if (num2 <= usualAgeToHaveChildren && num >= usualAgeToHaveChildren)
			{
				return 1f;
			}
			float ageFactor = ChildRelationUtility.GetAgeFactor(num2, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
			float ageFactor2 = ChildRelationUtility.GetAgeFactor(num, minAgeToHaveChildren, maxAgeToHaveChildren, usualAgeToHaveChildren);
			return Mathf.Max(ageFactor, ageFactor2);
		}

		// Token: 0x0600C789 RID: 51081 RVA: 0x003A2A31 File Offset: 0x003A0C31
		public static bool ChildWantsNameOfAnyParent(Pawn child)
		{
			return Rand.ValueSeeded(child.thingIDNumber ^ 88271612) < 0.99f;
		}

		// Token: 0x0600C78A RID: 51082 RVA: 0x003A2A4B File Offset: 0x003A0C4B
		private static int NumberOfChildrenFemaleWantsEver(Pawn female)
		{
			Rand.PushState();
			Rand.Seed = female.thingIDNumber * 3;
			int result = Rand.RangeInclusive(0, 3);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600C78B RID: 51083 RVA: 0x003A2A6B File Offset: 0x003A0C6B
		private static float GetAgeFactor(float ageAtBirth, float min, float max, float mid)
		{
			return GenMath.GetFactorInInterval(min, mid, max, 1.6f, ageAtBirth);
		}

		// Token: 0x0600C78C RID: 51084 RVA: 0x003A2A7C File Offset: 0x003A0C7C
		public static bool DefinitelyHasNotBirthName(Pawn pawn)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple == null)
			{
				return true;
			}
			List<Pawn> spouses = pawn.GetSpouses(true);
			if (!spouses.Any<Pawn>())
			{
				return false;
			}
			for (int i = 0; i < spouses.Count; i++)
			{
				Pawn pawn2 = spouses[i];
				NameTriple nameTriple2 = pawn2.Name as NameTriple;
				if (nameTriple2 != null)
				{
					string last = nameTriple2.Last;
					if (!(nameTriple.Last != last))
					{
						if (pawn2.GetMother() == null)
						{
							goto IL_89;
						}
						NameTriple nameTriple3 = pawn2.GetMother().Name as NameTriple;
						if (nameTriple3 == null || !(nameTriple3.Last == last))
						{
							goto IL_89;
						}
						return true;
						IL_89:
						if (pawn2.GetFather() != null)
						{
							NameTriple nameTriple4 = pawn2.GetFather().Name as NameTriple;
							if (nameTriple4 != null && nameTriple4.Last == last)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600C78D RID: 51085 RVA: 0x003A2B54 File Offset: 0x003A0D54
		public static bool XenotypesCompatible(Pawn first, Pawn second)
		{
			return !ModsConfig.BiotechActive || (first.genes != null && second.genes != null && ((!first.genes.UniqueXenotype && !second.genes.UniqueXenotype) || GeneUtility.SameHeritableXenotype(first, second)) && (!first.genes.Xenotype.inheritable || first.genes.Xenotype == second.genes.Xenotype) && (!second.genes.Xenotype.inheritable || second.genes.Xenotype == first.genes.Xenotype));
		}

		// Token: 0x0400880A RID: 34826
		public const float MinFemaleAgeToHaveChildren = 16f;

		// Token: 0x0400880B RID: 34827
		public const float MaxFemaleAgeToHaveChildren = 45f;

		// Token: 0x0400880C RID: 34828
		public const float UsualFemaleAgeToHaveChildren = 27f;

		// Token: 0x0400880D RID: 34829
		public const float MinMaleAgeToHaveChildren = 14f;

		// Token: 0x0400880E RID: 34830
		public const float MaxMaleAgeToHaveChildren = 50f;

		// Token: 0x0400880F RID: 34831
		public const float UsualMaleAgeToHaveChildren = 30f;

		// Token: 0x04008810 RID: 34832
		public const float ChanceForChildToHaveNameOfAnyParent = 0.99f;
	}
}
