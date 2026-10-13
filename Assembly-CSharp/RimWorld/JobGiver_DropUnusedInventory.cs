using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001480 RID: 5248
	public class JobGiver_DropUnusedInventory : ThinkNode_JobGiver
	{
		// Token: 0x06007E61 RID: 32353 RVA: 0x00260980 File Offset: 0x0025EB80
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			if (!pawn.Map.areaManager.Home[pawn.Position])
			{
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (Find.TickManager.TicksGame > pawn.mindState.lastInventoryRawFoodUseTick + 150000)
			{
				for (int i = pawn.inventory.innerContainer.Count - 1; i >= 0; i--)
				{
					Thing thing = pawn.inventory.innerContainer[i];
					if (thing.def.IsIngestible && !thing.def.IsDrug && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
					{
						this.Drop(pawn, thing);
					}
				}
			}
			for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
			{
				Thing thing2 = pawn.inventory.innerContainer[j];
				if (!JobGiver_DropUnusedInventory.ShouldKeepDrugInInventory(pawn, thing2))
				{
					this.Drop(pawn, thing2);
				}
			}
			return null;
		}

		// Token: 0x06007E62 RID: 32354 RVA: 0x00260A88 File Offset: 0x0025EC88
		public static bool ShouldKeepDrugInInventory(Pawn pawn, Thing drug)
		{
			return !drug.def.IsDrug || pawn.drugs == null || pawn.drugs.AllowedToTakeScheduledEver(drug.def) || (pawn.drugs.CurrentPolicy != null && pawn.drugs.CurrentPolicy[drug.def].takeToInventory != 0) || AddictionUtility.IsAddicted(pawn, drug) || AddictionUtility.HasChemicalDependency(pawn, drug);
		}

		// Token: 0x06007E63 RID: 32355 RVA: 0x00260AFC File Offset: 0x0025ECFC
		private void Drop(Pawn pawn, Thing thing)
		{
			Thing thing2;
			pawn.inventory.innerContainer.TryDrop(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, out thing2, null, null);
		}

		// Token: 0x040054C0 RID: 21696
		private const int RawFoodDropDelay = 150000;
	}
}
