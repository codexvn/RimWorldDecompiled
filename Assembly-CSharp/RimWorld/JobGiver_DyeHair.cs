using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001253 RID: 4691
	public class JobGiver_DyeHair : ThinkNode_JobGiver
	{
		// Token: 0x060072C8 RID: 29384 RVA: 0x002359B8 File Offset: 0x00233BB8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.style.nextHairColor != null)
			{
				Color? nextHairColor = pawn.style.nextHairColor;
				Color hairColor = pawn.story.HairColor;
				if (nextHairColor == null || (nextHairColor != null && !(nextHairColor.GetValueOrDefault() == hairColor)))
				{
					Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.StylingStation), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, false);
					if (thing == null)
					{
						return null;
					}
					Thing thing2 = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1, null, false), null, false);
					if (thing2 == null)
					{
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.DyeHair, thing, thing2);
					job.count = 1;
					return job;
				}
			}
			return null;
		}
	}
}
