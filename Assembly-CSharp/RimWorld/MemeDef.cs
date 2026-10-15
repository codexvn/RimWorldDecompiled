using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class MemeDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__52 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MemeDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<ThingStyleCategoryWithPriority>.Enumerator _003C_003E7__wrap2;

		private List<IdeoDescriptionMaker.PatternEntry>.Enumerator _003C_003E7__wrap3;

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
		public _003CConfigErrors_003Ed__52(int _003C_003E1__state)
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
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 10:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			_003C_003E7__wrap3 = default(List<IdeoDescriptionMaker.PatternEntry>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MemeDef memeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)memeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0092;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0092;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e2;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0113;
				case 4:
					_003C_003E1__state = -4;
					goto IL_019a;
				case 5:
					_003C_003E1__state = -1;
					goto IL_01f5;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0269;
				case 7:
					_003C_003E1__state = -1;
					goto IL_02c1;
				case 8:
					_003C_003E1__state = -1;
					break;
				case 9:
					_003C_003E1__state = -1;
					goto IL_032d;
				case 10:
					{
						_003C_003E1__state = -5;
						goto IL_037e;
					}
					IL_0092:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (memeDef.category == MemeCategory.Structure && string.IsNullOrWhiteSpace(memeDef.worshipRoomLabel))
					{
						_003C_003E2__current = "Structure meme has empty worshipRoomLabel";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e2;
					IL_01b9:
					if (memeDef.category == MemeCategory.Structure && memeDef.impact != 0)
					{
						_003C_003E2__current = memeDef.defName + " structure meme impact must be 0.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_01f5;
					IL_01f5:
					if (memeDef.category != MemeCategory.Structure && (memeDef.impact < 1 || memeDef.impact > 3))
					{
						_003C_003E2__current = $"{memeDef.defName} normal meme impact must be between 1 and {3}.";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0269;
					IL_00e2:
					if (memeDef.fixedDeityNameTypes != null && memeDef.category != MemeCategory.Structure)
					{
						_003C_003E2__current = "fixedDeityNameTypes can only be used in structure memes.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0113;
					IL_037e:
					while (_003C_003E7__wrap3.MoveNext())
					{
						if (_003C_003E7__wrap3.Current.def == null)
						{
							_003C_003E2__current = "descriptionMaker pattern has null def.";
							_003C_003E1__state = 10;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<IdeoDescriptionMaker.PatternEntry>.Enumerator);
					break;
					IL_032d:
					_003C_003E7__wrap3 = memeDef.descriptionMaker.patterns.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_037e;
					IL_0113:
					if (!memeDef.thingStyleCategories.NullOrEmpty())
					{
						_003C_003E7__wrap2 = memeDef.thingStyleCategories.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_019a;
					}
					goto IL_01b9;
					IL_02c1:
					if (memeDef.category != MemeCategory.Structure)
					{
						break;
					}
					if (memeDef.descriptionMaker == null)
					{
						_003C_003E2__current = "descriptionMaker is required for structure memes.";
						_003C_003E1__state = 8;
						return true;
					}
					if (memeDef.descriptionMaker.patterns.NullOrEmpty())
					{
						_003C_003E2__current = "descriptionMaker must define at least one pattern for structure memes.";
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_032d;
					IL_019a:
					while (_003C_003E7__wrap2.MoveNext())
					{
						ThingStyleCategoryWithPriority current2 = _003C_003E7__wrap2.Current;
						if (current2.priority <= 0f)
						{
							_003C_003E2__current = "style category " + current2.category.LabelCap + " has <= 0 priority. It must be positive.";
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
					goto IL_01b9;
					IL_0269:
					if (!memeDef.agreeableTraits.NullOrEmpty() && !memeDef.disagreeableTraits.NullOrEmpty() && memeDef.agreeableTraits.SharesElementWith(memeDef.disagreeableTraits))
					{
						_003C_003E2__current = memeDef.defName + " agreeableTraits and disagreeableTraits share one or more values.";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_02c1;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__52 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__52(0)
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

	public string iconPath;

	public int renderOrder;

	public int impact = -999;

	public MemeCategory category;

	public MemeGroupDef groupDef;

	[NoTranslate]
	public List<string> exclusionTags = new List<string>();

	public List<List<PreceptDef>> requireOne;

	public PreceptsWithNoneChance selectOneOrNone;

	public List<RequiredRitualAndBuilding> requiredRituals;

	public IdeoWeaponClassPair preferredWeaponClasses;

	public float randomizationSelectionWeightFactor = 1f;

	public List<ThingDef> requireAnyRitualSeat;

	public IntRange deityCount = new IntRange(-1, -1);

	public bool allowDuringTutorial;

	public List<FactionDef> factionWhitelist;

	public bool hiddenInChooseMemes;

	public RulePack generalRules;

	public IdeoDescriptionMaker descriptionMaker;

	public RulePackDef deityNameMakerOverride;

	public RulePackDef deityTypeMakerOverride;

	public bool allowSymbolsFromDeity = true;

	public bool symbolPackOverride;

	[MustTranslate]
	public List<IdeoSymbolPack> symbolPacks;

	public List<DeityNameType> fixedDeityNameTypes;

	public List<ThingStyleCategoryWithPriority> thingStyleCategories;

	public IntRange ritualsToMake = IntRange.Zero;

	public List<string> replaceRitualsWithTags = new List<string>();

	public List<RitualPatternDef> replacementPatterns;

	public List<ThingDef> consumableBuildings;

	public int veneratedAnimalsCountOffset;

	public int veneratedAnimalsCountOverride = -1;

	public List<StyleItemTagWeighted> styleItemTags;

	[MustTranslate]
	public string worshipRoomLabel;

	public List<ResearchProjectDef> startingResearchProjects = new List<ResearchProjectDef>();

	public List<BuildableDef> addDesignators;

	public List<DesignatorDropdownGroupDef> addDesignatorGroups;

	public List<PreceptApparelRequirement> apparelRequirements;

	[XmlInheritanceAllowDuplicateNodes]
	public List<TraitRequirement> agreeableTraits;

	[XmlInheritanceAllowDuplicateNodes]
	public List<TraitRequirement> disagreeableTraits;

	public bool preventApparelRequirements;

	public XenotypeSet xenotypeSet;

	private Texture2D icon;

	private List<BuildableDef> cachedAllDesignatorBuildables;

	private List<string> unlockedRoles;

	private List<string> unlockedRituals;

	private Ideo unlockedRolesCachedFor;

	public Texture2D Icon
	{
		get
		{
			if (!ModLister.CheckIdeology("Memes"))
			{
				return BaseContent.BadTex;
			}
			if (icon == null)
			{
				icon = ContentFinder<Texture2D>.Get(iconPath);
			}
			return icon;
		}
	}

	public List<BuildableDef> AllDesignatorBuildables
	{
		get
		{
			if (cachedAllDesignatorBuildables == null)
			{
				cachedAllDesignatorBuildables = new List<BuildableDef>();
				if (addDesignators != null)
				{
					foreach (BuildableDef addDesignator in addDesignators)
					{
						cachedAllDesignatorBuildables.Add(addDesignator);
					}
				}
				if (addDesignatorGroups != null)
				{
					foreach (DesignatorDropdownGroupDef addDesignatorGroup in addDesignatorGroups)
					{
						cachedAllDesignatorBuildables.AddRange(addDesignatorGroup.BuildablesWithoutDefaultDesignators());
					}
				}
			}
			return cachedAllDesignatorBuildables;
		}
	}

	public List<string> UnlockedRoles(Ideo ideo)
	{
		if (unlockedRoles == null || ideo != unlockedRolesCachedFor)
		{
			unlockedRoles = new List<string>();
			foreach (PreceptDef item in DefDatabase<PreceptDef>.AllDefsListForReading)
			{
				if (typeof(Precept_Role).IsAssignableFrom(item.preceptClass) && !item.requiredMemes.NullOrEmpty() && item.requiredMemes.Contains(this))
				{
					unlockedRoles.Add(item.LabelCap.Resolve());
				}
			}
			unlockedRolesCachedFor = ideo;
		}
		return unlockedRoles;
	}

	public List<string> UnlockedRituals()
	{
		if (unlockedRituals == null)
		{
			unlockedRituals = new List<string>();
			if (consumableBuildings != null)
			{
				foreach (RitualPatternDef p in DefDatabase<RitualPatternDef>.AllDefsListForReading)
				{
					if (p.ignoreConsumableBuildingRequirement || p.ritualObligationTargetFilter == null || p.ritualObligationTargetFilter.thingDefs.NullOrEmpty() || !p.ritualObligationTargetFilter.thingDefs.Any((ThingDef td) => consumableBuildings.Contains(td)))
					{
						continue;
					}
					object obj = p.shortDescOverride.CapitalizeFirst();
					if (obj == null)
					{
						TaggedString? taggedString = DefDatabase<PreceptDef>.AllDefsListForReading.FirstOrDefault((PreceptDef pr) => pr.ritualPatternBase == p)?.LabelCap;
						obj = (taggedString.HasValue ? ((string)taggedString.GetValueOrDefault()) : null);
					}
					string text = (string)obj;
					if (text != null)
					{
						unlockedRituals.Add(text);
					}
				}
			}
		}
		return unlockedRituals;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__52))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__52(-2)
		{
			_003C_003E4__this = this
		};
	}
}
