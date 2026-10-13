using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B3 RID: 6323
	public class Thought_Counselled : Thought_Memory
	{
		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x0600907A RID: 36986 RVA: 0x002A9683 File Offset: 0x002A7883
		public override int DurationTicks
		{
			get
			{
				return this.durationTicksOverride;
			}
		}

		// Token: 0x0600907B RID: 36987 RVA: 0x002A968B File Offset: 0x002A788B
		public override float MoodOffset()
		{
			return this.moodOffsetOverride;
		}

		// Token: 0x0600907C RID: 36988 RVA: 0x002A9693 File Offset: 0x002A7893
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.moodOffsetOverride, "moodOffsetOverride", 0f, false);
		}

		// Token: 0x04005C46 RID: 23622
		public float moodOffsetOverride;
	}
}
