using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029D2 RID: 10706
	public static class ArchonexusCountdown
	{
		// Token: 0x1700245F RID: 9311
		// (get) Token: 0x0600EBFB RID: 60411 RVA: 0x004484E3 File Offset: 0x004466E3
		public static bool CountdownActivated
		{
			get
			{
				return ArchonexusCountdown.timeLeft > 0f;
			}
		}

		// Token: 0x0600EBFC RID: 60412 RVA: 0x004484F1 File Offset: 0x004466F1
		public static void InitiateCountdown(Building_ArchonexusCore archonexusCore)
		{
			if (!ModLister.CheckIdeology("Archonexus victory countdown"))
			{
				return;
			}
			ArchonexusCountdown.archonexusCoreRoot = archonexusCore;
			ArchonexusCountdown.timeLeft = 6f;
			SoundDefOf.Archotech_Invoked.PlayOneShot(archonexusCore);
			ScreenFader.StartFade(Color.white, 6f);
		}

		// Token: 0x0600EBFD RID: 60413 RVA: 0x0044852F File Offset: 0x0044672F
		public static void CancelCountdown()
		{
			ArchonexusCountdown.timeLeft = -1f;
			ArchonexusCountdown.archonexusCoreRoot = null;
			ScreenFader.SetColor(Color.clear);
		}

		// Token: 0x0600EBFE RID: 60414 RVA: 0x0044854C File Offset: 0x0044674C
		private static void EndGame()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> list = (from p in ArchonexusCountdown.archonexusCoreRoot.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
			where p.RaceProps.Humanlike
			select p).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (!pawn.Dead && !pawn.IsQuestLodger())
				{
					stringBuilder.AppendLine("   " + pawn.LabelCap);
					Find.StoryWatcher.statsRecord.colonistsLaunched++;
				}
			}
			GameVictoryUtility.ShowCredits(GameVictoryUtility.MakeEndCredits("GameOverArchotechInvokedIntro".Translate(), "GameOverArchotechInvokedEnding".Translate(), stringBuilder.ToString(), "GameOverColonistsTranscended", list), SongDefOf.ArchonexusVictorySong, true, 2.5f);
		}

		// Token: 0x0600EBFF RID: 60415 RVA: 0x0044865C File Offset: 0x0044685C
		public static void ArchonexusCountdownUpdate()
		{
			if (ArchonexusCountdown.timeLeft > 0f)
			{
				ArchonexusCountdown.timeLeft -= Time.deltaTime;
				if (ArchonexusCountdown.timeLeft <= 0f)
				{
					ArchonexusCountdown.EndGame();
				}
			}
		}

		// Token: 0x04009C8B RID: 40075
		private const float ScreenFadeSeconds = 6f;

		// Token: 0x04009C8C RID: 40076
		private const float SongStartDelay = 2.5f;

		// Token: 0x04009C8D RID: 40077
		private static float timeLeft = -1f;

		// Token: 0x04009C8E RID: 40078
		private static Building_ArchonexusCore archonexusCoreRoot;
	}
}
