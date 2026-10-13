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
	// Token: 0x02002A31 RID: 10801
	[StaticConstructorOnStartup]
	public class Building_MechCharger : Building
	{
		// Token: 0x1700253E RID: 9534
		// (get) Token: 0x0600EF93 RID: 61331 RVA: 0x00458B94 File Offset: 0x00456D94
		public CompPowerTrader Power
		{
			get
			{
				return this.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x1700253F RID: 9535
		// (get) Token: 0x0600EF94 RID: 61332 RVA: 0x00458B9C File Offset: 0x00456D9C
		public bool IsPowered
		{
			get
			{
				return this.Power.PowerOn;
			}
		}

		// Token: 0x17002540 RID: 9536
		// (get) Token: 0x0600EF95 RID: 61333 RVA: 0x00458BA9 File Offset: 0x00456DA9
		public bool IsFullOfWaste
		{
			get
			{
				return this.wasteProduced >= (float)this.WasteProducedPerChargingCycle && this.Container.innerContainer.Any;
			}
		}

		// Token: 0x17002541 RID: 9537
		// (get) Token: 0x0600EF96 RID: 61334 RVA: 0x00458BCC File Offset: 0x00456DCC
		private CompWasteProducer WasteProducer
		{
			get
			{
				if (this.wasteProducer == null)
				{
					this.wasteProducer = base.GetComp<CompWasteProducer>();
				}
				return this.wasteProducer;
			}
		}

		// Token: 0x17002542 RID: 9538
		// (get) Token: 0x0600EF97 RID: 61335 RVA: 0x00458BE8 File Offset: 0x00456DE8
		public CompThingContainer Container
		{
			get
			{
				if (this.container == null)
				{
					this.container = base.GetComp<CompThingContainer>();
				}
				return this.container;
			}
		}

		// Token: 0x17002543 RID: 9539
		// (get) Token: 0x0600EF98 RID: 61336 RVA: 0x00458C04 File Offset: 0x00456E04
		public GenDraw.FillableBarRequest BarDrawData
		{
			get
			{
				return this.def.building.BarDrawDataFor(base.Rotation);
			}
		}

		// Token: 0x17002544 RID: 9540
		// (get) Token: 0x0600EF99 RID: 61337 RVA: 0x00458C1C File Offset: 0x00456E1C
		private Material WireMaterial
		{
			get
			{
				if (this.wireMaterial == null)
				{
					this.wireMaterial = MaterialPool.MatFrom("Other/BundledWires", ShaderDatabase.Transparent, Color.white);
				}
				return this.wireMaterial;
			}
		}

		// Token: 0x17002545 RID: 9541
		// (get) Token: 0x0600EF9A RID: 61338 RVA: 0x00458C4C File Offset: 0x00456E4C
		private bool IsAttachedToMech
		{
			get
			{
				return this.currentlyChargingMech != null && this.wireExtensionTicks >= 70;
			}
		}

		// Token: 0x17002546 RID: 9542
		// (get) Token: 0x0600EF9B RID: 61339 RVA: 0x00458C65 File Offset: 0x00456E65
		private int WasteProducedPerChargingCycle
		{
			get
			{
				return this.Container.Props.stackLimit;
			}
		}

		// Token: 0x17002547 RID: 9543
		// (get) Token: 0x0600EF9C RID: 61340 RVA: 0x00458C77 File Offset: 0x00456E77
		private float WasteProducedPercentFull
		{
			get
			{
				return this.wasteProduced / (float)this.WasteProducedPerChargingCycle;
			}
		}

		// Token: 0x17002548 RID: 9544
		// (get) Token: 0x0600EF9D RID: 61341 RVA: 0x00458C87 File Offset: 0x00456E87
		private float WasteProducedPerTick
		{
			get
			{
				return this.currentlyChargingMech.GetStatValue(StatDefOf.WastepacksPerRecharge, true, -1) * (0.00083333335f / this.currentlyChargingMech.needs.energy.MaxLevel);
			}
		}

		// Token: 0x17002549 RID: 9545
		// (get) Token: 0x0600EF9E RID: 61342 RVA: 0x00458CB7 File Offset: 0x00456EB7
		public Pawn CurrentlyChargingMech
		{
			get
			{
				return this.currentlyChargingMech;
			}
		}

		// Token: 0x0600EF9F RID: 61343 RVA: 0x00458CBF File Offset: 0x00456EBF
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Mech recharger"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600EFA0 RID: 61344 RVA: 0x00458CDC File Offset: 0x00456EDC
		public bool CanPawnChargeCurrently(Pawn pawn)
		{
			if (this.Power.PowerNet == null)
			{
				return false;
			}
			if (this.IsFullOfWaste)
			{
				return false;
			}
			if (!this.IsCompatibleWithCharger(pawn.kindDef))
			{
				return false;
			}
			if (this.IsPowered)
			{
				if (this.currentlyChargingMech == null)
				{
					return true;
				}
				if (this.currentlyChargingMech == pawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600EFA1 RID: 61345 RVA: 0x00458D30 File Offset: 0x00456F30
		public bool IsCompatibleWithCharger(PawnKindDef kindDef)
		{
			return Building_MechCharger.IsCompatibleWithCharger(this.def, kindDef);
		}

		// Token: 0x0600EFA2 RID: 61346 RVA: 0x00458D3E File Offset: 0x00456F3E
		public static bool IsCompatibleWithCharger(ThingDef chargerDef, PawnKindDef kindDef)
		{
			return Building_MechCharger.IsCompatibleWithCharger(chargerDef, kindDef.race);
		}

		// Token: 0x0600EFA3 RID: 61347 RVA: 0x00458D4C File Offset: 0x00456F4C
		public static bool IsCompatibleWithCharger(ThingDef chargerDef, ThingDef mechRace)
		{
			return mechRace.race.IsMechanoid && mechRace.GetCompProperties<CompProperties_OverseerSubject>() != null && mechRace.race.mechWeightClass != null && chargerDef.building.requiredMechWeightClasses.NotNullAndContains(mechRace.race.mechWeightClass);
		}

		// Token: 0x0600EFA4 RID: 61348 RVA: 0x00458D98 File Offset: 0x00456F98
		protected override void Tick()
		{
			base.Tick();
			if (this.currentlyChargingMech != null && (this.currentlyChargingMech.CurJobDef != JobDefOf.MechCharge || this.currentlyChargingMech.CurJob.targetA.Thing != this))
			{
				Log.Warning("Mech did not clean up his charging job properly");
				this.StopCharging();
			}
			if (this.currentlyChargingMech != null && this.Power.PowerOn)
			{
				this.currentlyChargingMech.needs.energy.CurLevel += 0.00083333335f;
				this.wasteProduced += this.WasteProducedPerTick;
				this.wasteProduced = Mathf.Clamp(this.wasteProduced, 0f, (float)this.WasteProducedPerChargingCycle);
				if (this.wasteProduced >= (float)this.WasteProducedPerChargingCycle && !this.Container.innerContainer.Any)
				{
					this.wasteProduced = 0f;
					this.GenerateWastePack();
				}
				if (this.moteCablePulse == null || this.moteCablePulse.Destroyed)
				{
					this.moteCablePulse = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_ChargingCablesPulse, this, new TargetInfo(this.InteractionCell, base.Map, false));
				}
				Mote mote = this.moteCablePulse;
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			if (this.currentlyChargingMech != null && this.Power.PowerOn && this.IsAttachedToMech)
			{
				if (this.sustainerCharging == null)
				{
					this.sustainerCharging = SoundDefOf.MechChargerCharging.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.None));
				}
				this.sustainerCharging.Maintain();
				if (this.moteCharging == null || this.moteCharging.Destroyed)
				{
					this.moteCharging = MoteMaker.MakeAttachedOverlay(this.currentlyChargingMech, ThingDefOf.Mote_MechCharging, Vector3.zero, 1f, -1f);
				}
				Mote mote2 = this.moteCharging;
				if (mote2 != null)
				{
					mote2.Maintain();
				}
			}
			else if (this.sustainerCharging != null && (this.currentlyChargingMech == null || !this.Power.PowerOn))
			{
				this.sustainerCharging.End();
				this.sustainerCharging = null;
			}
			if (this.wireExtensionTicks < 70)
			{
				this.wireExtensionTicks++;
			}
		}

		// Token: 0x0600EFA5 RID: 61349 RVA: 0x00458FBF File Offset: 0x004571BF
		public void GenerateWastePack()
		{
			this.WasteProducer.ProduceWaste(this.WasteProducedPerChargingCycle);
			EffecterDefOf.MechChargerWasteProduced.Spawn(base.Position, base.Map, 1f).Cleanup();
		}

		// Token: 0x0600EFA6 RID: 61350 RVA: 0x00458FF2 File Offset: 0x004571F2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						this.wasteProduced = (float)this.WasteProducedPerChargingCycle;
					},
					defaultLabel = "DEV: Waste 100%"
				};
				yield return new Command_Action
				{
					action = delegate()
					{
						this.wasteProduced += 0.25f * (float)this.WasteProducedPerChargingCycle;
						this.wasteProduced = Mathf.Clamp(this.wasteProduced, 0f, (float)this.WasteProducedPerChargingCycle);
					},
					defaultLabel = "DEV: Waste +25%"
				};
				yield return new Command_Action
				{
					action = delegate()
					{
						this.wasteProduced = 0f;
					},
					defaultLabel = "DEV: Waste 0%"
				};
				if (!this.Container.innerContainer.Any)
				{
					yield return new Command_Action
					{
						action = new Action(this.GenerateWastePack),
						defaultLabel = "DEV: Generate waste"
					};
				}
				if (this.currentlyChargingMech != null)
				{
					yield return new Command_Action
					{
						action = delegate()
						{
							Need_MechEnergy need_MechEnergy;
							this.currentlyChargingMech.needs.TryGetNeed<Need_MechEnergy>(out need_MechEnergy);
							need_MechEnergy.CurLevelPercentage = 1f;
						},
						defaultLabel = "DEV: Charge 100%"
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EFA7 RID: 61351 RVA: 0x00459004 File Offset: 0x00457204
		public void StartCharging(Pawn mech)
		{
			if (!ModLister.CheckBiotech("Mech charging"))
			{
				return;
			}
			if (this.currentlyChargingMech != null)
			{
				Log.Error("Tried charging on already charging mech charger!");
				return;
			}
			if (!mech.IsColonyMech)
			{
				mech.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return;
			}
			this.currentlyChargingMech = mech;
			mech.needs.energy.currentCharger = this;
			this.wireExtensionTicks = 0;
			SoundDefOf.MechChargerStart.PlayOneShot(this);
		}

		// Token: 0x0600EFA8 RID: 61352 RVA: 0x00459078 File Offset: 0x00457278
		public void StopCharging()
		{
			if (this.currentlyChargingMech == null)
			{
				Log.Error("Tried stopping charging on currently not charging mech charger!");
				return;
			}
			if (this.currentlyChargingMech.needs.energy != null)
			{
				this.currentlyChargingMech.needs.energy.currentCharger = null;
			}
			this.currentlyChargingMech = null;
			this.wireExtensionTicks = 0;
		}

		// Token: 0x0600EFA9 RID: 61353 RVA: 0x004590D0 File Offset: 0x004572D0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.BeingTransportedOnGravship)
			{
				base.DeSpawn(mode);
				return;
			}
			if (this.currentlyChargingMech != null && mode != DestroyMode.WillReplace)
			{
				Messages.Message("MessageMechChargerDestroyedMechGoesBerserk".Translate(this.currentlyChargingMech.Named("PAWN")), new LookTargets(this.currentlyChargingMech), MessageTypeDefOf.NegativeEvent, true);
				this.currentlyChargingMech.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkMechanoid, null, false, false, false, null, false, false, false);
			}
			this.WasteProducer.ProduceWaste(Mathf.CeilToInt(this.wasteProduced));
			base.DeSpawn(mode);
		}

		// Token: 0x0600EFAA RID: 61354 RVA: 0x00459170 File Offset: 0x00457370
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			GenDraw.FillableBarRequest barDrawData = this.BarDrawData;
			barDrawData.center = this.DrawPos + Vector3.up * 0.1f;
			barDrawData.fillPercent = this.WasteProducedPercentFull;
			barDrawData.filledMat = Building_MechCharger.WasteBarFilledMat;
			barDrawData.unfilledMat = Building_MechCharger.WasteBarUnfilledMat;
			barDrawData.rotation = base.Rotation;
			GenDraw.DrawFillableBar(barDrawData);
			Vector3 a = drawLoc;
			float num = this.EaseInOutQuart((float)this.wireExtensionTicks / 70f);
			if (this.currentlyChargingMech == null)
			{
				num = 1f - num;
			}
			num = Mathf.Max(num, 0.32f);
			Vector3 b = Vector3.Lerp(drawLoc, this.InteractionCell.ToVector3Shifted(), num);
			b.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			a.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			GenDraw.DrawLineBetween(a, b, this.WireMaterial, 1f);
		}

		// Token: 0x0600EFAB RID: 61355 RVA: 0x0045925C File Offset: 0x0045745C
		private float EaseInOutQuart(float val)
		{
			if ((double)val >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * val + 2f, 4f) / 2f;
			}
			return 8f * val * val * val * val;
		}

		// Token: 0x0600EFAC RID: 61356 RVA: 0x0045929C File Offset: 0x0045749C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("WasteLevel".Translate() + ": " + this.WasteProducedPercentFull.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0600EFAD RID: 61357 RVA: 0x004592F7 File Offset: 0x004574F7
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IEnumerable<PawnKindDef> source = DefDatabase<PawnKindDef>.AllDefs.Where(new Func<PawnKindDef, bool>(this.IsCompatibleWithCharger));
			string t = (from pk in source
			select pk.LabelCap.Resolve()).ToLineList("  - ", false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "StatsReport_RechargerWeightClass".Translate(), (from w in this.def.building.requiredMechWeightClasses
			select w.label).ToCommaList(false, false).CapitalizeFirst(), "StatsReport_RechargerWeightClass_Desc".Translate() + ": " + "\n\n" + t, 99999, null, from pk in source
			select new Dialog_InfoCard.Hyperlink(pk.race, -1), false, false);
			yield break;
		}

		// Token: 0x0600EFAE RID: 61358 RVA: 0x00459308 File Offset: 0x00457508
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentlyChargingMech, "currentlyChargingMech", false);
			Scribe_Values.Look<float>(ref this.wasteProduced, "wasteProduced", 0f, false);
			Scribe_Values.Look<int>(ref this.wireExtensionTicks, "wireExtensionTicks", 0, false);
		}

		// Token: 0x04009E47 RID: 40519
		private Pawn currentlyChargingMech;

		// Token: 0x04009E48 RID: 40520
		private float wasteProduced;

		// Token: 0x04009E49 RID: 40521
		private int wireExtensionTicks = 70;

		// Token: 0x04009E4A RID: 40522
		private CompWasteProducer wasteProducer;

		// Token: 0x04009E4B RID: 40523
		private CompThingContainer container;

		// Token: 0x04009E4C RID: 40524
		private Sustainer sustainerCharging;

		// Token: 0x04009E4D RID: 40525
		private Mote moteCharging;

		// Token: 0x04009E4E RID: 40526
		private Mote moteCablePulse;

		// Token: 0x04009E4F RID: 40527
		public const float ChargePerDay = 50f;

		// Token: 0x04009E50 RID: 40528
		private const float ChargePerTick = 0.00083333335f;

		// Token: 0x04009E51 RID: 40529
		private static readonly Material WasteBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x04009E52 RID: 40530
		private static readonly Material WasteBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 1f), false);

		// Token: 0x04009E53 RID: 40531
		private const int TicksToExtendWire = 70;

		// Token: 0x04009E54 RID: 40532
		private const float MinWireExtension = 0.32f;

		// Token: 0x04009E55 RID: 40533
		private Material wireMaterial;
	}
}
