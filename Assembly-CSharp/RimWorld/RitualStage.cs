using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020026B6 RID: 9910
	public class RitualStage : IExposable
	{
		// Token: 0x0600D94D RID: 55629 RVA: 0x004056D0 File Offset: 0x004038D0
		public RitualRoleBehavior BehaviorForRole(string roleId)
		{
			if (roleId == null)
			{
				return null;
			}
			RitualRoleBehavior result;
			if (!this.behaviorForRole.TryGetValue(roleId, out result) && this.roleBehaviors != null)
			{
				for (int i = 0; i < this.roleBehaviors.Count; i++)
				{
					RitualRoleBehavior ritualRoleBehavior = this.roleBehaviors[i];
					if (ritualRoleBehavior.roleId == roleId)
					{
						this.behaviorForRole[roleId] = ritualRoleBehavior;
						result = ritualRoleBehavior;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600D94E RID: 55630 RVA: 0x00405740 File Offset: 0x00403940
		public DutyDef GetDuty(Pawn pawn, RitualRole forcedRole = null, LordJob_Ritual ritual = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					if (ritualRoleBehavior == null)
					{
						return null;
					}
					return ritualRoleBehavior.dutyDef;
				}
				else
				{
					RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
					if (ritualRole != null)
					{
						RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
						if (ritualRoleBehavior2 != null)
						{
							return ritualRoleBehavior2.dutyDef;
						}
					}
				}
			}
			return this.defaultDuty;
		}

		// Token: 0x0600D94F RID: 55631 RVA: 0x004057A4 File Offset: 0x004039A4
		public bool HasRole(Pawn p)
		{
			List<RitualRole> roles = this.parent.roles;
			for (int i = 0; i < roles.Count; i++)
			{
				string text;
				if (roles[i].AppliesToPawn(p, out text, TargetInfo.Invalid, null, null, null, true))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D950 RID: 55632 RVA: 0x004057EC File Offset: 0x004039EC
		public bool Applies(Precept_Ritual ritual, List<Pawn> participants)
		{
			foreach (RitualRole ritualRole in this.parent.RequiredRoles())
			{
				bool flag = false;
				Precept_Role precept_Role = (Precept_Role)Faction.OfPlayer.ideos.GetPrecept(ritualRole.precept);
				if (precept_Role == null)
				{
					return false;
				}
				foreach (Pawn p in participants)
				{
					if (precept_Role.Active && precept_Role.IsAssigned(p))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D951 RID: 55633 RVA: 0x004058B8 File Offset: 0x00403AB8
		public PawnStagePosition GetPawnPosition(IntVec3 spot, Pawn pawn, LordJob_Ritual ritual, RitualRole forcedRole = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					RitualPosition ritualPosition = (ritualRoleBehavior != null) ? ritualRoleBehavior.GetPosition(spot, pawn, ritual) : null;
					if (ritualPosition != null)
					{
						return ritualPosition.GetCell(spot, pawn, ritual);
					}
				}
				RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
				if (ritualRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
					if (ritualRoleBehavior2 != null)
					{
						RitualPosition position = ritualRoleBehavior2.GetPosition(spot, pawn, ritual);
						if (position != null)
						{
							return position.GetCell(spot, pawn, ritual);
						}
					}
				}
			}
			return new PawnStagePosition(spot, null, Rot4.Invalid, false);
		}

		// Token: 0x0600D952 RID: 55634 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float ProgressPerTick(LordJob_Ritual ritual)
		{
			return 1f;
		}

		// Token: 0x0600D953 RID: 55635 RVA: 0x00405945 File Offset: 0x00403B45
		public virtual TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return TargetInfo.Invalid;
		}

		// Token: 0x0600D954 RID: 55636 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			return null;
		}

		// Token: 0x0600D955 RID: 55637 RVA: 0x0040594C File Offset: 0x00403B4C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.parent, "parent");
			Scribe_Defs.Look<DutyDef>(ref this.defaultDuty, "defaultDuty");
			Scribe_Collections.Look<StageEndTrigger>(ref this.endTriggers, "endTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.spectatorsRequired, "spectatorsRequired", false, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.allowedSpectateSidesOverride, "allowedSpectateSidesOverride", SpectateRectSide.None, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistanceOverride, "spectateDistanceOverride", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.showProgressBar, "showProgressBar", false, false);
			Scribe_Collections.Look<StageFailTrigger>(ref this.failTriggers, "failTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualRoleBehavior>(ref this.roleBehaviors, "roleBehaviors", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.highlightRolePositions, "highlightRolePositions", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.visualEffectDef, "visualEffectDef");
			Scribe_Values.Look<bool>(ref this.ignoreDurationToFinishAfterStage, "ignoreDurationToFinishAfterStage", false, false);
			Scribe_Deep.Look<RitualStageAction>(ref this.postAction, "postAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.preAction, "preAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.interruptedAction, "interruptedAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.pawnLeaveAction, "pawnLeaveAction", Array.Empty<object>());
		}

		// Token: 0x0400933A RID: 37690
		public RitualBehaviorDef parent;

		// Token: 0x0400933B RID: 37691
		public DutyDef defaultDuty;

		// Token: 0x0400933C RID: 37692
		public List<StageEndTrigger> endTriggers;

		// Token: 0x0400933D RID: 37693
		public List<StageFailTrigger> failTriggers;

		// Token: 0x0400933E RID: 37694
		public RitualStageAction preAction;

		// Token: 0x0400933F RID: 37695
		public RitualStageAction postAction;

		// Token: 0x04009340 RID: 37696
		public RitualStageAction interruptedAction;

		// Token: 0x04009341 RID: 37697
		public RitualStageAction pawnLeaveAction;

		// Token: 0x04009342 RID: 37698
		[NoTranslate]
		public List<string> highlightRolePositions = new List<string>();

		// Token: 0x04009343 RID: 37699
		[NoTranslate]
		public List<string> highlightRolePawns = new List<string>();

		// Token: 0x04009344 RID: 37700
		public RitualStageTickActionMaker tickActionMaker;

		// Token: 0x04009345 RID: 37701
		public RitualVisualEffectDef visualEffectDef;

		// Token: 0x04009346 RID: 37702
		public bool spectatorsRequired;

		// Token: 0x04009347 RID: 37703
		public bool essential;

		// Token: 0x04009348 RID: 37704
		public bool ignoreDurationToFinishAfterStage;

		// Token: 0x04009349 RID: 37705
		public SpectateRectSide allowedSpectateSidesOverride;

		// Token: 0x0400934A RID: 37706
		public IntRange spectateDistanceOverride = IntRange.Zero;

		// Token: 0x0400934B RID: 37707
		public bool showProgressBar = true;

		// Token: 0x0400934C RID: 37708
		public List<RitualRoleBehavior> roleBehaviors;

		// Token: 0x0400934D RID: 37709
		private Dictionary<string, RitualRoleBehavior> behaviorForRole = new Dictionary<string, RitualRoleBehavior>();
	}
}
