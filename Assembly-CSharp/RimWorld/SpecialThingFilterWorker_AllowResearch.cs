using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200350A RID: 13578
	public abstract class SpecialThingFilterWorker_AllowResearch : SpecialThingFilterWorker
	{
		// Token: 0x06013145 RID: 78149 RVA: 0x005A90BA File Offset: 0x005A72BA
		protected SpecialThingFilterWorker_AllowResearch(ResearchTabDef tab)
		{
			this.tab = tab;
		}

		// Token: 0x06013146 RID: 78150 RVA: 0x005A90CC File Offset: 0x005A72CC
		public override bool Matches(Thing t)
		{
			Book book = t as Book;
			ReadingOutcomeDoerGainResearch readingOutcomeDoerGainResearch;
			return book != null && book.BookComp.TryGetDoer<ReadingOutcomeDoerGainResearch>(out readingOutcomeDoerGainResearch) && readingOutcomeDoerGainResearch.Props.tab == this.tab;
		}

		// Token: 0x06013147 RID: 78151 RVA: 0x00039A6E File Offset: 0x00037C6E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.HasComp<CompBook>();
		}

		// Token: 0x0400C5E0 RID: 50656
		private readonly ResearchTabDef tab;
	}
}
