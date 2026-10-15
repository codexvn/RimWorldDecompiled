using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_MechGestator : Building_WorkTableAutonomous
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__32 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_MechGestator _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_MechGestator CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_WorkTableAutonomous)CS_0024_003C_003E8__locals6).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals6.ActiveMechBill != null && CS_0024_003C_003E8__locals6.ActiveMechBill.State != 0 && CS_0024_003C_003E8__locals6.ActiveMechBill.State != FormingState.Formed)
					{
						_003C_003E2__current = new Command_Action
						{
							action = CS_0024_003C_003E8__locals6.ActiveMechBill.ForceCompleteAllCycles,
							defaultLabel = "DEV: Complete all cycles"
						};
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							action = delegate
							{
								CS_0024_003C_003E8__locals6.WasteProducer.ProduceWaste(5);
							},
							defaultLabel = "DEV: Generate 5 waste"
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private Mote workingMote;

	private Sustainer workingSound;

	private Graphic cylinderGraphic;

	private Graphic topGraphic;

	private CompPowerTrader power;

	private CompWasteProducer wasteProducer;

	private static Material FormingCycleBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.98f, 0.46f, 0f));

	private static Material FormingCycleUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 0f, 0f));

	public Bill_Mech ActiveMechBill => base.ActiveBill as Bill_Mech;

	public CompWasteProducer WasteProducer
	{
		get
		{
			if (wasteProducer == null)
			{
				wasteProducer = GetComp<CompWasteProducer>();
			}
			return wasteProducer;
		}
	}

	public CompPowerTrader Power
	{
		get
		{
			if (power == null)
			{
				power = this.TryGetComp<CompPowerTrader>();
			}
			return power;
		}
	}

	public bool PoweredOn => Power.PowerOn;

	public bool BoundPawnStateAllowsForming
	{
		get
		{
			if (ActiveMechBill.BoundPawn != null && !ActiveMechBill.BoundPawn.Dead)
			{
				return !ActiveMechBill.BoundPawn.Suspended;
			}
			return false;
		}
	}

	public Pawn GestatingMech
	{
		get
		{
			Pawn pawn = (Pawn)innerContainer.FirstOrDefault((Thing t) => t is Pawn);
			if (pawn != null)
			{
				return pawn;
			}
			return ResurrectingMechCorpse?.InnerPawn;
		}
	}

	public Corpse ResurrectingMechCorpse => (Corpse)innerContainer.FirstOrDefault((Thing t) => t is Corpse);

	public override void PostPostMake()
	{
		if (!ModLister.CheckBiotech("Mech gestator"))
		{
			Destroy();
		}
		else
		{
			base.PostPostMake();
		}
	}

	public bool CanBeUsedNowBy(Pawn user)
	{
		if (ActiveMechBill != null)
		{
			return ActiveMechBill.BoundPawn == user;
		}
		return true;
	}

	public override void Notify_StartForming(Pawn billDoer)
	{
		SoundDefOf.MechGestatorCycle_Started.PlayOneShot(this);
	}

	public override void Notify_FormingCompleted()
	{
		Pawn pawn = activeBill.CreateProducts() as Pawn;
		Messages.Message("GestationComplete".Translate() + ": " + pawn.kindDef.LabelCap, this, MessageTypeDefOf.PositiveEvent);
		innerContainer.ClearAndDestroyContents();
		innerContainer.TryAdd(pawn);
		WasteProducer.ProduceWaste((int)pawn.GetStatValue(StatDefOf.WastepacksPerRecharge));
		SoundDefOf.MechGestatorBill_Completed.PlayOneShot(this);
	}

	public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
	{
		SoundDefOf.MechGestator_MaterialInserted.PlayOneShot(this);
	}

	public override void EjectContents()
	{
		for (int num = innerContainer.Count - 1; num >= 0; num--)
		{
			if (innerContainer[num] is Pawn pawn)
			{
				innerContainer.RemoveAt(num);
				pawn.Destroy();
			}
		}
		base.EjectContents();
	}

	protected override void Tick()
	{
		base.Tick();
		if (activeBill != null && PoweredOn && BoundPawnStateAllowsForming)
		{
			activeBill.BillTick();
			ThingDef thingDef = null;
			if (ActiveMechBill.State == FormingState.Forming)
			{
				thingDef = def.building.gestatorFormingMote.GetForRotation(base.Rotation);
			}
			else if (ActiveMechBill.State == FormingState.Preparing && ActiveMechBill.GestationCyclesCompleted > 0)
			{
				thingDef = def.building.gestatorCycleCompleteMote.GetForRotation(base.Rotation);
			}
			else if (ActiveMechBill.State == FormingState.Formed)
			{
				thingDef = def.building.gestatorFormedMote.GetForRotation(base.Rotation);
			}
			if (thingDef != null)
			{
				if (workingMote == null || workingMote.Destroyed || workingMote.def != thingDef)
				{
					workingMote = MoteMaker.MakeAttachedOverlay(this, thingDef, Vector3.zero);
				}
				workingMote?.Maintain();
			}
		}
		if (this.IsHashIntervalTick(250))
		{
			if (activeBill != null && activeBill.State == FormingState.Forming)
			{
				Power.PowerOutput = 0f - Power.Props.PowerConsumption;
			}
			else
			{
				Power.PowerOutput = 0f - Power.Props.idlePowerDraw;
			}
		}
		if (activeBill != null && PoweredOn && activeBill.State != 0)
		{
			if (workingSound == null || workingSound.Ended)
			{
				workingSound = SoundDefOf.MechGestator_Ambience.TrySpawnSustainer(this);
			}
			workingSound.Maintain();
		}
		else if (workingSound != null)
		{
			workingSound.End();
			workingSound = null;
		}
	}

	protected override string GetInspectStringExtra()
	{
		if (activeBill is Bill_Mech { State: FormingState.Forming } bill_Mech)
		{
			return $"{"GestatingInspect".Translate()}: {Mathf.CeilToInt(bill_Mech.formingTicks * 1f / bill_Mech.WorkSpeedMultiplier).ToStringTicksToPeriod()}";
		}
		return null;
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (activeBill != null && activeBill.State != 0 && def.building.formingGraphicData != null)
		{
			Vector3 loc = drawLoc + def.building.formingMechPerRotationOffset[base.Rotation.AsInt];
			loc.y += 0.018292684f;
			loc.z += Mathf.PingPong((float)Find.TickManager.TicksGame * def.building.formingMechBobSpeed, def.building.formingMechYBobDistance);
			if (TryGetMechFormingGraphic(out var graphic))
			{
				graphic.Draw(loc, Rot4.South, this);
			}
			else
			{
				def.building.formingGraphicData.Graphic.Draw(loc, Rot4.North, this);
			}
		}
		GenDraw.FillableBarRequest barDrawData = base.BarDrawData;
		barDrawData.center = drawLoc;
		barDrawData.fillPercent = base.CurrentBillFormingPercent;
		barDrawData.filledMat = FormingCycleBarFilledMat;
		barDrawData.unfilledMat = FormingCycleUnfilledMat;
		barDrawData.rotation = base.Rotation;
		GenDraw.DrawFillableBar(barDrawData);
		if (topGraphic == null)
		{
			topGraphic = def.building.mechGestatorTopGraphic.GraphicColoredFor(this);
		}
		if (cylinderGraphic == null)
		{
			cylinderGraphic = def.building.mechGestatorCylinderGraphic.GraphicColoredFor(this);
		}
		Vector3 loc2 = new Vector3(drawLoc.x, AltitudeLayer.BuildingBelowTop.AltitudeFor(), drawLoc.z);
		cylinderGraphic.Draw(loc2, base.Rotation, this);
		Vector3 loc3 = new Vector3(drawLoc.x, AltitudeLayer.BuildingOnTop.AltitudeFor(), drawLoc.z);
		topGraphic.Draw(loc3, base.Rotation, this);
	}

	private bool TryGetMechFormingGraphic(out Graphic graphic)
	{
		graphic = null;
		if (ResurrectingMechCorpse != null)
		{
			graphic = ResurrectingMechCorpse.InnerPawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
		}
		else if (GestatingMech != null)
		{
			graphic = GestatingMech.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
		}
		if (graphic != null && graphic.drawSize.x <= def.building.maxFormedMechDrawSize.x && graphic.drawSize.y <= def.building.maxFormedMechDrawSize.y)
		{
			return true;
		}
		graphic = null;
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__32))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.PostLoadInit && ActiveMechBill is Bill_ResurrectMech { State: not FormingState.Gathering } && GestatingMech == null)
		{
			Log.Error($"Bill {base.ActiveBill} with recipe {base.ActiveBill.recipe} loaded with no mech to resurrect. Resetting bill.");
			base.ActiveBill.Reset();
		}
	}
}
