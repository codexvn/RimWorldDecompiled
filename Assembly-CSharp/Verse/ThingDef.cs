using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000301 RID: 769
	public class ThingDef : BuildableDef
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0005703E File Offset: 0x0005523E
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x00057050 File Offset: 0x00055250
		public bool EverPollutable
		{
			get
			{
				return !this.building.isNaturalRock;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x00057060 File Offset: 0x00055260
		public float VolumePerUnit
		{
			get
			{
				if (this.smallVolume)
				{
					return 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x00057075 File Offset: 0x00055275
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0005707D File Offset: 0x0005527D
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x00057088 File Offset: 0x00055288
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0005709B File Offset: 0x0005529B
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x000570A9 File Offset: 0x000552A9
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x000570B7 File Offset: 0x000552B7
		public float BaseMarketValue
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			set
			{
				this.SetStatBaseValue(StatDefOf.MarketValue, value);
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x000570C5 File Offset: 0x000552C5
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x000570D4 File Offset: 0x000552D4
		public int ArchonexusMaxAllowedCount
		{
			get
			{
				if (this.allowedArchonexusCount == -1)
				{
					return Mathf.Min(new int[]
					{
						this.stackLimit,
						25,
						(this.BaseMass > 0f) ? ((int)(5f / this.BaseMass)) : 0,
						(this.BaseMarketValue > 0f) ? ((int)(2000f / this.BaseMarketValue)) : 0
					});
				}
				return this.allowedArchonexusCount;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x0005714C File Offset: 0x0005534C
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop && (this != ThingDefOf.ReinforcedBarrel || Find.Storyteller == null || !Find.Storyteller.difficulty.classicMortars) && (this.requiresFactionToAcquire == null || Find.World == null || Find.World.factionManager == null || Find.FactionManager.FirstFactionOfDef(this.requiresFactionToAcquire) != null);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x000571B8 File Offset: 0x000553B8
		public bool EverTransmitsPower
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					CompProperties_Power compProperties_Power = this.comps[i] as CompProperties_Power;
					if (compProperties_Power != null && compProperties_Power.transmitsPower)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x000571FB File Offset: 0x000553FB
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x00057206 File Offset: 0x00055406
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x00057218 File Offset: 0x00055418
		public List<RecipeDef> AllRecipes
		{
			get
			{
				if (this.allRecipesCached == null)
				{
					this.allRecipesCached = new List<RecipeDef>();
					if (this.recipes != null)
					{
						for (int i = 0; i < this.recipes.Count; i++)
						{
							this.allRecipesCached.Add(this.recipes[i]);
						}
					}
					List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
						{
							this.allRecipesCached.Add(allDefsListForReading[j]);
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x000572C4 File Offset: 0x000554C4
		public bool ConnectToPower
		{
			get
			{
				if (this.EverTransmitsPower)
				{
					return false;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == typeof(CompPowerBattery))
					{
						return true;
					}
					if (this.comps[i].compClass == typeof(CompPowerTrader))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x0005733A File Offset: 0x0005553A
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x0005734F File Offset: 0x0005554F
		public FillCategory Fillage
		{
			get
			{
				if (this.fillPercent < 0.01f)
				{
					return FillCategory.None;
				}
				if (this.fillPercent > 0.99f)
				{
					return FillCategory.Full;
				}
				return FillCategory.Partial;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x00057370 File Offset: 0x00055570
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0005737C File Offset: 0x0005557C
		public bool CanOverlapZones
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (this.passability == Traversability.Impassable && this.category != ThingCategory.Plant && !this.HasComp(typeof(CompTransporter)))
				{
					return false;
				}
				if (this.surfaceType >= SurfaceType.Item)
				{
					return false;
				}
				if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					return false;
				}
				if (!this.canOverlapZones)
				{
					return false;
				}
				if (this.IsBlueprint || this.IsFrame)
				{
					ThingDef thingDef = this.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						return thingDef.CanOverlapZones;
					}
				}
				return true;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0005741B File Offset: 0x0005561B
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority > ResourceCountPriority.Uncounted;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00057426 File Offset: 0x00055626
		public List<VerbProperties> Verbs
		{
			get
			{
				if (this.verbs != null)
				{
					return this.verbs;
				}
				return ThingDef.EmptyVerbPropertiesList;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0005743C File Offset: 0x0005563C
		public bool CanHaveFaction
		{
			get
			{
				if (this.IsBlueprint || this.IsFrame)
				{
					return true;
				}
				ThingCategory thingCategory = this.category;
				return thingCategory == ThingCategory.Pawn || thingCategory == ThingCategory.Building;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00057471 File Offset: 0x00055671
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x00057498 File Offset: 0x00055698
		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				if (this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return null;
				}
				return this.thingCategories[0];
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x000574B5 File Offset: 0x000556B5
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x000574D8 File Offset: 0x000556D8
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || (this.plant != null && this.plant.canDeteriorate));
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00057504 File Offset: 0x00055704
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.canInteractThroughCorners || (this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock || this.IsSmoothed));
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00057550 File Offset: 0x00055750
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor || this.IsFence;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0005756B File Offset: 0x0005576B
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00057598 File Offset: 0x00055798
		public string DescriptionDetailed
		{
			get
			{
				if (this.descriptionDetailedCached == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.description);
					if (this.IsApparel)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(string.Format("{0}: {1}", "Layer".Translate(), this.apparel.GetLayersString()));
						stringBuilder.Append(string.Format("{0}: {1}", "Covers".Translate(), this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human)));
						if (this.equippedStatOffsets != null && this.equippedStatOffsets.Count > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
							for (int i = 0; i < this.equippedStatOffsets.Count; i++)
							{
								if (i > 0)
								{
									stringBuilder.AppendLine();
								}
								StatModifier statModifier = this.equippedStatOffsets[i];
								stringBuilder.Append(string.Format("{0}: {1}", statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset));
							}
						}
					}
					this.descriptionDetailedCached = stringBuilder.ToString();
				}
				return this.descriptionDetailedCached;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x000576C2 File Offset: 0x000558C2
		public bool CanBenefitFromCover
		{
			get
			{
				return this.category == ThingCategory.Pawn || (this.building != null && this.building.IsTurret);
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x000576E8 File Offset: 0x000558E8
		public bool PotentiallySmeltable
		{
			get
			{
				if (!this.smeltable)
				{
					return false;
				}
				if (base.MadeFromStuff)
				{
					using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(this, TechLevel.Undefined, false).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.smeltable)
							{
								return true;
							}
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00057754 File Offset: 0x00055954
		public bool HasSingleOrMultipleInteractionCells
		{
			get
			{
				return this.hasInteractionCell || !this.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>();
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x0005776E File Offset: 0x0005596E
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00057779 File Offset: 0x00055979
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00057790 File Offset: 0x00055990
		public bool IsWall
		{
			get
			{
				return this.building != null && this.building.isWall;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x000577A7 File Offset: 0x000559A7
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x000577BE File Offset: 0x000559BE
		public bool IsFrame
		{
			get
			{
				return this.isFrameInt;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x000577C6 File Offset: 0x000559C6
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x000577DC File Offset: 0x000559DC
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x000577E7 File Offset: 0x000559E7
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000577F9 File Offset: 0x000559F9
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00057810 File Offset: 0x00055A10
		public bool IsFence
		{
			get
			{
				return this.building != null && this.building.isFence;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00057827 File Offset: 0x00055A27
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00057832 File Offset: 0x00055A32
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0005783D File Offset: 0x00055A3D
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0005785B File Offset: 0x00055A5B
		public bool IsNutritionGivingIngestibleForHumanlikeBabies
		{
			get
			{
				return this.IsNutritionGivingIngestible && this.ingestible.HumanEdible && this.ingestible.babiesCanIngest;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0005787F File Offset: 0x00055A7F
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>()) && !this.IsApparel;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x000578AF File Offset: 0x00055AAF
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x000578C6 File Offset: 0x00055AC6
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x000578DD File Offset: 0x00055ADD
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x000578F4 File Offset: 0x00055AF4
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory > DrugCategory.None;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0005790E File Offset: 0x00055B0E
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0005792C File Offset: 0x00055B2C
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x00057949 File Offset: 0x00055B49
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00057966 File Offset: 0x00055B66
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0005797D File Offset: 0x00055B7D
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x00057988 File Offset: 0x00055B88
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001305 RID: 4869 RVA: 0x00057995 File Offset: 0x00055B95
		public bool IsSmoothable
		{
			get
			{
				BuildingProperties buildingProperties = this.building;
				return ((buildingProperties != null) ? buildingProperties.smoothedThing : null) != null;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x000579AC File Offset: 0x00055BAC
		public bool IsSmoothed
		{
			get
			{
				BuildingProperties buildingProperties = this.building;
				return ((buildingProperties != null) ? buildingProperties.unsmoothedThing : null) != null;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x000579C3 File Offset: 0x00055BC3
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x000579E4 File Offset: 0x00055BE4
		public bool IsCryptosleepCasket
		{
			get
			{
				return typeof(Building_CryptosleepCasket).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x000579FB File Offset: 0x00055BFB
		public bool IsGibbetCage
		{
			get
			{
				return typeof(Building_GibbetCage).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00057A12 File Offset: 0x00055C12
		public bool IsMechGestator
		{
			get
			{
				return typeof(Building_MechGestator).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00057A29 File Offset: 0x00055C29
		public bool IsMechRecharger
		{
			get
			{
				return typeof(Building_MechCharger).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00057A40 File Offset: 0x00055C40
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x00057A66 File Offset: 0x00055C66
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x00057A8B File Offset: 0x00055C8B
		public bool IsEgg
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && (this.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || this.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized));
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x00057AC4 File Offset: 0x00055CC4
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x00057AE9 File Offset: 0x00055CE9
		public bool IsWool
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Wools);
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x00057B10 File Offset: 0x00055D10
		public bool IsRangedWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (!this.verbs[i].IsMeleeAttack)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x00057B60 File Offset: 0x00055D60
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x00057B78 File Offset: 0x00055D78
		public bool IsWeaponUsingProjectiles
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (this.verbs[i].LaunchesProjectile)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x00057BC8 File Offset: 0x00055DC8
		public bool IsShieldThatBlocksRanged
		{
			get
			{
				return this.HasComp(typeof(CompShield)) && this.GetCompProperties<CompProperties_Shield>().blocksRangedWeapons;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x00057BE9 File Offset: 0x00055DE9
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x00057C25 File Offset: 0x00055E25
		public bool IsNonResourceNaturalRock
		{
			get
			{
				return this.category == ThingCategory.Building && this.building.isNaturalRock && !this.building.isResourceRock && !this.building.mineablePreventNaturalRockOnSurface && !this.IsSmoothed;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x00057C62 File Offset: 0x00055E62
		public bool HasSunShadows
		{
			get
			{
				if (this.hasSunShadowsCached == null)
				{
					this.hasSunShadowsCached = new bool?(typeof(Pawn).IsAssignableFrom(this.thingClass));
				}
				return this.hasSunShadowsCached.Value;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x00057C9C File Offset: 0x00055E9C
		public bool IsNaturalOrgan
		{
			get
			{
				if (this.isNaturalOrganCached == null)
				{
					if (this.category != ThingCategory.Item)
					{
						this.isNaturalOrganCached = new bool?(false);
					}
					else
					{
						List<BodyPartDef> allDefsListForReading = DefDatabase<BodyPartDef>.AllDefsListForReading;
						this.isNaturalOrganCached = new bool?(false);
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (allDefsListForReading[i].spawnThingOnRemoved == this)
							{
								this.isNaturalOrganCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isNaturalOrganCached.Value;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x00057D18 File Offset: 0x00055F18
		public bool IsFungus
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.Fungus);
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x00057D43 File Offset: 0x00055F43
		public bool IsAnimalProduct
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.AnimalProduct);
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x00057D6B File Offset: 0x00055F6B
		public bool IsProcessedFood
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.Processed);
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00057D96 File Offset: 0x00055F96
		public bool CanAffectLinker
		{
			get
			{
				return (this.graphicData != null && this.graphicData.Linked) || this.IsDoor;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00057DB8 File Offset: 0x00055FB8
		public bool IsNonDeconstructibleAttackableBuilding
		{
			get
			{
				return this.IsBuildingArtificial && !this.building.IsDeconstructible && this.destroyable && !this.mineable && this.building.isTargetable && this.building.draftAttackNonDeconstructable;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x00057E04 File Offset: 0x00056004
		public bool IsPlant
		{
			get
			{
				return typeof(Plant).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x00057E1B File Offset: 0x0005601B
		public bool IsDeadPlant
		{
			get
			{
				return typeof(DeadPlant).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x00057E32 File Offset: 0x00056032
		public bool IsStudiable
		{
			get
			{
				return this.HasAssignableCompFrom(typeof(CompStudiable));
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x00057E44 File Offset: 0x00056044
		public List<StyleCategoryDef> RelevantStyleCategories
		{
			get
			{
				if (this.cachedRelevantStyleCategories == null)
				{
					this.cachedRelevantStyleCategories = new List<StyleCategoryDef>();
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						if (!styleCategoryDef.thingDefStyles.NullOrEmpty<ThingDefStyle>())
						{
							using (List<ThingDefStyle>.Enumerator enumerator2 = styleCategoryDef.thingDefStyles.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.ThingDef == this)
									{
										this.cachedRelevantStyleCategories.Add(styleCategoryDef);
										break;
									}
								}
							}
						}
					}
				}
				return this.cachedRelevantStyleCategories;
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00057F04 File Offset: 0x00056104
		public bool BlocksPlanting(bool canWipePlants = false)
		{
			return (this.building == null || !this.building.SupportsPlants) && (this.building == null || !this.building.isAttachment) && (this.blockPlants || (!canWipePlants && this.category == ThingCategory.Plant) || this.Fillage > FillCategory.None || this.IsEdifice());
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanSpawnAt(IntVec3 pos, Rot4 rot, Map map)
		{
			return true;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00057F70 File Offset: 0x00056170
		public bool EverStorable(bool willMinifyIfPossible)
		{
			if (typeof(MinifiedThing).IsAssignableFrom(this.thingClass))
			{
				return true;
			}
			if (!this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				if (this.category == ThingCategory.Item)
				{
					return true;
				}
				if (willMinifyIfPossible && this.Minifiable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00057FBC File Offset: 0x000561BC
		public Thing GetConcreteExample(ThingDef stuff = null)
		{
			if (this.concreteExamplesInt == null)
			{
				this.concreteExamplesInt = new Dictionary<ThingDef, Thing>();
			}
			if (stuff == null)
			{
				stuff = ThingDefOf.Steel;
			}
			if (!this.concreteExamplesInt.ContainsKey(stuff))
			{
				if (this.race == null)
				{
					this.concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, base.MadeFromStuff ? stuff : null);
				}
				else
				{
					this.concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.AllDefsListForReading.FirstOrDefault((PawnKindDef pkd) => pkd.race == this), null, null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0005805C File Offset: 0x0005625C
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == typeof(T))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000580B0 File Offset: 0x000562B0
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (typeof(T).IsAssignableFrom(this.comps[i].compClass))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00058104 File Offset: 0x00056304
		public bool HasComp(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == compType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00058144 File Offset: 0x00056344
		public bool HasComp<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == typeof(T) || typeof(T).IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000581B0 File Offset: 0x000563B0
		public bool HasAssignableCompFrom(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (compType.IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000581F0 File Offset: 0x000563F0
		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00058240 File Offset: 0x00056440
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					GraphicData graphicData = this.graphicData;
					if (graphicData.shaderType == null)
					{
						graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					ContentFinderRequester.requester = this;
					try
					{
						this.graphic = this.graphicData.Graphic;
						if (this.drawerType != DrawerType.RealtimeOnly)
						{
							TextureAtlasGroup textureAtlasGroup = this.category.ToAtlasGroup();
							this.graphic.TryInsertIntoAtlas(textureAtlasGroup);
							if (textureAtlasGroup == TextureAtlasGroup.Building && this.Minifiable)
							{
								this.graphic.TryInsertIntoAtlas(TextureAtlasGroup.Item);
							}
						}
					}
					finally
					{
						ContentFinderRequester.requester = null;
					}
				});
			}
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			if (this.verbs != null && this.verbs.Count == 1 && this.verbs[0].label.NullOrEmpty())
			{
				this.verbs[0].label = this.label;
			}
			base.PostLoad();
			if (this.category == ThingCategory.Building && this.building == null)
			{
				this.building = new BuildingProperties();
			}
			BuildingProperties buildingProperties = this.building;
			if (buildingProperties != null)
			{
				buildingProperties.PostLoadSpecial(this);
			}
			ApparelProperties apparelProperties = this.apparel;
			if (apparelProperties != null)
			{
				apparelProperties.PostLoadSpecial(this);
			}
			PlantProperties plantProperties = this.plant;
			if (plantProperties != null)
			{
				plantProperties.PostLoadSpecial(this);
			}
			if (this.comps != null)
			{
				foreach (CompProperties compProperties in this.comps)
				{
					compProperties.PostLoadSpecial(this);
				}
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00058388 File Offset: 0x00056588
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			if (this.category == ThingCategory.Pawn)
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					return;
				}
				if (!this.race.Humanlike)
				{
					PawnKindDef anyPawnKind = this.race.AnyPawnKind;
					if (anyPawnKind != null)
					{
						Material material = ((ModsConfig.BiotechActive && anyPawnKind.RaceProps.IsMechanoid) ? anyPawnKind.lifeStages.First<PawnKindLifeStage>() : anyPawnKind.lifeStages.Last<PawnKindLifeStage>()).bodyGraphicData.Graphic.MatAt(Rot4.East, null);
						this.uiIcon = (Texture2D)material.mainTexture;
						this.uiIconColor = material.color;
						Shader shader;
						MaterialRequest req;
						if (ShaderDatabase.TryGetUIShader(material.shader, out shader) && MaterialPool.TryGetRequestForMat(material, out req))
						{
							req.shader = shader;
							this.uiIconMaterial = MaterialPool.MatFrom(req);
							return;
						}
					}
				}
			}
			else
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(this);
				if (this.colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
				{
					this.uiIconColor = this.colorGenerator.ExemplaryColor;
				}
				else if (thingDef != null)
				{
					this.uiIconColor = base.GetColorForStuff(thingDef);
				}
				else if (this.graphicData != null)
				{
					this.uiIconColor = this.graphicData.color;
				}
				if (this.rotatable && this.graphic != null && this.graphic != BaseContent.BadGraphic && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
				{
					this.uiIconAngle = 180f + this.graphic.DrawRotatedExtraAngleOffset;
				}
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00058534 File Offset: 0x00056734
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.ingestible != null)
			{
				this.ingestible.parent = this;
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.parent = this;
			}
			BuildingProperties buildingProperties = this.building;
			if (buildingProperties != null)
			{
				buildingProperties.ResolveReferencesSpecial();
			}
			GraphicData graphicData = this.graphicData;
			if (graphicData != null)
			{
				graphicData.ResolveReferencesSpecial();
			}
			RaceProperties raceProperties = this.race;
			if (raceProperties != null)
			{
				raceProperties.ResolveReferencesSpecial();
			}
			StuffProperties stuffProperties = this.stuffProps;
			if (stuffProperties != null)
			{
				stuffProperties.ResolveReferencesSpecial();
			}
			ApparelProperties apparelProperties = this.apparel;
			if (apparelProperties != null)
			{
				apparelProperties.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpact_Ground;
			}
			if (this.soundDrop == null)
			{
				this.soundDrop = SoundDefOf.Standard_Drop;
			}
			if (this.soundPickup == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.soundInteract == null)
			{
				this.soundInteract = SoundDefOf.Standard_Pickup;
			}
			if (this.inspectorTabs != null && this.inspectorTabs.Any<Type>())
			{
				this.inspectorTabsResolved = new List<InspectTabBase>();
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate inspector tab of type ";
						Type type = this.inspectorTabs[i];
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					this.comps[j].ResolveReferences(this);
				}
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000586E4 File Offset: 0x000568E4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category != ThingCategory.Ethereal && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.category == ThingCategory.Building && !this.IsFrame && this.building.IsDeconstructible && this.thingClass != null && typeof(Building).IsSubclassOf(this.thingClass))
			{
				yield return "has building category and is marked as deconstructible, but thing class is not a subclass of building (" + this.thingClass.Name + ")";
			}
			if (this.graphicData != null)
			{
				foreach (string text2 in this.graphicData.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			if (this.projectile != null)
			{
				foreach (string text3 in this.projectile.ConfigErrors(this))
				{
					yield return text3;
				}
				enumerator = null;
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if (this.statBases.Count((StatModifier st) => st.stat == statBase.stat) > 1)
						{
							string str = "defines the stat base ";
							StatDef stat = statBase.stat;
							yield return str + ((stat != null) ? stat.ToString() : null) + " more than once.";
						}
					}
				}
				List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
			}
			if (!BeautyUtility.BeautyRelevant(this.category) && this.StatBaseDefined(StatDefOf.Beauty))
			{
				yield return "Beauty stat base is defined, but Things of category " + this.category.ToString() + " cannot have beauty.";
			}
			if (!BeautyUtility.BeautyRelevant(this.category) && this.StatBaseDefined(StatDefOf.BeautyOutdoors))
			{
				yield return "BeautyOutdoors stat base is defined, but Things of category " + this.category.ToString() + " cannot have beauty.";
			}
			if (char.IsNumber(this.defName[this.defName.Length - 1]))
			{
				yield return "ends with a numerical digit, which is not allowed on ThingDefs.";
			}
			if (this.thingClass == null)
			{
				yield return "has null thingClass.";
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return "has components but it's thingClass is not a ThingWithComps";
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return "connects to power but does not add to map mesh. Will not create wire meshes.";
			}
			if (this.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in this.costList)
				{
					if (thingDefCountClass.count == 0)
					{
						string str2 = "cost in ";
						ThingDef thingDef = thingDefCountClass.thingDef;
						yield return str2 + ((thingDef != null) ? thingDef.ToString() : null) + " is zero.";
					}
				}
				List<ThingDefCountClass>.Enumerator enumerator3 = default(List<ThingDefCountClass>.Enumerator);
			}
			List<ThingCategoryDef> list = this.thingCategories;
			ThingCategoryDef thingCategoryDef = (list != null) ? list.FirstOrDefault((ThingCategoryDef cat) => this.thingCategories.Count((ThingCategoryDef c) => c == cat) > 1) : null;
			if (thingCategoryDef != null)
			{
				string str3 = "has duplicate thingCategory ";
				ThingCategoryDef thingCategoryDef2 = thingCategoryDef;
				yield return str3 + ((thingCategoryDef2 != null) ? thingCategoryDef2.ToString() : null) + ".";
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return "gives full cover but is not a building.";
			}
			if (this.equipmentType != EquipmentType.None)
			{
				if (this.techLevel == TechLevel.Undefined && !this.destroyOnDrop)
				{
					yield return "is equipment but has no tech level.";
				}
				if (!this.comps.Any((CompProperties c) => typeof(CompEquippable).IsAssignableFrom(c.compClass)))
				{
					yield return "is equipment but has no CompEquippable";
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return " is a bullet but has no damageDef.";
			}
			if (this.destroyOnDrop && this.tradeability != Tradeability.None)
			{
				yield return "destroyOnDrop but tradeability is " + this.tradeability.ToString();
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return "has stackLimit > 1 but also has drawGUIOverlay = false.";
			}
			if (this.damageMultipliers != null)
			{
				using (List<DamageMultiplier>.Enumerator enumerator4 = this.damageMultipliers.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						DamageMultiplier mult = enumerator4.Current;
						if (this.damageMultipliers.Count((DamageMultiplier m) => m.damageDef == mult.damageDef) > 1)
						{
							string str4 = "has multiple damage multipliers for damageDef ";
							DamageDef damageDef = mult.damageDef;
							yield return str4 + ((damageDef != null) ? damageDef.ToString() : null);
							break;
						}
					}
				}
				List<DamageMultiplier>.Enumerator enumerator4 = default(List<DamageMultiplier>.Enumerator);
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
			}
			if (base.MadeFromStuff && this.constructEffect != null)
			{
				yield return "madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
			}
			if (base.MadeFromStuff && this.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				yield return "madeFromStuff but has no stuffCategories.";
			}
			if (this.costList.NullOrEmpty<ThingDefCountClass>() && this.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
			}
			if (this.costStuffCount > 0 && this.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				yield return "has costStuffCount but no stuffCategories.";
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 1E-05f && !this.CanEverDeteriorate && !this.destroyOnDrop)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
			}
			if (this.smeltProducts != null && !this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
			}
			if (this.smeltable && this.smeltProducts.NullOrEmpty<ThingDefCountClass>() && base.CostList.NullOrEmpty<ThingDefCountClass>() && !this.IsStuff && !base.MadeFromStuff && !this.destroyOnDrop)
			{
				yield return "is smeltable but does not give anything for smelting.";
			}
			if (this.equipmentType != EquipmentType.None && this.verbs.NullOrEmpty<VerbProperties>() && this.tools.NullOrEmpty<Tool>())
			{
				yield return "is equipment but has no verbs or tools";
			}
			if (this.Minifiable && this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is minifiable but not in any thing category";
			}
			if (this.category == ThingCategory.Building && !this.Minifiable && !this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is not minifiable yet has thing categories (could be confusing in thing filters because it can't be moved/stored anyway)";
			}
			if (!this.destroyOnDrop && !typeof(MinifiedThing).IsAssignableFrom(this.thingClass) && (this.EverHaulable || this.Minifiable))
			{
				if (!this.statBases.NullOrEmpty<StatModifier>())
				{
					if (this.statBases.Any((StatModifier s) => s.stat == StatDefOf.Mass))
					{
						goto IL_B94;
					}
				}
				yield return "is haulable, but does not have an authored mass value";
			}
			IL_B94:
			if (this.ingestible == null && this.GetStatValueAbstract(StatDefOf.Nutrition, null) != 0f)
			{
				yield return "has nutrition but ingestible properties are null";
			}
			if (this.BaseFlammability != 0f && !this.useHitPoints && this.category != ThingCategory.Pawn && !this.destroyOnDrop)
			{
				yield return "flammable but has no hitpoints (will burn indefinitely)";
			}
			GraphicData graphicData = this.graphicData;
			if (((graphicData != null) ? graphicData.shadowData : null) != null && this.staticSunShadowHeight > 0f)
			{
				yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
			}
			if (this.saveCompressible && this.Claimable)
			{
				yield return "claimable item is compressible; faction will be unset after load";
			}
			if (this.deepCommonality > 0f != this.deepLumpSizeRange.TrueMax > 0)
			{
				yield return "if deepCommonality or deepLumpSizeRange is set, the other also must be set";
			}
			if (this.deepCommonality > 0f && this.deepCountPerPortion <= 0)
			{
				yield return "deepCommonality > 0 but deepCountPerPortion is not set";
			}
			if (this.verbs != null)
			{
				int num;
				for (int i = 0; i < this.verbs.Count; i = num + 1)
				{
					foreach (string arg in this.verbs[i].ConfigErrors(this))
					{
						yield return string.Format("verb {0}: {1}", i, arg);
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.building != null)
			{
				foreach (string text4 in this.building.ConfigErrors(this))
				{
					yield return text4;
				}
				enumerator = null;
				if ((this.building.isAirtight || this.building.isStuffableAirtight) && this.Fillage != FillCategory.Full)
				{
					yield return "is airtight but Fillage is not Full";
				}
			}
			if (this.apparel != null)
			{
				foreach (string text5 in this.apparel.ConfigErrors(this))
				{
					yield return text5;
				}
				enumerator = null;
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string text6 in this.comps[i].ConfigErrors(this))
					{
						yield return text6;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.race != null)
			{
				foreach (string text7 in this.race.ConfigErrors(this))
				{
					yield return text7;
				}
				enumerator = null;
				if (this.race.body != null && this.race != null && this.tools != null)
				{
					ThingDef.<>c__DisplayClass401_3 CS$<>8__locals3 = new ThingDef.<>c__DisplayClass401_3();
					CS$<>8__locals3.<>4__this = this;
					CS$<>8__locals3.i = 0;
					while (CS$<>8__locals3.i < this.tools.Count)
					{
						if (this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(CS$<>8__locals3.<>4__this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup)))
						{
							string[] array = new string[5];
							array[0] = "has tool with linkedBodyPartsGroup ";
							int num2 = 1;
							BodyPartGroupDef linkedBodyPartsGroup = this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup;
							array[num2] = ((linkedBodyPartsGroup != null) ? linkedBodyPartsGroup.ToString() : null);
							array[2] = " but body ";
							int num3 = 3;
							BodyDef body = this.race.body;
							array[num3] = ((body != null) ? body.ToString() : null);
							array[4] = " has no parts with that group.";
							yield return string.Concat(array);
						}
						int num = CS$<>8__locals3.i;
						CS$<>8__locals3.i = num + 1;
					}
					CS$<>8__locals3 = null;
				}
				if (this.race.Animal && this.GetStatValueAbstract(StatDefOf.Wildness, null) < 0f)
				{
					yield return "is animal but wildness is not defined";
				}
			}
			if (this.ingestible != null)
			{
				foreach (string text8 in this.ingestible.ConfigErrors())
				{
					yield return text8;
				}
				enumerator = null;
			}
			if (this.plant != null)
			{
				foreach (string text9 in this.plant.ConfigErrors())
				{
					yield return text9;
				}
				enumerator = null;
			}
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return "duplicate thingdef tool id " + tool.id;
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text10 in tool2.ConfigErrors())
					{
						yield return text10;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator5 = default(List<Tool>.Enumerator);
			}
			if (!this.randomStyle.NullOrEmpty<ThingStyleChance>())
			{
				using (List<ThingStyleChance>.Enumerator enumerator6 = this.randomStyle.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						if (enumerator6.Current.Chance <= 0f)
						{
							yield return "style chance <= 0.";
						}
					}
				}
				List<ThingStyleChance>.Enumerator enumerator6 = default(List<ThingStyleChance>.Enumerator);
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompStyleable)))
				{
					yield return "random style assigned, but missing CompStyleable!";
				}
			}
			if (this.relicChance > 0f && this.category != ThingCategory.Item)
			{
				yield return "relic chance > 0 but category != item";
			}
			if (this.hasInteractionCell && !this.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>())
			{
				yield return "both single and multiple interaction cells are defined, it should be one or the other";
			}
			if (this.Fillage != FillCategory.Full && this.passability == Traversability.Impassable && !this.IsDoor && base.BuildableByPlayer && !this.disableImpassableShotOverConfigError)
			{
				yield return "impassable, player-buildable building that can be shot/seen over.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000586F4 File Offset: 0x000568F4
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001331 RID: 4913 RVA: 0x000586FD File Offset: 0x000568FD
		public string LabelAsStuff
		{
			get
			{
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					return this.stuffProps.stuffAdjective;
				}
				return this.label;
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00058724 File Offset: 0x00056924
		public bool IsWithinCategory(ThingCategoryDef category)
		{
			if (this.thingCategories == null)
			{
				return false;
			}
			for (int i = 0; i < this.thingCategories.Count; i++)
			{
				for (ThingCategoryDef thingCategoryDef = this.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
				{
					if (thingCategoryDef == category)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00058774 File Offset: 0x00056974
		public void Notify_UnlockedByResearch()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_PostUnlockedByResearch(this);
				}
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000587B1 File Offset: 0x000569B1
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.apparel != null)
			{
				string coveredOuterPartsString = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredOuterPartsString, "Stat_Thing_Apparel_Covers_Desc".Translate(), 2750, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Layer".Translate(), this.apparel.GetLayersString(), "Stat_Thing_Apparel_Layer_Desc".Translate(), 2751, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_CountsAsClothingNudity_Name".Translate(), this.apparel.countsAsClothingForNudity ? "Yes".Translate() : "No".Translate(), "Stat_Thing_Apparel_CountsAsClothingNudity_Desc".Translate(), 2753, null, null, false, false);
				if (ModsConfig.BiotechActive)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_ValidLifestage".Translate(), this.apparel.developmentalStageFilter.ToCommaList(false).CapitalizeFirst(), "Stat_Thing_Apparel_ValidLifestage_Desc".Translate(), 2748, null, null, false, false);
				}
				if (this.apparel.gender != Gender.None)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_Gender".Translate(), this.apparel.gender.GetLabel(false).CapitalizeFirst(), "Stat_Thing_Apparel_Gender_Desc".Translate(), 2749, null, null, false, false);
				}
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), "Stat_Thing_Drug_MedicineXpGainFactor_Desc".Translate(), 1000, null, null, false, false);
			}
			if (this.fillPercent > 0f && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), "CoverEffectivenessExplanation".Translate(), 2000, null, null, false, false);
				yield return statDrawEntry2;
			}
			if (this.constructionSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Construction.LabelCap), this.constructionSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Construction.LabelCap), 1100, null, null, false, false);
			}
			if (this.artisticSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Artistic.LabelCap), this.artisticSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Artistic.LabelCap), 1100, null, null, false, false);
			}
			IEnumerable<RecipeDef> recipes = from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.products.Count == 1 && r.products.Any((ThingDefCountClass p) => p.thingDef == this) && !r.IsSurgery
			select r;
			if (recipes.Any<RecipeDef>())
			{
				IEnumerable<string> enumerable = (from u in (from x in recipes
				where x.recipeUsers != null
				select x).SelectMany((RecipeDef r) => r.recipeUsers)
				select u.label).Concat(from x in DefDatabase<ThingDef>.AllDefsListForReading
				where x.recipes != null && x.recipes.Any((RecipeDef y) => y.products.Any((ThingDefCountClass z) => z.thingDef == this))
				select x.label).Distinct<string>();
				if (enumerable.Any<string>())
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CreatedAt".Translate(), enumerable.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_CreatedAt_Desc".Translate(), 1103, null, null, false, false);
				}
				RecipeDef recipeDef = recipes.FirstOrDefault<RecipeDef>();
				if (recipeDef != null && !recipeDef.ingredients.NullOrEmpty<IngredientCount>())
				{
					BuildableDef.tmpCostList.Clear();
					BuildableDef.tmpHyperlinks.Clear();
					for (int j = 0; j < recipeDef.ingredients.Count; j++)
					{
						IngredientCount ingredientCount = recipeDef.ingredients[j];
						if (!ingredientCount.filter.Summary.NullOrEmpty())
						{
							IEnumerable<ThingDef> allowedThingDefs = ingredientCount.filter.AllowedThingDefs;
							if (allowedThingDefs.Any<ThingDef>())
							{
								using (IEnumerator<ThingDef> enumerator2 = allowedThingDefs.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										ThingDef p = enumerator2.Current;
										if (!BuildableDef.tmpHyperlinks.Any((Dialog_InfoCard.Hyperlink x) => x.def == p))
										{
											BuildableDef.tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(p, -1));
										}
									}
								}
							}
							BuildableDef.tmpCostList.Add(recipeDef.IngredientValueGetter.BillRequirementsDescription(recipeDef, ingredientCount));
						}
					}
				}
				if (BuildableDef.tmpCostList.Any<string>())
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), BuildableDef.tmpCostList.ToCommaList(false, false), "Stat_Thing_Ingredients".Translate(), 1102, null, BuildableDef.tmpHyperlinks, false, false);
				}
			}
			if (this.thingClass != null && typeof(Building_Bed).IsAssignableFrom(this.thingClass) && !this.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.BedRestEffectiveness, StatDefOf.BedRestEffectiveness.valueIfMissing, StatRequest.ForEmpty(), ToStringNumberSense.Undefined, null, false);
			}
			if (!this.verbs.NullOrEmpty<VerbProperties>())
			{
				VerbProperties verb = this.verbs.First((VerbProperties x) => x.isPrimary);
				StatCategoryDef verbStatCategory = (this.category == ThingCategory.Pawn) ? StatCategoryDefOf.PawnCombat : null;
				float num = verb.warmupTime;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_Weapon_RangedWarmupTime_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToString("0.##") + " " + "LetterSecond".Translate());
				if (num > 0f)
				{
					if (req.HasThing)
					{
						float statValue = req.Thing.GetStatValue(StatDefOf.RangedWeapon_WarmupMultiplier, true, -1);
						num *= statValue;
						if (!Mathf.Approximately(statValue, 1f))
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine("Stat_Thing_Weapon_WarmupTime_Multiplier".Translate() + ": x" + statValue.ToStringPercent());
							stringBuilder.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_WarmupMultiplier, req.Thing));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToString("0.##") + " " + "LetterSecond".Translate());
					yield return new StatDrawEntry(verbStatCategory ?? StatCategoryDefOf.Weapon_Ranged, "RangedWarmupTime".Translate(), num.ToString("0.##") + " " + "LetterSecond".Translate(), stringBuilder.ToString(), 3555, null, null, false, false);
				}
				ThingDef defaultProjectile = verb.defaultProjectile;
				if (((defaultProjectile != null) ? defaultProjectile.projectile.damageDef : null) != null && verb.defaultProjectile.projectile.damageDef.harmsHealth)
				{
					StatCategoryDef statCat = verbStatCategory ?? StatCategoryDefOf.Weapon_Ranged;
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendLine("Stat_Thing_Damage_Desc".Translate());
					stringBuilder2.AppendLine();
					float num2 = (float)verb.defaultProjectile.projectile.GetDamageAmount(req.Thing, stringBuilder2);
					yield return new StatDrawEntry(statCat, "Damage".Translate(), num2.ToString(), stringBuilder2.ToString(), 5500, null, null, false, false);
					if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
					{
						StringBuilder stringBuilder3 = new StringBuilder();
						float armorPenetration = verb.defaultProjectile.projectile.GetArmorPenetration(req.Thing, stringBuilder3);
						TaggedString taggedString = "ArmorPenetrationExplanation".Translate();
						if (stringBuilder3.Length != 0)
						{
							TaggedString t = taggedString;
							string str = "\n\n";
							StringBuilder stringBuilder4 = stringBuilder3;
							taggedString = t + (str + ((stringBuilder4 != null) ? stringBuilder4.ToString() : null));
						}
						yield return new StatDrawEntry(statCat, "ArmorPenetration".Translate(), armorPenetration.ToStringPercent(), taggedString, 5400, null, null, false, false);
					}
					float buildingDamageFactor = verb.defaultProjectile.projectile.damageDef.buildingDamageFactor;
					float dmgBuildingsImpassable = verb.defaultProjectile.projectile.damageDef.buildingDamageFactorImpassable;
					float dmgBuildingsPassable = verb.defaultProjectile.projectile.damageDef.buildingDamageFactorPassable;
					if (buildingDamageFactor != 1f)
					{
						yield return new StatDrawEntry(statCat, "BuildingDamageFactor".Translate(), buildingDamageFactor.ToStringPercent(), "BuildingDamageFactorExplanation".Translate(), 5410, null, null, false, false);
					}
					if (dmgBuildingsImpassable != 1f)
					{
						yield return new StatDrawEntry(statCat, "BuildingDamageFactorImpassable".Translate(), dmgBuildingsImpassable.ToStringPercent(), "BuildingDamageFactorImpassableExplanation".Translate(), 5420, null, null, false, false);
					}
					if (dmgBuildingsPassable != 1f)
					{
						yield return new StatDrawEntry(statCat, "BuildingDamageFactorPassable".Translate(), dmgBuildingsPassable.ToStringPercent(), "BuildingDamageFactorPassableExplanation".Translate(), 5430, null, null, false, false);
					}
					statCat = null;
				}
				if (verb.defaultProjectile == null && verb.beamDamageDef != null)
				{
					yield return new StatDrawEntry(verbStatCategory ?? StatCategoryDefOf.Weapon_Ranged, "ArmorPenetration".Translate(), verb.beamDamageDef.defaultArmorPenetration.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false, false);
				}
				if (verb.Ranged)
				{
					float num3 = (float)verb.burstShotCount;
					float num4 = (float)verb.ticksBetweenBurstShots;
					VerbProperties verbProperties = verb;
					float? num5;
					if (verbProperties == null)
					{
						num5 = null;
					}
					else
					{
						ThingDef defaultProjectile2 = verbProperties.defaultProjectile;
						if (defaultProjectile2 == null)
						{
							num5 = null;
						}
						else
						{
							ProjectileProperties projectileProperties = defaultProjectile2.projectile;
							num5 = ((projectileProperties != null) ? new float?(projectileProperties.stoppingPower) : null);
						}
					}
					float? num6 = num5;
					float dmgBuildingsPassable = num6.GetValueOrDefault();
					StringBuilder stringBuilder5 = new StringBuilder("Stat_Thing_Weapon_BurstShotFireRate_Desc".Translate());
					stringBuilder5.AppendLine();
					stringBuilder5.AppendLine();
					stringBuilder5.AppendLine("StatsReport_BaseValue".Translate() + ": " + verb.burstShotCount.ToString());
					stringBuilder5.AppendLine();
					StringBuilder ticksBetweenBurstShotsExplanation = new StringBuilder("Stat_Thing_Weapon_BurstShotFireRate_Desc".Translate());
					ticksBetweenBurstShotsExplanation.AppendLine();
					ticksBetweenBurstShotsExplanation.AppendLine();
					ticksBetweenBurstShotsExplanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + (60f / verb.ticksBetweenBurstShots.TicksToSeconds()).ToString("0.##") + " rpm");
					ticksBetweenBurstShotsExplanation.AppendLine();
					StringBuilder stoppingPowerExplanation = new StringBuilder("StoppingPowerExplanation".Translate());
					stoppingPowerExplanation.AppendLine();
					stoppingPowerExplanation.AppendLine();
					stoppingPowerExplanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + dmgBuildingsPassable.ToString("F1"));
					stoppingPowerExplanation.AppendLine();
					CompUniqueWeapon compUniqueWeapon;
					if (req.HasThing && req.Thing.TryGetComp(out compUniqueWeapon))
					{
						bool flag = false;
						bool flag2 = false;
						bool flag3 = false;
						foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
						{
							if (!Mathf.Approximately(weaponTraitDef.burstShotCountMultiplier, 1f))
							{
								if (!flag)
								{
									stringBuilder5.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
									flag = true;
								}
								num3 *= weaponTraitDef.burstShotCountMultiplier;
								stringBuilder5.AppendLine("    " + weaponTraitDef.LabelCap + ": " + weaponTraitDef.burstShotCountMultiplier.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
							}
							if (!Mathf.Approximately(weaponTraitDef.burstShotSpeedMultiplier, 1f))
							{
								if (!flag2)
								{
									ticksBetweenBurstShotsExplanation.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
									flag2 = true;
								}
								num4 /= weaponTraitDef.burstShotSpeedMultiplier;
								ticksBetweenBurstShotsExplanation.AppendLine("    " + weaponTraitDef.LabelCap + ": " + weaponTraitDef.burstShotSpeedMultiplier.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
							}
							if (!Mathf.Approximately(weaponTraitDef.additionalStoppingPower, 0f))
							{
								if (!flag3)
								{
									stoppingPowerExplanation.AppendLine("StatsReport_WeaponTraits".Translate() + ":");
									flag3 = true;
								}
								dmgBuildingsPassable += weaponTraitDef.additionalStoppingPower;
								stoppingPowerExplanation.AppendLine("    " + weaponTraitDef.LabelCap + ": " + weaponTraitDef.additionalStoppingPower.ToStringByStyle(ToStringStyle.FloatOne, ToStringNumberSense.Offset));
							}
						}
					}
					stringBuilder5.AppendLine();
					stringBuilder5.AppendLine("StatsReport_FinalValue".Translate() + ": " + Mathf.CeilToInt(num3).ToString());
					float dmgBuildingsImpassable = 60f / ((int)num4).TicksToSeconds();
					ticksBetweenBurstShotsExplanation.AppendLine();
					ticksBetweenBurstShotsExplanation.AppendLine("StatsReport_FinalValue".Translate() + ": " + dmgBuildingsImpassable.ToString("0.##") + " rpm");
					stoppingPowerExplanation.AppendLine();
					stoppingPowerExplanation.AppendLine("StatsReport_FinalValue".Translate() + ": " + dmgBuildingsPassable.ToString("F1"));
					StatCategoryDef statCat = verbStatCategory ?? StatCategoryDefOf.Weapon_Ranged;
					if (verb.showBurstShotStats && verb.burstShotCount > 1)
					{
						yield return new StatDrawEntry(statCat, "BurstShotCount".Translate(), Mathf.CeilToInt(num3).ToString(), stringBuilder5.ToString(), 5391, null, null, false, false);
						yield return new StatDrawEntry(statCat, "BurstShotFireRate".Translate(), dmgBuildingsImpassable.ToString("0.##") + " rpm", ticksBetweenBurstShotsExplanation.ToString(), 5395, null, null, false, false);
					}
					if (dmgBuildingsPassable > 0f)
					{
						yield return new StatDrawEntry(statCat, "StoppingPower".Translate(), dmgBuildingsPassable.ToString("F1"), stoppingPowerExplanation.ToString(), 5402, null, null, false, false);
					}
					float num7 = verb.range;
					StringBuilder stringBuilder6 = new StringBuilder("Stat_Thing_Weapon_Range_Desc".Translate());
					stringBuilder6.AppendLine();
					stringBuilder6.AppendLine();
					stringBuilder6.AppendLine("StatsReport_BaseValue".Translate() + ": " + num7.ToString("F0"));
					if (req.HasThing)
					{
						float statValue2 = req.Thing.GetStatValue(StatDefOf.RangedWeapon_RangeMultiplier, true, -1);
						num7 *= statValue2;
						if (!Mathf.Approximately(statValue2, 1f))
						{
							stringBuilder6.AppendLine();
							stringBuilder6.AppendLine("Stat_Thing_Weapon_Range_Multiplier".Translate() + ": x" + statValue2.ToStringPercent());
							stringBuilder6.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_RangeMultiplier, req.Thing));
						}
						Map map = req.Thing.Map ?? req.Thing.MapHeld;
						if (map != null && map.weatherManager.CurWeatherMaxRangeCap >= 0f)
						{
							WeatherManager weatherManager = (req.Thing.Map ?? req.Thing.MapHeld).weatherManager;
							bool flag4 = num7 > weatherManager.CurWeatherMaxRangeCap;
							float num8 = num7;
							num7 = Mathf.Min(num7, weatherManager.CurWeatherMaxRangeCap);
							if (flag4)
							{
								stringBuilder6.AppendLine();
								stringBuilder6.AppendLine("    " + "Stat_Thing_Weapon_Range_Clamped".Translate(num7.ToString("F0").Named("CAP"), num8.ToString("F0").Named("ORIGINAL")));
							}
						}
					}
					stringBuilder6.AppendLine();
					stringBuilder6.AppendLine("StatsReport_FinalValue".Translate() + ": " + num7.ToString("F0"));
					yield return new StatDrawEntry(statCat, "Range".Translate(), num7.ToString("F0"), stringBuilder6.ToString(), 5390, null, null, false, false);
					ticksBetweenBurstShotsExplanation = null;
					stoppingPowerExplanation = null;
					statCat = null;
				}
				if (verb.ForcedMissRadius > 0f)
				{
					StatCategoryDef statCat = verbStatCategory ?? StatCategoryDefOf.Weapon_Ranged;
					yield return new StatDrawEntry(statCat, "MissRadius".Translate(), verb.ForcedMissRadius.ToString("0.#"), "Stat_Thing_Weapon_MissRadius_Desc".Translate(), 3557, null, null, false, false);
					yield return new StatDrawEntry(statCat, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(verb.ForcedMissRadius)).ToStringPercent(), "Stat_Thing_Weapon_DirectHitChance_Desc".Translate(), 3560, null, null, false, false);
					statCat = null;
				}
				verb = null;
				verbStatCategory = null;
			}
			if (this.plant != null)
			{
				foreach (StatDrawEntry statDrawEntry3 in this.plant.SpecialDisplayStats())
				{
					yield return statDrawEntry3;
				}
				enumerator = null;
			}
			if (this.ingestible != null)
			{
				foreach (StatDrawEntry statDrawEntry4 in this.ingestible.SpecialDisplayStats())
				{
					yield return statDrawEntry4;
				}
				enumerator = null;
			}
			if (this.race != null)
			{
				foreach (StatDrawEntry statDrawEntry5 in this.race.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry5;
				}
				enumerator = null;
			}
			if (this.building != null)
			{
				foreach (StatDrawEntry statDrawEntry6 in this.building.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry6;
				}
				enumerator = null;
			}
			if (this.isTechHediff)
			{
				IEnumerable<RecipeDef> enumerable2 = from x in DefDatabase<RecipeDef>.AllDefs
				where x.addsHediff != null && x.IsIngredient(this)
				select x;
				foreach (StatDrawEntry statDrawEntry7 in MedicalRecipesUtility.GetMedicalStatsFromRecipeDefs(enumerable2))
				{
					yield return statDrawEntry7;
				}
				enumerator = null;
			}
			int num9;
			for (int i = 0; i < this.comps.Count; i = num9 + 1)
			{
				foreach (StatDrawEntry statDrawEntry8 in this.comps[i].SpecialDisplayStats(req))
				{
					yield return statDrawEntry8;
				}
				enumerator = null;
				num9 = i;
			}
			if (this.building != null)
			{
				if (this.building.mineableThing != null)
				{
					Dialog_InfoCard.Hyperlink[] hyperlinks = new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(this.building.mineableThing, -1)
					};
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_MineableThing_Name".Translate(), this.building.mineableThing.LabelCap, "Stat_MineableThing_Desc".Translate(), 2200, null, hyperlinks, false, false);
					StringBuilder stringBuilder7 = new StringBuilder();
					stringBuilder7.AppendLine("Stat_MiningYield_Desc".Translate());
					stringBuilder7.AppendLine();
					stringBuilder7.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_MiningYield_Name".Translate(), Mathf.CeilToInt((float)this.building.EffectiveMineableYield).ToString("F0"), stringBuilder7.ToString(), 2200, null, hyperlinks, false, false);
					hyperlinks = null;
				}
				if (this.building.IsTurret)
				{
					ThingDef turret = this.building.turretGunDef;
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Weapon_Name".Translate(), turret.LabelCap, "Stat_Weapon_Desc".Translate(), 5389, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(turret, -1)
					}, false, false);
					StatRequest request = StatRequest.For(turret, null, QualityCategory.Normal);
					foreach (StatDrawEntry statDrawEntry9 in turret.SpecialDisplayStats(request))
					{
						if (statDrawEntry9.category == StatCategoryDefOf.Weapon_Ranged)
						{
							yield return statDrawEntry9;
						}
					}
					enumerator = null;
					for (int i = 0; i < turret.statBases.Count; i = num9 + 1)
					{
						StatModifier statModifier = turret.statBases[i];
						if (statModifier.stat.category == StatCategoryDefOf.Weapon_Ranged)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Weapon_Ranged, statModifier.stat, statModifier.value, request, ToStringNumberSense.Undefined, null, false);
						}
						num9 = i;
					}
					turret = null;
					request = default(StatRequest);
				}
				if (ModsConfig.OdysseyActive && this.Fillage == FillCategory.Full)
				{
					bool b = this.building.isAirtight || (this.building.isStuffableAirtight && req.StuffDef.stuffProps.isAirtight);
					yield return new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Airtight".Translate(), b.ToStringYesNo(), "Stat_Airtight_Desc".Translate(), 6100, null, null, false, false);
				}
			}
			if (this.IsMeat)
			{
				List<ThingDef> list = new List<ThingDef>();
				bool flag5 = false;
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.race != null && thingDef.race.meatDef == this && !thingDef.IsCorpse)
					{
						if (!Find.HiddenItemsManager.Hidden(thingDef))
						{
							flag5 = true;
						}
						list.Add(thingDef);
					}
				}
				string valueString;
				if (flag5)
				{
					valueString = string.Join(", ", (from x in list
					where !Find.HiddenItemsManager.Hidden(x)
					select x into p
					select p.label).ToArray<string>()).CapitalizeFirst();
				}
				else
				{
					valueString = string.Format("({0})", "NotYetDiscovered".Translate());
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list), false, false);
			}
			if (this.IsLeather)
			{
				List<ThingDef> list2 = new List<ThingDef>();
				bool flag6 = false;
				foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef2.race != null && thingDef2.race.leatherDef == this && !thingDef2.IsCorpse)
					{
						if (!Find.HiddenItemsManager.Hidden(thingDef2))
						{
							flag6 = true;
						}
						list2.Add(thingDef2);
					}
				}
				string valueString2;
				if (flag6)
				{
					valueString2 = string.Join(", ", (from x in list2
					where !Find.HiddenItemsManager.Hidden(x)
					select x into p
					select p.label).ToArray<string>()).CapitalizeFirst();
				}
				else
				{
					valueString2 = string.Format("({0})", "NotYetDiscovered".Translate());
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString2, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list2), false, false);
			}
			if (!this.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.equippedStatOffsets.Count; i = num9 + 1)
				{
					StatDef stat = this.equippedStatOffsets[i].stat;
					float num10 = this.equippedStatOffsets[i].value;
					StringBuilder stringBuilder8 = new StringBuilder(stat.description);
					if (req.HasThing && stat.Worker != null)
					{
						stringBuilder8.AppendLine();
						stringBuilder8.AppendLine();
						stringBuilder8.AppendLine("StatsReport_BaseValue".Translate() + ": " + stat.ValueToString(num10, ToStringNumberSense.Offset, stat.finalizeEquippedStatOffset));
						num10 = StatWorker.StatOffsetFromGear(req.Thing, stat);
						if (!stat.parts.NullOrEmpty<StatPart>())
						{
							stringBuilder8.AppendLine();
							for (int k = 0; k < stat.parts.Count; k++)
							{
								string text = stat.parts[k].ExplanationPart(req);
								if (!text.NullOrEmpty())
								{
									stringBuilder8.AppendLine(text);
								}
							}
						}
						stringBuilder8.AppendLine();
						stringBuilder8.AppendLine("StatsReport_FinalValue".Translate() + ": " + stat.ValueToString(num10, ToStringNumberSense.Offset, !stat.formatString.NullOrEmpty()));
					}
					yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, this.equippedStatOffsets[i].stat, num10, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, true).SetReportText(stringBuilder8.ToString());
					num9 = i;
				}
			}
			if (this.IsDrug)
			{
				foreach (StatDrawEntry statDrawEntry10 in DrugStatsUtility.SpecialDisplayStats(this))
				{
					yield return statDrawEntry10;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04001162 RID: 4450
		public Type thingClass;

		// Token: 0x04001163 RID: 4451
		public ThingCategory category;

		// Token: 0x04001164 RID: 4452
		public TickerType tickerType;

		// Token: 0x04001165 RID: 4453
		public int stackLimit = 1;

		// Token: 0x04001166 RID: 4454
		public IntVec2 size = IntVec2.One;

		// Token: 0x04001167 RID: 4455
		public bool destroyable = true;

		// Token: 0x04001168 RID: 4456
		public bool rotatable = true;

		// Token: 0x04001169 RID: 4457
		public bool smallVolume;

		// Token: 0x0400116A RID: 4458
		public bool useHitPoints = true;

		// Token: 0x0400116B RID: 4459
		public bool receivesSignals;

		// Token: 0x0400116C RID: 4460
		public List<CompProperties> comps = new List<CompProperties>();

		// Token: 0x0400116D RID: 4461
		public List<ThingDef> virtualDefs = new List<ThingDef>();

		// Token: 0x0400116E RID: 4462
		public ThingDef virtualDefParent;

		// Token: 0x0400116F RID: 4463
		[NoTranslate]
		public string devNote;

		// Token: 0x04001170 RID: 4464
		public List<ThingDefCountRangeClass> killedLeavingsRanges;

		// Token: 0x04001171 RID: 4465
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x04001172 RID: 4466
		public List<ThingDefCountClass> killedLeavingsPlayerHostile;

		// Token: 0x04001173 RID: 4467
		public float killedLeavingsChance = 1f;

		// Token: 0x04001174 RID: 4468
		public bool forceLeavingsAllowed;

		// Token: 0x04001175 RID: 4469
		public List<ThingDefCountClass> butcherProducts;

		// Token: 0x04001176 RID: 4470
		public List<ThingDefCountClass> smeltProducts;

		// Token: 0x04001177 RID: 4471
		public bool smeltable;

		// Token: 0x04001178 RID: 4472
		public bool burnableByRecipe;

		// Token: 0x04001179 RID: 4473
		public bool randomizeRotationOnSpawn;

		// Token: 0x0400117A RID: 4474
		public List<DamageMultiplier> damageMultipliers;

		// Token: 0x0400117B RID: 4475
		public bool isTechHediff;

		// Token: 0x0400117C RID: 4476
		public RecipeMakerProperties recipeMaker;

		// Token: 0x0400117D RID: 4477
		public ThingDef minifiedDef;

		// Token: 0x0400117E RID: 4478
		public bool isUnfinishedThing;

		// Token: 0x0400117F RID: 4479
		public bool leaveResourcesWhenKilled;

		// Token: 0x04001180 RID: 4480
		public ThingDef slagDef;

		// Token: 0x04001181 RID: 4481
		public bool isFrameInt;

		// Token: 0x04001182 RID: 4482
		public List<IntVec3> multipleInteractionCellOffsets;

		// Token: 0x04001183 RID: 4483
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		// Token: 0x04001184 RID: 4484
		public bool hasInteractionCell;

		// Token: 0x04001185 RID: 4485
		public ThingDef interactionCellIcon;

		// Token: 0x04001186 RID: 4486
		public bool interactionCellIconReverse;

		// Token: 0x04001187 RID: 4487
		public ThingDef filthLeaving;

		// Token: 0x04001188 RID: 4488
		public bool forceDebugSpawnable;

		// Token: 0x04001189 RID: 4489
		public bool intricate;

		// Token: 0x0400118A RID: 4490
		public bool scatterableOnMapGen = true;

		// Token: 0x0400118B RID: 4491
		public float deepCommonality;

		// Token: 0x0400118C RID: 4492
		public int deepCountPerCell = 300;

		// Token: 0x0400118D RID: 4493
		public int deepCountPerPortion = -1;

		// Token: 0x0400118E RID: 4494
		public IntRange deepLumpSizeRange = IntRange.Zero;

		// Token: 0x0400118F RID: 4495
		public float generateCommonality = 1f;

		// Token: 0x04001190 RID: 4496
		public float generateAllowChance = 1f;

		// Token: 0x04001191 RID: 4497
		private bool canOverlapZones = true;

		// Token: 0x04001192 RID: 4498
		public FloatRange startingHpRange = FloatRange.One;

		// Token: 0x04001193 RID: 4499
		[NoTranslate]
		public List<string> thingSetMakerTags;

		// Token: 0x04001194 RID: 4500
		public bool alwaysFlee;

		// Token: 0x04001195 RID: 4501
		public List<RecipeDef> recipes;

		// Token: 0x04001196 RID: 4502
		public bool messageOnDeteriorateInStorage = true;

		// Token: 0x04001197 RID: 4503
		public bool deteriorateFromEnvironmentalEffects = true;

		// Token: 0x04001198 RID: 4504
		public bool canDeteriorateUnspawned;

		// Token: 0x04001199 RID: 4505
		public bool canLoadIntoCaravan = true;

		// Token: 0x0400119A RID: 4506
		public bool isMechClusterThreat;

		// Token: 0x0400119B RID: 4507
		public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

		// Token: 0x0400119C RID: 4508
		public int minRewardCount = 1;

		// Token: 0x0400119D RID: 4509
		public bool preventSkyfallersLandingOn;

		// Token: 0x0400119E RID: 4510
		public FactionDef requiresFactionToAcquire;

		// Token: 0x0400119F RID: 4511
		public float relicChance;

		// Token: 0x040011A0 RID: 4512
		public OrderedTakeGroupDef orderedTakeGroup;

		// Token: 0x040011A1 RID: 4513
		public int allowedArchonexusCount;

		// Token: 0x040011A2 RID: 4514
		public int possessionCount;

		// Token: 0x040011A3 RID: 4515
		public bool notifyMapRemoved;

		// Token: 0x040011A4 RID: 4516
		public bool canScatterOver = true;

		// Token: 0x040011A5 RID: 4517
		public bool genericMarketSellable = true;

		// Token: 0x040011A6 RID: 4518
		public bool drawHighlight;

		// Token: 0x040011A7 RID: 4519
		public Color? highlightColor;

		// Token: 0x040011A8 RID: 4520
		public bool drawHighlightOnlyForHostile;

		// Token: 0x040011A9 RID: 4521
		public bool autoTargetNearbyIdenticalThings;

		// Token: 0x040011AA RID: 4522
		public bool preventDroppingThingsOn;

		// Token: 0x040011AB RID: 4523
		public bool hiddenWhileUndiscovered;

		// Token: 0x040011AC RID: 4524
		public bool disableImpassableShotOverConfigError;

		// Token: 0x040011AD RID: 4525
		public bool showInSearch = true;

		// Token: 0x040011AE RID: 4526
		public bool bringAlongOnGravship = true;

		// Token: 0x040011AF RID: 4527
		public ThingDef dropPodFaller;

		// Token: 0x040011B0 RID: 4528
		public ThingDef dropPodActive;

		// Token: 0x040011B1 RID: 4529
		public bool preventSpawningInResourcePod;

		// Token: 0x040011B2 RID: 4530
		public bool pathfinderDangerous;

		// Token: 0x040011B3 RID: 4531
		public bool noRightClickDraftAttack;

		// Token: 0x040011B4 RID: 4532
		public int gravshipSpawnPriority = 1;

		// Token: 0x040011B5 RID: 4533
		public List<string> replaceTags;

		// Token: 0x040011B6 RID: 4534
		public bool preventGravshipLandingOn;

		// Token: 0x040011B7 RID: 4535
		public bool canInteractThroughCorners;

		// Token: 0x040011B8 RID: 4536
		public GraphicData graphicData;

		// Token: 0x040011B9 RID: 4537
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		// Token: 0x040011BA RID: 4538
		public bool drawOffscreen;

		// Token: 0x040011BB RID: 4539
		public ColorGenerator colorGenerator;

		// Token: 0x040011BC RID: 4540
		public float hideAtSnowOrSandDepth = 99999f;

		// Token: 0x040011BD RID: 4541
		public bool drawDamagedOverlay = true;

		// Token: 0x040011BE RID: 4542
		public bool castEdgeShadows;

		// Token: 0x040011BF RID: 4543
		public float staticSunShadowHeight;

		// Token: 0x040011C0 RID: 4544
		public bool useSameGraphicForGhost;

		// Token: 0x040011C1 RID: 4545
		public bool useBlueprintGraphicAsGhost;

		// Token: 0x040011C2 RID: 4546
		public List<ThingStyleChance> randomStyle;

		// Token: 0x040011C3 RID: 4547
		public float randomStyleChance;

		// Token: 0x040011C4 RID: 4548
		public bool canEditAnyStyle;

		// Token: 0x040011C5 RID: 4549
		public bool dontPrint;

		// Token: 0x040011C6 RID: 4550
		public ThingDef defaultStuff;

		// Token: 0x040011C7 RID: 4551
		public int killedLeavingsExpandRect;

		// Token: 0x040011C8 RID: 4552
		public bool minifiedManualDraw;

		// Token: 0x040011C9 RID: 4553
		public float minifiedDrawScale = 1f;

		// Token: 0x040011CA RID: 4554
		public Rot4 overrideMinifiedRot = Rot4.Invalid;

		// Token: 0x040011CB RID: 4555
		public Vector3 minifiedDrawOffset = Vector3.zero;

		// Token: 0x040011CC RID: 4556
		public float deselectedSelectionBracketFactor = 1f;

		// Token: 0x040011CD RID: 4557
		public bool selectable;

		// Token: 0x040011CE RID: 4558
		public bool containedPawnsSelectable;

		// Token: 0x040011CF RID: 4559
		public bool containedItemsSelectable;

		// Token: 0x040011D0 RID: 4560
		public bool neverMultiSelect;

		// Token: 0x040011D1 RID: 4561
		public bool isAutoAttackableMapObject;

		// Token: 0x040011D2 RID: 4562
		public bool hasTooltip;

		// Token: 0x040011D3 RID: 4563
		public List<Type> inspectorTabs;

		// Token: 0x040011D4 RID: 4564
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x040011D5 RID: 4565
		public bool seeThroughFog;

		// Token: 0x040011D6 RID: 4566
		public bool drawGUIOverlay;

		// Token: 0x040011D7 RID: 4567
		public bool drawGUIOverlayQuality = true;

		// Token: 0x040011D8 RID: 4568
		public ResourceCountPriority resourceReadoutPriority;

		// Token: 0x040011D9 RID: 4569
		public bool resourceReadoutAlwaysShow;

		// Token: 0x040011DA RID: 4570
		public bool drawPlaceWorkersWhileSelected;

		// Token: 0x040011DB RID: 4571
		public bool drawPlaceWorkersWhileInstallBlueprintSelected;

		// Token: 0x040011DC RID: 4572
		public ConceptDef storedConceptLearnOpportunity;

		// Token: 0x040011DD RID: 4573
		public float uiIconScale = 1f;

		// Token: 0x040011DE RID: 4574
		public bool hasCustomRectForSelector;

		// Token: 0x040011DF RID: 4575
		public bool hideStats;

		// Token: 0x040011E0 RID: 4576
		public bool hideInspect;

		// Token: 0x040011E1 RID: 4577
		public bool onlyShowInspectString;

		// Token: 0x040011E2 RID: 4578
		public bool hideMainDesc;

		// Token: 0x040011E3 RID: 4579
		public bool alwaysHaulable;

		// Token: 0x040011E4 RID: 4580
		public bool designateHaulable;

		// Token: 0x040011E5 RID: 4581
		public List<ThingCategoryDef> thingCategories;

		// Token: 0x040011E6 RID: 4582
		public bool mineable;

		// Token: 0x040011E7 RID: 4583
		public bool socialPropernessMatters;

		// Token: 0x040011E8 RID: 4584
		public bool stealable = true;

		// Token: 0x040011E9 RID: 4585
		public SoundDef soundSpawned;

		// Token: 0x040011EA RID: 4586
		public SoundDef soundDrop;

		// Token: 0x040011EB RID: 4587
		public SoundDef soundPickup;

		// Token: 0x040011EC RID: 4588
		public SoundDef soundInteract;

		// Token: 0x040011ED RID: 4589
		public SoundDef soundImpactDefault;

		// Token: 0x040011EE RID: 4590
		public SoundDef soundPlayInstrument;

		// Token: 0x040011EF RID: 4591
		public SoundDef soundOpen;

		// Token: 0x040011F0 RID: 4592
		public bool saveCompressible;

		// Token: 0x040011F1 RID: 4593
		public bool isSaveable = true;

		// Token: 0x040011F2 RID: 4594
		public bool holdsRoof;

		// Token: 0x040011F3 RID: 4595
		public float fillPercent;

		// Token: 0x040011F4 RID: 4596
		public bool coversFloor;

		// Token: 0x040011F5 RID: 4597
		public bool neverOverlapFloors;

		// Token: 0x040011F6 RID: 4598
		public SurfaceType surfaceType;

		// Token: 0x040011F7 RID: 4599
		public bool wipesPlants;

		// Token: 0x040011F8 RID: 4600
		public bool blockPlants;

		// Token: 0x040011F9 RID: 4601
		public bool blockLight;

		// Token: 0x040011FA RID: 4602
		public bool blockWind;

		// Token: 0x040011FB RID: 4603
		public bool blockWeather;

		// Token: 0x040011FC RID: 4604
		public Tradeability tradeability = Tradeability.All;

		// Token: 0x040011FD RID: 4605
		[NoTranslate]
		public List<string> tradeTags;

		// Token: 0x040011FE RID: 4606
		public bool tradeNeverStack;

		// Token: 0x040011FF RID: 4607
		public bool tradeNeverGenerateStacked;

		// Token: 0x04001200 RID: 4608
		public bool healthAffectsPrice = true;

		// Token: 0x04001201 RID: 4609
		public ColorGenerator colorGeneratorInTraderStock;

		// Token: 0x04001202 RID: 4610
		private List<VerbProperties> verbs;

		// Token: 0x04001203 RID: 4611
		public List<Tool> tools;

		// Token: 0x04001204 RID: 4612
		public float equippedAngleOffset;

		// Token: 0x04001205 RID: 4613
		public float equippedDistanceOffset;

		// Token: 0x04001206 RID: 4614
		public EquipmentType equipmentType;

		// Token: 0x04001207 RID: 4615
		public TechLevel techLevel;

		// Token: 0x04001208 RID: 4616
		public List<WeaponClassDef> weaponClasses;

		// Token: 0x04001209 RID: 4617
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x0400120A RID: 4618
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x0400120B RID: 4619
		public bool violentTechHediff;

		// Token: 0x0400120C RID: 4620
		public bool destroyOnDrop;

		// Token: 0x0400120D RID: 4621
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x0400120E RID: 4622
		public SoundDef meleeHitSound;

		// Token: 0x0400120F RID: 4623
		public float recoilPower = 1f;

		// Token: 0x04001210 RID: 4624
		public float recoilRelaxation = 10f;

		// Token: 0x04001211 RID: 4625
		public bool rotateInShelves = true;

		// Token: 0x04001212 RID: 4626
		public bool mergeVerbGizmos = true;

		// Token: 0x04001213 RID: 4627
		public BuildableDef entityDefToBuild;

		// Token: 0x04001214 RID: 4628
		public ThingDef projectileWhenLoaded;

		// Token: 0x04001215 RID: 4629
		public RulePackDef ideoBuildingNamerBase;

		// Token: 0x04001216 RID: 4630
		public EntityCodexEntryDef entityCodexEntry;

		// Token: 0x04001217 RID: 4631
		public IngestibleProperties ingestible;

		// Token: 0x04001218 RID: 4632
		public FilthProperties filth;

		// Token: 0x04001219 RID: 4633
		public GasProperties gas;

		// Token: 0x0400121A RID: 4634
		public BuildingProperties building;

		// Token: 0x0400121B RID: 4635
		public RaceProperties race;

		// Token: 0x0400121C RID: 4636
		public ApparelProperties apparel;

		// Token: 0x0400121D RID: 4637
		public MoteProperties mote;

		// Token: 0x0400121E RID: 4638
		public PlantProperties plant;

		// Token: 0x0400121F RID: 4639
		public ProjectileProperties projectile;

		// Token: 0x04001220 RID: 4640
		public StuffProperties stuffProps;

		// Token: 0x04001221 RID: 4641
		public SkyfallerProperties skyfaller;

		// Token: 0x04001222 RID: 4642
		public PawnFlyerProperties pawnFlyer;

		// Token: 0x04001223 RID: 4643
		public RitualFocusProperties ritualFocus;

		// Token: 0x04001224 RID: 4644
		public IngredientProperties ingredient;

		// Token: 0x04001225 RID: 4645
		public MapPortalProperties portal;

		// Token: 0x04001226 RID: 4646
		public bool canBeUsedUnderRoof = true;

		// Token: 0x04001227 RID: 4647
		[Unsaved(false)]
		private string descriptionDetailedCached;

		// Token: 0x04001228 RID: 4648
		[Unsaved(false)]
		public Graphic interactionCellGraphic;

		// Token: 0x04001229 RID: 4649
		[Unsaved(false)]
		private bool? isNaturalOrganCached;

		// Token: 0x0400122A RID: 4650
		[Unsaved(false)]
		private bool? hasSunShadowsCached;

		// Token: 0x0400122B RID: 4651
		[Unsaved(false)]
		private List<StyleCategoryDef> cachedRelevantStyleCategories;

		// Token: 0x0400122C RID: 4652
		public const int SmallUnitPerVolume = 10;

		// Token: 0x0400122D RID: 4653
		public const float SmallVolumePerUnit = 0.1f;

		// Token: 0x0400122E RID: 4654
		public const float ArchonexusMaxItemStackMass = 5f;

		// Token: 0x0400122F RID: 4655
		public const int ArchonexusMaxItemStackCount = 25;

		// Token: 0x04001230 RID: 4656
		public const float ArchonexusMaxItemStackValue = 2000f;

		// Token: 0x04001231 RID: 4657
		public const int ArchonexusAutoCalculateValue = -1;

		// Token: 0x04001232 RID: 4658
		private List<RecipeDef> allRecipesCached;

		// Token: 0x04001233 RID: 4659
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		// Token: 0x04001234 RID: 4660
		private Dictionary<ThingDef, Thing> concreteExamplesInt;
	}
}
