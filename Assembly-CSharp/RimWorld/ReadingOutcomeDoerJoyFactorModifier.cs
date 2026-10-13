using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193C RID: 6460
	public class ReadingOutcomeDoerJoyFactorModifier : BookOutcomeDoer
	{
		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x060092EA RID: 37610 RVA: 0x002B3C6B File Offset: 0x002B1E6B
		public new BookOutcomeProperties_JoyFactorModifier Props
		{
			get
			{
				return (BookOutcomeProperties_JoyFactorModifier)this.props;
			}
		}

		// Token: 0x060092EB RID: 37611 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return true;
		}

		// Token: 0x060092EC RID: 37612 RVA: 0x002B3C78 File Offset: 0x002B1E78
		public override void OnBookGenerated(Pawn author = null)
		{
			base.Book.SetJoyFactor(BookUtility.GetNovelJoyFactorForQuality(base.Quality));
		}

		// Token: 0x060092ED RID: 37613 RVA: 0x002B3C90 File Offset: 0x002B1E90
		public override string GetBenefitsString(Pawn reader = null)
		{
			return string.Format(" - {0}: x{1}", "BookJoyFactor".Translate(), base.Book.JoyFactor.ToStringPercent());
		}
	}
}
