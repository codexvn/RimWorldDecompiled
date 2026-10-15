using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StatDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__78 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StatDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<PawnCapacityFactor>.Enumerator _003C_003E7__wrap2;

		private int _003Ci_003E5__4;

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
		public _003CConfigErrors_003Ed__78(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
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
			_003C_003E7__wrap2 = default(List<PawnCapacityFactor>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StatDef statDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)statDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f6;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_01bf;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (statDef.capacityFactors != null)
					{
						_003C_003E7__wrap2 = statDef.capacityFactors.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f6;
					}
					goto IL_0115;
					IL_01eb:
					if (_003Ci_003E5__4 >= statDef.parts.Count)
					{
						break;
					}
					_003C_003E7__wrap1 = statDef.parts[_003Ci_003E5__4].ConfigErrors().GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01bf;
					IL_00f6:
					while (_003C_003E7__wrap2.MoveNext())
					{
						if (_003C_003E7__wrap2.Current.weight > 1f)
						{
							_003C_003E2__current = statDef.defName + " has activity factor with weight > 1";
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<PawnCapacityFactor>.Enumerator);
					goto IL_0115;
					IL_01bf:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = statDef.defName + " has error in StatPart " + statDef.parts[_003Ci_003E5__4].ToString() + ": " + current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__4++;
					goto IL_01eb;
					IL_0115:
					if (statDef.parts == null)
					{
						break;
					}
					_003Ci_003E5__4 = 0;
					goto IL_01eb;
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
			_003CConfigErrors_003Ed__78 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__78(0)
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

	private static HashSet<StatDef> mutableStats;

	public StatCategoryDef category;

	public Type workerClass = typeof(StatWorker);

	public string labelForFullStatList;

	public bool forInformationOnly;

	[MustTranslate]
	private string offsetLabel;

	public float hideAtValue = -2.1474836E+09f;

	public bool alwaysHide;

	public bool showNonAbstract = true;

	public bool showIfUndefined = true;

	public bool showOnPawns = true;

	public bool showOnHumanlikes = true;

	public bool showOnNonWildManHumanlikes = true;

	public bool showOnAnimals = true;

	public bool showOnMechanoids = true;

	public bool showOnNonWorkTables = true;

	public bool showOnEntities = true;

	public bool showOnDrones = true;

	public bool showOnNonPowerPlants = true;

	public bool showOnDefaultValue = true;

	public bool showOnUnhaulables = true;

	public bool showOnUntradeables = true;

	public List<string> showIfModsLoaded;

	public List<string> showIfModsLoadedAny;

	public List<HediffDef> showIfHediffsPresent;

	public bool neverDisabled;

	public bool showZeroBaseValue;

	public bool showOnSlavesOnly;

	public bool showOnPlayerMechanoids;

	public DevelopmentalStage showDevelopmentalStageFilter = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

	public bool hideInClassicMode;

	public List<PawnKindDef> showOnPawnKind;

	public bool overridesHideStats;

	public int displayPriorityInCategory;

	public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

	public ToStringStyle toStringStyle;

	private ToStringStyle? toStringStyleUnfinalized;

	[MustTranslate]
	public string formatString;

	[MustTranslate]
	public string formatStringUnfinalized;

	public bool finalizeEquippedStatOffset = true;

	[MustTranslate]
	public string statFactorsExplanationHeader;

	public float defaultBaseValue = 1f;

	public List<SkillNeed> skillNeedOffsets;

	public float noSkillOffset;

	public List<PawnCapacityOffset> capacityOffsets;

	public List<StatDef> statFactors;

	public bool applyFactorsIfNegative = true;

	public List<SkillNeed> skillNeedFactors;

	public float noSkillFactor = 1f;

	public List<PawnCapacityFactor> capacityFactors;

	public SimpleCurve postProcessCurve;

	public List<StatDef> postProcessStatFactors;

	public float minValue = -9999999f;

	public float maxValue = 9999999f;

	public float valueIfMissing;

	public bool roundValue;

	public float roundToFiveOver = float.MaxValue;

	public bool minifiedThingInherits;

	public bool supressDisabledError;

	public bool cacheable;

	public bool displayMaxWhenAboveOrEqual;

	public bool scenarioRandomizable;

	public SkillDef disableIfSkillDisabled;

	public List<StatPart> parts;

	[Unsaved(false)]
	private StatWorker workerInt;

	[Unsaved(false)]
	public bool immutable;

	public StatWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				if (parts != null)
				{
					for (int i = 0; i < parts.Count; i++)
					{
						parts[i].parentStat = this;
					}
				}
				workerInt = (StatWorker)Activator.CreateInstance(workerClass);
				workerInt.InitSetStat(this);
			}
			return workerInt;
		}
	}

	public ToStringStyle ToStringStyleUnfinalized
	{
		get
		{
			if (!toStringStyleUnfinalized.HasValue)
			{
				return toStringStyle;
			}
			return toStringStyleUnfinalized.Value;
		}
	}

	public string LabelForFullStatList
	{
		get
		{
			if (!labelForFullStatList.NullOrEmpty())
			{
				return labelForFullStatList;
			}
			return label;
		}
	}

	public string LabelForFullStatListCap => LabelForFullStatList.CapitalizeFirst(this);

	public string OffsetLabel => offsetLabel ?? label;

	public string OffsetLabelCap => OffsetLabel.CapitalizeFirst();

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__78))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__78(-2)
		{
			_003C_003E4__this = this
		};
	}

	public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
	{
		return Worker.ValueToString(val, finalized, numberSense);
	}

	public static StatDef Named(string defName)
	{
		return DefDatabase<StatDef>.GetNamed(defName);
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (parts != null)
		{
			List<StatPart> partsCopy = parts.ToList();
			parts.SortBy((StatPart x) => 0f - x.priority, (StatPart x) => partsCopy.IndexOf(x));
		}
	}

	public T GetStatPart<T>() where T : StatPart
	{
		return parts.OfType<T>().FirstOrDefault();
	}

	public bool CanShowWithLoadedMods()
	{
		if (!showIfModsLoaded.NullOrEmpty())
		{
			for (int i = 0; i < showIfModsLoaded.Count; i++)
			{
				if (!ModsConfig.IsActive(showIfModsLoaded[i]))
				{
					return false;
				}
			}
		}
		if (!showIfModsLoadedAny.NullOrEmpty())
		{
			bool result = false;
			for (int j = 0; j < showIfModsLoadedAny.Count; j++)
			{
				if (ModsConfig.IsActive(showIfModsLoadedAny[j]))
				{
					result = true;
					break;
				}
			}
			return result;
		}
		return true;
	}

	private static void PopulateMutableStats()
	{
		mutableStats = new HashSet<StatDef>();
		foreach (TraitDef item in DefDatabase<TraitDef>.AllDefsListForReading)
		{
			foreach (TraitDegreeData degreeData in item.degreeDatas)
			{
				AddStatsFromModifiers(degreeData.statOffsets);
				AddStatsFromModifiers(degreeData.statFactors);
			}
		}
		foreach (HediffDef item2 in DefDatabase<HediffDef>.AllDefsListForReading)
		{
			if (item2.stages == null)
			{
				continue;
			}
			foreach (HediffStage stage in item2.stages)
			{
				AddStatsFromModifiers(stage.statOffsets);
				AddStatsFromModifiers(stage.statFactors);
				if (stage.statOffsetEffectMultiplier != null)
				{
					mutableStats.Add(stage.statOffsetEffectMultiplier);
				}
				if (stage.statFactorEffectMultiplier != null)
				{
					mutableStats.Add(stage.statFactorEffectMultiplier);
				}
				if (stage.capacityFactorEffectMultiplier != null)
				{
					mutableStats.Add(stage.capacityFactorEffectMultiplier);
				}
			}
		}
		foreach (PreceptDef item3 in DefDatabase<PreceptDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item3.statOffsets);
			AddStatsFromModifiers(item3.statFactors);
			if (item3.roleEffects != null)
			{
				foreach (RoleEffect roleEffect in item3.roleEffects)
				{
					if (roleEffect is RoleEffect_PawnStatOffset roleEffect_PawnStatOffset)
					{
						mutableStats.Add(roleEffect_PawnStatOffset.statDef);
					}
					if (roleEffect is RoleEffect_PawnStatFactor roleEffect_PawnStatFactor)
					{
						mutableStats.Add(roleEffect_PawnStatFactor.statDef);
					}
				}
			}
			if (item3.abilityStatFactors == null)
			{
				continue;
			}
			foreach (AbilityStatModifiers abilityStatFactor in item3.abilityStatFactors)
			{
				AddStatsFromModifiers(abilityStatFactor.modifiers);
			}
		}
		foreach (GeneDef item4 in DefDatabase<GeneDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item4.statOffsets);
			AddStatsFromModifiers(item4.statFactors);
			if (item4.conditionalStatAffecters == null)
			{
				continue;
			}
			foreach (ConditionalStatAffecter conditionalStatAffecter in item4.conditionalStatAffecters)
			{
				AddStatsFromModifiers(conditionalStatAffecter.statOffsets);
				AddStatsFromModifiers(conditionalStatAffecter.statFactors);
			}
		}
		foreach (ThingDef item5 in DefDatabase<ThingDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item5.equippedStatOffsets);
			if (!item5.HasAssignableCompFrom(typeof(CompFacility)))
			{
				continue;
			}
			CompProperties_Facility compProperties = item5.GetCompProperties<CompProperties_Facility>();
			if (compProperties?.statOffsets != null)
			{
				AddStatsFromModifiers(compProperties.statOffsets);
			}
			if (!(compProperties is CompProperties_FacilityQualityBased compProperties_FacilityQualityBased))
			{
				continue;
			}
			foreach (StatDef key in compProperties_FacilityQualityBased.statOffsetsPerQuality.Keys)
			{
				mutableStats.Add(key);
			}
		}
		foreach (WeaponTraitDef item6 in DefDatabase<WeaponTraitDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item6.equippedStatOffsets);
		}
		foreach (LifeStageDef item7 in DefDatabase<LifeStageDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item7.statFactors);
			AddStatsFromModifiers(item7.statOffsets);
		}
		foreach (InspirationDef item8 in DefDatabase<InspirationDef>.AllDefsListForReading)
		{
			AddStatsFromModifiers(item8.statOffsets);
			AddStatsFromModifiers(item8.statFactors);
		}
		static void AddStatsFromModifiers(List<StatModifier> mods)
		{
			if (mods != null)
			{
				mutableStats.AddRange(mods.Select((StatModifier mod) => mod.stat));
			}
		}
	}

	public bool IsImmutable()
	{
		if (workerClass != typeof(StatWorker))
		{
			return false;
		}
		if (!skillNeedOffsets.NullOrEmpty() || !skillNeedFactors.NullOrEmpty())
		{
			return false;
		}
		if (!capacityOffsets.NullOrEmpty() || !capacityFactors.NullOrEmpty())
		{
			return false;
		}
		if (!statFactors.NullOrEmpty())
		{
			return false;
		}
		if (!parts.NullOrEmpty())
		{
			return false;
		}
		if (!postProcessStatFactors.NullOrEmpty())
		{
			return false;
		}
		if (mutableStats.Contains(this))
		{
			return false;
		}
		return true;
	}

	public static void SetImmutability()
	{
		PopulateMutableStats();
		foreach (StatDef item in DefDatabase<StatDef>.AllDefsListForReading)
		{
			item.immutable = item.IsImmutable();
			item.Worker.SetCacheability(item.immutable);
		}
	}

	public static void ResetStaticData()
	{
		mutableStats = null;
		foreach (StatDef item in DefDatabase<StatDef>.AllDefsListForReading)
		{
			item.immutable = false;
			item.Worker.DeleteStatCache();
		}
	}
}
