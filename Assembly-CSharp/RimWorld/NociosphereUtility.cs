using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002322 RID: 8994
	public static class NociosphereUtility
	{
		// Token: 0x0600C33F RID: 49983 RVA: 0x00389CC0 File Offset: 0x00387EC0
		public static Thing FindTarget(Pawn pawn)
		{
			List<Thing> source = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn);
			NociosphereUtility.CheckForTargets(pawn, source, NociosphereUtility.targetsTmp, new Func<Thing, bool>(NociosphereUtility.IsPawnTarget));
			if (NociosphereUtility.targetsTmp.Empty<Thing>())
			{
				List<Thing> source2 = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				NociosphereUtility.CheckForTargets(pawn, source2, NociosphereUtility.targetsTmp, new Func<Thing, bool>(NociosphereUtility.IsBuildingTarget));
			}
			Thing result = null;
			if (NociosphereUtility.targetsTmp.Any<Thing>())
			{
				result = NociosphereUtility.targetsTmp.RandomElement<Thing>();
			}
			NociosphereUtility.targetsTmp.Clear();
			return result;
		}

		// Token: 0x0600C340 RID: 49984 RVA: 0x00389D54 File Offset: 0x00387F54
		public static void SkipTo(Pawn pawn, IntVec3 cell)
		{
			Ability ability = pawn.abilities.GetAbility(AbilityDefOf.EntitySkip, false);
			ability.ResetCooldown();
			if (pawn.IsOnHoldingPlatform)
			{
				Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)pawn.ParentHolder;
				Thing thing;
				building_HoldingPlatform.innerContainer.TryDrop(pawn, building_HoldingPlatform.Position, building_HoldingPlatform.Map, ThingPlaceMode.Direct, 1, out thing, null, null);
				CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.targetHolder = null;
				}
			}
			Job job = ability.GetJob(pawn, cell);
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x0600C341 RID: 49985 RVA: 0x00389DFC File Offset: 0x00387FFC
		private static void CheckForTargets(Pawn pawn, List<Thing> source, List<Thing> output, Func<Thing, bool> validator)
		{
			output.Clear();
			for (int i = 0; i < source.Count; i++)
			{
				if (pawn.Position.DistanceToSquared(source[i].Position) <= NociosphereUtility.NociosphereLOSSqr && validator(source[i]) && GenSight.LineOfSightToThing(pawn.Position, source[i], pawn.Map, true, null))
				{
					output.Add(source[i]);
				}
			}
		}

		// Token: 0x0600C342 RID: 49986 RVA: 0x00389E78 File Offset: 0x00388078
		private static bool IsPawnTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.Downed && pawn.kindDef != PawnKindDefOf.Nociosphere;
		}

		// Token: 0x0600C343 RID: 49987 RVA: 0x00389EAC File Offset: 0x003880AC
		private static bool IsBuildingTarget(Thing thing)
		{
			Building building = thing as Building;
			return building != null && building.def.building.IsTurret;
		}

		// Token: 0x04008525 RID: 34085
		public const float NociosphereLOS = 24.9f;

		// Token: 0x04008526 RID: 34086
		public static readonly int NociosphereLOSSqr = Mathf.FloorToInt(620.01f);

		// Token: 0x04008527 RID: 34087
		private static readonly List<Thing> targetsTmp = new List<Thing>();
	}
}
