using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9E RID: 7326
	public class QuestPart_ReservePawns : QuestPart
	{
		// Token: 0x0600A337 RID: 41783 RVA: 0x002EDE0E File Offset: 0x002EC00E
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x002EDE1C File Offset: 0x002EC01C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040070CC RID: 28876
		public List<Pawn> pawns = new List<Pawn>();
	}
}
