using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020022C5 RID: 8901
	public class LifeStageWorker_HumanlikeChild : LifeStageWorker
	{
		// Token: 0x0600C19D RID: 49565 RVA: 0x00381074 File Offset: 0x0037F274
		public override void Notify_LifeStageStarted(Pawn pawn, LifeStageDef previousLifeStage)
		{
			base.Notify_LifeStageStarted(pawn, previousLifeStage);
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (previousLifeStage == null || !previousLifeStage.developmentalStage.Baby())
			{
				return;
			}
			if (pawn.story.bodyType != BodyTypeDefOf.Child)
			{
				Pawn_ApparelTracker apparel2 = pawn.apparel;
				if (apparel2 != null)
				{
					apparel2.DropAllOrMoveAllToInventory((Apparel apparel) => !apparel.def.apparel.developmentalStageFilter.Has(DevelopmentalStage.Child));
				}
				BodyTypeDef bodyTypeFor = PawnGenerator.GetBodyTypeFor(pawn);
				pawn.story.bodyType = bodyTypeFor;
				pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (ModsConfig.IdeologyActive && pawn.Faction != null)
			{
				Pawn pawn2 = pawn.GetMother();
				if (((pawn2 != null) ? pawn2.Faction : null) != pawn.Faction)
				{
					Pawn father = pawn.GetFather();
					if (father != null && father.Faction == pawn.Faction)
					{
						pawn2 = father;
					}
					else
					{
						pawn2 = null;
					}
				}
				if (pawn2 != null && pawn2.IsSlave)
				{
					pawn.guest.SetGuestStatus(pawn.Faction, GuestStatus.Slave);
				}
			}
			bool flag = pawn.Ideo == null && pawn.ideo.TryJoinIdeoFromExposures();
			if (!ModsConfig.IdeologyActive)
			{
				flag = false;
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				List<WorkTypeDef> list = new List<WorkTypeDef>();
				List<LifeStageWorkSettings> lifeStageWorkSettings = pawn.RaceProps.lifeStageWorkSettings;
				for (int i = 0; i < lifeStageWorkSettings.Count; i++)
				{
					if (lifeStageWorkSettings[i].minAge <= pawn.ageTracker.AgeBiologicalYears)
					{
						list.Add(lifeStageWorkSettings[i].workType);
					}
				}
				TaggedString taggedString = "LetterBecameChild".Translate(pawn) + "\n\n" + (from wt in list
				select wt.labelShort.CapitalizeFirst()).ToLineList(" - ", false) + "\n\n" + "LetterBecameChildChanges".Translate();
				if (ModsConfig.IdeologyActive)
				{
					taggedString += "\n\n" + ((flag && !Find.IdeoManager.classicMode) ? ("LetterChildFollowIdeo".Translate(pawn, pawn.Ideo) + "\n\n") : TaggedString.Empty) + "LetterChildLegalStatus".Translate(pawn);
					ChoiceLetter_BabyToChild choiceLetter_BabyToChild = (ChoiceLetter_BabyToChild)LetterMaker.MakeLetter("LetterLabelBecameChild".Translate(pawn), taggedString, LetterDefOf.BabyToChild, pawn, null, null, null);
					choiceLetter_BabyToChild.Start();
					Find.LetterStack.ReceiveLetter(choiceLetter_BabyToChild, null, 0, true);
				}
				else
				{
					ChoiceLetter let = LetterMaker.MakeLetter("LetterLabelBecameChild".Translate(pawn), taggedString, LetterDefOf.PositiveEvent, pawn, null, null, null);
					Find.LetterStack.ReceiveLetter(let, null, 0, true);
				}
				if (pawn.Spawned)
				{
					EffecterDefOf.Birthday.SpawnAttached(pawn, pawn.Map, 1f);
				}
			}
			List<BackstoryCategoryFilter> list2 = LifeStageWorker_HumanlikeChild.ChildBackstoryFilters;
			if (pawn.IsColonist && Faction.OfPlayer.def == FactionDefOf.PlayerTribe)
			{
				list2 = LifeStageWorker_HumanlikeChild.ChildTribalBackstoryFilters;
			}
			BackstorySlot slot = BackstorySlot.Childhood;
			List<BackstoryCategoryFilter> backstoryCategories = list2;
			Faction faction = pawn.Faction;
			PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, slot, backstoryCategories, (faction != null) ? faction.def : null, null);
			pawn.Notify_DisabledWorkTypesChanged();
		}

		// Token: 0x0400844B RID: 33867
		private static readonly List<BackstoryCategoryFilter> ChildBackstoryFilters = new List<BackstoryCategoryFilter>
		{
			new BackstoryCategoryFilter
			{
				categories = new List<string>
				{
					"Child"
				}
			}
		};

		// Token: 0x0400844C RID: 33868
		public static readonly List<BackstoryCategoryFilter> ChildTribalBackstoryFilters = new List<BackstoryCategoryFilter>
		{
			new BackstoryCategoryFilter
			{
				categories = new List<string>
				{
					"ChildTribal"
				}
			}
		};
	}
}
