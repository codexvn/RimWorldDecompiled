using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032A5 RID: 12965
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x17002F0B RID: 12043
		// (get) Token: 0x060123E7 RID: 74727 RVA: 0x0055D755 File Offset: 0x0055B955
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x060123E8 RID: 74728 RVA: 0x0055D768 File Offset: 0x0055B968
		public override void PreOpen()
		{
			base.PreOpen();
			if (this.storyteller == null)
			{
				this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
				where d.listVisible
				orderby d.listOrder
				select d).First<StorytellerDef>();
			}
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x060123E9 RID: 74729 RVA: 0x0055D7E0 File Offset: 0x0055B9E0
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			StorytellerUI.DrawStorytellerSelectionInterface(base.GetMainRect(rect, 0f, false), ref this.storyteller, ref this.difficulty, ref this.difficultyValues, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true, true);
			Rect rect2 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060123EA RID: 74730 RVA: 0x0055D890 File Offset: 0x0055BA90
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.difficulty == null)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				this.difficulty = DifficultyDefOf.Rough;
				this.difficultyValues = new Difficulty(this.difficulty);
			}
			if (!Find.GameInitData.permadeathChosen)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Reload anytime mode has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				Find.GameInitData.permadeathChosen = true;
				Find.GameInitData.permadeath = false;
			}
			Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty, this.difficultyValues);
			return true;
		}

		// Token: 0x0400BDD5 RID: 48597
		private StorytellerDef storyteller;

		// Token: 0x0400BDD6 RID: 48598
		private DifficultyDef difficulty;

		// Token: 0x0400BDD7 RID: 48599
		private Difficulty difficultyValues = new Difficulty();

		// Token: 0x0400BDD8 RID: 48600
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
