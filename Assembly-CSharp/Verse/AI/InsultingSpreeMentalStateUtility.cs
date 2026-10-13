using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F58 RID: 3928
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x0600612C RID: 24876 RVA: 0x001EC4CC File Offset: 0x001EA6CC
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && SocialInteractionUtility.CanReceiveInteraction(insulted, null) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x0600612D RID: 24877 RVA: 0x001EC598 File Offset: 0x001EA798
		public static void GetInsultCandidatesFor(Pawn bully, List<Pawn> outCandidates, bool allowPrisoners = true)
		{
			outCandidates.Clear();
			Region region = bully.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			TraverseParms traverseParams = TraverseParms.For(bully, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = (Pawn)list[i];
					if (InsultingSpreeMentalStateUtility.CanChaseAndInsult(bully, pawn, true, allowPrisoners))
					{
						outCandidates.Add(pawn);
					}
				}
				return false;
			}, 40, RegionType.Set_Passable);
		}

		// Token: 0x040045F9 RID: 17913
		private const int MaxRegionsToSearch = 40;

		// Token: 0x040045FA RID: 17914
		public const int MaxDistance = 40;

		// Token: 0x040045FB RID: 17915
		public const int MinTicksBetweenInsults = 1200;
	}
}
