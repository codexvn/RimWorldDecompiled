using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class TraitSet : IExposable
{
	[CompilerGenerated]
	private sealed class _003Cget_TheOnlyAllowedMentalBreaks_003Ed__30 : IEnumerable<MentalBreakDef>, IEnumerable, IEnumerator<MentalBreakDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private MentalBreakDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TraitSet _003C_003E4__this;

		private int _003Ci_003E5__2;

		private Trait _003Ctrait_003E5__3;

		private int _003Cj_003E5__4;

		MentalBreakDef IEnumerator<MentalBreakDef>.Current
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
		public _003Cget_TheOnlyAllowedMentalBreaks_003Ed__30(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Ctrait_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TraitSet traitSet = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__4++;
				goto IL_00b0;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_00e4;
			IL_00cd:
			_003Ctrait_003E5__3 = null;
			goto IL_00d4;
			IL_00d4:
			_003Ci_003E5__2++;
			goto IL_00e4;
			IL_00e4:
			if (_003Ci_003E5__2 < traitSet.allTraits.Count)
			{
				_003Ctrait_003E5__3 = traitSet.allTraits[_003Ci_003E5__2];
				if (!_003Ctrait_003E5__3.Suppressed)
				{
					if (_003Ctrait_003E5__3.CurrentData.theOnlyAllowedMentalBreaks != null)
					{
						_003Cj_003E5__4 = 0;
						goto IL_00b0;
					}
					goto IL_00cd;
				}
				goto IL_00d4;
			}
			return false;
			IL_00b0:
			if (_003Cj_003E5__4 < _003Ctrait_003E5__3.CurrentData.theOnlyAllowedMentalBreaks.Count)
			{
				_003C_003E2__current = _003Ctrait_003E5__3.CurrentData.theOnlyAllowedMentalBreaks[_003Cj_003E5__4];
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_00cd;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<MentalBreakDef> IEnumerable<MentalBreakDef>.GetEnumerator()
		{
			_003Cget_TheOnlyAllowedMentalBreaks_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_TheOnlyAllowedMentalBreaks_003Ed__30(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<MentalBreakDef>)this).GetEnumerator();
		}
	}

	protected Pawn pawn;

	public List<Trait> allTraits = new List<Trait>();

	private bool anyTraitHasIngestibleOverrides;

	private Dictionary<NeedDef, Trait> cachedEnabledNeeds;

	private Dictionary<NeedDef, Trait> cachedDisabledNeeds;

	private readonly List<Trait> tmpTraits = new List<Trait>();

	public bool AnyTraitHasIngestibleOverrides => anyTraitHasIngestibleOverrides;

	public float HungerRateFactor
	{
		get
		{
			float num = 1f;
			foreach (Trait allTrait in allTraits)
			{
				if (!allTrait.Suppressed)
				{
					num *= allTrait.CurrentData.hungerRateFactor;
				}
			}
			return num;
		}
	}

	public List<Trait> TraitsSorted
	{
		get
		{
			tmpTraits.Clear();
			if (!allTraits.NullOrEmpty())
			{
				for (int i = 0; i < allTraits.Count; i++)
				{
					if (allTraits[i].sourceGene == null || !allTraits[i].Suppressed)
					{
						tmpTraits.Add(allTraits[i]);
					}
				}
				tmpTraits.SortBy((Trait y) => y.sourceGene != null, (Trait x) => x.Suppressed);
			}
			return tmpTraits;
		}
	}

	public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
	{
		[IteratorStateMachine(typeof(_003Cget_TheOnlyAllowedMentalBreaks_003Ed__30))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_TheOnlyAllowedMentalBreaks_003Ed__30(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public TraitSet(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref allTraits, "allTraits", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			if (allTraits.RemoveAll((Trait x) => x == null) != 0)
			{
				Log.Error("Some traits were null after loading.");
			}
			if (allTraits.RemoveAll((Trait x) => x.def == null) != 0)
			{
				Log.Error("Some traits had null def after loading.");
			}
			for (int i = 0; i < allTraits.Count; i++)
			{
				allTraits[i].pawn = pawn;
			}
		}
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		RecacheTraits();
		for (int num = allTraits.Count - 1; num >= 0; num--)
		{
			Trait t = allTraits[num];
			if (t.sourceGene?.def != null && (t.sourceGene.def.forcedTraits == null || !t.sourceGene.def.forcedTraits.Any((GeneticTraitData x) => x.def == t.def && x.degree == t.Degree)))
			{
				allTraits.Remove(t);
			}
		}
	}

	public bool DisableHostilityFrom(Pawn p)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (!allTraits[i].Suppressed)
			{
				if (p.Faction != null && allTraits[i].def.disableHostilityFromFaction == p.Faction.def)
				{
					return true;
				}
				if (allTraits[i].def.disableHostilityFromAnimalType.HasValue && allTraits[i].def.disableHostilityFromAnimalType == p.RaceProps.animalType)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsThoughtDisallowed(ThoughtDef thought)
	{
		if (pawn.story == null || thought == null)
		{
			return false;
		}
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].Suppressed)
			{
				continue;
			}
			TraitDegreeData currentData = allTraits[i].CurrentData;
			if (currentData.disallowedThoughts == null)
			{
				continue;
			}
			for (int j = 0; j < currentData.disallowedThoughts.Count; j++)
			{
				if (currentData.disallowedThoughts[j] == thought)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsThoughtFromIngestionDisallowed(ThoughtDef thought, ThingDef ingestible, MeatSourceCategory meatSourceCategory)
	{
		if (thought == null || ingestible == null)
		{
			return false;
		}
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].Suppressed)
			{
				continue;
			}
			TraitDegreeData currentData = allTraits[i].CurrentData;
			if (currentData.disallowedThoughtsFromIngestion == null)
			{
				continue;
			}
			for (int j = 0; j < currentData.disallowedThoughtsFromIngestion.Count; j++)
			{
				TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.disallowedThoughtsFromIngestion[j];
				if (traitIngestionThoughtsOverride.thoughts.NullOrEmpty())
				{
					continue;
				}
				if (traitIngestionThoughtsOverride.thing != null)
				{
					if (traitIngestionThoughtsOverride.thing != ingestible)
					{
						continue;
					}
				}
				else if (meatSourceCategory == MeatSourceCategory.NotMeat || traitIngestionThoughtsOverride.meatSource != meatSourceCategory)
				{
					continue;
				}
				for (int k = 0; k < traitIngestionThoughtsOverride.thoughts.Count; k++)
				{
					if (traitIngestionThoughtsOverride.thoughts[k] == thought)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void GetExtraThoughtsFromIngestion(List<ThoughtDef> buffer, ThingDef ingestible, MeatSourceCategory meatSourceCategory, bool direct)
	{
		if (ingestible == null || buffer == null)
		{
			return;
		}
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].Suppressed)
			{
				continue;
			}
			TraitDegreeData currentData = allTraits[i].CurrentData;
			if (currentData.extraThoughtsFromIngestion == null)
			{
				continue;
			}
			for (int j = 0; j < currentData.extraThoughtsFromIngestion.Count; j++)
			{
				TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.extraThoughtsFromIngestion[j];
				if (traitIngestionThoughtsOverride.thing != null)
				{
					if (traitIngestionThoughtsOverride.thing != ingestible)
					{
						continue;
					}
				}
				else if (meatSourceCategory == MeatSourceCategory.NotMeat || traitIngestionThoughtsOverride.meatSource != meatSourceCategory)
				{
					continue;
				}
				if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty())
				{
					buffer.AddRange(traitIngestionThoughtsOverride.thoughts);
				}
				if (direct)
				{
					if (!traitIngestionThoughtsOverride.thoughtsDirect.NullOrEmpty())
					{
						buffer.AddRange(traitIngestionThoughtsOverride.thoughtsDirect);
					}
				}
				else if (!traitIngestionThoughtsOverride.thoughtsAsIngredient.NullOrEmpty())
				{
					buffer.AddRange(traitIngestionThoughtsOverride.thoughtsAsIngredient);
				}
			}
		}
	}

	public void GainTrait(Trait trait, bool suppressConflicts = false)
	{
		if (!suppressConflicts && HasTrait(trait.def))
		{
			Log.Warning(pawn?.ToString() + " already has trait " + trait.def);
		}
		else
		{
			if (HasNonSuppressedTrait(trait))
			{
				return;
			}
			allTraits.Add(trait);
			trait.pawn = pawn;
			if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && suppressConflicts)
			{
				if (allTraits.Any((Trait x) => x != trait && trait.def.CanSuppress(x) && !x.def.canBeSuppressed))
				{
					trait.suppressedByTrait = true;
				}
				else
				{
					for (int i = 0; i < allTraits.Count; i++)
					{
						if (allTraits[i] != trait && trait.def.CanSuppress(allTraits[i]))
						{
							allTraits[i].suppressedByTrait = true;
						}
					}
				}
				RecalculateSuppression();
			}
			pawn.Notify_DisabledWorkTypesChanged();
			if (trait.CurrentData.HasDefinedGraphicProperties)
			{
				pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (pawn.skills != null)
			{
				pawn.skills.Notify_SkillDisablesChanged();
				pawn.skills.DirtyAptitudes();
			}
			if (!pawn.Dead && pawn.RaceProps.Humanlike && pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty())
			{
				for (int j = 0; j < abilities.Count; j++)
				{
					pawn.abilities.GainAbility(abilities[j]);
				}
			}
			if (((trait.def.disableHostilityFromAnimalType.HasValue && trait.def.disableHostilityFromAnimalType != AnimalType.None) || trait.def.disableHostilityFromFaction != null) && pawn.Map != null)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			RecacheTraits();
			if (!trait.CurrentData.enablesNeeds.NullOrEmpty() || !trait.CurrentData.disablesNeeds.NullOrEmpty())
			{
				pawn.needs?.AddOrRemoveNeedsAsAppropriate();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
		}
	}

	public void RemoveTrait(Trait trait, bool unsuppressConflicts = false)
	{
		if (!HasTrait(trait.def))
		{
			Log.Warning("Trying to remove " + trait.Label + " but " + pawn?.ToString() + " doesn't have it.");
			return;
		}
		List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
		if (!abilities.NullOrEmpty())
		{
			for (int i = 0; i < abilities.Count; i++)
			{
				pawn.abilities.RemoveAbility(abilities[i]);
			}
		}
		allTraits.Remove(trait);
		if (trait.sourceGene != null)
		{
			pawn.genes.RemoveGene(trait.sourceGene);
		}
		if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && unsuppressConflicts)
		{
			for (int j = 0; j < allTraits.Count; j++)
			{
				if (!allTraits[j].Suppressed)
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < allTraits.Count; k++)
				{
					if (allTraits[j] != allTraits[k] && (allTraits[k].sourceGene == null || !allTraits[k].sourceGene.Overridden) && allTraits[j].def.CanSuppress(allTraits[k]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					allTraits[j].suppressedByTrait = false;
				}
			}
		}
		pawn.Notify_DisabledWorkTypesChanged();
		if (trait.CurrentData.HasDefinedGraphicProperties)
		{
			pawn.Drawer.renderer.SetAllGraphicsDirty();
		}
		if (pawn.skills != null)
		{
			pawn.skills.Notify_SkillDisablesChanged();
			pawn.skills.DirtyAptitudes();
		}
		if (!pawn.Dead && pawn.RaceProps.Humanlike && pawn.needs.mood != null)
		{
			pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
		}
		if (((trait.def.disableHostilityFromAnimalType.HasValue && trait.def.disableHostilityFromAnimalType != AnimalType.None) || trait.def.disableHostilityFromFaction != null) && pawn.Map != null)
		{
			pawn.Map.attackTargetsCache.UpdateTarget(pawn);
		}
		RecacheTraits();
		if (!trait.CurrentData.enablesNeeds.NullOrEmpty() || !trait.CurrentData.disablesNeeds.NullOrEmpty())
		{
			pawn.needs?.AddOrRemoveNeedsAsAppropriate();
		}
		MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
	}

	public void Notify_GeneRemoved(Gene gene)
	{
		if (!ModsConfig.BiotechActive)
		{
			return;
		}
		for (int num = allTraits.Count - 1; num >= 0; num--)
		{
			if (allTraits[num].sourceGene == gene)
			{
				RemoveTrait(allTraits[num], unsuppressConflicts: true);
			}
		}
		RecalculateSuppression();
	}

	public void RecalculateSuppression()
	{
		if (!ModsConfig.BiotechActive || pawn.genes == null)
		{
			return;
		}
		List<Gene> genesListForReading = pawn.genes.GenesListForReading;
		int i;
		for (i = 0; i < allTraits.Count; i++)
		{
			bool flag = false;
			if (allTraits[i].sourceGene != null && allTraits[i].sourceGene.Overridden)
			{
				allTraits[i].suppressedByGene = allTraits[i].sourceGene.overriddenByGene;
				flag = true;
			}
			else
			{
				for (int j = 0; j < genesListForReading.Count; j++)
				{
					if (genesListForReading[j].Active && genesListForReading[j].def.suppressedTraits != null && genesListForReading[j].def.suppressedTraits.Any((GeneticTraitData x) => x.def == allTraits[i].def && x.degree == allTraits[i].Degree))
					{
						allTraits[i].suppressedByGene = genesListForReading[j];
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				allTraits[i].suppressedByGene = null;
			}
		}
	}

	public bool HasTrait(TraitDef tDef)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].def == tDef && !allTraits[i].Suppressed)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasTrait(TraitDef tDef, int degree)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].def == tDef && allTraits[i].Degree == degree && !allTraits[i].Suppressed)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasNonSuppressedTrait(Trait trait)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			Trait trait2 = allTraits[i];
			if ((trait.sourceGene == null || trait2.sourceGene == null || !trait.sourceGene.def.Overrides(trait2.sourceGene.def, pawn.genes.IsXenogene(trait.sourceGene), pawn.genes.IsXenogene(trait2.sourceGene))) && trait2.def == trait.def && trait2.Degree == trait.Degree && !trait2.Suppressed)
			{
				return true;
			}
		}
		return false;
	}

	public bool TryGetNeedEnablingTrait(NeedDef def, out Trait trait)
	{
		if (cachedEnabledNeeds == null)
		{
			trait = null;
			return false;
		}
		return cachedEnabledNeeds.TryGetValue(def, out trait);
	}

	public bool TryGetNeedDisablingTrait(NeedDef def, out Trait trait)
	{
		if (cachedDisabledNeeds == null)
		{
			trait = null;
			return false;
		}
		return cachedDisabledNeeds.TryGetValue(def, out trait);
	}

	public bool EnablesNeed(NeedDef def)
	{
		return cachedEnabledNeeds?.ContainsKey(def) ?? false;
	}

	public bool DisablesNeed(NeedDef def)
	{
		return cachedDisabledNeeds?.ContainsKey(def) ?? false;
	}

	public Trait GetTrait(TraitDef tDef)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].def == tDef && !allTraits[i].Suppressed)
			{
				return allTraits[i];
			}
		}
		return null;
	}

	public Trait GetTrait(TraitDef tDef, int degree)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].def == tDef && allTraits[i].Degree == degree && !allTraits[i].Suppressed)
			{
				return allTraits[i];
			}
		}
		return null;
	}

	public int DegreeOfTrait(TraitDef tDef)
	{
		for (int i = 0; i < allTraits.Count; i++)
		{
			if (allTraits[i].def == tDef && !allTraits[i].Suppressed)
			{
				return allTraits[i].Degree;
			}
		}
		return 0;
	}

	private void RecacheTraits()
	{
		anyTraitHasIngestibleOverrides = false;
		cachedDisabledNeeds?.Clear();
		cachedEnabledNeeds?.Clear();
		for (int i = 0; i < allTraits.Count; i++)
		{
			Trait trait = allTraits[i];
			if (!trait.Suppressed && !trait.CurrentData.ingestibleModifiers.NullOrEmpty())
			{
				anyTraitHasIngestibleOverrides = true;
			}
			if (!trait.Suppressed && !trait.CurrentData.enablesNeeds.NullOrEmpty())
			{
				for (int j = 0; j < trait.CurrentData.enablesNeeds.Count; j++)
				{
					NeedDef key = trait.CurrentData.enablesNeeds[j];
					if (cachedEnabledNeeds == null)
					{
						cachedEnabledNeeds = new Dictionary<NeedDef, Trait>();
					}
					cachedEnabledNeeds[key] = trait;
				}
			}
			if (trait.Suppressed || trait.CurrentData.disablesNeeds.NullOrEmpty())
			{
				continue;
			}
			for (int k = 0; k < trait.CurrentData.disablesNeeds.Count; k++)
			{
				NeedDef key2 = trait.CurrentData.disablesNeeds[k];
				if (cachedDisabledNeeds == null)
				{
					cachedDisabledNeeds = new Dictionary<NeedDef, Trait>();
				}
				cachedDisabledNeeds[key2] = trait;
			}
		}
	}
}
