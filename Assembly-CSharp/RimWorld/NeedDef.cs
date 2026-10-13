using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA3 RID: 6819
	public class NeedDef : Def
	{
		// Token: 0x060098B2 RID: 39090 RVA: 0x002C87FC File Offset: 0x002C69FC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.description.NullOrEmpty() && this.showOnNeedList)
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker) && (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f))
			{
				yield return "seeker rise/fall rates not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400676D RID: 26477
		public Type needClass;

		// Token: 0x0400676E RID: 26478
		public Intelligence minIntelligence;

		// Token: 0x0400676F RID: 26479
		public bool colonistAndPrisonersOnly;

		// Token: 0x04006770 RID: 26480
		public bool playerMechsOnly;

		// Token: 0x04006771 RID: 26481
		public bool colonistsOnly;

		// Token: 0x04006772 RID: 26482
		public bool slavesOnly;

		// Token: 0x04006773 RID: 26483
		public bool onlyIfCausedByHediff;

		// Token: 0x04006774 RID: 26484
		public bool onlyIfCausedByGene;

		// Token: 0x04006775 RID: 26485
		public bool onlyIfCausedByTrait;

		// Token: 0x04006776 RID: 26486
		public bool onlyIfCausedByIdeo;

		// Token: 0x04006777 RID: 26487
		public bool neverOnPrisoner;

		// Token: 0x04006778 RID: 26488
		public bool neverOnSlave;

		// Token: 0x04006779 RID: 26489
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x0400677A RID: 26490
		public List<HediffDef> hediffRequiredAny;

		// Token: 0x0400677B RID: 26491
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x0400677C RID: 26492
		public List<CompProperties> requiredComps;

		// Token: 0x0400677D RID: 26493
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x0400677E RID: 26494
		public bool showOnNeedList = true;

		// Token: 0x0400677F RID: 26495
		public float baseLevel = 0.5f;

		// Token: 0x04006780 RID: 26496
		public bool major;

		// Token: 0x04006781 RID: 26497
		public int listPriority;

		// Token: 0x04006782 RID: 26498
		[NoTranslate]
		public string tutorHighlightTag;

		// Token: 0x04006783 RID: 26499
		public bool showForCaravanMembers;

		// Token: 0x04006784 RID: 26500
		public bool scaleBar;

		// Token: 0x04006785 RID: 26501
		public bool showUnitTicks;

		// Token: 0x04006786 RID: 26502
		public float fallPerDay = 0.5f;

		// Token: 0x04006787 RID: 26503
		public float seekerRisePerHour;

		// Token: 0x04006788 RID: 26504
		public float seekerFallPerHour;

		// Token: 0x04006789 RID: 26505
		public bool freezeWhileSleeping;

		// Token: 0x0400678A RID: 26506
		public bool freezeInMentalState;
	}
}
