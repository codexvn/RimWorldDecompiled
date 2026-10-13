using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000710 RID: 1808
	public class HediffComp_ThoughtSetter : HediffComp
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002C27 RID: 11303 RVA: 0x000E5968 File Offset: 0x000E3B68
		public HediffCompProperties_ThoughtSetter Props
		{
			get
			{
				return (HediffCompProperties_ThoughtSetter)this.props;
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000E5978 File Offset: 0x000E3B78
		public void OverrideMoodOffset(int offset)
		{
			Pawn_NeedsTracker needs = base.Pawn.needs;
			Thought_Memory thought_Memory;
			if (needs == null)
			{
				thought_Memory = null;
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					thought_Memory = null;
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts == null)
					{
						thought_Memory = null;
					}
					else
					{
						MemoryThoughtHandler memories = thoughts.memories;
						thought_Memory = ((memories != null) ? memories.GetFirstMemoryOfDef(this.Props.thought) : null);
					}
				}
			}
			Thought_Memory thought_Memory2 = thought_Memory;
			if (thought_Memory2 != null)
			{
				thought_Memory2.moodOffset = offset;
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000E59D6 File Offset: 0x000E3BD6
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.TryAddMemory();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000E59E8 File Offset: 0x000E3BE8
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			Pawn_NeedsTracker needs = base.Pawn.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			ThoughtHandler thoughts = mood.thoughts;
			if (thoughts == null)
			{
				return;
			}
			MemoryThoughtHandler memories = thoughts.memories;
			if (memories == null)
			{
				return;
			}
			memories.RemoveMemoriesOfDef(this.Props.thought);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000E5A39 File Offset: 0x000E3C39
		public override void Notify_Spawned()
		{
			this.TryAddMemory();
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000E5A44 File Offset: 0x000E3C44
		private void TryAddMemory()
		{
			Pawn_NeedsTracker needs = base.Pawn.needs;
			bool flag;
			if (needs == null)
			{
				flag = (null != null);
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					flag = (null != null);
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts == null)
					{
						flag = (null != null);
					}
					else
					{
						MemoryThoughtHandler memories = thoughts.memories;
						flag = (((memories != null) ? memories.GetFirstMemoryOfDef(this.Props.thought) : null) != null);
					}
				}
			}
			if (flag)
			{
				return;
			}
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(this.Props.thought);
			thought_Memory.permanent = true;
			if (this.Props.moodOffsetRange == FloatRange.Zero)
			{
				thought_Memory.moodOffset = this.Props.moodOffset;
			}
			else
			{
				thought_Memory.moodOffset = Mathf.RoundToInt(this.Props.moodOffsetRange.RandomInRange);
			}
			Pawn_NeedsTracker needs2 = base.Pawn.needs;
			if (needs2 == null)
			{
				return;
			}
			Need_Mood mood2 = needs2.mood;
			if (mood2 == null)
			{
				return;
			}
			ThoughtHandler thoughts2 = mood2.thoughts;
			if (thoughts2 == null)
			{
				return;
			}
			MemoryThoughtHandler memories2 = thoughts2.memories;
			if (memories2 == null)
			{
				return;
			}
			memories2.TryGainMemory(thought_Memory, null);
		}
	}
}
