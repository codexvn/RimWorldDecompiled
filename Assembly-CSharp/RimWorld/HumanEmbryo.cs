using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ACB RID: 10955
	[StaticConstructorOnStartup]
	public class HumanEmbryo : GeneSetHolderBase
	{
		// Token: 0x17002627 RID: 9767
		// (get) Token: 0x0600F40C RID: 62476 RVA: 0x0046EB01 File Offset: 0x0046CD01
		public Pawn Mother
		{
			get
			{
				List<Pawn> pawnSources = this.TryGetComp<CompHasPawnSources>().pawnSources;
				if (pawnSources == null)
				{
					return null;
				}
				return pawnSources.FirstOrDefault((Pawn p) => p.gender == Gender.Female);
			}
		}

		// Token: 0x17002628 RID: 9768
		// (get) Token: 0x0600F40D RID: 62477 RVA: 0x0046EB38 File Offset: 0x0046CD38
		public Pawn Father
		{
			get
			{
				List<Pawn> pawnSources = this.TryGetComp<CompHasPawnSources>().pawnSources;
				if (pawnSources == null)
				{
					return null;
				}
				return pawnSources.FirstOrDefault((Pawn p) => p.gender == Gender.Male);
			}
		}

		// Token: 0x17002629 RID: 9769
		// (get) Token: 0x0600F40E RID: 62478 RVA: 0x0046EB6F File Offset: 0x0046CD6F
		protected override string InspectGeneDescription
		{
			get
			{
				return "InspectGenesEmbryoDesc".Translate();
			}
		}

		// Token: 0x0600F40F RID: 62479 RVA: 0x0046EB80 File Offset: 0x0046CD80
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("human embryo"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
		}

		// Token: 0x0600F410 RID: 62480 RVA: 0x0046EB9C File Offset: 0x0046CD9C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				yield break;
			}
			if (base.MapHeld != null)
			{
				this.EnsureImplantTargetValid();
				if (this.implantTarget == null)
				{
					List<FloatMenuOption> surrogateOptions = new List<FloatMenuOption>();
					foreach (Pawn pawn in base.MapHeld.mapPawns.FreeColonistsAndPrisonersSpawned)
					{
						FloatMenuOption floatMenuOption = this.CanImplantFloatOption(pawn, true);
						if (floatMenuOption != null)
						{
							surrogateOptions.Add(floatMenuOption);
						}
					}
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "ImplantLabel".Translate() + "...",
						defaultDesc = "ImplantDescription".Translate(),
						icon = HumanEmbryo.ImplantIcon.Texture,
						action = delegate()
						{
							Find.WindowStack.Add(new FloatMenu(surrogateOptions));
						}
					};
					if (surrogateOptions.Count == 0)
					{
						command_Action.Disable("ImplantDisabledNoWomen".Translate());
					}
					yield return command_Action;
					Building_GrowthVat bestVat = this.BestAvailableGrowthVat();
					Command_Action command_Action2 = new Command_Action
					{
						defaultLabel = "InsertGrowthVatLabel".Translate() + "...",
						defaultDesc = "InsertEmbryoGrowthVatDesc".Translate(540000.ToStringTicksToPeriod(true, false, true, true, false)).Resolve(),
						icon = Building_GrowthVat.InsertEmbryoIcon.Texture,
						activateSound = SoundDefOf.Tick_Tiny,
						action = delegate()
						{
							bestVat.SelectEmbryo(this);
							this.implantTarget = bestVat;
						}
					};
					if (bestVat == null)
					{
						command_Action2.Disable("ImplantDisabledNoVats".Translate());
					}
					yield return command_Action2;
				}
				else if (base.Spawned)
				{
					yield return new Command_Action
					{
						defaultLabel = "ImplantEmbryoCancel".Translate(),
						defaultDesc = "ImplantEmbryoCancel".Translate(),
						icon = HumanEmbryo.CancelImplantIcon,
						action = delegate()
						{
							this.ImplantPawnValid(true);
							this.ImplantVatValid(true);
							this.implantTarget = null;
						},
						activateSound = SoundDefOf.Designate_Cancel
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F411 RID: 62481 RVA: 0x0046EBAC File Offset: 0x0046CDAC
		private void EnsureImplantTargetValid()
		{
			if (this.implantTarget == null)
			{
				return;
			}
			Pawn pawn = this.implantTarget as Pawn;
			if (pawn != null)
			{
				if (pawn.Destroyed || !this.ImplantPawnValid(false))
				{
					this.implantTarget = null;
					return;
				}
			}
			else
			{
				Building_GrowthVat building_GrowthVat = this.implantTarget as Building_GrowthVat;
				if (building_GrowthVat != null && (building_GrowthVat.Destroyed || !this.ImplantVatValid(false)))
				{
					this.implantTarget = null;
				}
			}
		}

		// Token: 0x0600F412 RID: 62482 RVA: 0x0046EC14 File Offset: 0x0046CE14
		private bool ImplantPawnValid(bool cancel)
		{
			Pawn pawn = this.implantTarget as Pawn;
			if (pawn != null)
			{
				foreach (Bill bill in pawn.BillStack)
				{
					Bill_Medical bill_Medical = bill as Bill_Medical;
					if (bill_Medical != null && !bill_Medical.uniqueRequiredIngredients.NullOrEmpty<Thing>())
					{
						foreach (Thing thing in bill_Medical.uniqueRequiredIngredients)
						{
							if (this == thing)
							{
								if (cancel)
								{
									bill_Medical.billStack.Delete(bill_Medical);
									this.implantTarget = null;
									return false;
								}
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600F413 RID: 62483 RVA: 0x0046ECE8 File Offset: 0x0046CEE8
		private bool ImplantVatValid(bool cancel)
		{
			Building_GrowthVat building_GrowthVat = this.implantTarget as Building_GrowthVat;
			if (building_GrowthVat == null)
			{
				return false;
			}
			if (cancel)
			{
				building_GrowthVat.selectedEmbryo = null;
				this.implantTarget = null;
				return false;
			}
			return building_GrowthVat.selectedEmbryo == this;
		}

		// Token: 0x0600F414 RID: 62484 RVA: 0x0046ED24 File Offset: 0x0046CF24
		private Building_GrowthVat BestAvailableGrowthVat()
		{
			if (base.MapHeld == null)
			{
				return null;
			}
			List<Building> list = base.MapHeld.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GrowthVat);
			this.tmpEligibleVats.Clear();
			foreach (Building building in list)
			{
				Building_GrowthVat building_GrowthVat = building as Building_GrowthVat;
				if (building_GrowthVat != null && !building_GrowthVat.Working && building_GrowthVat.selectedEmbryo == null && building_GrowthVat.SelectedPawn == null)
				{
					this.tmpEligibleVats.Add(building_GrowthVat);
				}
			}
			if (this.tmpEligibleVats.NullOrEmpty<Building_GrowthVat>())
			{
				return null;
			}
			this.tmpEligibleVats.SortBy(delegate(Building_GrowthVat v)
			{
				if (!v.PowerOn)
				{
					return 1;
				}
				return 0;
			}, (Building_GrowthVat v) => (base.PositionHeld - v.Position).LengthHorizontal);
			return this.tmpEligibleVats[0];
		}

		// Token: 0x0600F415 RID: 62485 RVA: 0x0046EE14 File Offset: 0x0046D014
		private FloatMenuOption CanImplantFloatOption(Pawn pawn, bool pawnAsLabel)
		{
			AcceptanceReport acceptanceReport = this.CanImplantReport(pawn);
			if (acceptanceReport.Accepted)
			{
				Action <>9__1;
				return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "ImplantLabel".Translate().ToString(), delegate()
				{
					float num = PregnancyUtility.PregnancyChanceImplantEmbryo(pawn);
					if (num < 1f)
					{
						TaggedString text = "ImplantSurgeryConfirm".Translate(this.Label, pawn.LabelShort, num.ToStringPercent());
						Action confirmedAct;
						if ((confirmedAct = <>9__1) == null)
						{
							confirmedAct = (<>9__1 = delegate()
							{
								HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing>
								{
									this
								}, true);
								this.implantTarget = pawn;
							});
						}
						Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog);
						Find.WindowStack.Add(window);
						return;
					}
					HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing>
					{
						this
					}, true);
					this.implantTarget = pawn;
				}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("ImplantCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600F416 RID: 62486 RVA: 0x0046EF30 File Offset: 0x0046D130
		private AcceptanceReport CanImplantReport(Pawn pawn)
		{
			if (pawn.gender != Gender.Female)
			{
				return false;
			}
			if (pawn.IsQuestLodger())
			{
				return false;
			}
			HashSet<Pawn> hashSet = new HashSet<Pawn>();
			base.Map.reservationManager.ReserversOf(pawn, hashSet);
			if (hashSet.Any<Pawn>())
			{
				Pawn pawn2 = hashSet.First<Pawn>();
				return "ReservedBy".Translate(pawn2.LabelShort, pawn2);
			}
			if (pawn.BillStack.Bills.Any((Bill b) => b.recipe == RecipeDefOf.ImplantEmbryo))
			{
				return "CannotImplantingOtherEmbryo".Translate();
			}
			if (pawn.ageTracker.AgeBiologicalYears < 16)
			{
				return "CannotMustBeAge".Translate(16).CapitalizeFirst();
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false))
			{
				return "CannotPregnant".Translate();
			}
			if (pawn.Sterile())
			{
				return "CannotSterile".Translate();
			}
			return true;
		}

		// Token: 0x0600F417 RID: 62487 RVA: 0x0046F060 File Offset: 0x0046D260
		public bool TryPopulateGenes()
		{
			bool result = true;
			if (this.geneSet == null)
			{
				this.geneSet = PregnancyUtility.GetInheritedGeneSet(this.Father, this.Mother, out result);
			}
			return result;
		}

		// Token: 0x0600F418 RID: 62488 RVA: 0x0046F094 File Offset: 0x0046D294
		public override void Notify_DebugSpawned()
		{
			CompHasPawnSources compHasPawnSources = this.TryGetComp<CompHasPawnSources>();
			Pawn pawn;
			if ((from x in base.Map.mapPawns.AllPawns
			where x.RaceProps.Humanlike && x.gender == Gender.Male
			select x).TryRandomElement(out pawn))
			{
				compHasPawnSources.AddSource(pawn);
			}
			Pawn pawn2;
			if ((from x in base.Map.mapPawns.AllPawns
			where x.RaceProps.Humanlike && x.gender == Gender.Female
			select x).TryRandomElement(out pawn2))
			{
				compHasPawnSources.AddSource(pawn2);
			}
			this.geneSet = PregnancyUtility.GetInheritedGeneSet(this.Father, this.Mother);
		}

		// Token: 0x0600F419 RID: 62489 RVA: 0x0046F147 File Offset: 0x0046D347
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.implantTarget, "implantTarget", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.geneSet == null)
			{
				this.TryPopulateGenes();
			}
		}

		// Token: 0x0400A0E3 RID: 41187
		private const int MinImplantationAge = 16;

		// Token: 0x0400A0E4 RID: 41188
		public static readonly CachedTexture ImplantIcon = new CachedTexture("UI/Gizmos/ImplantEmbryo");

		// Token: 0x0400A0E5 RID: 41189
		private static readonly Texture2D CancelImplantIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400A0E6 RID: 41190
		public Thing implantTarget;

		// Token: 0x0400A0E7 RID: 41191
		private List<Building_GrowthVat> tmpEligibleVats = new List<Building_GrowthVat>();
	}
}
