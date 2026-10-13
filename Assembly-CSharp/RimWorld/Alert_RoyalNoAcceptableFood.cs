using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003013 RID: 12307
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		// Token: 0x06011515 RID: 70933 RVA: 0x004F7418 File Offset: 0x004F5618
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002CE4 RID: 11492
		// (get) Token: 0x06011516 RID: 70934 RVA: 0x004F7474 File Offset: 0x004F5674
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						if (pawn.Spawned && (pawn.story == null || !pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
						{
							Pawn_RoyaltyTracker royalty = pawn.royalty;
							RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
							if (royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined)
							{
								bool flag = false;
								foreach (KeyValuePair<Alert_RoyalNoAcceptableFood.CachedResult, bool> keyValuePair in this.cachedResults)
								{
									Alert_RoyalNoAcceptableFood.CachedResult cachedResult;
									bool flag2;
									keyValuePair.Deconstruct(out cachedResult, out flag2);
									Alert_RoyalNoAcceptableFood.CachedResult cachedResult2 = cachedResult;
									bool flag3 = flag2;
									if (cachedResult2.requirement.Equals(royalTitle.def.foodRequirement) && pawn.CanReach(cachedResult2.cell, PathEndMode.ClosestTouch, Danger.Unspecified, false, false, TraverseMode.ByPawn))
									{
										flag = true;
										if (!flag3)
										{
											this.targetsResult.Add(pawn);
											break;
										}
										break;
									}
								}
								if (!flag)
								{
									Alert_RoyalNoAcceptableFood.CachedResult key = new Alert_RoyalNoAcceptableFood.CachedResult(royalTitle.def.foodRequirement, pawn.Position);
									Thing thing;
									ThingDef thingDef;
									this.cachedResults[key] = FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, false, false, false, false, true, false, false, FoodPreferability.DesperateOnly);
									if (!this.cachedResults[key])
									{
										this.targetsResult.Add(pawn);
									}
								}
							}
						}
					}
				}
				this.cachedResults.Clear();
				return this.targetsResult;
			}
		}

		// Token: 0x06011517 RID: 70935 RVA: 0x004F7690 File Offset: 0x004F5890
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06011518 RID: 70936 RVA: 0x004F76A0 File Offset: 0x004F58A0
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle mostSeniorTitle = t.royalty.MostSeniorTitle;
				string[] array = new string[5];
				array[0] = t.LabelShort;
				array[1] = " (";
				array[2] = mostSeniorTitle.def.GetLabelFor(t.gender);
				array[3] = "):\n";
				array[4] = (from m in mostSeniorTitle.def.SatisfyingMeals(false)
				select m.LabelCap).ToLineList("  - ", false);
				string text = string.Concat(array);
				if (ModsConfig.IdeologyActive && t.Ideo != null && t.Ideo.VeneratedAnimals.Any<ThingDef>())
				{
					text = text + "\n\n" + "AlertRoyalTitleNoVeneratedAnimalMeat".Translate(t.Named("PAWN"), t.Ideo.Named("IDEO"), (from x in t.Ideo.VeneratedAnimals
					select x.label).ToCommaList(false, false).Named("ANIMALS")).Resolve();
				}
				return text;
			}).ToLineList("\n", false);
		}

		// Token: 0x0400B373 RID: 45939
		private readonly List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x0400B374 RID: 45940
		private readonly Dictionary<Alert_RoyalNoAcceptableFood.CachedResult, bool> cachedResults = new Dictionary<Alert_RoyalNoAcceptableFood.CachedResult, bool>();

		// Token: 0x02003014 RID: 12308
		private struct CachedResult
		{
			// Token: 0x06011519 RID: 70937 RVA: 0x004F76F7 File Offset: 0x004F58F7
			public CachedResult(RoyalTitleFoodRequirement requirement, IntVec3 cell)
			{
				this.requirement = requirement;
				this.cell = cell;
			}

			// Token: 0x0400B375 RID: 45941
			public RoyalTitleFoodRequirement requirement;

			// Token: 0x0400B376 RID: 45942
			public IntVec3 cell;
		}
	}
}
