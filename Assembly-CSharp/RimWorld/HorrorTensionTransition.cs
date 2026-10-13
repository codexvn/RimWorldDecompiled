using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200290C RID: 10508
	public class HorrorTensionTransition : MusicTransition
	{
		// Token: 0x0600E5EA RID: 58858 RVA: 0x004314E4 File Offset: 0x0042F6E4
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
				if (map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness) || map.gameConditionManager.ConditionIsActive(GameConditionDefOf.DeathPall) || map.gameConditionManager.ConditionIsActive(GameConditionDefOf.BloodRain))
				{
					return true;
				}
			}
			foreach (Map map2 in Find.Maps)
			{
				if (map2.listerThings.ThingsInGroup(ThingRequestGroup.BuildingGroundSpawner).Any<Thing>())
				{
					return true;
				}
				using (IEnumerator<Pawn> enumerator2 = map2.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (HorrorTensionTransition.IsValidPawn(enumerator2.Current))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600E5EB RID: 58859 RVA: 0x00431620 File Offset: 0x0042F820
		private static bool IsValidPawn(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && (lord.LordJob is LordJob_PsychicRitual || lord.LordJob is LordJob_HateChant);
		}
	}
}
