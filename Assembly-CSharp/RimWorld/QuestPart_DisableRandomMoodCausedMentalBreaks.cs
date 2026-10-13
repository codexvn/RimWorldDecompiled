using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C36 RID: 7222
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		// Token: 0x0600A0C8 RID: 41160 RVA: 0x002E4A7C File Offset: 0x002E2C7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A0C9 RID: 41161 RVA: 0x002E4AD8 File Offset: 0x002E2CD8
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006F2B RID: 28459
		public List<Pawn> pawns = new List<Pawn>();
	}
}
