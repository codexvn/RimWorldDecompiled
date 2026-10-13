using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D7 RID: 5591
	public class WorkGiver_HaulResourcesToCarrier : WorkGiver_Scanner
	{
		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x0600840E RID: 33806 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600840F RID: 33807 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x00279BD8 File Offset: 0x00277DD8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul resources to carrier"))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			if (!pawn2.IsColonyMech || !pawn2.Spawned || pawn2.Downed)
			{
				return false;
			}
			CompMechCarrier comp = pawn2.GetComp<CompMechCarrier>();
			if (comp == null)
			{
				return false;
			}
			int amountToAutofill = comp.AmountToAutofill;
			return amountToAutofill > 0 && pawn.CanReserve(t, 1, -1, null, forced) && !HaulAIUtility.FindFixedIngredientCount(pawn, comp.Props.fixedIngredient, amountToAutofill).NullOrEmpty<Thing>();
		}

		// Token: 0x06008411 RID: 33809 RVA: 0x00279C5C File Offset: 0x00277E5C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompMechCarrier compMechCarrier = t.TryGetComp<CompMechCarrier>();
			if (compMechCarrier == null)
			{
				return null;
			}
			int amountToAutofill = compMechCarrier.AmountToAutofill;
			if (amountToAutofill <= 0)
			{
				return null;
			}
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, compMechCarrier.Props.fixedIngredient, amountToAutofill);
			if (!list.NullOrEmpty<Thing>())
			{
				Job job = HaulAIUtility.HaulToContainerJob(pawn, list[0], t);
				job.count = Mathf.Min(job.count, amountToAutofill);
				job.targetQueueB = (from i in list.Skip(1)
				select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
				return job;
			}
			return null;
		}
	}
}
