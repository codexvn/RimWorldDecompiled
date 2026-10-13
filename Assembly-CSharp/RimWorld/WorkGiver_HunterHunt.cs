using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015E6 RID: 5606
	public class WorkGiver_HunterHunt : WorkGiver_Scanner
	{
		// Token: 0x0600844D RID: 33869 RVA: 0x0027A93D File Offset: 0x00278B3D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x0600844E RID: 33870 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600844F RID: 33871 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06008450 RID: 33872 RVA: 0x0027A94D File Offset: 0x00278B4D
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) || WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn) || !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt);
		}

		// Token: 0x06008451 RID: 33873 RVA: 0x0027A980 File Offset: 0x00278B80
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2.AnimalOrWildMan() && pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) != null && (!HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && this.CanFindHuntingPosition(pawn, pawn2);
		}

		// Token: 0x06008452 RID: 33874 RVA: 0x0027AA37 File Offset: 0x00278C37
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Hunt, t);
		}

		// Token: 0x06008453 RID: 33875 RVA: 0x0027AA4C File Offset: 0x00278C4C
		public static bool HasHuntingWeapon(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth() && !p.equipment.PrimaryEq.PrimaryVerb.UsesExplosiveProjectiles();
		}

		// Token: 0x06008454 RID: 33876 RVA: 0x0027AAB0 File Offset: 0x00278CB0
		private bool CanFindHuntingPosition(Pawn hunter, Pawn animal)
		{
			CastPositionRequest castPositionRequest = new CastPositionRequest
			{
				caster = hunter,
				target = animal,
				verb = hunter.TryGetAttackVerb(animal, false, false),
				wantCoverFromTarget = false
			};
			castPositionRequest.maxRangeFromTarget = (animal.Downed ? Mathf.Min(castPositionRequest.verb.EffectiveRange, (float)animal.RaceProps.executionRange) : Mathf.Max(castPositionRequest.verb.EffectiveRange * 0.95f, 1.42f));
			IntVec3 intVec;
			return CastPositionFinder.TryFindCastPosition(castPositionRequest, out intVec);
		}

		// Token: 0x06008455 RID: 33877 RVA: 0x0027AB40 File Offset: 0x00278D40
		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def.IsShieldThatBlocksRanged)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
