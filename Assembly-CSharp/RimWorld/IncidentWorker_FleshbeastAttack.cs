using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9B RID: 7579
	public class IncidentWorker_FleshbeastAttack : IncidentWorker
	{
		// Token: 0x0600A98B RID: 43403 RVA: 0x0030864A File Offset: 0x0030684A
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms);
		}

		// Token: 0x0600A98C RID: 43404 RVA: 0x00308668 File Offset: 0x00306868
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = parms.points * 0.6f;
			List<Thing> list = new List<Thing>();
			int num2 = 0;
			while (num > 0f)
			{
				IntVec3 cell;
				if (!LargeBuildingCellFinder.TryFindCell(out cell, map, IncidentWorker_FleshbeastAttack.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow), null, null, false))
				{
					return false;
				}
				float num3 = Mathf.Min(num, 500f);
				Thing item = FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell, map, num3, IncidentWorker_FleshbeastAttack.PitBurrowEmergenceDelayRangeTicks, IncidentWorker_FleshbeastAttack.FleshbeastSpawnDelayTicks, true);
				list.Add(item);
				num -= num3;
				num2++;
				if (num2 > 100)
				{
					break;
				}
			}
			base.SendStandardLetter(this.def.letterLabel, (list.Count > 1) ? this.def.letterTextPlural : this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04007540 RID: 30016
		private const int MaxIterations = 100;

		// Token: 0x04007541 RID: 30017
		private const float FleshbeastPointsFactor = 0.6f;

		// Token: 0x04007542 RID: 30018
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x04007543 RID: 30019
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x04007544 RID: 30020
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false,
			attemptSpawnLocationType = SpawnLocationType.Outdoors
		};
	}
}
