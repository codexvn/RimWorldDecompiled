using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003307 RID: 13063
	public class FloatMenuOptionProvider_HandleCorpse : FloatMenuOptionProvider
	{
		// Token: 0x17002FB3 RID: 12211
		// (get) Token: 0x060125FA RID: 75258 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FB4 RID: 12212
		// (get) Token: 0x060125FB RID: 75259 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FB5 RID: 12213
		// (get) Token: 0x060125FC RID: 75260 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FB6 RID: 12214
		// (get) Token: 0x060125FD RID: 75261 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125FE RID: 75262 RVA: 0x00569EEB File Offset: 0x005680EB
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			Corpse corpse = clickedThing as Corpse;
			if (corpse == null)
			{
				yield break;
			}
			Building_GibbetCage cage = Building_GibbetCage.FindGibbetCageFor(corpse, context.FirstSelectedPawn, false);
			if (cage != null)
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(corpse, cage), delegate()
				{
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(context.FirstSelectedPawn, corpse, cage), new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(corpse), "ReservedBy", null);
			}
			if (!corpse.IsInValidStorage())
			{
				yield break;
			}
			StoragePriority priority = StoreUtility.CurrentHaulDestinationOf(corpse).GetStoreSettings().Priority;
			IHaulDestination haulDestination;
			if (StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(corpse, context.FirstSelectedPawn, context.FirstSelectedPawn.Map, priority, Faction.OfPlayer, out haulDestination, true, true) && haulDestination.GetStoreSettings().Priority == priority)
			{
				Building_Grave grave = haulDestination as Building_Grave;
				if (grave != null)
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PrioritizeGeneric".Translate("Burying".Translate(), corpse.Label).CapitalizeFirst(), delegate()
					{
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(context.FirstSelectedPawn, corpse, grave), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(corpse), "ReservedBy", null);
				}
			}
			yield break;
		}
	}
}
