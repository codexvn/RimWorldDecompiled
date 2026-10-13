using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002F14 RID: 12052
	public static class ShipCountdown
	{
		// Token: 0x17002BEB RID: 11243
		// (get) Token: 0x0601103D RID: 69693 RVA: 0x004E4D03 File Offset: 0x004E2F03
		public static bool CountingDown
		{
			get
			{
				return ShipCountdown.timeLeft >= 0f;
			}
		}

		// Token: 0x0601103E RID: 69694 RVA: 0x004E4D14 File Offset: 0x004E2F14
		public static void InitiateCountdown(Building launchingShipRoot)
		{
			SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
			ShipCountdown.shipRoot = launchingShipRoot;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = null;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x0601103F RID: 69695 RVA: 0x004E4D46 File Offset: 0x004E2F46
		public static void InitiateCountdown(string launchString)
		{
			ShipCountdown.shipRoot = null;
			ShipCountdown.timeLeft = 7.2f;
			ShipCountdown.customLaunchString = launchString;
			ScreenFader.StartFade(Color.white, 7.2f);
		}

		// Token: 0x06011040 RID: 69696 RVA: 0x004E4D6D File Offset: 0x004E2F6D
		public static void ShipCountdownUpdate()
		{
			if (ShipCountdown.timeLeft > 0f)
			{
				ShipCountdown.timeLeft -= Time.deltaTime;
				if (ShipCountdown.timeLeft <= 0f)
				{
					ShipCountdown.CountdownEnded();
				}
			}
		}

		// Token: 0x06011041 RID: 69697 RVA: 0x004E4D9C File Offset: 0x004E2F9C
		public static void CancelCountdown()
		{
			ShipCountdown.timeLeft = -1000f;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x06011042 RID: 69698 RVA: 0x004E4DB4 File Offset: 0x004E2FB4
		private static void CountdownEnded()
		{
			if (ShipCountdown.shipRoot != null)
			{
				TaggedString taggedString = "GameOverShipLaunchedEnding".Translate();
				List<Building> list = ShipUtility.ShipBuildingsAttachedTo(ShipCountdown.shipRoot).ToList<Building>();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Building building in list)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						stringBuilder.AppendLine("   " + building_CryptosleepCasket.ContainedThing.LabelCap);
						Find.StoryWatcher.statsRecord.colonistsLaunched++;
						TaleRecorder.RecordTale(TaleDefOf.LaunchedShip, new object[]
						{
							building_CryptosleepCasket.ContainedThing
						});
					}
				}
				GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), taggedString, stringBuilder.ToString(), "GameOverColonistsEscaped", null), SongDefOf.EndCreditsSong, false, 5f);
				using (List<Building>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Building building2 = enumerator.Current;
						building2.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			if (!ShipCountdown.customLaunchString.NullOrEmpty())
			{
				GameVictoryUtility.ShowCredits(ShipCountdown.customLaunchString, SongDefOf.EndCreditsSong, false, 5f);
				return;
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverShipLaunchedIntro".Translate(), "GameOverShipLaunchedEnding".Translate(), null, "GameOverColonistsEscaped", null), SongDefOf.EndCreditsSong, false, 5f);
		}

		// Token: 0x0400B0D7 RID: 45271
		private static float timeLeft = -1000f;

		// Token: 0x0400B0D8 RID: 45272
		private static Building shipRoot;

		// Token: 0x0400B0D9 RID: 45273
		private static string customLaunchString;

		// Token: 0x0400B0DA RID: 45274
		private const float InitialTime = 7.2f;
	}
}
