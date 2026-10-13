using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032A7 RID: 12967
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x17002F0C RID: 12044
		// (get) Token: 0x060123F0 RID: 74736 RVA: 0x0055D755 File Offset: 0x0055B955
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x060123F1 RID: 74737 RVA: 0x0055D9B3 File Offset: 0x0055BBB3
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x060123F2 RID: 74738 RVA: 0x0055D9D4 File Offset: 0x0055BBD4
		public override void PreOpen()
		{
			base.PreOpen();
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x060123F3 RID: 74739 RVA: 0x0055D9E4 File Offset: 0x0055BBE4
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficultyDef, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x060123F4 RID: 74740 RVA: 0x0055DA4C File Offset: 0x0055BC4C
		public override void PreClose()
		{
			foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
			where x.costListForDifficulty != null
			select x)
			{
				thingDef.costListForDifficulty.RecacheApplies();
			}
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
		}

		// Token: 0x0400BDDC RID: 48604
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
