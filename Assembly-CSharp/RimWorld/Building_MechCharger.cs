using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_MechCharger : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__46 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_MechCharger _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__46(int _003C_003E1__state)
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
				Building_MechCharger CS_0024_003C_003E8__locals13 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals13).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals13.wasteProduced += 0.25f * (float)CS_0024_003C_003E8__locals13.WasteProducedPerChargingCycle;
							CS_0024_003C_003E8__locals13.wasteProduced = Mathf.Clamp(CS_0024_003C_003E8__locals13.wasteProduced, 0f, CS_0024_003C_003E8__locals13.WasteProducedPerChargingCycle);
						},
						defaultLabel = "DEV: Waste +25%"
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals13.wasteProduced = 0f;
						},
						defaultLabel = "DEV: Waste 0%"
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals13.Container.innerContainer.Any)
					{
						_003C_003E2__current = new Command_Action
						{
							action = CS_0024_003C_003E8__locals13.GenerateWastePack,
							defaultLabel = "DEV: Generate waste"
						};
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_01a9;
				case 5:
					_003C_003E1__state = -1;
					goto IL_01a9;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
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
								CS_0024_003C_003E8__locals13.wasteProduced = CS_0024_003C_003E8__locals13.WasteProducedPerChargingCycle;
							},
							defaultLabel = "DEV: Waste 100%"
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
					IL_01a9:
					if (CS_0024_003C_003E8__locals13.currentlyChargingMech != null)
					{
						_003C_003E2__current = new Command_Action
						{
							action = delegate
							{
								CS_0024_003C_003E8__locals13.currentlyChargingMech.needs.TryGetNeed(out Need_MechEnergy need);
								need.CurLevelPercentage = 1f;
							},
							defaultLabel = "DEV: Charge 100%"
						};
						_003C_003E1__state = 6;
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
			_003CGetGizmos_003Ed__46 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__46(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__53 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_MechCharger _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__53(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Building_MechCharger building_MechCharger = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				IEnumerable<PawnKindDef> source = DefDatabase<PawnKindDef>.AllDefs.Where(building_MechCharger.IsCompatibleWithCharger);
				string text = source.Select((PawnKindDef pk) => pk.LabelCap.Resolve()).ToLineList("  - ");
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "StatsReport_RechargerWeightClass".Translate(), building_MechCharger.def.building.requiredMechWeightClasses.Select((MechWeightClassDef w) => w.label).ToCommaList().CapitalizeFirst(), "StatsReport_RechargerWeightClass_Desc".Translate() + ": " + "\n\n" + text, 99999, null, source.Select((PawnKindDef pk) => new Dialog_InfoCard.Hyperlink(pk.race)));
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__53 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__53(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private Pawn currentlyChargingMech;

	private float wasteProduced;

	private int wireExtensionTicks = 70;

	private CompWasteProducer wasteProducer;

	private CompThingContainer container;

	private Sustainer sustainerCharging;

	private Mote moteCharging;

	private Mote moteCablePulse;

	public const float ChargePerDay = 50f;

	private const float ChargePerTick = 0.00083333335f;

	private static readonly Material WasteBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f));

	private static readonly Material WasteBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 1f));

	private const int TicksToExtendWire = 70;

	private const float MinWireExtension = 0.32f;

	private Material wireMaterial;

	public CompPowerTrader Power => this.TryGetComp<CompPowerTrader>();

	public bool IsPowered => Power.PowerOn;

	public bool IsFullOfWaste
	{
		get
		{
			if (wasteProduced >= (float)WasteProducedPerChargingCycle)
			{
				return Container.innerContainer.Any;
			}
			return false;
		}
	}

	private CompWasteProducer WasteProducer
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

	public CompThingContainer Container
	{
		get
		{
			if (container == null)
			{
				container = GetComp<CompThingContainer>();
			}
			return container;
		}
	}

	public GenDraw.FillableBarRequest BarDrawData => def.building.BarDrawDataFor(base.Rotation);

	private Material WireMaterial
	{
		get
		{
			if (wireMaterial == null)
			{
				wireMaterial = MaterialPool.MatFrom("Other/BundledWires", ShaderDatabase.Transparent, Color.white);
			}
			return wireMaterial;
		}
	}

	private bool IsAttachedToMech
	{
		get
		{
			if (currentlyChargingMech != null)
			{
				return wireExtensionTicks >= 70;
			}
			return false;
		}
	}

	private int WasteProducedPerChargingCycle => Container.Props.stackLimit;

	private float WasteProducedPercentFull => wasteProduced / (float)WasteProducedPerChargingCycle;

	private float WasteProducedPerTick => currentlyChargingMech.GetStatValue(StatDefOf.WastepacksPerRecharge) * (0.00083333335f / currentlyChargingMech.needs.energy.MaxLevel);

	public Pawn CurrentlyChargingMech => currentlyChargingMech;

	public override void PostPostMake()
	{
		if (!ModLister.CheckBiotech("Mech recharger"))
		{
			Destroy();
		}
		else
		{
			base.PostPostMake();
		}
	}

	public bool CanPawnChargeCurrently(Pawn pawn)
	{
		if (Power.PowerNet == null)
		{
			return false;
		}
		if (IsFullOfWaste)
		{
			return false;
		}
		if (!IsCompatibleWithCharger(pawn.kindDef))
		{
			return false;
		}
		if (IsPowered)
		{
			if (currentlyChargingMech == null)
			{
				return true;
			}
			if (currentlyChargingMech == pawn)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsCompatibleWithCharger(PawnKindDef kindDef)
	{
		return IsCompatibleWithCharger(def, kindDef);
	}

	public static bool IsCompatibleWithCharger(ThingDef chargerDef, PawnKindDef kindDef)
	{
		return IsCompatibleWithCharger(chargerDef, kindDef.race);
	}

	public static bool IsCompatibleWithCharger(ThingDef chargerDef, ThingDef mechRace)
	{
		if (mechRace.race.IsMechanoid && mechRace.GetCompProperties<CompProperties_OverseerSubject>() != null && mechRace.race.mechWeightClass != null)
		{
			return chargerDef.building.requiredMechWeightClasses.NotNullAndContains(mechRace.race.mechWeightClass);
		}
		return false;
	}

	protected override void Tick()
	{
		base.Tick();
		if (currentlyChargingMech != null && (currentlyChargingMech.CurJobDef != JobDefOf.MechCharge || currentlyChargingMech.CurJob.targetA.Thing != this))
		{
			Log.Warning("Mech did not clean up his charging job properly");
			StopCharging();
		}
		if (currentlyChargingMech != null && Power.PowerOn)
		{
			currentlyChargingMech.needs.energy.CurLevel += 0.00083333335f;
			wasteProduced += WasteProducedPerTick;
			wasteProduced = Mathf.Clamp(wasteProduced, 0f, WasteProducedPerChargingCycle);
			if (wasteProduced >= (float)WasteProducedPerChargingCycle && !Container.innerContainer.Any)
			{
				wasteProduced = 0f;
				GenerateWastePack();
			}
			if (moteCablePulse == null || moteCablePulse.Destroyed)
			{
				moteCablePulse = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_ChargingCablesPulse, this, new TargetInfo(InteractionCell, base.Map));
			}
			moteCablePulse?.Maintain();
		}
		if (currentlyChargingMech != null && Power.PowerOn && IsAttachedToMech)
		{
			if (sustainerCharging == null)
			{
				sustainerCharging = SoundDefOf.MechChargerCharging.TrySpawnSustainer(SoundInfo.InMap(this));
			}
			sustainerCharging.Maintain();
			if (moteCharging == null || moteCharging.Destroyed)
			{
				moteCharging = MoteMaker.MakeAttachedOverlay(currentlyChargingMech, ThingDefOf.Mote_MechCharging, Vector3.zero);
			}
			moteCharging?.Maintain();
		}
		else if (sustainerCharging != null && (currentlyChargingMech == null || !Power.PowerOn))
		{
			sustainerCharging.End();
			sustainerCharging = null;
		}
		if (wireExtensionTicks < 70)
		{
			wireExtensionTicks++;
		}
	}

	public void GenerateWastePack()
	{
		WasteProducer.ProduceWaste(WasteProducedPerChargingCycle);
		EffecterDefOf.MechChargerWasteProduced.Spawn(base.Position, base.Map).Cleanup();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__46))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__46(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void StartCharging(Pawn mech)
	{
		if (ModLister.CheckBiotech("Mech charging"))
		{
			if (currentlyChargingMech != null)
			{
				Log.Error("Tried charging on already charging mech charger!");
				return;
			}
			if (!mech.IsColonyMech)
			{
				mech.jobs.EndCurrentJob(JobCondition.Incompletable);
				return;
			}
			currentlyChargingMech = mech;
			mech.needs.energy.currentCharger = this;
			wireExtensionTicks = 0;
			SoundDefOf.MechChargerStart.PlayOneShot(this);
		}
	}

	public void StopCharging()
	{
		if (currentlyChargingMech == null)
		{
			Log.Error("Tried stopping charging on currently not charging mech charger!");
			return;
		}
		if (currentlyChargingMech.needs.energy != null)
		{
			currentlyChargingMech.needs.energy.currentCharger = null;
		}
		currentlyChargingMech = null;
		wireExtensionTicks = 0;
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (base.BeingTransportedOnGravship)
		{
			base.DeSpawn(mode);
			return;
		}
		if (currentlyChargingMech != null && mode != DestroyMode.WillReplace)
		{
			Messages.Message("MessageMechChargerDestroyedMechGoesBerserk".Translate(currentlyChargingMech.Named("PAWN")), new LookTargets(currentlyChargingMech), MessageTypeDefOf.NegativeEvent);
			currentlyChargingMech.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkMechanoid);
		}
		WasteProducer.ProduceWaste(Mathf.CeilToInt(wasteProduced));
		base.DeSpawn(mode);
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		GenDraw.FillableBarRequest barDrawData = BarDrawData;
		barDrawData.center = DrawPos + Vector3.up * 0.1f;
		barDrawData.fillPercent = WasteProducedPercentFull;
		barDrawData.filledMat = WasteBarFilledMat;
		barDrawData.unfilledMat = WasteBarUnfilledMat;
		barDrawData.rotation = base.Rotation;
		GenDraw.DrawFillableBar(barDrawData);
		Vector3 a = drawLoc;
		float num = EaseInOutQuart((float)wireExtensionTicks / 70f);
		if (currentlyChargingMech == null)
		{
			num = 1f - num;
		}
		num = Mathf.Max(num, 0.32f);
		Vector3 b = Vector3.Lerp(drawLoc, InteractionCell.ToVector3Shifted(), num);
		b.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
		a.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
		GenDraw.DrawLineBetween(a, b, WireMaterial, 1f);
	}

	private float EaseInOutQuart(float val)
	{
		if (!((double)val < 0.5))
		{
			return 1f - Mathf.Pow(-2f * val + 2f, 4f) / 2f;
		}
		return 8f * val * val * val * val;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		stringBuilder.AppendLineIfNotEmpty();
		stringBuilder.Append("WasteLevel".Translate() + ": " + WasteProducedPercentFull.ToStringPercent());
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__53))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__53(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref currentlyChargingMech, "currentlyChargingMech");
		Scribe_Values.Look(ref wasteProduced, "wasteProduced", 0f);
		Scribe_Values.Look(ref wireExtensionTicks, "wireExtensionTicks", 0);
	}
}
