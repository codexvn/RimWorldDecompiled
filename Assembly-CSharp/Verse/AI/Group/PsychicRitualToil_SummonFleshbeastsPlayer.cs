using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001087 RID: 4231
	public class PsychicRitualToil_SummonFleshbeastsPlayer : PsychicRitualToil
	{
		// Token: 0x06006789 RID: 26505 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SummonFleshbeastsPlayer()
		{
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x0020CD09 File Offset: 0x0020AF09
		public PsychicRitualToil_SummonFleshbeastsPlayer(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0020CD18 File Offset: 0x0020AF18
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn == null)
			{
				return;
			}
			IntVec3 intVec = PsychicRitualToil_SummonFleshbeastsPlayer.FindSpawnCenter(psychicRitual, pawn);
			this.SpawnFleshbeasts(psychicRitual, intVec, pawn);
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "SummonFleshbeastsPlayerCompleteText".Translate(pawn, psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, new TargetInfo(intVec, psychicRitual.Map, false), null, null, null, null, 0, true);
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x0020CDB0 File Offset: 0x0020AFB0
		private static IntVec3 FindSpawnCenter(PsychicRitual psychicRitual, Pawn invoker)
		{
			IntVec3 intVec = IntVec3.Invalid;
			List<Pawn> list = (from t in psychicRitual.Map.attackTargetsCache.TargetsHostileToColony.Where(delegate(IAttackTarget t)
			{
				if (!t.Thing.Fogged())
				{
					Pawn pawn = t.Thing as Pawn;
					if (pawn != null && !t.ThreatDisabled(invoker))
					{
						return !pawn.IsOnHoldingPlatform;
					}
				}
				return false;
			})
			select t.Thing as Pawn).ToList<Pawn>();
			IEnumerable<IAttackTarget> source = from h in list
			where h.RaceProps.Humanlike && !h.IsSubhuman
			select h;
			if (!source.Any<IAttackTarget>())
			{
				source = from h in list
				where !h.IsSubhuman && !h.RaceProps.IsAnomalyEntity
				select h;
				if (!source.Any<IAttackTarget>())
				{
					source = list;
				}
			}
			IAttackTarget[] array = source.ToArray<IAttackTarget>();
			array.Shuffle<IAttackTarget>();
			intVec = PsychicRitualToil_SummonFleshbeastsPlayer.TryFindingValidHostileSpawnCenter(psychicRitual, array);
			if (intVec != IntVec3.Invalid)
			{
				return intVec;
			}
			IntVec3 result;
			CellFinder.TryFindRandomCell(psychicRitual.Map, (IntVec3 cell) => cell.Walkable(psychicRitual.Map) && !cell.Fogged(psychicRitual.Map) && !psychicRitual.Map.thingGrid.ThingsListAtFast(cell).Any<Thing>(), out result);
			return result;
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x0020CED4 File Offset: 0x0020B0D4
		private static IntVec3 TryFindingValidHostileSpawnCenter(PsychicRitual psychicRitual, IAttackTarget[] hostiles)
		{
			int num = 0;
			while (num < hostiles.Length && num <= 4)
			{
				int num2 = 0;
				IntVec3 positionHeld = hostiles[num].Thing.PositionHeld;
				Map map = psychicRitual.Map;
				CellRect cellRect = CellRect.CenteredOn(positionHeld, 10);
				cellRect.ClipInsideMap(map);
				foreach (IntVec3 intVec in cellRect)
				{
					if (intVec != positionHeld && intVec.Walkable(map) && !intVec.Fogged(map))
					{
						num2++;
					}
				}
				if (num2 >= 5)
				{
					return positionHeld;
				}
				num++;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x0020CF90 File Offset: 0x0020B190
		private void SpawnFleshbeasts(PsychicRitual psychicRitual, IntVec3 spawnCenter, Pawn invoker)
		{
			float num = ((PsychicRitualDef_SummonFleshbeastsPlayer)psychicRitual.def).fleshbeastCombatPointsFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			int num2 = 0;
			while (num > 0f)
			{
				IntVec3 intVec = IntVec3.Invalid;
				IntVec3 intVec2;
				if (LargeBuildingCellFinder.TryFindCellNear(spawnCenter, psychicRitual.Map, 10, PsychicRitualToil_SummonFleshbeastsPlayer.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow), out intVec2, false) && intVec2 != spawnCenter)
				{
					intVec = intVec2;
				}
				if (intVec != IntVec3.Invalid)
				{
					float num3 = Mathf.Min(num, 500f);
					num -= num3;
					FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(intVec, psychicRitual.Map, num3, PsychicRitualToil_SummonFleshbeastsPlayer.PitBurrowEmergenceDelayRangeTicks, PsychicRitualToil_SummonFleshbeastsPlayer.FleshbeastSpawnDelayTicks, false);
				}
				num2++;
				if (num2 > 100)
				{
					break;
				}
			}
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x0020D041 File Offset: 0x0020B241
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049C5 RID: 18885
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x040049C6 RID: 18886
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x040049C7 RID: 18887
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x040049C8 RID: 18888
		private const int MaxIterations = 100;

		// Token: 0x040049C9 RID: 18889
		private const int SpawnRadius = 10;

		// Token: 0x040049CA RID: 18890
		private const int MinWalkableCells = 5;

		// Token: 0x040049CB RID: 18891
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
