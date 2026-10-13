using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200332D RID: 13101
	public class FloatMenuOptionProvider_Reload : FloatMenuOptionProvider
	{
		// Token: 0x17002FE7 RID: 12263
		// (get) Token: 0x060126AD RID: 75437 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FE8 RID: 12264
		// (get) Token: 0x060126AE RID: 75438 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FE9 RID: 12265
		// (get) Token: 0x060126AF RID: 75439 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060126B0 RID: 75440 RVA: 0x0056D9EB File Offset: 0x0056BBEB
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			using (IEnumerator<IReloadableComp> enumerator = this.GetReloadablesUsingAmmo(context.FirstSelectedPawn, clickedThing).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IReloadableComp reloadable = enumerator.Current;
					ThingComp thingComp = reloadable as ThingComp;
					string text = "Reload".Translate(thingComp.parent.Named("GEAR"), reloadable.AmmoDef.Named("AMMO")) + " (" + reloadable.LabelRemaining + ")";
					List<Thing> chosenAmmo;
					if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						yield return new FloatMenuOption(text + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (!reloadable.NeedsReload(true))
					{
						yield return new FloatMenuOption(text + ": " + "ReloadFull".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if ((chosenAmmo = ReloadableUtility.FindEnoughAmmo(context.FirstSelectedPawn, clickedThing.Position, reloadable, true)) == null)
					{
						yield return new FloatMenuOption(text + ": " + "ReloadNotEnough".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else if (context.FirstSelectedPawn.carryTracker.AvailableStackSpace(reloadable.AmmoDef) < reloadable.MinAmmoNeeded(true))
					{
						yield return new FloatMenuOption(text + ": " + "ReloadCannotCarryEnough".Translate(reloadable.AmmoDef.Named("AMMO")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						Action action = delegate()
						{
							context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobGiver_Reload.MakeReloadJob(reloadable, chosenAmmo), new JobTag?(JobTag.Misc), false);
						};
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
					}
				}
			}
			IEnumerator<IReloadableComp> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060126B1 RID: 75441 RVA: 0x0056DA09 File Offset: 0x0056BC09
		private IEnumerable<IReloadableComp> GetReloadablesUsingAmmo(Pawn pawn, Thing clickedThing)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.PrimaryEq : null) != null)
			{
				IReloadableComp reloadableComp = pawn.equipment.PrimaryEq as IReloadableComp;
				if (reloadableComp != null && clickedThing.def == reloadableComp.AmmoDef)
				{
					yield return reloadableComp;
				}
			}
			if (pawn.apparel != null)
			{
				foreach (Apparel thing in pawn.apparel.WornApparel)
				{
					IReloadableComp reloadableComp2 = thing.TryGetComp<CompApparelReloadable>();
					if (reloadableComp2 != null && clickedThing.def == reloadableComp2.AmmoDef)
					{
						yield return reloadableComp2;
					}
				}
				List<Apparel>.Enumerator enumerator = default(List<Apparel>.Enumerator);
			}
			yield break;
			yield break;
		}
	}
}
