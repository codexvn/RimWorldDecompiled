using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C7 RID: 455
	public class PriorityWork : IExposable
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0003AC94 File Offset: 0x00038E94
		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0003ACCC File Offset: 0x00038ECC
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0003ACD4 File Offset: 0x00038ED4
		public PriorityWork()
		{
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0003ACF7 File Offset: 0x00038EF7
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0003AD24 File Offset: 0x00038F24
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0003AD6D File Offset: 0x00038F6D
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0003AD8D File Offset: 0x00038F8D
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0003ADA8 File Offset: 0x00038FA8
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0003ADC1 File Offset: 0x00038FC1
		public IEnumerable<Gizmo> GetGizmos()
		{
			if ((this.IsPrioritized || (this.pawn.CurJob != null && this.pawn.CurJob.playerForced && this.pawn.jobs.IsCurrentJobPlayerInterruptible()) || this.pawn.jobs.jobQueue.AnyPlayerForced) && !this.pawn.Drafted && !this.pawn.Deathresting)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandClearPrioritizedWork".Translate(),
					defaultDesc = "CommandClearPrioritizedWorkDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						this.ClearPrioritizedWorkAndJobQueue();
						if (this.pawn.CurJob.playerForced && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					},
					hotKey = KeyBindingDefOf.Designator_Cancel,
					groupKeyIgnoreContent = 6165612
				};
			}
			yield break;
		}

		// Token: 0x0400071F RID: 1823
		private Pawn pawn;

		// Token: 0x04000720 RID: 1824
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x04000721 RID: 1825
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x04000722 RID: 1826
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x04000723 RID: 1827
		private const int Timeout = 30000;
	}
}
