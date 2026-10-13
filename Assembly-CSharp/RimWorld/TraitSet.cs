using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200254E RID: 9550
	public class TraitSet : IExposable
	{
		// Token: 0x1700203D RID: 8253
		// (get) Token: 0x0600D115 RID: 53525 RVA: 0x003D7A4B File Offset: 0x003D5C4B
		public bool AnyTraitHasIngestibleOverrides
		{
			get
			{
				return this.anyTraitHasIngestibleOverrides;
			}
		}

		// Token: 0x1700203E RID: 8254
		// (get) Token: 0x0600D116 RID: 53526 RVA: 0x003D7A54 File Offset: 0x003D5C54
		public float HungerRateFactor
		{
			get
			{
				float num = 1f;
				foreach (Trait trait in this.allTraits)
				{
					if (!trait.Suppressed)
					{
						num *= trait.CurrentData.hungerRateFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x1700203F RID: 8255
		// (get) Token: 0x0600D117 RID: 53527 RVA: 0x003D7AC0 File Offset: 0x003D5CC0
		public List<Trait> TraitsSorted
		{
			get
			{
				this.tmpTraits.Clear();
				if (!this.allTraits.NullOrEmpty<Trait>())
				{
					for (int i = 0; i < this.allTraits.Count; i++)
					{
						if (this.allTraits[i].sourceGene == null || !this.allTraits[i].Suppressed)
						{
							this.tmpTraits.Add(this.allTraits[i]);
						}
					}
					this.tmpTraits.SortBy((Trait y) => y.sourceGene != null, (Trait x) => x.Suppressed);
				}
				return this.tmpTraits;
			}
		}

		// Token: 0x0600D118 RID: 53528 RVA: 0x003D7B8A File Offset: 0x003D5D8A
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600D119 RID: 53529 RVA: 0x003D7BB0 File Offset: 0x003D5DB0
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allTraits.RemoveAll((Trait x) => x == null) != 0)
				{
					Log.Error("Some traits were null after loading.");
				}
				if (this.allTraits.RemoveAll((Trait x) => x.def == null) != 0)
				{
					Log.Error("Some traits had null def after loading.");
				}
				for (int i = 0; i < this.allTraits.Count; i++)
				{
					this.allTraits[i].pawn = this.pawn;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheTraits();
				for (int j = this.allTraits.Count - 1; j >= 0; j--)
				{
					Trait t = this.allTraits[j];
					Gene sourceGene = t.sourceGene;
					if (((sourceGene != null) ? sourceGene.def : null) != null && (t.sourceGene.def.forcedTraits == null || !t.sourceGene.def.forcedTraits.Any((GeneticTraitData x) => x.def == t.def && x.degree == t.Degree)))
					{
						this.allTraits.Remove(t);
					}
				}
			}
		}

		// Token: 0x0600D11A RID: 53530 RVA: 0x003D7D28 File Offset: 0x003D5F28
		public bool DisableHostilityFrom(Pawn p)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					if (p.Faction != null && this.allTraits[i].def.disableHostilityFromFaction == p.Faction.def)
					{
						return true;
					}
					if (this.allTraits[i].def.disableHostilityFromAnimalType != null)
					{
						AnimalType? disableHostilityFromAnimalType = this.allTraits[i].def.disableHostilityFromAnimalType;
						AnimalType animalType = p.RaceProps.animalType;
						if (disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D11B RID: 53531 RVA: 0x003D7DEC File Offset: 0x003D5FEC
		public bool IsThoughtDisallowed(ThoughtDef thought)
		{
			if (this.pawn.story == null || thought == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.disallowedThoughts != null)
					{
						for (int j = 0; j < currentData.disallowedThoughts.Count; j++)
						{
							if (currentData.disallowedThoughts[j] == thought)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D11C RID: 53532 RVA: 0x003D7E78 File Offset: 0x003D6078
		public bool IsThoughtFromIngestionDisallowed(ThoughtDef thought, ThingDef ingestible, MeatSourceCategory meatSourceCategory)
		{
			if (thought == null || ingestible == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.disallowedThoughtsFromIngestion != null)
					{
						for (int j = 0; j < currentData.disallowedThoughtsFromIngestion.Count; j++)
						{
							TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.disallowedThoughtsFromIngestion[j];
							if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
							{
								if (traitIngestionThoughtsOverride.thing != null)
								{
									if (traitIngestionThoughtsOverride.thing != ingestible)
									{
										goto IL_A8;
									}
								}
								else if (meatSourceCategory == MeatSourceCategory.NotMeat || traitIngestionThoughtsOverride.meatSource != meatSourceCategory)
								{
									goto IL_A8;
								}
								for (int k = 0; k < traitIngestionThoughtsOverride.thoughts.Count; k++)
								{
									if (traitIngestionThoughtsOverride.thoughts[k] == thought)
									{
										return true;
									}
								}
							}
							IL_A8:;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D11D RID: 53533 RVA: 0x003D7F58 File Offset: 0x003D6158
		public void GetExtraThoughtsFromIngestion(List<ThoughtDef> buffer, ThingDef ingestible, MeatSourceCategory meatSourceCategory, bool direct)
		{
			if (ingestible == null || buffer == null)
			{
				return;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.extraThoughtsFromIngestion != null)
					{
						int j = 0;
						while (j < currentData.extraThoughtsFromIngestion.Count)
						{
							TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.extraThoughtsFromIngestion[j];
							if (traitIngestionThoughtsOverride.thing != null)
							{
								if (traitIngestionThoughtsOverride.thing == ingestible)
								{
									goto IL_74;
								}
							}
							else if (meatSourceCategory != MeatSourceCategory.NotMeat && traitIngestionThoughtsOverride.meatSource == meatSourceCategory)
							{
								goto IL_74;
							}
							IL_C5:
							j++;
							continue;
							IL_74:
							if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
							{
								buffer.AddRange(traitIngestionThoughtsOverride.thoughts);
							}
							if (direct)
							{
								if (!traitIngestionThoughtsOverride.thoughtsDirect.NullOrEmpty<ThoughtDef>())
								{
									buffer.AddRange(traitIngestionThoughtsOverride.thoughtsDirect);
									goto IL_C5;
								}
								goto IL_C5;
							}
							else
							{
								if (!traitIngestionThoughtsOverride.thoughtsAsIngredient.NullOrEmpty<ThoughtDef>())
								{
									buffer.AddRange(traitIngestionThoughtsOverride.thoughtsAsIngredient);
									goto IL_C5;
								}
								goto IL_C5;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600D11E RID: 53534 RVA: 0x003D8054 File Offset: 0x003D6254
		public void GainTrait(Trait trait, bool suppressConflicts = false)
		{
			if (!suppressConflicts && this.HasTrait(trait.def))
			{
				Pawn pawn = this.pawn;
				string str = (pawn != null) ? pawn.ToString() : null;
				string str2 = " already has trait ";
				TraitDef def = trait.def;
				Log.Warning(str + str2 + ((def != null) ? def.ToString() : null));
				return;
			}
			if (this.HasTrait(trait.def, trait.Degree))
			{
				return;
			}
			this.allTraits.Add(trait);
			trait.pawn = this.pawn;
			if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && suppressConflicts)
			{
				if (this.allTraits.Any((Trait x) => x != trait && trait.def.CanSuppress(x) && !x.def.canBeSuppressed))
				{
					trait.suppressedByTrait = true;
				}
				else
				{
					for (int i = 0; i < this.allTraits.Count; i++)
					{
						if (this.allTraits[i] != trait && trait.def.CanSuppress(this.allTraits[i]))
						{
							this.allTraits[i].suppressedByTrait = true;
						}
					}
				}
				this.RecalculateSuppression();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (trait.CurrentData.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
				this.pawn.skills.DirtyAptitudes();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int j = 0; j < abilities.Count; j++)
				{
					this.pawn.abilities.GainAbility(abilities[j]);
				}
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_28D;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_2B5;
			}
			IL_28D:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_2B5:
			this.RecacheTraits();
			if (!trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>() || !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x0600D11F RID: 53535 RVA: 0x003D836C File Offset: 0x003D656C
		public void RemoveTrait(Trait trait, bool unsuppressConflicts = false)
		{
			if (!this.HasTrait(trait.def))
			{
				string[] array = new string[5];
				array[0] = "Trying to remove ";
				array[1] = trait.Label;
				array[2] = " but ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = " doesn't have it.";
				Log.Warning(string.Concat(array));
				return;
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < abilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(abilities[i]);
				}
			}
			this.allTraits.Remove(trait);
			if (trait.sourceGene != null)
			{
				this.pawn.genes.RemoveGene(trait.sourceGene);
			}
			if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && unsuppressConflicts)
			{
				for (int j = 0; j < this.allTraits.Count; j++)
				{
					if (this.allTraits[j].Suppressed)
					{
						bool flag = false;
						for (int k = 0; k < this.allTraits.Count; k++)
						{
							if (this.allTraits[j] != this.allTraits[k] && (this.allTraits[k].sourceGene == null || !this.allTraits[k].sourceGene.Overridden) && this.allTraits[j].def.CanSuppress(this.allTraits[k]))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.allTraits[j].suppressedByTrait = false;
						}
					}
				}
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (trait.CurrentData.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
				this.pawn.skills.DirtyAptitudes();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_2A2;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_2CA;
			}
			IL_2A2:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_2CA:
			this.RecacheTraits();
			if (!trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>() || !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x0600D120 RID: 53536 RVA: 0x003D8690 File Offset: 0x003D6890
		public void Notify_GeneRemoved(Gene gene)
		{
			if (ModsConfig.BiotechActive)
			{
				for (int i = this.allTraits.Count - 1; i >= 0; i--)
				{
					if (this.allTraits[i].sourceGene == gene)
					{
						this.RemoveTrait(this.allTraits[i], true);
					}
				}
				this.RecalculateSuppression();
			}
		}

		// Token: 0x0600D121 RID: 53537 RVA: 0x003D86EC File Offset: 0x003D68EC
		public void RecalculateSuppression()
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return;
			}
			List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
			int i;
			Predicate<GeneticTraitData> <>9__0;
			int i2;
			for (i = 0; i < this.allTraits.Count; i = i2 + 1)
			{
				bool flag = false;
				if (this.allTraits[i].sourceGene != null && this.allTraits[i].sourceGene.Overridden)
				{
					this.allTraits[i].suppressedByGene = this.allTraits[i].sourceGene.overriddenByGene;
					flag = true;
				}
				else
				{
					for (int j = 0; j < genesListForReading.Count; j++)
					{
						if (genesListForReading[j].Active && genesListForReading[j].def.suppressedTraits != null)
						{
							List<GeneticTraitData> suppressedTraits = genesListForReading[j].def.suppressedTraits;
							Predicate<GeneticTraitData> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((GeneticTraitData x) => x.def == this.allTraits[i].def && x.degree == this.allTraits[i].Degree));
							}
							if (suppressedTraits.Any(predicate))
							{
								this.allTraits[i].suppressedByGene = genesListForReading[j];
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					this.allTraits[i].suppressedByGene = null;
				}
				i2 = i;
			}
		}

		// Token: 0x0600D122 RID: 53538 RVA: 0x003D8878 File Offset: 0x003D6A78
		public bool HasTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D123 RID: 53539 RVA: 0x003D88C8 File Offset: 0x003D6AC8
		public bool HasTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree && !this.allTraits[i].Suppressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D124 RID: 53540 RVA: 0x003D8929 File Offset: 0x003D6B29
		public bool TryGetNeedEnablingTrait(NeedDef def, out Trait trait)
		{
			if (this.cachedEnabledNeeds == null)
			{
				trait = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out trait);
		}

		// Token: 0x0600D125 RID: 53541 RVA: 0x003D8945 File Offset: 0x003D6B45
		public bool TryGetNeedDisablingTrait(NeedDef def, out Trait trait)
		{
			if (this.cachedDisabledNeeds == null)
			{
				trait = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out trait);
		}

		// Token: 0x0600D126 RID: 53542 RVA: 0x003D8961 File Offset: 0x003D6B61
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Trait> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x0600D127 RID: 53543 RVA: 0x003D8975 File Offset: 0x003D6B75
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Trait> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x17002040 RID: 8256
		// (get) Token: 0x0600D128 RID: 53544 RVA: 0x003D8989 File Offset: 0x003D6B89
		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				int num;
				for (int i = 0; i < this.allTraits.Count; i = num + 1)
				{
					Trait trait = this.allTraits[i];
					if (!trait.Suppressed)
					{
						if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
						{
							for (int j = 0; j < trait.CurrentData.theOnlyAllowedMentalBreaks.Count; j = num + 1)
							{
								yield return trait.CurrentData.theOnlyAllowedMentalBreaks[j];
								num = j;
							}
						}
						trait = null;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600D129 RID: 53545 RVA: 0x003D899C File Offset: 0x003D6B9C
		public Trait GetTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x0600D12A RID: 53546 RVA: 0x003D89F4 File Offset: 0x003D6BF4
		public Trait GetTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x0600D12B RID: 53547 RVA: 0x003D8A60 File Offset: 0x003D6C60
		public int DegreeOfTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i].Degree;
				}
			}
			return 0;
		}

		// Token: 0x0600D12C RID: 53548 RVA: 0x003D8AC0 File Offset: 0x003D6CC0
		private void RecacheTraits()
		{
			this.anyTraitHasIngestibleOverrides = false;
			Dictionary<NeedDef, Trait> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Trait> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				Trait trait = this.allTraits[i];
				if (!trait.Suppressed && !trait.CurrentData.ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
				{
					this.anyTraitHasIngestibleOverrides = true;
				}
				if (!trait.Suppressed && !trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int j = 0; j < trait.CurrentData.enablesNeeds.Count; j++)
					{
						NeedDef key = trait.CurrentData.enablesNeeds[j];
						if (this.cachedEnabledNeeds == null)
						{
							this.cachedEnabledNeeds = new Dictionary<NeedDef, Trait>();
						}
						this.cachedEnabledNeeds[key] = trait;
					}
				}
				if (!trait.Suppressed && !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int k = 0; k < trait.CurrentData.disablesNeeds.Count; k++)
					{
						NeedDef key2 = trait.CurrentData.disablesNeeds[k];
						if (this.cachedDisabledNeeds == null)
						{
							this.cachedDisabledNeeds = new Dictionary<NeedDef, Trait>();
						}
						this.cachedDisabledNeeds[key2] = trait;
					}
				}
			}
		}

		// Token: 0x04008E06 RID: 36358
		protected Pawn pawn;

		// Token: 0x04008E07 RID: 36359
		public List<Trait> allTraits = new List<Trait>();

		// Token: 0x04008E08 RID: 36360
		private bool anyTraitHasIngestibleOverrides;

		// Token: 0x04008E09 RID: 36361
		private Dictionary<NeedDef, Trait> cachedEnabledNeeds;

		// Token: 0x04008E0A RID: 36362
		private Dictionary<NeedDef, Trait> cachedDisabledNeeds;

		// Token: 0x04008E0B RID: 36363
		private readonly List<Trait> tmpTraits = new List<Trait>();
	}
}
