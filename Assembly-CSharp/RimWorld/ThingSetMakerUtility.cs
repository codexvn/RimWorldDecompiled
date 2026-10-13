using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B9B RID: 11163
	public static class ThingSetMakerUtility
	{
		// Token: 0x0600F982 RID: 63874 RVA: 0x00489108 File Offset: 0x00487308
		public static void Reset()
		{
			ThingSetMakerUtility.allGeneratableItems.Clear();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (ThingSetMakerUtility.CanGenerate(thingDef))
				{
					ThingSetMakerUtility.allGeneratableItems.Add(thingDef);
				}
			}
			ThingSetMaker_Meteorite.Reset();
		}

		// Token: 0x0600F983 RID: 63875 RVA: 0x00489170 File Offset: 0x00487370
		public static bool CanGenerate(ThingDef thingDef)
		{
			return (thingDef.category == ThingCategory.Item || thingDef.Minifiable) && (thingDef.category != ThingCategory.Item || thingDef.EverHaulable) && !thingDef.isUnfinishedThing && !thingDef.IsCorpse && !thingDef.destroyOnDrop && thingDef.graphicData != null && !typeof(MinifiedThing).IsAssignableFrom(thingDef.thingClass);
		}

		// Token: 0x0600F984 RID: 63876 RVA: 0x004891DC File Offset: 0x004873DC
		public static IEnumerable<ThingDef> GetAllowedThingDefs(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel.GetValueOrDefault();
			IEnumerable<ThingDef> source = parms.filter.AllowedThingDefs;
			if (techLevel != TechLevel.Undefined)
			{
				source = from x in source
				where x.techLevel <= techLevel
				select x;
			}
			RoyalTitleDef highestTitle = null;
			if (parms.makingFaction != null && parms.makingFaction.def.HasRoyalTitles)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					RoyalTitleDef royalTitleDef = (pawn.royalty != null) ? pawn.royalty.GetCurrentTitle(parms.makingFaction) : null;
					if (royalTitleDef != null && (highestTitle == null || royalTitleDef.seniority > highestTitle.seniority))
					{
						highestTitle = royalTitleDef;
					}
				}
			}
			source = source.Where(delegate(ThingDef x)
			{
				if (!x.PlayerAcquirable)
				{
					return false;
				}
				CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
				if (compProperties != null)
				{
					if (parms.makingFaction != null && !compProperties.project.heldByFactionCategoryTags.Contains(parms.makingFaction.def.categoryTag))
					{
						return false;
					}
					if (compProperties.project.IsFinished || compProperties.project.TechprintRequirementMet)
					{
						return false;
					}
				}
				if (parms.makingFaction != null && parms.makingFaction.def.HasRoyalTitles)
				{
					RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(x, parms.makingFaction, 0);
					if (minTitleToUse != null && (highestTitle == null || highestTitle.seniority < minTitleToUse.seniority))
					{
						return false;
					}
				}
				return !ModsConfig.AnomalyActive || x != ThingDefOf.Tome || Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent;
			});
			return source.Where(delegate(ThingDef x)
			{
				if (ThingSetMakerUtility.CanGenerate(x))
				{
					if (parms.maxThingMarketValue != null)
					{
						float baseMarketValue = x.BaseMarketValue;
						float? maxThingMarketValue = parms.maxThingMarketValue;
						if (!(baseMarketValue <= maxThingMarketValue.GetValueOrDefault() & maxThingMarketValue != null))
						{
							return false;
						}
					}
					return parms.validator == null || parms.validator(x);
				}
				return false;
			});
		}

		// Token: 0x0600F985 RID: 63877 RVA: 0x00489314 File Offset: 0x00487514
		public static void AssignQuality(Thing thing, QualityGenerator? qualityGenerator)
		{
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = QualityUtility.GenerateQuality(qualityGenerator.GetValueOrDefault());
				compQuality.SetQuality(q, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
		}

		// Token: 0x0600F986 RID: 63878 RVA: 0x00489348 File Offset: 0x00487548
		public static bool IsDerpAndDisallowed(ThingDef thing, ThingDef stuff, QualityGenerator? qualityGenerator)
		{
			QualityGenerator? qualityGenerator2 = qualityGenerator;
			QualityGenerator qualityGenerator3 = QualityGenerator.Gift;
			if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
			{
				qualityGenerator2 = qualityGenerator;
				qualityGenerator3 = QualityGenerator.Reward;
				if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
				{
					qualityGenerator2 = qualityGenerator;
					qualityGenerator3 = QualityGenerator.Super;
					if (!(qualityGenerator2.GetValueOrDefault() == qualityGenerator3 & qualityGenerator2 != null))
					{
						return false;
					}
				}
			}
			return PawnWeaponGenerator.IsDerpWeapon(thing, stuff) || PawnApparelGenerator.IsDerpApparel(thing, stuff);
		}

		// Token: 0x0600F987 RID: 63879 RVA: 0x004893B4 File Offset: 0x004875B4
		public static float AdjustedBigCategoriesSelectionWeight(ThingDef d, int numMeats, int numLeathers, int numEggs)
		{
			float num = 1f;
			if (d.IsMeat)
			{
				num *= Mathf.Min(5f / (float)numMeats, 1f);
			}
			if (d.IsLeather)
			{
				num *= Mathf.Min(5f / (float)numLeathers, 1f);
			}
			if (d.IsEgg)
			{
				num *= Mathf.Min(5f / (float)numEggs, 1f);
			}
			return num;
		}

		// Token: 0x0600F988 RID: 63880 RVA: 0x00489420 File Offset: 0x00487620
		public static bool PossibleToWeighNoMoreThan(ThingDef t, float maxMass, IEnumerable<ThingDef> allowedStuff)
		{
			if (maxMass == 3.4028235E+38f || t.category == ThingCategory.Pawn)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			if (t.MadeFromStuff)
			{
				foreach (ThingDef stuff in allowedStuff)
				{
					if (t.GetStatValueAbstract(StatDefOf.Mass, stuff) <= maxMass)
					{
						return true;
					}
				}
				return false;
			}
			return t.GetStatValueAbstract(StatDefOf.Mass, null) <= maxMass;
		}

		// Token: 0x0600F989 RID: 63881 RVA: 0x004894B0 File Offset: 0x004876B0
		public static bool TryGetRandomThingWhichCanWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, QualityGenerator? qualityGenerator, out ThingStuffPair thingStuffPair)
		{
			ThingDef thingDef;
			if (!(from x in candidates
			where ThingSetMakerUtility.PossibleToWeighNoMoreThan(x, maxMass, GenStuff.AllowedStuffsFor(x, stuffTechLevel, false))
			select x).TryRandomElement(out thingDef))
			{
				thingStuffPair = default(ThingStuffPair);
				return false;
			}
			ThingDef stuff;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel, false)
				where thingDef.GetStatValueAbstract(StatDefOf.Mass, x) <= maxMass && !ThingSetMakerUtility.IsDerpAndDisallowed(thingDef, x, qualityGenerator)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					thingStuffPair = default(ThingStuffPair);
					return false;
				}
			}
			else
			{
				stuff = null;
			}
			thingStuffPair = new ThingStuffPair(thingDef, stuff, 1f);
			return true;
		}

		// Token: 0x0600F98A RID: 63882 RVA: 0x00489580 File Offset: 0x00487780
		public static bool PossibleToWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, int count)
		{
			if (maxMass == 3.4028235E+38f || count <= 0)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			float num = float.MaxValue;
			foreach (ThingDef thingDef in candidates)
			{
				num = Mathf.Min(num, ThingSetMakerUtility.GetMinMass(thingDef, stuffTechLevel));
			}
			return num <= maxMass * (float)count;
		}

		// Token: 0x0600F98B RID: 63883 RVA: 0x004895F8 File Offset: 0x004877F8
		public static float GetMinMass(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef2 = enumerator.Current;
						if (thingDef2.stuffProps.commonality > 0f)
						{
							num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, thingDef2));
						}
					}
					return num;
				}
			}
			num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, null));
			return num;
		}

		// Token: 0x0600F98C RID: 63884 RVA: 0x00489688 File Offset: 0x00487888
		public static float GetMinMarketValue(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef2 = enumerator.Current;
						if (thingDef2.stuffProps.commonality > 0f)
						{
							num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, thingDef2, QualityCategory.Awful), true));
						}
					}
					return num;
				}
			}
			num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, null, QualityCategory.Awful), true));
			return num;
		}

		// Token: 0x0400A416 RID: 42006
		public static List<ThingDef> allGeneratableItems = new List<ThingDef>();
	}
}
