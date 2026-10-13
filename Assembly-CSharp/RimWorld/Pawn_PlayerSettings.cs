using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020024FB RID: 9467
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x17001FB2 RID: 8114
		// (get) Token: 0x0600CEC6 RID: 52934 RVA: 0x003CB4EB File Offset: 0x003C96EB
		// (set) Token: 0x0600CEC7 RID: 52935 RVA: 0x003CB4F4 File Offset: 0x003C96F4
		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master == value)
				{
					return;
				}
				if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573);
					return;
				}
				bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
				this.master = value;
				if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x17001FB3 RID: 8115
		// (get) Token: 0x0600CEC8 RID: 52936 RVA: 0x003CB57C File Offset: 0x003C977C
		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				if (!this.RespectsAllowedArea)
				{
					return null;
				}
				Area result;
				if (!this.allowedAreas.TryGetValue(this.pawn.MapHeld, out result))
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x17001FB4 RID: 8116
		// (get) Token: 0x0600CEC9 RID: 52937 RVA: 0x003CB5B0 File Offset: 0x003C97B0
		// (set) Token: 0x0600CECA RID: 52938 RVA: 0x003CB5E8 File Offset: 0x003C97E8
		public Area AreaRestrictionInPawnCurrentMap
		{
			get
			{
				Area result;
				if (this.pawn.MapHeld == null || !this.allowedAreas.TryGetValue(this.pawn.MapHeld, out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				Map map = this.pawn.MapHeld;
				if (map == null && this.pawn.DevelopmentalStage == DevelopmentalStage.Baby)
				{
					Pawn firstDirectRelationPawn = this.pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent, null);
					map = ((firstDirectRelationPawn != null) ? firstDirectRelationPawn.MapHeld : null);
				}
				if (map == null)
				{
					return;
				}
				this.allowedAreas.SetOrAdd(map, value);
				if (this.pawn.Spawned && !this.pawn.Drafted && value != null && value == this.EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					if (((jobs != null) ? jobs.curJob : null) != null && this.pawn.jobs.curJob.AnyTargetOutsideArea(value))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x17001FB5 RID: 8117
		// (get) Token: 0x0600CECB RID: 52939 RVA: 0x003CB6BA File Offset: 0x003C98BA
		public bool RespectsAllowedArea
		{
			get
			{
				return this.SupportsAllowedAreas && this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x17001FB6 RID: 8118
		// (get) Token: 0x0600CECC RID: 52940 RVA: 0x003CB6F7 File Offset: 0x003C98F7
		public bool SupportsAllowedAreas
		{
			get
			{
				return !this.pawn.RaceProps.Roamer && !this.pawn.RaceProps.disableAreaControl;
			}
		}

		// Token: 0x17001FB7 RID: 8119
		// (get) Token: 0x0600CECD RID: 52941 RVA: 0x003CB720 File Offset: 0x003C9920
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x17001FB8 RID: 8120
		// (get) Token: 0x0600CECE RID: 52942 RVA: 0x003CB758 File Offset: 0x003C9958
		public Pawn RespectedMaster
		{
			get
			{
				if (!this.RespectsMaster)
				{
					return null;
				}
				return this.Master;
			}
		}

		// Token: 0x17001FB9 RID: 8121
		// (get) Token: 0x0600CECF RID: 52943 RVA: 0x003CB76C File Offset: 0x003C996C
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return (this.pawn.IsColonist || (this.pawn.IsColonySubhuman && !this.pawn.mutant.Def.disableHostilityResponse)) && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x0600CED0 RID: 52944 RVA: 0x003CB7BC File Offset: 0x003C99BC
		public Pawn_PlayerSettings(Pawn pawn)
		{
			this.pawn = pawn;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.joinTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.joinTick = 0;
			}
			this.Notify_FactionChanged();
		}

		// Token: 0x0600CED1 RID: 52945 RVA: 0x003CB830 File Offset: 0x003C9A30
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Values.Look<bool>(ref this.animalsReleased, "animalsReleased", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.medCare, "medCare", MedicalCareCategory.NoCare, false);
			Scribe_Collections.Look<Map, Area>(ref this.allowedAreas, "allowedAreas", LookMode.Reference, LookMode.Reference, ref this.allowedAreasKeys, ref this.allowedAreasValues, true, false, false);
			Scribe_References.Look<Pawn>(ref this.master, "master", false);
			Scribe_Values.Look<bool>(ref this.followDrafted, "followDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.followFieldwork, "followFieldwork", false, false);
			Scribe_Values.Look<HostilityResponseMode>(ref this.hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee, false);
			Scribe_Values.Look<bool>(ref this.selfTend, "selfTend", false, false);
			Scribe_Values.Look<int>(ref this.displayOrder, "displayOrder", 0, false);
			Scribe_Values.Look<bool>(ref this.animalForage, "animalForage", true, false);
			Scribe_Values.Look<bool>(ref this.animalDig, "animalDig", true, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.allowedAreas != null)
			{
				this.allowedAreas.RemoveAll((KeyValuePair<Map, Area> kvp) => kvp.Key == null || kvp.Value == null);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pawn.def.race != null && this.pawn.def.race.Roamer)
			{
				this.allowedAreas.Clear();
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.allowedAreas == null)
			{
				this.allowedAreas = new Dictionary<Map, Area>();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Area area = null;
				Scribe_References.Look<Area>(ref area, "areaAllowed", false);
				if (area != null && Find.AnyPlayerHomeMap != null)
				{
					this.allowedAreas.Add(Find.AnyPlayerHomeMap, area);
				}
			}
		}

		// Token: 0x0600CED2 RID: 52946 RVA: 0x003CB9F7 File Offset: 0x003C9BF7
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				int num = 0;
				bool flag = false;
				int canAttackTargetCount = 0;
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					if (pawn.training.HasLearned(TrainableDefOf.Release))
					{
						flag = true;
						if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
						{
							num++;
						}
					}
					if (ModsConfig.OdysseyActive && pawn.training.HasLearned(TrainableDefOf.AttackTarget))
					{
						int num2 = canAttackTargetCount;
						canAttackTargetCount = num2 + 1;
					}
				}
				if (flag)
				{
					Command_Toggle command_Toggle = new Command_Toggle();
					command_Toggle.defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((num != 0) ? (" (" + num.ToString() + ")") : "");
					command_Toggle.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
					command_Toggle.icon = TexCommand.ReleaseAnimals;
					command_Toggle.hotKey = KeyBindingDefOf.Misc7;
					command_Toggle.isActive = (() => this.animalsReleased);
					command_Toggle.toggleAction = delegate()
					{
						this.animalsReleased = !this.animalsReleased;
						if (this.animalsReleased)
						{
							foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
							{
								if (pawn2.caller != null)
								{
									pawn2.caller.Notify_Released();
								}
								pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
						}
					};
					if (num == 0)
					{
						command_Toggle.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
					}
					yield return command_Toggle;
				}
				if (canAttackTargetCount > 0)
				{
					yield return new Command_Target
					{
						defaultLabel = "AnimalsAttackTarget".Translate() + string.Format(" ({0})", canAttackTargetCount),
						defaultDesc = "AnimalsAttackTargetDesc".Translate(),
						targetingParams = TargetingParameters.ForAttackAny(),
						hotKey = KeyBindingDefOf.Misc8,
						icon = Pawn_TrainingTracker.AttackTargetTexture,
						action = delegate(LocalTargetInfo target)
						{
							if (!target.Cell.InHorDistOf(this.pawn.Position, 25.9f))
							{
								Messages.Message("MessageNotInRangeOfMaster".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							string text = null;
							bool flag2 = false;
							foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
							{
								if (pawn2.training.HasLearned(TrainableDefOf.AttackTarget))
								{
									Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn2, target, out text, true);
									if (meleeAttackAction != null)
									{
										meleeAttackAction();
									}
									if (text.NullOrEmpty())
									{
										flag2 = true;
										pawn2.training.attackTarget = target.Thing;
										pawn2.mindState.enemyTarget = target.Thing;
									}
								}
							}
							if (!flag2)
							{
								Messages.Message(text, MessageTypeDefOf.RejectInput, false);
							}
						},
						onUpdate = delegate(LocalTargetInfo _)
						{
							GenDraw.DrawRadiusRing(this.pawn.Position, 25.9f);
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0600CED3 RID: 52947 RVA: 0x003CBA07 File Offset: 0x003C9C07
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.allowedAreas.Clear();
		}

		// Token: 0x0600CED4 RID: 52948 RVA: 0x003CBA1C File Offset: 0x003C9C1C
		public void ResetMedicalCare()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				return;
			}
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				if (ModsConfig.AnomalyActive && this.pawn.IsGhoul)
				{
					this.medCare = Find.PlaySettings.defaultCareForGhouls;
					return;
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsEntity)
				{
					this.medCare = Find.PlaySettings.defaultCareForEntities;
					return;
				}
				if (!this.pawn.RaceProps.Animal)
				{
					this.medCare = (this.pawn.IsSlave ? Find.PlaySettings.defaultCareForSlave : Find.PlaySettings.defaultCareForColonist);
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForTamedAnimal;
				return;
			}
			else
			{
				if (ModsConfig.AnomalyActive && this.pawn.IsEntity)
				{
					this.medCare = Find.PlaySettings.defaultCareForEntities;
					return;
				}
				if (this.pawn.IsPrisoner)
				{
					this.medCare = Find.PlaySettings.defaultCareForPrisoner;
					return;
				}
				if (this.pawn.Faction == null && this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForWildlife;
					return;
				}
				if (this.pawn.Faction != null)
				{
					FactionRelationKind kind = this.pawn.Faction.RelationWith(Faction.OfPlayer, false).kind;
					if (kind == FactionRelationKind.Ally)
					{
						this.medCare = Find.PlaySettings.defaultCareForFriendlyFaction;
						return;
					}
					if (kind == FactionRelationKind.Neutral)
					{
						this.medCare = Find.PlaySettings.defaultCareForNeutralFaction;
						return;
					}
					if (kind == FactionRelationKind.Hostile)
					{
						this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
						return;
					}
				}
				else
				{
					if (!this.pawn.Faction.HostileTo(Faction.OfPlayer))
					{
						this.medCare = Find.PlaySettings.defaultCareForNoFaction;
						return;
					}
					this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
				}
				return;
			}
		}

		// Token: 0x0600CED5 RID: 52949 RVA: 0x003CBBF0 File Offset: 0x003C9DF0
		public void Notify_AreaRemoved(Area area)
		{
			foreach (Map key in this.allowedAreas.Keys.ToList<Map>())
			{
				if (this.allowedAreas[key] == area)
				{
					this.allowedAreas.Remove(key);
				}
			}
		}

		// Token: 0x0600CED6 RID: 52950 RVA: 0x003CBC64 File Offset: 0x003C9E64
		public void Notify_MapRemoved(Map map)
		{
			if (this.allowedAreas.ContainsKey(map))
			{
				this.allowedAreas.Remove(map);
			}
		}

		// Token: 0x04008CB9 RID: 36025
		public const int UnsetDisplayOrder = -9999999;

		// Token: 0x04008CBA RID: 36026
		private Pawn pawn;

		// Token: 0x04008CBB RID: 36027
		private Dictionary<Map, Area> allowedAreas = new Dictionary<Map, Area>();

		// Token: 0x04008CBC RID: 36028
		public int joinTick;

		// Token: 0x04008CBD RID: 36029
		private Pawn master;

		// Token: 0x04008CBE RID: 36030
		public bool followDrafted;

		// Token: 0x04008CBF RID: 36031
		public bool followFieldwork;

		// Token: 0x04008CC0 RID: 36032
		public bool animalsReleased;

		// Token: 0x04008CC1 RID: 36033
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x04008CC2 RID: 36034
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x04008CC3 RID: 36035
		public bool selfTend;

		// Token: 0x04008CC4 RID: 36036
		public int displayOrder = -9999999;

		// Token: 0x04008CC5 RID: 36037
		public bool animalForage = true;

		// Token: 0x04008CC6 RID: 36038
		public bool animalDig = true;

		// Token: 0x04008CC7 RID: 36039
		private List<Map> allowedAreasKeys;

		// Token: 0x04008CC8 RID: 36040
		private List<Area> allowedAreasValues;
	}
}
