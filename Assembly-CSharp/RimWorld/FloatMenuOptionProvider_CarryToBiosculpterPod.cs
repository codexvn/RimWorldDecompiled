using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032D4 RID: 13012
	public class FloatMenuOptionProvider_CarryToBiosculpterPod : FloatMenuOptionProvider
	{
		// Token: 0x17002F59 RID: 12121
		// (get) Token: 0x060124FB RID: 75003 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F5A RID: 12122
		// (get) Token: 0x060124FC RID: 75004 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F5B RID: 12123
		// (get) Token: 0x060124FD RID: 75005 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F5C RID: 12124
		// (get) Token: 0x060124FE RID: 75006 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124FF RID: 75007 RVA: 0x00565319 File Offset: 0x00563519
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x06012500 RID: 75008 RVA: 0x00565320 File Offset: 0x00563520
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsColonistPlayerControlled && !clickedPawn.IsPrisonerOfColony)
			{
				yield break;
			}
			if (!clickedPawn.IsPrisoner && !clickedPawn.Downed)
			{
				yield break;
			}
			if (clickedPawn.IsPrisoner && PrisonBreakUtility.IsPrisonBreaking(clickedPawn))
			{
				yield break;
			}
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				yield break;
			}
			Thing thing = CompBiosculpterPod.FindPodFor(context.FirstSelectedPawn, clickedPawn, true) ?? CompBiosculpterPod.FindPodFor(context.FirstSelectedPawn, clickedPawn, false);
			CompBiosculpterPod podComp;
			if (thing == null || !thing.TryGetComp(out podComp))
			{
				yield break;
			}
			using (List<CompBiosculpterPod_Cycle>.Enumerator enumerator = podComp.AvailableCycles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBiosculpterPod_Cycle cycle = enumerator.Current;
					TaggedString taggedString = "CarryToBiosculpterPod".Translate(clickedPawn.Named("PAWN"), cycle.Props.label.Named("CYCLE"));
					if (clickedPawn.IsQuestLodger())
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
						yield break;
					}
					if (clickedPawn.GetExtraHostFaction(null) != null)
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
						yield break;
					}
					string text = podComp.CannotUseNowCycleReason(cycle) ?? podComp.CannotUseNowPawnCycleReason(context.FirstSelectedPawn, clickedPawn, cycle, true);
					if (text != null)
					{
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CompBiosculpterPod.CannotStartText(cycle, text), null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
						yield break;
					}
					Action action = delegate()
					{
						if (!podComp.CanAcceptOnceCycleChosen(clickedPawn))
						{
							Messages.Message("CannotCarryToBiosculpterPod".Translate() + ": " + "NoBiosculpterPod".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
							return;
						}
						podComp.PrepareCycleJob(context.FirstSelectedPawn, clickedPawn, cycle, podComp.MakeCarryToBiosculpterJob(clickedPawn));
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
				}
			}
			List<CompBiosculpterPod_Cycle>.Enumerator enumerator = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			yield break;
			yield break;
		}
	}
}
