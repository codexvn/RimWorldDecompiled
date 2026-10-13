using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A04 RID: 10756
	[StaticConstructorOnStartup]
	public class Building_GeneExtractor : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x170024DB RID: 9435
		// (get) Token: 0x0600EDEA RID: 60906 RVA: 0x00450843 File Offset: 0x0044EA43
		private Pawn ContainedPawn
		{
			get
			{
				return this.innerContainer.FirstOrDefault<Thing>() as Pawn;
			}
		}

		// Token: 0x170024DC RID: 9436
		// (get) Token: 0x0600EDEB RID: 60907 RVA: 0x00450855 File Offset: 0x0044EA55
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x170024DD RID: 9437
		// (get) Token: 0x0600EDEC RID: 60908 RVA: 0x00002501 File Offset: 0x00000701
		public override bool IsContentsSuspended
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170024DE RID: 9438
		// (get) Token: 0x0600EDED RID: 60909 RVA: 0x00450862 File Offset: 0x0044EA62
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				if (this.cachedPowerComp == null)
				{
					this.cachedPowerComp = this.TryGetComp<CompPowerTrader>();
				}
				return this.cachedPowerComp;
			}
		}

		// Token: 0x170024DF RID: 9439
		// (get) Token: 0x0600EDEE RID: 60910 RVA: 0x0045087E File Offset: 0x0044EA7E
		public Texture2D InsertPawnTex
		{
			get
			{
				if (this.cachedInsertPawnTex == null)
				{
					this.cachedInsertPawnTex = ContentFinder<Texture2D>.Get("UI/Gizmos/InsertPawn", true);
				}
				return this.cachedInsertPawnTex;
			}
		}

		// Token: 0x170024E0 RID: 9440
		// (get) Token: 0x0600EDEF RID: 60911 RVA: 0x004508A5 File Offset: 0x0044EAA5
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x170024E1 RID: 9441
		// (get) Token: 0x0600EDF0 RID: 60912 RVA: 0x004508B8 File Offset: 0x0044EAB8
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.Opposite.AsAngle;
			}
		}

		// Token: 0x170024E2 RID: 9442
		// (get) Token: 0x0600EDF1 RID: 60913 RVA: 0x00075162 File Offset: 0x00073362
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x170024E3 RID: 9443
		// (get) Token: 0x0600EDF2 RID: 60914 RVA: 0x004508DC File Offset: 0x0044EADC
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return IntVec3.West.RotatedBy(base.Rotation).ToVector3() / (float)this.def.size.x;
			}
		}

		// Token: 0x0600EDF3 RID: 60915 RVA: 0x00450917 File Offset: 0x0044EB17
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("gene extractor"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600EDF4 RID: 60916 RVA: 0x00450933 File Offset: 0x0044EB33
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			this.sustainerWorking = null;
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600EDF5 RID: 60917 RVA: 0x00450960 File Offset: 0x0044EB60
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.PowerTraderComp.PowerOutput = (base.Working ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			if (base.Working)
			{
				if (this.ContainedPawn == null)
				{
					this.Cancel();
					return;
				}
				if (this.PowerTraderComp.PowerOn)
				{
					this.TickEffects();
					if (this.PowerOn)
					{
						this.ticksRemaining--;
					}
					if (this.ticksRemaining <= 0)
					{
						this.Finish();
						return;
					}
				}
				else
				{
					this.powerCutTicks++;
					if (this.powerCutTicks >= 60000)
					{
						Pawn containedPawn = this.ContainedPawn;
						if (containedPawn != null)
						{
							Messages.Message("GeneExtractorNoPowerEjectedMessage".Translate(containedPawn.Named("PAWN")), containedPawn, MessageTypeDefOf.NegativeEvent, false);
						}
						this.Cancel();
						return;
					}
				}
			}
			else
			{
				if (this.selectedPawn != null && this.selectedPawn.Dead)
				{
					this.Cancel();
				}
				if (this.progressBar != null)
				{
					this.progressBar.Cleanup();
					this.progressBar = null;
				}
			}
		}

		// Token: 0x0600EDF6 RID: 60918 RVA: 0x00450A9C File Offset: 0x0044EC9C
		private void TickEffects()
		{
			if (this.sustainerWorking == null || this.sustainerWorking.Ended)
			{
				this.sustainerWorking = SoundDefOf.GeneExtractor_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
			else
			{
				this.sustainerWorking.Maintain();
			}
			if (this.progressBar == null)
			{
				this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			this.progressBar.EffectTick(new TargetInfo(base.Position + IntVec3.North.RotatedBy(base.Rotation), base.Map, false), TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = 1f - Mathf.Clamp01((float)this.ticksRemaining / 30000f);
				mote.offsetZ = ((base.Rotation == Rot4.North) ? 0.5f : -0.5f);
			}
		}

		// Token: 0x0600EDF7 RID: 60919 RVA: 0x00450B98 File Offset: 0x0044ED98
		public override AcceptanceReport CanAcceptPawn(Pawn pawn)
		{
			if (!pawn.IsColonist && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony && (!pawn.IsColonySubhuman || !pawn.IsGhoul))
			{
				return false;
			}
			if (this.selectedPawn != null && this.selectedPawn != pawn)
			{
				return false;
			}
			if (!pawn.RaceProps.Humanlike || pawn.IsQuestLodger())
			{
				return false;
			}
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.innerContainer.Count > 0)
			{
				return "Occupied".Translate();
			}
			if (pawn.genes != null)
			{
				if (pawn.genes.GenesListForReading.Any((Gene x) => x.def.passOnDirectly))
				{
					if (!pawn.genes.GenesListForReading.Any((Gene x) => x.def.biostatArc == 0))
					{
						return "PawnHasNoNonArchiteGenes".Translate(pawn.Named("PAWN"));
					}
					if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogerminationComa, false))
					{
						return "InXenogerminationComa".Translate();
					}
					return true;
				}
			}
			return "PawnHasNoGenes".Translate(pawn.Named("PAWN"));
		}

		// Token: 0x0600EDF8 RID: 60920 RVA: 0x00450D14 File Offset: 0x0044EF14
		private void Cancel()
		{
			this.startTick = -1;
			this.selectedPawn = null;
			this.sustainerWorking = null;
			this.powerCutTicks = 0;
			this.innerContainer.TryDropAll(this.def.hasInteractionCell ? this.InteractionCell : base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600EDF9 RID: 60921 RVA: 0x00450D70 File Offset: 0x0044EF70
		private void Finish()
		{
			this.selectedPawn = null;
			this.sustainerWorking = null;
			this.powerCutTicks = 0;
			if (this.ContainedPawn != null)
			{
				Pawn containedPawn = this.ContainedPawn;
				Rand.PushState(containedPawn.thingIDNumber ^ this.startTick);
				Building_GeneExtractor.<>c__DisplayClass34_0 CS$<>8__locals1 = new Building_GeneExtractor.<>c__DisplayClass34_0();
				CS$<>8__locals1.genesToAdd = new List<GeneDef>();
				Genepack genepack = (Genepack)ThingMaker.MakeThing(ThingDefOf.Genepack, null);
				int num = Mathf.Min((int)Building_GeneExtractor.GeneCountChanceCurve.RandomElementByWeight((CurvePoint p) => p.y).x, containedPawn.genes.GenesListForReading.Count((Gene x) => x.def.biostatArc == 0));
				int num2 = 0;
				Gene gene;
				while (num2 < num && containedPawn.genes.GenesListForReading.TryRandomElementByWeight(new Func<Gene, float>(CS$<>8__locals1.<Finish>g__SelectionWeight|0), out gene))
				{
					CS$<>8__locals1.genesToAdd.Add(gene.def);
					num2++;
				}
				if (CS$<>8__locals1.genesToAdd.Any<GeneDef>())
				{
					genepack.Initialize(CS$<>8__locals1.genesToAdd);
				}
				GeneUtility.ExtractXenogerm(containedPawn, Mathf.RoundToInt(60000f * GeneTuning.GeneExtractorRegrowingDurationDaysRange.RandomInRange));
				IntVec3 intVec = this.def.hasInteractionCell ? this.InteractionCell : base.Position;
				this.innerContainer.TryDropAll(intVec, base.Map, ThingPlaceMode.Near, null, null, true);
				if (!containedPawn.Dead && (containedPawn.IsPrisonerOfColony || containedPawn.IsSlaveOfColony))
				{
					Pawn_NeedsTracker needs = containedPawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							ThoughtHandler thoughts = mood.thoughts;
							if (thoughts != null)
							{
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories != null)
								{
									memories.TryGainMemory(ThoughtDefOf.XenogermHarvested_Prisoner, null, null);
								}
							}
						}
					}
				}
				if (CS$<>8__locals1.genesToAdd.Any<GeneDef>())
				{
					GenPlace.TryPlaceThing(genepack, intVec, base.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				Messages.Message("GeneExtractionComplete".Translate(containedPawn.Named("PAWN")) + ": " + (from x in CS$<>8__locals1.genesToAdd
				select x.label).ToCommaList(false, false).CapitalizeFirst(), new LookTargets(new TargetInfo[]
				{
					containedPawn,
					genepack
				}), MessageTypeDefOf.PositiveEvent, true);
				Rand.PopState();
			}
			this.startTick = -1;
		}

		// Token: 0x0600EDFA RID: 60922 RVA: 0x00451008 File Offset: 0x0044F208
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (this.CanAcceptPawn(pawn))
			{
				this.selectedPawn = pawn;
				bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.innerContainer.TryAddOrTransfer(pawn, true))
				{
					this.startTick = Find.TickManager.TicksGame;
					this.ticksRemaining = 30000;
				}
				if (flag)
				{
					Find.Selector.Select(pawn, false, false);
				}
			}
		}

		// Token: 0x0600EDFB RID: 60923 RVA: 0x0045106C File Offset: 0x0044F26C
		protected override void SelectPawn(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmExtractXenogermWillKill".Translate(pawn.Named("PAWN")), delegate()
				{
					this.<>n__0(pawn);
				}, false, null, WindowLayer.Dialog));
				return;
			}
			base.SelectPawn(pawn);
		}

		// Token: 0x0600EDFC RID: 60924 RVA: 0x004510EF File Offset: 0x0044F2EF
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				AcceptanceReport acceptanceReport = this.CanAcceptPawn(selPawn);
				if (acceptanceReport.Accepted)
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate()
					{
						this.SelectPawn(selPawn);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
				}
				else if (base.SelectedPawn == selPawn && !selPawn.IsPrisonerOfColony)
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
				}
				else if (!acceptanceReport.Reason.NullOrEmpty())
				{
					yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EDFD RID: 60925 RVA: 0x00451106 File Offset: 0x0044F306
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Working)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelExtraction".Translate(),
					defaultDesc = "CommandCancelExtractionDesc".Translate(),
					icon = Building_GeneExtractor.CancelIcon,
					action = delegate()
					{
						this.Cancel();
					},
					activateSound = SoundDefOf.Designate_Cancel
				};
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Finish extraction",
						action = delegate()
						{
							this.Finish();
						}
					};
				}
			}
			else if (this.selectedPawn != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelLoad".Translate(),
					defaultDesc = "CommandCancelLoadDesc".Translate(),
					icon = Building_GeneExtractor.CancelIcon,
					activateSound = SoundDefOf.Designate_Cancel,
					action = delegate()
					{
						this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
						if (this.selectedPawn.CurJobDef == JobDefOf.EnterBuilding)
						{
							this.selectedPawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						this.selectedPawn = null;
						this.startTick = -1;
						this.sustainerWorking = null;
					}
				};
			}
			else
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "InsertPerson".Translate() + "...";
				command_Action.defaultDesc = "InsertPersonGeneExtractorDesc".Translate();
				command_Action.icon = this.InsertPawnTex;
				command_Action.action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Pawn pawn2 in base.Map.mapPawns.AllPawnsSpawned.OrderBy(delegate(Pawn p)
					{
						if (p.IsColonist)
						{
							return 0;
						}
						if (p.IsPrisonerOfColony || p.IsSlaveOfColony)
						{
							return 1;
						}
						return 2;
					}).ThenBy((Pawn p) => p.Label))
					{
						Pawn pawn = pawn2;
						if (pawn.genes != null)
						{
							AcceptanceReport acceptanceReport = this.CanAcceptPawn(pawn);
							string text = pawn.LabelShortCap + ", " + pawn.genes.XenotypeLabelCap;
							if (!acceptanceReport.Accepted)
							{
								if (!acceptanceReport.Reason.NullOrEmpty())
								{
									list.Add(new FloatMenuOption(text + ": " + acceptanceReport.Reason, null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
							else
							{
								Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating, false);
								if (firstHediffOfDef != null)
								{
									text = string.Concat(new string[]
									{
										text,
										" (",
										firstHediffOfDef.LabelBase,
										", ",
										firstHediffOfDef.TryGetComp<HediffComp_Disappears>().ticksToDisappear.ToStringTicksToPeriod(true, true, true, true, false).Colorize(ColoredText.SubtleGrayColor),
										")"
									});
								}
								list.Add(new FloatMenuOption(text, delegate()
								{
									this.SelectPawn(pawn);
								}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
					}
					if (!list.Any<FloatMenuOption>())
					{
						list.Add(new FloatMenuOption("NoExtractablePawns".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				if (!this.PowerOn)
				{
					command_Action.Disable("NoPower".Translate().CapitalizeFirst());
				}
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EDFE RID: 60926 RVA: 0x00451118 File Offset: 0x0044F318
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			if (base.Working && this.ContainedPawn != null)
			{
				this.ContainedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + this.PawnDrawOffset, null, true);
			}
		}

		// Token: 0x0600EDFF RID: 60927 RVA: 0x0045116C File Offset: 0x0044F36C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.selectedPawn != null && this.innerContainer.Count == 0)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "WaitingForPawn".Translate(this.selectedPawn.Named("PAWN")).Resolve();
			}
			else if (base.Working && this.ContainedPawn != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + "ExtractingXenogermFrom".Translate(this.ContainedPawn.Named("PAWN")).Resolve() + "\n";
				if (this.PowerOn)
				{
					text += "DurationLeft".Translate(this.ticksRemaining.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				}
				else
				{
					text += "ExtractionPausedNoPower".Translate((60000 - this.powerCutTicks).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Colorize(ColorLibrary.RedReadable);
				}
			}
			return text;
		}

		// Token: 0x0600EE00 RID: 60928 RVA: 0x004512A3 File Offset: 0x0044F4A3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksRemaining, "ticksRemaining", 0, false);
			Scribe_Values.Look<int>(ref this.powerCutTicks, "powerCutTicks", 0, false);
		}

		// Token: 0x04009D69 RID: 40297
		private int ticksRemaining;

		// Token: 0x04009D6A RID: 40298
		private int powerCutTicks;

		// Token: 0x04009D6B RID: 40299
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04009D6C RID: 40300
		[Unsaved(false)]
		private Texture2D cachedInsertPawnTex;

		// Token: 0x04009D6D RID: 40301
		[Unsaved(false)]
		private Sustainer sustainerWorking;

		// Token: 0x04009D6E RID: 40302
		[Unsaved(false)]
		private Effecter progressBar;

		// Token: 0x04009D6F RID: 40303
		private const int TicksToExtract = 30000;

		// Token: 0x04009D70 RID: 40304
		private const int NoPowerEjectCumulativeTicks = 60000;

		// Token: 0x04009D71 RID: 40305
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04009D72 RID: 40306
		private static readonly SimpleCurve GeneCountChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.7f),
				true
			},
			{
				new CurvePoint(2f, 0.2f),
				true
			},
			{
				new CurvePoint(3f, 0.08f),
				true
			},
			{
				new CurvePoint(4f, 0.02f),
				true
			}
		};
	}
}
