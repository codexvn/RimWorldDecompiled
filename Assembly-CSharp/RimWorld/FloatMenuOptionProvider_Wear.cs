using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003346 RID: 13126
	public class FloatMenuOptionProvider_Wear : FloatMenuOptionProvider
	{
		// Token: 0x17003011 RID: 12305
		// (get) Token: 0x0601272A RID: 75562 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003012 RID: 12306
		// (get) Token: 0x0601272B RID: 75563 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003013 RID: 12307
		// (get) Token: 0x0601272C RID: 75564 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601272D RID: 75565 RVA: 0x0056FB27 File Offset: 0x0056DD27
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.apparel != null;
		}

		// Token: 0x0601272E RID: 75566 RVA: 0x0056FB38 File Offset: 0x0056DD38
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Apparel apparel = clickedThing as Apparel;
			if (apparel == null)
			{
				return null;
			}
			string key = "CannotWear";
			string key2 = "ForceWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				key = "CannotEquipApparel";
				key2 = "ForceEquipApparel";
			}
			if (!context.FirstSelectedPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.apparel.WouldReplaceLockedApparel(apparel))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.IsMutant && context.FirstSelectedPawn.mutant.Def.disableApparel)
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + context.FirstSelectedPawn.mutant.Def.LabelCap, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!ApparelUtility.HasPartsToWear(context.FirstSelectedPawn, apparel.def))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string t;
			if (!EquipmentUtility.CanEquip(apparel, context.FirstSelectedPawn, out t, true))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate()
			{
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						apparel.SetForbidden(false, true);
						Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					});
				}
				Action action2 = action;
				Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(context.FirstSelectedPawn, apparel);
				if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(context.FirstSelectedPawn, apparelReplacedByNewApparel, action2))
				{
					action2();
				}
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, apparel, "ReservedBy", null);
		}
	}
}
