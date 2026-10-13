using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013F3 RID: 5107
	public static class DrugAIUtility
	{
		// Token: 0x06007C8E RID: 31886 RVA: 0x002583C4 File Offset: 0x002565C4
		public static Job IngestAndTakeToInventoryJob(Thing drug, Pawn pawn, int maxNumToCarry = 9999)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, drug);
			job.count = Mathf.Min(new int[]
			{
				drug.stackCount,
				drug.def.ingestible.defaultNumToIngestAtOnce,
				maxNumToCarry
			});
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy != null)
			{
				DrugPolicyEntry drugPolicyEntry = drugPolicy[drug.def];
				int num = pawn.inventory.innerContainer.TotalStackCountOfDef(drug.def) - job.count;
				if (drugPolicyEntry.allowScheduled && num <= 0)
				{
					job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
				}
			}
			return job;
		}
	}
}
