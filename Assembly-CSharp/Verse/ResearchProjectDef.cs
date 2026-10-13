using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020002D1 RID: 721
	public class ResearchProjectDef : Def
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00052B6B File Offset: 0x00050D6B
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x00052B73 File Offset: 0x00050D73
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x00052B7B File Offset: 0x00050D7B
		public float CostApparent
		{
			get
			{
				return this.Cost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x00052B99 File Offset: 0x00050D99
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00052BA6 File Offset: 0x00050DA6
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00052BC4 File Offset: 0x00050DC4
		public string ProgressApparentString
		{
			get
			{
				string format = (this.baseCost > 0f) ? "F0" : "F2";
				return this.ProgressApparent.ToString(format);
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x00052BFA File Offset: 0x00050DFA
		public float ProgressPercent
		{
			get
			{
				return Mathf.Clamp01(this.ProgressReal / this.Cost);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00052C0E File Offset: 0x00050E0E
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.Cost;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00052C21 File Offset: 0x00050E21
		public float Cost
		{
			get
			{
				if (this.baseCost <= 0f)
				{
					return this.knowledgeCost;
				}
				return this.baseCost;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00040924 File Offset: 0x0003EB24
		public override TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00052C3D File Offset: 0x00050E3D
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00052C45 File Offset: 0x00050E45
		public bool IsHidden
		{
			get
			{
				return !this.IsFinished && ModsConfig.AnomalyActive && Find.EntityCodex.Hidden(this);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00052C64 File Offset: 0x00050E64
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench) && this.PlayerMechanitorRequirementMet && this.AnalyzedThingsRequirementsMet && !this.IsHidden && this.InspectionRequirementsMet;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00052CBC File Offset: 0x00050EBC
		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.hiddenPrerequisites != null)
				{
					for (int j = 0; j < this.hiddenPrerequisites.Count; j++)
					{
						if (!this.hiddenPrerequisites[j].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00052D30 File Offset: 0x00050F30
		public int TechprintCount
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					return 0;
				}
				return this.techprintCount;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00052D41 File Offset: 0x00050F41
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x00052D4E File Offset: 0x00050F4E
		public bool TechprintRequirementMet
		{
			get
			{
				return this.TechprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.TechprintCount;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00052D70 File Offset: 0x00050F70
		public ThingDef Techprint
		{
			get
			{
				if (this.TechprintCount <= 0)
				{
					return null;
				}
				if (this.cachedTechprint == null)
				{
					this.cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
					{
						CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
						return compProperties != null && compProperties.project == this;
					});
					if (this.cachedTechprint == null)
					{
						Log.ErrorOnce("Could not find techprint for research project " + ((this != null) ? this.ToString() : null), (int)this.shortHash ^ 873231450);
					}
				}
				return this.cachedTechprint;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00052DE2 File Offset: 0x00050FE2
		public int RequiredAnalyzedThingCount
		{
			get
			{
				List<ThingDef> list = this.requiredAnalyzed;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00052DF8 File Offset: 0x00050FF8
		public int AnalyzedThingsCompleted
		{
			get
			{
				if (this.requiredAnalyzed.NullOrEmpty<ThingDef>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.requiredAnalyzed.Count; i++)
				{
					AnalysisManager analysisManager = Find.AnalysisManager;
					CompProperties_CompAnalyzableUnlockResearch compProperties = this.requiredAnalyzed[i].GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
					AnalysisDetails analysisDetails;
					analysisManager.TryGetAnalysisProgress((compProperties != null) ? compProperties.analysisID : -1, out analysisDetails);
					if (analysisDetails != null && analysisDetails.Satisfied)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00052E66 File Offset: 0x00051066
		public bool AnalyzedThingsRequirementsMet
		{
			get
			{
				return this.RequiredAnalyzedThingCount <= 0 || this.AnalyzedThingsCompleted >= this.RequiredAnalyzedThingCount;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00052E84 File Offset: 0x00051084
		public bool InspectionRequirementsMet
		{
			get
			{
				return !ModsConfig.OdysseyActive || !this.requireGravEngineInspected || Find.ResearchManager.gravEngineInspected;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00052EA4 File Offset: 0x000510A4
		public List<Def> UnlockedDefs
		{
			get
			{
				if (this.cachedUnlockedDefs == null)
				{
					this.cachedUnlockedDefs = (from x in (from x in (from x in DefDatabase<RecipeDef>.AllDefs
					where x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))
					select x).SelectMany((RecipeDef x) => from y in x.products
					select y.thingDef)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.plant != null && x.plant.sowResearchPrerequisites != null && x.plant.sowResearchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<TerrainDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<RecipeDef>.AllDefs
					where x.IsSurgery && x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<PsychicRitualDef>.AllDefs
					where x.researchPrerequisite == this
					select x)
					orderby x.label
					select x).Distinct<Def>().ToList<Def>();
				}
				return this.cachedUnlockedDefs;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x0005306C File Offset: 0x0005126C
		public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
					List<Def> unlockedDefs = this.UnlockedDefs;
					if (unlockedDefs != null)
					{
						for (int i = 0; i < unlockedDefs.Count; i++)
						{
							this.cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i], -1));
						}
					}
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x000530C8 File Offset: 0x000512C8
		public bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x0005312F File Offset: 0x0005132F
		public bool PlayerMechanitorRequirementMet
		{
			get
			{
				return !ModsConfig.BiotechActive || !this.requiresMechanitor || MechanitorUtility.AnyMechanitorInPlayerFaction();
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00053147 File Offset: 0x00051347
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00053162 File Offset: 0x00051362
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.techLevel == TechLevel.Undefined && this.knowledgeCategory == null)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0f || this.ResearchViewY < 0f)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			if (this.techprintCount == 0 && !this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires no techprints but has heldByFactionCategoryTags.";
			}
			if (this.techprintCount > 0 && this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires techprints but has no heldByFactionCategoryTags.";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			int num3;
			for (int i = 0; i < rpDefs.Count; i = num3 + 1)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					string[] array = new string[9];
					array[0] = "same research view coords and tab as ";
					int num = 1;
					ResearchProjectDef researchProjectDef = rpDefs[i];
					array[num] = ((researchProjectDef != null) ? researchProjectDef.ToString() : null);
					array[2] = ": ";
					array[3] = this.ResearchViewX.ToString();
					array[4] = ", ";
					array[5] = this.ResearchViewY.ToString();
					array[6] = "(";
					int num2 = 7;
					ResearchTabDef researchTabDef = this.tab;
					array[num2] = ((researchTabDef != null) ? researchTabDef.ToString() : null);
					array[8] = ")";
					yield return string.Concat(array);
				}
				num3 = i;
			}
			if (!ModLister.RoyaltyInstalled && this.techprintCount > 0)
			{
				yield return "defines techprintCount, but techprints are a Royalty-specific game system and only work with Royalty installed.";
			}
			if (!this.requiredAnalyzed.NullOrEmpty<ThingDef>())
			{
				foreach (ThingDef thingDef in this.requiredAnalyzed)
				{
					if (!thingDef.HasAssignableCompFrom(typeof(CompAnalyzable)))
					{
						yield return string.Concat(new string[]
						{
							"requires analyzing ",
							thingDef.label,
							" but ",
							thingDef.label,
							" cannot be analyzed."
						});
					}
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			}
			if (this.knowledgeCost > 0f && this.knowledgeCategory == null)
			{
				yield return "has knowledge cost, but no knowledge category.";
			}
			if (this.baseCost > 0f && this.knowledgeCost > 0f)
			{
				yield return "baseCost and knowledgeCost are both defined. This is unsupported.";
			}
			yield break;
			yield break;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00053172 File Offset: 0x00051372
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
			if (!ModLister.BiotechInstalled)
			{
				this.requiredAnalyzed = null;
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00053198 File Offset: 0x00051398
		public string GetTip()
		{
			string text = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor);
			if (this.ProgressReal > 0f && !this.IsFinished)
			{
				text = string.Concat(new string[]
				{
					text,
					" (",
					this.ProgressApparentString,
					" / ",
					this.CostApparent.ToString("F0"),
					")"
				});
			}
			if (this.cachedDescription == null)
			{
				this.cachedDescription = this.Description;
				if (this.TechprintCount > 0)
				{
					this.cachedDescription = this.cachedDescription + "\n\n" + ("RequiredTechprintTip".Translate() + ": " + this.Techprint.LabelCap).ToString();
				}
				if (this.RequiredAnalyzedThingCount > 0)
				{
					string[] array = new string[5];
					array[0] = this.cachedDescription;
					array[1] = "\n\n";
					array[2] = "StudyRequirementTip".Translate().ToString();
					array[3] = ": ";
					array[4] = (from t in this.requiredAnalyzed
					select t.label).ToCommaList(true, false).CapitalizeFirst();
					this.cachedDescription = string.Concat(array);
				}
				if (this.modContentPack != null && !this.modContentPack.IsCoreMod)
				{
					Color color = this.modContentPack.IsOfficialMod ? ModLister.GetExpansionWithIdentifier(this.modContentPack.PackageId.ToLower()).primaryColor : ColoredText.SubtleGrayColor;
					this.cachedDescription = this.cachedDescription + "\n\n" + ("Stat_Source_Label".Translate().ToString() + ": " + this.modContentPack.Name).Colorize(color);
				}
			}
			return text + "\n" + this.cachedDescription;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000533A4 File Offset: 0x000515A4
		public float CostFactor(TechLevel researcherTechLevel)
		{
			if (this.techLevel == TechLevel.Undefined)
			{
				return 1f;
			}
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			if (researcherTechLevel >= techLevel)
			{
				return 1f;
			}
			int num = (int)(techLevel - researcherTechLevel);
			return 1f + (float)num * 0.5f;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000533E9 File Offset: 0x000515E9
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00053404 File Offset: 0x00051604
		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				CompAffectedByFacilities affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
				if (affectedByFacilities == null)
				{
					return false;
				}
				List<Thing> linkedFacilitiesListForReading = affectedByFacilities.LinkedFacilitiesListForReading;
				int i;
				Predicate<Thing> <>9__0;
				int j;
				for (i = 0; i < this.requiredResearchFacilities.Count; i = j + 1)
				{
					List<Thing> list = linkedFacilitiesListForReading;
					Predicate<Thing> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Thing x) => x.def == this.requiredResearchFacilities[i] && affectedByFacilities.IsFacilityActive(x)));
					}
					if (list.Find(match) == null)
					{
						return false;
					}
					j = i;
				}
			}
			return true;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x000534D4 File Offset: 0x000516D4
		public bool AnyOtherVisiblePrerequisitesCompleted(ResearchProjectDef prerequisite)
		{
			if (this.prerequisites == null)
			{
				return false;
			}
			for (int i = 0; i < this.prerequisites.Count; i++)
			{
				if (this.prerequisites[i] != prerequisite && this.prerequisites[i].IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00053528 File Offset: 0x00051728
		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						string str = "Exception applying research mod for project ";
						string str2 = (this != null) ? this.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000535A8 File Offset: 0x000517A8
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x000535B4 File Offset: 0x000517B4
		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			do
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				num++;
			}
			while (num <= 200);
			Log.Error("Couldn't relax research project coordinates apart after " + 200.ToString() + " passes.");
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0005385C File Offset: 0x00051A5C
		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000538B4 File Offset: 0x00051AB4
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			bool flag = Mathf.Abs(delta.x) > 0.01f;
			bool flag2 = Mathf.Abs(delta.y) > 0.01f;
			if (flag)
			{
				this.x += delta.x;
			}
			if (flag2)
			{
				this.y += delta.y;
			}
			if (flag || flag2)
			{
				this.positionModified = true;
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00053920 File Offset: 0x00051B20
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 2f) / 2f;
			this.y = Mathf.Round(this.y * 10f) / 10f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0005396D File Offset: 0x00051B6D
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		// Token: 0x04000FF5 RID: 4085
		public float baseCost;

		// Token: 0x04000FF6 RID: 4086
		public List<ResearchProjectDef> prerequisites;

		// Token: 0x04000FF7 RID: 4087
		public List<ResearchProjectDef> hiddenPrerequisites;

		// Token: 0x04000FF8 RID: 4088
		public TechLevel techLevel;

		// Token: 0x04000FF9 RID: 4089
		public List<ResearchProjectDef> requiredByThis;

		// Token: 0x04000FFA RID: 4090
		private List<ResearchMod> researchMods;

		// Token: 0x04000FFB RID: 4091
		public ThingDef requiredResearchBuilding;

		// Token: 0x04000FFC RID: 4092
		public List<ThingDef> requiredResearchFacilities;

		// Token: 0x04000FFD RID: 4093
		public List<ResearchProjectTagDef> tags;

		// Token: 0x04000FFE RID: 4094
		public ResearchTabDef tab;

		// Token: 0x04000FFF RID: 4095
		public float researchViewX = 1f;

		// Token: 0x04001000 RID: 4096
		public float researchViewY = 1f;

		// Token: 0x04001001 RID: 4097
		[MustTranslate]
		public string discoveredLetterTitle;

		// Token: 0x04001002 RID: 4098
		[MustTranslate]
		public string discoveredLetterText;

		// Token: 0x04001003 RID: 4099
		public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

		// Token: 0x04001004 RID: 4100
		public int techprintCount;

		// Token: 0x04001005 RID: 4101
		public float techprintCommonality = 1f;

		// Token: 0x04001006 RID: 4102
		public float techprintMarketValue = 1000f;

		// Token: 0x04001007 RID: 4103
		[NoTranslate]
		public List<string> heldByFactionCategoryTags;

		// Token: 0x04001008 RID: 4104
		public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

		// Token: 0x04001009 RID: 4105
		public bool requiresMechanitor;

		// Token: 0x0400100A RID: 4106
		public List<ThingDef> requiredAnalyzed;

		// Token: 0x0400100B RID: 4107
		public bool recalculatePower;

		// Token: 0x0400100C RID: 4108
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x0400100D RID: 4109
		public float knowledgeCost;

		// Token: 0x0400100E RID: 4110
		public ConceptDef teachConcept;

		// Token: 0x0400100F RID: 4111
		public RulePack generalRules;

		// Token: 0x04001010 RID: 4112
		public bool requireGravEngineInspected;

		// Token: 0x04001011 RID: 4113
		[MustTranslate]
		public List<string> customUnlockTexts;

		// Token: 0x04001012 RID: 4114
		[Unsaved(false)]
		private float x = 1f;

		// Token: 0x04001013 RID: 4115
		[Unsaved(false)]
		private float y = 1f;

		// Token: 0x04001014 RID: 4116
		[Unsaved(false)]
		private bool positionModified;

		// Token: 0x04001015 RID: 4117
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		// Token: 0x04001016 RID: 4118
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		// Token: 0x04001017 RID: 4119
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		// Token: 0x04001018 RID: 4120
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x04001019 RID: 4121
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		// Token: 0x0400101A RID: 4122
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}
