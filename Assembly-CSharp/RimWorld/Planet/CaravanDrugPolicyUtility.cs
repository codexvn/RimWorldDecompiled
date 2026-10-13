using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D44 RID: 15684
	public static class CaravanDrugPolicyUtility
	{
		// Token: 0x060155DE RID: 87518 RVA: 0x0066386D File Offset: 0x00661A6D
		public static void CheckTakeScheduledDrugs(Caravan caravan, int delta)
		{
			if (caravan.IsHashIntervalTick(120, delta))
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(caravan);
			}
		}

		// Token: 0x060155DF RID: 87519 RVA: 0x00663880 File Offset: 0x00661A80
		public static void TryTakeScheduledDrugs(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				CaravanDrugPolicyUtility.TryTakeScheduledDrugs(pawnsListForReading[i], caravan);
			}
		}

		// Token: 0x060155E0 RID: 87520 RVA: 0x006638B4 File Offset: 0x00661AB4
		private static void TryTakeScheduledDrugs(Pawn pawn, Caravan caravan)
		{
			if (pawn.drugs == null || pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			if (currentPolicy == null)
			{
				return;
			}
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				Thing drug;
				Pawn drugOwner;
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug) && CaravanInventoryUtility.TryGetThingOfDef(caravan, currentPolicy[i].drug, out drug, out drugOwner))
				{
					caravan.needs.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		// Token: 0x0400EDE9 RID: 60905
		private const int TryTakeScheduledDrugsIntervalTicks = 120;
	}
}
