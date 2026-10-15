using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class FactionDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__150 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FactionDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__150(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FactionDef CS_0024_003C_003E8__locals18 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)CS_0024_003C_003E8__locals18).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_008e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_008e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d8;
				case 3:
					_003C_003E1__state = -1;
					goto IL_010b;
				case 4:
					_003C_003E1__state = -1;
					goto IL_014b;
				case 5:
					_003C_003E1__state = -1;
					goto IL_017b;
				case 6:
					_003C_003E1__state = -1;
					goto IL_01ab;
				case 7:
					_003C_003E1__state = -1;
					goto IL_01f9;
				case 8:
					_003C_003E1__state = -1;
					goto IL_021e;
				case 9:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_008e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals18.pawnGroupMakers != null && CS_0024_003C_003E8__locals18.maxPawnCostPerTotalPointsCurve == null)
					{
						_003C_003E2__current = "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d8;
					IL_014b:
					if (CS_0024_003C_003E8__locals18.permanentEnemy && CS_0024_003C_003E8__locals18.mustStartOneEnemy)
					{
						_003C_003E2__current = "permanentEnemy has mustStartOneEnemy = true, which is redundant";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_017b;
					IL_017b:
					if (CS_0024_003C_003E8__locals18.disallowedMemes != null && CS_0024_003C_003E8__locals18.allowedMemes != null)
					{
						_003C_003E2__current = "both disallowedMemes (black list) and allowedMemes (white list) are defined";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_01ab;
					IL_00d8:
					if (CS_0024_003C_003E8__locals18.techLevel == TechLevel.Undefined)
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals18.defName + " has no tech level.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_010b;
					IL_01f9:
					if (CS_0024_003C_003E8__locals18.raidLootValueFromPointsCurve == null)
					{
						_003C_003E2__current = "raidLootValueFromPointsCurve must be defined";
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_021e;
					IL_010b:
					if (CS_0024_003C_003E8__locals18.humanlikeFaction && CS_0024_003C_003E8__locals18.backstoryFilters.NullOrEmpty())
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals18.defName + " is humanlikeFaction but has no backstory categories.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_014b;
					IL_021e:
					if (CS_0024_003C_003E8__locals18.dropPodActive == null != (CS_0024_003C_003E8__locals18.dropPodIncoming == null))
					{
						_003C_003E2__current = "Both drop pod and drop pod incoming must be defined or both must be undefined";
						_003C_003E1__state = 9;
						return true;
					}
					break;
					IL_01ab:
					if (CS_0024_003C_003E8__locals18.requiredMemes != null)
					{
						MemeDef memeDef = CS_0024_003C_003E8__locals18.requiredMemes.FirstOrDefault((MemeDef x) => !IdeoUtility.IsMemeAllowedFor(x, CS_0024_003C_003E8__locals18));
						if (memeDef != null)
						{
							_003C_003E2__current = "has a required meme which is not allowed: " + memeDef.defName;
							_003C_003E1__state = 7;
							return true;
						}
					}
					goto IL_01f9;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__150 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__150(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public bool isPlayer;

	public RulePackDef factionNameMaker;

	public RulePackDef settlementNameMaker;

	public RulePackDef playerInitialSettlementNameMaker;

	[MustTranslate]
	public string fixedName;

	public bool humanlikeFaction = true;

	public bool hidden;

	public float listOrderPriority;

	public List<PawnGroupMaker> pawnGroupMakers;

	public SimpleCurve raidCommonalityFromPointsCurve;

	public bool autoFlee = true;

	public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

	public bool canSiege;

	public bool canStageAttacks;

	public bool canUseAvoidGrid = true;

	public bool canPsychicRitualSiege;

	public float earliestRaidDays;

	public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

	public SimpleCurve minSettlementTemperatureChanceCurve;

	public PawnKindDef basicMemberKind;

	public List<ResearchProjectTagDef> startingResearchTags;

	public List<ResearchProjectTagDef> startingTechprintsResearchTags;

	[NoTranslate]
	public List<string> recipePrerequisiteTags;

	public bool rescueesCanJoin;

	[MustTranslate]
	public string pawnSingular = "member";

	[MustTranslate]
	public string pawnsPlural = "members";

	[MustTranslate]
	public string leaderTitle = "leader";

	[MustTranslate]
	public string leaderTitleFemale;

	[MustTranslate]
	public string royalFavorLabel;

	[NoTranslate]
	public string royalFavorIconPath;

	public List<PawnKindDef> fixedLeaderKinds;

	public bool leaderForceGenerateNewPawn;

	public float forageabilityFactor = 1f;

	public SimpleCurve maxPawnCostPerTotalPointsCurve;

	public List<string> royalTitleTags;

	[NoTranslate]
	public string categoryTag;

	public bool hostileToFactionlessHumanlikes;

	public ThingDef dropPodActive;

	public ThingDef dropPodIncoming;

	public bool raidsForbidden;

	public bool animalsFleeDanger = true;

	private PawnGroupKindDef defaultSettlementGroupKindDef;

	public bool canRequestTraders = true;

	public bool canRequestMilitaryAid = true;

	public bool canRequestOrbitalTrader;

	public bool canGenerateQuestSites = true;

	public bool hideGiftingInHostilityText;

	public List<PawnsArrivalModeDef> arrivalModeWhitelist = new List<PawnsArrivalModeDef>();

	public List<PawnsArrivalModeDef> arrivalModeBlacklist = new List<PawnsArrivalModeDef>();

	public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> arrivalLayerWhitelist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> arrivalLayerBlacklist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> neutralArrivalLayerWhitelist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> neutralArrivalLayerBlacklist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> raidArrivalLayerWhitelist = new List<PlanetLayerDef>();

	public List<PlanetLayerDef> raidArrivalLayerBlacklist = new List<PlanetLayerDef>();

	public int requiredCountAtGameStart;

	public float settlementGenerationWeight;

	public bool generateNewLeaderFromMapMembersOnly;

	public int maxConfigurableAtWorldCreation = -1;

	public int startingCountAtWorldCreation = 1;

	public int configurationListOrderPriority;

	public FactionDef replacesFaction;

	public bool displayInFactionSelection = true;

	public TechLevel techLevel;

	[NoTranslate]
	public List<BackstoryCategoryFilter> backstoryFilters;

	[NoTranslate]
	private List<string> backstoryCategories;

	public ThingFilter apparelStuffFilter;

	public ThingSetMakerDef settlementLootMaker;

	public ThingSetMakerDef raidLootMaker;

	public SimpleCurve raidLootValueFromPointsCurve;

	public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

	public List<TraderKindDef> orbitalTraderKinds = new List<TraderKindDef>();

	public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

	public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

	public XenotypeSet xenotypeSet;

	public FloatRange melaninRange = FloatRange.ZeroToOne;

	public List<RaidStrategyDef> disallowedRaidStrategies;

	public List<RaidAgeRestrictionDef> disallowedRaidAgeRestrictions;

	public bool mustStartOneEnemy;

	public bool naturalEnemy;

	public bool permanentEnemy;

	public bool permanentEnemyToEveryoneExceptPlayer;

	public List<FactionDef> permanentEnemyToEveryoneExcept;

	[NoTranslate]
	public string settlementTexturePath;

	[NoTranslate]
	public string factionIconPath;

	public List<Color> colorSpectrum;

	public List<PawnRelationDef> royalTitleInheritanceRelations;

	public Type royalTitleInheritanceWorkerClass;

	public List<RoyalImplantRule> royalImplantRules;

	public string renounceTitleMessage;

	public List<CultureDef> allowedCultures;

	public List<MemeDef> requiredMemes;

	public List<MemeDef> allowedMemes;

	public List<MemeDef> disallowedMemes;

	public List<PreceptDef> disallowedPrecepts;

	public List<MemeWeight> structureMemeWeights;

	public bool classicIdeo;

	public bool fixedIdeo;

	public string ideoName;

	public bool hiddenIdeo;

	[MustTranslate]
	public string ideoDescription;

	public List<StyleCategoryDef> styles;

	public List<DeityPreset> deityPresets;

	public List<MemeDef> forcedMemes;

	public bool requiredPreceptsOnly;

	[Obsolete("Will be removed in future version.")]
	public int maxCountAtGameStart;

	[Obsolete("Will be removed in future version.")]
	public bool canMakeRandomly;

	[MayTranslate]
	public string dialogFactionGreetingHostile;

	[MayTranslate]
	public string dialogFactionGreetingHostileAppreciative;

	[MayTranslate]
	public string dialogFactionGreetingWary;

	[MayTranslate]
	public string dialogFactionGreetingWarm;

	[MayTranslate]
	public string dialogMilitaryAidSent;

	[MustTranslate]
	public string messageDefendersAttacking;

	[Unsaved(false)]
	private Texture2D factionIcon;

	[Unsaved(false)]
	private Texture2D settlementTexture;

	[Unsaved(false)]
	private Texture2D royalFavorIcon;

	[Unsaved(false)]
	private string cachedDescription;

	[Unsaved(false)]
	private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

	[Unsaved(false)]
	private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

	[Unsaved(false)]
	private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;

	public List<RoyalTitleDef> RoyalTitlesAwardableInSeniorityOrderForReading
	{
		get
		{
			if (royalTitlesAwardableInSeniorityOrderForReading == null)
			{
				royalTitlesAwardableInSeniorityOrderForReading = new List<RoyalTitleDef>();
				if (royalTitleTags != null && ModLister.RoyaltyInstalled)
				{
					foreach (RoyalTitleDef item in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
					{
						if (item.Awardable && item.tags.SharesElementWith(royalTitleTags))
						{
							royalTitlesAwardableInSeniorityOrderForReading.Add(item);
						}
					}
					royalTitlesAwardableInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
				}
			}
			return royalTitlesAwardableInSeniorityOrderForReading;
		}
	}

	public List<RoyalTitleDef> RoyalTitlesAllInSeniorityOrderForReading
	{
		get
		{
			if (royalTitlesAllInSeniorityOrderForReading == null)
			{
				royalTitlesAllInSeniorityOrderForReading = new List<RoyalTitleDef>();
				if (royalTitleTags != null && ModLister.RoyaltyInstalled)
				{
					foreach (RoyalTitleDef item in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
					{
						if (item.tags.SharesElementWith(royalTitleTags))
						{
							royalTitlesAllInSeniorityOrderForReading.Add(item);
						}
					}
					royalTitlesAllInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
				}
			}
			return royalTitlesAllInSeniorityOrderForReading;
		}
	}

	public RoyalTitleInheritanceWorker RoyalTitleInheritanceWorker
	{
		get
		{
			if (royalTitleInheritanceWorker == null && royalTitleInheritanceWorkerClass != null)
			{
				royalTitleInheritanceWorker = (RoyalTitleInheritanceWorker)Activator.CreateInstance(royalTitleInheritanceWorkerClass);
			}
			return royalTitleInheritanceWorker;
		}
	}

	public bool CanEverBeNonHostile => !permanentEnemy;

	public Texture2D SettlementTexture
	{
		get
		{
			if (settlementTexture == null)
			{
				if (!settlementTexturePath.NullOrEmpty())
				{
					settlementTexture = ContentFinder<Texture2D>.Get(settlementTexturePath);
				}
				else
				{
					settlementTexture = BaseContent.BadTex;
				}
			}
			return settlementTexture;
		}
	}

	public Texture2D FactionIcon
	{
		get
		{
			if (factionIcon == null)
			{
				if (!factionIconPath.NullOrEmpty())
				{
					factionIcon = ContentFinder<Texture2D>.Get(factionIconPath);
				}
				else
				{
					factionIcon = BaseContent.BadTex;
				}
			}
			return factionIcon;
		}
	}

	public Texture2D RoyalFavorIcon
	{
		get
		{
			if (royalFavorIcon == null && !royalFavorIconPath.NullOrEmpty())
			{
				royalFavorIcon = ContentFinder<Texture2D>.Get(royalFavorIconPath);
			}
			return royalFavorIcon;
		}
	}

	public bool HasRoyalTitles => RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;

	public Color DefaultColor
	{
		get
		{
			if (colorSpectrum.NullOrEmpty())
			{
				return Color.white;
			}
			return colorSpectrum[0];
		}
	}

	public float BaselinerChance
	{
		get
		{
			if (xenotypeSet != null)
			{
				return xenotypeSet.BaselinerChance;
			}
			return 1f;
		}
	}

	public string Description
	{
		get
		{
			if (cachedDescription == null)
			{
				if (description.NullOrEmpty())
				{
					description = string.Empty;
				}
				else
				{
					cachedDescription = description;
				}
				if (ModsConfig.BiotechActive && humanlikeFaction)
				{
					List<XenotypeChance> list = new List<XenotypeChance>();
					cachedDescription = cachedDescription + "\n\n" + ("MemberXenotypeChances".Translate() + ":").AsTipTitle() + "\n";
					if (BaselinerChance > 0f)
					{
						list.Add(new XenotypeChance(XenotypeDefOf.Baseliner, BaselinerChance));
					}
					if (xenotypeSet != null)
					{
						for (int i = 0; i < xenotypeSet.Count; i++)
						{
							if (xenotypeSet[i].xenotype != XenotypeDefOf.Baseliner)
							{
								list.Add(xenotypeSet[i]);
							}
						}
					}
					if (list.Any())
					{
						list.SortBy((XenotypeChance x) => 0f - x.chance);
						cachedDescription += list.Select((XenotypeChance x) => $"{x.xenotype.LabelCap}: {Mathf.Min(x.chance, 1f).ToStringPercent()}").ToLineList("  - ");
					}
				}
			}
			return cachedDescription;
		}
	}

	public bool PermanentlyHostileTo(FactionDef otherFactionDef)
	{
		if (permanentEnemy)
		{
			return true;
		}
		if (permanentEnemyToEveryoneExcept != null && !permanentEnemyToEveryoneExcept.Contains(otherFactionDef))
		{
			return true;
		}
		if (permanentEnemyToEveryoneExceptPlayer && !otherFactionDef.isPlayer)
		{
			return true;
		}
		return false;
	}

	public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind, PawnGroupMakerParms parms = null)
	{
		if (pawnGroupMakers == null)
		{
			return 0f;
		}
		IEnumerable<PawnGroupMaker> source = pawnGroupMakers.Where((PawnGroupMaker x) => x.kindDef == groupKind);
		if (!source.Any())
		{
			return 0f;
		}
		return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything(this, parms));
	}

	public bool CanUseStuffForApparel(ThingDef stuffDef)
	{
		if (apparelStuffFilter == null)
		{
			return true;
		}
		return apparelStuffFilter.Allows(stuffDef);
	}

	public float RaidCommonalityFromPoints(float points)
	{
		if (points < 0f || raidCommonalityFromPointsCurve == null)
		{
			return 1f;
		}
		return raidCommonalityFromPointsCurve.Evaluate(points);
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (apparelStuffFilter != null)
		{
			apparelStuffFilter.ResolveReferences();
		}
	}

	public override void PostLoad()
	{
		if (backstoryCategories != null && backstoryCategories.Count > 0)
		{
			if (backstoryFilters == null)
			{
				backstoryFilters = new List<BackstoryCategoryFilter>();
			}
			backstoryFilters.Add(new BackstoryCategoryFilter
			{
				categories = backstoryCategories
			});
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__150))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__150(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static FactionDef Named(string defName)
	{
		return DefDatabase<FactionDef>.GetNamed(defName);
	}
}
