using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002909 RID: 10505
	public class HorrorCombatTransition : MusicTransition
	{
		// Token: 0x0600E5DF RID: 58847 RVA: 0x004311C8 File Offset: 0x0042F3C8
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (this.IsValidMap(map))
				{
					foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
					{
						if (this.IsValidPawn(pawn))
						{
							return true;
						}
						if (this.IsValidEntity(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600E5E0 RID: 58848 RVA: 0x0043128C File Offset: 0x0042F48C
		private bool IsValidMap(Map map)
		{
			return map.mapPawns.ColonistsSpawnedCount > 0;
		}

		// Token: 0x0600E5E1 RID: 58849 RVA: 0x0043129C File Offset: 0x0042F49C
		private bool IsValidPawn(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && (lord.LordJob is LordJob_PsychicRitual || lord.LordJob is LordJob_HateChant);
		}

		// Token: 0x0600E5E2 RID: 58850 RVA: 0x004312D4 File Offset: 0x0042F4D4
		private bool IsValidEntity(Pawn pawn)
		{
			return (pawn.RaceProps.IsAnomalyEntity || pawn.IsSubhuman) && !pawn.Downed && !pawn.IsOnHoldingPlatform && !pawn.IsPsychologicallyInvisible() && !pawn.Fogged() && pawn.HostileTo(Faction.OfPlayer);
		}
	}
}
