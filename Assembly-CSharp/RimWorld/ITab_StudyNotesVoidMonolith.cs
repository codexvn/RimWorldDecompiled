using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020033D7 RID: 13271
	public class ITab_StudyNotesVoidMonolith : ITab_StudyNotes
	{
		// Token: 0x170030CC RID: 12492
		// (get) Token: 0x06012A56 RID: 76374 RVA: 0x00584896 File Offset: 0x00582A96
		public override bool IsVisible
		{
			get
			{
				return base.Studiable && base.StudiableThing is Building_VoidMonolith && !Find.Anomaly.QuestlineEnded;
			}
		}

		// Token: 0x170030CD RID: 12493
		// (get) Token: 0x06012A57 RID: 76375 RVA: 0x005848BC File Offset: 0x00582ABC
		protected override IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return Find.Anomaly.MonolithLetters;
			}
		}

		// Token: 0x170030CE RID: 12494
		// (get) Token: 0x06012A58 RID: 76376 RVA: 0x005848C8 File Offset: 0x00582AC8
		protected override bool StudyCompleted
		{
			get
			{
				return Find.Anomaly.MonolithStudyCompleted;
			}
		}
	}
}
