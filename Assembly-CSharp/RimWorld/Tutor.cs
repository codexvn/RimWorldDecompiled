using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034EE RID: 13550
	public class Tutor : IExposable
	{
		// Token: 0x0601309C RID: 77980 RVA: 0x005A4730 File Offset: 0x005A2930
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		// Token: 0x0601309D RID: 77981 RVA: 0x005A477C File Offset: 0x005A297C
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x0601309E RID: 77982 RVA: 0x005A4794 File Offset: 0x005A2994
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x0400C54A RID: 50506
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x0400C54B RID: 50507
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x0400C54C RID: 50508
		public TutorialState tutorialState = new TutorialState();
	}
}
