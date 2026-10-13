using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4E RID: 6990
	public sealed class GameEnder : IExposable
	{
		// Token: 0x06009C1A RID: 39962 RVA: 0x002D4E80 File Offset: 0x002D3080
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.gameEnding, "gameEnding", false, false);
			Scribe_Values.Look<int>(ref this.ticksToGameOver, "ticksToGameOver", -1, false);
			Scribe_Values.Look<int>(ref this.newWanderersCreatedTick, "newWanderersCreatedTick", -99999, false);
		}

		// Token: 0x06009C1B RID: 39963 RVA: 0x002D4EBC File Offset: 0x002D30BC
		public void CheckOrUpdateGameOver()
		{
			if (Find.TickManager.TicksGame < 300)
			{
				return;
			}
			if (ShipCountdown.CountingDown)
			{
				this.gameEnding = false;
				return;
			}
			if (ModsConfig.OdysseyActive && WorldComponent_GravshipController.CutsceneInProgress)
			{
				this.gameEnding = false;
				return;
			}
			if (Find.CurrentGravship != null)
			{
				this.gameEnding = false;
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount >= 1)
				{
					this.gameEnding = false;
					return;
				}
			}
			for (int j = 0; j < maps.Count; j++)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = maps[j].mapPawns.AllPawnsSpawned;
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].carryTracker != null)
					{
						Pawn pawn = allPawnsSpawned[k].carryTracker.CarriedThing as Pawn;
						if (pawn != null && pawn.IsFreeColonist)
						{
							this.gameEnding = false;
							return;
						}
					}
				}
			}
			if (ModsConfig.AnomalyActive && DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal())
			{
				this.gameEnding = false;
				return;
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int l = 0; l < caravans.Count; l++)
			{
				if (this.IsPlayerControlledWithFreeColonist(caravans[l]))
				{
					this.gameEnding = false;
					return;
				}
			}
			List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
			for (int m = 0; m < travellingTransporters.Count; m++)
			{
				if (travellingTransporters[m].PodsHaveAnyFreeColonist)
				{
					this.gameEnding = false;
					return;
				}
			}
			if (QuestUtility.TotalBorrowedColonistCount() > 0)
			{
				return;
			}
			if (ModsConfig.AnomalyActive)
			{
				for (int n = 0; n < maps.Count; n++)
				{
					List<Pawn> allPawnsUnspawned = maps[n].mapPawns.AllPawnsUnspawned;
					for (int num = 0; num < allPawnsUnspawned.Count; num++)
					{
						Pawn pawn2 = allPawnsUnspawned[num];
						if (pawn2.IsColonist && pawn2.HostFaction == null && pawn2.ParentHolder is CompDevourer)
						{
							this.gameEnding = false;
							return;
						}
					}
				}
			}
			if (this.gameEnding)
			{
				return;
			}
			this.gameEnding = true;
			this.ticksToGameOver = 400;
		}

		// Token: 0x06009C1C RID: 39964 RVA: 0x002D50E4 File Offset: 0x002D32E4
		public bool CanSpawnNewWanderers()
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Dead)
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && (DeathRefusalUtility.HasPlayerControlledDeathRefusal(pawn) || DeathRefusalUtility.PlayerHasCorpseWithDeathRefusal()))
				{
					return false;
				}
			}
			return !ModsConfig.AnomalyActive || !Find.Anomaly.VoidAwakeningActive();
		}

		// Token: 0x06009C1D RID: 39965 RVA: 0x002D517C File Offset: 0x002D337C
		public void GameEndTick()
		{
			if (this.gameEnding)
			{
				this.ticksToGameOver--;
				if (this.ticksToGameOver == 0)
				{
					if (Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded) == null)
					{
						Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("GameOver".Translate(), "GameOverEveryoneDead".Translate(), LetterDefOf.GameEnded, null, null), null, 0, true);
					}
				}
				if (Mathf.Abs(this.ticksToGameOver) == 20000 && this.CanSpawnNewWanderers())
				{
					Letter letter3 = Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded);
					ChoiceLetter choiceLetter = LetterMaker.MakeLetter("GameOverCreateNewWanderers".Translate(), "GameOverCreateNewWanderersText".Translate(), LetterDefOf.GameEnded, null, null);
					Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
					Find.LetterStack.RemoveLetter(letter3);
					choiceLetter.arrivalTick = letter3.arrivalTick;
					return;
				}
			}
			else
			{
				Letter letter2 = Find.LetterStack.LettersListForReading.Find((Letter let) => let.def == LetterDefOf.GameEnded);
				if (letter2 != null)
				{
					Find.LetterStack.RemoveLetter(letter2);
				}
			}
		}

		// Token: 0x06009C1E RID: 39966 RVA: 0x002D52DC File Offset: 0x002D34DC
		private bool IsPlayerControlledWithFreeColonist(Caravan caravan)
		{
			if (!caravan.IsPlayerControlled)
			{
				return false;
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.IsColonist && pawn.HostFaction == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006C49 RID: 27721
		public bool gameEnding;

		// Token: 0x04006C4A RID: 27722
		private int ticksToGameOver = -1;

		// Token: 0x04006C4B RID: 27723
		public int newWanderersCreatedTick = -99999;

		// Token: 0x04006C4C RID: 27724
		private const int GameEndCountdownDuration = 400;

		// Token: 0x04006C4D RID: 27725
		public const int NewWanderersDelay = 20000;

		// Token: 0x04006C4E RID: 27726
		public const int NewWanderersRespiteDays = 5;
	}
}
