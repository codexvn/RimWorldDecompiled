using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016AF RID: 5807
	public class PsychicRitualRoleDef : Def, ILordJobRole
	{
		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x060088E4 RID: 35044 RVA: 0x0028CFC3 File Offset: 0x0028B1C3
		public virtual int MaxCount
		{
			get
			{
				return this.allowedCount.max;
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x060088E5 RID: 35045 RVA: 0x0028CFD0 File Offset: 0x0028B1D0
		public virtual int MinCount
		{
			get
			{
				return this.allowedCount.min;
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x060088E6 RID: 35046 RVA: 0x0028CFDD File Offset: 0x0028B1DD
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x060088E7 RID: 35047 RVA: 0x0028CFEA File Offset: 0x0028B1EA
		public TaggedString CategoryLabel
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x060088E8 RID: 35048 RVA: 0x0028CFF2 File Offset: 0x0028B1F2
		public TaggedString CategoryLabelCap
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x060088E9 RID: 35049 RVA: 0x0028CFFA File Offset: 0x0028B1FA
		public virtual PsychicRitualRoleDef.Condition AllowedConditions
		{
			get
			{
				return this.allowedConditions;
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x060088EA RID: 35050 RVA: 0x0028D002 File Offset: 0x0028B202
		public virtual bool CanHandleOfferings
		{
			get
			{
				return this.canHandleOfferings;
			}
		}

		// Token: 0x060088EB RID: 35051 RVA: 0x0028D00C File Offset: 0x0028B20C
		public virtual bool ConditionAllowed(PawnLostCondition condition)
		{
			if (condition == PawnLostCondition.LordRejected)
			{
				return false;
			}
			PsychicRitualRoleDef.Condition condition2;
			if (!PsychicRitualRoleDef.PawnLostConditionToCondition.TryGetValue(condition, out condition2))
			{
				throw new InvalidOperationException(string.Format("Unknown PawnLostCondition {0}", condition));
			}
			return condition2 == PsychicRitualRoleDef.Condition.None || this.ConditionAllowed(condition2);
		}

		// Token: 0x060088EC RID: 35052 RVA: 0x0028D051 File Offset: 0x0028B251
		public virtual bool ConditionAllowed(PsychicRitualRoleDef.Condition condition)
		{
			return (this.AllowedConditions & condition) > PsychicRitualRoleDef.Condition.None;
		}

		// Token: 0x060088ED RID: 35053 RVA: 0x0028D060 File Offset: 0x0028B260
		public static TaggedString ConditionToReason(Pawn pawn, PsychicRitualRoleDef.Condition disqualifier)
		{
			if (disqualifier > PsychicRitualRoleDef.Condition.NonPlayerFaction)
			{
				if (disqualifier <= PsychicRitualRoleDef.Condition.Child)
				{
					if (disqualifier <= PsychicRitualRoleDef.Condition.LeftVoluntarily)
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.NotOnMap)
						{
							return "PsychicRitualLeaveReason_ExitedMap".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier == PsychicRitualRoleDef.Condition.HasAnotherLord)
						{
							return "PsychicRitualLeaveReason_PawnBusyWithLord".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.LeftVoluntarily)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_LeftVoluntarily".Translate(pawn.Named("PAWN"));
					}
					else
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.Vanished)
						{
							return "PsychicRitualLeaveReason_Vanished".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.Baby)
						{
							if (disqualifier != PsychicRitualRoleDef.Condition.Child)
							{
								goto IL_41D;
							}
							goto IL_35F;
							for (;;)
							{
								IL_35F:
								goto IL_35F;
							}
						}
					}
				}
				else if (disqualifier <= PsychicRitualRoleDef.Condition.SubHuman)
				{
					if (disqualifier != PsychicRitualRoleDef.Condition.Adult)
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.NoPath)
						{
							return "PsychicRitualLeaveReason_CannotReachTarget".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.SubHuman)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_Mutant".Translate(pawn.Named("PAWN"));
					}
				}
				else if (disqualifier <= PsychicRitualRoleDef.Condition.NoPsychicSensitivity)
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.IdeoUnwilling)
					{
						return "PsychicRitualLeaveReason_IdeoUnwilling".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"));
					}
					if (disqualifier != PsychicRitualRoleDef.Condition.NoPsychicSensitivity)
					{
						goto IL_41D;
					}
					return "PsychicRitualLeaveReason_NoPsychicSensitivity".Translate(pawn.Named("PAWN"));
				}
				else
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.TemporaryFactionMember)
					{
						return "PsychicRitualLeaveReason_TemporaryFactionMember".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier == PsychicRitualRoleDef.Condition.Error)
					{
						goto IL_191;
					}
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_DevelopmentalStage".Translate(pawn.Named("PAWN"), pawn.DevelopmentalStage.ToString().Translate().Named("STAGE"));
			}
			if (disqualifier <= PsychicRitualRoleDef.Condition.Humanlike)
			{
				if (disqualifier <= PsychicRitualRoleDef.Condition.Burning)
				{
					switch (disqualifier)
					{
					case PsychicRitualRoleDef.Condition.None:
						break;
					case PsychicRitualRoleDef.Condition.Dead:
						return "PsychicRitualLeaveReason_Killed".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Downed:
						return "PsychicRitualLeaveReason_Incapped".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Downed:
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Drafted:
					case PsychicRitualRoleDef.Condition.Downed | PsychicRitualRoleDef.Condition.Drafted:
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Downed | PsychicRitualRoleDef.Condition.Drafted:
						goto IL_41D;
					case PsychicRitualRoleDef.Condition.Drafted:
						return "PsychicRitualLeaveReason_Drafted".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Sleeping:
						return "PsychicRitualLeaveReason_Sleeping".Translate(pawn.Named("PAWN"));
					default:
						if (disqualifier == PsychicRitualRoleDef.Condition.Bleeding)
						{
							return "PsychicRitualLeaveReason_Bleeding".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.Burning)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_Burning".Translate(pawn.Named("PAWN"));
					}
				}
				else
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.MentalState)
					{
						return "PsychicRitualLeaveReason_InMentalState".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier == PsychicRitualRoleDef.Condition.ExtremeTemperature)
					{
						return "PsychicRitualLeaveReason_ExtremeTemperature".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier != PsychicRitualRoleDef.Condition.Humanlike)
					{
						goto IL_41D;
					}
					return "PsychicRitualLeaveReason_Humanlike".Translate(pawn.Named("PAWN"));
				}
			}
			else if (disqualifier <= PsychicRitualRoleDef.Condition.Slave)
			{
				if (disqualifier == PsychicRitualRoleDef.Condition.NonHumanlike)
				{
					return "PsychicRitualLeaveReason_NonHumanlike".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier == PsychicRitualRoleDef.Condition.Prisoner)
				{
					return "PsychicRitualLeaveReason_Prisoner".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier != PsychicRitualRoleDef.Condition.Slave)
				{
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_Slave".Translate(pawn.Named("PAWN"));
			}
			else
			{
				if (disqualifier == PsychicRitualRoleDef.Condition.Freeman)
				{
					return "PsychicRitualLeaveReason_Freeman".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier == PsychicRitualRoleDef.Condition.InsideBuilding)
				{
					Building arg = (Building)pawn.SpawnedParentOrMe;
					return "PsychicRitualLeaveReason_InsideBuilding".Translate(pawn.Named("PAWN"), arg.Named("BUILDING"));
				}
				if (disqualifier != PsychicRitualRoleDef.Condition.NonPlayerFaction)
				{
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_WrongFaction".Translate(pawn.Named("PAWN"), Faction.OfPlayer.Named("FACTION"));
			}
			IL_191:
			return TaggedString.Empty;
			IL_41D:
			throw new InvalidOperationException(string.Format("Unknown RoleDisqualifiersEnum {0}", disqualifier));
		}

		// Token: 0x060088EE RID: 35054 RVA: 0x0028D4A0 File Offset: 0x0028B6A0
		public static TaggedString PawnLostConditionToPsychicRitualReason(PsychicRitual psychicRitual, Pawn pawn, PawnLostCondition condition)
		{
			if (condition == PawnLostCondition.LordRejected)
			{
				throw new InvalidOperationException("Already informed player of pawn lost reason.");
			}
			PsychicRitualRoleDef.Condition disqualifier;
			if (!PsychicRitualRoleDef.PawnLostConditionToCondition.TryGetValue(condition, out disqualifier))
			{
				throw new InvalidOperationException(string.Format("Unknown PawnLostCondition {0}", condition));
			}
			return PsychicRitualRoleDef.ConditionToReason(pawn, disqualifier);
		}

		// Token: 0x060088EF RID: 35055 RVA: 0x0028D4EC File Offset: 0x0028B6EC
		public bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out PsychicRitualRoleDef.Reason reason)
		{
			AnyEnum reasonCode;
			if (!this.PawnCanDo(context, pawn, target, out reasonCode))
			{
				reason = new PsychicRitualRoleDef.Reason
				{
					context = context,
					def = this,
					pawn = pawn,
					reasonCode = reasonCode,
					target = target
				};
				return false;
			}
			reason = PsychicRitualRoleDef.Reason.None;
			return true;
		}

		// Token: 0x060088F0 RID: 35056 RVA: 0x0028D550 File Offset: 0x0028B750
		protected virtual bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
		{
			if (pawn == null)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Error);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Dead) && pawn.Dead)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Dead);
				return false;
			}
			if (ModsConfig.AnomalyActive && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.SubHuman) && pawn.IsSubhuman)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.SubHuman);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Humanlike) && pawn.RaceProps.Humanlike)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Humanlike);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NonHumanlike) && !pawn.RaceProps.Humanlike)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NonHumanlike);
				return false;
			}
			PsychicRitualRoleDef.Condition condition = PsychicRitualRoleDef.DevelopmentalStageToCondition[pawn.DevelopmentalStage];
			if (!this.ConditionAllowed(condition))
			{
				if (context == PsychicRitualRoleDef.Context.InvokePsychicRitualGizmo)
				{
					reason = AnyEnum.None;
					return false;
				}
				if (context != PsychicRitualRoleDef.Context.Runtime && context == PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
				{
					reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(condition);
					return false;
				}
			}
			if (context != PsychicRitualRoleDef.Context.NonPlayerFaction && context != PsychicRitualRoleDef.Context.Runtime && !pawn.IsPrisoner && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.NonPlayerFaction) && !pawn.Faction.IsPlayerSafe())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NonPlayerFaction);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.HasAnotherLord) && pawn.GetLord() != null && context != PsychicRitualRoleDef.Context.Runtime)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.HasAnotherLord);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Slave) && pawn.IsSlave)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Slave);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Prisoner) && pawn.IsPrisoner)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Prisoner);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Freeman) && pawn.IsFreeman)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Freeman);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Downed) && pawn.Downed && (!pawn.health.capacities.CanBeAwake || pawn.Awake()))
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Downed);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.MentalState) && pawn.InMentalState)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.MentalState);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Bleeding) && pawn.health.hediffSet.BleedRateTotal > 0f)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Bleeding);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Burning) && pawn.IsBurning())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Burning);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.ExtremeTemperature) && target.IsValid)
			{
				Map map = target.Map;
				if (map != null && pawn.Faction == Faction.OfPlayer && !pawn.SafeTemperatureAtCell(target.Cell, map))
				{
					reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.ExtremeTemperature);
					return false;
				}
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.InsideBuilding) && pawn.SpawnedParentOrMe is Building)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.InsideBuilding);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping) && !pawn.Awake() && context != PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Sleeping);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted) && pawn.Drafted && context != PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Drafted);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath) && target.IsValid && context != PsychicRitualRoleDef.Context.Runtime && !this.CanReach(pawn, target) && !pawn.IsPrisoner)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NoPath);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.IdeoUnwilling) && context == PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual && pawn.Ideo != null && !pawn.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.InvolvedInPsychicRitual, pawn.Named(HistoryEventArgsNames.Doer))))
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.IdeoUnwilling);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPsychicSensitivity) && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NoPsychicSensitivity);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.TemporaryFactionMember) && pawn.IsQuestLodger())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.TemporaryFactionMember);
				return false;
			}
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x060088F1 RID: 35057 RVA: 0x0028D9AC File Offset: 0x0028BBAC
		public virtual TaggedString PawnCannotDoReason(AnyEnum reason, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target)
		{
			if (reason == AnyEnum.None)
			{
				return TaggedString.Empty;
			}
			PsychicRitualRoleDef.Condition? condition = reason.As<PsychicRitualRoleDef.Condition>();
			if (condition != null)
			{
				PsychicRitualRoleDef.Condition valueOrDefault = condition.GetValueOrDefault();
				return PsychicRitualRoleDef.ConditionToReason(pawn, valueOrDefault);
			}
			throw new ArgumentException("Unknown status reason type " + reason.enumType.ToStringSafe<Type>() + ".  Did you forget to override PawnCannotDoReason in a child class?");
		}

		// Token: 0x060088F2 RID: 35058 RVA: 0x0028DA0E File Offset: 0x0028BC0E
		public IEnumerable<string> BlockingIssues(Pawn pawn, TargetInfo target)
		{
			PsychicRitualRoleDef.Reason reason;
			if (!this.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, target, out reason))
			{
				yield return reason.ToPlayerReadable();
			}
			yield break;
		}

		// Token: 0x060088F3 RID: 35059 RVA: 0x0028DA2C File Offset: 0x0028BC2C
		public virtual IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, Pawn pawn)
		{
			if (pawn.IsColonistPlayerControlled)
			{
				if (psychicRitual.def.nonRequiredPawnsMayLeave)
				{
					PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
					if (psychicRitualRoleDef != null && psychicRitual.assignments.RoleAssignedCount(psychicRitualRoleDef) > psychicRitualRoleDef.MinCount)
					{
						yield return PsychicRitualGizmo.LeaveGizmo(psychicRitual, pawn);
					}
				}
				yield return PsychicRitualGizmo.CancelGizmo(psychicRitual);
			}
			yield break;
		}

		// Token: 0x060088F4 RID: 35060 RVA: 0x0028DA43 File Offset: 0x0028BC43
		public virtual bool CanReach(Pawn pawn, TargetInfo target)
		{
			return pawn.CanReachNonLocal(target, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		// Token: 0x060088F5 RID: 35061 RVA: 0x0028DA50 File Offset: 0x0028BC50
		public virtual void Notify_PawnJobDone(PsychicRitual psychicRitual, Pawn pawn, JobCondition condition)
		{
			if (condition == JobCondition.ErroredPather && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath) && !this.CanReach(pawn, psychicRitual.assignments.Target))
			{
				psychicRitual.LeaveOrCancelPsychicRitual(this, pawn, PsychicRitualRoleDef.ConditionToReason(pawn, PsychicRitualRoleDef.Condition.NoPath));
			}
		}

		// Token: 0x0400594E RID: 22862
		public static IReadOnlyDictionary<DevelopmentalStage, PsychicRitualRoleDef.Condition> DevelopmentalStageToCondition = new Dictionary<DevelopmentalStage, PsychicRitualRoleDef.Condition>
		{
			{
				DevelopmentalStage.None,
				PsychicRitualRoleDef.Condition.None
			},
			{
				DevelopmentalStage.Newborn,
				PsychicRitualRoleDef.Condition.Baby
			},
			{
				DevelopmentalStage.Baby,
				PsychicRitualRoleDef.Condition.Baby
			},
			{
				DevelopmentalStage.Child,
				PsychicRitualRoleDef.Condition.Child
			},
			{
				DevelopmentalStage.Adult,
				PsychicRitualRoleDef.Condition.Adult
			}
		};

		// Token: 0x0400594F RID: 22863
		public static IReadOnlyDictionary<PawnLostCondition, PsychicRitualRoleDef.Condition> PawnLostConditionToCondition = new Dictionary<PawnLostCondition, PsychicRitualRoleDef.Condition>
		{
			{
				PawnLostCondition.ChangedFaction,
				PsychicRitualRoleDef.Condition.NonPlayerFaction
			},
			{
				PawnLostCondition.Drafted,
				PsychicRitualRoleDef.Condition.Drafted
			},
			{
				PawnLostCondition.ExitedMap,
				PsychicRitualRoleDef.Condition.NotOnMap
			},
			{
				PawnLostCondition.ForcedByPlayerAction,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.ForcedByQuest,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.ForcedToJoinOtherLord,
				PsychicRitualRoleDef.Condition.HasAnotherLord
			},
			{
				PawnLostCondition.Incapped,
				PsychicRitualRoleDef.Condition.Downed
			},
			{
				PawnLostCondition.InMentalState,
				PsychicRitualRoleDef.Condition.MentalState
			},
			{
				PawnLostCondition.Killed,
				PsychicRitualRoleDef.Condition.Dead
			},
			{
				PawnLostCondition.LeftVoluntarily,
				PsychicRitualRoleDef.Condition.LeftVoluntarily
			},
			{
				PawnLostCondition.MadePrisoner,
				PsychicRitualRoleDef.Condition.Prisoner
			},
			{
				PawnLostCondition.MadeSlave,
				PsychicRitualRoleDef.Condition.Slave
			},
			{
				PawnLostCondition.NoLongerEnteringTransportPods,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.Undefined,
				PsychicRitualRoleDef.Condition.Error
			},
			{
				PawnLostCondition.Vanished,
				PsychicRitualRoleDef.Condition.Vanished
			}
		};

		// Token: 0x04005950 RID: 22864
		protected IntRange allowedCount = new IntRange(0, 1);

		// Token: 0x04005951 RID: 22865
		protected PsychicRitualRoleDef.Condition allowedConditions = PsychicRitualRoleDef.Condition.Default;

		// Token: 0x04005952 RID: 22866
		protected bool canHandleOfferings;

		// Token: 0x04005953 RID: 22867
		public bool psychicRitualWaitsForArrival = true;

		// Token: 0x04005954 RID: 22868
		public bool showInvocationVfx;

		// Token: 0x04005955 RID: 22869
		public bool applyPowerOffset = true;

		// Token: 0x04005956 RID: 22870
		public bool removeOnLost = true;

		// Token: 0x020016B0 RID: 5808
		[Flags]
		public enum Condition : uint
		{
			// Token: 0x04005958 RID: 22872
			None = 0U,
			// Token: 0x04005959 RID: 22873
			Dead = 1U,
			// Token: 0x0400595A RID: 22874
			Downed = 2U,
			// Token: 0x0400595B RID: 22875
			Drafted = 4U,
			// Token: 0x0400595C RID: 22876
			Sleeping = 8U,
			// Token: 0x0400595D RID: 22877
			Bleeding = 16U,
			// Token: 0x0400595E RID: 22878
			Burning = 32U,
			// Token: 0x0400595F RID: 22879
			MentalState = 64U,
			// Token: 0x04005960 RID: 22880
			ExtremeTemperature = 128U,
			// Token: 0x04005961 RID: 22881
			Humanlike = 256U,
			// Token: 0x04005962 RID: 22882
			NonHumanlike = 512U,
			// Token: 0x04005963 RID: 22883
			Prisoner = 1024U,
			// Token: 0x04005964 RID: 22884
			Slave = 2048U,
			// Token: 0x04005965 RID: 22885
			Freeman = 4096U,
			// Token: 0x04005966 RID: 22886
			InsideBuilding = 8192U,
			// Token: 0x04005967 RID: 22887
			NonPlayerFaction = 16384U,
			// Token: 0x04005968 RID: 22888
			NotOnMap = 32768U,
			// Token: 0x04005969 RID: 22889
			HasAnotherLord = 65536U,
			// Token: 0x0400596A RID: 22890
			LeftVoluntarily = 131072U,
			// Token: 0x0400596B RID: 22891
			Vanished = 262144U,
			// Token: 0x0400596C RID: 22892
			Baby = 524288U,
			// Token: 0x0400596D RID: 22893
			Child = 1048576U,
			// Token: 0x0400596E RID: 22894
			Adult = 2097152U,
			// Token: 0x0400596F RID: 22895
			NoPath = 4194304U,
			// Token: 0x04005970 RID: 22896
			SubHuman = 8388608U,
			// Token: 0x04005971 RID: 22897
			IdeoUnwilling = 16777216U,
			// Token: 0x04005972 RID: 22898
			NoPsychicSensitivity = 33554432U,
			// Token: 0x04005973 RID: 22899
			TemporaryFactionMember = 67108864U,
			// Token: 0x04005974 RID: 22900
			Error = 134217728U,
			// Token: 0x04005975 RID: 22901
			All = 4294967295U,
			// Token: 0x04005976 RID: 22902
			Default = 70260992U,
			// Token: 0x04005977 RID: 22903
			DefaultInvoker = 3152128U
		}

		// Token: 0x020016B1 RID: 5809
		public enum Context
		{
			// Token: 0x04005979 RID: 22905
			InvokePsychicRitualGizmo,
			// Token: 0x0400597A RID: 22906
			Dialog_BeginPsychicRitual,
			// Token: 0x0400597B RID: 22907
			Runtime,
			// Token: 0x0400597C RID: 22908
			NonPlayerFaction
		}

		// Token: 0x020016B2 RID: 5810
		public struct Reason
		{
			// Token: 0x060088F8 RID: 35064 RVA: 0x0028DBBC File Offset: 0x0028BDBC
			public Reason(PsychicRitualRoleDef def, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, AnyEnum reasonCode)
			{
				this.def = def;
				this.context = context;
				this.pawn = pawn;
				this.target = target;
				this.reasonCode = reasonCode;
			}

			// Token: 0x170014F2 RID: 5362
			// (get) Token: 0x060088F9 RID: 35065 RVA: 0x0028DBE4 File Offset: 0x0028BDE4
			public static PsychicRitualRoleDef.Reason None
			{
				get
				{
					return default(PsychicRitualRoleDef.Reason);
				}
			}

			// Token: 0x060088FA RID: 35066 RVA: 0x0028DBFA File Offset: 0x0028BDFA
			public TaggedString ToPlayerReadable()
			{
				PsychicRitualRoleDef psychicRitualRoleDef = this.def;
				if (psychicRitualRoleDef == null)
				{
					return TaggedString.Empty;
				}
				return psychicRitualRoleDef.PawnCannotDoReason(this.reasonCode, this.context, this.pawn, this.target);
			}

			// Token: 0x0400597D RID: 22909
			public PsychicRitualRoleDef def;

			// Token: 0x0400597E RID: 22910
			public PsychicRitualRoleDef.Context context;

			// Token: 0x0400597F RID: 22911
			public Pawn pawn;

			// Token: 0x04005980 RID: 22912
			public TargetInfo target;

			// Token: 0x04005981 RID: 22913
			public AnyEnum reasonCode;
		}
	}
}
