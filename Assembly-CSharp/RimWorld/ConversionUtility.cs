using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002208 RID: 8712
	public static class ConversionUtility
	{
		// Token: 0x0600BEB5 RID: 48821 RVA: 0x00374AF0 File Offset: 0x00372CF0
		public static TaggedString GetCertaintyReductionFactorsDescription(Pawn pawn)
		{
			TaggedString taggedString = string.Empty;
			if (pawn.Ideo != null)
			{
				float num = 1f;
				foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
				{
					if (precept.def.statFactors != null)
					{
						num *= precept.def.statFactors.GetStatFactorFromList(StatDefOf.CertaintyLossFactor);
					}
				}
				if (num != 1f)
				{
					taggedString = "AbilityIdeoConvertBreakdownIdeoCertaintyReduction".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO")) + ": " + num.ToStringPercent();
				}
				float num2 = Find.Storyteller.difficulty.CertaintyReductionFactor(null, pawn);
				if (num2 != 1f)
				{
					if (!taggedString.NullOrEmpty())
					{
						taggedString += "\n";
					}
					taggedString += " -  " + "Difficulty_LowPopConversionBoost_Label".Translate() + ": " + num2.ToStringPercent();
				}
			}
			return taggedString;
		}

		// Token: 0x0600BEB6 RID: 48822 RVA: 0x00374C28 File Offset: 0x00372E28
		public static float ConversionPowerFactor_MemesVsTraits(Pawn initiator, Pawn recipient, StringBuilder sb = null)
		{
			ConversionUtility.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.sb = sb;
			CS$<>8__locals1.recipient = recipient;
			return Mathf.Max(1f + ConversionUtility.<ConversionPowerFactor_MemesVsTraits>g__OffsetFromIdeo|1_1(initiator, false, ref CS$<>8__locals1) + ConversionUtility.<ConversionPowerFactor_MemesVsTraits>g__OffsetFromIdeo|1_1(CS$<>8__locals1.recipient, true, ref CS$<>8__locals1), -0.4f);
		}

		// Token: 0x0600BEB7 RID: 48823 RVA: 0x00374C70 File Offset: 0x00372E70
		[CompilerGenerated]
		internal static string <ConversionPowerFactor_MemesVsTraits>g__MemeAndTraitDesc|1_0(MemeDef meme, Trait trait, float offset, ref ConversionUtility.<>c__DisplayClass1_0 A_3)
		{
			if (A_3.sb == null)
			{
				return string.Empty;
			}
			return "\n   -  " + "AbilityIdeoConvertBreakdownMemeVsTrait".Translate(meme.label.Named("MEME"), trait.Label.Named("TRAIT")).CapitalizeFirst() + ": " + (1f + offset).ToStringPercent();
		}

		// Token: 0x0600BEB8 RID: 48824 RVA: 0x00374CE8 File Offset: 0x00372EE8
		[CompilerGenerated]
		internal static float <ConversionPowerFactor_MemesVsTraits>g__OffsetFromIdeo|1_1(Pawn pawn, bool invert, ref ConversionUtility.<>c__DisplayClass1_0 A_2)
		{
			Ideo ideo = pawn.Ideo;
			string text = string.Empty;
			float num = 0f;
			if (pawn.Ideo == null)
			{
				return num;
			}
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.agreeableTraits.NullOrEmpty<TraitRequirement>())
				{
					foreach (TraitRequirement traitRequirement in memeDef.agreeableTraits)
					{
						if (traitRequirement.HasTrait(A_2.recipient))
						{
							float num2 = invert ? -0.2f : 0.2f;
							num += num2;
							text += ConversionUtility.<ConversionPowerFactor_MemesVsTraits>g__MemeAndTraitDesc|1_0(memeDef, traitRequirement.GetTrait(A_2.recipient), num2, ref A_2);
						}
					}
				}
				if (!memeDef.disagreeableTraits.NullOrEmpty<TraitRequirement>())
				{
					foreach (TraitRequirement traitRequirement2 in memeDef.disagreeableTraits)
					{
						if (traitRequirement2.HasTrait(A_2.recipient))
						{
							float num3 = invert ? 0.2f : -0.2f;
							num += num3;
							text += ConversionUtility.<ConversionPowerFactor_MemesVsTraits>g__MemeAndTraitDesc|1_0(memeDef, traitRequirement2.GetTrait(A_2.recipient), num3, ref A_2);
						}
					}
				}
			}
			if (A_2.sb != null && !text.NullOrEmpty())
			{
				A_2.sb.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownPawnIdeo".Translate(pawn.Named("PAWN")) + ": " + text);
			}
			return num;
		}
	}
}
