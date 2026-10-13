using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022C3 RID: 8899
	public class LifeStageWorker_HumanlikeAdult : LifeStageWorker
	{
		// Token: 0x0600C197 RID: 49559 RVA: 0x00380E10 File Offset: 0x0037F010
		public override void Notify_LifeStageStarted(Pawn pawn, LifeStageDef previousLifeStage)
		{
			base.Notify_LifeStageStarted(pawn, previousLifeStage);
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (pawn.Spawned && previousLifeStage != null && previousLifeStage.developmentalStage.Juvenile())
			{
				EffecterDefOf.Birthday.SpawnAttached(pawn, pawn.Map, 1f);
			}
			if (pawn.story.bodyType == BodyTypeDefOf.Child || pawn.story.bodyType == BodyTypeDefOf.Baby)
			{
				Pawn_ApparelTracker apparel2 = pawn.apparel;
				if (apparel2 != null)
				{
					apparel2.DropAllOrMoveAllToInventory((Apparel apparel) => !apparel.def.apparel.developmentalStageFilter.Has(DevelopmentalStage.Adult));
				}
				BodyTypeDef bodyTypeFor = PawnGenerator.GetBodyTypeFor(pawn);
				pawn.story.bodyType = bodyTypeFor;
				pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (pawn.IsColonist)
			{
				List<BackstoryCategoryFilter> list = (Faction.OfPlayer.def == FactionDefOf.PlayerTribe) ? LifeStageWorker_HumanlikeAdult.BackstoryFiltersTribal : LifeStageWorker_HumanlikeAdult.BackstoryFiltersColonist;
				if (previousLifeStage.developmentalStage.Juvenile())
				{
					if (pawn.ageTracker.vatGrowTicks >= 1200000L)
					{
						BackstorySlot slot = BackstorySlot.Childhood;
						List<BackstoryCategoryFilter> vatgrowBackstoryFilter = LifeStageWorker_HumanlikeAdult.VatgrowBackstoryFilter;
						Faction faction = pawn.Faction;
						PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, slot, vatgrowBackstoryFilter, (faction != null) ? faction.def : null, null);
					}
					else
					{
						BackstoryDef backstory = pawn.story.GetBackstory(BackstorySlot.Childhood);
						if (backstory != null && backstory.IsPlayerColonyChildBackstory)
						{
							BackstorySlot slot2 = BackstorySlot.Childhood;
							List<BackstoryCategoryFilter> backstoryCategories = list;
							Faction faction2 = pawn.Faction;
							PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, slot2, backstoryCategories, (faction2 != null) ? faction2.def : null, null);
						}
					}
				}
				if (pawn.story.GetBackstory(BackstorySlot.Adulthood) == null)
				{
					BackstorySlot slot3 = BackstorySlot.Adulthood;
					List<BackstoryCategoryFilter> backstoryCategories2 = list;
					Faction faction3 = pawn.Faction;
					PawnBioAndNameGenerator.FillBackstorySlotShuffled(pawn, slot3, backstoryCategories2, (faction3 != null) ? faction3.def : null, null);
				}
			}
		}

		// Token: 0x04008445 RID: 33861
		public const int VatGrowBackstoryTicks = 1200000;

		// Token: 0x04008446 RID: 33862
		private static readonly List<BackstoryCategoryFilter> VatgrowBackstoryFilter = new List<BackstoryCategoryFilter>
		{
			new BackstoryCategoryFilter
			{
				categories = new List<string>
				{
					"VatGrown"
				}
			}
		};

		// Token: 0x04008447 RID: 33863
		private static readonly List<BackstoryCategoryFilter> BackstoryFiltersTribal = new List<BackstoryCategoryFilter>
		{
			new BackstoryCategoryFilter
			{
				categories = new List<string>
				{
					"AdultTribal"
				}
			}
		};

		// Token: 0x04008448 RID: 33864
		private static readonly List<BackstoryCategoryFilter> BackstoryFiltersColonist = new List<BackstoryCategoryFilter>
		{
			new BackstoryCategoryFilter
			{
				categories = new List<string>
				{
					"AdultColonist"
				}
			}
		};
	}
}
