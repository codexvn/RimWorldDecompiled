using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001417 RID: 5143
	public static class TrashUtility
	{
		// Token: 0x06007D2D RID: 32045 RVA: 0x0025B024 File Offset: 0x00259224
		public static bool ShouldTrashPlant(Pawn pawn, Plant p)
		{
			if (!p.sown || p.def.plant.IsTree || !p.FlammableNow || !TrashUtility.CanTrash(pawn, p))
			{
				return false;
			}
			foreach (IntVec3 c in CellRect.CenteredOn(p.Position, 2).ClipInsideMap(p.Map))
			{
				if (c.InBounds(p.Map) && c.ContainsStaticFire(p.Map))
				{
					return false;
				}
			}
			return p.Position.Roofed(p.Map) || p.Map.weatherManager.RainRate <= 0.25f;
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x0025B108 File Offset: 0x00259308
		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			if (!TrashUtility.ShouldTrashBuilding(b))
			{
				return false;
			}
			if (pawn.mindState.spawnedByInfestationThingComp && b.GetComp<CompCreatesInfestations>() != null)
			{
				return false;
			}
			if (((b.def.building.isInert || b.def.IsFrame) && !attackAllInert) || b.def.building.isTrap)
			{
				int num = GenLocalDate.HourOfDay(pawn) / 3;
				int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 73427324;
				if (!Rand.ChanceSeeded(0.008f, specialSeed))
				{
					return false;
				}
			}
			return TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b);
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x0025B1BC File Offset: 0x002593BC
		public static bool ShouldTrashBuilding(Building b)
		{
			if (((b != null) ? b.def.building : null) == null)
			{
				return false;
			}
			if (!b.def.useHitPoints || b.def.building.ai_neverTrashThis || b.IsClearableFreeBuilding)
			{
				return false;
			}
			if (b.def.building.isTrap)
			{
				return false;
			}
			CompCanBeDormant comp = b.GetComp<CompCanBeDormant>();
			return (comp == null || comp.Awake) && (b.Faction == null || b.Faction != Faction.OfMechanoids);
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x0025B247 File Offset: 0x00259447
		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && !t.IsBurning();
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x0025B268 File Offset: 0x00259468
		public static Job TrashJob(Pawn pawn, Thing t, bool allowPunchingInert = false, bool killIncappedTarget = false)
		{
			if (t is Plant && t.FlammableNow)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Ignite, t);
				TrashUtility.FinalizeTrashJob(job);
				return job;
			}
			if (pawn.equipment != null && Rand.Value < 0.7f)
			{
				foreach (Verb verb in pawn.equipment.AllEquipmentVerbs)
				{
					if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, t);
						job2.verbToUse = verb;
						TrashUtility.FinalizeTrashJob(job2);
						return job2;
					}
				}
			}
			Job job3;
			if (Rand.Value < 0.35f && pawn.natives.IgniteVerb != null && pawn.natives.IgniteVerb.IsStillUsableBy(pawn) && t.FlammableNow && !t.IsBurning() && !(t is Building_Door))
			{
				job3 = JobMaker.MakeJob(JobDefOf.Ignite, t);
			}
			else
			{
				Building building = t as Building;
				if (building != null && building.def.building.isInert && !allowPunchingInert)
				{
					return null;
				}
				job3 = JobMaker.MakeJob(JobDefOf.AttackMelee, t);
			}
			job3.killIncappedTarget = killIncappedTarget;
			TrashUtility.FinalizeTrashJob(job3);
			return job3;
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x0025B3C4 File Offset: 0x002595C4
		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x0400543A RID: 21562
		private const float ChanceHateInertBuilding = 0.008f;

		// Token: 0x0400543B RID: 21563
		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);
	}
}
