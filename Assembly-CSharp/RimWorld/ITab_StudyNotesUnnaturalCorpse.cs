using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020033D6 RID: 13270
	public class ITab_StudyNotesUnnaturalCorpse : ITab_StudyNotes
	{
		// Token: 0x170030C6 RID: 12486
		// (get) Token: 0x06012A4F RID: 76367 RVA: 0x00584840 File Offset: 0x00582A40
		public override bool IsVisible
		{
			get
			{
				return base.Studiable && base.StudiableThing is UnnaturalCorpse;
			}
		}

		// Token: 0x170030C7 RID: 12487
		// (get) Token: 0x06012A50 RID: 76368 RVA: 0x0058485A File Offset: 0x00582A5A
		private UnnaturalCorpse UnnaturalCorpse
		{
			get
			{
				return (UnnaturalCorpse)base.StudiableThing;
			}
		}

		// Token: 0x170030C8 RID: 12488
		// (get) Token: 0x06012A51 RID: 76369 RVA: 0x00584867 File Offset: 0x00582A67
		private UnnaturalCorpseTracker Tracker
		{
			get
			{
				return this.UnnaturalCorpse.Tracker;
			}
		}

		// Token: 0x170030C9 RID: 12489
		// (get) Token: 0x06012A52 RID: 76370 RVA: 0x00584874 File Offset: 0x00582A74
		protected override IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.Tracker.Letters;
			}
		}

		// Token: 0x170030CA RID: 12490
		// (get) Token: 0x06012A53 RID: 76371 RVA: 0x00584881 File Offset: 0x00582A81
		protected override bool StudyCompleted
		{
			get
			{
				return this.Tracker.CanDestroyViaResearch;
			}
		}

		// Token: 0x170030CB RID: 12491
		// (get) Token: 0x06012A54 RID: 76372 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DrawThingIcon
		{
			get
			{
				return false;
			}
		}
	}
}
