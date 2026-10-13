using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002262 RID: 8802
	public class CompAbilityEffect_ReimplantXenogerm : CompAbilityEffect
	{
		// Token: 0x17001D50 RID: 7504
		// (get) Token: 0x0600BFE5 RID: 49125 RVA: 0x00379C73 File Offset: 0x00377E73
		private new CompProperties_AbilityReimplantXenogerm Props
		{
			get
			{
				return (CompProperties_AbilityReimplantXenogerm)this.props;
			}
		}

		// Token: 0x0600BFE6 RID: 49126 RVA: 0x00379C80 File Offset: 0x00377E80
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckBiotech("xenogerm reimplantation"))
			{
				return;
			}
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			GeneUtility.ReimplantXenogerm(this.parent.pawn, pawn);
			FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, new Vector3(0f, 0f, 0.26f), 1f, -1f);
			if (PawnUtility.ShouldSendNotificationAbout(this.parent.pawn) || PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				int max = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				int max2 = HediffDefOf.XenogermLossShock.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				Find.LetterStack.ReceiveLetter("LetterLabelGenesImplanted".Translate(), "LetterTextGenesImplanted".Translate(this.parent.pawn.Named("CASTER"), pawn.Named("TARGET"), max.ToStringTicksToPeriod(true, false, true, true, false).Named("COMADURATION"), max2.ToStringTicksToPeriod(true, false, true, true, false).Named("SHOCKDURATION")), LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
				{
					this.parent.pawn,
					pawn
				}), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600BFE7 RID: 49127 RVA: 0x00379DD4 File Offset: 0x00377FD4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return base.Valid(target, throwMessages);
			}
			if (pawn.IsQuestLodger())
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (pawn.HostileTo(this.parent.pawn) && !pawn.Downed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.parent.pawn.genes.Xenogenes.Any<Gene>())
			{
				if (throwMessages)
				{
					Messages.Message("MessagePawnHasNoXenogenes".Translate(this.parent.pawn), this.parent.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (GeneUtility.SameXenotype(pawn, this.parent.pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotUseOnSameXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant(this.parent.pawn, pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotBecomeNonPreferredXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.Valid(target, throwMessages);
		}

		// Token: 0x0600BFE8 RID: 49128 RVA: 0x00379F56 File Offset: 0x00378156
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			if (GeneUtility.PawnWouldDieFromReimplanting(this.parent.pawn))
			{
				return Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(this.parent.pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x0600BFE9 RID: 49129 RVA: 0x00379F94 File Offset: 0x00378194
		public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			yield return MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_XenogermImplantation, new Vector3(0f, 0f, 0.3f), 1f, -1f);
			yield break;
		}

		// Token: 0x0600BFEA RID: 49130 RVA: 0x00379FA4 File Offset: 0x003781A4
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (GeneUtility.CanAbsorbXenogerm(this.parent.pawn))
			{
				Pawn myPawn = this.parent.pawn;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "ForceXenogermImplantation".Translate(),
					defaultDesc = "ForceXenogermImplantationDesc".Translate(),
					icon = CompAbilityEffect_ReimplantXenogerm.ReimplantIcon.Texture,
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						List<Pawn> list2 = myPawn.MapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
						for (int i = 0; i < list2.Count; i++)
						{
							Pawn absorber = list2[i];
							if (absorber.genes != null && absorber.IsColonistPlayerControlled && !GeneUtility.SameXenotype(absorber, myPawn) && absorber.CanReach(myPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
							{
								if (!CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant(this.parent.pawn, absorber))
								{
									list.Add(new FloatMenuOption(absorber.LabelCap + ": " + "IdeoligionForbids".Translate(), null, absorber, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								else
								{
									Action <>9__2;
									list.Add(new FloatMenuOption(absorber.LabelShort, delegate()
									{
										if (GeneUtility.PawnWouldDieFromReimplanting(myPawn))
										{
											WindowStack windowStack = Find.WindowStack;
											TaggedString text = "WarningPawnWillDieFromReimplanting".Translate(myPawn.Named("PAWN"));
											Action confirmedAct;
											if ((confirmedAct = <>9__2) == null)
											{
												confirmedAct = (<>9__2 = delegate()
												{
													GeneUtility.GiveReimplantJob(absorber, myPawn);
												});
											}
											windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
											return;
										}
										GeneUtility.GiveReimplantJob(absorber, myPawn);
									}, absorber, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
						}
						if (list.Any<FloatMenuOption>())
						{
							Find.WindowStack.Add(new FloatMenu(list));
						}
					}
				};
				if (myPawn.IsQuestLodger())
				{
					command_Action.Disable("TemporaryFactionMember".Translate(myPawn.Named("PAWN")));
				}
				else if (myPawn.health.hediffSet.HasHediff(HediffDefOf.XenogermLossShock, false))
				{
					command_Action.Disable("XenogermLossShockPresent".Translate(myPawn.Named("PAWN")));
				}
				else if (myPawn.IsPrisonerOfColony && !myPawn.Downed)
				{
					command_Action.Disable("MessageTargetMustBeDownedToForceReimplant".Translate(myPawn.Named("PAWN")));
				}
				yield return command_Action;
			}
			yield break;
		}

		// Token: 0x0600BFEB RID: 49131 RVA: 0x00379FB4 File Offset: 0x003781B4
		public static bool PawnIdeoCanAcceptReimplant(Pawn implanter, Pawn implantee)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return true;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, implantee))
			{
				if (implanter.genes.Xenogenes.Any((Gene x) => x.def == GeneDefOf.Bloodfeeder))
				{
					return false;
				}
			}
			return IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, implantee) || implantee.Ideo.IsPreferredXenotype(implanter);
		}

		// Token: 0x0400838B RID: 33675
		private static readonly CachedTexture ReimplantIcon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");
	}
}
