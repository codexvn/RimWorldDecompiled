using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023A7 RID: 9127
	public static class PawnSkinColors
	{
		// Token: 0x17001E13 RID: 7699
		// (get) Token: 0x0600C5BF RID: 50623 RVA: 0x00399188 File Offset: 0x00397388
		public static List<GeneDef> SkinColorGenesInOrder
		{
			get
			{
				if (PawnSkinColors.skinColorGenes == null)
				{
					PawnSkinColors.skinColorGenes = new List<GeneDef>();
					foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
					{
						if ((geneDef.endogeneCategory == EndogeneCategory.Melanin || geneDef.minMelanin < 0f) && geneDef.skinColorBase != null)
						{
							PawnSkinColors.skinColorGenes.Add(geneDef);
						}
					}
					PawnSkinColors.skinColorGenes.SortBy((GeneDef x) => x.minMelanin);
				}
				return PawnSkinColors.skinColorGenes;
			}
		}

		// Token: 0x0600C5C0 RID: 50624 RVA: 0x00399240 File Offset: 0x00397440
		public static void ResetStaticData()
		{
			PawnSkinColors.skinColorGenes = null;
		}

		// Token: 0x0600C5C1 RID: 50625 RVA: 0x00399248 File Offset: 0x00397448
		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
		}

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x00399293 File Offset: 0x00397493
		public static Color GetSkinColor(float melanin)
		{
			return PawnSkinColors.GetSkinColorGene(melanin).skinColorBase.Value;
		}

		// Token: 0x0600C5C3 RID: 50627 RVA: 0x003992A8 File Offset: 0x003974A8
		public static GeneDef GetSkinColorGene(float melanin)
		{
			List<GeneDef> skinColorGenesInOrder = PawnSkinColors.SkinColorGenesInOrder;
			for (int i = skinColorGenesInOrder.Count - 1; i >= 0; i--)
			{
				if (melanin >= skinColorGenesInOrder[i].minMelanin)
				{
					return skinColorGenesInOrder[i];
				}
			}
			return skinColorGenesInOrder.RandomElement<GeneDef>();
		}

		// Token: 0x0600C5C4 RID: 50628 RVA: 0x003992EC File Offset: 0x003974EC
		public static GeneDef RandomSkinColorGene(Pawn pawn)
		{
			if (pawn.Faction != null)
			{
				return PawnSkinColors.GetSkinColorGene(pawn.Faction.def.melaninRange.RandomInRange);
			}
			return PawnSkinColors.SkinColorGenesInOrder.RandomElementByWeight((GeneDef x) => x.selectionWeight);
		}

		// Token: 0x0600C5C5 RID: 50629 RVA: 0x00399348 File Offset: 0x00397548
		public static int SkinColorIndex(Pawn pawn)
		{
			if (((pawn != null) ? pawn.genes : null) == null)
			{
				return -1;
			}
			GeneDef firstEndogeneByCategory = pawn.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin);
			if (firstEndogeneByCategory != null)
			{
				return PawnSkinColors.SkinColorGenesInOrder.IndexOf(firstEndogeneByCategory);
			}
			return -1;
		}

		// Token: 0x0600C5C6 RID: 50630 RVA: 0x00399384 File Offset: 0x00397584
		public static List<GeneDef> SkinColorsFromParents(Pawn father, Pawn mother)
		{
			PawnSkinColors.tmpSkinColorGenes.Clear();
			if (father == null && mother == null)
			{
				return PawnSkinColors.tmpSkinColorGenes;
			}
			int num = PawnSkinColors.SkinColorIndex(father);
			int num2 = PawnSkinColors.SkinColorIndex(mother);
			if (num >= 0 && num2 < 0)
			{
				PawnSkinColors.tmpSkinColorGenes.Add(father.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin));
			}
			else if (num2 >= 0 && num < 0)
			{
				PawnSkinColors.tmpSkinColorGenes.Add(mother.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin));
			}
			else if (num >= 0 && num2 >= 0)
			{
				int num3 = Mathf.Min(num, num2);
				int num4 = Mathf.Max(num, num2);
				for (int i = num3; i <= num4; i++)
				{
					PawnSkinColors.tmpSkinColorGenes.Add(PawnSkinColors.SkinColorGenesInOrder[i]);
				}
			}
			return PawnSkinColors.tmpSkinColorGenes;
		}

		// Token: 0x040086E5 RID: 34533
		private static List<GeneDef> skinColorGenes;

		// Token: 0x040086E6 RID: 34534
		private static List<GeneDef> tmpSkinColorGenes = new List<GeneDef>();
	}
}
