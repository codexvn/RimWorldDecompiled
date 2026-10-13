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
	// Token: 0x02002A4C RID: 10828
	[StaticConstructorOnStartup]
	public class Building_SubcoreScanner : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x17002569 RID: 9577
		// (get) Token: 0x0600F069 RID: 61545 RVA: 0x004508A5 File Offset: 0x0044EAA5
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x1700256A RID: 9578
		// (get) Token: 0x0600F06A RID: 61546 RVA: 0x0045B7C0 File Offset: 0x004599C0
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x1700256B RID: 9579
		// (get) Token: 0x0600F06B RID: 61547 RVA: 0x00075162 File Offset: 0x00073362
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x1700256C RID: 9580
		// (get) Token: 0x0600F06C RID: 61548 RVA: 0x0045B7DB File Offset: 0x004599DB
		public bool PowerOn
		{
			get
			{
				return this.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x1700256D RID: 9581
		// (get) Token: 0x0600F06D RID: 61549 RVA: 0x001E5B28 File Offset: 0x001E3D28
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x1700256E RID: 9582
		// (get) Token: 0x0600F06E RID: 61550 RVA: 0x0045B7E8 File Offset: 0x004599E8
		public Pawn Occupant
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null)
					{
						return pawn;
					}
				}
				return null;
			}
		}

		// Token: 0x1700256F RID: 9583
		// (get) Token: 0x0600F06F RID: 61551 RVA: 0x0045B824 File Offset: 0x00459A24
		public bool AllRequiredIngredientsLoaded
		{
			get
			{
				if (!this.debugDisableNeedForIngredients)
				{
					for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
					{
						if (this.GetRequiredCountOf(this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient) > 0)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17002570 RID: 9584
		// (get) Token: 0x0600F070 RID: 61552 RVA: 0x0045B880 File Offset: 0x00459A80
		public SubcoreScannerState State
		{
			get
			{
				if (!this.initScanner || !this.PowerOn)
				{
					return SubcoreScannerState.Inactive;
				}
				if (!this.AllRequiredIngredientsLoaded)
				{
					return SubcoreScannerState.WaitingForIngredients;
				}
				if (this.Occupant == null)
				{
					return SubcoreScannerState.WaitingForOccupant;
				}
				return SubcoreScannerState.Occupied;
			}
		}

		// Token: 0x17002571 RID: 9585
		// (get) Token: 0x0600F071 RID: 61553 RVA: 0x0045B8A9 File Offset: 0x00459AA9
		public bool DestroyOccupantBrain
		{
			get
			{
				return this.def.building.destroyBrain;
			}
		}

		// Token: 0x0600F072 RID: 61554 RVA: 0x0045B8BC File Offset: 0x00459ABC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				this.def.building.subcoreScannerFixedIngredients[i].ResolveReferences();
			}
		}

		// Token: 0x0600F073 RID: 61555 RVA: 0x0045B90C File Offset: 0x00459B0C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			Effecter effecter2 = this.effectHusk;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.effectHusk = null;
			Effecter effecter3 = this.effectStart;
			if (effecter3 != null)
			{
				effecter3.Cleanup();
			}
			this.effectStart = null;
			if (!base.BeingTransportedOnGravship && this.DestroyOccupantBrain && this.Occupant != null)
			{
				this.KillOccupant();
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600F074 RID: 61556 RVA: 0x0045B988 File Offset: 0x00459B88
		public int GetRequiredCountOf(ThingDef thingDef)
		{
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				if (this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient == thingDef)
				{
					int num = this.innerContainer.TotalStackCountOfDef(this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient);
					return (int)this.def.building.subcoreScannerFixedIngredients[i].GetBaseCount() - num;
				}
			}
			return 0;
		}

		// Token: 0x0600F075 RID: 61557 RVA: 0x0045BA1C File Offset: 0x00459C1C
		public override AcceptanceReport CanAcceptPawn(Pawn selPawn)
		{
			if (!selPawn.IsColonist && !selPawn.IsSlaveOfColony && !selPawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.selectedPawn != null && this.selectedPawn != selPawn)
			{
				return false;
			}
			if (!this.PowerOn)
			{
				return "CannotUseNoPower".Translate();
			}
			if (this.State != SubcoreScannerState.WaitingForOccupant)
			{
				switch (this.State)
				{
				case SubcoreScannerState.Inactive:
					return "SubcoreScannerNotInit".Translate();
				case SubcoreScannerState.WaitingForIngredients:
				{
					StringBuilder stringBuilder = new StringBuilder("SubcoreScannerRequiresIngredients".Translate() + ": ");
					bool flag = false;
					for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
					{
						IngredientCount ingredientCount = this.def.building.subcoreScannerFixedIngredients[i];
						int num = this.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
						int num2 = (int)ingredientCount.GetBaseCount();
						if (num < num2)
						{
							if (flag)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(string.Format("{0} x{1}", ingredientCount.FixedIngredient.LabelCap, num2 - num));
							flag = true;
						}
					}
					return stringBuilder.ToString();
				}
				case SubcoreScannerState.Occupied:
					return "SubcoreScannerOccupied".Translate();
				}
			}
			else
			{
				if (selPawn.IsQuestLodger())
				{
					return "CryptosleepCasketGuestsNotAllowed".Translate();
				}
				if (selPawn.health.hediffSet.HasHediff(HediffDefOf.ScanningSickness, false))
				{
					return "SubcoreScannerPawnHasSickness".Translate(HediffDefOf.ScanningSickness.label);
				}
				if (selPawn.DevelopmentalStage.Baby())
				{
					return "SubcoreScannerBabyNotAllowed".Translate();
				}
			}
			return true;
		}

		// Token: 0x0600F076 RID: 61558 RVA: 0x0045BC04 File Offset: 0x00459E04
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (!this.CanAcceptPawn(pawn))
			{
				return;
			}
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
			this.fabricationTicksLeft = this.def.building.subcoreScannerTicks;
		}

		// Token: 0x0600F077 RID: 61559 RVA: 0x0045BC78 File Offset: 0x00459E78
		public bool CanAcceptIngredient(Thing thing)
		{
			return this.GetRequiredCountOf(thing.def) > 0;
		}

		// Token: 0x0600F078 RID: 61560 RVA: 0x0045BC8C File Offset: 0x00459E8C
		public void EjectContents()
		{
			Pawn occupant = this.Occupant;
			if (occupant == null)
			{
				this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			else
			{
				if (this.def.building.subcoreScannerHediff != null)
				{
					Pawn_HealthTracker health = occupant.health;
					if (health != null)
					{
						health.AddHediff(this.def.building.subcoreScannerHediff, null, null, null);
					}
				}
				if (this.DestroyOccupantBrain)
				{
					this.KillOccupant();
				}
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					if (this.innerContainer[i] is Pawn || this.innerContainer[i] is Corpse)
					{
						Thing thing;
						this.innerContainer.TryDrop(this.innerContainer[i], this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
					}
				}
				this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			this.selectedPawn = null;
			this.initScanner = false;
		}

		// Token: 0x0600F079 RID: 61561 RVA: 0x0045BD94 File Offset: 0x00459F94
		private void KillOccupant()
		{
			Pawn occupant = this.Occupant;
			DamageInfo dinfo = new DamageInfo(DamageDefOf.ExecutionCut, 9999f, 999f, -1f, null, occupant.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetIgnoreInstantKillProtection(true);
			dinfo.SetAllowDamagePropagation(false);
			occupant.forceNoDeathNotification = true;
			occupant.TakeDamage(dinfo);
			occupant.forceNoDeathNotification = false;
			ThoughtUtility.GiveThoughtsForPawnExecuted(occupant, null, PawnExecutionKind.Ripscanned);
			Messages.Message("MessagePawnKilledRipscanner".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x0600F07A RID: 61562 RVA: 0x0045BE35 File Offset: 0x0045A035
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
					Action <>9__1;
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate()
					{
						if (this.DestroyOccupantBrain)
						{
							WindowStack windowStack = Find.WindowStack;
							TaggedString text = "ConfirmRipscanPawn".Translate(selPawn.Named("PAWN"));
							Action confirmedAct;
							if ((confirmedAct = <>9__1) == null)
							{
								confirmedAct = (<>9__1 = delegate()
								{
									this.SelectPawn(selPawn);
								});
							}
							windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
							return;
						}
						this.SelectPawn(selPawn);
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

		// Token: 0x0600F07B RID: 61563 RVA: 0x0045BE4C File Offset: 0x0045A04C
		public static bool WasLoadingCancelled(Thing thing)
		{
			Building_SubcoreScanner building_SubcoreScanner = thing as Building_SubcoreScanner;
			return building_SubcoreScanner != null && !building_SubcoreScanner.initScanner;
		}

		// Token: 0x0600F07C RID: 61564 RVA: 0x0045BE70 File Offset: 0x0045A070
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			Pawn occupant = this.Occupant;
			if (occupant != null)
			{
				occupant.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, null, true);
			}
		}

		// Token: 0x0600F07D RID: 61565 RVA: 0x0045BEAC File Offset: 0x0045A0AC
		protected override void Tick()
		{
			base.Tick();
			if (Building_SubcoreScanner.MotePerRotation == null)
			{
				Building_SubcoreScanner.MotePerRotation = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.SoftScannerGlow_South
					},
					{
						Rot4.East,
						ThingDefOf.SoftScannerGlow_East
					},
					{
						Rot4.West,
						ThingDefOf.SoftScannerGlow_West
					},
					{
						Rot4.North,
						ThingDefOf.SoftScannerGlow_North
					}
				};
				Building_SubcoreScanner.MotePerRotationRip = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.RipScannerGlow_South
					},
					{
						Rot4.East,
						ThingDefOf.RipScannerGlow_East
					},
					{
						Rot4.West,
						ThingDefOf.RipScannerGlow_West
					},
					{
						Rot4.North,
						ThingDefOf.RipScannerGlow_North
					}
				};
			}
			SubcoreScannerState state = this.State;
			if (state == SubcoreScannerState.Occupied)
			{
				this.fabricationTicksLeft--;
				if (this.fabricationTicksLeft <= 0)
				{
					if (!this.DestroyOccupantBrain)
					{
						Messages.Message("MessageSubcoreSoftscannerCompleted".Translate(this.Occupant.Named("PAWN")), this.Occupant, MessageTypeDefOf.PositiveEvent, true);
					}
					this.EjectContents();
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(this.def.building.subcoreScannerOutputDef, null), this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, null, 1);
					if (this.def.building.subcoreScannerComplete != null)
					{
						this.def.building.subcoreScannerComplete.PlayOneShot(this);
					}
				}
				if (this.workingMote == null || this.workingMote.Destroyed)
				{
					this.workingMote = MoteMaker.MakeAttachedOverlay(this, this.DestroyOccupantBrain ? Building_SubcoreScanner.MotePerRotationRip[base.Rotation] : Building_SubcoreScanner.MotePerRotation[base.Rotation], Vector3.zero, 1f, -1f);
				}
				Mote mote = this.workingMote;
				if (mote != null)
				{
					mote.Maintain();
				}
				if (this.DestroyOccupantBrain)
				{
					if (this.effectHusk == null)
					{
						this.effectHusk = EffecterDefOf.RipScannerHeadGlow.Spawn(this, base.MapHeld, Building_SubcoreScanner.HuskEffectOffsets[base.Rotation]);
					}
					this.effectHusk.EffectTick(this, this);
				}
				if (this.progressBarEffecter == null)
				{
					this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
				MoteProgressBar mote2 = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
				mote2.progress = 1f - (float)this.fabricationTicksLeft / (float)this.def.building.subcoreScannerTicks;
				mote2.offsetZ = -0.8f;
				if (this.def.building.subcoreScannerWorking != null)
				{
					if (this.sustainerWorking == null || this.sustainerWorking.Ended)
					{
						this.sustainerWorking = this.def.building.subcoreScannerWorking.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					else
					{
						this.sustainerWorking.Maintain();
					}
				}
			}
			else
			{
				Effecter effecter = this.effectHusk;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effectHusk = null;
				Effecter effecter2 = this.progressBarEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.progressBarEffecter = null;
			}
			if (state == SubcoreScannerState.Occupied)
			{
				if (this.def.building.subcoreScannerStartEffect != null)
				{
					if (this.effectStart == null)
					{
						this.effectStart = this.def.building.subcoreScannerStartEffect.Spawn();
						this.effectStart.Trigger(this, new TargetInfo(this.InteractionCell, base.Map, false), -1);
					}
					this.effectStart.EffectTick(this, new TargetInfo(this.InteractionCell, base.Map, false));
					return;
				}
			}
			else
			{
				Effecter effecter3 = this.effectStart;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.effectStart = null;
			}
		}

		// Token: 0x0600F07E RID: 61566 RVA: 0x0045C286 File Offset: 0x0045A486
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!this.initScanner)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "SubcoreScannerStart".Translate();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SubcoreScannerProduces".Translate() + " " + this.def.building.subcoreScannerOutputDef.label + ".");
				stringBuilder.Append("\n\n");
				stringBuilder.Append("DurationHours".Translate() + ": " + this.def.building.subcoreScannerTicks.ToStringTicksToPeriod(true, false, true, true, false));
				stringBuilder.Append("\n\n");
				string value = (from i in this.def.building.subcoreScannerFixedIngredients
				select i.Summary).ToCommaList(true, false);
				stringBuilder.Append("SubcoreScannerStartDesc".Translate(this.def.label, value));
				command_Action.defaultDesc = stringBuilder.ToString();
				command_Action.icon = this.InitScannerIcon.Texture;
				command_Action.action = delegate()
				{
					this.initScanner = true;
				};
				command_Action.activateSound = SoundDefOf.Tick_Tiny;
				yield return command_Action;
			}
			else if (base.SelectedPawn == null)
			{
				Command_Action command_Action2 = new Command_Action();
				command_Action2.defaultLabel = "InsertPerson".Translate() + "...";
				command_Action2.defaultDesc = "InsertPersonSubcoreScannerDesc".Translate(this.def.label);
				command_Action2.icon = Building_SubcoreScanner.InsertPersonIcon.Texture;
				command_Action2.action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
					for (int i = 0; i < allPawnsSpawned.Count; i++)
					{
						Pawn pawn = allPawnsSpawned[i];
						AcceptanceReport acceptanceReport = this.CanAcceptPawn(pawn);
						if (!acceptanceReport.Accepted)
						{
							if (!acceptanceReport.Reason.NullOrEmpty())
							{
								list.Add(new FloatMenuOption(pawn.LabelShortCap + ": " + acceptanceReport.Reason, null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						else
						{
							Action <>9__4;
							list.Add(new FloatMenuOption(pawn.LabelShortCap, delegate()
							{
								if (this.def.building.destroyBrain)
								{
									WindowStack windowStack = Find.WindowStack;
									TaggedString text = "ConfirmRipscanPawn".Translate(pawn.Named("PAWN"));
									Action confirmedAct;
									if ((confirmedAct = <>9__4) == null)
									{
										confirmedAct = (<>9__4 = delegate()
										{
											this.SelectPawn(pawn);
										});
									}
									windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
									return;
								}
								this.SelectPawn(pawn);
							}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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
					command_Action2.Disable("NoPower".Translate().CapitalizeFirst());
				}
				else if (this.State == SubcoreScannerState.WaitingForIngredients)
				{
					StringBuilder stringBuilder2 = new StringBuilder("SubcoreScannerWaitingForIngredientsDesc".Translate().CapitalizeFirst() + ":\n");
					this.AppendIngredientsList(stringBuilder2);
					command_Action2.Disable(stringBuilder2.ToString());
				}
				yield return command_Action2;
			}
			if (this.initScanner)
			{
				yield return new Command_Action
				{
					defaultLabel = ((this.State == SubcoreScannerState.Occupied) ? "CommandCancelSubcoreScan".Translate() : "CommandCancelLoad".Translate()),
					defaultDesc = ((this.State == SubcoreScannerState.Occupied) ? "CommandCancelSubcoreScanDesc".Translate() : "CommandCancelLoadDesc".Translate()),
					icon = Building_SubcoreScanner.CancelLoadingIcon,
					action = delegate()
					{
						if (this.State == SubcoreScannerState.Occupied && this.DestroyOccupantBrain)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelRipscan".Translate(this.Occupant.Named("PAWN")), new Action(this.EjectContents), true, null, WindowLayer.Dialog));
							return;
						}
						this.EjectContents();
					},
					activateSound = SoundDefOf.Designate_Cancel
				};
			}
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.State == SubcoreScannerState.Occupied)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Complete",
						action = delegate()
						{
							this.fabricationTicksLeft = 0;
						}
					};
				}
				yield return new Command_Action
				{
					defaultLabel = (this.debugDisableNeedForIngredients ? "DEV: Enable Ingredients" : "DEV: Disable Ingredients"),
					action = delegate()
					{
						this.debugDisableNeedForIngredients = !this.debugDisableNeedForIngredients;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F07F RID: 61567 RVA: 0x0045C298 File Offset: 0x0045A498
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			switch (this.State)
			{
			case SubcoreScannerState.WaitingForIngredients:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerWaitingForIngredients".Translate());
				this.AppendIngredientsList(stringBuilder);
				break;
			case SubcoreScannerState.WaitingForOccupant:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerWaitingForOccupant".Translate());
				break;
			case SubcoreScannerState.Occupied:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerCompletesIn".Translate() + ": " + this.fabricationTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
				break;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F080 RID: 61568 RVA: 0x0045C360 File Offset: 0x0045A560
		private void AppendIngredientsList(StringBuilder sb)
		{
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				IngredientCount ingredientCount = this.def.building.subcoreScannerFixedIngredients[i];
				int num = this.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
				int num2 = (int)ingredientCount.GetBaseCount();
				sb.AppendInNewLine(string.Format(" - {0} {1} / {2}", ingredientCount.FixedIngredient.LabelCap, num, num2));
			}
		}

		// Token: 0x0600F081 RID: 61569 RVA: 0x0045C3EB File Offset: 0x0045A5EB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.initScanner, "initScanner", false, false);
			Scribe_Values.Look<int>(ref this.fabricationTicksLeft, "fabricationTicksLeft", 0, false);
		}

		// Token: 0x04009EB2 RID: 40626
		private bool initScanner;

		// Token: 0x04009EB3 RID: 40627
		private int fabricationTicksLeft;

		// Token: 0x04009EB4 RID: 40628
		private Effecter effectStart;

		// Token: 0x04009EB5 RID: 40629
		private Effecter effectHusk;

		// Token: 0x04009EB6 RID: 40630
		private bool debugDisableNeedForIngredients;

		// Token: 0x04009EB7 RID: 40631
		private Mote workingMote;

		// Token: 0x04009EB8 RID: 40632
		private Sustainer sustainerWorking;

		// Token: 0x04009EB9 RID: 40633
		private Effecter progressBarEffecter;

		// Token: 0x04009EBA RID: 40634
		public static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04009EBB RID: 40635
		public static readonly CachedTexture InsertPersonIcon = new CachedTexture("UI/Icons/InsertPersonSubcoreScanner");

		// Token: 0x04009EBC RID: 40636
		private static Dictionary<Rot4, ThingDef> MotePerRotation;

		// Token: 0x04009EBD RID: 40637
		private static Dictionary<Rot4, ThingDef> MotePerRotationRip;

		// Token: 0x04009EBE RID: 40638
		private static readonly Dictionary<Rot4, Vector3> HuskEffectOffsets = new Dictionary<Rot4, Vector3>
		{
			{
				Rot4.North,
				new Vector3(0f, 0f, 0.47f)
			},
			{
				Rot4.South,
				new Vector3(0f, 0f, -0.3f)
			},
			{
				Rot4.East,
				new Vector3(0.4f, 0f, -0.025f)
			},
			{
				Rot4.West,
				new Vector3(-0.4f, 0f, -0.025f)
			}
		};

		// Token: 0x04009EBF RID: 40639
		private const float ProgressBarOffsetZ = -0.8f;

		// Token: 0x04009EC0 RID: 40640
		public CachedTexture InitScannerIcon = new CachedTexture("UI/Icons/SubcoreScannerStart");
	}
}
