using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200384E RID: 14414
	public static class HealthAIUtility
	{
		// Token: 0x06013EF6 RID: 81654 RVA: 0x005EA229 File Offset: 0x005E8429
		public static bool ShouldSeekMedicalRestUrgent(Pawn pawn)
		{
			return (pawn.Downed && !LifeStageUtility.AlwaysDowned(pawn)) || pawn.health.HasHediffsNeedingTend(false) || HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) || pawn.health.hediffSet.InLabor(true);
		}

		// Token: 0x06013EF7 RID: 81655 RVA: 0x005EA264 File Offset: 0x005E8464
		public static bool ShouldSeekMedicalRest(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || pawn.health.hediffSet.HasTendedAndHealingInjury() || pawn.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x06013EF8 RID: 81656 RVA: 0x005EA292 File Offset: 0x005E8492
		public static bool ShouldBeTendedNowByPlayerUrgent(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowByPlayer(pawn) && HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 45000;
		}

		// Token: 0x06013EF9 RID: 81657 RVA: 0x005EA2AB File Offset: 0x005E84AB
		public static bool ShouldBeTendedNowByPlayer(Pawn pawn)
		{
			return pawn.playerSettings != null && HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(pawn) && pawn.health.HasHediffsNeedingTendByPlayer(false);
		}

		// Token: 0x06013EFA RID: 81658 RVA: 0x005EA2D0 File Offset: 0x005E84D0
		public static bool ShouldEverReceiveMedicalCareFromPlayer(Pawn pawn)
		{
			Pawn_PlayerSettings playerSettings = pawn.playerSettings;
			return (playerSettings == null || playerSettings.medCare != MedicalCareCategory.NoCare) && (pawn.guest == null || !pawn.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Execution)) && !pawn.ShouldBeSlaughtered();
		}

		// Token: 0x06013EFB RID: 81659 RVA: 0x005EA31D File Offset: 0x005E851D
		public static bool ShouldHaveSurgeryDoneNow(Pawn pawn)
		{
			return pawn.health.surgeryBills.AnyShouldDoNow && WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(pawn);
		}

		// Token: 0x06013EFC RID: 81660 RVA: 0x005EA33C File Offset: 0x005E853C
		public static bool WantsToBeRescued(Pawn pawn)
		{
			if (!pawn.Downed)
			{
				return false;
			}
			if (pawn.InBed() || pawn.IsCharging())
			{
				return false;
			}
			if (pawn.IsDeactivated())
			{
				return false;
			}
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(pawn))
			{
				return false;
			}
			if (pawn.IsMutant && !pawn.mutant.Def.entitledToMedicalCare)
			{
				return false;
			}
			if (pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return false;
			}
			if (pawn.ShouldBeSlaughtered())
			{
				return false;
			}
			Lord lord;
			if (pawn.TryGetLord(out lord))
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				RitualRole ritualRole;
				if (lordJob_Ritual != null && lordJob_Ritual.TryGetRoleFor(pawn, out ritualRole, false) && ritualRole.allowDowned)
				{
					return false;
				}
			}
			return !LifeStageUtility.AlwaysDowned(pawn) || HealthAIUtility.ShouldSeekMedicalRest(pawn);
		}

		// Token: 0x06013EFD RID: 81661 RVA: 0x005EA400 File Offset: 0x005E8600
		public static bool CanRescueNow(Pawn rescuer, Pawn patient, bool forced = false)
		{
			return (forced || patient.Faction == rescuer.Faction) && HealthAIUtility.WantsToBeRescued(patient) && (forced || !patient.IsForbidden(rescuer)) && (forced || !GenAI.EnemyIsNear(patient, 25f, false)) && rescuer.CanReserveAndReach(patient, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, forced);
		}

		// Token: 0x06013EFE RID: 81662 RVA: 0x005EA464 File Offset: 0x005E8664
		public static Thing FindBestMedicine(Pawn healer, Pawn patient, bool onlyUseInventory = false)
		{
			HealthAIUtility.<>c__DisplayClass9_0 CS$<>8__locals1 = new HealthAIUtility.<>c__DisplayClass9_0();
			CS$<>8__locals1.patient = patient;
			CS$<>8__locals1.onlyUseInventory = onlyUseInventory;
			CS$<>8__locals1.healer = healer;
			if (CS$<>8__locals1.patient.playerSettings != null && CS$<>8__locals1.patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				return null;
			}
			if (Medicine.GetMedicineCountToFullyHeal(CS$<>8__locals1.patient) <= 0)
			{
				return null;
			}
			CS$<>8__locals1.validator = delegate(Thing m)
			{
				bool flag = ((CS$<>8__locals1.patient.playerSettings != null) ? CS$<>8__locals1.patient.playerSettings.medCare : MedicalCareCategory.NoMeds).AllowsMedicine(m.def);
				if (CS$<>8__locals1.patient.playerSettings == null & CS$<>8__locals1.onlyUseInventory)
				{
					flag = true;
				}
				return !m.IsForbidden(CS$<>8__locals1.healer) && flag && CS$<>8__locals1.healer.CanReserve(m, 10, 1, null, false);
			};
			Thing thing = CS$<>8__locals1.<FindBestMedicine>g__GetBestMedInInventory|2(CS$<>8__locals1.healer.inventory.innerContainer);
			if (CS$<>8__locals1.onlyUseInventory)
			{
				return thing;
			}
			Thing thing2 = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.patient.PositionHeld, CS$<>8__locals1.patient.MapHeld, CS$<>8__locals1.patient.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Medicine), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.healer, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, CS$<>8__locals1.validator, new Func<Thing, float>(HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0), false);
			if (thing == null || thing2 == null)
			{
				if (thing == null && thing2 == null && CS$<>8__locals1.healer.IsColonist && CS$<>8__locals1.healer.Map != null)
				{
					foreach (Pawn pawn in CS$<>8__locals1.healer.Map.mapPawns.SpawnedColonyAnimals)
					{
						Thing thing3 = CS$<>8__locals1.<FindBestMedicine>g__GetBestMedInInventory|2(pawn.inventory.innerContainer);
						if (thing3 != null && (thing2 == null || HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing2) < HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing3)) && !pawn.IsForbidden(CS$<>8__locals1.healer) && CS$<>8__locals1.healer.CanReach(pawn, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
						{
							thing2 = thing3;
						}
					}
				}
				return thing ?? thing2;
			}
			if (HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing) < HealthAIUtility.<FindBestMedicine>g__PriorityOf|9_0(thing2))
			{
				return thing2;
			}
			return thing;
		}

		// Token: 0x06013EFF RID: 81663 RVA: 0x005EA63C File Offset: 0x005E883C
		[CompilerGenerated]
		internal static float <FindBestMedicine>g__PriorityOf|9_0(Thing t)
		{
			return t.def.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
		}

		// Token: 0x0400DC2A RID: 56362
		private const float MinDistFromEnemyToRescue = 25f;
	}
}
