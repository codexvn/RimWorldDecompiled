using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class ResearchProjectDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__91 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ResearchProjectDef _003C_003E4__this;

		private List<ResearchProjectDef> _003CrpDefs_003E5__2;

		private IEnumerator<string> _003C_003E7__wrap2;

		private int _003Ci_003E5__4;

		private List<ThingDef>.Enumerator _003C_003E7__wrap4;

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
		public _003CConfigErrors_003Ed__91(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 8:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CrpDefs_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap4 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ResearchProjectDef researchProjectDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = ((Def)researchProjectDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0092;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0092;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00dc;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0116;
				case 4:
					_003C_003E1__state = -1;
					goto IL_014b;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0181;
				case 6:
					_003C_003E1__state = -1;
					goto IL_02ba;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0312;
				case 8:
					_003C_003E1__state = -4;
					goto IL_03b0;
				case 9:
					_003C_003E1__state = -1;
					goto IL_0402;
				case 10:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0092:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					if (researchProjectDef.techLevel == TechLevel.Undefined && researchProjectDef.knowledgeCategory == null)
					{
						_003C_003E2__current = "techLevel is Undefined";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00dc;
					IL_0181:
					_003CrpDefs_003E5__2 = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
					_003Ci_003E5__4 = 0;
					goto IL_02cc;
					IL_02ba:
					_003Ci_003E5__4++;
					goto IL_02cc;
					IL_0402:
					if (researchProjectDef.baseCost > 0f && researchProjectDef.knowledgeCost > 0f)
					{
						_003C_003E2__current = "baseCost and knowledgeCost are both defined. This is unsupported.";
						_003C_003E1__state = 10;
						return true;
					}
					break;
					IL_00dc:
					if (researchProjectDef.ResearchViewX < 0f || researchProjectDef.ResearchViewY < 0f)
					{
						_003C_003E2__current = "researchViewX and/or researchViewY not set";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0116;
					IL_0312:
					if (!researchProjectDef.requiredAnalyzed.NullOrEmpty())
					{
						_003C_003E7__wrap4 = researchProjectDef.requiredAnalyzed.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_03b0;
					}
					goto IL_03cf;
					IL_0116:
					if (researchProjectDef.techprintCount == 0 && !researchProjectDef.heldByFactionCategoryTags.NullOrEmpty())
					{
						_003C_003E2__current = "requires no techprints but has heldByFactionCategoryTags.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_014b;
					IL_03b0:
					while (_003C_003E7__wrap4.MoveNext())
					{
						ThingDef current2 = _003C_003E7__wrap4.Current;
						if (!current2.HasAssignableCompFrom(typeof(CompAnalyzable)))
						{
							_003C_003E2__current = "requires analyzing " + current2.label + " but " + current2.label + " cannot be analyzed.";
							_003C_003E1__state = 8;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = default(List<ThingDef>.Enumerator);
					goto IL_03cf;
					IL_014b:
					if (researchProjectDef.techprintCount > 0 && researchProjectDef.heldByFactionCategoryTags.NullOrEmpty())
					{
						_003C_003E2__current = "requires techprints but has no heldByFactionCategoryTags.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0181;
					IL_02cc:
					if (_003Ci_003E5__4 < _003CrpDefs_003E5__2.Count)
					{
						if (_003CrpDefs_003E5__2[_003Ci_003E5__4] != researchProjectDef && _003CrpDefs_003E5__2[_003Ci_003E5__4].tab == researchProjectDef.tab && _003CrpDefs_003E5__2[_003Ci_003E5__4].ResearchViewX == researchProjectDef.ResearchViewX && _003CrpDefs_003E5__2[_003Ci_003E5__4].ResearchViewY == researchProjectDef.ResearchViewY)
						{
							_003C_003E2__current = "same research view coords and tab as " + _003CrpDefs_003E5__2[_003Ci_003E5__4]?.ToString() + ": " + researchProjectDef.ResearchViewX + ", " + researchProjectDef.ResearchViewY + "(" + researchProjectDef.tab?.ToString() + ")";
							_003C_003E1__state = 6;
							return true;
						}
						goto IL_02ba;
					}
					if (!ModLister.RoyaltyInstalled && researchProjectDef.techprintCount > 0)
					{
						_003C_003E2__current = "defines techprintCount, but techprints are a Royalty-specific game system and only work with Royalty installed.";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0312;
					IL_03cf:
					if (researchProjectDef.knowledgeCost > 0f && researchProjectDef.knowledgeCategory == null)
					{
						_003C_003E2__current = "has knowledge cost, but no knowledge category.";
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_0402;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__91 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__91(0)
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

	public float baseCost;

	public List<ResearchProjectDef> prerequisites;

	public List<ResearchProjectDef> hiddenPrerequisites;

	public TechLevel techLevel;

	public List<ResearchProjectDef> requiredByThis;

	private List<ResearchMod> researchMods;

	public ThingDef requiredResearchBuilding;

	public List<ThingDef> requiredResearchFacilities;

	public List<ResearchProjectTagDef> tags;

	public ResearchTabDef tab;

	public float researchViewX = 1f;

	public float researchViewY = 1f;

	[MustTranslate]
	public string discoveredLetterTitle;

	[MustTranslate]
	public string discoveredLetterText;

	public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

	public int techprintCount;

	public float techprintCommonality = 1f;

	public float techprintMarketValue = 1000f;

	[NoTranslate]
	public List<string> heldByFactionCategoryTags;

	public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

	public bool requiresMechanitor;

	public List<ThingDef> requiredAnalyzed;

	public bool recalculatePower;

	public KnowledgeCategoryDef knowledgeCategory;

	public float knowledgeCost;

	public ConceptDef teachConcept;

	public RulePack generalRules;

	public bool requireGravEngineInspected;

	[MustTranslate]
	public List<string> customUnlockTexts;

	[Unsaved(false)]
	private float x = 1f;

	[Unsaved(false)]
	private float y = 1f;

	[Unsaved(false)]
	private bool positionModified;

	[Unsaved(false)]
	private ThingDef cachedTechprint;

	[Unsaved(false)]
	private List<Def> cachedUnlockedDefs;

	[Unsaved(false)]
	private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

	[Unsaved(false)]
	private string cachedDescription;

	public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

	private const float ResearchCostFactorPerTechLevelDiff = 0.5f;

	public float ResearchViewX => x;

	public float ResearchViewY => y;

	public float CostApparent => Cost * CostFactor(Faction.OfPlayer.def.techLevel);

	public float ProgressReal => Find.ResearchManager.GetProgress(this);

	public float ProgressApparent => ProgressReal * CostFactor(Faction.OfPlayer.def.techLevel);

	public string ProgressApparentString
	{
		get
		{
			string text = ((baseCost > 0f) ? "F0" : "F2");
			return ProgressApparent.ToString(text);
		}
	}

	public float ProgressPercent => Mathf.Clamp01(ProgressReal / Cost);

	public bool IsFinished => ProgressReal >= Cost;

	public float Cost
	{
		get
		{
			if (!(baseCost > 0f))
			{
				return knowledgeCost;
			}
			return baseCost;
		}
	}

	public override TaggedString LabelCap
	{
		get
		{
			if (label.NullOrEmpty())
			{
				return null;
			}
			if (cachedLabelCap.NullOrEmpty())
			{
				cachedLabelCap = label.CapitalizeFirst();
			}
			return cachedLabelCap;
		}
	}

	public string Description => description;

	public bool IsHidden
	{
		get
		{
			if (!IsFinished && ModsConfig.AnomalyActive)
			{
				return Find.EntityCodex.Hidden(this);
			}
			return false;
		}
	}

	public bool CanStartNow
	{
		get
		{
			if (!IsFinished && PrerequisitesCompleted && TechprintRequirementMet && (requiredResearchBuilding == null || PlayerHasAnyAppropriateResearchBench) && PlayerMechanitorRequirementMet && AnalyzedThingsRequirementsMet && !IsHidden)
			{
				return InspectionRequirementsMet;
			}
			return false;
		}
	}

	public bool PrerequisitesCompleted
	{
		get
		{
			if (prerequisites != null)
			{
				for (int i = 0; i < prerequisites.Count; i++)
				{
					if (!prerequisites[i].IsFinished)
					{
						return false;
					}
				}
			}
			if (hiddenPrerequisites != null)
			{
				for (int j = 0; j < hiddenPrerequisites.Count; j++)
				{
					if (!hiddenPrerequisites[j].IsFinished)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public int TechprintCount
	{
		get
		{
			if (!ModLister.RoyaltyInstalled)
			{
				return 0;
			}
			return techprintCount;
		}
	}

	public int TechprintsApplied => Find.ResearchManager.GetTechprints(this);

	public bool TechprintRequirementMet
	{
		get
		{
			if (TechprintCount > 0 && Find.ResearchManager.GetTechprints(this) < TechprintCount)
			{
				return false;
			}
			return true;
		}
	}

	public ThingDef Techprint
	{
		get
		{
			if (TechprintCount <= 0)
			{
				return null;
			}
			if (cachedTechprint == null)
			{
				cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
				{
					CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
					return compProperties != null && compProperties.project == this;
				});
				if (cachedTechprint == null)
				{
					Log.ErrorOnce("Could not find techprint for research project " + this, shortHash ^ 0x340C745A);
				}
			}
			return cachedTechprint;
		}
	}

	public int RequiredAnalyzedThingCount => requiredAnalyzed?.Count ?? 0;

	public int AnalyzedThingsCompleted
	{
		get
		{
			if (requiredAnalyzed.NullOrEmpty())
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < requiredAnalyzed.Count; i++)
			{
				Find.AnalysisManager.TryGetAnalysisProgress(requiredAnalyzed[i].GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>()?.analysisID ?? (-1), out var details);
				if (details != null && details.Satisfied)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool AnalyzedThingsRequirementsMet
	{
		get
		{
			if (RequiredAnalyzedThingCount > 0)
			{
				return AnalyzedThingsCompleted >= RequiredAnalyzedThingCount;
			}
			return true;
		}
	}

	public bool InspectionRequirementsMet
	{
		get
		{
			if (ModsConfig.OdysseyActive && requireGravEngineInspected)
			{
				return Find.ResearchManager.gravEngineInspected;
			}
			return true;
		}
	}

	public List<Def> UnlockedDefs
	{
		get
		{
			if (cachedUnlockedDefs == null)
			{
				cachedUnlockedDefs = (from x in (from x in DefDatabase<RecipeDef>.AllDefs.Where((RecipeDef x) => x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))).SelectMany((RecipeDef x) => ((IEnumerable<ThingDefCountClass>)x.products).Select((Func<ThingDefCountClass, Def>)((ThingDefCountClass y) => y.thingDef)))
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
						select x)
						.Concat(from x in DefDatabase<RecipeDef>.AllDefs
							where x.IsSurgery && x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
							orderby x.label
							select x)
						.Concat(DefDatabase<PsychicRitualDef>.AllDefs.Where((PsychicRitualDef x) => x.researchPrerequisite == this))
					orderby x.label
					select x).Distinct().ToList();
			}
			return cachedUnlockedDefs;
		}
	}

	public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
	{
		get
		{
			if (cachedHyperlinks == null)
			{
				cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
				List<Def> unlockedDefs = UnlockedDefs;
				if (unlockedDefs != null)
				{
					for (int i = 0; i < unlockedDefs.Count; i++)
					{
						cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i]));
					}
				}
			}
			return cachedHyperlinks;
		}
	}

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
					if (allBuildingsColonist[j] is Building_ResearchBench bench && CanBeResearchedAt(bench, ignoreResearchBenchPowerStatus: true))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool PlayerMechanitorRequirementMet
	{
		get
		{
			if (!ModsConfig.BiotechActive || !requiresMechanitor)
			{
				return true;
			}
			return MechanitorUtility.AnyMechanitorInPlayerFaction();
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (tab == null)
		{
			tab = ResearchTabDefOf.Main;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__91))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__91(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (!ModLister.RoyaltyInstalled)
		{
			techprintCount = 0;
		}
		if (!ModLister.BiotechInstalled)
		{
			requiredAnalyzed = null;
		}
	}

	public string GetTip()
	{
		string text = LabelCap.Colorize(ColoredText.TipSectionTitleColor);
		if (ProgressReal > 0f && !IsFinished)
		{
			text = text + " (" + ProgressApparentString + " / " + CostApparent.ToString("F0") + ")";
		}
		if (cachedDescription == null)
		{
			cachedDescription = Description;
			if (TechprintCount > 0)
			{
				cachedDescription = cachedDescription + "\n\n" + ("RequiredTechprintTip".Translate() + ": " + Techprint.LabelCap).ToString();
			}
			if (RequiredAnalyzedThingCount > 0)
			{
				cachedDescription = cachedDescription + "\n\n" + "StudyRequirementTip".Translate().ToString() + ": " + requiredAnalyzed.Select((ThingDef t) => t.label).ToCommaList(useAnd: true).CapitalizeFirst();
			}
			if (modContentPack != null && !modContentPack.IsCoreMod)
			{
				Color color = (modContentPack.IsOfficialMod ? ModLister.GetExpansionWithIdentifier(modContentPack.PackageId.ToLower()).primaryColor : ColoredText.SubtleGrayColor);
				cachedDescription = cachedDescription + "\n\n" + ("Stat_Source_Label".Translate().ToString() + ": " + modContentPack.Name).Colorize(color);
			}
		}
		return text + "\n" + cachedDescription;
	}

	public float CostFactor(TechLevel researcherTechLevel)
	{
		if (this.techLevel == TechLevel.Undefined)
		{
			return 1f;
		}
		TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
		if ((int)researcherTechLevel >= (int)techLevel)
		{
			return 1f;
		}
		int num = techLevel - researcherTechLevel;
		return 1f + (float)num * 0.5f;
	}

	public bool HasTag(ResearchProjectTagDef tag)
	{
		if (tags == null)
		{
			return false;
		}
		return tags.Contains(tag);
	}

	public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
	{
		if (requiredResearchBuilding != null && bench.def != requiredResearchBuilding)
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
		if (!requiredResearchFacilities.NullOrEmpty())
		{
			CompAffectedByFacilities affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
			if (affectedByFacilities == null)
			{
				return false;
			}
			List<Thing> linkedFacilitiesListForReading = affectedByFacilities.LinkedFacilitiesListForReading;
			int i;
			for (i = 0; i < requiredResearchFacilities.Count; i++)
			{
				if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredResearchFacilities[i] && affectedByFacilities.IsFacilityActive(x)) == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool AnyOtherVisiblePrerequisitesCompleted(ResearchProjectDef prerequisite)
	{
		if (prerequisites == null)
		{
			return false;
		}
		for (int i = 0; i < prerequisites.Count; i++)
		{
			if (prerequisites[i] != prerequisite && prerequisites[i].IsFinished)
			{
				return true;
			}
		}
		return false;
	}

	public void ReapplyAllMods()
	{
		if (researchMods == null)
		{
			return;
		}
		for (int i = 0; i < researchMods.Count; i++)
		{
			try
			{
				researchMods[i].Apply();
			}
			catch (Exception ex)
			{
				Log.Error("Exception applying research mod for project " + this?.ToString() + ": " + ex);
			}
		}
	}

	public static ResearchProjectDef Named(string defName)
	{
		return DefDatabase<ResearchProjectDef>.GetNamed(defName);
	}

	public static void GenerateNonOverlappingCoordinates()
	{
		foreach (ResearchProjectDef item in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
		{
			item.x = item.researchViewX;
			item.y = item.researchViewY;
		}
		int num = 0;
		while (true)
		{
			bool flag = false;
			foreach (ResearchProjectDef item2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				foreach (ResearchProjectDef item3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					if (item2 == item3 || item2.tab != item3.tab)
					{
						continue;
					}
					bool num2 = Mathf.Abs(item2.x - item3.x) < 0.5f;
					bool flag2 = Mathf.Abs(item2.y - item3.y) < 0.25f;
					if (num2 && flag2)
					{
						flag = true;
						if (item2.x <= item3.x)
						{
							item2.x -= 0.1f;
							item3.x += 0.1f;
						}
						else
						{
							item2.x += 0.1f;
							item3.x -= 0.1f;
						}
						if (item2.y <= item3.y)
						{
							item2.y -= 0.1f;
							item3.y += 0.1f;
						}
						else
						{
							item2.y += 0.1f;
							item3.y -= 0.1f;
						}
						item2.x += 0.001f;
						item2.y += 0.001f;
						item3.x -= 0.001f;
						item3.y -= 0.001f;
						ClampInCoordinateLimits(item2);
						ClampInCoordinateLimits(item3);
					}
				}
			}
			if (flag)
			{
				num++;
				if (num > 200)
				{
					Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.");
					break;
				}
				continue;
			}
			break;
		}
	}

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

	public void Debug_ApplyPositionDelta(Vector2 delta)
	{
		bool num = Mathf.Abs(delta.x) > 0.01f;
		bool flag = Mathf.Abs(delta.y) > 0.01f;
		if (num)
		{
			x += delta.x;
		}
		if (flag)
		{
			y += delta.y;
		}
		if (num || flag)
		{
			positionModified = true;
		}
	}

	public void Debug_SnapPositionData()
	{
		x = Mathf.Round(x * 2f) / 2f;
		y = Mathf.Round(y * 10f) / 10f;
		ClampInCoordinateLimits(this);
	}

	public bool Debug_IsPositionModified()
	{
		return positionModified;
	}
}
