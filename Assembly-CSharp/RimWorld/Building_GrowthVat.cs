using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A1C RID: 10780
	[StaticConstructorOnStartup]
	public class Building_GrowthVat : Building_Enterable, IStoreSettingsParent, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x1700250A RID: 9482
		// (get) Token: 0x0600EEBD RID: 61117 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700250B RID: 9483
		// (get) Token: 0x0600EEBE RID: 61118 RVA: 0x004508A5 File Offset: 0x0044EAA5
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x1700250C RID: 9484
		// (get) Token: 0x0600EEBF RID: 61119 RVA: 0x004543B8 File Offset: 0x004525B8
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x1700250D RID: 9485
		// (get) Token: 0x0600EEC0 RID: 61120 RVA: 0x00075162 File Offset: 0x00073362
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x1700250E RID: 9486
		// (get) Token: 0x0600EEC1 RID: 61121 RVA: 0x004543D3 File Offset: 0x004525D3
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x1700250F RID: 9487
		// (get) Token: 0x0600EEC2 RID: 61122 RVA: 0x004543E0 File Offset: 0x004525E0
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return CompBiosculpterPod.FloatingOffset((float)Find.TickManager.TicksGame);
			}
		}

		// Token: 0x17002510 RID: 9488
		// (get) Token: 0x0600EEC3 RID: 61123 RVA: 0x004543F2 File Offset: 0x004525F2
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

		// Token: 0x17002511 RID: 9489
		// (get) Token: 0x0600EEC4 RID: 61124 RVA: 0x0045440E File Offset: 0x0045260E
		public float BiostarvationDailyOffset
		{
			get
			{
				if (!base.Working)
				{
					return 0f;
				}
				if (!this.PowerOn || this.containedNutrition <= 0f)
				{
					return 0.5f;
				}
				return -0.1f;
			}
		}

		// Token: 0x17002512 RID: 9490
		// (get) Token: 0x0600EEC5 RID: 61125 RVA: 0x00454440 File Offset: 0x00452640
		private float BiostarvationSeverityPercent
		{
			get
			{
				if (this.selectedEmbryo != null)
				{
					return this.embryoStarvation;
				}
				if (this.selectedPawn != null)
				{
					Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
					if (firstHediffOfDef != null)
					{
						return firstHediffOfDef.Severity / HediffDefOf.BioStarvation.maxSeverity;
					}
				}
				return 0f;
			}
		}

		// Token: 0x17002513 RID: 9491
		// (get) Token: 0x0600EEC6 RID: 61126 RVA: 0x0045449C File Offset: 0x0045269C
		public float NutritionConsumedPerDay
		{
			get
			{
				float num = (this.selectedEmbryo != null) ? 6f : 3f;
				if (this.BiostarvationSeverityPercent > 0f)
				{
					float num2 = 1.1f;
					num *= num2;
				}
				return num;
			}
		}

		// Token: 0x17002514 RID: 9492
		// (get) Token: 0x0600EEC7 RID: 61127 RVA: 0x004544D8 File Offset: 0x004526D8
		public float NutritionStored
		{
			get
			{
				float num = this.containedNutrition;
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Thing thing = this.innerContainer[i];
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				}
				return num;
			}
		}

		// Token: 0x17002515 RID: 9493
		// (get) Token: 0x0600EEC8 RID: 61128 RVA: 0x00454528 File Offset: 0x00452728
		public float NutritionNeeded
		{
			get
			{
				if (this.selectedPawn == null && this.selectedEmbryo == null)
				{
					return 0f;
				}
				return 10f - this.NutritionStored;
			}
		}

		// Token: 0x17002516 RID: 9494
		// (get) Token: 0x0600EEC9 RID: 61129 RVA: 0x0045454C File Offset: 0x0045274C
		public int EmbryoGestationTicksRemaining
		{
			get
			{
				return this.startTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002517 RID: 9495
		// (get) Token: 0x0600EECA RID: 61130 RVA: 0x0045455F File Offset: 0x0045275F
		public float EmbryoGestationPct
		{
			get
			{
				return 1f - Mathf.Clamp01((float)this.EmbryoGestationTicksRemaining / 540000f);
			}
		}

		// Token: 0x17002518 RID: 9496
		// (get) Token: 0x0600EECB RID: 61131 RVA: 0x00454579 File Offset: 0x00452779
		private Graphic TopGraphic
		{
			get
			{
				if (this.cachedTopGraphic == null)
				{
					this.cachedTopGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Misc/GrowthVat/GrowthVatTop", ShaderDatabase.Transparent, this.def.graphicData.drawSize, Color.white);
				}
				return this.cachedTopGraphic;
			}
		}

		// Token: 0x17002519 RID: 9497
		// (get) Token: 0x0600EECC RID: 61132 RVA: 0x004545B3 File Offset: 0x004527B3
		private Graphic FetusEarlyStage
		{
			get
			{
				if (this.fetusEarlyStageGraphic == null)
				{
					this.fetusEarlyStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_EarlyStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.fetusEarlyStageGraphic;
			}
		}

		// Token: 0x1700251A RID: 9498
		// (get) Token: 0x0600EECD RID: 61133 RVA: 0x004545E2 File Offset: 0x004527E2
		private Graphic FetusLateStage
		{
			get
			{
				if (this.fetusLateStageGraphic == null)
				{
					this.fetusLateStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_LateStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.fetusLateStageGraphic;
			}
		}

		// Token: 0x1700251B RID: 9499
		// (get) Token: 0x0600EECE RID: 61134 RVA: 0x00454614 File Offset: 0x00452814
		private Hediff VatLearning
		{
			get
			{
				if (this.cachedVatLearning == null)
				{
					Hediff hediff;
					if ((hediff = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VatLearning, false)) == null)
					{
						hediff = this.selectedPawn.health.AddHediff(HediffDefOf.VatLearning, null, null, null);
					}
					this.cachedVatLearning = hediff;
				}
				return this.cachedVatLearning;
			}
		}

		// Token: 0x0600EECF RID: 61135 RVA: 0x00454674 File Offset: 0x00452874
		public override void PostMake()
		{
			base.PostMake();
			this.allowedNutritionSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.allowedNutritionSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600EED0 RID: 61136 RVA: 0x004546C0 File Offset: 0x004528C0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Color color = this.EmbryoColor();
					this.fetusEarlyStageGraphic = this.FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
					this.fetusLateStageGraphic = this.FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				});
			}
		}

		// Token: 0x0600EED1 RID: 61137 RVA: 0x004546FC File Offset: 0x004528FC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				if (this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
				{
					this.Notify_PawnRemoved();
				}
				this.DestroyEmbryo(false);
			}
			this.sustainerWorking = null;
			this.cachedVatLearning = null;
			base.DeSpawn(mode);
		}

		// Token: 0x0600EED2 RID: 61138 RVA: 0x0045474C File Offset: 0x0045294C
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Working && this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
			{
				Hediff vatLearning = this.VatLearning;
				if (vatLearning != null)
				{
					vatLearning.TickInterval(delta);
				}
				Hediff vatLearning2 = this.VatLearning;
				if (vatLearning2 == null)
				{
					return;
				}
				vatLearning2.PostTickInterval(delta);
			}
		}

		// Token: 0x0600EED3 RID: 61139 RVA: 0x004547A8 File Offset: 0x004529A8
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.PowerTraderComp.PowerOutput = (base.Working ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			Pawn selectedPawn = this.selectedPawn;
			if (selectedPawn == null || !selectedPawn.Destroyed)
			{
				HumanEmbryo humanEmbryo = this.selectedEmbryo;
				if (humanEmbryo == null || !humanEmbryo.Destroyed)
				{
					goto IL_78;
				}
			}
			this.OnStop();
			IL_78:
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				HumanEmbryo humanEmbryo2 = thing as HumanEmbryo;
				if (humanEmbryo2 != null && humanEmbryo2 != this.selectedEmbryo)
				{
					Thing thing2;
					this.innerContainer.TryDrop(thing, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing2, null, null);
				}
			}
			if (!base.Working)
			{
				this.TryGrowEmbryo();
				Effecter effecter = this.bubbleEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.bubbleEffecter = null;
				return;
			}
			if (this.selectedPawn != null)
			{
				if (this.selectedPawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
				{
					Messages.Message("OccupantEjectedFromGrowthVat".Translate(this.selectedPawn.Named("PAWN")) + ": " + "PawnIsTooOld".Translate(this.selectedPawn.Named("PAWN")), this.selectedPawn, MessageTypeDefOf.NeutralEvent, true);
					this.Finish();
					return;
				}
				if (this.innerContainer.Contains(this.selectedPawn))
				{
					int ticks = Mathf.RoundToInt(20f * this.selectedPawn.GetStatValue(StatDefOf.GrowthVatOccupantSpeed, true, -1));
					this.selectedPawn.ageTracker.Notify_TickedInGrowthVat(ticks);
					Hediff vatLearning = this.VatLearning;
					if (vatLearning != null)
					{
						vatLearning.Tick();
					}
					Hediff vatLearning2 = this.VatLearning;
					if (vatLearning2 != null)
					{
						vatLearning2.PostTick();
					}
				}
				float num = this.BiostarvationDailyOffset / 60000f * HediffDefOf.BioStarvation.maxSeverity;
				Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity += num;
					if (firstHediffOfDef.ShouldRemove)
					{
						this.selectedPawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
				else if (num > 0f)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, this.selectedPawn, null);
					hediff.Severity = num;
					this.selectedPawn.health.AddHediff(hediff, null, null, null);
				}
			}
			else if (this.selectedEmbryo != null)
			{
				if (this.EmbryoGestationTicksRemaining <= 0)
				{
					this.Finish();
					return;
				}
				this.embryoStarvation = Mathf.Clamp01(this.embryoStarvation + this.BiostarvationDailyOffset / 60000f);
			}
			if (this.BiostarvationSeverityPercent >= 1f)
			{
				this.Fail();
				return;
			}
			if (this.sustainerWorking == null || this.sustainerWorking.Ended)
			{
				this.sustainerWorking = SoundDefOf.GrowthVat_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
			else
			{
				this.sustainerWorking.Maintain();
			}
			this.containedNutrition = Mathf.Clamp(this.containedNutrition - this.NutritionConsumedPerDay / 60000f, 0f, 2.1474836E+09f);
			if (this.containedNutrition <= 0f)
			{
				this.TryAbsorbNutritiousThing();
			}
			if (Building_GrowthVat.GlowMotePerRotation == null)
			{
				Building_GrowthVat.GlowMotePerRotation = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.Mote_VatGlowVertical
					},
					{
						Rot4.East,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.West,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.North,
						ThingDefOf.Mote_VatGlowVertical
					}
				};
				Building_GrowthVat.BubbleEffecterPerRotation = new Dictionary<Rot4, EffecterDef>
				{
					{
						Rot4.South,
						EffecterDefOf.Vat_Bubbles_South
					},
					{
						Rot4.East,
						EffecterDefOf.Vat_Bubbles_East
					},
					{
						Rot4.West,
						EffecterDefOf.Vat_Bubbles_West
					},
					{
						Rot4.North,
						EffecterDefOf.Vat_Bubbles_North
					}
				};
			}
			if (this.IsHashIntervalTick(132))
			{
				MoteMaker.MakeStaticMote(this.DrawPos, base.MapHeld, Building_GrowthVat.GlowMotePerRotation[base.Rotation], 1f, false, 0f);
			}
			if (this.bubbleEffecter == null)
			{
				this.bubbleEffecter = Building_GrowthVat.BubbleEffecterPerRotation[base.Rotation].SpawnAttached(this, base.MapHeld, 1f);
			}
			this.bubbleEffecter.EffectTick(this, this);
		}

		// Token: 0x0600EED4 RID: 61140 RVA: 0x00454C40 File Offset: 0x00452E40
		public override AcceptanceReport CanAcceptPawn(Pawn pawn)
		{
			if (base.Working)
			{
				return "Occupied".Translate();
			}
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.selectedEmbryo != null)
			{
				return "EmbryoSelected".Translate();
			}
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
			{
				return "TooOld".Translate(pawn.Named("PAWN"), 18f.Named("AGEYEARS"));
			}
			if (this.selectedPawn != null && this.selectedPawn != pawn)
			{
				return "WaitingForPawn".Translate(this.selectedPawn.Named("PAWN"));
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioStarvation, false))
			{
				return "PawnBiostarving".Translate(pawn.Named("PAWN"));
			}
			return pawn.IsColonist && !pawn.IsQuestLodger();
		}

		// Token: 0x0600EED5 RID: 61141 RVA: 0x00454D5C File Offset: 0x00452F5C
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (this.selectedPawn != null && this.CanAcceptPawn(pawn))
			{
				this.selectedPawn = pawn;
				bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.innerContainer.TryAddOrTransfer(pawn, true))
				{
					SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
					this.startTick = Find.TickManager.TicksGame;
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatLearning, false))
					{
						pawn.health.AddHediff(HediffDefOf.VatLearning, null, null, null);
					}
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatGrowing, false))
					{
						pawn.health.AddHediff(HediffDefOf.VatGrowing, null, null, null);
					}
				}
				if (flag)
				{
					Find.Selector.Select(pawn, false, false);
				}
			}
		}

		// Token: 0x0600EED6 RID: 61142 RVA: 0x00454E44 File Offset: 0x00453044
		private void TryGrowEmbryo()
		{
			if (base.Working || !this.PowerOn)
			{
				return;
			}
			if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
				this.startTick = Find.TickManager.TicksGame + 540000;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Color color = this.EmbryoColor();
					this.fetusEarlyStageGraphic = this.FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
					this.fetusLateStageGraphic = this.FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				});
				if (this.selectedPawn != null)
				{
					Log.Error("Growing embryo while pawn was somehow marked as selected");
					this.selectedPawn = null;
				}
			}
		}

		// Token: 0x0600EED7 RID: 61143 RVA: 0x00454ED4 File Offset: 0x004530D4
		private void TryAbsorbNutritiousThing()
		{
			for (int i = 0; i < this.innerContainer.Count; i++)
			{
				if (this.innerContainer[i] != this.selectedPawn && this.innerContainer[i].def != ThingDefOf.Xenogerm && this.innerContainer[i].def != ThingDefOf.HumanEmbryo)
				{
					float statValue = this.innerContainer[i].GetStatValue(StatDefOf.Nutrition, true, -1);
					if (statValue > 0f)
					{
						this.containedNutrition += statValue;
						this.innerContainer[i].SplitOff(1).Destroy(DestroyMode.Vanish);
						return;
					}
				}
			}
		}

		// Token: 0x0600EED8 RID: 61144 RVA: 0x00454F89 File Offset: 0x00453189
		private void Finish()
		{
			if (this.selectedPawn != null)
			{
				this.FinishPawn();
				return;
			}
			if (this.selectedEmbryo != null)
			{
				this.FinishEmbryo();
			}
		}

		// Token: 0x0600EED9 RID: 61145 RVA: 0x00454FA8 File Offset: 0x004531A8
		private void FinishEmbryo()
		{
			this.EmbryoBirth();
			this.DestroyEmbryo(false);
			this.OnStop();
		}

		// Token: 0x0600EEDA RID: 61146 RVA: 0x00454FC0 File Offset: 0x004531C0
		private void FinishPawn()
		{
			if (this.selectedPawn == null || !this.innerContainer.Contains(this.selectedPawn))
			{
				return;
			}
			this.Notify_PawnRemoved();
			Thing thing;
			this.innerContainer.TryDrop(this.selectedPawn, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
			this.OnStop();
		}

		// Token: 0x0600EEDB RID: 61147 RVA: 0x0045501C File Offset: 0x0045321C
		private void Fail()
		{
			if (this.innerContainer.Contains(this.selectedPawn))
			{
				this.Notify_PawnRemoved();
				Thing thing;
				this.innerContainer.TryDrop(this.selectedPawn, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
				Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
				this.selectedPawn.Kill(null, firstHediffOfDef);
			}
			this.DestroyEmbryo(true);
			this.OnStop();
		}

		// Token: 0x0600EEDC RID: 61148 RVA: 0x004550A4 File Offset: 0x004532A4
		private void OnStop()
		{
			this.selectedPawn = null;
			this.selectedEmbryo = null;
			this.startTick = -1;
			this.embryoStarvation = 0f;
			this.sustainerWorking = null;
			this.cachedVatLearning = null;
		}

		// Token: 0x0600EEDD RID: 61149 RVA: 0x004550D4 File Offset: 0x004532D4
		private void DestroyEmbryo(bool biostarvation = false)
		{
			if (this.startTick >= 0 && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				if (this.startTick > Find.TickManager.TicksGame)
				{
					if (biostarvation)
					{
						Messages.Message("EmbryoEjectedFromGrowthVatBiostarvation".Translate(this.selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent, true);
					}
					else
					{
						Messages.Message("EmbryoEjectedFromGrowthVat".Translate(this.selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				this.innerContainer.Remove(this.selectedEmbryo);
				this.selectedEmbryo.Destroy(DestroyMode.Vanish);
				this.selectedEmbryo = null;
			}
		}

		// Token: 0x0600EEDE RID: 61150 RVA: 0x004551AC File Offset: 0x004533AC
		private void EmbryoBirth()
		{
			if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo) && this.startTick <= Find.TickManager.TicksGame)
			{
				Precept_Ritual precept_Ritual = Faction.OfPlayer.ideos.PrimaryIdeo.GetPrecept(PreceptDefOf.ChildBirth) as Precept_Ritual;
				RitualOutcomePossibility outcome = ((RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance()).GetOutcome(0.7f, null);
				float quality = 0.7f;
				Precept_Ritual ritual = precept_Ritual;
				HumanEmbryo humanEmbryo = this.selectedEmbryo;
				List<GeneDef> genes;
				if (humanEmbryo == null)
				{
					genes = null;
				}
				else
				{
					GeneSet geneSet = humanEmbryo.GeneSet;
					genes = ((geneSet != null) ? geneSet.GenesListForReading : null);
				}
				Thing thing = PregnancyUtility.ApplyBirthOutcome(outcome, quality, ritual, genes, this.selectedEmbryo.Mother, this, this.selectedEmbryo.Father, null, null, null, false);
				if (thing != null && this.embryoStarvation > 0f)
				{
					Corpse corpse = thing as Corpse;
					Pawn pawn = (corpse != null) ? corpse.InnerPawn : ((Pawn)thing);
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, pawn, null);
					hediff.Severity = Mathf.Lerp(0f, HediffDefOf.BioStarvation.maxSeverity, this.embryoStarvation);
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
		}

		// Token: 0x0600EEDF RID: 61151 RVA: 0x004552DE File Offset: 0x004534DE
		private void Notify_PawnRemoved()
		{
			SoundDefOf.GrowthVat_Open.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
		}

		// Token: 0x0600EEE0 RID: 61152 RVA: 0x004552F6 File Offset: 0x004534F6
		public bool CanAcceptNutrition(Thing thing)
		{
			return this.allowedNutritionSettings.AllowedToAccept(thing);
		}

		// Token: 0x0600EEE1 RID: 61153 RVA: 0x00455304 File Offset: 0x00453504
		public StorageSettings GetStoreSettings()
		{
			return this.allowedNutritionSettings;
		}

		// Token: 0x0600EEE2 RID: 61154 RVA: 0x00440EF5 File Offset: 0x0043F0F5
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600EEE3 RID: 61155 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0600EEE4 RID: 61156 RVA: 0x0045530C File Offset: 0x0045350C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.allowedNutritionSettings))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (base.Working)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelGrowth".Translate(),
					defaultDesc = "CommandCancelGrowthDesc".Translate(),
					icon = Building_GrowthVat.CancelLoadingIcon,
					activateSound = SoundDefOf.Designate_Cancel,
					action = delegate()
					{
						Action action = delegate()
						{
							this.Finish();
							this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
						};
						if (this.startTick > Find.TickManager.TicksGame && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
						{
							Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ImplantEmbryoCancelVat".Translate(this.selectedEmbryo.Label), action, true, null, WindowLayer.Dialog);
							Find.WindowStack.Add(window);
							return;
						}
						action();
					}
				};
				if (this.selectedEmbryo != null)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "InspectGenes".Translate() + "...";
					command_Action.defaultDesc = "InspectGenesEmbryoDesc".Translate();
					command_Action.icon = GeneSetHolderBase.GeneticInfoTex.Texture;
					command_Action.action = delegate()
					{
						InspectPaneUtility.OpenTab(typeof(ITab_Genes));
					};
					yield return command_Action;
				}
				if (DebugSettings.ShowDevGizmos)
				{
					if (this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Advance 1 year",
							action = delegate()
							{
								this.selectedPawn.ageTracker.Notify_TickedInGrowthVat(3600000);
							}
						};
						yield return new Command_Action
						{
							defaultLabel = "DEV: Learn",
							action = new Action(((Hediff_VatLearning)this.VatLearning).Learn)
						};
					}
					if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Advance gestation 1 day",
							action = delegate()
							{
								this.startTick = Mathf.Max(0, this.startTick - 60000);
							}
						};
						yield return new Command_Action
						{
							defaultLabel = "DEV: Embryo birth now",
							action = delegate()
							{
								this.startTick = Find.TickManager.TicksGame;
								this.Finish();
							}
						};
					}
				}
			}
			else
			{
				if (this.selectedPawn != null || this.selectedEmbryo != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandCancelLoad".Translate(),
						defaultDesc = "CommandCancelLoadDesc".Translate(),
						icon = Building_GrowthVat.CancelLoadingIcon,
						activateSound = SoundDefOf.Designate_Cancel,
						action = delegate()
						{
							this.DestroyEmbryo(false);
							this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
							if (this.innerContainer.Contains(this.selectedPawn))
							{
								this.Notify_PawnRemoved();
							}
							Pawn selectedPawn = this.selectedPawn;
							if (((selectedPawn != null) ? selectedPawn.CurJobDef : null) == JobDefOf.EnterBuilding)
							{
								this.selectedPawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
							this.OnStop();
						}
					};
				}
				if (this.selectedPawn == null)
				{
					Command_Action command_Action2 = new Command_Action();
					command_Action2.defaultLabel = "InsertPerson".Translate() + "...";
					command_Action2.defaultDesc = "InsertPersonGrowthVatDesc".Translate();
					command_Action2.icon = Building_GrowthVat.InsertPawnIcon.Texture;
					command_Action2.action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						foreach (Pawn pawn2 in base.Map.mapPawns.AllPawnsSpawned)
						{
							Pawn pawn = pawn2;
							if (this.CanAcceptPawn(pawn2))
							{
								list.Add(new FloatMenuOption(pawn2.LabelCap, delegate()
								{
									this.SelectPawn(pawn);
								}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						if (!list.Any<FloatMenuOption>())
						{
							list.Add(new FloatMenuOption("NoViablePawns".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					};
					if (this.selectedEmbryo != null)
					{
						command_Action2.Disable("EmbryoSelected".Translate().CapitalizeFirst());
					}
					else if (!this.PowerOn)
					{
						command_Action2.Disable("NoPower".Translate().CapitalizeFirst());
					}
					else if (!base.AnyAcceptablePawns)
					{
						command_Action2.Disable("NoPawnsCanEnterGrowthVat".Translate(18f).ToString());
					}
					yield return command_Action2;
				}
				if (this.selectedEmbryo == null && Find.Storyteller.difficulty.ChildrenAllowed)
				{
					List<Thing> embryos = base.Map.listerThings.ThingsOfDef(ThingDefOf.HumanEmbryo);
					Command_Action command_Action3 = new Command_Action();
					command_Action3.defaultLabel = "ImplantEmbryo".Translate() + "...";
					command_Action3.defaultDesc = "InsertEmbryoGrowthVatDesc".Translate(540000.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
					command_Action3.icon = Building_GrowthVat.InsertEmbryoIcon.Texture;
					command_Action3.action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						using (List<Thing>.Enumerator enumerator2 = embryos.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Thing embryo = enumerator2.Current;
								list.Add(new FloatMenuOption(embryo.LabelCap, delegate()
								{
									this.SelectEmbryo(embryo as HumanEmbryo);
								}, embryo, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					};
					if (embryos.NullOrEmpty<Thing>())
					{
						command_Action3.Disable("ImplantNoEmbryos".Translate().CapitalizeFirst());
					}
					else if (this.selectedPawn != null)
					{
						command_Action3.Disable("PersonSelected".Translate().CapitalizeFirst());
					}
					else if (!this.PowerOn)
					{
						command_Action3.Disable("NoPower".Translate().CapitalizeFirst());
					}
					yield return command_Action3;
				}
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fill nutrition",
					action = delegate()
					{
						this.containedNutrition = 10f;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Empty nutrition",
					action = delegate()
					{
						this.containedNutrition = 0f;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EEE5 RID: 61157 RVA: 0x0045531C File Offset: 0x0045351C
		public void SelectEmbryo(HumanEmbryo embryo)
		{
			this.selectedEmbryo = embryo;
			embryo.implantTarget = this;
		}

		// Token: 0x0600EEE6 RID: 61158 RVA: 0x0045532C File Offset: 0x0045352C
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (base.Working && this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
			{
				this.selectedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + this.PawnDrawOffset, null, true);
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600EEE7 RID: 61159 RVA: 0x00455394 File Offset: 0x00453594
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (base.Working && this.selectedPawn == null && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				Vector2 drawSize = Vector2.one * Mathf.Lerp(0.4f, 0.95f, this.EmbryoGestationPct);
				if (this.EmbryoGestationTicksRemaining > 540000)
				{
					this.FetusEarlyStage.drawSize = drawSize;
					this.FetusEarlyStage.DrawFromDef(this.DrawPos + this.PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null, 0f);
				}
				else
				{
					this.FetusLateStage.drawSize = drawSize;
					this.FetusLateStage.DrawFromDef(this.DrawPos + this.PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null, 0f);
				}
			}
			this.TopGraphic.Draw(this.DrawPos + Altitudes.AltIncVect * 2f, base.Rotation, this, 0f);
		}

		// Token: 0x0600EEE8 RID: 61160 RVA: 0x004554D0 File Offset: 0x004536D0
		private Color EmbryoColor()
		{
			Color result = PawnSkinColors.GetSkinColor(0.5f);
			HumanEmbryo humanEmbryo = this.selectedEmbryo;
			if (((humanEmbryo != null) ? humanEmbryo.GeneSet : null) != null)
			{
				foreach (GeneDef geneDef in this.selectedEmbryo.GeneSet.GenesListForReading)
				{
					if (geneDef.skinColorOverride != null)
					{
						return geneDef.skinColorOverride.Value;
					}
					if (geneDef.skinColorBase != null)
					{
						result = geneDef.skinColorBase.Value;
					}
				}
				return result;
			}
			return result;
		}

		// Token: 0x0600EEE9 RID: 61161 RVA: 0x0045557C File Offset: 0x0045377C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.selectedEmbryo != null && this.selectedEmbryo.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.selectedEmbryo.TrueCenter());
			}
		}

		// Token: 0x0600EEEA RID: 61162 RVA: 0x004555B8 File Offset: 0x004537B8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.Working)
			{
				if (this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
				{
					stringBuilder.AppendLineIfNotEmpty().Append(string.Format("{0}: {1}, {2}", "CasketContains".Translate().ToString(), this.selectedPawn.NameShortColored.Resolve(), this.selectedPawn.ageTracker.AgeBiologicalYears));
				}
				if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
				{
					stringBuilder.AppendLineIfNotEmpty().AppendLine("Gestating".Translate() + ": " + this.selectedEmbryo.Label.CapitalizeFirst());
					stringBuilder.AppendLineTagged("EmbryoTimeUntilBirth".Translate() + ": " + this.EmbryoGestationTicksRemaining.ToStringTicksToDays("F1").Colorize(ColoredText.DateTimeColor));
					stringBuilder.Append("EmbryoBirthQuality".Translate() + ": " + 0.7f.ToStringPercent());
				}
				float biostarvationSeverityPercent = this.BiostarvationSeverityPercent;
				if (biostarvationSeverityPercent > 0f)
				{
					string str = (this.BiostarvationDailyOffset >= 0f) ? "+" : string.Empty;
					stringBuilder.AppendLineIfNotEmpty().Append(string.Format("{0}: {1} ({2})", "Biostarvation".Translate(), biostarvationSeverityPercent.ToStringPercent(), "PerDay".Translate(str + this.BiostarvationDailyOffset.ToStringPercent())));
				}
			}
			else if (this.selectedPawn != null)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("WaitingForPawn".Translate(this.selectedPawn.Named("PAWN")).Resolve());
			}
			stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ").Append(this.NutritionStored.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute));
			if (base.Working)
			{
				stringBuilder.Append(" (-").Append("PerDay".Translate(this.NutritionConsumedPerDay.ToString("F1"))).Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600EEEB RID: 61163 RVA: 0x00455854 File Offset: 0x00453A54
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
				else if (!acceptanceReport.Reason.NullOrEmpty())
				{
					yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EEEC RID: 61164 RVA: 0x0045586C File Offset: 0x00453A6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<HumanEmbryo>(ref this.selectedEmbryo, "selectedEmbryo", false);
			Scribe_Values.Look<float>(ref this.embryoStarvation, "embryoStarvation", 0f, false);
			Scribe_Values.Look<float>(ref this.containedNutrition, "containedNutrition", 0f, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedNutritionSettings, "allowedNutritionSettings", new object[]
			{
				this
			});
			if (this.allowedNutritionSettings == null)
			{
				this.allowedNutritionSettings = new StorageSettings(this);
				if (this.def.building.defaultStorageSettings != null)
				{
					this.allowedNutritionSettings.CopyFrom(this.def.building.defaultStorageSettings);
				}
			}
		}

		// Token: 0x04009DDB RID: 40411
		public HumanEmbryo selectedEmbryo;

		// Token: 0x04009DDC RID: 40412
		private float embryoStarvation;

		// Token: 0x04009DDD RID: 40413
		private float containedNutrition;

		// Token: 0x04009DDE RID: 40414
		private StorageSettings allowedNutritionSettings;

		// Token: 0x04009DDF RID: 40415
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04009DE0 RID: 40416
		[Unsaved(false)]
		private Graphic cachedTopGraphic;

		// Token: 0x04009DE1 RID: 40417
		[Unsaved(false)]
		private Graphic fetusEarlyStageGraphic;

		// Token: 0x04009DE2 RID: 40418
		[Unsaved(false)]
		private Graphic fetusLateStageGraphic;

		// Token: 0x04009DE3 RID: 40419
		[Unsaved(false)]
		private Sustainer sustainerWorking;

		// Token: 0x04009DE4 RID: 40420
		[Unsaved(false)]
		private Hediff cachedVatLearning;

		// Token: 0x04009DE5 RID: 40421
		[Unsaved(false)]
		private Effecter bubbleEffecter;

		// Token: 0x04009DE6 RID: 40422
		private static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04009DE7 RID: 40423
		public static readonly CachedTexture InsertPawnIcon = new CachedTexture("UI/Gizmos/InsertPawn");

		// Token: 0x04009DE8 RID: 40424
		public static readonly CachedTexture InsertEmbryoIcon = new CachedTexture("UI/Gizmos/InsertEmbryo");

		// Token: 0x04009DE9 RID: 40425
		private const float BiostarvationGainPerDayNoFoodOrPower = 0.5f;

		// Token: 0x04009DEA RID: 40426
		private const float BiostarvationFallPerDayPoweredAndFed = 0.1f;

		// Token: 0x04009DEB RID: 40427
		private const float BasePawnConsumedNutritionPerDay = 3f;

		// Token: 0x04009DEC RID: 40428
		private const float BaseEmbryoConsumedNutritionPerDay = 6f;

		// Token: 0x04009DED RID: 40429
		private const float AgeToEject = 18f;

		// Token: 0x04009DEE RID: 40430
		public const float NutritionBuffer = 10f;

		// Token: 0x04009DEF RID: 40431
		public const int AgeTicksPerTickInGrowthVat = 20;

		// Token: 0x04009DF0 RID: 40432
		private const float EmbryoBirthQuality = 0.7f;

		// Token: 0x04009DF1 RID: 40433
		public const int EmbryoGestationTicks = 540000;

		// Token: 0x04009DF2 RID: 40434
		private const int EmbryoLateStageGraphicTicksRemaining = 540000;

		// Token: 0x04009DF3 RID: 40435
		private const float FetusMinSize = 0.4f;

		// Token: 0x04009DF4 RID: 40436
		private const float FetusMaxSize = 0.95f;

		// Token: 0x04009DF5 RID: 40437
		private const int GlowIntervalTicks = 132;

		// Token: 0x04009DF6 RID: 40438
		private static Dictionary<Rot4, ThingDef> GlowMotePerRotation;

		// Token: 0x04009DF7 RID: 40439
		private static Dictionary<Rot4, EffecterDef> BubbleEffecterPerRotation;
	}
}
