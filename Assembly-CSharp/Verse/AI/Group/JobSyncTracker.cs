using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02001052 RID: 4178
	public class JobSyncTracker : IExposable
	{
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060065E3 RID: 26083 RVA: 0x0020480B File Offset: 0x00202A0B
		public bool AllPawnsDone
		{
			get
			{
				return this.pawns.Count == 0;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060065E4 RID: 26084 RVA: 0x0020481B File Offset: 0x00202A1B
		public bool AnyPawnsDone
		{
			get
			{
				return this.initialCount != this.pawns.Count;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060065E5 RID: 26085 RVA: 0x00204833 File Offset: 0x00202A33
		public int PawnDoneCount
		{
			get
			{
				return this.initialCount - this.pawns.Count;
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x060065E6 RID: 26086 RVA: 0x00204847 File Offset: 0x00202A47
		public IEnumerable<Pawn> BlockingPawns
		{
			get
			{
				return this.pawns;
			}
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x0020484F File Offset: 0x00202A4F
		public bool WaitingOnPawn(Pawn pawn)
		{
			return this.pawns.Contains(pawn);
		}

		// Token: 0x060065E8 RID: 26088 RVA: 0x000024DF File Offset: 0x000006DF
		protected JobSyncTracker()
		{
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x00204860 File Offset: 0x00202A60
		public JobSyncTracker(IEnumerable<Pawn> pawns, ILoadReferenceable source, string dutyTag = null, string thinkNodeTag = null, JobCondition allowedTerminalStates = JobCondition.Succeeded)
		{
			this.pawns = new HashSet<Pawn>(pawns);
			this.allowedTerminalStates = allowedTerminalStates;
			this.dutyTag = dutyTag;
			this.initialCount = this.pawns.Count;
			this.source = source;
			this.thinkNodeTag = thinkNodeTag;
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x002048AE File Offset: 0x00202AAE
		public void ForcePawnFinished(Pawn pawn)
		{
			this.pawns.Remove(pawn);
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x002048C0 File Offset: 0x00202AC0
		public void Notify_PawnJobDone(Pawn pawn, string dutyTag, string thinkNodeTag, ILoadReferenceable source, JobCondition condition)
		{
			if (!this.pawns.Contains(pawn))
			{
				return;
			}
			if ((this.allowedTerminalStates & condition) == JobCondition.None)
			{
				return;
			}
			if (this.dutyTag != null && this.dutyTag != dutyTag)
			{
				return;
			}
			if (this.source != null && this.source != source)
			{
				return;
			}
			if (this.thinkNodeTag != null && this.thinkNodeTag != thinkNodeTag)
			{
				return;
			}
			this.pawns.Remove(pawn);
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x00204936 File Offset: 0x00202B36
		public void Notify_PawnJobDone(Pawn pawn, Job job, JobCondition condition)
		{
			string text = job.dutyTag;
			ThinkNode jobGiver = job.jobGiver;
			this.Notify_PawnJobDone(pawn, text, (jobGiver != null) ? jobGiver.tag : null, job.source, condition);
		}

		// Token: 0x060065ED RID: 26093 RVA: 0x0020495E File Offset: 0x00202B5E
		public void Notify_PawnLost(Pawn pawn)
		{
			if (this.pawns.Remove(pawn))
			{
				this.initialCount--;
			}
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x0020497C File Offset: 0x00202B7C
		public void ExposeData()
		{
			Scribe_Values.Look<JobCondition>(ref this.allowedTerminalStates, "allowedTerminalStates", JobCondition.None, false);
			Scribe_Values.Look<string>(ref this.dutyTag, "dutyTag", null, false);
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
		}

		// Token: 0x040048ED RID: 18669
		public JobCondition allowedTerminalStates;

		// Token: 0x040048EE RID: 18670
		public string dutyTag;

		// Token: 0x040048EF RID: 18671
		public string thinkNodeTag;

		// Token: 0x040048F0 RID: 18672
		private int initialCount;

		// Token: 0x040048F1 RID: 18673
		private HashSet<Pawn> pawns;

		// Token: 0x040048F2 RID: 18674
		private ILoadReferenceable source;
	}
}
