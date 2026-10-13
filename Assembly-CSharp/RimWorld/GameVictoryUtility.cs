using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F15 RID: 12053
	public static class GameVictoryUtility
	{
		// Token: 0x06011044 RID: 69700 RVA: 0x004E4F64 File Offset: 0x004E3164
		public static string MakeEndCredits(string intro, string ending, string escapees, string colonistsEscapeeTKey = "GameOverColonistsEscaped", IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(intro);
			if (!escapees.NullOrEmpty())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(colonistsEscapeeTKey.Translate(escapees));
			}
			stringBuilder.AppendLine();
			string text = GameVictoryUtility.PawnsLeftBehind(escaped);
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine("GameOverColonistsLeft".Translate(text));
			}
			stringBuilder.AppendLine(ending);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(GameVictoryUtility.InMemoryOfSection());
			return stringBuilder.ToString();
		}

		// Token: 0x06011045 RID: 69701 RVA: 0x004E5000 File Offset: 0x004E3200
		public static void ShowCredits(string victoryText, SongDef endCreditsSong, bool exitToMainMenu = false, float songStartDelay = 5f)
		{
			Screen_Credits screen_Credits = new Screen_Credits(victoryText, 500);
			screen_Credits.wonGame = true;
			screen_Credits.exitToMainMenu = exitToMainMenu;
			Find.WindowStack.Add(screen_Credits);
			if (endCreditsSong != null)
			{
				screen_Credits.endCreditsSong = endCreditsSong;
				screen_Credits.songStartDelay = songStartDelay;
				Find.MusicManagerPlay.ForceSilenceFor(999f);
			}
			ScreenFader.StartFade(Color.clear, 3f);
		}

		// Token: 0x06011046 RID: 69702 RVA: 0x004E5064 File Offset: 0x004E3264
		public static string PawnsLeftBehind(IList<Pawn> escaped = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (escaped == null || !escaped.Contains(pawn))
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (caravan.IsPlayerControlled)
				{
					List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
					for (int j = 0; j < pawnsListForReading.Count; j++)
					{
						Pawn pawn2 = pawnsListForReading[j];
						if (pawn2.IsColonist && pawn2.HostFaction == null && (escaped == null || !escaped.Contains(pawn2)))
						{
							stringBuilder.AppendLine("   " + pawn2.LabelCap);
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06011047 RID: 69703 RVA: 0x004E5184 File Offset: 0x004E3384
		public static string InMemoryOfSection()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from p in Find.WorldPawns.AllPawnsDead
			where p.IsColonist
			select p;
			if (enumerable.Any<Pawn>())
			{
				stringBuilder.AppendLine("GameOverInMemoryOf".Translate());
				foreach (Pawn pawn in enumerable)
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
