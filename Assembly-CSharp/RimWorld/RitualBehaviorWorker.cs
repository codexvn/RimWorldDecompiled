using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020026C5 RID: 9925
	public class RitualBehaviorWorker : IExposable
	{
		// Token: 0x0600D995 RID: 55701 RVA: 0x000024DF File Offset: 0x000006DF
		public RitualBehaviorWorker()
		{
		}

		// Token: 0x0600D996 RID: 55702 RVA: 0x0040655F File Offset: 0x0040475F
		public RitualBehaviorWorker(RitualBehaviorDef def)
		{
			this.def = def;
		}

		// Token: 0x17002154 RID: 8532
		// (get) Token: 0x0600D997 RID: 55703 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string descriptionOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002155 RID: 8533
		// (get) Token: 0x0600D998 RID: 55704 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Sustainer SoundPlaying
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002156 RID: 8534
		// (get) Token: 0x0600D999 RID: 55705 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ChecksReservations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D99A RID: 55706 RVA: 0x00406570 File Offset: 0x00404770
		public virtual string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			if (target.IsValid && target.Map.Tile.Valid)
			{
				PlanetLayerDef layerDef = target.Map.Tile.LayerDef;
				if ((!ritual.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !ritual.layerWhitelist.Contains(layerDef)) || (!ritual.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && ritual.layerBlacklist.Contains(layerDef)))
				{
					return "CantStartRitualLayer".Translate(ritual.Label.Named("RITUAL"), layerDef.gerundLabel.Named("GERUND"), layerDef.LabelCap.Named("LAYER")).CapitalizeFirst();
				}
			}
			if (!ritual.allowOtherInstances)
			{
				using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(target.Map).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Ritual == ritual)
						{
							return "CantStartRitualAlreadyInProgress".Translate(ritual.Label).CapitalizeFirst();
						}
					}
				}
			}
			if (selectedPawn != null)
			{
				RitualBehaviorWorker behavior = ritual.behavior;
				if (((behavior != null) ? behavior.def.roles : null) != null)
				{
					foreach (RitualRole ritualRole in ritual.behavior.def.roles)
					{
						string text;
						if (ritualRole.defaultForSelectedColonist && !ritualRole.AppliesToPawn(selectedPawn, out text, target, null, null, ritual, false))
						{
							if (text.NullOrEmpty())
							{
								return "CantStartRitualSelectedPawnCannotBeRole".Translate(selectedPawn.Named("PAWN"), ritualRole.Label.Named("ROLE")).CapitalizeFirst();
							}
							return text;
						}
					}
				}
			}
			if (target.IsValid)
			{
				List<Pawn> list = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.ToList<Pawn>();
				list.AddRange(target.Map.mapPawns.SpawnedColonyAnimals);
				if (!ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
				{
					using (List<RitualRole>.Enumerator enumerator2 = ritual.behavior.def.roles.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							RitualRole role = enumerator2.Current;
							if (role.required && !role.substitutable)
							{
								IEnumerable<RitualRole> source = (role.mergeId == null) ? Gen.YieldSingle<RitualRole>(role) : (from r in ritual.behavior.def.roles
								where r.mergeId == role.mergeId
								select r);
								if (list.Count(delegate(Pawn p)
								{
									string text2;
									return role.AppliesToPawn(p, out text2, target, null, null, null, true);
								}) < source.Count<RitualRole>() && (forcedForRole == null || !forcedForRole.ContainsKey(role.id)))
								{
									Precept precept = ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
									if (precept != null)
									{
										return "MessageNeedAssignedRoleToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(precept.LabelCap, false, false), ritual.Label);
									}
									if (!role.noCandidatesGizmoDesc.NullOrEmpty())
									{
										return role.noCandidatesGizmoDesc;
									}
									if (source.Count<RitualRole>() == 1)
									{
										return "MessageNoRequiredRolePawnToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(role.Label, false, false), ritual.Label);
									}
									return "MessageNoRequiredRolePawnToBeginRitual".Translate(source.Count<RitualRole>().ToString() + " " + (role.missingDesc ?? Find.ActiveLanguageWorker.Pluralize(role.Label, -1)), ritual.Label);
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600D99B RID: 55707 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return null;
		}

		// Token: 0x0600D99C RID: 55708 RVA: 0x00406A98 File Offset: 0x00404C98
		public virtual bool CanExecuteOn(TargetInfo target, RitualObligation obligation)
		{
			return obligation == null || obligation.precept.obligationTargetFilter.CanUseTarget(target, obligation).canUse;
		}

		// Token: 0x0600D99D RID: 55709 RVA: 0x00406AB8 File Offset: 0x00404CB8
		public virtual void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
		{
			if (this.CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
			{
				return;
			}
			if (!this.CanExecuteOn(target, obligation))
			{
				return;
			}
			if (playerForced)
			{
				foreach (Pawn pawn in assignments.Participants)
				{
					RitualRole ritualRole = assignments.RoleForPawn(pawn, true);
					if (ritualRole == null || !ritualRole.allowKeepLayingDown || pawn.GetPosture() != PawnPosture.LayingInBed)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
					}
				}
			}
			foreach (Pawn pawn2 in assignments.Participants)
			{
				Lord lord = pawn2.GetLord();
				if (((lord != null) ? lord.LordJob : null) is LordJob_VoluntarilyJoinable)
				{
					pawn2.GetLord().Notify_PawnLost(pawn2, PawnLostCondition.LeftVoluntarily, null);
				}
			}
			LordJob_Ritual lordJob = (LordJob_Ritual)this.CreateLordJob(target, organizer, ritual, obligation, assignments);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate(Pawn p)
			{
				RitualRole ritualRole2 = lordJob.RoleFor(p, false);
				return ritualRole2 == null || ritualRole2.addToLord;
			}));
			RitualOutcomeEffectWorker outcomeEffect = ritual.outcomeEffect;
			if (outcomeEffect != null)
			{
				outcomeEffect.ResetCompDatas();
			}
			lordJob.PreparePawns();
			AbilityGroupDef useCooldownFromAbilityGroupDef = ritual.def.useCooldownFromAbilityGroupDef;
			if (useCooldownFromAbilityGroupDef != null && useCooldownFromAbilityGroupDef.cooldownTicks > 0 && !useCooldownFromAbilityGroupDef.ritualRoleIds.NullOrEmpty<string>())
			{
				foreach (string roleId in useCooldownFromAbilityGroupDef.ritualRoleIds)
				{
					if (assignments.AnyPawnAssigned(roleId))
					{
						foreach (Pawn pawn3 in assignments.AssignedPawns(roleId))
						{
							foreach (Ability ability in pawn3.abilities.AllAbilitiesForReading)
							{
								ability.Notify_GroupStartedCooldown(useCooldownFromAbilityGroupDef, useCooldownFromAbilityGroupDef.cooldownTicks);
							}
						}
					}
				}
				ritual.Notify_CooldownFromAbilityStarted(useCooldownFromAbilityGroupDef.cooldownTicks);
			}
			this.PostExecute(target, organizer, ritual, obligation, assignments);
		}

		// Token: 0x0600D99E RID: 55710 RVA: 0x00406D50 File Offset: 0x00404F50
		protected virtual void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Messages.Message("RitualBegun".Translate(ritual.Label).CapitalizeFirst(), target, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600D99F RID: 55711 RVA: 0x00406D90 File Offset: 0x00404F90
		protected virtual LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual(target, ritual, obligation, this.def.stages, assignments, null, null);
		}

		// Token: 0x0600D9A0 RID: 55712 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600D9A1 RID: 55713 RVA: 0x00406DC0 File Offset: 0x00404FC0
		public bool SpectatorsRequired()
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return true;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].spectatorsRequired)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D9A2 RID: 55714 RVA: 0x00406E18 File Offset: 0x00405018
		public bool HasAnyRole(Pawn p)
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return false;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].HasRole(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D9A3 RID: 55715 RVA: 0x00406E70 File Offset: 0x00405070
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.def, "def");
		}

		// Token: 0x0600D9A4 RID: 55716 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Cleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600D9A5 RID: 55717 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostCleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600D9A6 RID: 55718 RVA: 0x00406E82 File Offset: 0x00405082
		public virtual string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return this.def.durationTicks.max.ToStringTicksToPeriod(false, false, true, true, false);
		}

		// Token: 0x0600D9A7 RID: 55719 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShouldInitAsSpectator(Pawn pawn, RitualRoleAssignments assignments)
		{
			return true;
		}

		// Token: 0x0600D9A8 RID: 55720 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawPreviewOnTarget(TargetInfo targetInfo)
		{
		}

		// Token: 0x0600D9A9 RID: 55721 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600D9AA RID: 55722 RVA: 0x00406E9E File Offset: 0x0040509E
		public virtual bool PawnCanFillRole(Pawn pawn, RitualRole role, out string s, TargetInfo ritualTarget)
		{
			s = null;
			return true;
		}

		// Token: 0x0400937C RID: 37756
		public RitualBehaviorDef def;
	}
}
