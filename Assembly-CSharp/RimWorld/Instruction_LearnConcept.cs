using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034DA RID: 13530
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x17003185 RID: 12677
		// (get) Token: 0x0601302D RID: 77869 RVA: 0x005A3233 File Offset: 0x005A1433
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x0601302E RID: 77870 RVA: 0x005A3245 File Offset: 0x005A1445
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x0601302F RID: 77871 RVA: 0x005A3262 File Offset: 0x005A1462
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(this.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
