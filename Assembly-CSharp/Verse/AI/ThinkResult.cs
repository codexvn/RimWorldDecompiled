using System;

namespace Verse.AI
{
	// Token: 0x02001010 RID: 4112
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060064B9 RID: 25785 RVA: 0x001FE31A File Offset: 0x001FC51A
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060064BA RID: 25786 RVA: 0x001FE322 File Offset: 0x001FC522
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060064BB RID: 25787 RVA: 0x001FE32A File Offset: 0x001FC52A
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x060064BC RID: 25788 RVA: 0x001FE332 File Offset: 0x001FC532
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x060064BD RID: 25789 RVA: 0x001FE33A File Offset: 0x001FC53A
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060064BE RID: 25790 RVA: 0x001FE348 File Offset: 0x001FC548
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x001FE366 File Offset: 0x001FC566
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x001FE388 File Offset: 0x001FC588
		public override string ToString()
		{
			string text = (this.Job != null) ? this.Job.ToString() : "null";
			string text2 = (this.SourceNode != null) ? this.SourceNode.ToString() : "null";
			return string.Concat(new string[]
			{
				"(job=",
				text,
				" sourceNode=",
				text2,
				")"
			});
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x001FE3F6 File Offset: 0x001FC5F6
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		// Token: 0x060064C2 RID: 25794 RVA: 0x001FE425 File Offset: 0x001FC625
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x001FE440 File Offset: 0x001FC640
		public bool Equals(ThinkResult other)
		{
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? jobTag = this.tag;
				JobTag? jobTag2 = other.tag;
				if (jobTag.GetValueOrDefault() == jobTag2.GetValueOrDefault() & jobTag != null == (jobTag2 != null))
				{
					return this.fromQueue == other.fromQueue;
				}
			}
			return false;
		}

		// Token: 0x060064C4 RID: 25796 RVA: 0x001FE4AA File Offset: 0x001FC6AA
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x001FE4B4 File Offset: 0x001FC6B4
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040047FF RID: 18431
		private Job jobInt;

		// Token: 0x04004800 RID: 18432
		private ThinkNode sourceNodeInt;

		// Token: 0x04004801 RID: 18433
		private JobTag? tag;

		// Token: 0x04004802 RID: 18434
		private bool fromQueue;
	}
}
