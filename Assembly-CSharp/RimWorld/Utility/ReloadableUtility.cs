using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Utility
{
	// Token: 0x02003896 RID: 14486
	public static class ReloadableUtility
	{
		// Token: 0x06014049 RID: 81993 RVA: 0x005F1970 File Offset: 0x005EFB70
		public static IReloadableComp FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.PrimaryEq : null) != null)
			{
				IReloadableComp reloadableComp = pawn.equipment.PrimaryEq as IReloadableComp;
				if (reloadableComp != null && reloadableComp.NeedsReload(allowForcedReload))
				{
					return reloadableComp;
				}
			}
			if (pawn.apparel != null)
			{
				foreach (Apparel thing in pawn.apparel.WornApparel)
				{
					CompApparelReloadable compApparelReloadable = thing.TryGetComp<CompApparelReloadable>();
					if (compApparelReloadable != null && compApparelReloadable.NeedsReload(allowForcedReload))
					{
						return compApparelReloadable;
					}
				}
			}
			return null;
		}

		// Token: 0x0601404A RID: 81994 RVA: 0x005F1A14 File Offset: 0x005EFC14
		public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, IReloadableComp reloadable, bool forceReload)
		{
			if (reloadable == null)
			{
				return null;
			}
			IntRange desiredQuantity = new IntRange(reloadable.MinAmmoNeeded(forceReload), reloadable.MaxAmmoNeeded(forceReload));
			return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == reloadable.AmmoDef);
		}

		// Token: 0x0601404B RID: 81995 RVA: 0x005F1A6C File Offset: 0x005EFC6C
		public static Pawn OwnerOf(IReloadableComp reloadable)
		{
			IThingHolder parentHolder = reloadable.ReloadableThing.ParentHolder;
			Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				return pawn_ApparelTracker.pawn;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker == null)
			{
				return null;
			}
			return pawn_EquipmentTracker.pawn;
		}

		// Token: 0x0601404C RID: 81996 RVA: 0x005F1AAC File Offset: 0x005EFCAC
		public static int TotalChargesFromQueuedJobs(Pawn pawn, ThingWithComps gear)
		{
			ICompWithCharges compWithCharges = gear.TryGetComp<CompApparelVerbOwner_Charged>();
			ICompWithCharges compWithCharges2 = compWithCharges ?? gear.TryGetComp<CompEquippableAbilityReloadable>();
			int num = 0;
			if (compWithCharges2 != null && pawn != null)
			{
				foreach (Job job in pawn.jobs.AllJobs())
				{
					Verb verbToUse = job.verbToUse;
					if (verbToUse != null && (compWithCharges2 == verbToUse.ReloadableCompSource || compWithCharges2 == verbToUse.DirectOwner))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0601404D RID: 81997 RVA: 0x005F1B38 File Offset: 0x005EFD38
		public static bool CanUseConsideringQueuedJobs(Pawn pawn, ThingWithComps gear, bool showMessage = true)
		{
			ICompWithCharges compWithCharges = gear.TryGetComp<CompApparelVerbOwner_Charged>();
			ICompWithCharges compWithCharges2 = compWithCharges ?? gear.TryGetComp<CompEquippableAbilityReloadable>();
			if (compWithCharges2 == null)
			{
				return true;
			}
			string text = null;
			if (!Event.current.shift)
			{
				string text2;
				if (!compWithCharges2.CanBeUsed(out text2))
				{
					text = text2;
				}
			}
			else if (ReloadableUtility.TotalChargesFromQueuedJobs(pawn, gear) + 1 > compWithCharges2.RemainingCharges)
			{
				IReloadableComp reloadableComp = compWithCharges2 as IReloadableComp;
				if (reloadableComp != null)
				{
					text = reloadableComp.DisabledReason(reloadableComp.MaxAmmoAmount(), reloadableComp.MaxAmmoAmount());
				}
				else
				{
					text = "CommandNoUsesLeft".Translate();
				}
			}
			if (text != null)
			{
				if (showMessage)
				{
					Messages.Message(text, pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
