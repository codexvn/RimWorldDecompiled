using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A6F RID: 10863
	[StaticConstructorOnStartup]
	public class Building_MechGestator : Building_WorkTableAutonomous
	{
		// Token: 0x170025C6 RID: 9670
		// (get) Token: 0x0600F1BD RID: 61885 RVA: 0x00462023 File Offset: 0x00460223
		public Bill_Mech ActiveMechBill
		{
			get
			{
				return base.ActiveBill as Bill_Mech;
			}
		}

		// Token: 0x170025C7 RID: 9671
		// (get) Token: 0x0600F1BE RID: 61886 RVA: 0x00462030 File Offset: 0x00460230
		public CompWasteProducer WasteProducer
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

		// Token: 0x170025C8 RID: 9672
		// (get) Token: 0x0600F1BF RID: 61887 RVA: 0x0046204C File Offset: 0x0046024C
		public CompPowerTrader Power
		{
			get
			{
				if (this.power == null)
				{
					this.power = this.TryGetComp<CompPowerTrader>();
				}
				return this.power;
			}
		}

		// Token: 0x170025C9 RID: 9673
		// (get) Token: 0x0600F1C0 RID: 61888 RVA: 0x00462068 File Offset: 0x00460268
		public bool PoweredOn
		{
			get
			{
				return this.Power.PowerOn;
			}
		}

		// Token: 0x170025CA RID: 9674
		// (get) Token: 0x0600F1C1 RID: 61889 RVA: 0x00462075 File Offset: 0x00460275
		public bool BoundPawnStateAllowsForming
		{
			get
			{
				return this.ActiveMechBill.BoundPawn != null && !this.ActiveMechBill.BoundPawn.Dead && !this.ActiveMechBill.BoundPawn.Suspended;
			}
		}

		// Token: 0x170025CB RID: 9675
		// (get) Token: 0x0600F1C2 RID: 61890 RVA: 0x004620AC File Offset: 0x004602AC
		public Pawn GestatingMech
		{
			get
			{
				Pawn pawn = (Pawn)this.innerContainer.FirstOrDefault((Thing t) => t is Pawn);
				if (pawn != null)
				{
					return pawn;
				}
				Corpse resurrectingMechCorpse = this.ResurrectingMechCorpse;
				if (resurrectingMechCorpse != null)
				{
					return resurrectingMechCorpse.InnerPawn;
				}
				return null;
			}
		}

		// Token: 0x170025CC RID: 9676
		// (get) Token: 0x0600F1C3 RID: 61891 RVA: 0x00462100 File Offset: 0x00460300
		public Corpse ResurrectingMechCorpse
		{
			get
			{
				return (Corpse)this.innerContainer.FirstOrDefault((Thing t) => t is Corpse);
			}
		}

		// Token: 0x0600F1C4 RID: 61892 RVA: 0x00462131 File Offset: 0x00460331
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Mech gestator"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600F1C5 RID: 61893 RVA: 0x0046214D File Offset: 0x0046034D
		public bool CanBeUsedNowBy(Pawn user)
		{
			return this.ActiveMechBill == null || this.ActiveMechBill.BoundPawn == user;
		}

		// Token: 0x0600F1C6 RID: 61894 RVA: 0x00462167 File Offset: 0x00460367
		public override void Notify_StartForming(Pawn billDoer)
		{
			SoundDefOf.MechGestatorCycle_Started.PlayOneShot(this);
		}

		// Token: 0x0600F1C7 RID: 61895 RVA: 0x0046217C File Offset: 0x0046037C
		public override void Notify_FormingCompleted()
		{
			Pawn pawn = this.activeBill.CreateProducts() as Pawn;
			Messages.Message("GestationComplete".Translate() + ": " + pawn.kindDef.LabelCap, this, MessageTypeDefOf.PositiveEvent, true);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			this.innerContainer.TryAdd(pawn, true);
			this.WasteProducer.ProduceWaste((int)pawn.GetStatValue(StatDefOf.WastepacksPerRecharge, true, -1));
			SoundDefOf.MechGestatorBill_Completed.PlayOneShot(this);
		}

		// Token: 0x0600F1C8 RID: 61896 RVA: 0x00462217 File Offset: 0x00460417
		public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			SoundDefOf.MechGestator_MaterialInserted.PlayOneShot(this);
		}

		// Token: 0x0600F1C9 RID: 61897 RVA: 0x0046222C File Offset: 0x0046042C
		public override void EjectContents()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.innerContainer[i] as Pawn;
				if (pawn != null)
				{
					this.innerContainer.RemoveAt(i);
					pawn.Destroy(DestroyMode.Vanish);
				}
			}
			base.EjectContents();
		}

		// Token: 0x0600F1CA RID: 61898 RVA: 0x00462280 File Offset: 0x00460480
		protected override void Tick()
		{
			base.Tick();
			if (this.activeBill != null && this.PoweredOn && this.BoundPawnStateAllowsForming)
			{
				this.activeBill.BillTick();
				ThingDef thingDef = null;
				if (this.ActiveMechBill.State == FormingState.Forming)
				{
					thingDef = this.def.building.gestatorFormingMote.GetForRotation(base.Rotation);
				}
				else if (this.ActiveMechBill.State == FormingState.Preparing && this.ActiveMechBill.GestationCyclesCompleted > 0)
				{
					thingDef = this.def.building.gestatorCycleCompleteMote.GetForRotation(base.Rotation);
				}
				else if (this.ActiveMechBill.State == FormingState.Formed)
				{
					thingDef = this.def.building.gestatorFormedMote.GetForRotation(base.Rotation);
				}
				if (thingDef != null)
				{
					if (this.workingMote == null || this.workingMote.Destroyed || this.workingMote.def != thingDef)
					{
						this.workingMote = MoteMaker.MakeAttachedOverlay(this, thingDef, Vector3.zero, 1f, -1f);
					}
					Mote mote = this.workingMote;
					if (mote != null)
					{
						mote.Maintain();
					}
				}
			}
			if (this.IsHashIntervalTick(250))
			{
				if (this.activeBill != null && this.activeBill.State == FormingState.Forming)
				{
					this.Power.PowerOutput = -this.Power.Props.PowerConsumption;
				}
				else
				{
					this.Power.PowerOutput = -this.Power.Props.idlePowerDraw;
				}
			}
			if (this.activeBill != null && this.PoweredOn && this.activeBill.State != FormingState.Gathering)
			{
				if (this.workingSound == null || this.workingSound.Ended)
				{
					this.workingSound = SoundDefOf.MechGestator_Ambience.TrySpawnSustainer(this);
				}
				this.workingSound.Maintain();
				return;
			}
			if (this.workingSound != null)
			{
				this.workingSound.End();
				this.workingSound = null;
			}
		}

		// Token: 0x0600F1CB RID: 61899 RVA: 0x00462470 File Offset: 0x00460670
		protected override string GetInspectStringExtra()
		{
			Bill_Mech bill_Mech = this.activeBill as Bill_Mech;
			if (bill_Mech == null || bill_Mech.State != FormingState.Forming)
			{
				return null;
			}
			return string.Format("{0}: {1}", "GestatingInspect".Translate(), Mathf.CeilToInt(bill_Mech.formingTicks * 1f / bill_Mech.WorkSpeedMultiplier).ToStringTicksToPeriod(true, false, true, true, false));
		}

		// Token: 0x0600F1CC RID: 61900 RVA: 0x004624D4 File Offset: 0x004606D4
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.activeBill != null && this.activeBill.State != FormingState.Gathering && this.def.building.formingGraphicData != null)
			{
				Vector3 loc = drawLoc + this.def.building.formingMechPerRotationOffset[base.Rotation.AsInt];
				loc.y += 0.018292684f;
				loc.z += Mathf.PingPong((float)Find.TickManager.TicksGame * this.def.building.formingMechBobSpeed, this.def.building.formingMechYBobDistance);
				Graphic graphic;
				if (this.TryGetMechFormingGraphic(out graphic))
				{
					graphic.Draw(loc, Rot4.South, this, 0f);
				}
				else
				{
					this.def.building.formingGraphicData.Graphic.Draw(loc, Rot4.North, this, 0f);
				}
			}
			GenDraw.FillableBarRequest barDrawData = base.BarDrawData;
			barDrawData.center = drawLoc;
			barDrawData.fillPercent = base.CurrentBillFormingPercent;
			barDrawData.filledMat = Building_MechGestator.FormingCycleBarFilledMat;
			barDrawData.unfilledMat = Building_MechGestator.FormingCycleUnfilledMat;
			barDrawData.rotation = base.Rotation;
			GenDraw.DrawFillableBar(barDrawData);
			if (this.topGraphic == null)
			{
				this.topGraphic = this.def.building.mechGestatorTopGraphic.GraphicColoredFor(this);
			}
			if (this.cylinderGraphic == null)
			{
				this.cylinderGraphic = this.def.building.mechGestatorCylinderGraphic.GraphicColoredFor(this);
			}
			Vector3 loc2 = new Vector3(drawLoc.x, AltitudeLayer.BuildingBelowTop.AltitudeFor(), drawLoc.z);
			this.cylinderGraphic.Draw(loc2, base.Rotation, this, 0f);
			Vector3 loc3 = new Vector3(drawLoc.x, AltitudeLayer.BuildingOnTop.AltitudeFor(), drawLoc.z);
			this.topGraphic.Draw(loc3, base.Rotation, this, 0f);
		}

		// Token: 0x0600F1CD RID: 61901 RVA: 0x004626C8 File Offset: 0x004608C8
		private bool TryGetMechFormingGraphic(out Graphic graphic)
		{
			graphic = null;
			if (this.ResurrectingMechCorpse != null)
			{
				graphic = this.ResurrectingMechCorpse.InnerPawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
			}
			else if (this.GestatingMech != null)
			{
				graphic = this.GestatingMech.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
			}
			if (graphic != null && graphic.drawSize.x <= this.def.building.maxFormedMechDrawSize.x && graphic.drawSize.y <= this.def.building.maxFormedMechDrawSize.y)
			{
				return true;
			}
			graphic = null;
			return false;
		}

		// Token: 0x0600F1CE RID: 61902 RVA: 0x00462777 File Offset: 0x00460977
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
						this.WasteProducer.ProduceWaste(5);
					},
					defaultLabel = "DEV: Generate 5 waste"
				};
				if (this.ActiveMechBill != null && this.ActiveMechBill.State != FormingState.Gathering && this.ActiveMechBill.State != FormingState.Formed)
				{
					yield return new Command_Action
					{
						action = new Action(this.ActiveMechBill.ForceCompleteAllCycles),
						defaultLabel = "DEV: Complete all cycles"
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F1CF RID: 61903 RVA: 0x00462788 File Offset: 0x00460988
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Bill_ResurrectMech bill_ResurrectMech = this.ActiveMechBill as Bill_ResurrectMech;
				if (bill_ResurrectMech != null && bill_ResurrectMech.State != FormingState.Gathering && this.GestatingMech == null)
				{
					Log.Error(string.Format("Bill {0} with recipe {1} loaded with no mech to resurrect. Resetting bill.", base.ActiveBill, base.ActiveBill.recipe));
					base.ActiveBill.Reset();
				}
			}
		}

		// Token: 0x04009F53 RID: 40787
		private Mote workingMote;

		// Token: 0x04009F54 RID: 40788
		private Sustainer workingSound;

		// Token: 0x04009F55 RID: 40789
		private Graphic cylinderGraphic;

		// Token: 0x04009F56 RID: 40790
		private Graphic topGraphic;

		// Token: 0x04009F57 RID: 40791
		private CompPowerTrader power;

		// Token: 0x04009F58 RID: 40792
		private CompWasteProducer wasteProducer;

		// Token: 0x04009F59 RID: 40793
		private static Material FormingCycleBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.98f, 0.46f, 0f), false);

		// Token: 0x04009F5A RID: 40794
		private static Material FormingCycleUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 0f, 0f), false);
	}
}
