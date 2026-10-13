using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001813 RID: 6163
	public class Thought_Situational_WearingDesiredApparel : Thought_Situational
	{
		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x002A423C File Offset: 0x002A243C
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : this.SourceApparelPrecept.apparelDef.label.Named("APPAREL")).CapitalizeFirst();
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06008ED5 RID: 36565 RVA: 0x002A42A4 File Offset: 0x002A24A4
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(this.SourceApparelPrecept.apparelDef.label, false, false).Named("APPAREL"), this.pawn.Named("PAWN")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x002A4332 File Offset: 0x002A2532
		private Precept_Apparel SourceApparelPrecept
		{
			get
			{
				return (Precept_Apparel)this.sourcePrecept;
			}
		}

		// Token: 0x06008ED7 RID: 36567 RVA: 0x002A4340 File Offset: 0x002A2540
		protected override ThoughtState CurrentStateInternal()
		{
			if (!ModsConfig.IdeologyActive || this.pawn.Ideo == null)
			{
				return ThoughtState.Inactive;
			}
			List<Precept> allPreceptsAllowingSituationalThought = this.pawn.Ideo.GetAllPreceptsAllowingSituationalThought(this.def);
			if (allPreceptsAllowingSituationalThought.Count == 0)
			{
				return ThoughtState.Inactive;
			}
			int num = 0;
			foreach (Precept precept in allPreceptsAllowingSituationalThought)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null && (precept_Apparel.TargetGender == Gender.None || precept_Apparel.TargetGender == this.pawn.gender))
				{
					if (!this.HasApparel(precept_Apparel.apparelDef))
					{
						return ThoughtState.Inactive;
					}
					num++;
				}
			}
			this.multipleApparelItems = (num > 1);
			if (num != 0)
			{
				return base.CurrentStateInternal();
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06008ED8 RID: 36568 RVA: 0x002A4424 File Offset: 0x002A2624
		private bool HasApparel(ThingDef thingDef)
		{
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008ED9 RID: 36569 RVA: 0x002A4488 File Offset: 0x002A2688
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.multipleApparelItems, "multipleApparelItems", false, false);
		}

		// Token: 0x04005C18 RID: 23576
		public bool multipleApparelItems;
	}
}
