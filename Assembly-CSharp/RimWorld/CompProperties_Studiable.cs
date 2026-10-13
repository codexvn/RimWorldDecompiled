using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E5B RID: 11867
	public class CompProperties_Studiable : CompProperties
	{
		// Token: 0x17002ACE RID: 10958
		// (get) Token: 0x06010B40 RID: 68416 RVA: 0x004D0D77 File Offset: 0x004CEF77
		public bool Completable
		{
			get
			{
				return this.studyAmountToComplete > 0f;
			}
		}

		// Token: 0x06010B41 RID: 68417 RVA: 0x004D0D86 File Offset: 0x004CEF86
		public CompProperties_Studiable()
		{
			this.compClass = typeof(CompStudiable);
		}

		// Token: 0x06010B42 RID: 68418 RVA: 0x004D0DC2 File Offset: 0x004CEFC2
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.requiresHoldingPlatform && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
			{
				yield return "requiresHoldingPlatform can only be used on Pawns";
			}
			if (this.requiresImprisonment && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
			{
				yield return "requiresImprisonment can only be used on Pawns";
			}
			if (ModsConfig.AnomalyActive)
			{
				float? num = this.anomalyKnowledge;
				float num2 = 0f;
				if (num.GetValueOrDefault() > num2 & num != null)
				{
					if (this.knowledgeCategory == null && parentDef != ThingDefOf.VoidMonolith)
					{
						yield return "anomalyKnowledge is set but knowledgeCategory is not";
					}
					if (this.minMonolithLevelForStudy == 0)
					{
						yield return "anomalyKnowledge is set but minMonolithLevelForStudy is not";
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AE20 RID: 44576
		public int frequencyTicks = -1;

		// Token: 0x0400AE21 RID: 44577
		public float studyAmountToComplete = -1f;

		// Token: 0x0400AE22 RID: 44578
		public bool showToggleGizmo;

		// Token: 0x0400AE23 RID: 44579
		public bool studyEnabledByDefault = true;

		// Token: 0x0400AE24 RID: 44580
		public bool canBeActivityDeactivated;

		// Token: 0x0400AE25 RID: 44581
		internal float? anomalyKnowledge;

		// Token: 0x0400AE26 RID: 44582
		internal KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x0400AE27 RID: 44583
		public float knowledgeFactorOutdoors = 1f;

		// Token: 0x0400AE28 RID: 44584
		public int minMonolithLevelForStudy;

		// Token: 0x0400AE29 RID: 44585
		internal bool requiresHoldingPlatform;

		// Token: 0x0400AE2A RID: 44586
		internal bool requiresImprisonment;

		// Token: 0x0400AE2B RID: 44587
		[MustTranslate]
		public string completedLetterTitle;

		// Token: 0x0400AE2C RID: 44588
		[MustTranslate]
		public string completedLetterText;

		// Token: 0x0400AE2D RID: 44589
		[MustTranslate]
		public string completedMessage;

		// Token: 0x0400AE2E RID: 44590
		public LetterDef completedLetterDef;
	}
}
