using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AEB RID: 10987
	[StaticConstructorOnStartup]
	public class Xenogerm : GeneSetHolderBase
	{
		// Token: 0x17002654 RID: 9812
		// (get) Token: 0x0600F515 RID: 62741 RVA: 0x004735FF File Offset: 0x004717FF
		public override string LabelNoCount
		{
			get
			{
				if (this.xenotypeName.NullOrEmpty())
				{
					return base.LabelNoCount;
				}
				return "NamedXenogerm".Translate(this.xenotypeName.Named("NAME"));
			}
		}

		// Token: 0x17002655 RID: 9813
		// (get) Token: 0x0600F516 RID: 62742 RVA: 0x00473634 File Offset: 0x00471834
		private int RequiredMedicineForImplanting
		{
			get
			{
				int num = 0;
				for (int i = 0; i < RecipeDefOf.ImplantXenogerm.ingredients.Count; i++)
				{
					IngredientCount ingredientCount = RecipeDefOf.ImplantXenogerm.ingredients[i];
					if (ingredientCount.filter.Allows(ThingDefOf.MedicineIndustrial))
					{
						num += (int)ingredientCount.GetBaseCount();
					}
				}
				return num;
			}
		}

		// Token: 0x0600F517 RID: 62743 RVA: 0x0047368C File Offset: 0x0047188C
		public override void Notify_DebugSpawned()
		{
			this.geneSet = GeneUtility.GenerateGeneSet(null);
			this.xenotypeName = "Unique".Translate();
			this.iconDef = XenotypeIconDefOf.Basic;
		}

		// Token: 0x0600F518 RID: 62744 RVA: 0x004736CD File Offset: 0x004718CD
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("xenogerm"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
		}

		// Token: 0x0600F519 RID: 62745 RVA: 0x004736EC File Offset: 0x004718EC
		public void SetTargetPawn(Pawn newTarget)
		{
			int trueMax = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.TrueMax;
			TaggedString taggedString = "ImplantXenogermWarningDesc".Translate(newTarget.Named("PAWN"), trueMax.ToStringTicksToPeriod(true, false, true, true, false).Named("COMADURATION"));
			if (newTarget.genes.Xenogenes.Any<Gene>())
			{
				taggedString += "\n\n" + "ImplantXenogermWarningOverwriteXenogenes".Translate(newTarget.Named("PAWN"), newTarget.genes.XenotypeLabelCap.Named("XENOTYPE"), (from x in newTarget.genes.Xenogenes
				select x.LabelCap).ToLineList("  - ", false).Named("XENOGENES"));
			}
			int num = GeneUtility.MetabolismAfterImplanting(newTarget, this.geneSet);
			taggedString += "\n\n" + "ImplantXenogermWarningNewMetabolism".Translate(newTarget.Named("PAWN"), num.Named("MET"), GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num).ToStringPercent().Named("CONSUMPTION"));
			taggedString += "\n\n" + "WouldYouLikeToContinue".Translate();
			Predicate<Bill> <>9__2;
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
			{
				Pawn pawn = this.targetPawn;
				Bill bill;
				if (pawn == null)
				{
					bill = null;
				}
				else
				{
					BillStack billStack = pawn.BillStack;
					if (billStack == null)
					{
						bill = null;
					}
					else
					{
						List<Bill> bills = billStack.Bills;
						if (bills == null)
						{
							bill = null;
						}
						else
						{
							Predicate<Bill> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((Bill x) => x.xenogerm == this));
							}
							bill = bills.FirstOrDefault(predicate);
						}
					}
				}
				Bill bill2 = bill;
				if (bill2 != null)
				{
					this.targetPawn.BillStack.Delete(bill2);
				}
				HealthCardUtility.CreateSurgeryBill(newTarget, RecipeDefOf.ImplantXenogerm, null, null, true).xenogerm = this;
				this.targetPawn = newTarget;
				this.SendImplantationLetter(newTarget);
			}, true, null, WindowLayer.Dialog));
		}

		// Token: 0x0600F51A RID: 62746 RVA: 0x0047389C File Offset: 0x00471A9C
		private void SendImplantationLetter(Pawn targetPawn)
		{
			string arg = string.Empty;
			if (!targetPawn.InBed() && !targetPawn.Map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(targetPawn, x.def) && ((Building_Bed)x).Medical))
			{
				arg = "XenogermOrderedImplantedBedNeeded".Translate(targetPawn.Named("PAWN"));
			}
			int requiredMedicineForImplanting = this.RequiredMedicineForImplanting;
			string arg2 = string.Empty;
			if (targetPawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine).Sum((Thing x) => x.stackCount) < requiredMedicineForImplanting)
			{
				arg2 = "XenogermOrderedImplantedMedicineNeeded".Translate(requiredMedicineForImplanting.Named("MEDICINENEEDED"));
			}
			Find.LetterStack.ReceiveLetter("LetterLabelXenogermOrderedImplanted".Translate(), "LetterXenogermOrderedImplanted".Translate(targetPawn.Named("PAWN"), requiredMedicineForImplanting.Named("MEDICINENEEDED"), arg.Named("BEDINFO"), arg2.Named("MEDICINEINFO")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x0600F51B RID: 62747 RVA: 0x004739D8 File Offset: 0x00471BD8
		public void Initialize(List<Genepack> genepacks, string xenotypeName, XenotypeIconDef iconDef)
		{
			this.xenotypeName = xenotypeName;
			this.iconDef = iconDef;
			this.geneSet = new GeneSet();
			for (int i = 0; i < genepacks.Count; i++)
			{
				if (genepacks[i].GeneSet != null)
				{
					List<GeneDef> genesListForReading = genepacks[i].GeneSet.GenesListForReading;
					for (int j = 0; j < genesListForReading.Count; j++)
					{
						this.geneSet.AddGene(genesListForReading[j]);
					}
				}
			}
		}

		// Token: 0x0600F51C RID: 62748 RVA: 0x00473A52 File Offset: 0x00471C52
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (ModsConfig.BiotechActive && selPawn.genes != null)
			{
				int num = GeneUtility.MetabolismAfterImplanting(selPawn, this.geneSet);
				if (num < GeneTuning.BiostatRange.TrueMin)
				{
					yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(selPawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "ResultingMetTooLow".Translate() + " (" + num.ToString() + ")"), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else if (this.PawnIdeoDisallowsImplanting(selPawn))
				{
					yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(selPawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "IdeoligionForbids".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return new FloatMenuOption("OrderImplantationIntoPawn".Translate(selPawn.Named("PAWN")) + " (" + this.xenotypeName + ")", delegate()
					{
						this.SetTargetPawn(selPawn);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F51D RID: 62749 RVA: 0x00473A6C File Offset: 0x00471C6C
		public bool PawnIdeoDisallowsImplanting(Pawn selPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, selPawn))
			{
				if (base.GeneSet.GenesListForReading.Any((GeneDef x) => x == GeneDefOf.Bloodfeeder))
				{
					return true;
				}
			}
			return !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, selPawn);
		}

		// Token: 0x0600F51E RID: 62750 RVA: 0x00473AD9 File Offset: 0x00471CD9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.geneSet != null)
			{
				if (this.targetPawn == null)
				{
					yield return new Command_Action
					{
						defaultLabel = "ImplantXenogerm".Translate() + "...",
						defaultDesc = "ImplantXenogermDesc".Translate(this.RequiredMedicineForImplanting.Named("MEDICINEAMOUNT")),
						icon = Xenogerm.ImplantTex.Texture,
						action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							foreach (Pawn pawn2 in base.Map.mapPawns.AllPawnsSpawned)
							{
								Pawn pawn = pawn2;
								if (!pawn.IsQuestLodger() && pawn.genes != null && (pawn.IsColonistPlayerControlled || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony || (pawn.IsColonySubhuman && pawn.IsGhoul)))
								{
									int num = GeneUtility.MetabolismAfterImplanting(pawn, this.geneSet);
									if (num < GeneTuning.BiostatRange.TrueMin)
									{
										list.Add(new FloatMenuOption(pawn.LabelShortCap + ": " + "ResultingMetTooLow".Translate() + " (" + num.ToString() + ")", null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
									else if (this.PawnIdeoDisallowsImplanting(pawn))
									{
										list.Add(new FloatMenuOption(pawn.LabelShortCap + ": " + "IdeoligionForbids".Translate(), null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
									else
									{
										list.Add(new FloatMenuOption(pawn.LabelShortCap + ", " + pawn.genes.XenotypeLabelCap, delegate()
										{
											this.SetTargetPawn(pawn);
										}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
									}
								}
							}
							if (!list.Any<FloatMenuOption>())
							{
								list.Add(new FloatMenuOption("NoImplantablePawns".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "CancelImplanting".Translate(),
						defaultDesc = "CancelImplantingDesc".Translate(this.targetPawn.Named("PAWN")),
						icon = Xenogerm.CancelIcon,
						action = delegate()
						{
							BillStack billStack = this.targetPawn.BillStack;
							Bill bill = (billStack != null) ? billStack.Bills.FirstOrDefault((Bill x) => x.xenogerm == this) : null;
							if (bill != null)
							{
								this.targetPawn.BillStack.Delete(bill);
							}
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F51F RID: 62751 RVA: 0x00473AE9 File Offset: 0x00471CE9
		public void Notify_BillRemoved()
		{
			this.targetPawn = null;
		}

		// Token: 0x0600F520 RID: 62752 RVA: 0x00473AF4 File Offset: 0x00471CF4
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			if (target.Pawn.IsQuestLodger())
			{
				if (showMessages)
				{
					Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.IsColonist && !target.Pawn.IsPrisonerOfColony && !target.Pawn.IsSlaveOfColony)
			{
				if (showMessages)
				{
					Messages.Message("MessageCanOnlyTargetColonistsPrisonersAndSlaves".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600F521 RID: 62753 RVA: 0x00473BA4 File Offset: 0x00471DA4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.targetPawn != null && this.targetPawn.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.targetPawn.TrueCenter());
			}
		}

		// Token: 0x0600F522 RID: 62754 RVA: 0x00473BE0 File Offset: 0x00471DE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.targetPawn, "targetPawn", false);
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.iconDef == null)
			{
				this.iconDef = XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x0400A175 RID: 41333
		private Pawn targetPawn;

		// Token: 0x0400A176 RID: 41334
		public string xenotypeName;

		// Token: 0x0400A177 RID: 41335
		public XenotypeIconDef iconDef;

		// Token: 0x0400A178 RID: 41336
		private static readonly CachedTexture ImplantTex = new CachedTexture("UI/Gizmos/ImplantGenes");

		// Token: 0x0400A179 RID: 41337
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
