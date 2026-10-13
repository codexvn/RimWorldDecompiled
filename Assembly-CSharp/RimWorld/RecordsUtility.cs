using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BA RID: 9146
	public static class RecordsUtility
	{
		// Token: 0x0600C691 RID: 50833 RVA: 0x0039DEA8 File Offset: 0x0039C0A8
		public static void Notify_PawnKilled(Pawn killed, Pawn killer)
		{
			killer.records.Increment(RecordDefOf.Kills);
			RaceProperties raceProps = killed.RaceProps;
			if (ModsConfig.AnomalyActive && (killed.IsSubhuman || raceProps.IsAnomalyEntity))
			{
				killer.records.Increment(RecordDefOf.KillsEntities);
			}
			else if (raceProps.Humanlike)
			{
				killer.records.Increment(RecordDefOf.KillsHumanlikes);
			}
			if (raceProps.Animal)
			{
				killer.records.Increment(RecordDefOf.KillsAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				killer.records.Increment(RecordDefOf.KillsMechanoids);
			}
		}

		// Token: 0x0600C692 RID: 50834 RVA: 0x0039DF40 File Offset: 0x0039C140
		public static void Notify_PawnDowned(Pawn downed, Pawn instigator)
		{
			instigator.records.Increment(RecordDefOf.PawnsDowned);
			RaceProperties raceProps = downed.RaceProps;
			if (ModsConfig.AnomalyActive && (downed.IsSubhuman || raceProps.IsAnomalyEntity))
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedEntities);
			}
			else if (raceProps.Humanlike)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedHumanlikes);
			}
			if (raceProps.Animal)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedMechanoids);
			}
		}

		// Token: 0x0600C693 RID: 50835 RVA: 0x0039DFD8 File Offset: 0x0039C1D8
		public static void Notify_BillDone(Pawn billDoer, List<Thing> products)
		{
			for (int i = 0; i < products.Count; i++)
			{
				for (int j = 0; j < products[i].stackCount; j++)
				{
					if (products[i].def.IsNutritionGivingIngestible && products[i].def.ingestible.preferability >= FoodPreferability.MealAwful)
					{
						billDoer.records.Increment(RecordDefOf.MealsCooked);
					}
					else if (RecordsUtility.ShouldIncrementThingsCrafted(products[i]))
					{
						billDoer.records.Increment(RecordDefOf.ThingsCrafted);
					}
				}
			}
		}

		// Token: 0x0600C694 RID: 50836 RVA: 0x0039E06C File Offset: 0x0039C26C
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
