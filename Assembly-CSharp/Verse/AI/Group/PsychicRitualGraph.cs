using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001060 RID: 4192
	public class PsychicRitualGraph : PsychicRitualToil
	{
		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06006633 RID: 26163 RVA: 0x00205C48 File Offset: 0x00203E48
		public PsychicRitualGraph Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06006634 RID: 26164 RVA: 0x00205C50 File Offset: 0x00203E50
		public PsychicRitualGraph RootGraph
		{
			get
			{
				PsychicRitualGraph psychicRitualGraph = this;
				for (int i = 0; i < 1000; i++)
				{
					if (psychicRitualGraph.parent == null)
					{
						return psychicRitualGraph;
					}
					psychicRitualGraph = psychicRitualGraph.parent;
				}
				throw new InvalidOperationException("Max depth exceeded while trying to find root for PsychicRitualGraph " + this.ToString() + ". Is there a cycle?");
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06006635 RID: 26165 RVA: 0x00205C9C File Offset: 0x00203E9C
		public int ToilCount
		{
			get
			{
				return this.toils.Count;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06006636 RID: 26166 RVA: 0x00205CA9 File Offset: 0x00203EA9
		public PsychicRitualToil CurrentToil
		{
			get
			{
				return this.GetToil(this.currentToilIndex);
			}
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x00205CB7 File Offset: 0x00203EB7
		public PsychicRitualGraph()
		{
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x00205CC6 File Offset: 0x00203EC6
		public PsychicRitualGraph(IEnumerable<PsychicRitualToil> toils)
		{
			this.toils = new List<PsychicRitualToil>(toils);
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x00205CE1 File Offset: 0x00203EE1
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.parent = parent;
			this.toils = (this.toils ?? psychicRitual.def.CreateToils(psychicRitual, this));
			PsychicRitualToilRunner.Start(this.GetToil(this.currentToilIndex), psychicRitual, this);
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x00205D1A File Offset: 0x00203F1A
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			PsychicRitualToilRunner.End(this.GetToil(this.currentToilIndex), psychicRitual, this, success);
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x00205D30 File Offset: 0x00203F30
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			int num = this.currentToilIndex;
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			for (int i = 0; i < 1000; i++)
			{
				if (toil == null)
				{
					return true;
				}
				bool flag = PsychicRitualToilRunner.Tick(toil, psychicRitual, this);
				if (!flag)
				{
					return false;
				}
				if (flag && !this.willAdvancePastLastToil && this.currentToilIndex + 1 == this.toils.Count)
				{
					return true;
				}
				if (DebugSettings.logPsychicRitualTransitions)
				{
					Log.Warning(string.Format("Toil {0} has finished during tick {1}.", toil, Find.TickManager.TicksGame));
				}
				PsychicRitualToilRunner.End(toil, psychicRitual, this, true);
				this.currentToilIndex = (this.nextToilIndexOverride ?? (this.currentToilIndex + 1));
				this.nextToilIndexOverride = null;
				toil = this.GetToil(this.currentToilIndex);
				PsychicRitualToilRunner.Start(toil, psychicRitual, this);
				PsychicRitualToilRunner.UpdateAllDuties(toil, psychicRitual, this);
				if (toil != null)
				{
					foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
					{
						if (toil.ClearJobOnStart(psychicRitual, parent, pawn) && pawn != null)
						{
							Pawn_JobTracker jobs = pawn.jobs;
							if (jobs != null)
							{
								jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
						}
					}
				}
			}
			throw new InvalidOperationException(string.Format("Max iterations exceeded, starting from toil {0}: {1} ", num, this.GetToil(num).ToStringSafe<PsychicRitualToil>()) + string.Format("and ending on toil {0}: {1}", this.currentToilIndex, this.GetToil(num).ToStringSafe<PsychicRitualToil>()));
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x00205ED0 File Offset: 0x002040D0
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToilRunner.UpdateAllDuties(this.GetToil(this.currentToilIndex), psychicRitual, this);
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x00205EE5 File Offset: 0x002040E5
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn victim, PawnLostCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_PawnLost(psychicRitual, this, victim, condition);
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x00205F02 File Offset: 0x00204102
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_PawnJobDone(psychicRitual, this, pawn, job, condition);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x00205F21 File Offset: 0x00204121
		public override void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingLost(psychicRitual, this, building);
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x00205F3C File Offset: 0x0020413C
		public override void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_CorpseLost(psychicRitual, this, corpse);
		}

		// Token: 0x06006641 RID: 26177 RVA: 0x00205F57 File Offset: 0x00204157
		public override void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_ReachedDutyLocation(psychicRitual, this, pawn);
		}

		// Token: 0x06006642 RID: 26178 RVA: 0x00205F72 File Offset: 0x00204172
		public override void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_ConstructionFailed(psychicRitual, this, pawn, frame, newBlueprint);
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x00205F91 File Offset: 0x00204191
		public override void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingSpawnedOnMap(psychicRitual, this, building);
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x00205FAC File Offset: 0x002041AC
		public override void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingDespawnedOnMap(psychicRitual, this, building);
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x00205FC7 File Offset: 0x002041C7
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return result;
			}
			return toil.Notify_DutyResult(psychicRitual, this, result, pawn, issueParams);
		}

		// Token: 0x06006646 RID: 26182 RVA: 0x00205FE7 File Offset: 0x002041E7
		public override ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return result;
			}
			return toil.Notify_DutyConstantResult(psychicRitual, this, result, pawn, issueParams);
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x00206007 File Offset: 0x00204207
		public override string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetPawnReport(psychicRitual, this, pawn);
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x00206023 File Offset: 0x00204223
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetJobReport(psychicRitual, this, pawn);
		}

		// Token: 0x06006649 RID: 26185 RVA: 0x0020603F File Offset: 0x0020423F
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetReport(psychicRitual, this);
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x0020605C File Offset: 0x0020425C
		public override AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.AbilityAllowed(psychicRitual, parent, ability);
			}
			return toil.AbilityAllowed(psychicRitual, this, ability);
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x0020608C File Offset: 0x0020428C
		public override AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.AllowsFloatMenu(psychicRitual, parent, pawn);
			}
			return toil.AllowsFloatMenu(psychicRitual, this, pawn);
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x002060BC File Offset: 0x002042BC
		public override bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.BlocksSocialInteraction(psychicRitual, parent, pawn);
			}
			return toil.BlocksSocialInteraction(psychicRitual, this, pawn);
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x002060EC File Offset: 0x002042EC
		public override bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.DutyActiveWhenDown(psychicRitual, parent, pawn);
			}
			return toil.DutyActiveWhenDown(psychicRitual, this, pawn);
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x0020611C File Offset: 0x0020431C
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.GetPawnGizmos(psychicRitual, parent, pawn);
			}
			return toil.GetPawnGizmos(psychicRitual, this, pawn);
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x0020614C File Offset: 0x0020434C
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.GetBuildingGizmos(psychicRitual, parent, building);
			}
			return toil.GetBuildingGizmos(psychicRitual, this, building);
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x0020617C File Offset: 0x0020437C
		public override void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				base.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
				return;
			}
			toil.SetPawnDuty(pawn, psychicRitual, this, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
		}

		// Token: 0x06006651 RID: 26193 RVA: 0x002061D8 File Offset: 0x002043D8
		public override bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
			}
			return toil.ShouldRemovePawn(psychicRitual, this, pawn, condition);
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x0020620C File Offset: 0x0020440C
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.ClearJobOnStart(psychicRitual, parent, pawn);
			}
			return toil.ClearJobOnStart(psychicRitual, this, pawn);
		}

		// Token: 0x06006653 RID: 26195 RVA: 0x0020623C File Offset: 0x0020443C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.nextToilIndexOverride, "nextToilIndexOverride", null, false);
			Scribe_Values.Look<int>(ref this.currentToilIndex, "currentToilIndex", 0, false);
			Scribe_Collections.Look<PsychicRitualToil>(ref this.toils, "toils", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<PsychicRitualGraph>(ref this.parent, "parent", false);
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x002062A4 File Offset: 0x002044A4
		public PsychicRitualToil GetToil(int toilIndex)
		{
			if (this.toils == null)
			{
				return null;
			}
			if (toilIndex == -1)
			{
				return null;
			}
			if (toilIndex == this.toils.Count)
			{
				return null;
			}
			if (this.currentToilIndex < 0 || this.currentToilIndex > this.toils.Count)
			{
				throw new InvalidOperationException(string.Format("Cannot start toil at index {0}; ", this.currentToilIndex) + string.Format("it is outside the range of valid toils [0, ${0})", this.toils.Count));
			}
			return this.toils[toilIndex];
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x00206334 File Offset: 0x00204534
		public bool SetNextToil(PsychicRitualToil toil)
		{
			if (this.toils == null)
			{
				throw new InvalidOperationException("Cannot set next toil to " + toil.ToStringSafe<PsychicRitualToil>() + "; there are no toils set.");
			}
			int num;
			if (toil == null)
			{
				num = -1;
			}
			else
			{
				num = this.toils.IndexOf(toil);
				if (num < 0)
				{
					throw new ArgumentException("Cannot set next toil to " + toil.ToStringSafe<PsychicRitualToil>() + "; it is not in the list of toils.");
				}
			}
			this.nextToilIndexOverride = new int?(num);
			return true;
		}

		// Token: 0x0400491D RID: 18717
		public const int EndPsychicRitualIndex = -1;

		// Token: 0x0400491E RID: 18718
		protected const int MaxJumpsPerTick = 1000;

		// Token: 0x0400491F RID: 18719
		protected const int MaxGraphDepth = 1000;

		// Token: 0x04004920 RID: 18720
		protected int? nextToilIndexOverride;

		// Token: 0x04004921 RID: 18721
		protected int currentToilIndex;

		// Token: 0x04004922 RID: 18722
		protected List<PsychicRitualToil> toils;

		// Token: 0x04004923 RID: 18723
		protected PsychicRitualGraph parent;

		// Token: 0x04004924 RID: 18724
		public bool willAdvancePastLastToil = true;
	}
}
