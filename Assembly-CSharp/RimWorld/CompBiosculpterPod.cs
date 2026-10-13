using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C11 RID: 11281
	[StaticConstructorOnStartup]
	public class CompBiosculpterPod : ThingComp, ISuspendableThingHolder, IThingHolder, IThingHolderWithDrawnPawn, IStoreSettingsParent, INotifyHauledTo, ISearchableContents
	{
		// Token: 0x170027BB RID: 10171
		// (get) Token: 0x0600FCC6 RID: 64710 RVA: 0x004948B3 File Offset: 0x00492AB3
		public CompProperties_BiosculpterPod Props
		{
			get
			{
				return this.props as CompProperties_BiosculpterPod;
			}
		}

		// Token: 0x170027BC RID: 10172
		// (get) Token: 0x0600FCC7 RID: 64711 RVA: 0x004948C0 File Offset: 0x00492AC0
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x170027BD RID: 10173
		// (get) Token: 0x0600FCC8 RID: 64712 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170027BE RID: 10174
		// (get) Token: 0x0600FCC9 RID: 64713 RVA: 0x004948C8 File Offset: 0x00492AC8
		public float RequiredNutritionRemaining
		{
			get
			{
				return Mathf.Max(5f - this.liquifiedNutrition, 0f);
			}
		}

		// Token: 0x170027BF RID: 10175
		// (get) Token: 0x0600FCCA RID: 64714 RVA: 0x004948E0 File Offset: 0x00492AE0
		public bool NutritionLoaded
		{
			get
			{
				return this.RequiredNutritionRemaining <= 0f;
			}
		}

		// Token: 0x170027C0 RID: 10176
		// (get) Token: 0x0600FCCB RID: 64715 RVA: 0x004948F2 File Offset: 0x00492AF2
		public bool AutoAgeReversal
		{
			get
			{
				return this.autoAgeReversal;
			}
		}

		// Token: 0x170027C1 RID: 10177
		// (get) Token: 0x0600FCCC RID: 64716 RVA: 0x004948FA File Offset: 0x00492AFA
		private Texture2D AutoAgeReversalIcon
		{
			get
			{
				if (this.cachedAutoAgeReverseIcon == null)
				{
					this.cachedAutoAgeReverseIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/BiosculpterAutoAgeReversal", true);
				}
				return this.cachedAutoAgeReverseIcon;
			}
		}

		// Token: 0x170027C2 RID: 10178
		// (get) Token: 0x0600FCCD RID: 64717 RVA: 0x00494921 File Offset: 0x00492B21
		public BiosculpterPodState State
		{
			get
			{
				if (this.Occupant != null)
				{
					return BiosculpterPodState.Occupied;
				}
				if (this.NutritionLoaded)
				{
					return BiosculpterPodState.SelectingCycle;
				}
				return BiosculpterPodState.LoadingNutrition;
			}
		}

		// Token: 0x170027C3 RID: 10179
		// (get) Token: 0x0600FCCE RID: 64718 RVA: 0x00494938 File Offset: 0x00492B38
		public Pawn Occupant
		{
			get
			{
				if (this.pawnEnteringBiosculpter != null)
				{
					return this.pawnEnteringBiosculpter;
				}
				if (this.currentCycleKey == null)
				{
					return null;
				}
				if (this.innerContainer.Count != 1)
				{
					return null;
				}
				return this.innerContainer[0] as Pawn;
			}
		}

		// Token: 0x170027C4 RID: 10180
		// (get) Token: 0x0600FCCF RID: 64719 RVA: 0x00494974 File Offset: 0x00492B74
		public CompBiosculpterPod_Cycle CurrentCycle
		{
			get
			{
				if (this.currentCycleKey == null)
				{
					return null;
				}
				foreach (CompBiosculpterPod_Cycle compBiosculpterPod_Cycle in this.AvailableCycles)
				{
					if (compBiosculpterPod_Cycle.Props.key == this.currentCycleKey)
					{
						return compBiosculpterPod_Cycle;
					}
				}
				return null;
			}
		}

		// Token: 0x170027C5 RID: 10181
		// (get) Token: 0x0600FCD0 RID: 64720 RVA: 0x004949EC File Offset: 0x00492BEC
		public List<CompBiosculpterPod_Cycle> AvailableCycles
		{
			get
			{
				if (this.cachedAvailableCycles == null)
				{
					this.SetupCycleCaches();
				}
				return this.cachedAvailableCycles;
			}
		}

		// Token: 0x170027C6 RID: 10182
		// (get) Token: 0x0600FCD1 RID: 64721 RVA: 0x00494A02 File Offset: 0x00492C02
		public string AgeReversalCycleKey
		{
			get
			{
				if (CompBiosculpterPod.cachedAgeReversalCycleKey == null)
				{
					this.SetupCycleCaches();
				}
				return CompBiosculpterPod.cachedAgeReversalCycleKey;
			}
		}

		// Token: 0x170027C7 RID: 10183
		// (get) Token: 0x0600FCD2 RID: 64722 RVA: 0x00494A16 File Offset: 0x00492C16
		private float CycleSpeedFactorNoPawn
		{
			get
			{
				return this.CleanlinessSpeedFactor * this.BiotunedSpeedFactor;
			}
		}

		// Token: 0x170027C8 RID: 10184
		// (get) Token: 0x0600FCD3 RID: 64723 RVA: 0x00494A25 File Offset: 0x00492C25
		public float CycleSpeedFactor
		{
			get
			{
				if (this.Occupant == null)
				{
					return Mathf.Max(0.1f, this.CycleSpeedFactorNoPawn);
				}
				return this.GetCycleSpeedFactorForPawn(this.Occupant);
			}
		}

		// Token: 0x170027C9 RID: 10185
		// (get) Token: 0x0600FCD4 RID: 64724 RVA: 0x00494A4C File Offset: 0x00492C4C
		private float CleanlinessSpeedFactor
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.BiosculpterPodSpeedFactor, true, -1);
			}
		}

		// Token: 0x170027CA RID: 10186
		// (get) Token: 0x0600FCD5 RID: 64725 RVA: 0x00494A60 File Offset: 0x00492C60
		private float BiotunedSpeedFactor
		{
			get
			{
				if (this.biotunedTo == null)
				{
					return 1f;
				}
				return this.Props.biotunedCycleSpeedFactor;
			}
		}

		// Token: 0x170027CB RID: 10187
		// (get) Token: 0x0600FCD6 RID: 64726 RVA: 0x00494A7B File Offset: 0x00492C7B
		public bool PowerOn
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x170027CC RID: 10188
		// (get) Token: 0x0600FCD7 RID: 64727 RVA: 0x00494A8D File Offset: 0x00492C8D
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.parent.DrawPos.y - 0.03658537f;
			}
		}

		// Token: 0x170027CD RID: 10189
		// (get) Token: 0x0600FCD8 RID: 64728 RVA: 0x00494AA8 File Offset: 0x00492CA8
		public float HeldPawnBodyAngle
		{
			get
			{
				return this.parent.Rotation.Opposite.AsAngle;
			}
		}

		// Token: 0x170027CE RID: 10190
		// (get) Token: 0x0600FCD9 RID: 64729 RVA: 0x00075162 File Offset: 0x00073362
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x0600FCDA RID: 64730 RVA: 0x00494AD0 File Offset: 0x00492CD0
		public CompBiosculpterPod()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600FCDB RID: 64731 RVA: 0x00494B44 File Offset: 0x00492D44
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedNutritionSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600FCDC RID: 64732 RVA: 0x00494B9B File Offset: 0x00492D9B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Biosculpter pod comp"))
			{
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600FCDD RID: 64733 RVA: 0x00494BB4 File Offset: 0x00492DB4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.currentCycleKey, "currentCycleKey", null, false);
			Scribe_Values.Look<float>(ref this.currentCycleTicksRemaining, "currentCycleTicksRemaining", 0f, false);
			Scribe_Values.Look<int>(ref this.currentCyclePowerCutTicks, "currentCyclePowerCutTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.biotunedTo, "biotunedTo", false);
			Scribe_Values.Look<int>(ref this.biotunedCountdownTicks, "biotunedCountdownTicks", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedNutritionSettings, "allowedNutritionSettings", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.liquifiedNutrition, "liquifiedNutrition", 0f, false);
			Scribe_Values.Look<bool>(ref this.autoLoadNutrition, "autoLoadNutrition", false, false);
			Scribe_Values.Look<bool>(ref this.devFillPodLatch, "devFillPodLatch", false, false);
			Scribe_Values.Look<bool>(ref this.autoAgeReversal, "autoAgeReversal", false, false);
			Scribe_Values.Look<int>(ref this.tickEntered, "tickEntered", 0, false);
			Scribe_References.Look<Job>(ref this.queuedEnterJob, "queuedEnterJob", false);
			Scribe_References.Look<Pawn>(ref this.queuedPawn, "queuedPawn", false);
			if (this.allowedNutritionSettings == null)
			{
				this.allowedNutritionSettings = new StorageSettings(this);
				if (this.parent.def.building.defaultStorageSettings != null)
				{
					this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.currentCycleKey == "healing")
				{
					this.currentCycleKey = "medic";
				}
				if (this.biotunedTo != null)
				{
					this.SetBiotuned(this.biotunedTo);
				}
				this.LiquifyNutrition();
			}
		}

		// Token: 0x0600FCDE RID: 64734 RVA: 0x00494D5D File Offset: 0x00492F5D
		public CompBiosculpterPod_Cycle GetCycle(string key)
		{
			if (this.cycleLookup == null)
			{
				this.SetupCycleCaches();
			}
			return this.cycleLookup[key];
		}

		// Token: 0x0600FCDF RID: 64735 RVA: 0x00494D79 File Offset: 0x00492F79
		public float GetCycleSpeedFactorForPawn(Pawn p)
		{
			return Mathf.Max(0.1f, this.CycleSpeedFactorNoPawn * p.GetStatValue(StatDefOf.BiosculpterOccupantSpeed, true, -1));
		}

		// Token: 0x0600FCE0 RID: 64736 RVA: 0x00494D9C File Offset: 0x00492F9C
		private void SetupCycleCaches()
		{
			this.cachedAvailableCycles = new List<CompBiosculpterPod_Cycle>();
			this.cachedAvailableCycles.AddRange(this.parent.AllComps.OfType<CompBiosculpterPod_Cycle>());
			this.cycleLookup = new Dictionary<string, CompBiosculpterPod_Cycle>();
			foreach (CompBiosculpterPod_Cycle compBiosculpterPod_Cycle in this.cachedAvailableCycles)
			{
				CompBiosculpterPod_AgeReversalCycle compBiosculpterPod_AgeReversalCycle = compBiosculpterPod_Cycle as CompBiosculpterPod_AgeReversalCycle;
				if (compBiosculpterPod_AgeReversalCycle != null)
				{
					CompBiosculpterPod.cachedAgeReversalCycleKey = compBiosculpterPod_AgeReversalCycle.Props.key;
				}
				this.cycleLookup[compBiosculpterPod_Cycle.Props.key] = compBiosculpterPod_Cycle;
			}
		}

		// Token: 0x0600FCE1 RID: 64737 RVA: 0x00494E4C File Offset: 0x0049304C
		public void SetBiotuned(Pawn newBiotunedTo)
		{
			if (newBiotunedTo != this.biotunedTo)
			{
				this.autoAgeReversal = false;
			}
			if (this.biotunedTo != null && CompBiosculpterPod.cachedBiotunedPods.ContainsKey(this.biotunedTo))
			{
				CompBiosculpterPod.cachedBiotunedPods[this.biotunedTo].Remove(this);
			}
			if (newBiotunedTo != null && !CompBiosculpterPod.cachedBiotunedPods.ContainsKey(newBiotunedTo))
			{
				CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo] = new List<CompBiosculpterPod>();
			}
			if (newBiotunedTo != null && !CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo].Contains(this))
			{
				CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo].Add(this);
			}
			if (newBiotunedTo != null && newBiotunedTo != this.biotunedTo)
			{
				this.biotunedCountdownTicks = 4800000;
			}
			this.biotunedTo = newBiotunedTo;
		}

		// Token: 0x0600FCE2 RID: 64738 RVA: 0x00494EFF File Offset: 0x004930FF
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.SetBiotuned(null);
			if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
			{
				this.EjectContents(true, false, previousMap);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x0600FCE3 RID: 64739 RVA: 0x00494F30 File Offset: 0x00493130
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.EjectContents(true, false, map);
				this.currentCycleKey = null;
			}
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			Effecter effecter2 = this.operatingEffecter;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.operatingEffecter = null;
			Effecter effecter3 = this.readyEffecter;
			if (effecter3 != null)
			{
				effecter3.Cleanup();
			}
			this.readyEffecter = null;
		}

		// Token: 0x0600FCE4 RID: 64740 RVA: 0x00494F9C File Offset: 0x0049319C
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (Find.ScreenshotModeHandler.Active)
			{
				return;
			}
			if (this.biotunedTo != null || this.Occupant != null)
			{
				Thing parent = this.parent;
				Pawn pawn = this.biotunedTo;
				GenMapUI.DrawThingLabel(parent, ((pawn != null) ? pawn.LabelShort : null) ?? this.Occupant.LabelShort, GenMapUI.DefaultThingLabelColor);
			}
		}

		// Token: 0x0600FCE5 RID: 64741 RVA: 0x00495000 File Offset: 0x00493200
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			BiosculpterPodState state = this.State;
			if (this.parent.Spawned)
			{
				CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
				if (currentCycle != null)
				{
					stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterPodCycleLabel".Translate()).Append(": ").Append(currentCycle.Props.LabelCap);
					if (this.biotunedTo == null)
					{
						stringBuilder.Append(" " + "BiosculpterPodCycleWillBiotune".Translate());
					}
				}
				else if (state == BiosculpterPodState.SelectingCycle)
				{
					if (this.PowerOn)
					{
						if (this.queuedEnterJob != null && !this.queuedEnterJob.biosculpterCycleKey.NullOrEmpty())
						{
							stringBuilder.Append("BiosculpterPodCycleStandby".Translate(this.GetCycle(this.queuedEnterJob.biosculpterCycleKey).Props.label.Named("CYCLE"), this.queuedPawn.Named("PAWN")));
						}
						else
						{
							stringBuilder.Append("BiosculpterPodCycleSelection".Translate().CapitalizeFirst());
						}
					}
					else
					{
						stringBuilder.Append("BiosculpterPodCycleSelectionNoPower".Translate().CapitalizeFirst());
					}
				}
				if (state == BiosculpterPodState.LoadingNutrition)
				{
					stringBuilder.Append("BiosculpterPodCycleLabelLoading".Translate().CapitalizeFirst());
					stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ").Append(this.liquifiedNutrition.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute)).Append(" / ").Append(5f);
				}
				if (state == BiosculpterPodState.Occupied)
				{
					float num = this.currentCycleTicksRemaining / this.CycleSpeedFactor;
					stringBuilder.AppendLineIfNotEmpty().Append("Contains".Translate()).Append(": ").Append(this.Occupant.NameShortColored.Resolve());
					if (!this.PowerOn)
					{
						stringBuilder.AppendLine().Append("BiosculpterCycleNoPowerInterrupt".Translate((60000 - this.currentCyclePowerCutTicks).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Colorize(ColorLibrary.RedReadable));
					}
					stringBuilder.AppendLine().Append("BiosculpterCycleTimeRemaining".Translate()).Append(": ").Append(((int)num).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor));
					Ideo ideo = this.Occupant.Ideo;
					if (ideo != null && ideo.HasPrecept(PreceptDefOf.Biosculpting_Accelerated))
					{
						stringBuilder.Append(" (" + "BiosculpterCycleAccelerated".Translate() + ")");
					}
					if (this.biotunedTo != null)
					{
						stringBuilder.AppendLine().Append("BiosculpterBiotunedSpeedFactor".Translate()).Append(": ").Append(this.BiotunedSpeedFactor.ToStringPercent());
					}
					stringBuilder.AppendLine().Append("BiosculpterCleanlinessSpeedFactor".Translate()).Append(": ").Append(this.CleanlinessSpeedFactor.ToStringPercent());
				}
			}
			if (this.biotunedTo != null && state != BiosculpterPodState.Occupied)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterBiotunedTo".Translate()).Append(": ").Append(this.biotunedTo.LabelShort).Append(" (").Append(this.biotunedCountdownTicks.ToStringTicksToPeriod(true, false, true, true, false)).Append(")");
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600FCE6 RID: 64742 RVA: 0x004953CD File Offset: 0x004935CD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			BiosculpterPodState state = this.State;
			string cycleIndependentCannotUseReason = this.CannotUseNowReason();
			using (List<CompBiosculpterPod_Cycle>.Enumerator enumerator = this.AvailableCycles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBiosculpterPod_Cycle cycle = enumerator.Current;
					string text = cycleIndependentCannotUseReason ?? this.CannotUseNowCycleReason(cycle);
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "BiosculpterPodCycleCommand".Translate(cycle.Props.label) + ((this.biotunedTo != null) ? (" (" + this.biotunedTo.LabelShort + ")") : "");
					command_Action.defaultDesc = this.CycleDescription(cycle);
					command_Action.icon = cycle.Props.Icon;
					command_Action.action = delegate()
					{
						List<FloatMenuOption> list2;
						this.SelectPawnsForCycleOptions(cycle, out list2, false);
						if (this.biotunedTo != null && list2.Count > 0)
						{
							list2[0].action();
							if (!(cycle is CompBiosculpterPod_HealingCycle))
							{
								Messages.Message("BiosculpterEnteringMessage".Translate(this.biotunedTo.Named("PAWN"), cycle.Props.label.Named("CYCLE")).CapitalizeFirst(), this.parent, MessageTypeDefOf.SilentInput, false);
							}
							return;
						}
						Find.WindowStack.Add(new FloatMenu(list2));
					};
					command_Action.activateSound = SoundDefOf.Tick_Tiny;
					command_Action.Disabled = (text != null);
					List<FloatMenuOption> list;
					if (text != null)
					{
						command_Action.Disable(text);
					}
					else if (!this.SelectPawnsForCycleOptions(cycle, out list, true))
					{
						command_Action.Disable((this.biotunedTo != null) ? "BiosculpterNoEligiblePawnsBiotuned".Translate(this.biotunedTo.Named("PAWN")) : "BiosculpterNoEligiblePawns".Translate());
					}
					yield return command_Action;
				}
			}
			List<CompBiosculpterPod_Cycle>.Enumerator enumerator = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			if (state == BiosculpterPodState.Occupied)
			{
				yield return new Command_Action
				{
					defaultLabel = "BiosculpterInteruptCycle".Translate(),
					defaultDesc = "BiosculpterInteruptCycleDesc".Translate(),
					icon = CompBiosculpterPod.InterruptCycleIcon,
					action = delegate()
					{
						this.EjectContents(true, true, null);
					},
					activateSound = SoundDefOf.Designate_Cancel
				};
			}
			yield return new Command_Toggle
			{
				defaultLabel = "BiosculpterAutoLoadNutritionLabel".Translate(),
				defaultDesc = "BiosculpterAutoLoadNutritionDescription".Translate(),
				icon = (this.autoLoadNutrition ? TexCommand.ForbidOff : TexCommand.ForbidOn),
				isActive = (() => this.autoLoadNutrition),
				toggleAction = delegate()
				{
					this.autoLoadNutrition = !this.autoLoadNutrition;
				}
			};
			Pawn pawn = this.biotunedTo;
			bool? flag;
			if (pawn == null)
			{
				flag = null;
			}
			else
			{
				Ideo ideo = pawn.Ideo;
				flag = ((ideo != null) ? new bool?(ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded)) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				Command_Toggle command_Toggle = new Command_Toggle();
				command_Toggle.defaultLabel = "BiosculpterAutoAgeReversalLabel".Translate(this.biotunedTo.Named("PAWN"));
				TaggedString taggedString = (this.biotunedTo.ageTracker.AgeReversalDemandedDeadlineTicks > 0L) ? "BiosculpterAutoAgeReversalDescriptionFuture".Translate(this.biotunedTo.Named("PAWN"), ((int)this.biotunedTo.ageTracker.AgeReversalDemandedDeadlineTicks).ToStringTicksToPeriodVague(true, true).Named("TIME")) : "BiosculpterAutoAgeReversalDescriptionNow".Translate(this.biotunedTo.Named("PAWN"));
				command_Toggle.defaultDesc = "BiosculpterAutoAgeReversalDescription".Translate(this.biotunedTo.Named("PAWN"), taggedString.Named("NEXTTREATMENT"));
				command_Toggle.icon = this.AutoAgeReversalIcon;
				command_Toggle.isActive = (() => this.AutoAgeReversal);
				command_Toggle.toggleAction = delegate()
				{
					this.autoAgeReversal = !this.autoAgeReversal;
				};
				if (!CompBiosculpterPod.CanAgeReverse(this.biotunedTo))
				{
					command_Toggle.Disable("UnderMinBiosculpterAgeReversalAge".Translate(this.biotunedTo.ageTracker.AdultMinAge.Named("ADULTAGE")).CapitalizeFirst());
					this.autoAgeReversal = false;
				}
				yield return command_Toggle;
			}
			foreach (Gizmo gizmo in StorageSettingsClipboard.CopyPasteGizmosFor(this.allowedNutritionSettings))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator2 = null;
			Gizmo gizmo2 = Building.SelectContainedItemGizmo(this.parent, this.Occupant);
			if (gizmo2 != null)
			{
				yield return gizmo2;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: complete cycle",
					action = delegate()
					{
						this.currentCycleTicksRemaining = 10f;
					},
					Disabled = (this.State != BiosculpterPodState.Occupied)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: advance cycle +1 day",
					action = delegate()
					{
						this.currentCycleTicksRemaining -= 60000f;
					},
					Disabled = (this.State != BiosculpterPodState.Occupied)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: complete biotune timer",
					action = delegate()
					{
						this.biotunedCountdownTicks = 10;
					},
					Disabled = (this.biotunedCountdownTicks <= 0)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: fill nutrition and cycle ingredients",
					action = delegate()
					{
						this.liquifiedNutrition = 5f;
						this.devFillPodLatch = true;
					},
					Disabled = (this.State == BiosculpterPodState.Occupied || (this.devFillPodLatch && this.liquifiedNutrition == 5f))
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FCE7 RID: 64743 RVA: 0x004953E0 File Offset: 0x004935E0
		private string IngredientsDescription(CompBiosculpterPod_Cycle cycle)
		{
			this.tmpIngredientsStrings.Clear();
			if (!cycle.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>() && !this.devFillPodLatch)
			{
				for (int i = 0; i < cycle.Props.extraRequiredIngredients.Count; i++)
				{
					this.tmpIngredientsStrings.Add(cycle.Props.extraRequiredIngredients[i].Summary);
				}
			}
			return this.tmpIngredientsStrings.ToCommaList(true, false);
		}

		// Token: 0x0600FCE8 RID: 64744 RVA: 0x0049545C File Offset: 0x0049365C
		private string CycleDescription(CompBiosculpterPod_Cycle cycle)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(cycle.Description(this.biotunedTo));
			float num = cycle.Props.durationDays / this.CycleSpeedFactor;
			float num2 = num / PreceptDefOf.Biosculpting_Accelerated.biosculpterPodCycleSpeedFactor;
			stringBuilder.AppendLine("\n\n" + "BiosculpterPodCycleDuration".Translate() + ": " + ((int)(num * 60000f)).ToStringTicksToDays("F1"));
			if (!Find.IdeoManager.classicMode)
			{
				stringBuilder.Append("BiosculpterPodCycleDurationTranshumanists".Translate() + ": " + ((int)(num2 * 60000f)).ToStringTicksToDays("F1"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600FCE9 RID: 64745 RVA: 0x0049552C File Offset: 0x0049372C
		public bool PawnCanUseNow(Pawn pawn, CompBiosculpterPod_Cycle cycle)
		{
			string text;
			if ((text = this.CannotUseNowReason()) == null && (text = this.CannotUseNowPawnReason(pawn)) == null)
			{
				text = (this.CannotUseNowCycleReason(cycle) ?? this.CannotUseNowPawnCycleReason(pawn, cycle, true));
			}
			return text == null;
		}

		// Token: 0x0600FCEA RID: 64746 RVA: 0x0049555A File Offset: 0x0049375A
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (selPawn.IsQuestLodger())
			{
				yield return new FloatMenuOption("CannotEnter".Translate() + ": " + "CryptosleepCasketGuestsNotAllowed".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			string cycleIndependentfailureReason = this.CannotUseNowReason() ?? this.CannotUseNowPawnReason(selPawn);
			using (List<CompBiosculpterPod_Cycle>.Enumerator enumerator = this.AvailableCycles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBiosculpterPod_Cycle cycle = enumerator.Current;
					string text;
					if ((text = cycleIndependentfailureReason) == null)
					{
						text = (this.CannotUseNowCycleReason(cycle) ?? this.CannotUseNowPawnCycleReason(selPawn, cycle, true));
					}
					string text2 = text;
					if (text2 != null)
					{
						yield return new FloatMenuOption(CompBiosculpterPod.CannotStartText(cycle, text2), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						string label = "EnterBiosculpterPod".Translate(cycle.Props.label, ((int)(cycle.Props.durationDays / this.GetCycleSpeedFactorForPawn(selPawn) * 60000f)).ToStringTicksToDays("F1"));
						yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, delegate()
						{
							this.PrepareCycleJob(selPawn, selPawn, cycle, this.EnterBiosculpterJob());
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy", null);
					}
				}
			}
			List<CompBiosculpterPod_Cycle>.Enumerator enumerator = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600FCEB RID: 64747 RVA: 0x00495571 File Offset: 0x00493771
		public static bool CanAgeReverse(Pawn biosculptee)
		{
			return biosculptee.ageTracker.Adult;
		}

		// Token: 0x0600FCEC RID: 64748 RVA: 0x0049557E File Offset: 0x0049377E
		public static List<CompBiosculpterPod> BiotunedPods(Pawn pawn)
		{
			return CompBiosculpterPod.cachedBiotunedPods.TryGetValue(pawn, null);
		}

		// Token: 0x0600FCED RID: 64749 RVA: 0x0049558C File Offset: 0x0049378C
		public static bool HasBiotunedAutoAgeReversePod(Pawn pawn)
		{
			List<CompBiosculpterPod> list = CompBiosculpterPod.cachedBiotunedPods.TryGetValue(pawn, null);
			if (list == null)
			{
				return false;
			}
			using (List<CompBiosculpterPod>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AutoAgeReversal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600FCEE RID: 64750 RVA: 0x004955F4 File Offset: 0x004937F4
		public static string CannotStartText(CompBiosculpterPod_Cycle cycle, string translatedReason)
		{
			return "BiosculpterCannotStartCycle".Translate(cycle.Props.label) + ": " + translatedReason.CapitalizeFirst();
		}

		// Token: 0x0600FCEF RID: 64751 RVA: 0x0049562C File Offset: 0x0049382C
		public string CannotUseNowCycleReason(CompBiosculpterPod_Cycle cycle)
		{
			List<string> list = cycle.MissingResearchLabels();
			if (list.Any<string>())
			{
				return "MissingRequiredResearch".Translate() + " " + list.ToCommaList(false, false);
			}
			return null;
		}

		// Token: 0x0600FCF0 RID: 64752 RVA: 0x00495670 File Offset: 0x00493870
		public string CannotUseNowPawnCycleReason(Pawn p, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
		{
			return this.CannotUseNowPawnCycleReason(p, p, cycle, checkIngredients);
		}

		// Token: 0x0600FCF1 RID: 64753 RVA: 0x0049567C File Offset: 0x0049387C
		private bool CanReachOrHasIngredients(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool useCache = false)
		{
			return this.PawnCarryingExtraCycleIngredients(biosculptee, cycle, false) || (biosculptee != hauler && this.PawnCarryingExtraCycleIngredients(hauler, cycle, false)) || this.CanReachRequiredIngredients(hauler, cycle, useCache);
		}

		// Token: 0x0600FCF2 RID: 64754 RVA: 0x004956A4 File Offset: 0x004938A4
		public string CannotUseNowPawnCycleReason(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
		{
			if (this.AgeReversalCycleKey != null && cycle.Props.key == this.AgeReversalCycleKey && !CompBiosculpterPod.CanAgeReverse(biosculptee))
			{
				return "UnderMinBiosculpterAgeReversalAge".Translate(biosculptee.ageTracker.AdultMinAge.Named("ADULTAGE")).CapitalizeFirst();
			}
			if (checkIngredients && !this.CanReachOrHasIngredients(hauler, biosculptee, cycle, true))
			{
				return "BiosculpterMissingIngredients".Translate(this.IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0600FCF3 RID: 64755 RVA: 0x00495748 File Offset: 0x00493948
		public string CannotUseNowPawnReason(Pawn p)
		{
			if (this.biotunedTo != null && this.biotunedTo != p)
			{
				return "BiosculpterBiotunedToAnother".Translate().CapitalizeFirst();
			}
			if (!p.CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0600FCF4 RID: 64756 RVA: 0x004957B0 File Offset: 0x004939B0
		public string CannotUseNowReason()
		{
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.State == BiosculpterPodState.LoadingNutrition)
			{
				return "BiosculpterNutritionNotLoaded".Translate().CapitalizeFirst();
			}
			if (this.State == BiosculpterPodState.Occupied)
			{
				return "BiosculpterOccupied".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0600FCF5 RID: 64757 RVA: 0x00495820 File Offset: 0x00493A20
		private List<IngredientCount> RequiredIngredients(CompBiosculpterPod_Cycle cycle)
		{
			List<ThingDefCountClass> extraRequiredIngredients = cycle.Props.extraRequiredIngredients;
			if (extraRequiredIngredients == null || this.devFillPodLatch)
			{
				return null;
			}
			if (!this.cachedExtraIngredients.ContainsKey(cycle))
			{
				this.cachedExtraIngredients[cycle] = (from tc in extraRequiredIngredients
				select tc.ToIngredientCount()).ToList<IngredientCount>();
			}
			return this.cachedExtraIngredients[cycle];
		}

		// Token: 0x0600FCF6 RID: 64758 RVA: 0x00495898 File Offset: 0x00493A98
		private bool CanReachRequiredIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool useCache = false)
		{
			this.chosenExtraItems.Clear();
			if (cycle.Props.extraRequiredIngredients == null || this.devFillPodLatch)
			{
				return true;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (useCache && this.cacheReachIngredientsPawn == pawn && this.cacheReachIngredientsCycle == cycle && realtimeSinceStartup < this.cacheReachIngredientsTime + 2f)
			{
				return this.cacheReachIngredientsResult;
			}
			this.cacheReachIngredientsPawn = pawn;
			this.cacheReachIngredientsCycle = cycle;
			this.cacheReachIngredientsTime = realtimeSinceStartup;
			this.cacheReachIngredientsResult = WorkGiver_DoBill.TryFindBestFixedIngredients(this.RequiredIngredients(cycle), pawn, this.parent, this.chosenExtraItems, 999f);
			return this.cacheReachIngredientsResult;
		}

		// Token: 0x0600FCF7 RID: 64759 RVA: 0x00495938 File Offset: 0x00493B38
		private bool SelectPawnCycleOption(Pawn pawn, CompBiosculpterPod_Cycle cycle, out FloatMenuOption option)
		{
			string text = this.CannotUseNowPawnReason(pawn) ?? this.CannotUseNowPawnCycleReason(pawn, cycle, false);
			string label = pawn.Label + ((text == null) ? "" : (": " + text));
			Action action = null;
			if (text == null)
			{
				action = delegate()
				{
					this.PrepareCycleJob(pawn, pawn, cycle, this.EnterBiosculpterJob());
				};
			}
			option = new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			return text == null;
		}

		// Token: 0x0600FCF8 RID: 64760 RVA: 0x004959D8 File Offset: 0x00493BD8
		private bool SelectPawnsForCycleOptions(CompBiosculpterPod_Cycle cycle, out List<FloatMenuOption> options, bool shortCircuit = false)
		{
			this.cycleEligiblePawnOptions.Clear();
			options = this.cycleEligiblePawnOptions;
			if (!this.cachedAnyPawnEligible.ContainsKey(cycle))
			{
				this.cachedAnyPawnEligible[cycle] = new CacheAnyPawnEligibleCycle
				{
					gameTime = float.MinValue
				};
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (shortCircuit && (float)ticksGame < this.cachedAnyPawnEligible[cycle].gameTime + 2f)
			{
				return this.cachedAnyPawnEligible[cycle].anyEligible;
			}
			this.cachedAnyPawnEligible[cycle].gameTime = (float)ticksGame;
			if (this.biotunedTo != null)
			{
				if (this.biotunedTo.Dead || !this.biotunedTo.Spawned || this.biotunedTo.Map != this.parent.Map)
				{
					this.cachedAnyPawnEligible[cycle].anyEligible = false;
					return this.cachedAnyPawnEligible[cycle].anyEligible;
				}
				FloatMenuOption item;
				if (this.SelectPawnCycleOption(this.biotunedTo, cycle, out item) && shortCircuit)
				{
					this.cachedAnyPawnEligible[cycle].anyEligible = true;
					return this.cachedAnyPawnEligible[cycle].anyEligible;
				}
				this.cycleEligiblePawnOptions.Add(item);
			}
			else
			{
				foreach (Pawn pawn in this.parent.Map.mapPawns.FreeColonistsSpawned)
				{
					FloatMenuOption item2;
					if (this.SelectPawnCycleOption(pawn, cycle, out item2) && shortCircuit)
					{
						this.cachedAnyPawnEligible[cycle].anyEligible = true;
						return this.cachedAnyPawnEligible[cycle].anyEligible;
					}
					this.cycleEligiblePawnOptions.Add(item2);
				}
			}
			this.cachedAnyPawnEligible[cycle].anyEligible = (this.cycleEligiblePawnOptions.Count > 0);
			return this.cachedAnyPawnEligible[cycle].anyEligible;
		}

		// Token: 0x0600FCF9 RID: 64761 RVA: 0x00495BDC File Offset: 0x00493DDC
		public Job EnterBiosculpterJob()
		{
			return JobMaker.MakeJob(JobDefOf.EnterBiosculpterPod, this.parent);
		}

		// Token: 0x0600FCFA RID: 64762 RVA: 0x00495BF3 File Offset: 0x00493DF3
		public Job MakeCarryToBiosculpterJob(Pawn willBeCarried)
		{
			return JobMaker.MakeJob(JobDefOf.CarryToBiosculpterPod, willBeCarried, LocalTargetInfo.Invalid, this.parent);
		}

		// Token: 0x0600FCFB RID: 64763 RVA: 0x00495C18 File Offset: 0x00493E18
		public void ConfigureJobForCycle(Job job, CompBiosculpterPod_Cycle cycle, List<ThingCount> extraIngredients)
		{
			if (!extraIngredients.NullOrEmpty<ThingCount>())
			{
				job.targetQueueB = new List<LocalTargetInfo>(extraIngredients.Count);
				job.countQueue = new List<int>(extraIngredients.Count);
				foreach (ThingCount thingCount in extraIngredients)
				{
					job.targetQueueB.Add(thingCount.Thing);
					job.countQueue.Add(thingCount.Count);
				}
			}
			job.haulMode = HaulMode.ToCellNonStorage;
			job.biosculpterCycleKey = cycle.Props.key;
		}

		// Token: 0x0600FCFC RID: 64764 RVA: 0x00495CCC File Offset: 0x00493ECC
		public void PrepareCycleJob(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, Job job)
		{
			CompBiosculpterPod.OrderToPod(cycle, biosculptee, delegate
			{
				this.chosenExtraItems.Clear();
				if (!this.CanReachOrHasIngredients(hauler, biosculptee, cycle, false))
				{
					Messages.Message("BiosculpterMissingIngredients".Translate(this.IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst(), this.parent, MessageTypeDefOf.NegativeEvent, false);
					return;
				}
				this.ConfigureJobForCycle(job, cycle, this.chosenExtraItems);
				if (cycle.Props.extraRequiredIngredients != null && !this.devFillPodLatch)
				{
					if (job.def == JobDefOf.CarryToBiosculpterPod)
					{
						Messages.Message("BiosculpterCarryStartedMessage".Translate(hauler.Named("PAWN"), this.IngredientsDescription(cycle).Named("INGREDIENTS"), biosculptee.Named("DOWNED"), cycle.Props.label.Named("CYCLE")), this.parent, MessageTypeDefOf.SilentInput, false);
					}
					else
					{
						Messages.Message("BiosculpterLoadingStartedMessage".Translate(hauler.Named("PAWN"), this.IngredientsDescription(cycle).Named("INGREDIENTS"), cycle.Props.label.Named("CYCLE")), this.parent, MessageTypeDefOf.SilentInput, false);
					}
				}
				if (hauler.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false))
				{
					this.SetQueuedInformation(job, biosculptee);
				}
			});
		}

		// Token: 0x0600FCFD RID: 64765 RVA: 0x00495D20 File Offset: 0x00493F20
		public void ClearQueuedInformation()
		{
			this.SetQueuedInformation(null, null);
		}

		// Token: 0x0600FCFE RID: 64766 RVA: 0x00495D2A File Offset: 0x00493F2A
		public void SetQueuedInformation(Job job, Pawn biosculptee)
		{
			this.queuedEnterJob = job;
			this.queuedPawn = biosculptee;
		}

		// Token: 0x0600FCFF RID: 64767 RVA: 0x00495D3A File Offset: 0x00493F3A
		public bool CanAcceptNutrition(Thing thing)
		{
			return this.allowedNutritionSettings.AllowedToAccept(thing);
		}

		// Token: 0x0600FD00 RID: 64768 RVA: 0x00495D48 File Offset: 0x00493F48
		public bool CanAcceptOnceCycleChosen(Pawn pawn)
		{
			return this.State == BiosculpterPodState.SelectingCycle && this.PowerOn && (this.biotunedTo == null || this.biotunedTo == pawn);
		}

		// Token: 0x0600FD01 RID: 64769 RVA: 0x00495D71 File Offset: 0x00493F71
		public bool PawnCarryingExtraCycleIngredients(Pawn pawn, string cycleKey, bool remove = false)
		{
			return this.PawnCarryingExtraCycleIngredients(pawn, this.GetCycle(cycleKey), remove);
		}

		// Token: 0x0600FD02 RID: 64770 RVA: 0x00495D84 File Offset: 0x00493F84
		public bool PawnCarryingExtraCycleIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool remove = false)
		{
			if (cycle.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>() || this.devFillPodLatch)
			{
				return true;
			}
			foreach (ThingDefCountClass thingDefCountClass in cycle.Props.extraRequiredIngredients)
			{
				if (pawn.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					return false;
				}
			}
			if (remove)
			{
				foreach (ThingDefCountClass thingDefCountClass2 in cycle.Props.extraRequiredIngredients)
				{
					pawn.inventory.RemoveCount(thingDefCountClass2.thingDef, thingDefCountClass2.count, true);
				}
			}
			return true;
		}

		// Token: 0x0600FD03 RID: 64771 RVA: 0x00495E6C File Offset: 0x0049406C
		public bool TryAcceptPawn(Pawn pawn, string cycleKey)
		{
			return this.TryAcceptPawn(pawn, this.GetCycle(cycleKey));
		}

		// Token: 0x0600FD04 RID: 64772 RVA: 0x00495E7C File Offset: 0x0049407C
		public bool TryAcceptPawn(Pawn pawn, CompBiosculpterPod_Cycle cycle)
		{
			if (!this.CanAcceptOnceCycleChosen(pawn))
			{
				return false;
			}
			if (!this.PawnCarryingExtraCycleIngredients(pawn, cycle, true))
			{
				return false;
			}
			this.currentCycleKey = cycle.Props.key;
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			this.pawnEnteringBiosculpter = pawn;
			bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (pawn.holdingOwner != null)
			{
				pawn.holdingOwner.TryTransferToContainer(pawn, this.innerContainer, true);
			}
			else
			{
				this.innerContainer.TryAdd(pawn, true);
			}
			if (flag)
			{
				Find.Selector.Select(pawn, false, false);
			}
			this.pawnEnteringBiosculpter = null;
			this.currentCycleTicksRemaining = cycle.Props.durationDays * 60000f;
			this.liquifiedNutrition = 0f;
			this.devFillPodLatch = false;
			this.ClearQueuedInformation();
			this.tickEntered = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x0600FD05 RID: 64773 RVA: 0x00495F50 File Offset: 0x00494150
		public void EjectContents(bool interrupted, bool playSounds, Map destMap = null)
		{
			if (destMap == null)
			{
				destMap = this.parent.Map;
			}
			Pawn occupant = this.Occupant;
			this.currentCycleKey = null;
			this.currentCycleTicksRemaining = 0f;
			this.currentCyclePowerCutTicks = 0;
			this.liquifiedNutrition = 0f;
			this.devFillPodLatch = false;
			this.innerContainer.TryDropAll(this.parent.InteractionCell, destMap, ThingPlaceMode.Near, null, null, true);
			if (occupant != null)
			{
				FilthMaker.TryMakeFilth(this.parent.InteractionCell, destMap, ThingDefOf.Filth_PodSlime, new IntRange(3, 6).RandomInRange, FilthSourceFlags.None, true);
				if (interrupted)
				{
					Pawn_NeedsTracker needs = occupant.needs;
					if (needs != null)
					{
						needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoakingWet, null, null);
					}
					Pawn_HealthTracker health = occupant.health;
					if (health != null)
					{
						health.AddHediff(HediffDefOf.BiosculptingSickness, null, null, null);
					}
				}
			}
			if (playSounds)
			{
				SoundDef exitSound = this.Props.exitSound;
				if (exitSound == null)
				{
					return;
				}
				exitSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.parent.Position, this.parent.Map, false), MaintenanceType.None));
			}
		}

		// Token: 0x0600FD06 RID: 64774 RVA: 0x0049606C File Offset: 0x0049426C
		private void CycleCompleted()
		{
			Pawn occupant = this.Occupant;
			CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
			this.SetBiotuned(occupant);
			currentCycle.CycleCompleted(occupant);
			this.EjectContents(false, true, null);
			if (occupant != null)
			{
				Pawn_NeedsTracker needs = occupant.needs;
				Need_Food need_Food = (needs != null) ? needs.food : null;
				if (need_Food != null)
				{
					need_Food.CurLevelPercentage = 1f;
				}
				Pawn_NeedsTracker needs2 = occupant.needs;
				Need_Rest need_Rest = (needs2 != null) ? needs2.rest : null;
				if (need_Rest != null)
				{
					need_Rest.CurLevelPercentage = 1f;
				}
				if (currentCycle.Props.gainThoughtOnCompletion != null)
				{
					Pawn_NeedsTracker needs3 = occupant.needs;
					if (needs3 != null)
					{
						Need_Mood mood = needs3.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AgeReversalReceived, null, null);
						}
					}
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedBiosculpterPod, occupant.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.tickEntered > 0)
			{
				occupant.drugs.Notify_LeftSuspension(Find.TickManager.TicksGame - this.tickEntered);
			}
		}

		// Token: 0x0600FD07 RID: 64775 RVA: 0x00496168 File Offset: 0x00494368
		private void LiquifyNutrition()
		{
			CompBiosculpterPod.tmpItems.AddRange(this.innerContainer);
			foreach (Thing thing in CompBiosculpterPod.tmpItems)
			{
				float num = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
				if (num > 0f && !(thing is Pawn))
				{
					this.liquifiedNutrition = Mathf.Min(5f, this.liquifiedNutrition + num);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			CompBiosculpterPod.tmpItems.Clear();
		}

		// Token: 0x0600FD08 RID: 64776 RVA: 0x00496214 File Offset: 0x00494414
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				return;
			}
			base.CompTick();
			if (this.State != BiosculpterPodState.SelectingCycle || !this.PowerOn)
			{
				Effecter effecter = this.readyEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.readyEffecter = null;
			}
			else if (this.Props.readyEffecter != null)
			{
				if (this.readyEffecter == null)
				{
					this.readyEffecter = this.Props.readyEffecter.Spawn();
					this.ColorizeEffecter(this.readyEffecter, this.Props.selectCycleColor);
					this.readyEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
				}
				this.readyEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
			}
			if (this.State != BiosculpterPodState.Occupied)
			{
				Effecter effecter2 = this.progressBarEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.progressBarEffecter = null;
				Effecter effecter3 = this.operatingEffecter;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.operatingEffecter = null;
			}
			else
			{
				Pawn occupant = this.Occupant;
				this.biotunedCountdownTicks = 4800000;
				if (this.PowerOn)
				{
					int num = 1;
					this.currentCycleTicksRemaining -= (float)num * this.CycleSpeedFactor;
					if (this.currentCycleTicksRemaining <= 0f)
					{
						this.CycleCompleted();
					}
				}
				else
				{
					this.currentCyclePowerCutTicks++;
					if (this.currentCyclePowerCutTicks >= 60000)
					{
						this.EjectContents(true, true, null);
						Messages.Message("BiosculpterNoPowerEjectedMessage".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeEvent, false);
					}
				}
				if (this.currentCycleTicksRemaining > 0f)
				{
					if (this.progressBarEffecter == null)
					{
						this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
					}
					this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
					SubEffecter_ProgressBar subEffecter_ProgressBar = this.progressBarEffecter.children[0] as SubEffecter_ProgressBar;
					MoteProgressBar moteProgressBar = (subEffecter_ProgressBar != null) ? subEffecter_ProgressBar.mote : null;
					if (moteProgressBar != null)
					{
						float num2 = this.CurrentCycle.Props.durationDays * 60000f;
						moteProgressBar.progress = 1f - Mathf.Clamp01(this.currentCycleTicksRemaining / num2);
						int num3 = (this.parent.RotatedSize.z - 1) / 2;
						moteProgressBar.offsetZ = -((float)num3 + 0.5f);
					}
					if (this.Props.operatingEffecter != null)
					{
						if (!this.PowerOn)
						{
							Effecter effecter4 = this.operatingEffecter;
							if (effecter4 != null)
							{
								effecter4.Cleanup();
							}
							this.operatingEffecter = null;
						}
						else
						{
							if (this.operatingEffecter == null)
							{
								this.operatingEffecter = this.Props.operatingEffecter.Spawn();
								this.ColorizeEffecter(this.operatingEffecter, this.CurrentCycle.Props.operatingColor);
								this.operatingEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
							}
							this.operatingEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
						}
					}
				}
			}
			if (this.PowerOn && this.biotunedCountdownTicks > 0)
			{
				this.biotunedCountdownTicks--;
			}
			if (this.biotunedCountdownTicks <= 0)
			{
				this.SetBiotuned(null);
			}
			this.SetPower();
			Pawn pawn = this.biotunedTo;
			if (((pawn != null) ? pawn.Ideo : null) != null && !this.biotunedTo.Ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.autoAgeReversal = false;
			}
		}

		// Token: 0x0600FD09 RID: 64777 RVA: 0x004965DC File Offset: 0x004947DC
		private void SetPower()
		{
			if (this.powerTraderComp == null)
			{
				this.powerTraderComp = this.parent.TryGetComp<CompPowerTrader>();
			}
			if (this.powerComp == null)
			{
				this.powerComp = this.parent.TryGetComp<CompPower>();
			}
			if (this.State == BiosculpterPodState.Occupied)
			{
				this.powerTraderComp.PowerOutput = -this.powerComp.Props.PowerConsumption;
				return;
			}
			this.powerTraderComp.PowerOutput = -this.powerComp.Props.idlePowerDraw;
		}

		// Token: 0x0600FD0A RID: 64778 RVA: 0x00496660 File Offset: 0x00494860
		private void ColorizeEffecter(Effecter effecter, Color color)
		{
			foreach (SubEffecter subEffecter in effecter.children)
			{
				SubEffecter_Sprayer subEffecter_Sprayer = subEffecter as SubEffecter_Sprayer;
				if (subEffecter_Sprayer != null)
				{
					subEffecter_Sprayer.colorOverride = new Color?(color * subEffecter.def.color);
				}
			}
		}

		// Token: 0x0600FD0B RID: 64779 RVA: 0x004966D4 File Offset: 0x004948D4
		public override void PostDraw()
		{
			base.PostDraw();
			Rot4 rotation = this.parent.Rotation;
			Vector3 s = new Vector3(this.parent.def.graphicData.drawSize.x * 0.8f, 1f, this.parent.def.graphicData.drawSize.y * 0.8f);
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y -= 0.07317074f;
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(drawPos, rotation.AsQuat, s), CompBiosculpterPod.BackgroundMat, 0);
			if (this.State == BiosculpterPodState.Occupied)
			{
				Pawn occupant = this.Occupant;
				Vector3 drawLoc = this.parent.DrawPos + CompBiosculpterPod.FloatingOffset(this.currentCycleTicksRemaining + (float)this.currentCyclePowerCutTicks);
				Rot4 rotation2 = this.parent.Rotation;
				if (rotation2 == Rot4.East || rotation2 == Rot4.West)
				{
					drawLoc.z += 0.2f;
				}
				occupant.Drawer.renderer.RenderPawnAt(drawLoc, null, true);
			}
		}

		// Token: 0x0600FD0C RID: 64780 RVA: 0x00496800 File Offset: 0x00494A00
		public static Vector3 FloatingOffset(float tickOffset)
		{
			float num = tickOffset % 500f / 500f;
			float num2 = Mathf.Sin(3.1415927f * num);
			float z = num2 * num2 * 0.04f;
			return new Vector3(0f, 0f, z);
		}

		// Token: 0x0600FD0D RID: 64781 RVA: 0x00496841 File Offset: 0x00494A41
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600FD0E RID: 64782 RVA: 0x004948C0 File Offset: 0x00492AC0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600FD0F RID: 64783 RVA: 0x0049684F File Offset: 0x00494A4F
		public StorageSettings GetStoreSettings()
		{
			return this.allowedNutritionSettings;
		}

		// Token: 0x0600FD10 RID: 64784 RVA: 0x00496857 File Offset: 0x00494A57
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x0600FD11 RID: 64785 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0600FD12 RID: 64786 RVA: 0x00496870 File Offset: 0x00494A70
		private static void OrderToPod(CompBiosculpterPod_Cycle cycle, Pawn pawn, Action giveJobAct)
		{
			CompBiosculpterPod_HealingCycle compBiosculpterPod_HealingCycle = cycle as CompBiosculpterPod_HealingCycle;
			if (compBiosculpterPod_HealingCycle != null)
			{
				string healingDescriptionForPawn = compBiosculpterPod_HealingCycle.GetHealingDescriptionForPawn(pawn);
				string str = healingDescriptionForPawn.NullOrEmpty() ? "BiosculpterNoCoditionsToHeal".Translate(pawn.Named("PAWN"), compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() : ("OnCompletionOfCycle".Translate(compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() + ":\n\n" + healingDescriptionForPawn);
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, giveJobAct, healingDescriptionForPawn.NullOrEmpty(), null, WindowLayer.Dialog));
				return;
			}
			giveJobAct();
		}

		// Token: 0x170027CF RID: 10191
		// (get) Token: 0x0600FD13 RID: 64787 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600FD14 RID: 64788 RVA: 0x00496928 File Offset: 0x00494B28
		public static Thing FindPodFor(Pawn pawn, Pawn traveller, bool biotuned)
		{
			CompBiosculpterPod.<>c__DisplayClass143_0 CS$<>8__locals1 = new CompBiosculpterPod.<>c__DisplayClass143_0();
			CS$<>8__locals1.biotuned = biotuned;
			CS$<>8__locals1.traveller = traveller;
			if (CompBiosculpterPod.cachedPodDefs.NullOrEmpty<ThingDef>())
			{
				CompBiosculpterPod.cachedPodDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where def.GetCompProperties<CompProperties_BiosculpterPod>() != null
				select def).ToList<ThingDef>();
			}
			foreach (ThingDef singleDef in CompBiosculpterPod.cachedPodDefs)
			{
				Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.traveller.Position, pawn.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindPodFor>g__Validator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x0600FD15 RID: 64789 RVA: 0x00496A18 File Offset: 0x00494C18
		public static bool WasLoadingCanceled(Thing thing)
		{
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			return compBiosculpterPod != null && compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition;
		}

		// Token: 0x0600FD16 RID: 64790 RVA: 0x00496A3A File Offset: 0x00494C3A
		public void ClearCycle()
		{
			this.currentCycleKey = null;
		}

		// Token: 0x0600FD17 RID: 64791 RVA: 0x00496A43 File Offset: 0x00494C43
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			this.LiquifyNutrition();
			SoundDefOf.Standard_Drop.PlayOneShot(this.parent);
		}

		// Token: 0x0400A5B1 RID: 42417
		private const int NoPowerEjectCumulativeTicks = 60000;

		// Token: 0x0400A5B2 RID: 42418
		private const int BiotunedDuration = 4800000;

		// Token: 0x0400A5B3 RID: 42419
		private const float NutritionRequired = 5f;

		// Token: 0x0400A5B4 RID: 42420
		private const float CacheForSecs = 2f;

		// Token: 0x0400A5B5 RID: 42421
		private static readonly Texture2D InterruptCycleIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400A5B6 RID: 42422
		private static readonly Material BackgroundMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.082f, 0.078f, 0.063f), ShaderDatabase.SolidColorBehind);

		// Token: 0x0400A5B7 RID: 42423
		private const float BackgroundRect_YOff = 0.07317074f;

		// Token: 0x0400A5B8 RID: 42424
		private const float Pawn_YOff = 0.03658537f;

		// Token: 0x0400A5B9 RID: 42425
		private string currentCycleKey;

		// Token: 0x0400A5BA RID: 42426
		private float currentCycleTicksRemaining;

		// Token: 0x0400A5BB RID: 42427
		private int currentCyclePowerCutTicks;

		// Token: 0x0400A5BC RID: 42428
		private ThingOwner innerContainer;

		// Token: 0x0400A5BD RID: 42429
		private Pawn biotunedTo;

		// Token: 0x0400A5BE RID: 42430
		private int biotunedCountdownTicks;

		// Token: 0x0400A5BF RID: 42431
		private StorageSettings allowedNutritionSettings;

		// Token: 0x0400A5C0 RID: 42432
		private float liquifiedNutrition;

		// Token: 0x0400A5C1 RID: 42433
		public bool autoLoadNutrition = true;

		// Token: 0x0400A5C2 RID: 42434
		public bool devFillPodLatch;

		// Token: 0x0400A5C3 RID: 42435
		private bool autoAgeReversal;

		// Token: 0x0400A5C4 RID: 42436
		private int tickEntered = -99999;

		// Token: 0x0400A5C5 RID: 42437
		public Job queuedEnterJob;

		// Token: 0x0400A5C6 RID: 42438
		public Pawn queuedPawn;

		// Token: 0x0400A5C7 RID: 42439
		private List<ThingCount> chosenExtraItems = new List<ThingCount>();

		// Token: 0x0400A5C8 RID: 42440
		private List<FloatMenuOption> cycleEligiblePawnOptions = new List<FloatMenuOption>();

		// Token: 0x0400A5C9 RID: 42441
		private Pawn pawnEnteringBiosculpter;

		// Token: 0x0400A5CA RID: 42442
		private Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>> cachedExtraIngredients = new Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>>();

		// Token: 0x0400A5CB RID: 42443
		private Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle> cachedAnyPawnEligible = new Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle>();

		// Token: 0x0400A5CC RID: 42444
		private static Dictionary<Pawn, List<CompBiosculpterPod>> cachedBiotunedPods = new Dictionary<Pawn, List<CompBiosculpterPod>>();

		// Token: 0x0400A5CD RID: 42445
		private Pawn cacheReachIngredientsPawn;

		// Token: 0x0400A5CE RID: 42446
		private CompBiosculpterPod_Cycle cacheReachIngredientsCycle;

		// Token: 0x0400A5CF RID: 42447
		private float cacheReachIngredientsTime = float.MinValue;

		// Token: 0x0400A5D0 RID: 42448
		private bool cacheReachIngredientsResult;

		// Token: 0x0400A5D1 RID: 42449
		private Effecter progressBarEffecter;

		// Token: 0x0400A5D2 RID: 42450
		private Effecter operatingEffecter;

		// Token: 0x0400A5D3 RID: 42451
		private Effecter readyEffecter;

		// Token: 0x0400A5D4 RID: 42452
		private Texture2D cachedAutoAgeReverseIcon;

		// Token: 0x0400A5D5 RID: 42453
		private List<CompBiosculpterPod_Cycle> cachedAvailableCycles;

		// Token: 0x0400A5D6 RID: 42454
		private Dictionary<string, CompBiosculpterPod_Cycle> cycleLookup;

		// Token: 0x0400A5D7 RID: 42455
		private static string cachedAgeReversalCycleKey = null;

		// Token: 0x0400A5D8 RID: 42456
		private List<string> tmpIngredientsStrings = new List<string>();

		// Token: 0x0400A5D9 RID: 42457
		private static readonly List<Thing> tmpItems = new List<Thing>();

		// Token: 0x0400A5DA RID: 42458
		private CompPowerTrader powerTraderComp;

		// Token: 0x0400A5DB RID: 42459
		private CompPower powerComp;

		// Token: 0x0400A5DC RID: 42460
		private static List<ThingDef> cachedPodDefs;
	}
}
