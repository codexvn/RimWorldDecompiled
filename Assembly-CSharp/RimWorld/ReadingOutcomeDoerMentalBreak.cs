using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193E RID: 6462
	public class ReadingOutcomeDoerMentalBreak : BookOutcomeDoer
	{
		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x060092F1 RID: 37617 RVA: 0x002B3CF9 File Offset: 0x002B1EF9
		public new BookOutcomeProperties_MentalBreak Props
		{
			get
			{
				return (BookOutcomeProperties_MentalBreak)this.props;
			}
		}

		// Token: 0x060092F2 RID: 37618 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return false;
		}

		// Token: 0x060092F3 RID: 37619 RVA: 0x002B3D08 File Offset: 0x002B1F08
		public override void OnBookGenerated(Pawn author = null)
		{
			float num = this.Props.chancePerHourRange.RandomInRange;
			num = this.Props.chanceMinMaxRange.ClampToRange(num);
			base.Book.SetMentalBreakChance(num);
		}
	}
}
