using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C91 RID: 7313
	public class QuestPart_RequirementsToAcceptPawnOnColonyMap : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600A2F0 RID: 41712 RVA: 0x002ED15C File Offset: 0x002EB35C
		public override AcceptanceReport CanAccept()
		{
			if (this.pawn != null && this.pawn.Map != null && this.pawn.Map.IsPlayerHome)
			{
				return true;
			}
			return new AcceptanceReport("QuestPawnNotOnColonyMap".Translate(this.pawn.Named("PAWN")));
		}

		// Token: 0x0600A2F1 RID: 41713 RVA: 0x002ED1BB File Offset: 0x002EB3BB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x0600A2F2 RID: 41714 RVA: 0x002ED1D4 File Offset: 0x002EB3D4
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.pawn != null)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.pawn, -1, false);
				}
				yield break;
			}
		}

		// Token: 0x040070A9 RID: 28841
		public Pawn pawn;
	}
}
