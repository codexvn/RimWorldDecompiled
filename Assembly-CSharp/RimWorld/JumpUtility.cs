using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003781 RID: 14209
	public static class JumpUtility
	{
		// Token: 0x06013AF6 RID: 80630 RVA: 0x005D8F28 File Offset: 0x005D7128
		public static bool DoJump(Pawn pawn, LocalTargetInfo currentTarget, CompApparelReloadable comp, VerbProperties verbProps, Ability triggeringAbility = null, LocalTargetInfo target = default(LocalTargetInfo), ThingDef pawnFlyerOverride = null)
		{
			string text;
			if (comp != null && !comp.CanBeUsed(out text))
			{
				return false;
			}
			if (comp != null)
			{
				comp.UsedOnce();
			}
			IntVec3 position = pawn.Position;
			IntVec3 cell = currentTarget.Cell;
			Map map = pawn.Map;
			bool flag = Find.Selector.IsSelected(pawn);
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(pawnFlyerOverride ?? ThingDefOf.PawnFlyer, pawn, cell, verbProps.flightEffecterDef, verbProps.soundLanding, verbProps.flyWithCarriedThing, null, triggeringAbility, target);
			if (pawnFlyer != null)
			{
				FleckMaker.ThrowDustPuff(position.ToVector3Shifted() + Gen.RandomHorizontalVector(0.5f), map, 2f);
				GenSpawn.Spawn(pawnFlyer, cell, map, WipeMode.Vanish);
				if (flag)
				{
					Find.Selector.Select(pawn, false, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06013AF7 RID: 80631 RVA: 0x005D8FF0 File Offset: 0x005D71F0
		public static void OrderJump(Pawn pawn, LocalTargetInfo target, Verb verb, float range)
		{
			Map map = pawn.Map;
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(target.Cell, pawn, (IntVec3 c) => JumpUtility.ValidJumpTarget(pawn, map, c) && JumpUtility.CanHitTargetFrom(pawn, pawn.Position, c, range), false);
			Job job = JobMaker.MakeJob(JobDefOf.CastJump, intVec);
			job.verbToUse = verb;
			if (pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false))
			{
				FleckMaker.Static(intVec, map, FleckDefOf.FeedbackGoto, 1f);
			}
		}

		// Token: 0x06013AF8 RID: 80632 RVA: 0x005D908C File Offset: 0x005D728C
		public static bool CanHitTargetFrom(Pawn pawn, IntVec3 root, LocalTargetInfo targ, float range)
		{
			float num = range * range;
			IntVec3 cell = targ.Cell;
			return (float)pawn.Position.DistanceToSquared(cell) <= num && GenSight.LineOfSight(root, cell, pawn.Map);
		}

		// Token: 0x06013AF9 RID: 80633 RVA: 0x005D90C4 File Offset: 0x005D72C4
		public static bool ValidJumpTarget(Thing flying, Map map, IntVec3 cell)
		{
			if (!cell.IsValid || !cell.InBounds(map))
			{
				return false;
			}
			if (cell.Impassable(map) || cell.Fogged(map))
			{
				return false;
			}
			Pawn pawn = flying as Pawn;
			if (pawn != null)
			{
				if (!cell.WalkableBy(map, pawn))
				{
					return false;
				}
			}
			else if (!cell.WalkableByAny(map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			if (edifice != null)
			{
				Building_Door building_Door = edifice as Building_Door;
				if (building_Door != null && !building_Door.Open)
				{
					return false;
				}
			}
			return true;
		}
	}
}
