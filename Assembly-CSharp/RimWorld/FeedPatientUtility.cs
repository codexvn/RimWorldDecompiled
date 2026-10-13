using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C4 RID: 5572
	public static class FeedPatientUtility
	{
		// Token: 0x060083B8 RID: 33720 RVA: 0x002782A4 File Offset: 0x002764A4
		public static bool ShouldBeFed(Pawn p)
		{
			if (p.GetPosture() == PawnPosture.Standing)
			{
				return false;
			}
			if (p.NonHumanlikeOrWildMan())
			{
				Building_Bed building_Bed = p.CurrentBed();
				if (building_Bed == null || building_Bed.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (!p.InBed())
				{
					return false;
				}
			}
			if (p.RaceProps.EatsFood)
			{
				Pawn_NeedsTracker needs = p.needs;
				if (((needs != null) ? needs.food : null) != null)
				{
					if (p.Spawned && p.ShouldBeSlaughtered())
					{
						return false;
					}
					if (!HealthAIUtility.ShouldSeekMedicalRest(p))
					{
						return false;
					}
					if (p.HostFaction != null)
					{
						if (p.HostFaction != Faction.OfPlayer)
						{
							return false;
						}
						if (p.guest != null && !p.guest.CanBeBroughtFood)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060083B9 RID: 33721 RVA: 0x00278370 File Offset: 0x00276570
		public static bool IsHungry(Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.CurLevelPercentage <= p.needs.food.PercentageThreshHungry + 0.02f;
		}
	}
}
