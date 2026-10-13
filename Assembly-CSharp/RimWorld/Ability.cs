using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021DA RID: 8666
	public class Ability : IVerbOwner, IExposable, ILoadReferenceable
	{
		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x0600BD76 RID: 48502 RVA: 0x0036EC3B File Offset: 0x0036CE3B
		public Verb verb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x0600BD77 RID: 48503 RVA: 0x0036EC48 File Offset: 0x0036CE48
		public string UniqueVerbOwnerID()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x0600BD78 RID: 48504 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x0600BD79 RID: 48505 RVA: 0x0036EC50 File Offset: 0x0036CE50
		// (set) Token: 0x0600BD7A RID: 48506 RVA: 0x0036EC58 File Offset: 0x0036CE58
		public List<Tool> Tools { get; private set; }

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x0600BD7B RID: 48507 RVA: 0x0036EC61 File Offset: 0x0036CE61
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x0600BD7C RID: 48508 RVA: 0x0036EC69 File Offset: 0x0036CE69
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return new List<VerbProperties>
				{
					this.def.verbProperties
				};
			}
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x0600BD7D RID: 48509 RVA: 0x0036EC81 File Offset: 0x0036CE81
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x0600BD7E RID: 48510 RVA: 0x0036EC88 File Offset: 0x0036CE88
		public int CooldownTicksRemaining
		{
			get
			{
				if (this.inCooldown)
				{
					return Mathf.Max(this.cooldownEndTick - GenTicks.TicksGame, 0);
				}
				return 0;
			}
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x0600BD7F RID: 48511 RVA: 0x0036ECA6 File Offset: 0x0036CEA6
		public int CooldownTicksTotal
		{
			get
			{
				return this.cooldownDuration;
			}
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x0600BD80 RID: 48512 RVA: 0x0036ECAE File Offset: 0x0036CEAE
		public bool UsesCharges
		{
			get
			{
				return this.maxCharges > 0;
			}
		}

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x0600BD81 RID: 48513 RVA: 0x0036ECB9 File Offset: 0x0036CEB9
		// (set) Token: 0x0600BD82 RID: 48514 RVA: 0x0036ECC1 File Offset: 0x0036CEC1
		public int RemainingCharges
		{
			get
			{
				return this.charges;
			}
			set
			{
				this.charges = value;
			}
		}

		// Token: 0x17001CEB RID: 7403
		// (get) Token: 0x0600BD83 RID: 48515 RVA: 0x0036ECCA File Offset: 0x0036CECA
		public string GizmoExtraLabel
		{
			get
			{
				if (!this.UsesCharges)
				{
					return null;
				}
				return string.Format("{0} / {1}", this.charges, this.maxCharges);
			}
		}

		// Token: 0x17001CEC RID: 7404
		// (get) Token: 0x0600BD84 RID: 48516 RVA: 0x0036ECF6 File Offset: 0x0036CEF6
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17001CED RID: 7405
		// (get) Token: 0x0600BD85 RID: 48517 RVA: 0x0036ED14 File Offset: 0x0036CF14
		public bool HasCooldown
		{
			get
			{
				return this.def.cooldownTicksRange != default(IntRange) || (this.def.groupDef != null && this.def.groupDef.cooldownTicks > 0) || this.def.hasExternallyHandledCooldown;
			}
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x0600BD86 RID: 48518 RVA: 0x0036ED69 File Offset: 0x0036CF69
		public bool OnCooldown
		{
			get
			{
				return this.HasCooldown && this.inCooldown;
			}
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x0600BD87 RID: 48519 RVA: 0x0036ED7C File Offset: 0x0036CF7C
		public virtual AcceptanceReport CanCast
		{
			get
			{
				if (!this.comps.NullOrEmpty<AbilityComp>())
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (!this.comps[i].CanCast)
						{
							return false;
						}
					}
				}
				if (this.inCooldown)
				{
					return this.def.cooldownPerCharge && this.charges > 0;
				}
				if (this.UsesCharges)
				{
					return this.charges > 0;
				}
				if (this.pawn.MapHeld != null)
				{
					if (this.pawn.MapHeld.IsPocketMap)
					{
						if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInPocketMaps))
						{
							return "CannotUseReason_PocketMap".Translate(this.pawn.MapHeld.generatorDef.label);
						}
					}
					if (this.pawn.MapHeld.Biome.inVacuum)
					{
						if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInVacuum))
						{
							return "CannotFunctionInVacuum".Translate();
						}
					}
					PlanetTile tile = this.pawn.MapHeld.Tile;
					if (this.LayerPreventsCast(this.pawn.MapHeld.Tile))
					{
						PlanetLayerDef layerDef = tile.LayerDef;
						return "CannotPerformPlanetLayer".Translate(layerDef.gerundLabel.Named("GERUND"), layerDef.label.Named("LAYER"));
					}
				}
				return true;
			}
		}

		// Token: 0x0600BD88 RID: 48520 RVA: 0x0036EF38 File Offset: 0x0036D138
		private bool LayerPreventsCast(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return false;
			}
			foreach (VerbProperties verbProperties in this.VerbProperties)
			{
				if (!verbProperties.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !verbProperties.layerWhitelist.Contains(tile.LayerDef))
				{
					return true;
				}
				if (!verbProperties.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && verbProperties.layerBlacklist.Contains(tile.LayerDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001CF0 RID: 7408
		// (get) Token: 0x0600BD89 RID: 48521 RVA: 0x0036EFDC File Offset: 0x0036D1DC
		public bool Casting
		{
			get
			{
				if (!this.verb.WarmingUp)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					if (!(((jobs != null) ? jobs.curDriver : null) is JobDriver_CastAbilityWorld) || this.pawn.CurJob.ability != this)
					{
						Pawn pawn = this.pawn;
						return ((pawn != null) ? pawn.CurJobDef : null) != null && this.pawn.CurJobDef.abilityCasting && this.pawn.CurJob.ability == this;
					}
				}
				return true;
			}
		}

		// Token: 0x17001CF1 RID: 7409
		// (get) Token: 0x0600BD8A RID: 48522 RVA: 0x0036F064 File Offset: 0x0036D264
		public bool CanCooldown
		{
			get
			{
				if (!this.HasCooldown)
				{
					return false;
				}
				if (this.def.waitForJobEnd)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					JobDef jobDef;
					if (jobs == null)
					{
						jobDef = null;
					}
					else
					{
						Job curJob = jobs.curJob;
						jobDef = ((curJob != null) ? curJob.def : null);
					}
					return jobDef != this.def.jobDef;
				}
				return true;
			}
		}

		// Token: 0x17001CF2 RID: 7410
		// (get) Token: 0x0600BD8B RID: 48523 RVA: 0x0036F0C0 File Offset: 0x0036D2C0
		public virtual string Tooltip
		{
			get
			{
				string text = this.def.GetTooltip(this.pawn);
				if (this.UsesCharges)
				{
					if (this.def.cooldownPerCharge)
					{
						text = text + "\n\n" + "Uses".Translate().ToString() + string.Format(": {0} / {1}", this.charges, this.maxCharges);
					}
					else
					{
						text = text + "\n\n" + "Charges".Translate().ToString() + string.Format(": {0} / {1}", this.charges, this.maxCharges);
					}
				}
				if (this.EffectComps != null)
				{
					foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
					{
						string text2 = compAbilityEffect.ExtraTooltipPart();
						if (!text2.NullOrEmpty())
						{
							text = text + "\n\n" + text2;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x17001CF3 RID: 7411
		// (get) Token: 0x0600BD8C RID: 48524 RVA: 0x0036F1E4 File Offset: 0x0036D3E4
		public virtual bool CanQueueCast
		{
			get
			{
				if (!this.CanCast)
				{
					return false;
				}
				if (this.pawn.jobs == null)
				{
					return false;
				}
				int num = 0;
				foreach (Job j in this.pawn.jobs.AllJobs())
				{
					if (this.<get_CanQueueCast>g__SameForQueueing|63_0(j))
					{
						num++;
						if (!this.UsesCharges || num >= this.charges)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17001CF4 RID: 7412
		// (get) Token: 0x0600BD8D RID: 48525 RVA: 0x0036F27C File Offset: 0x0036D47C
		public List<CompAbilityEffect> EffectComps
		{
			get
			{
				if (this.effectComps == null)
				{
					IEnumerable<CompAbilityEffect> enumerable = this.CompsOfType<CompAbilityEffect>();
					this.effectComps = ((enumerable == null) ? new List<CompAbilityEffect>() : enumerable.ToList<CompAbilityEffect>());
				}
				return this.effectComps;
			}
		}

		// Token: 0x0600BD8E RID: 48526 RVA: 0x0036F2B4 File Offset: 0x0036D4B4
		public Ability()
		{
		}

		// Token: 0x0600BD8F RID: 48527 RVA: 0x0036F318 File Offset: 0x0036D518
		public Ability(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600BD90 RID: 48528 RVA: 0x0036F384 File Offset: 0x0036D584
		public Ability(Pawn pawn, Precept sourcePrecept)
		{
			this.pawn = pawn;
			this.sourcePrecept = sourcePrecept;
		}

		// Token: 0x0600BD91 RID: 48529 RVA: 0x0036F3F8 File Offset: 0x0036D5F8
		public Ability(Pawn pawn, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.Initialize();
		}

		// Token: 0x0600BD92 RID: 48530 RVA: 0x0036F470 File Offset: 0x0036D670
		public Ability(Pawn pawn, Precept sourcePrecept, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.sourcePrecept = sourcePrecept;
			this.Initialize();
		}

		// Token: 0x0600BD93 RID: 48531 RVA: 0x0036F4F0 File Offset: 0x0036D6F0
		public virtual bool CanApplyOn(LocalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.effectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target, null))
						{
							return false;
						}
					}
				}
			}
			return this.def.verbProperties.targetable || !this.def.verbProperties.targetParams.canTargetSelf || target == this.pawn;
		}

		// Token: 0x0600BD94 RID: 48532 RVA: 0x0036F594 File Offset: 0x0036D794
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			if (this.EffectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600BD95 RID: 48533 RVA: 0x0036F5F8 File Offset: 0x0036D7F8
		public virtual bool AICanTargetNow(LocalTargetInfo target)
		{
			if (!this.def.aiCanUse || !this.CanCast)
			{
				return false;
			}
			if (!this.CanApplyOn(target))
			{
				return false;
			}
			if (this.EffectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.AICanTargetNow(target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600BD96 RID: 48534 RVA: 0x0036F684 File Offset: 0x0036D884
		public virtual LocalTargetInfo AIGetAOETarget()
		{
			if (this.def.ai_SearchAOEForTargets)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.pawn.Position, this.pawn.Map, this.verb.EffectiveRange, true))
				{
					if (this.ValidAOEAffectedTarget(thing))
					{
						bool flag = true;
						using (List<CompAbilityEffect>.Enumerator enumerator2 = this.EffectComps.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (!enumerator2.Current.AICanTargetNow(thing))
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							return thing;
						}
					}
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x0600BD97 RID: 48535 RVA: 0x0036F768 File Offset: 0x0036D968
		public Window ConfirmationDialog(LocalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x0600BD98 RID: 48536 RVA: 0x0036F80C File Offset: 0x0036DA0C
		public Window ConfirmationDialog(GlobalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x0600BD99 RID: 48537 RVA: 0x0036F8B0 File Offset: 0x0036DAB0
		protected virtual void PreActivate(LocalTargetInfo? target)
		{
			if (this.UsesCharges)
			{
				this.charges--;
			}
			if (this.HasCooldown)
			{
				if (this.def.groupDef != null)
				{
					int num = this.def.overrideGroupCooldown ? this.def.cooldownTicksRange.RandomInRange : this.def.groupDef.cooldownTicks;
					foreach (Ability ability in this.pawn.abilities.AllAbilitiesForReading)
					{
						ability.Notify_GroupStartedCooldown(this.def.groupDef, num);
					}
					if (this.pawn.Ideo == null)
					{
						goto IL_191;
					}
					using (IEnumerator<Precept_Ritual> enumerator2 = this.pawn.Ideo.PreceptsListForReading.OfType<Precept_Ritual>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Precept_Ritual precept_Ritual = enumerator2.Current;
							if (precept_Ritual.def.useCooldownFromAbilityGroupDef == this.def.groupDef)
							{
								precept_Ritual.Notify_CooldownFromAbilityStarted(num);
							}
						}
						goto IL_191;
					}
				}
				if (this.UsesCharges)
				{
					if (this.def.cooldownPerCharge)
					{
						if (this.charges < this.maxCharges && this.CooldownTicksRemaining == 0)
						{
							this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
						}
					}
					else if (this.charges <= 0)
					{
						this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
					}
				}
				else
				{
					this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
				}
			}
			IL_191:
			Pawn pawn = this.ConstantCaster as Pawn;
			if (pawn != null)
			{
				Pawn_EquipmentTracker equipment = pawn.equipment;
				if (equipment != null)
				{
					equipment.Notify_AbilityUsed(this);
				}
			}
			if (this.def.writeCombatLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_AbilityUsed(this.pawn, (target != null) ? target.GetValueOrDefault().Thing : null, this.def, RulePackDefOf.Event_AbilityUsed));
			}
			this.customWarmupMotes.Clear();
		}

		// Token: 0x0600BD9A RID: 48538 RVA: 0x0036FAE0 File Offset: 0x0036DCE0
		public virtual bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			this.PreActivate(new LocalTargetInfo?(target));
			if (this.def.hostile && this.pawn.mindState != null)
			{
				this.pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
			if (this.EffectComps.Any<CompAbilityEffect>())
			{
				this.affectedTargetsCached.Clear();
				this.affectedTargetsCached.AddRange(this.GetAffectedTargets(target));
				this.ApplyEffects(this.EffectComps, this.affectedTargetsCached, dest);
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x0600BD9B RID: 48539 RVA: 0x0036FB78 File Offset: 0x0036DD78
		public virtual bool Activate(GlobalTargetInfo target)
		{
			this.PreActivate(null);
			if (this.def.hostile && this.pawn.mindState != null)
			{
				this.pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
			if (this.EffectComps.Any<CompAbilityEffect>())
			{
				this.ApplyEffects(this.EffectComps, target);
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x0600BD9C RID: 48540 RVA: 0x0036FBEE File Offset: 0x0036DDEE
		public IEnumerable<LocalTargetInfo> GetAffectedTargets(LocalTargetInfo target)
		{
			if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
			{
				foreach (LocalTargetInfo localTargetInfo in from t in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, this.def.EffectRadius, true).Where(new Func<Thing, bool>(this.ValidAOEAffectedTarget))
				select new LocalTargetInfo(t))
				{
					yield return localTargetInfo;
				}
				IEnumerator<LocalTargetInfo> enumerator = null;
			}
			else
			{
				yield return target;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BD9D RID: 48541 RVA: 0x0036FC08 File Offset: 0x0036DE08
		private bool ValidAOEAffectedTarget(Thing target)
		{
			if (!this.verb.targetParams.CanTarget(target, null))
			{
				return false;
			}
			if (target.Fogged())
			{
				return false;
			}
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				if (!this.EffectComps[i].Valid(target, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BD9E RID: 48542 RVA: 0x0036FC70 File Offset: 0x0036DE70
		public virtual void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			if (this.verb.verbProps.nonInterruptingSelfCast)
			{
				this.verb.TryStartCastOn(this.verb.Caster, false, true, false, false);
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				this.needToRecacheWarmupMotes = true;
				this.pawn.jobs.TryTakeOrderedJob(this.GetJob(target, destination), new JobTag?(JobTag.Misc), false);
			});
		}

		// Token: 0x0600BD9F RID: 48543 RVA: 0x0036FCFC File Offset: 0x0036DEFC
		public virtual Job GetJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnThing);
			job.verbToUse = this.verb;
			job.targetA = target;
			job.targetB = destination;
			job.ability = this;
			this.needToRecacheWarmupMotes = true;
			return job;
		}

		// Token: 0x0600BDA0 RID: 48544 RVA: 0x0036FD4C File Offset: 0x0036DF4C
		public virtual void QueueCastingJob(GlobalTargetInfo target)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				if (!this.pawn.IsCaravanMember())
				{
					Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnWorldTile);
					job.verbToUse = this.verb;
					job.globalTarget = target;
					job.ability = this;
					this.needToRecacheWarmupMotes = true;
					this.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					return;
				}
				this.Activate(target);
			});
		}

		// Token: 0x0600BDA1 RID: 48545 RVA: 0x0036FD9C File Offset: 0x0036DF9C
		private void ShowCastingConfirmationIfNeeded(LocalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x0600BDA2 RID: 48546 RVA: 0x0036FDC8 File Offset: 0x0036DFC8
		private void ShowCastingConfirmationIfNeeded(GlobalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x0600BDA3 RID: 48547 RVA: 0x0036FDF4 File Offset: 0x0036DFF4
		public bool ValidateGlobalTarget(GlobalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				if (!this.EffectComps[i].Valid(target, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BDA4 RID: 48548 RVA: 0x0036FE30 File Offset: 0x0036E030
		public virtual bool GizmoDisabled(out string reason)
		{
			if (this.CanCooldown && this.OnCooldown && (!this.def.cooldownPerCharge || this.charges == 0))
			{
				reason = "AbilityOnCooldown".Translate(this.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				return true;
			}
			if (this.UsesCharges && this.charges <= 0)
			{
				reason = "AbilityNoCharges".Translate();
				return true;
			}
			if (!this.comps.NullOrEmpty<AbilityComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].GizmoDisabled(out reason))
					{
						return true;
					}
				}
			}
			AcceptanceReport canCast = this.CanCast;
			if (!canCast.Accepted)
			{
				reason = canCast.Reason;
				return true;
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				AcceptanceReport report = lord.AbilityAllowed(this);
				if (!report)
				{
					reason = report.Reason;
					return true;
				}
			}
			if (!this.pawn.Drafted && this.def.disableGizmoWhileUndrafted && this.pawn.GetCaravan() == null && !DebugSettings.ShowDevGizmos)
			{
				reason = "AbilityDisabledUndrafted".Translate();
				return true;
			}
			if (this.pawn.DevelopmentalStage.Baby())
			{
				reason = "IsIncapped".Translate(this.pawn.LabelShort, this.pawn);
				return true;
			}
			if (this.pawn.Downed)
			{
				reason = "CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn);
				return true;
			}
			if (this.pawn.Deathresting)
			{
				reason = "CommandDisabledDeathresting".Translate(this.pawn);
				return true;
			}
			if (this.def.casterMustBeCapableOfViolence && this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				reason = "IsIncapableOfViolence".Translate(this.pawn.LabelShort, this.pawn);
				return true;
			}
			if (!this.CanQueueCast)
			{
				reason = "AbilityAlreadyQueued".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x0600BDA5 RID: 48549 RVA: 0x00370080 File Offset: 0x0036E280
		public virtual void AbilityTick()
		{
			this.VerbTracker.VerbsTick();
			if (this.def.emittedFleck != null && this.Casting && this.pawn.IsHashIntervalTick(this.def.emissionInterval))
			{
				FleckMaker.ThrowMetaIcon(this.verb.CurrentTarget.Cell, this.pawn.Map, this.def.emittedFleck, 0.75f);
			}
			this.MoteTick();
			this.EffectersTick();
			this.WarmupTick();
			this.CastingTick();
			this.SoundTick();
			this.CooldownTick();
			if (!this.comps.NullOrEmpty<AbilityComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompTick();
				}
			}
			if (this.wasCastingOnPrevTick && !this.Casting)
			{
				this.lastCastTick = Find.TickManager.TicksGame;
			}
			this.wasCastingOnPrevTick = this.Casting;
		}

		// Token: 0x0600BDA6 RID: 48550 RVA: 0x0037017C File Offset: 0x0036E37C
		private void MoteTick()
		{
			if ((this.def.warmupMote == null && this.def.WarmupMoteSocialSymbol == null) || !this.Casting)
			{
				return;
			}
			Vector3 vector = this.pawn.DrawPos + this.def.moteDrawOffset;
			vector += (this.verb.CurrentTarget.CenterVector3 - vector) * this.def.moteOffsetAmountTowardsTarget;
			if (this.warmupMote != null && !this.warmupMote.Destroyed)
			{
				if (!(this.warmupMote is MoteAttached))
				{
					this.warmupMote.exactPosition = vector;
				}
				this.warmupMote.Maintain();
				return;
			}
			if (!(this.def.WarmupMoteSocialSymbol == null))
			{
				this.warmupMote = MoteMaker.MakeInteractionBubble(this.pawn, this.verb.CurrentTarget.Pawn, ThingDefOf.Mote_Speech, this.def.WarmupMoteSocialSymbol, null);
				return;
			}
			if (this.def.warmupMote.thingClass != typeof(MoteAttached))
			{
				this.warmupMote = MoteMaker.MakeStaticMote(vector, this.pawn.Map, this.def.warmupMote, 1f, false, 0f);
				return;
			}
			this.warmupMote = MoteMaker.MakeAttachedOverlay((this.verb.CurrentTarget.Thing as Corpse) ?? this.verb.CurrentTarget.Thing, this.def.warmupMote, Vector3.zero, 1f, -1f);
		}

		// Token: 0x0600BDA7 RID: 48551 RVA: 0x00370334 File Offset: 0x0036E534
		private void EffectersTick()
		{
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				Effecter item = this.maintainedEffecters[i].Item1;
				if (item.ticksLeft > 0)
				{
					TargetInfo item2 = this.maintainedEffecters[i].Item2;
					TargetInfo item3 = this.maintainedEffecters[i].Item3;
					item.EffectTick(item2, item3);
					item.ticksLeft--;
				}
				else
				{
					item.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600BDA8 RID: 48552 RVA: 0x003703C4 File Offset: 0x0036E5C4
		private void CooldownTick()
		{
			if (!this.inCooldown || !this.CanCooldown || this.CooldownTicksRemaining > 0)
			{
				return;
			}
			this.inCooldown = false;
			if (this.UsesCharges && this.def.cooldownPerCharge)
			{
				int a2 = this.charges + 1;
				this.charges = a2;
				this.charges = Mathf.Min(a2, this.maxCharges);
				if (this.charges < this.maxCharges)
				{
					this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
				}
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				if (this.def.sendLetterOnCooldownComplete)
				{
					Find.LetterStack.ReceiveLetter("AbilityReadyLabel".Translate(this.def.LabelCap), "AbilityReadyText".Translate(this.pawn, this.def.label), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, null, null, 0, true);
				}
				if (this.def.sendMessageOnCooldownComplete && !this.pawn.IsPrisoner)
				{
					Messages.Message("AbilityReadyLabel".Translate(this.def.LabelCap, this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			if (this.def.groupDef != null && this.def.groupDef.sendMessageOnCooldownComplete && MessagesRepeatAvoider.MessageShowAllowed(this.def.groupDef.label + "_offcooldown", 1f))
			{
				string value = (from a in this.pawn.abilities.AllAbilitiesForReading
				where a.def.groupDef == this.def.groupDef
				select a.def.label).ToCommaList(false, false);
				Messages.Message("AbilitiesReadyLabel".Translate(value, this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600BDA9 RID: 48553 RVA: 0x00370600 File Offset: 0x0036E800
		private void CastingTick()
		{
			if (!this.Casting)
			{
				if (this.warmupEffecter != null)
				{
					this.warmupEffecter.Cleanup();
					this.warmupEffecter = null;
				}
				return;
			}
			this.SetupWarmupEffecter(this.verb.CurrentTarget);
			Effecter effecter = this.warmupEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.verbTargetInfoTmp, this.verbTargetInfoTmp);
			}
			if (this.needToRecacheWarmupMotes && !this.EffectComps.NullOrEmpty<CompAbilityEffect>() && this.verb.CurrentTarget.Thing != null)
			{
				this.customWarmupMotes.Clear();
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					foreach (Mote item in compAbilityEffect.CustomWarmupMotes(this.verb.CurrentTarget))
					{
						this.customWarmupMotes.Add(item);
					}
				}
				this.needToRecacheWarmupMotes = false;
			}
			foreach (Mote mote in this.customWarmupMotes)
			{
				mote.Maintain();
			}
		}

		// Token: 0x0600BDAA RID: 48554 RVA: 0x0037076C File Offset: 0x0036E96C
		private void WarmupTick()
		{
			if (!this.verb.WarmingUp)
			{
				return;
			}
			if (!(this.def.targetWorldCell ? this.CanApplyOn(this.pawn.CurJob.globalTarget) : this.CanApplyOn(this.verb.CurrentTarget)) && this.def.targetRequired)
			{
				if (this.def.targetWorldCell)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				Stance_Warmup warmupStance = this.verb.WarmupStance;
				if (warmupStance != null)
				{
					warmupStance.Interrupt();
				}
				this.verb.Reset();
				this.customWarmupMotes.Clear();
				this.preCastActions.Clear();
				return;
			}
			for (int i = this.preCastActions.Count - 1; i >= 0; i--)
			{
				if (this.preCastActions[i].ticksAwayFromCast >= this.verb.WarmupTicksLeft)
				{
					this.preCastActions[i].action(this.verb.CurrentTarget, this.verb.CurrentDestination);
					this.preCastActions.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600BDAB RID: 48555 RVA: 0x00370894 File Offset: 0x0036EA94
		private void SoundTick()
		{
			if (!this.pawn.Spawned || !this.Casting)
			{
				return;
			}
			if (this.def.warmupSound != null)
			{
				if (this.soundCast == null || this.soundCast.Ended)
				{
					this.soundCast = this.def.warmupSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
				}
				else
				{
					this.soundCast.Maintain();
				}
			}
			if (!this.wasCastingOnPrevTick && this.def.warmupStartSound != null)
			{
				this.def.warmupStartSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
			}
			if (this.def.warmupPreEndSound != null && this.verb.WarmupTicksLeft == this.def.warmupPreEndSoundSeconds.SecondsToTicks())
			{
				this.def.warmupPreEndSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
			}
		}

		// Token: 0x0600BDAC RID: 48556 RVA: 0x003709C0 File Offset: 0x0036EBC0
		private void SetupWarmupEffecter(LocalTargetInfo target)
		{
			if (this.warmupEffecter != null || this.def.warmupEffecter == null)
			{
				return;
			}
			if (!this.def.useAverageTargetPositionForWarmupEffecter || this.EffectComps.NullOrEmpty<CompAbilityEffect>())
			{
				if (target.Thing != null)
				{
					this.warmupEffecter = this.def.warmupEffecter.SpawnAttached(target.Thing, this.pawn.MapHeld, 1f);
					this.verbTargetInfoTmp = target.Thing;
				}
				else
				{
					this.warmupEffecter = this.def.warmupEffecter.Spawn(target.Cell, this.pawn.MapHeld, 1f);
					this.verbTargetInfoTmp = new TargetInfo(target.Cell, this.pawn.MapHeld, false);
				}
			}
			else
			{
				Vector3 vector = Vector3.zero;
				IEnumerable<LocalTargetInfo> affectedTargets = this.GetAffectedTargets(target);
				foreach (LocalTargetInfo localTargetInfo in affectedTargets)
				{
					vector += localTargetInfo.Cell.ToVector3Shifted();
				}
				vector /= (float)affectedTargets.Count<LocalTargetInfo>();
				this.warmupEffecter = this.def.warmupEffecter.Spawn(vector.ToIntVec3(), this.pawn.MapHeld, 1f);
				this.verbTargetInfoTmp = new TargetInfo(vector.ToIntVec3(), this.pawn.MapHeld, false);
			}
			this.warmupEffecter.Trigger(this.verbTargetInfoTmp, this.verbTargetInfoTmp, -1);
		}

		// Token: 0x0600BDAD RID: 48557 RVA: 0x00370B68 File Offset: 0x0036ED68
		public void Notify_StartedCasting()
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.preCastActions.AddRange(this.EffectComps[i].GetPreCastActions());
			}
		}

		// Token: 0x0600BDAE RID: 48558 RVA: 0x00370BA8 File Offset: 0x0036EDA8
		public void DrawEffectPreviews(LocalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.EffectComps[i].DrawEffectPreview(target);
			}
		}

		// Token: 0x0600BDAF RID: 48559 RVA: 0x00370BE0 File Offset: 0x0036EDE0
		public bool GizmosVisible()
		{
			if (!this.EffectComps.NullOrEmpty<CompAbilityEffect>())
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ShouldHideGizmo)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600BDB0 RID: 48560 RVA: 0x00370C48 File Offset: 0x0036EE48
		public virtual IEnumerable<Command> GetGizmos()
		{
			if (this.gizmo == null)
			{
				this.gizmo = (Command)Activator.CreateInstance(this.def.gizmoClass, new object[]
				{
					this,
					this.pawn
				});
				this.gizmo.Order = this.def.uiOrder;
			}
			if (!this.pawn.Drafted || this.def.showWhenDrafted)
			{
				yield return this.gizmo;
			}
			if (DebugSettings.ShowDevGizmos && this.inCooldown && this.CanCooldown)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reset cooldown",
					action = delegate()
					{
						this.inCooldown = false;
						this.charges = this.maxCharges;
					}
				};
			}
			yield break;
		}

		// Token: 0x0600BDB1 RID: 48561 RVA: 0x00370C58 File Offset: 0x0036EE58
		public virtual IEnumerable<Gizmo> GetGizmosExtra()
		{
			if (this.comps != null)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					foreach (Gizmo gizmo in abilityComp.CompGetGizmosExtra())
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator2 = null;
				}
				List<AbilityComp>.Enumerator enumerator = default(List<AbilityComp>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BDB2 RID: 48562 RVA: 0x00370C68 File Offset: 0x0036EE68
		public string GetInspectString()
		{
			if (this.comps.NullOrEmpty<AbilityComp>())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BDB3 RID: 48563 RVA: 0x00370CD8 File Offset: 0x0036EED8
		private void ApplyEffects(IEnumerable<CompAbilityEffect> effects, List<LocalTargetInfo> targets, LocalTargetInfo dest)
		{
			foreach (LocalTargetInfo target in targets)
			{
				this.ApplyEffects(effects, target, dest);
			}
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.PostApplied(targets, this.pawn.MapHeld);
			}
		}

		// Token: 0x0600BDB4 RID: 48564 RVA: 0x00370D68 File Offset: 0x0036EF68
		public void StartCooldown(int ticks)
		{
			this.inCooldown = true;
			this.cooldownEndTick = GenTicks.TicksGame + ticks;
			this.cooldownDuration = ticks;
			if (!this.def.cooldownPerCharge)
			{
				this.charges = this.maxCharges;
			}
		}

		// Token: 0x0600BDB5 RID: 48565 RVA: 0x00370D9E File Offset: 0x0036EF9E
		public void ResetCooldown()
		{
			this.inCooldown = false;
		}

		// Token: 0x0600BDB6 RID: 48566 RVA: 0x00370DA7 File Offset: 0x0036EFA7
		public void Notify_GroupStartedCooldown(AbilityGroupDef group, int ticks)
		{
			if (group == this.def.groupDef)
			{
				this.StartCooldown(ticks);
			}
		}

		// Token: 0x0600BDB7 RID: 48567 RVA: 0x00370DC0 File Offset: 0x0036EFC0
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target, dest);
			}
		}

		// Token: 0x0600BDB8 RID: 48568 RVA: 0x00370E08 File Offset: 0x0036F008
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, GlobalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target);
			}
		}

		// Token: 0x0600BDB9 RID: 48569 RVA: 0x00370E50 File Offset: 0x0036F050
		public virtual void OnGizmoUpdate()
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				compAbilityEffect.OnGizmoUpdate();
			}
		}

		// Token: 0x0600BDBA RID: 48570 RVA: 0x00370EA0 File Offset: 0x0036F0A0
		public IEnumerable<T> CompsOfType<T>() where T : AbilityComp
		{
			List<AbilityComp> list = this.comps;
			if (list == null)
			{
				return null;
			}
			return (from c in list
			where c is T
			select c).Cast<T>();
		}

		// Token: 0x0600BDBB RID: 48571 RVA: 0x00370ED7 File Offset: 0x0036F0D7
		public T CompOfType<T>() where T : AbilityComp
		{
			List<AbilityComp> list = this.comps;
			object obj;
			if (list == null)
			{
				obj = null;
			}
			else
			{
				obj = list.FirstOrDefault((AbilityComp c) => c is T);
			}
			return obj as T;
		}

		// Token: 0x0600BDBC RID: 48572 RVA: 0x00370F14 File Offset: 0x0036F114
		public void Initialize()
		{
			if (this.def.comps.Any<AbilityCompProperties>())
			{
				this.comps = new List<AbilityComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					AbilityComp abilityComp = null;
					try
					{
						abilityComp = (AbilityComp)Activator.CreateInstance(this.def.comps[i].compClass);
						abilityComp.parent = this;
						this.comps.Add(abilityComp);
						abilityComp.Initialize(this.def.comps[i]);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize an AbilityComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(abilityComp);
					}
				}
			}
			if (this.Id == -1)
			{
				this.Id = Find.UniqueIDsManager.GetNextAbilityID();
			}
			IAbilityVerb abilityVerb = this.VerbTracker.PrimaryVerb as IAbilityVerb;
			if (abilityVerb != null)
			{
				abilityVerb.Ability = this;
			}
			if (this.def.charges > 0)
			{
				this.maxCharges = this.def.charges;
				this.charges = this.maxCharges;
			}
		}

		// Token: 0x0600BDBD RID: 48573 RVA: 0x0037104C File Offset: 0x0036F24C
		public float FinalPsyfocusCost(LocalTargetInfo target)
		{
			if (this.def.AnyCompOverridesPsyfocusCost)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					if (abilityComp.props.OverridesPsyfocusCost)
					{
						return abilityComp.PsyfocusCostForTarget(target);
					}
				}
			}
			return this.def.PsyfocusCost;
		}

		// Token: 0x0600BDBE RID: 48574 RVA: 0x003710CC File Offset: 0x0036F2CC
		public float HemogenCost()
		{
			if (this.comps != null)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					CompAbilityEffect_HemogenCost compAbilityEffect_HemogenCost = abilityComp as CompAbilityEffect_HemogenCost;
					if (compAbilityEffect_HemogenCost != null)
					{
						return compAbilityEffect_HemogenCost.Props.hemogenCost;
					}
				}
			}
			return 0f;
		}

		// Token: 0x0600BDBF RID: 48575 RVA: 0x00371140 File Offset: 0x0036F340
		public TaggedString WorldMapExtraLabel(GlobalTargetInfo t)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				string text = compAbilityEffect.WorldMapExtraLabel(t);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x0600BDC0 RID: 48576 RVA: 0x003711A8 File Offset: 0x0036F3A8
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo targetInfo = new TargetInfo(pos, map ?? this.pawn.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, targetInfo, targetInfo));
		}

		// Token: 0x0600BDC1 RID: 48577 RVA: 0x003711EC File Offset: 0x0036F3EC
		public void AddEffecterToMaintain(Effecter eff, IntVec3 posA, IntVec3 posB, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo item = new TargetInfo(posA, map ?? this.pawn.Map, false);
			TargetInfo item2 = new TargetInfo(posB, map ?? this.pawn.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, item, item2));
		}

		// Token: 0x0600BDC2 RID: 48578 RVA: 0x00371248 File Offset: 0x0036F448
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
			if (this.def == null)
			{
				return;
			}
			Scribe_Values.Look<int>(ref this.Id, "Id", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.Id == -1)
				{
					this.Id = Find.UniqueIDsManager.GetNextAbilityID();
				}
				this.Initialize();
			}
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.inCooldown, "inCooldown", false, false);
			Scribe_Values.Look<int>(ref this.cooldownEndTick, "cooldownEndTick", 0, false);
			Scribe_Values.Look<int>(ref this.cooldownDuration, "cooldownDuration", 0, false);
			Scribe_Values.Look<int>(ref this.maxCharges, "maxCharges", this.def.charges, false);
			Scribe_Values.Look<int>(ref this.charges, "charges", this.def.charges, false);
			Scribe_Values.Look<int>(ref this.lastCastTick, "lastCastTick", -99999, false);
			this.ExposeBackCompatability();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x0600BDC3 RID: 48579 RVA: 0x0037138C File Offset: 0x0036F58C
		private void ExposeBackCompatability()
		{
			int num = 0;
			int num2 = 0;
			Scribe_Values.Look<int>(ref num, "cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref num2, "cooldownTicksDuration", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && num != 0 && num2 != 0)
			{
				this.inCooldown = true;
				this.cooldownDuration = num2;
				this.cooldownEndTick = GenTicks.TicksGame + num;
			}
		}

		// Token: 0x0600BDC4 RID: 48580 RVA: 0x003713E2 File Offset: 0x0036F5E2
		public string GetUniqueLoadID()
		{
			return "Ability_" + this.Id.ToString();
		}

		// Token: 0x0600BDC5 RID: 48581 RVA: 0x003713FC File Offset: 0x0036F5FC
		[CompilerGenerated]
		private bool <get_CanQueueCast>g__SameForQueueing|63_0(Job j)
		{
			return j.verbToUse == this.verb || (this.def.groupDef != null && j.ability != null && j.ability.def.groupDef == this.def.groupDef);
		}

		// Token: 0x04008267 RID: 33383
		public int Id = -1;

		// Token: 0x04008268 RID: 33384
		public Pawn pawn;

		// Token: 0x04008269 RID: 33385
		public AbilityDef def;

		// Token: 0x0400826A RID: 33386
		public List<AbilityComp> comps;

		// Token: 0x0400826B RID: 33387
		protected Command gizmo;

		// Token: 0x0400826C RID: 33388
		private VerbTracker verbTracker;

		// Token: 0x0400826D RID: 33389
		private bool inCooldown;

		// Token: 0x0400826E RID: 33390
		private int cooldownDuration;

		// Token: 0x0400826F RID: 33391
		private int cooldownEndTick;

		// Token: 0x04008270 RID: 33392
		private Mote warmupMote;

		// Token: 0x04008271 RID: 33393
		private Effecter warmupEffecter;

		// Token: 0x04008272 RID: 33394
		private Sustainer soundCast;

		// Token: 0x04008273 RID: 33395
		private bool wasCastingOnPrevTick;

		// Token: 0x04008274 RID: 33396
		private int charges;

		// Token: 0x04008275 RID: 33397
		public int maxCharges;

		// Token: 0x04008276 RID: 33398
		public int lastCastTick = -99999;

		// Token: 0x04008277 RID: 33399
		public Precept sourcePrecept;

		// Token: 0x04008278 RID: 33400
		private List<PreCastAction> preCastActions = new List<PreCastAction>();

		// Token: 0x04008279 RID: 33401
		private List<Tuple<Effecter, TargetInfo, TargetInfo>> maintainedEffecters = new List<Tuple<Effecter, TargetInfo, TargetInfo>>();

		// Token: 0x0400827A RID: 33402
		private List<Mote> customWarmupMotes = new List<Mote>();

		// Token: 0x0400827B RID: 33403
		private bool needToRecacheWarmupMotes = true;

		// Token: 0x0400827C RID: 33404
		private List<CompAbilityEffect> effectComps;

		// Token: 0x0400827E RID: 33406
		private readonly List<LocalTargetInfo> affectedTargetsCached = new List<LocalTargetInfo>();

		// Token: 0x0400827F RID: 33407
		private TargetInfo verbTargetInfoTmp = null;
	}
}
