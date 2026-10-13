using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034D5 RID: 13525
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x17003180 RID: 12672
		// (get) Token: 0x0601301C RID: 77852
		protected abstract Area MyArea { get; }

		// Token: 0x17003181 RID: 12673
		// (get) Token: 0x0601301D RID: 77853 RVA: 0x005A305D File Offset: 0x005A125D
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x0601301E RID: 77854 RVA: 0x005A307F File Offset: 0x005A127F
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x0601301F RID: 77855 RVA: 0x005A3098 File Offset: 0x005A1298
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x06013020 RID: 77856 RVA: 0x005A2B45 File Offset: 0x005A0D45
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400C513 RID: 50451
		private int startingAreaCount = -1;
	}
}
