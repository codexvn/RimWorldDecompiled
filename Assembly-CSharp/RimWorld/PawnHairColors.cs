using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200238C RID: 9100
	public static class PawnHairColors
	{
		// Token: 0x17001DEB RID: 7659
		// (get) Token: 0x0600C536 RID: 50486 RVA: 0x003962A0 File Offset: 0x003944A0
		private static List<GeneDef> HairColorGenes
		{
			get
			{
				if (PawnHairColors.cachedHairColorGenes == null)
				{
					PawnHairColors.cachedHairColorGenes = (from x in DefDatabase<GeneDef>.AllDefs
					where x.hairColorOverride != null
					select x).ToList<GeneDef>();
				}
				return PawnHairColors.cachedHairColorGenes;
			}
		}

		// Token: 0x0600C537 RID: 50487 RVA: 0x003962EC File Offset: 0x003944EC
		public static void ResetStaticData()
		{
			PawnHairColors.cachedHairColorGenes = null;
		}

		// Token: 0x0600C538 RID: 50488 RVA: 0x003962F4 File Offset: 0x003944F4
		public static Color RandomHairColor(Pawn pawn, Color skinColor, int ageYears)
		{
			if (Rand.Value < 0.02f)
			{
				return new Color(Rand.Value, Rand.Value, Rand.Value);
			}
			if (PawnHairColors.HasGreyHair(pawn, ageYears))
			{
				return PawnHairColors.RandomGreyHairColor();
			}
			if (PawnSkinColors.IsDarkSkin(skinColor) || Rand.Value < 0.5f)
			{
				float value = Rand.Value;
				if (value < 0.25f)
				{
					return PawnHairColors.DarkBlack;
				}
				if (value < 0.5f)
				{
					return PawnHairColors.MidBlack;
				}
				if (value < 0.75f)
				{
					return PawnHairColors.DarkReddish;
				}
				return PawnHairColors.DarkSaturatedReddish;
			}
			else
			{
				float value2 = Rand.Value;
				if (value2 < 0.25f)
				{
					return PawnHairColors.DarkBrown;
				}
				if (value2 < 0.5f)
				{
					return PawnHairColors.ReddishBrown;
				}
				if (value2 < 0.75f)
				{
					return PawnHairColors.SandyBlonde;
				}
				return PawnHairColors.Blonde;
			}
		}

		// Token: 0x0600C539 RID: 50489 RVA: 0x003963B0 File Offset: 0x003945B0
		public static bool HasGreyHair(Pawn pawn, int ageYears)
		{
			if (ageYears <= 40)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				using (List<Gene>.Enumerator enumerator = pawn.genes.GenesListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.neverGrayHair)
						{
							return false;
						}
					}
				}
			}
			float num = GenMath.SmootherStep(40f, 75f, (float)ageYears);
			return Rand.Value < num;
		}

		// Token: 0x0600C53A RID: 50490 RVA: 0x00396444 File Offset: 0x00394644
		public static Color RandomGreyHairColor()
		{
			float num = Rand.Range(0.65f, 0.85f);
			return new Color(num, num, num);
		}

		// Token: 0x0600C53B RID: 50491 RVA: 0x0039645C File Offset: 0x0039465C
		public static GeneDef RandomHairColorGeneFor(Pawn pawn)
		{
			return PawnHairColors.RandomHairColorGene(pawn.story.SkinColorBase, ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult);
		}

		// Token: 0x0600C53C RID: 50492 RVA: 0x00396488 File Offset: 0x00394688
		private static GeneDef RandomHairColorGene(Color skinColor, bool cultist = false)
		{
			GeneDef result;
			if (PawnHairColors.HairColorGenes.TryRandomElementByWeight(delegate(GeneDef g)
			{
				float num = g.selectionWeight;
				if (PawnSkinColors.IsDarkSkin(skinColor))
				{
					num *= g.selectionWeightFactorDarkSkin;
				}
				if (cultist)
				{
					num *= g.selectionWeightCultist;
				}
				return num;
			}, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600C53D RID: 50493 RVA: 0x003964C8 File Offset: 0x003946C8
		public static GeneDef ClosestHairColorGene(Color hairColor, Color skinColor)
		{
			foreach (GeneDef geneDef in PawnHairColors.HairColorGenes)
			{
				if (hairColor.IndistinguishableFrom(geneDef.hairColorOverride.Value))
				{
					return geneDef;
				}
			}
			return PawnHairColors.RandomHairColorGene(skinColor, false);
		}

		// Token: 0x04008696 RID: 34454
		public static readonly Color DarkBlack = new Color(0.2f, 0.2f, 0.2f);

		// Token: 0x04008697 RID: 34455
		public static readonly Color MidBlack = new Color(0.31f, 0.28f, 0.26f);

		// Token: 0x04008698 RID: 34456
		public static readonly Color DarkReddish = new Color(0.25f, 0.2f, 0.15f);

		// Token: 0x04008699 RID: 34457
		public static readonly Color DarkSaturatedReddish = new Color(0.3f, 0.2f, 0.1f);

		// Token: 0x0400869A RID: 34458
		public static readonly Color DarkBrown = new Color(0.3529412f, 0.22745098f, 0.1254902f);

		// Token: 0x0400869B RID: 34459
		public static readonly Color ReddishBrown = new Color(0.5176471f, 0.3254902f, 0.18431373f);

		// Token: 0x0400869C RID: 34460
		public static readonly Color SandyBlonde = new Color(0.75686276f, 0.57254905f, 0.33333334f);

		// Token: 0x0400869D RID: 34461
		public static readonly Color Blonde = new Color(0.92941177f, 0.7921569f, 0.6117647f);

		// Token: 0x0400869E RID: 34462
		private static List<GeneDef> cachedHairColorGenes;
	}
}
