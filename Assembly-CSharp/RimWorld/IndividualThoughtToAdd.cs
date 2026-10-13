using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002336 RID: 9014
	public struct IndividualThoughtToAdd
	{
		// Token: 0x17001DD8 RID: 7640
		// (get) Token: 0x0600C398 RID: 50072 RVA: 0x0038BBA0 File Offset: 0x00389DA0
		public string LabelCap
		{
			get
			{
				string text = this.thought.LabelCap;
				float num = this.thought.MoodOffset();
				if (num != 0f)
				{
					text = text + " " + Mathf.RoundToInt(num).ToStringWithSign();
				}
				return text;
			}
		}

		// Token: 0x0600C399 RID: 50073 RVA: 0x0038BBE8 File Offset: 0x00389DE8
		public IndividualThoughtToAdd(ThoughtDef thoughtDef, Pawn addTo, Pawn otherPawn = null, float moodPowerFactor = 1f, float opinionOffsetFactor = 1f)
		{
			this.addTo = addTo;
			this.otherPawn = otherPawn;
			this.thought = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			this.thought.moodPowerFactor = moodPowerFactor;
			this.thought.otherPawn = otherPawn;
			this.thought.pawn = addTo;
			Thought_MemorySocial thought_MemorySocial = this.thought as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= opinionOffsetFactor;
			}
		}

		// Token: 0x0600C39A RID: 50074 RVA: 0x0038BC58 File Offset: 0x00389E58
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x04008545 RID: 34117
		public Thought_Memory thought;

		// Token: 0x04008546 RID: 34118
		public Pawn addTo;

		// Token: 0x04008547 RID: 34119
		private Pawn otherPawn;
	}
}
