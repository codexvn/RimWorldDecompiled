using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EE0 RID: 3808
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06005FB1 RID: 24497 RVA: 0x001E6C00 File Offset: 0x001E4E00
		public void Clear()
		{
			this.initAction = null;
			this.tickAction = null;
			this.tickIntervalAction = null;
			this.endConditions.Clear();
			this.preInitActions = null;
			this.preTickActions = null;
			this.preTickIntervalActions = null;
			this.finishActions = null;
			this.atomicWithPrevious = false;
			this.socialMode = RandomSocialMode.Normal;
			this.activeSkill = null;
			this.defaultCompleteMode = ToilCompleteMode.Instant;
			this.defaultDuration = 0;
			this.handlingFacing = false;
			this.debugName = null;
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x001E6C7C File Offset: 0x001E4E7C
		public void Cleanup(int myIndex, JobDriver jobDriver)
		{
			if (this.finishActions != null)
			{
				for (int i = 0; i < this.finishActions.Count; i++)
				{
					try
					{
						this.finishActions[i]();
					}
					catch (Exception ex)
					{
						string[] array = new string[9];
						array[0] = string.Format("Pawn {0} threw exception while executing toil {1}'s finish action ({2})", this.actor.ToStringSafe<Pawn>(), this, i);
						array[1] = ", jobDriver=";
						array[2] = jobDriver.ToStringSafe<JobDriver>();
						array[3] = ", job=";
						array[4] = jobDriver.job.ToStringSafe<Job>();
						array[5] = ", toilIndex=";
						array[6] = myIndex.ToString();
						array[7] = ": ";
						int num = 8;
						Exception ex2 = ex;
						array[num] = ((ex2 != null) ? ex2.ToString() : null);
						Log.Error(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x001E6D5C File Offset: 0x001E4F5C
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x001E6D64 File Offset: 0x001E4F64
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x001E6D95 File Offset: 0x001E4F95
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x001E6DA3 File Offset: 0x001E4FA3
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x001E6DC4 File Offset: 0x001E4FC4
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06005FB8 RID: 24504 RVA: 0x001E6DE5 File Offset: 0x001E4FE5
		public void AddPreTickIntervalAction(Action<int> newAct)
		{
			if (this.preTickIntervalActions == null)
			{
				this.preTickIntervalActions = new List<Action<int>>();
			}
			this.preTickIntervalActions.Add(newAct);
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x001E6E06 File Offset: 0x001E5006
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x001E6E27 File Offset: 0x001E5027
		public override string ToString()
		{
			return this.debugName ?? "unnamed";
		}

		// Token: 0x040044E9 RID: 17641
		public Pawn actor;

		// Token: 0x040044EA RID: 17642
		public Action initAction;

		// Token: 0x040044EB RID: 17643
		public Action tickAction;

		// Token: 0x040044EC RID: 17644
		public Action<int> tickIntervalAction;

		// Token: 0x040044ED RID: 17645
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x040044EE RID: 17646
		public List<Action> preInitActions;

		// Token: 0x040044EF RID: 17647
		public List<Action> preTickActions;

		// Token: 0x040044F0 RID: 17648
		public List<Action<int>> preTickIntervalActions;

		// Token: 0x040044F1 RID: 17649
		public List<Action> finishActions;

		// Token: 0x040044F2 RID: 17650
		public bool atomicWithPrevious;

		// Token: 0x040044F3 RID: 17651
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x040044F4 RID: 17652
		public Func<SkillDef> activeSkill;

		// Token: 0x040044F5 RID: 17653
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x040044F6 RID: 17654
		public int defaultDuration;

		// Token: 0x040044F7 RID: 17655
		public bool handlingFacing;

		// Token: 0x040044F8 RID: 17656
		public bool inPool = true;

		// Token: 0x040044F9 RID: 17657
		public string debugName;
	}
}
