using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200249C RID: 9372
	public class Pawn_AbilityTracker : IExposable
	{
		// Token: 0x17001EF8 RID: 7928
		// (get) Token: 0x0600CB42 RID: 52034 RVA: 0x003B8C14 File Offset: 0x003B6E14
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.allAbilitiesCachedDirty)
				{
					this.allAbilitiesCached.Clear();
					this.allAbilitiesCached.AddRange(this.abilities);
					if (this.pawn.health != null)
					{
						foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
						{
							if (!hediff.def.abilities.NullOrEmpty<AbilityDef>())
							{
								this.allAbilitiesCached.AddRange(hediff.AllAbilitiesForReading);
							}
						}
					}
					if (this.pawn.equipment != null)
					{
						ThingWithComps primary = this.pawn.equipment.Primary;
						CompEquippableAbility compEquippableAbility = (primary != null) ? primary.TryGetComp<CompEquippableAbility>() : null;
						if (compEquippableAbility != null && compEquippableAbility.AbilityForReading != null)
						{
							this.allAbilitiesCached.Add(compEquippableAbility.AbilityForReading);
						}
					}
					if (this.pawn.apparel != null)
					{
						foreach (Apparel apparel in this.pawn.apparel.WornApparel)
						{
							this.allAbilitiesCached.AddRange(apparel.AllAbilitiesForReading);
						}
					}
					if (ModsConfig.AnomalyActive && this.pawn.IsMutant)
					{
						this.allAbilitiesCached.AddRange(this.pawn.mutant.AllAbilitiesForReading);
					}
					if (this.pawn.royalty != null)
					{
						this.allAbilitiesCached.AddRange(this.pawn.royalty.AllAbilitiesForReading);
					}
					if (ModsConfig.IdeologyActive)
					{
						Ideo ideo = this.pawn.Ideo;
						Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(this.pawn) : null;
						if (precept_Role != null && precept_Role.Active && !precept_Role.AbilitiesFor(this.pawn).NullOrEmpty<Ability>())
						{
							foreach (Ability ability in precept_Role.AbilitiesFor(this.pawn))
							{
								bool flag = false;
								if (!ability.def.requiredMemes.NullOrEmpty<MemeDef>())
								{
									foreach (MemeDef item in ability.def.requiredMemes)
									{
										if (!this.pawn.Ideo.memes.Contains(item))
										{
											flag = true;
											break;
										}
									}
								}
								if (!flag)
								{
									this.allAbilitiesCached.Add(ability);
								}
							}
						}
					}
					if (ModsConfig.AnomalyActive && this.pawn.IsMutant && this.pawn.mutant.Def.abilityWhitelist.Any<AbilityDef>())
					{
						this.allAbilitiesCached = (from a in this.allAbilitiesCached
						where this.pawn.mutant.Def.abilityWhitelist.Contains(a.def)
						select a).ToList<Ability>();
					}
					this.allAbilitiesCached.SortBy(delegate(Ability a)
					{
						AbilityCategoryDef category = a.def.category;
						if (category == null)
						{
							return 0;
						}
						return category.displayOrder;
					}, (Ability a) => a.def.displayOrder, (Ability a) => a.def.level);
					this.allAbilitiesCachedDirty = false;
				}
				return this.allAbilitiesCached;
			}
		}

		// Token: 0x0600CB43 RID: 52035 RVA: 0x003B8FB4 File Offset: 0x003B71B4
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CB44 RID: 52036 RVA: 0x003B8FEC File Offset: 0x003B71EC
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.AllAbilitiesForReading.Count; i++)
			{
				this.AllAbilitiesForReading[i].AbilityTick();
			}
		}

		// Token: 0x0600CB45 RID: 52037 RVA: 0x003B9020 File Offset: 0x003B7220
		public void GainAbility(AbilityDef def)
		{
			if (!this.abilities.Any((Ability a) => a.def == def))
			{
				this.abilities.Add(AbilityUtility.MakeAbility(def, this.pawn));
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600CB46 RID: 52038 RVA: 0x003B9078 File Offset: 0x003B7278
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600CB47 RID: 52039 RVA: 0x003B90C0 File Offset: 0x003B72C0
		public Ability GetAbility(AbilityDef def, bool includeTemporary = false)
		{
			List<Ability> list = includeTemporary ? this.AllAbilitiesForReading : this.abilities;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600CB48 RID: 52040 RVA: 0x003B9108 File Offset: 0x003B7308
		public List<Ability> AICastableAbilities(LocalTargetInfo target, bool offensive)
		{
			this.tmpAbilities.Clear();
			foreach (Ability ability in this.AllAbilitiesForReading)
			{
				bool flag = true;
				if (this.pawn.IsAnimal)
				{
					Faction faction = this.pawn.Faction;
					if (faction != null && faction.IsPlayer && this.pawn.RaceProps.specialTrainables != null)
					{
						foreach (TrainableDef trainableDef in this.pawn.RaceProps.specialTrainables)
						{
							if (trainableDef.enablesAbility == ability.def)
							{
								Pawn_TrainingTracker training = this.pawn.training;
								if (training != null && training.HasLearned(trainableDef))
								{
									flag = false;
									break;
								}
							}
						}
					}
				}
				if (flag && ability.def.ai_IsOffensive == offensive && (ability.AICanTargetNow(target) || ability.AICanTargetNow(this.pawn)))
				{
					this.tmpAbilities.Add(ability);
				}
			}
			return this.tmpAbilities;
		}

		// Token: 0x0600CB49 RID: 52041 RVA: 0x003B9258 File Offset: 0x003B7458
		public IEnumerable<Gizmo> GetGizmos()
		{
			bool visiblePrimary = this.pawn.IsColonistPlayerControlled || this.pawn.IsColonyMechPlayerControlled || this.pawn.IsColonySubhumanPlayerControlled;
			foreach (Ability a in this.AllAbilitiesForReading)
			{
				if (!visiblePrimary && this.pawn.training != null && this.pawn.RaceProps.specialTrainables != null)
				{
					foreach (TrainableDef trainableDef in this.pawn.RaceProps.specialTrainables)
					{
						if (trainableDef.enablesAbility == a.def && this.pawn.training.HasLearned(trainableDef))
						{
							visiblePrimary = true;
						}
					}
				}
				if (visiblePrimary || DebugSettings.ShowDevGizmos)
				{
					bool visibleSecondary = (this.pawn.Drafted || a.def.displayGizmoWhileUndrafted) && a.GizmosVisible();
					if (visibleSecondary || (!this.pawn.IsColonistPlayerControlled && !this.pawn.IsColonyMechPlayerControlled && DebugSettings.ShowDevGizmos))
					{
						foreach (Command command in a.GetGizmos())
						{
							Command_Ability command_Ability = command as Command_Ability;
							if (command_Ability != null)
							{
								command_Ability.devGizmo = (!visiblePrimary && !visibleSecondary && DebugSettings.ShowDevGizmos);
							}
							yield return command;
						}
						IEnumerator<Command> enumerator3 = null;
					}
				}
				foreach (Gizmo gizmo in a.GetGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator4 = null;
				a = null;
			}
			List<Ability>.Enumerator enumerator = default(List<Ability>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CB4A RID: 52042 RVA: 0x003B9268 File Offset: 0x003B7468
		public void Notify_TemporaryAbilitiesChanged()
		{
			this.allAbilitiesCachedDirty = true;
		}

		// Token: 0x0600CB4B RID: 52043 RVA: 0x003B9274 File Offset: 0x003B7474
		public void ExposeData()
		{
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.abilities.RemoveAll((Ability a) => a.def == null || a.def == AbilityDefOf.Speech);
			}
		}

		// Token: 0x04008AA6 RID: 35494
		public Pawn pawn;

		// Token: 0x04008AA7 RID: 35495
		public List<Ability> abilities = new List<Ability>();

		// Token: 0x04008AA8 RID: 35496
		private bool allAbilitiesCachedDirty = true;

		// Token: 0x04008AA9 RID: 35497
		private List<Ability> allAbilitiesCached = new List<Ability>();

		// Token: 0x04008AAA RID: 35498
		private readonly List<Ability> tmpAbilities = new List<Ability>();
	}
}
