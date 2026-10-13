using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002340 RID: 9024
	public static class PawnApparelGenerator
	{
		// Token: 0x0600C3C6 RID: 50118 RVA: 0x0038D9A3 File Offset: 0x0038BBA3
		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		// Token: 0x0600C3C7 RID: 50119 RVA: 0x0038D9D8 File Offset: 0x0038BBD8
		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeToxicEnvironmentResistanceApparelMaxPrice = (float)(ModsConfig.BiotechActive ? ((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_GasMask, null) * 1.3f)) : 0);
			if (ModsConfig.OdysseyActive)
			{
				float num = (float)((int)StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Vacsuit, null));
				int num2 = (int)StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_VacsuitHelmet, null);
				PawnApparelGenerator.freeVacuumResistanceApparelMaxPrice = (num + (float)num2) * 1.3f;
				return;
			}
			PawnApparelGenerator.freeVacuumResistanceApparelMaxPrice = 0f;
		}

		// Token: 0x0600C3C8 RID: 50120 RVA: 0x0038DAD4 File Offset: 0x0038BCD4
		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.ToolUser || !pawn.RaceProps.IsFlesh || pawn.RaceProps.IsAnomalyEntity)
			{
				return;
			}
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			Pawn_OutfitTracker outfits = pawn.outfits;
			if (outfits != null)
			{
				OutfitForcedHandler forcedHandler = outfits.forcedHandler;
				if (forcedHandler != null)
				{
					forcedHandler.Reset();
				}
			}
			float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
			float mapTemperature;
			NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
			bool flag = PawnApparelGenerator.NeedVacuumResistance(pawn, request);
			bool flag2 = Rand.Value < pawn.kindDef.apparelAllowHeadgearChance;
			float num = PawnApparelGenerator.ApparelToxicEnvironmentToAddress(pawn, request);
			PawnApparelGenerator.debugSb = null;
			if (DebugViewSettings.logApparelGeneration)
			{
				PawnApparelGenerator.debugSb = new StringBuilder();
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Generating apparel for {0}", pawn));
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Money: {0:F0}", randomInRange));
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Needed warmth: {0}", neededWarmth));
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Needs vacuum resistance: {0}", flag));
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Needed toxic environment resistance: {0}", num));
				PawnApparelGenerator.debugSb.AppendLine(string.Format("Headgear allowed: {0}", flag2));
			}
			int @int = Rand.Int;
			PawnApparelGenerator.tmpApparelCandidates.Clear();
			for (int i = 0; i < PawnApparelGenerator.allApparelPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnApparelGenerator.allApparelPairs[i];
				if (PawnApparelGenerator.CanUsePair(thingStuffPair, pawn, randomInRange, flag2, @int))
				{
					PawnApparelGenerator.tmpApparelCandidates.Add(thingStuffPair);
				}
			}
			if (!pawn.IsColonist && Rand.Chance(pawn.kindDef.nakedChance))
			{
				StringBuilder stringBuilder = PawnApparelGenerator.debugSb;
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine("Apparel overridden by nakedChance in PawnKindDef");
				}
				PawnApparelGenerator.workingSet.Reset(pawn);
				PawnApparelGenerator.GenerateSpecificRequiredApparel(pawn, randomInRange, true);
			}
			else if (randomInRange < 0.001f)
			{
				PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
			}
			else
			{
				int num2 = 0;
				for (;;)
				{
					PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
					StringBuilder stringBuilder2 = PawnApparelGenerator.debugSb;
					if (stringBuilder2 != null)
					{
						string str = num2.ToString().PadRight(5);
						string str2 = "Trying: ";
						PawnApparelGenerator.PossibleApparelSet possibleApparelSet = PawnApparelGenerator.workingSet;
						stringBuilder2.Append(str + str2 + ((possibleApparelSet != null) ? possibleApparelSet.ToString() : null));
					}
					if (num2 >= 10 || Rand.Value >= 0.85f || randomInRange >= 9999999f)
					{
						goto IL_371;
					}
					float num3 = Rand.Range(0.45f, 0.8f);
					float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
					if (totalPrice >= randomInRange * num3)
					{
						goto IL_371;
					}
					StringBuilder stringBuilder3 = PawnApparelGenerator.debugSb;
					if (stringBuilder3 != null)
					{
						stringBuilder3.AppendLine(string.Concat(new string[]
						{
							" -- Failed: Spent $",
							totalPrice.ToString("F0"),
							", < ",
							(num3 * 100f).ToString("F0"),
							"% of money."
						}));
					}
					IL_4E5:
					num2++;
					continue;
					IL_371:
					if (num2 < 20 && Rand.Value < 0.97f && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
					{
						StringBuilder stringBuilder4 = PawnApparelGenerator.debugSb;
						if (stringBuilder4 == null)
						{
							goto IL_4E5;
						}
						stringBuilder4.AppendLine(" -- Failed: Does not cover torso.");
						goto IL_4E5;
					}
					else if (num2 < 30 && Rand.Value < 0.8f && PawnApparelGenerator.workingSet.CoatButNoShirt())
					{
						StringBuilder stringBuilder5 = PawnApparelGenerator.debugSb;
						if (stringBuilder5 == null)
						{
							goto IL_4E5;
						}
						stringBuilder5.AppendLine(" -- Failed: Coat but no shirt.");
						goto IL_4E5;
					}
					else
					{
						if (num2 < 50)
						{
							bool mustBeSafe = num2 < 17;
							if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
							{
								StringBuilder stringBuilder6 = PawnApparelGenerator.debugSb;
								if (stringBuilder6 == null)
								{
									goto IL_4E5;
								}
								stringBuilder6.AppendLine(" -- Failed: Wrong warmth.");
								goto IL_4E5;
							}
						}
						if (ModsConfig.BiotechActive && num2 < 10 && !PawnApparelGenerator.workingSet.SatisfiesNeededToxicEnvironmentResistance(num))
						{
							StringBuilder stringBuilder7 = PawnApparelGenerator.debugSb;
							if (stringBuilder7 == null)
							{
								goto IL_4E5;
							}
							stringBuilder7.AppendLine(" -- Failed: Wrong toxic environment resistance.");
							goto IL_4E5;
						}
						else
						{
							if (num2 >= 80 || !PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
							{
								break;
							}
							StringBuilder stringBuilder8 = PawnApparelGenerator.debugSb;
							if (stringBuilder8 == null)
							{
								goto IL_4E5;
							}
							stringBuilder8.AppendLine(" -- Failed: Naked.");
							goto IL_4E5;
						}
					}
				}
				StringBuilder stringBuilder9 = PawnApparelGenerator.debugSb;
				if (stringBuilder9 != null)
				{
					stringBuilder9.Append(" -- Approved! Total price: $" + PawnApparelGenerator.workingSet.TotalPrice.ToString("F0") + ", TotalInsulationCold: " + PawnApparelGenerator.workingSet.TotalInsulationCold.ToString());
				}
			}
			if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, true, mapTemperature))
			{
				PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth, mapTemperature, pawn);
			}
			if (ModsConfig.BiotechActive && !pawn.kindDef.apparelIgnorePollution && num > 0.05f && !PawnApparelGenerator.workingSet.SatisfiesNeededToxicEnvironmentResistance(num))
			{
				PawnApparelGenerator.workingSet.AddFreeToxicEnvironmentResistanceAsNeeded(num, (ThingStuffPair pa) => pa.thing.apparel.CorrectAgeForWearing(pawn) && ((pawn.kindDef.apparelIgnoreSeasons && !request.ForceAddFreeWarmLayerIfNeeded) || PawnApparelGenerator.workingSet.GetReplacedInsulationCold(pa) <= pa.InsulationCold));
			}
			if (flag)
			{
				PawnApparelGenerator.workingSet.AddFreeVacuumResistanceAsNeeded(pawn);
			}
			if (DebugViewSettings.logApparelGeneration && PawnApparelGenerator.debugSb != null && PawnApparelGenerator.debugSb.Length > 0)
			{
				Log.Message(PawnApparelGenerator.debugSb.ToString());
			}
			PawnApparelGenerator.workingSet.GiveToPawn(pawn);
			PawnApparelGenerator.workingSet.Reset(null, null);
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
				if (compBiocodable != null && !compBiocodable.Biocoded && Rand.Chance(request.BiocodeApparelChance))
				{
					compBiocodable.CodeFor(pawn);
				}
			}
		}

		// Token: 0x0600C3C9 RID: 50121 RVA: 0x0038E13C File Offset: 0x0038C33C
		public static void PostProcessApparel(Apparel apparel, Pawn pawn)
		{
			if (pawn.kindDef.apparelColor != Color.white)
			{
				apparel.SetColor(pawn.kindDef.apparelColor, false);
			}
			Ideo ideo = pawn.Ideo;
			ThingStyleDef thingStyleDef = (ideo != null) ? ideo.GetStyleFor(apparel.def) : null;
			if (thingStyleDef != null)
			{
				apparel.SetStyleDef(thingStyleDef);
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def))
					{
						Color color = specificApparelRequirements[i].GetColor();
						if (color != default(Color))
						{
							apparel.SetColor(color, false);
						}
						if (specificApparelRequirements[i].UseRandomStyleDef)
						{
							if (!apparel.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(apparel.def.randomStyleChance))
							{
								apparel.SetStyleDef(apparel.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef);
							}
						}
						else if (specificApparelRequirements[i].StyleDef != null)
						{
							apparel.SetStyleDef(specificApparelRequirements[i].StyleDef);
						}
						if (specificApparelRequirements[i].Locked)
						{
							pawn.apparel.Lock(apparel);
						}
						if (specificApparelRequirements[i].Biocode)
						{
							CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
							if (compBiocodable != null)
							{
								compBiocodable.CodeFor(pawn);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C3CA RID: 50122 RVA: 0x0038E2C4 File Offset: 0x0038C4C4
		public static Apparel GenerateApparelOfDefFor(Pawn pawn, ThingDef apparelDef)
		{
			ThingStuffPair thingStuffPair;
			if (!(from pa in PawnApparelGenerator.allApparelPairs
			where pa.thing == apparelDef && PawnApparelGenerator.CanUseStuff(pawn, pa)
			select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
			{
				if (!(from pa in PawnApparelGenerator.allApparelPairs
				where pa.thing == apparelDef
				select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
				{
					return null;
				}
			}
			return (Apparel)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
		}

		// Token: 0x0600C3CB RID: 50123 RVA: 0x0038E37C File Offset: 0x0038C57C
		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, List<ThingStuffPair> apparelCandidates)
		{
			PawnApparelGenerator.workingSet.Reset(pawn);
			float num = PawnApparelGenerator.GenerateSpecificRequiredApparel(pawn, money, false);
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int l;
				int i;
				for (i = 0; i < reqApparel.Count; i = l + 1)
				{
					if (reqApparel[i].apparel.CorrectAgeForWearing(pawn))
					{
						ThingStuffPair pair;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where pa.thing == reqApparel[i] && PawnApparelGenerator.CanUseStuff(pawn, pa) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)
						select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair))
						{
							PawnApparelGenerator.workingSet.Add(pair);
							num -= pair.Price;
						}
					}
					l = i;
				}
			}
			PawnApparelGenerator.usableApparel.Clear();
			for (int j = 0; j < apparelCandidates.Count; j++)
			{
				if (!PawnApparelGenerator.workingSet.PairOverlapsAnything(apparelCandidates[j]))
				{
					PawnApparelGenerator.usableApparel.Add(apparelCandidates[j]);
				}
			}
			Func<ThingStuffPair, bool> <>9__2;
			while (pawn.Ideo == null || !pawn.Ideo.IdeoPrefersNudityForGender(pawn.gender) || (pawn.Faction != null && pawn.Faction.IsPlayer))
			{
				if (!pawn.IsColonist)
				{
					Pawn_StoryTracker story = pawn.story;
					if (((story != null) ? story.traits : null) != null && pawn.story.traits.HasTrait(TraitDefOf.Nudist))
					{
						break;
					}
				}
				if (Rand.Value < 0.1f && money < 9999999f)
				{
					break;
				}
				IEnumerable<ThingStuffPair> source = PawnApparelGenerator.usableApparel;
				Func<ThingStuffPair, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((ThingStuffPair pa) => PawnApparelGenerator.CanUseStuff(pawn, pa)));
				}
				ThingStuffPair pair2;
				if (!source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair2))
				{
					break;
				}
				PawnApparelGenerator.workingSet.Add(pair2);
				num -= pair2.Price;
				for (int k = PawnApparelGenerator.usableApparel.Count - 1; k >= 0; k--)
				{
					if (PawnApparelGenerator.usableApparel[k].Price > num || PawnApparelGenerator.workingSet.PairOverlapsAnything(PawnApparelGenerator.usableApparel[k]))
					{
						PawnApparelGenerator.usableApparel.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x0600C3CC RID: 50124 RVA: 0x0038E654 File Offset: 0x0038C854
		private static float GenerateSpecificRequiredApparel(Pawn pawn, float moneyLeft, bool onlyGenerateIgnoreNaked)
		{
			List<SpecificApparelRequirement> att = pawn.kindDef.specificApparelRequirements;
			if (att != null)
			{
				int i;
				Func<ThingStuffPair, bool> <>9__0;
				int j;
				for (i = 0; i < att.Count; i = j + 1)
				{
					if (!att[i].RequiredTag.NullOrEmpty() || (!att[i].AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && (!onlyGenerateIgnoreNaked || att[i].IgnoreNaked)))
					{
						IEnumerable<ThingStuffPair> source = PawnApparelGenerator.allApparelPairs;
						Func<ThingStuffPair, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingStuffPair pa) => PawnApparelGenerator.ApparelRequirementTagsMatch(att[i], pa.thing) && PawnApparelGenerator.ApparelRequirementHandlesThing(att[i], pa.thing) && PawnApparelGenerator.CanUseStuff(pawn, pa) && pa.thing.apparel.PawnCanWear(pawn, false) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)));
						}
						ThingStuffPair pair;
						if (source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair))
						{
							PawnApparelGenerator.workingSet.Add(pair);
							moneyLeft -= pair.Price;
						}
					}
					j = i;
				}
			}
			return moneyLeft;
		}

		// Token: 0x0600C3CD RID: 50125 RVA: 0x0038E784 File Offset: 0x0038C984
		private static bool CanUseStuff(Pawn pawn, ThingStuffPair pair)
		{
			if (pair.stuff != null && !pair.stuff.stuffProps.allowedInStuffGeneration)
			{
				return false;
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (!PawnApparelGenerator.ApparelRequirementCanUseStuff(specificApparelRequirements[i], pair))
					{
						return false;
					}
				}
			}
			return pair.stuff == null || pawn.Faction == null || pawn.kindDef.ignoreFactionApparelStuffRequirements || pawn.Faction.def.CanUseStuffForApparel(pair.stuff);
		}

		// Token: 0x0600C3CE RID: 50126 RVA: 0x0038E818 File Offset: 0x0038CA18
		public static bool IsDerpApparel(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (!thing.IsApparel)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < thing.stuffCategories.Count; i++)
			{
				if (thing.stuffCategories[i] != StuffCategoryDefOf.Woody && thing.stuffCategories[i] != StuffCategoryDefOf.Stony)
				{
					flag = true;
					break;
				}
			}
			return flag && (stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Woody) || stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)) && (thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) || thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs));
		}

		// Token: 0x0600C3CF RID: 50127 RVA: 0x0038E8DC File Offset: 0x0038CADC
		public static bool ApparelRequirementHandlesThing(SpecificApparelRequirement req, ThingDef thing)
		{
			return (req.BodyPartGroup == null || thing.apparel.bodyPartGroups.Contains(req.BodyPartGroup)) && (req.ApparelLayer == null || thing.apparel.layers.Contains(req.ApparelLayer)) && (req.ApparelDef == null || thing == req.ApparelDef);
		}

		// Token: 0x0600C3D0 RID: 50128 RVA: 0x0038E944 File Offset: 0x0038CB44
		public static bool ApparelRequirementTagsMatch(SpecificApparelRequirement req, ThingDef thing)
		{
			return (!req.RequiredTag.NullOrEmpty() && thing.apparel.tags.Contains(req.RequiredTag)) || (!req.AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && req.AlternateTagChoices.Any((SpecificApparelRequirement.TagChance x) => thing.apparel.tags.Contains(x.tag) && Rand.Value < x.chance));
		}

		// Token: 0x0600C3D1 RID: 50129 RVA: 0x0038E9B0 File Offset: 0x0038CBB0
		private static bool ApparelRequirementCanUseStuff(SpecificApparelRequirement req, ThingStuffPair pair)
		{
			return req.Stuff == null || !PawnApparelGenerator.ApparelRequirementHandlesThing(req, pair.thing) || (pair.stuff != null && req.Stuff == pair.stuff);
		}

		// Token: 0x0600C3D2 RID: 50130 RVA: 0x0038E9E4 File Offset: 0x0038CBE4
		private static bool CanUsePair(ThingStuffPair pair, Pawn pawn, float moneyLeft, bool allowHeadgear, int fixedSeed)
		{
			if (pair.Price > moneyLeft)
			{
				return false;
			}
			if (!allowHeadgear && PawnApparelGenerator.IsHeadgear(pair.thing))
			{
				return false;
			}
			if (!pair.thing.apparel.PawnCanWear(pawn, false))
			{
				return false;
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				bool flag = false;
				for (int i = 0; i < pawn.kindDef.apparelTags.Count; i++)
				{
					for (int j = 0; j < pair.thing.apparel.tags.Count; j++)
					{
						if (pawn.kindDef.apparelTags[i] == pair.thing.apparel.tags[j])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>())
			{
				for (int k = 0; k < pawn.kindDef.apparelDisallowTags.Count; k++)
				{
					if (pair.thing.apparel.tags.Contains(pawn.kindDef.apparelDisallowTags[k]))
					{
						return false;
					}
				}
			}
			return pawn.kindDef.ignoreApparelAllowChance || pair.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(pair.thing.generateAllowChance, fixedSeed ^ (int)pair.thing.shortHash ^ 64128343);
		}

		// Token: 0x0600C3D3 RID: 50131 RVA: 0x0038EB50 File Offset: 0x0038CD50
		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Eyes);
		}

		// Token: 0x0600C3D4 RID: 50132 RVA: 0x0038EBA4 File Offset: 0x0038CDA4
		private static bool NeedVacuumResistance(Pawn pawn, PawnGenerationRequest request)
		{
			return ModsConfig.OdysseyActive && request.Tile.Valid && request.Tile.LayerDef.isSpace && pawn.ConcernedByVacuum;
		}

		// Token: 0x0600C3D5 RID: 50133 RVA: 0x0038EBEC File Offset: 0x0038CDEC
		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request, out float mapTemperature)
		{
			PlanetTile tile = request.Tile;
			if (!tile.Valid)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (!tile.Valid)
			{
				mapTemperature = 21f;
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
			mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			for (int i = 0; i < 2; i++)
			{
				NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
				if (neededWarmth2 != NeededWarmth.Any)
				{
					neededWarmth = neededWarmth2;
					break;
				}
				twelfth = twelfth.NextTwelfth();
			}
			if (!pawn.kindDef.apparelIgnoreSeasons)
			{
				return neededWarmth;
			}
			if (request.ForceAddFreeWarmLayerIfNeeded && neededWarmth == NeededWarmth.Warm)
			{
				return neededWarmth;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x0600C3D6 RID: 50134 RVA: 0x0038EC88 File Offset: 0x0038CE88
		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, PlanetTile tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4f)
			{
				return NeededWarmth.Warm;
			}
			if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4f)
			{
				return NeededWarmth.Cool;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x0600C3D7 RID: 50135 RVA: 0x0038ECD8 File Offset: 0x0038CED8
		private static float ApparelToxicEnvironmentToAddress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.kindDef.apparelIgnorePollution)
			{
				return 0f;
			}
			PlanetTile tile = request.Tile;
			if (!tile.Valid)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (!tile.Valid)
			{
				return 0f;
			}
			return Mathf.Clamp01(Find.WorldGrid[tile].pollution);
		}

		// Token: 0x0600C3D8 RID: 50136 RVA: 0x0038ED40 File Offset: 0x0038CF40
		[DebugOutput]
		private static void ApparelPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[8];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => (p.Commonality * 100f).ToString("F4"));
			array[4] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"));
			array[5] = new TableDataGetter<ThingStuffPair>("insulationCold", delegate(ThingStuffPair p)
			{
				if (p.InsulationCold != 0f)
				{
					return p.InsulationCold.ToString();
				}
				return "";
			});
			array[6] = new TableDataGetter<ThingStuffPair>("headgear", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsHeadgear(p.thing))
				{
					return "";
				}
				return "*";
			});
			array[7] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsDerpApparel(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x0600C3D9 RID: 50137 RVA: 0x0038EEE1 File Offset: 0x0038D0E1
		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		// Token: 0x0400856C RID: 34156
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		// Token: 0x0400856D RID: 34157
		private static float freeWarmParkaMaxPrice;

		// Token: 0x0400856E RID: 34158
		private static float freeWarmHatMaxPrice;

		// Token: 0x0400856F RID: 34159
		private static float freeVacuumResistanceApparelMaxPrice;

		// Token: 0x04008570 RID: 34160
		private static float freeToxicEnvironmentResistanceApparelMaxPrice;

		// Token: 0x04008571 RID: 34161
		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		// Token: 0x04008572 RID: 34162
		private static StringBuilder debugSb = null;

		// Token: 0x04008573 RID: 34163
		private const int PracticallyInfinity = 9999999;

		// Token: 0x04008574 RID: 34164
		private const float MinMapPollutionForFreeToxicResistanceApparel = 0.05f;

		// Token: 0x04008575 RID: 34165
		private static List<ThingStuffPair> tmpApparelCandidates = new List<ThingStuffPair>();

		// Token: 0x04008576 RID: 34166
		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		// Token: 0x02002341 RID: 9025
		private class PossibleApparelSet
		{
			// Token: 0x17001DDB RID: 7643
			// (get) Token: 0x0600C3DA RID: 50138 RVA: 0x0038EEED File Offset: 0x0038D0ED
			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			// Token: 0x17001DDC RID: 7644
			// (get) Token: 0x0600C3DB RID: 50139 RVA: 0x0038EEFA File Offset: 0x0038D0FA
			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			// Token: 0x17001DDD RID: 7645
			// (get) Token: 0x0600C3DC RID: 50140 RVA: 0x0038EF26 File Offset: 0x0038D126
			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			// Token: 0x17001DDE RID: 7646
			// (get) Token: 0x0600C3DD RID: 50141 RVA: 0x0038EF52 File Offset: 0x0038D152
			public List<ThingStuffPair> ApparelsForReading
			{
				get
				{
					return this.aps;
				}
			}

			// Token: 0x0600C3DE RID: 50142 RVA: 0x0038EF5A File Offset: 0x0038D15A
			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
				this.pawn = null;
			}

			// Token: 0x0600C3DF RID: 50143 RVA: 0x0038EF88 File Offset: 0x0038D188
			public void Reset(Pawn pawn)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.pawn = pawn;
				BodyDef bodyDef;
				if (pawn == null)
				{
					bodyDef = null;
				}
				else
				{
					RaceProperties raceProps = pawn.RaceProps;
					bodyDef = ((raceProps != null) ? raceProps.body : null);
				}
				this.body = bodyDef;
				this.raceDef = ((pawn != null) ? pawn.def : null);
			}

			// Token: 0x0600C3E0 RID: 50144 RVA: 0x0038EFE4 File Offset: 0x0038D1E4
			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						this.lgps.Add(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
					}
				}
			}

			// Token: 0x0600C3E1 RID: 50145 RVA: 0x0038F070 File Offset: 0x0038D270
			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				if (!this.lgps.Any<ApparelUtility.LayerGroupPair>())
				{
					return false;
				}
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						if (this.lgps.Contains(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j])))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x0600C3E2 RID: 50146 RVA: 0x0038F104 File Offset: 0x0038D304
			public bool CoatButNoShirt()
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						for (int j = 0; j < this.aps[i].thing.apparel.layers.Count; j++)
						{
							ApparelLayerDef apparelLayerDef = this.aps[i].thing.apparel.layers[j];
							if (apparelLayerDef == ApparelLayerDefOf.OnSkin)
							{
								flag2 = true;
							}
							if (apparelLayerDef == ApparelLayerDefOf.Shell || apparelLayerDef == ApparelLayerDefOf.Middle)
							{
								flag = true;
							}
						}
					}
				}
				return flag && !flag2;
			}

			// Token: 0x0600C3E3 RID: 50147 RVA: 0x0038F1D0 File Offset: 0x0038D3D0
			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if ((bp != BodyPartGroupDefOf.Legs || !this.aps[i].thing.apparel.legsNakedUnlessCoveredBySomethingElse) && this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600C3E4 RID: 50148 RVA: 0x0038F240 File Offset: 0x0038D440
			public bool IsNaked(Gender gender)
			{
				switch (gender)
				{
				case Gender.None:
					return false;
				case Gender.Male:
					return !this.Covers(BodyPartGroupDefOf.Legs);
				case Gender.Female:
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				default:
					return false;
				}
			}

			// Token: 0x0600C3E5 RID: 50149 RVA: 0x0038F294 File Offset: 0x0038D494
			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				if (warmth == NeededWarmth.Any)
				{
					return true;
				}
				if (mustBeSafe && !this.SafeTemperature(mapTemperature))
				{
					return false;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationHeat) >= -2f;
				}
				if (warmth == NeededWarmth.Warm)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold) >= 52f;
				}
				throw new NotImplementedException();
			}

			// Token: 0x0600C3E6 RID: 50150 RVA: 0x0038F32C File Offset: 0x0038D52C
			private bool SafeTemperature(float temp)
			{
				if (this.pawn != null)
				{
					return this.pawn.SafeTemperatureRange(this.aps).Includes(temp);
				}
				return GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(temp);
			}

			// Token: 0x0600C3E7 RID: 50151 RVA: 0x0038F378 File Offset: 0x0038D578
			public void AddFreeWarmthAsNeeded(NeededWarmth warmth, float mapTemperature, Pawn pawn)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0();
				CS$<>8__locals1.pawn = pawn;
				CS$<>8__locals1.<>4__this = this;
				if (warmth == NeededWarmth.Any)
				{
					return;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
				}
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0 CS$<>8__locals2 = CS$<>8__locals1;
				Faction homeFaction = CS$<>8__locals1.pawn.HomeFaction;
				CS$<>8__locals2.homeFaction = ((homeFaction != null) ? homeFaction.def : null);
				for (int i = 0; i < 3; i++)
				{
					if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Format("Checking to give free torso-cover at max price {0}", PawnApparelGenerator.freeWarmParkaMaxPrice));
						}
						int j = 0;
						while (j < 2)
						{
							ThingStuffPair candidate;
							if (j == 0)
							{
								IEnumerable<ThingStuffPair> allApparelPairs = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate;
								if ((predicate = CS$<>8__locals1.<>9__1) == null)
								{
									predicate = (CS$<>8__locals1.<>9__1 = ((ThingStuffPair pa) => base.<AddFreeWarmthAsNeeded>g__ParkaPairValidator|0(pa) && pa.InsulationCold < 40f));
								}
								if (allApparelPairs.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out candidate))
								{
									goto IL_174;
								}
							}
							else
							{
								IEnumerable<ThingStuffPair> source = PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeWarmthAsNeeded>g__ParkaPairValidator|0));
								Func<ThingStuffPair, float> selector;
								if ((selector = CS$<>8__locals1.<>9__3) == null)
								{
									selector = (CS$<>8__locals1.<>9__3 = ((ThingStuffPair x) => x.InsulationCold - CS$<>8__locals1.<>4__this.GetReplacedInsulationCold(x)));
								}
								if (source.TryMaxBy(selector, out candidate))
								{
									goto IL_174;
								}
							}
							j++;
							continue;
							IL_174:
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Format("Giving free torso-cover: {0} insulation={1}", candidate, candidate.InsulationCold));
								IEnumerable<ThingStuffPair> source2 = this.aps;
								Func<ThingStuffPair, bool> predicate2;
								Func<ThingStuffPair, bool> <>9__5;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, candidate.thing, CS$<>8__locals1.<>4__this.body)));
								}
								foreach (ThingStuffPair thingStuffPair in source2.Where(predicate2))
								{
									PawnApparelGenerator.debugSb.AppendLine(string.Format("    -replaces {0} InsulationCold={1}", thingStuffPair, thingStuffPair.InsulationCold));
								}
							}
							this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, candidate.thing, CS$<>8__locals1.<>4__this.body));
							this.aps.Add(candidate);
							break;
						}
					}
					if (this.SafeTemperature(mapTemperature))
					{
						break;
					}
				}
				if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine(string.Format("Checking to give free hat at max price {0}", PawnApparelGenerator.freeWarmHatMaxPrice));
					}
					ThingStuffPair hatPair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeWarmthAsNeeded>g__HatPairValidator|6)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out hatPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Format("Giving free hat: {0} insulation={1}", hatPair, hatPair.InsulationCold));
							IEnumerable<ThingStuffPair> source3 = this.aps;
							Func<ThingStuffPair, bool> predicate3;
							Func<ThingStuffPair, bool> <>9__9;
							if ((predicate3 = <>9__9) == null)
							{
								predicate3 = (<>9__9 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair thingStuffPair2 in source3.Where(predicate3))
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Format("    -replaces {0} InsulationCold={1}", thingStuffPair2, thingStuffPair2.InsulationCold));
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing, CS$<>8__locals1.<>4__this.body));
						this.aps.Add(hatPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine(string.Format("New TotalInsulationCold: {0}", this.TotalInsulationCold));
				}
			}

			// Token: 0x0600C3E8 RID: 50152 RVA: 0x0038F7B4 File Offset: 0x0038D9B4
			public bool SatisfiesNeededToxicEnvironmentResistance(float pollution)
			{
				if (pollution <= 0f)
				{
					return true;
				}
				return this.aps.Sum((ThingStuffPair ap) => ap.ToxicEnvironmentResistance) >= PawnApparelGenerator.PossibleApparelSet.ToxicEnvironmentResistanceOverPollutionCurve.Evaluate(pollution);
			}

			// Token: 0x0600C3E9 RID: 50153 RVA: 0x0038F808 File Offset: 0x0038DA08
			public void AddFreeVacuumResistanceAsNeeded(Pawn pawn)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass34_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass34_0();
				CS$<>8__locals1.pawn = pawn;
				CS$<>8__locals1.<>4__this = this;
				if (!ModsConfig.OdysseyActive)
				{
					return;
				}
				CS$<>8__locals1.desired = 0.9f - CS$<>8__locals1.pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1);
				if (CS$<>8__locals1.desired <= 0f)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free vacuum resistance.");
				}
				int num = 0;
				while (num < 10 && !CS$<>8__locals1.<AddFreeVacuumResistanceAsNeeded>g__Protected|1())
				{
					ThingStuffPair pair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeVacuumResistanceAsNeeded>g__VacuumValidator|0)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out pair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Format("Giving free vacuum resistance: {0} VacuumResistance={1}", pair, pair.VacuumResistance));
							IEnumerable<ThingStuffPair> source = this.aps;
							Func<ThingStuffPair, bool> predicate;
							Func<ThingStuffPair, bool> <>9__4;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, pair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair thingStuffPair in source.Where(predicate))
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Format("    -replaces {0} VacuumResistance={1}", thingStuffPair, thingStuffPair.VacuumResistance));
							}
						}
						for (int i = this.aps.Count - 1; i >= 0; i--)
						{
							ThingStuffPair thingStuffPair2 = this.aps[i];
							if (!ApparelUtility.CanWearTogether(thingStuffPair2.thing, pair.thing, this.body))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(string.Format("    -replaces {0} VacuumResistance={1}", thingStuffPair2, thingStuffPair2.VacuumResistance));
								}
								CS$<>8__locals1.desired += thingStuffPair2.VacuumResistance;
								this.aps.RemoveAt(i);
							}
						}
						this.aps.Add(pair);
						CS$<>8__locals1.desired -= pair.VacuumResistance;
					}
					num++;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine(string.Format("New VacuumResistance: {0}", this.aps.Sum((ThingStuffPair a) => a.VacuumResistance)));
				}
			}

			// Token: 0x0600C3EA RID: 50154 RVA: 0x0038FADC File Offset: 0x0038DCDC
			private bool CoversUnprotectedVacuumPart(ThingDef thing)
			{
				foreach (BodyPartRecord bodyPartRecord in this.GetUnprotectedVacuumParts())
				{
					for (int i = 0; i < bodyPartRecord.groups.Count; i++)
					{
						BodyPartGroupDef bodyPartGroupDef = bodyPartRecord.groups[i];
						for (int j = 0; j < thing.apparel.bodyPartGroups.Count; j++)
						{
							BodyPartGroupDef bodyPartGroupDef2 = thing.apparel.bodyPartGroups[j];
							if (bodyPartGroupDef == bodyPartGroupDef2)
							{
								return true;
							}
						}
					}
				}
				return false;
			}

			// Token: 0x0600C3EB RID: 50155 RVA: 0x0038FB8C File Offset: 0x0038DD8C
			private List<BodyPartRecord> GetUnprotectedVacuumParts()
			{
				PawnApparelGenerator.PossibleApparelSet.tmpParts.AddRange(this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Outside, null, null));
				for (int i = PawnApparelGenerator.PossibleApparelSet.tmpParts.Count - 1; i >= 0; i--)
				{
					BodyPartRecord bodyPartRecord = PawnApparelGenerator.PossibleApparelSet.tmpParts[i];
					if (!bodyPartRecord.def.canBeVacuumBurnt || !bodyPartRecord.def.IsSkinCovered(bodyPartRecord, this.pawn.health.hediffSet))
					{
						PawnApparelGenerator.PossibleApparelSet.tmpParts.RemoveAt(i);
					}
					else
					{
						for (int j = 0; j < this.aps.Count; j++)
						{
							ThingStuffPair thingStuffPair = this.aps[j];
							if (thingStuffPair.VacuumResistance >= 0.1f)
							{
								for (int k = 0; k < bodyPartRecord.groups.Count; k++)
								{
									BodyPartGroupDef bodyPartGroupDef = bodyPartRecord.groups[k];
									for (int l = 0; l < thingStuffPair.thing.apparel.bodyPartGroups.Count; l++)
									{
										BodyPartGroupDef bodyPartGroupDef2 = thingStuffPair.thing.apparel.bodyPartGroups[l];
										if (bodyPartGroupDef == bodyPartGroupDef2)
										{
											PawnApparelGenerator.PossibleApparelSet.tmpParts.RemoveAt(i);
											goto IL_12C;
										}
									}
								}
							}
						}
					}
					IL_12C:;
				}
				return PawnApparelGenerator.PossibleApparelSet.tmpParts;
			}

			// Token: 0x0600C3EC RID: 50156 RVA: 0x0038FCD8 File Offset: 0x0038DED8
			public void AddFreeToxicEnvironmentResistanceAsNeeded(float pollution, Func<ThingStuffPair, bool> extraValidator = null)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass38_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass38_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.extraValidator = extraValidator;
				for (int i = 0; i < 5; i++)
				{
					if (this.SatisfiesNeededToxicEnvironmentResistance(pollution))
					{
						break;
					}
					ThingStuffPair pollutionPair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeToxicEnvironmentResistanceAsNeeded>g__PollutionApparelValidator|0)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out pollutionPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Format("Giving free toxic environment resistance: {0} ToxicEnvironmentResistance={1}", pollutionPair, pollutionPair.ToxicEnvironmentResistance));
							IEnumerable<ThingStuffPair> source = this.aps;
							Func<ThingStuffPair, bool> predicate;
							Func<ThingStuffPair, bool> <>9__4;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, pollutionPair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair thingStuffPair in source.Where(predicate))
							{
								PawnApparelGenerator.debugSb.AppendLine(string.Format("    -replaces {0} ToxicEnvironmentResistance={1}", thingStuffPair, thingStuffPair.ToxicEnvironmentResistance));
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, pollutionPair.thing, CS$<>8__locals1.<>4__this.body));
						this.aps.Add(pollutionPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine(string.Format("New ToxicEnvironmentResistance: {0}", this.aps.Sum((ThingStuffPair a) => a.ToxicEnvironmentResistance)));
				}
			}

			// Token: 0x0600C3ED RID: 50157 RVA: 0x0038FEA4 File Offset: 0x0038E0A4
			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					Apparel apparel = (Apparel)ThingMaker.MakeThing(this.aps[i].thing, this.aps[i].stuff);
					PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
					if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						pawn.apparel.Wear(apparel, false, false);
					}
				}
				for (int j = 0; j < this.aps.Count; j++)
				{
					for (int k = 0; k < this.aps.Count; k++)
					{
						if (j != k && !ApparelUtility.CanWearTogether(this.aps[j].thing, this.aps[k].thing, pawn.RaceProps.body))
						{
							Log.Error(string.Format("{0} generated with apparel that cannot be worn together: {1}, {2}", pawn, this.aps[j], this.aps[k]));
							return;
						}
					}
				}
			}

			// Token: 0x0600C3EE RID: 50158 RVA: 0x0038FFB4 File Offset: 0x0038E1B4
			public float GetReplacedInsulationCold(ThingStuffPair newAp)
			{
				float num = 0f;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(this.aps[i].thing, newAp.thing, this.body))
					{
						num += this.aps[i].InsulationCold;
					}
				}
				return num;
			}

			// Token: 0x0600C3EF RID: 50159 RVA: 0x00390019 File Offset: 0x0038E219
			private bool CorrectFactionForApparel(FactionDef faction, ThingDef apparel)
			{
				if (faction != null)
				{
					if (apparel.apparel.anyTechLevelCanUseForWarmth)
					{
						return true;
					}
					if (faction.techLevel >= TechLevel.Industrial && apparel.techLevel == TechLevel.Neolithic)
					{
						return false;
					}
					if (faction.techLevel == TechLevel.Neolithic && apparel.techLevel >= TechLevel.Industrial)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0600C3F0 RID: 50160 RVA: 0x00390058 File Offset: 0x0038E258
			public override string ToString()
			{
				string str = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					str += string.Format("{0}, ", this.aps[i]);
				}
				return str + "]";
			}

			// Token: 0x04008577 RID: 34167
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			// Token: 0x04008578 RID: 34168
			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			// Token: 0x04008579 RID: 34169
			private BodyDef body;

			// Token: 0x0400857A RID: 34170
			private ThingDef raceDef;

			// Token: 0x0400857B RID: 34171
			private Pawn pawn;

			// Token: 0x0400857C RID: 34172
			private const float StartingMinTemperature = 12f;

			// Token: 0x0400857D RID: 34173
			private const float TargetMinTemperature = -40f;

			// Token: 0x0400857E RID: 34174
			private const float StartingMaxTemperature = 32f;

			// Token: 0x0400857F RID: 34175
			private const float TargetMaxTemperature = 30f;

			// Token: 0x04008580 RID: 34176
			private const float MinToxicEnvironmentResistanceForFreeApparel = 0.25f;

			// Token: 0x04008581 RID: 34177
			private const float MinVacuumResistanceForFreeApparel = 0.9f;

			// Token: 0x04008582 RID: 34178
			private const float MinToxicEnvironmentResistanceImprovement = 0.15f;

			// Token: 0x04008583 RID: 34179
			private const float MinVacuumResistanceImprovement = 0.15f;

			// Token: 0x04008584 RID: 34180
			private static readonly SimpleCurve ToxicEnvironmentResistanceOverPollutionCurve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(0.5f, 0.5f),
					true
				},
				{
					new CurvePoint(1f, 0.85f),
					true
				}
			};

			// Token: 0x04008585 RID: 34181
			private const float DesiredVacuumResistance = 0.9f;

			// Token: 0x04008586 RID: 34182
			private static readonly List<BodyPartRecord> tmpParts = new List<BodyPartRecord>();
		}
	}
}
