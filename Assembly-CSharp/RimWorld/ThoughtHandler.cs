using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002534 RID: 9524
	public sealed class ThoughtHandler : IExposable
	{
		// Token: 0x0600D052 RID: 53330 RVA: 0x003D4789 File Offset: 0x003D2989
		public ThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
			this.memories = new MemoryThoughtHandler(pawn);
			this.situational = new SituationalThoughtHandler(pawn);
		}

		// Token: 0x0600D053 RID: 53331 RVA: 0x003D47B0 File Offset: 0x003D29B0
		public void ExposeData()
		{
			Scribe_Deep.Look<MemoryThoughtHandler>(ref this.memories, "memories", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x0600D054 RID: 53332 RVA: 0x003D47D1 File Offset: 0x003D29D1
		public void ThoughtInterval()
		{
			this.situational.SituationalThoughtInterval();
			this.memories.MemoryThoughtInterval();
		}

		// Token: 0x0600D055 RID: 53333 RVA: 0x003D47EC File Offset: 0x003D29EC
		public void GetMoodThoughtsFor(ThoughtDef def, List<Thought> thoughts)
		{
			thoughts.Clear();
			for (int i = 0; i < this.memories.Memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories.Memories[i];
				if (thought_Memory.def == def && thought_Memory.MoodOffset() != 0f)
				{
					thoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(def, thoughts);
		}

		// Token: 0x0600D056 RID: 53334 RVA: 0x003D4858 File Offset: 0x003D2A58
		public void GetAllMoodThoughts(List<Thought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				Thought_Memory thought_Memory = list[i];
				if (thought_Memory.MoodOffset() != 0f)
				{
					outThoughts.Add(thought_Memory);
				}
			}
			this.situational.AppendMoodThoughts(outThoughts);
		}

		// Token: 0x0600D057 RID: 53335 RVA: 0x003D48B0 File Offset: 0x003D2AB0
		public void GetMoodThoughts(Thought group, List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!outThoughts[i].GroupsWith(group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600D058 RID: 53336 RVA: 0x003D48F0 File Offset: 0x003D2AF0
		public float MoodOffsetOfGroup(Thought group)
		{
			this.GetMoodThoughts(group, ThoughtHandler.tmpThoughts);
			if (!ThoughtHandler.tmpThoughts.Any<Thought>())
			{
				return 0f;
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < ThoughtHandler.tmpThoughts.Count; i++)
			{
				Thought thought = ThoughtHandler.tmpThoughts[i];
				num += thought.MoodOffset();
				num3 += num2;
				num2 *= thought.def.stackedEffectMultiplier;
			}
			float num4 = num / (float)ThoughtHandler.tmpThoughts.Count;
			ThoughtHandler.tmpThoughts.Clear();
			return num4 * num3;
		}

		// Token: 0x0600D059 RID: 53337 RVA: 0x003D4988 File Offset: 0x003D2B88
		public void GetDistinctMoodThoughtGroups(List<Thought> outThoughts)
		{
			this.GetAllMoodThoughts(outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				Thought other = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (outThoughts[j].GroupsWith(other))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600D05A RID: 53338 RVA: 0x003D49DC File Offset: 0x003D2BDC
		public float TotalMoodOffset()
		{
			this.GetDistinctMoodThoughtGroups(ThoughtHandler.tmpTotalMoodOffsetThoughts);
			float num = 0f;
			for (int i = 0; i < ThoughtHandler.tmpTotalMoodOffsetThoughts.Count; i++)
			{
				num += this.MoodOffsetOfGroup(ThoughtHandler.tmpTotalMoodOffsetThoughts[i]);
			}
			ThoughtHandler.tmpTotalMoodOffsetThoughts.Clear();
			return num;
		}

		// Token: 0x0600D05B RID: 53339 RVA: 0x003D4A30 File Offset: 0x003D2C30
		public void GetSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			outThoughts.Clear();
			List<Thought_Memory> list = this.memories.Memories;
			for (int i = 0; i < list.Count; i++)
			{
				ISocialThought socialThought = list[i] as ISocialThought;
				if (socialThought != null && socialThought.OtherPawn() == otherPawn)
				{
					outThoughts.Add(socialThought);
				}
			}
			this.situational.AppendSocialThoughts(otherPawn, outThoughts);
		}

		// Token: 0x0600D05C RID: 53340 RVA: 0x003D4A90 File Offset: 0x003D2C90
		public void GetSocialThoughts(Pawn otherPawn, ISocialThought group, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				if (!((Thought)outThoughts[i]).GroupsWith((Thought)group))
				{
					outThoughts.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600D05D RID: 53341 RVA: 0x003D4AD8 File Offset: 0x003D2CD8
		public int OpinionOffsetOfGroup(ISocialThought group, Pawn otherPawn)
		{
			this.GetSocialThoughts(otherPawn, group, ThoughtHandler.tmpSocialThoughts);
			for (int i = ThoughtHandler.tmpSocialThoughts.Count - 1; i >= 0; i--)
			{
				if (ThoughtHandler.tmpSocialThoughts[i].OpinionOffset() == 0f)
				{
					ThoughtHandler.tmpSocialThoughts.RemoveAt(i);
				}
			}
			if (!ThoughtHandler.tmpSocialThoughts.Any<ISocialThought>())
			{
				return 0;
			}
			ThoughtDef def = ((Thought)group).def;
			if (def.IsMemory && def.stackedEffectMultiplier != 1f)
			{
				ThoughtHandler.tmpSocialThoughts.Sort((ISocialThought a, ISocialThought b) => ((Thought_Memory)a).age.CompareTo(((Thought_Memory)b).age));
			}
			float num = 0f;
			float num2 = 1f;
			for (int j = 0; j < ThoughtHandler.tmpSocialThoughts.Count; j++)
			{
				num += ThoughtHandler.tmpSocialThoughts[j].OpinionOffset() * num2;
				num2 *= ((Thought)ThoughtHandler.tmpSocialThoughts[j]).def.stackedEffectMultiplier;
			}
			ThoughtHandler.tmpSocialThoughts.Clear();
			if (num == 0f)
			{
				return 0;
			}
			if (num > 0f)
			{
				return Mathf.Max(Mathf.RoundToInt(num), 1);
			}
			return Mathf.Min(Mathf.RoundToInt(num), -1);
		}

		// Token: 0x0600D05E RID: 53342 RVA: 0x003D4C14 File Offset: 0x003D2E14
		public void GetDistinctSocialThoughtGroups(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.GetSocialThoughts(otherPawn, outThoughts);
			for (int i = outThoughts.Count - 1; i >= 0; i--)
			{
				ISocialThought socialThought = outThoughts[i];
				for (int j = 0; j < i; j++)
				{
					if (((Thought)outThoughts[j]).GroupsWith((Thought)socialThought))
					{
						outThoughts.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600D05F RID: 53343 RVA: 0x003D4C74 File Offset: 0x003D2E74
		public int TotalOpinionOffset(Pawn otherPawn)
		{
			this.GetDistinctSocialThoughtGroups(otherPawn, ThoughtHandler.tmpTotalOpinionOffsetThoughts);
			int num = 0;
			for (int i = 0; i < ThoughtHandler.tmpTotalOpinionOffsetThoughts.Count; i++)
			{
				num += this.OpinionOffsetOfGroup(ThoughtHandler.tmpTotalOpinionOffsetThoughts[i], otherPawn);
			}
			ThoughtHandler.tmpTotalOpinionOffsetThoughts.Clear();
			return num;
		}

		// Token: 0x04008DC0 RID: 36288
		public Pawn pawn;

		// Token: 0x04008DC1 RID: 36289
		public MemoryThoughtHandler memories;

		// Token: 0x04008DC2 RID: 36290
		public SituationalThoughtHandler situational;

		// Token: 0x04008DC3 RID: 36291
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x04008DC4 RID: 36292
		private static List<Thought> tmpTotalMoodOffsetThoughts = new List<Thought>();

		// Token: 0x04008DC5 RID: 36293
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		// Token: 0x04008DC6 RID: 36294
		private static List<ISocialThought> tmpTotalOpinionOffsetThoughts = new List<ISocialThought>();
	}
}
