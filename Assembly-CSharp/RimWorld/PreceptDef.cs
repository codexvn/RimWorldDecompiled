using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PreceptDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__119 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private IEnumerator<PreceptThingChance> _003C_003E7__wrap2;

		private List<PreceptComp>.Enumerator _003C_003E7__wrap3;

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
		public _003CConfigErrors_003Ed__119(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
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
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -6:
			case -5:
			case 4:
				try
				{
					if (num == -6 || num == 4)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<PreceptComp>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PreceptDef preceptDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)preceptDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d3;
				case 3:
					_003C_003E1__state = -4;
					goto IL_017a;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_0201;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (preceptDef.preceptClass == typeof(Precept_Ritual) && preceptDef.ritualPatternBase == null)
					{
						_003C_003E2__current = "Ritual doesn't define a ritualPatternBase, was it meant to be abstract?";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d3;
					IL_0201:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					goto IL_021b;
					IL_021b:
					if (_003C_003E7__wrap3.MoveNext())
					{
						PreceptComp current3 = _003C_003E7__wrap3.Current;
						_003C_003E7__wrap1 = current3.ConfigErrors(preceptDef).GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_0201;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<PreceptComp>.Enumerator);
					return false;
					IL_0194:
					_003C_003E7__wrap3 = preceptDef.comps.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_021b;
					IL_00d3:
					if (typeof(Precept_ThingStyle).IsAssignableFrom(preceptDef.preceptClass))
					{
						_003C_003E7__wrap2 = preceptDef.Worker.ThingDefs.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_017a;
					}
					goto IL_0194;
					IL_017a:
					while (_003C_003E7__wrap2.MoveNext())
					{
						ThingDef def = _003C_003E7__wrap2.Current.def;
						if (!def.CanBeStyled())
						{
							_003C_003E2__current = "ThingDef " + def.defName + " is on available things list of " + preceptDef.defName + " precept, but missing CompStyleable thing comp!";
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_0194;
				}
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
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
			_003CConfigErrors_003Ed__119 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__119(0)
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

	public Type preceptClass = typeof(Precept);

	public IssueDef issue;

	public List<PreceptComp> comps = new List<PreceptComp>();

	public List<AbilityStatModifiers> abilityStatFactors;

	public List<StatModifier> statOffsets;

	public List<StatModifier> statFactors;

	public List<ConditionalStatAffecter> conditionalStatAffecters;

	public float selectionWeight = 1f;

	public List<WorkTypeDef> opposedWorkTypes = new List<WorkTypeDef>();

	public PreceptImpact impact;

	public List<MemeDef> associatedMemes = new List<MemeDef>();

	public List<MemeDef> conflictingMemes = new List<MemeDef>();

	public List<MemeDef> requiredMemes = new List<MemeDef>();

	public bool visible = true;

	public bool visibleOnAddFloatMenu = true;

	public bool listedForRoles = true;

	public PreceptDef takeNameFrom;

	public PreceptDef alsoAdds;

	public int maxCount = 1;

	public bool isNonIdeoRitual;

	[NoTranslate]
	public List<string> exclusionTags = new List<string>();

	public bool allowDuplicates;

	public bool ignoreLimitsInEditMode;

	public bool canUseAlreadyUsedThingDef;

	public bool classic;

	public bool classicExtra;

	public float defaultSelectionWeight;

	public bool enabledForNPCFactions = true;

	public bool countsTowardsPreceptLimit = true;

	public bool canGenerateAsSpecialPrecept = true;

	public RulePackDef nameMaker;

	public int nameMaxLength = 16;

	public SimpleCurve preceptInstanceCountCurve;

	public RitualPatternDef ritualPatternBase;

	public bool receivesExpectationsQualityOffset;

	public bool usesIdeoVisualEffects = true;

	public bool mergeRitualGizmosFromAllIdeos;

	public bool allowSpectatorsFromOtherIdeos = true;

	public bool allowOptionalRitualObligations;

	public bool classicModeOnly;

	public List<PreceptThingChanceClass> buildingDefChances;

	public List<ExpectationDef> buildingMinExpectations;

	public List<RoomRequirement> buildingRoomRequirements;

	public List<RoomRequirement> buildingRoomRequirementsFixed;

	public SimpleCurve roomRequirementCountCurve;

	public bool leaderRole;

	public int activationBelieverCount = -1;

	public int deactivationBelieverCount = -1;

	public List<RoleRequirement> roleRequirements;

	public WorkTags roleDisabledWorkTags;

	public WorkTags roleRequiredWorkTags;

	public WorkTags roleRequiredWorkTagAny;

	public List<PreceptApparelRequirement> roleApparelRequirements;

	public SimpleCurve roleApparelRequirementCountCurve;

	public List<AbilityDef> grantedAbilities;

	public List<RoleEffect> roleEffects;

	public List<string> roleTags;

	public string iconPath;

	public float restrictToSupremeGenderChance;

	public float certaintyLossFactor = 1f;

	public float convertPowerFactor = 1f;

	public int expectationsOffset;

	public bool createsRoleEmptyThought = true;

	public bool roleCanBeChild;

	public bool disallowLoggingCamps;

	public bool disallowMiningCamps;

	public bool disallowHuntingCamps;

	public bool disallowFarmingCamps;

	public bool approvesOfSlavery;

	public bool prefersNudity;

	public Gender genderPrefersNudity;

	public bool useChoicesFromBuildingDefs;

	public int displayOrderInImpact;

	public int displayOrderInIssue;

	public bool proselytizes;

	public int requiredScars;

	public bool approvesOfCharity;

	public float blindPawnChance = -1f;

	public bool approvesOfBlindness;

	public bool canRemoveInUI = true;

	public bool prefersDarkness;

	public List<NeedDef> enablesNeeds;

	public List<NeedDef> disablesNeeds;

	public float blindPsychicSensitivityOffset;

	public DrugPolicyDef defaultDrugPolicyOverride;

	public bool warnPlayerOnDesignateChopTree;

	public bool warnPlayerOnDesignateMine;

	public bool willingToConstructOtherIdeoBuildables;

	public float biosculpterPodCycleSpeedFactor = 1f;

	public float growthVatSpeedFactor = 1f;

	public bool prefersSlabBed;

	public bool useRepeatPenalty = true;

	public bool showRitualFloatMenuOption = true;

	[MustTranslate]
	public string tipLabelOverride;

	public bool capitalizeAsTitle = true;

	public bool ignoreNameUniqueness;

	[MustTranslate]
	public string extraTextPawnDeathLetter;

	public PreceptDef apparelPreceptSwapDef;

	public AbilityGroupDef useCooldownFromAbilityGroupDef;

	public bool iconIgnoresIdeoColor;

	public PreceptDef sourcePawnRoleDef;

	public AbilityDef sourceAbilityDef;

	public bool likesHumanLeatherApparel;

	public bool approvesOfRaiding;

	public float skipOpportunityLettersBeforeDay = 10f;

	public bool notifyPlayerOnOpportunity = true;

	public Type workerClass = typeof(PreceptWorker);

	private PreceptWorker worker;

	[Unsaved(false)]
	private List<TraitRequirement> traitsAffectingCached;

	[Unsaved(false)]
	private List<string> requiredMemeLabels;

	private Texture2D icon;

	public Texture2D Icon
	{
		get
		{
			if (icon == null)
			{
				if (iconPath != null)
				{
					icon = ContentFinder<Texture2D>.Get(iconPath);
				}
				else if (!issue.iconPath.NullOrEmpty())
				{
					icon = issue.Icon;
				}
			}
			return icon;
		}
	}

	public PreceptWorker Worker
	{
		get
		{
			if (worker == null)
			{
				worker = (PreceptWorker)Activator.CreateInstance(workerClass);
				worker.def = this;
			}
			return worker;
		}
	}

	public List<TraitRequirement> TraitsAffecting
	{
		get
		{
			if (traitsAffectingCached == null)
			{
				traitsAffectingCached = new List<TraitRequirement>();
				for (int i = 0; i < comps.Count; i++)
				{
					traitsAffectingCached.AddRange(comps[i].TraitsAffecting);
				}
				traitsAffectingCached.RemoveDuplicates((TraitRequirement a, TraitRequirement b) => a.def == b.def && (a.degree.HasValue ? a.degree.Value : 0) == (b.degree.HasValue ? b.degree.Value : 0));
			}
			return traitsAffectingCached;
		}
	}

	public List<string> RequiredMemeLabels
	{
		get
		{
			if (requiredMemeLabels == null && !requiredMemes.NullOrEmpty())
			{
				requiredMemeLabels = new List<string>();
				foreach (MemeDef requiredMeme in requiredMemes)
				{
					requiredMemeLabels.Add(requiredMeme.label);
				}
			}
			return requiredMemeLabels;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__119))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__119(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostLoad()
	{
		base.PostLoad();
		foreach (PreceptComp comp in comps)
		{
			comp.preceptDef = this;
		}
	}
}
