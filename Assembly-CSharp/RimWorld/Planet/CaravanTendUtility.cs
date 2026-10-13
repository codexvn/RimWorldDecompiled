using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D67 RID: 15719
	public static class CaravanTendUtility
	{
		// Token: 0x06015698 RID: 87704 RVA: 0x006681EC File Offset: 0x006663EC
		public static void CheckTend(Caravan caravan, int delta)
		{
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn = caravan.pawns[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn, null, caravan) && pawn.IsHashIntervalTick(1250, delta))
				{
					CaravanTendUtility.TryTendToAnyPawn(caravan);
				}
			}
		}

		// Token: 0x06015699 RID: 87705 RVA: 0x0066823C File Offset: 0x0066643C
		public static void TryTendToAnyPawn(Caravan caravan)
		{
			CaravanTendUtility.FindPawnsNeedingTend(caravan, CaravanTendUtility.tmpPawnsNeedingTreatment);
			if (!CaravanTendUtility.tmpPawnsNeedingTreatment.Any<Pawn>())
			{
				return;
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.SortByDescending(new Func<Pawn, float>(CaravanTendUtility.GetTendPriority));
			Pawn patient = null;
			Pawn pawn = null;
			for (int i = 0; i < CaravanTendUtility.tmpPawnsNeedingTreatment.Count; i++)
			{
				patient = CaravanTendUtility.tmpPawnsNeedingTreatment[i];
				pawn = CaravanTendUtility.FindBestDoctorFor(caravan, patient);
				if (pawn != null)
				{
					break;
				}
			}
			if (pawn == null)
			{
				return;
			}
			Medicine medicine = null;
			Pawn pawn2 = null;
			CaravanInventoryUtility.TryGetBestMedicine(caravan, patient, out medicine, out pawn2);
			TendUtility.DoTend(pawn, patient, medicine);
			if (medicine != null && medicine.Destroyed && pawn2 != null)
			{
				pawn2.inventory.innerContainer.Remove(medicine);
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.Clear();
		}

		// Token: 0x0601569A RID: 87706 RVA: 0x006682F4 File Offset: 0x006664F4
		private static void FindPawnsNeedingTend(Caravan caravan, List<Pawn> outPawnsNeedingTend)
		{
			outPawnsNeedingTend.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if ((pawn.playerSettings == null || pawn.playerSettings.medCare > MedicalCareCategory.NoCare) && pawn.health.HasHediffsNeedingTend(false))
				{
					outPawnsNeedingTend.Add(pawn);
				}
			}
		}

		// Token: 0x0601569B RID: 87707 RVA: 0x00668354 File Offset: 0x00666554
		private static Pawn FindBestDoctorFor(Caravan caravan, Pawn patient)
		{
			float num = 0f;
			Pawn pawn = null;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn2, patient, caravan))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.MedicalTendQuality, true, -1);
					if (statValue > num || pawn == null)
					{
						num = statValue;
						pawn = pawn2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0601569C RID: 87708 RVA: 0x006683B4 File Offset: 0x006665B4
		private static bool IsValidDoctorFor(Pawn doctor, Pawn patient, Caravan caravan)
		{
			if (!doctor.RaceProps.Humanlike)
			{
				if (!doctor.IsColonyMechPlayerControlled || !doctor.RaceProps.mechEnabledWorkTypes.NotNullAndContains(WorkTypeDefOf.Doctor))
				{
					return false;
				}
			}
			else if (!caravan.IsOwner(doctor))
			{
				return false;
			}
			return (doctor != patient || (doctor.IsColonist && doctor.playerSettings.selfTend)) && !doctor.Downed && !doctor.InMentalState && (doctor.story == null || !doctor.WorkTypeIsDisabled(WorkTypeDefOf.Doctor));
		}

		// Token: 0x0601569D RID: 87709 RVA: 0x00668440 File Offset: 0x00666640
		private static float GetTendPriority(Pawn patient)
		{
			int num = HealthUtility.TicksUntilDeathDueToBloodLoss(patient);
			if (num < 15000)
			{
				if (patient.RaceProps.Humanlike)
				{
					return GenMath.LerpDouble(0f, 15000f, 5f, 4f, (float)num);
				}
				return GenMath.LerpDouble(0f, 15000f, 4f, 3f, (float)num);
			}
			else
			{
				int i = 0;
				while (i < patient.health.hediffSet.hediffs.Count)
				{
					Hediff hediff = patient.health.hediffSet.hediffs[i];
					if ((hediff.IsCurrentlyLifeThreatening || hediff.IsLethal) && hediff.TendableNow(false))
					{
						if (patient.RaceProps.Humanlike)
						{
							return 2.5f;
						}
						return 2f;
					}
					else
					{
						i++;
					}
				}
				if (patient.health.hediffSet.BleedRateTotal >= 0.0001f)
				{
					if (patient.RaceProps.Humanlike)
					{
						return 1.5f;
					}
					return 1f;
				}
				else
				{
					if (patient.RaceProps.Humanlike)
					{
						return 0.5f;
					}
					return 0f;
				}
			}
		}

		// Token: 0x0400EE57 RID: 61015
		private static readonly List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		// Token: 0x0400EE58 RID: 61016
		private const int TendIntervalTicks = 1250;
	}
}
