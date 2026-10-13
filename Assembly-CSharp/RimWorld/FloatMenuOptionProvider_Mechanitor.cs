using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003315 RID: 13077
	public class FloatMenuOptionProvider_Mechanitor : FloatMenuOptionProvider
	{
		// Token: 0x17002FCA RID: 12234
		// (get) Token: 0x06012643 RID: 75331 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FCB RID: 12235
		// (get) Token: 0x06012644 RID: 75332 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FCC RID: 12236
		// (get) Token: 0x06012645 RID: 75333 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FCD RID: 12237
		// (get) Token: 0x06012646 RID: 75334 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012647 RID: 75335 RVA: 0x0056B9C7 File Offset: 0x00569BC7
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(context.FirstSelectedPawn);
		}

		// Token: 0x06012648 RID: 75336 RVA: 0x0056B9DD File Offset: 0x00569BDD
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsColonyMech)
			{
				yield break;
			}
			if (clickedPawn.GetOverseer() != context.FirstSelectedPawn)
			{
				if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CannotControlMech".Translate(clickedPawn.LabelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (!MechanitorUtility.CanControlMech(context.FirstSelectedPawn, clickedPawn))
				{
					AcceptanceReport acceptanceReport = MechanitorUtility.CanControlMech(context.FirstSelectedPawn, clickedPawn);
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						yield return new FloatMenuOption("CannotControlMech".Translate(clickedPawn.LabelShort) + ": " + acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				else
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ControlMech".Translate(clickedPawn.LabelShort), delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.ControlMech, clickedPawn);
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
				}
				yield return new FloatMenuOption("CannotDisassembleMech".Translate(clickedPawn.LabelCap) + ": " + "MustBeOverseer".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DisconnectMech".Translate(clickedPawn.LabelShort), delegate()
				{
					MechanitorUtility.ForceDisconnectMechFromOverseer(clickedPawn);
				}, MenuOptionPriority.Low, null, null, 0f, null, null, true, -10), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
				if (!clickedPawn.IsFighting())
				{
					Action <>9__5;
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DisassembleMech".Translate(clickedPawn.LabelCap), delegate()
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "ConfirmDisassemblingMech".Translate(clickedPawn.LabelCap) + ":\n" + (from x in MechanitorUtility.IngredientsFromDisassembly(clickedPawn.def)
						select x.Summary).ToLineList("  - ", false);
						Action confirmedAct;
						if ((confirmedAct = <>9__5) == null)
						{
							confirmedAct = (<>9__5 = delegate()
							{
								context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DisassembleMech, clickedPawn), new JobTag?(JobTag.Misc), false);
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
					}, MenuOptionPriority.Low, null, null, 0f, null, null, true, -20), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
				}
			}
			if (context.FirstSelectedPawn.Drafted && MechRepairUtility.CanRepair(clickedPawn))
			{
				if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CannotRepairMech".Translate(clickedPawn.LabelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RepairThing".Translate(clickedPawn.LabelShort), delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.RepairMech, clickedPawn);
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
				}
			}
			yield break;
		}
	}
}
