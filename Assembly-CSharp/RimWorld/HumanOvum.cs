using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002AD1 RID: 10961
	[StaticConstructorOnStartup]
	public class HumanOvum : ThingWithComps
	{
		// Token: 0x0600F437 RID: 62519 RVA: 0x0046F7D7 File Offset: 0x0046D9D7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!ModsConfig.BiotechActive)
			{
				yield break;
			}
			if (base.MapHeld != null)
			{
				Pawn pawn = this.fertilizingMan;
				JobDef jobDef;
				if (pawn == null)
				{
					jobDef = null;
				}
				else
				{
					Job curJob = pawn.CurJob;
					jobDef = ((curJob != null) ? curJob.def : null);
				}
				if (jobDef != JobDefOf.FertilizeOvum || this.fertilizingMan.CurJob.GetTarget(TargetIndex.A).Thing != this)
				{
					this.fertilizingMan = null;
				}
				if (this.fertilizingMan == null)
				{
					List<FloatMenuOption> maleOptions = new List<FloatMenuOption>();
					foreach (Pawn pawn2 in base.MapHeld.mapPawns.FreeColonistsSpawned)
					{
						FloatMenuOption floatMenuOption = this.CanFertilizeFloatOption(pawn2, true);
						if (floatMenuOption != null)
						{
							maleOptions.Add(floatMenuOption);
						}
					}
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = string.Format("{0}...", "Fertilize".Translate()),
						defaultDesc = "FertilizeGizmoDescription".Translate(),
						icon = HumanOvum.FertilizeIcon.Texture,
						action = delegate()
						{
							Find.WindowStack.Add(new FloatMenu(maleOptions));
						}
					};
					if (maleOptions.Count == 0)
					{
						command_Action.Disable("FertilizeDisabledNoMales".Translate());
					}
					yield return command_Action;
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "FertilizeCancel".Translate(this.fertilizingMan.LabelShort),
						defaultDesc = "FertilizeCancel".Translate(this.fertilizingMan.LabelShort),
						icon = HumanOvum.CancelFertilizeIcon,
						action = delegate()
						{
							this.fertilizingMan.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						},
						activateSound = SoundDefOf.Designate_Cancel
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F438 RID: 62520 RVA: 0x0046F7E8 File Offset: 0x0046D9E8
		public Thing ProduceEmbryo(Pawn father)
		{
			HumanEmbryo humanEmbryo = (HumanEmbryo)ThingMaker.MakeThing(ThingDefOf.HumanEmbryo, null);
			CompHasPawnSources comp = humanEmbryo.GetComp<CompHasPawnSources>();
			List<Pawn> pawnSources = base.GetComp<CompHasPawnSources>().pawnSources;
			if (!pawnSources.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn in pawnSources)
				{
					comp.AddSource(pawn);
				}
			}
			comp.AddSource(father);
			if (!humanEmbryo.TryPopulateGenes())
			{
				humanEmbryo.Destroy(DestroyMode.Vanish);
				return null;
			}
			return humanEmbryo;
		}

		// Token: 0x0600F439 RID: 62521 RVA: 0x0046F880 File Offset: 0x0046DA80
		public override Thing SplitOff(int count)
		{
			return base.SplitOff(count) as HumanOvum;
		}

		// Token: 0x0600F43A RID: 62522 RVA: 0x0046F88E File Offset: 0x0046DA8E
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			FloatMenuOption floatMenuOption2 = this.CanFertilizeFloatOption(selPawn, false);
			if (floatMenuOption2 != null)
			{
				yield return floatMenuOption2;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F43B RID: 62523 RVA: 0x0046F8A8 File Offset: 0x0046DAA8
		private FloatMenuOption CanFertilizeFloatOption(Pawn pawn, bool pawnAsLabel)
		{
			AcceptanceReport acceptanceReport = this.CanFertilizeReport(pawn);
			if (acceptanceReport.Accepted)
			{
				Action <>9__2;
				return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "FertilizeThing".Translate(this).ToString(), delegate()
				{
					Pawn pawn2 = this.GetComp<CompHasPawnSources>().pawnSources.FirstOrDefault<Pawn>();
					PawnRelationDef pawnRelationDef;
					float num = PregnancyUtility.InbredChanceFromParents(pawn, pawn2, out pawnRelationDef);
					if (num > 0f)
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "FertilizeOvumInbredChance".Translate(pawn2.Named("MOTHER"), pawn.Named("FATHER"), pawnRelationDef.label.Named("RELATION"), num.ToStringPercent().Named("CHANCE"));
						Action confirmedAct;
						if ((confirmedAct = <>9__2) == null)
						{
							confirmedAct = (<>9__2 = delegate()
							{
								base.<CanFertilizeFloatOption>g__TakeJob|1();
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
						return;
					}
					base.<CanFertilizeFloatOption>g__TakeJob|1();
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("FertilizeCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600F43C RID: 62524 RVA: 0x0046F9B0 File Offset: 0x0046DBB0
		private AcceptanceReport CanFertilizeReport(Pawn pawn)
		{
			if (pawn.gender != Gender.Male)
			{
				return false;
			}
			if (pawn.IsQuestLodger())
			{
				return false;
			}
			if (!pawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if ((float)pawn.ageTracker.AgeBiologicalYears < 14f)
			{
				return "CannotMustBeAge".Translate(14f).CapitalizeFirst();
			}
			if (pawn.Sterile())
			{
				return "CannotSterile".Translate();
			}
			if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return "Incapacitated".Translate().ToLower();
			}
			return true;
		}

		// Token: 0x0600F43D RID: 62525 RVA: 0x0046FA89 File Offset: 0x0046DC89
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.fertilizingMan, "fertilizingMan", false);
		}

		// Token: 0x0400A0FA RID: 41210
		public static readonly CachedTexture FertilizeIcon = new CachedTexture("UI/Gizmos/Fertilize");

		// Token: 0x0400A0FB RID: 41211
		private Pawn fertilizingMan;

		// Token: 0x0400A0FC RID: 41212
		private static readonly Texture2D CancelFertilizeIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
