using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034C2 RID: 13506
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x1700316E RID: 12654
		// (get) Token: 0x06012FAA RID: 77738 RVA: 0x005A18CB File Offset: 0x0059FACB
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x1700316F RID: 12655
		// (get) Token: 0x06012FAB RID: 77739 RVA: 0x005A18D3 File Offset: 0x0059FAD3
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x06012FAC RID: 77740 RVA: 0x005A18EC File Offset: 0x0059FAEC
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
		}

		// Token: 0x06012FAD RID: 77741 RVA: 0x005A1904 File Offset: 0x0059FB04
		public void Activate(InstructionDef id)
		{
			Lesson_Instruction lesson_Instruction = this.activeLesson as Lesson_Instruction;
			if (lesson_Instruction != null && id == lesson_Instruction.def)
			{
				return;
			}
			Lesson_Instruction lesson_Instruction2 = (Lesson_Instruction)Activator.CreateInstance(id.instructionClass);
			lesson_Instruction2.def = id;
			this.activeLesson = lesson_Instruction2;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06012FAE RID: 77742 RVA: 0x005A1954 File Offset: 0x0059FB54
		public void Activate(Lesson lesson)
		{
			Lesson_Note lesson_Note = lesson as Lesson_Note;
			if (lesson_Note != null && this.activeLesson != null)
			{
				lesson_Note.doFadeIn = false;
			}
			this.activeLesson = lesson;
			this.activeLesson.OnActivated();
		}

		// Token: 0x06012FAF RID: 77743 RVA: 0x005A198C File Offset: 0x0059FB8C
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x06012FB0 RID: 77744 RVA: 0x005A19B0 File Offset: 0x0059FBB0
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonOnGUI();
		}

		// Token: 0x06012FB1 RID: 77745 RVA: 0x005A19D2 File Offset: 0x0059FBD2
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonUpdate();
		}

		// Token: 0x06012FB2 RID: 77746 RVA: 0x005A19F4 File Offset: 0x0059FBF4
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x0400C4F0 RID: 50416
		private Lesson activeLesson;
	}
}
