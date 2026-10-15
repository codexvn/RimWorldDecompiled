using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompPollutionPump : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass12_0
	{
		public int ticks;

		public CompPollutionPump _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			_003C_003E4__this.ticksUntilPump = ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPollutionPump _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__12(int _003C_003E1__state)
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
			CompPollutionPump CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Pump",
						action = CS_0024_003C_003E8__locals8.Pump,
						Disabled = !CS_0024_003C_003E8__locals8.Active
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set next pump time",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[11]
						{
							60, 120, 180, 240, 300, 600, 900, 1200, 1500, 1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							_003C_003Ec__DisplayClass12_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass12_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals8,
								ticks = array[i]
							};
							list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals5.ticks.ToStringSecondsFromTicks("F0"), delegate
							{
								CS_0024_003C_003E8__locals5._003C_003E4__this.ticksUntilPump = CS_0024_003C_003E8__locals5.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = !CS_0024_003C_003E8__locals8.Active
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			return false;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__12(0)
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

	private const int CheckInterval = 60;

	private CompPowerTrader compPower;

	private int ticksUntilPump;

	private int currentIntervalPumps;

	[LoadAlias("disabledByArtificalBuildings")]
	private bool disabledByArtificialBuildings;

	public CompProperties_PollutionPump Props => (CompProperties_PollutionPump)props;

	private bool Active
	{
		get
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (disabledByArtificialBuildings)
			{
				return false;
			}
			if (compPower != null && !compPower.PowerOn)
			{
				return false;
			}
			if (!GetCellToUnpollute().IsValid)
			{
				return false;
			}
			return true;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Pollution pump"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			ticksUntilPump = Props.intervalTicks;
		}
		compPower = parent.GetComp<CompPowerTrader>();
	}

	private IntVec3 GetCellToUnpollute()
	{
		int num = GenRadial.NumCellsInRadius(Props.radius);
		Map map = parent.Map;
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
			if (intVec.InBounds(map) && intVec.CanUnpollute(map))
			{
				return intVec;
			}
		}
		return IntVec3.Invalid;
	}

	private void Pump()
	{
		IntVec3 cellToUnpollute = GetCellToUnpollute();
		if (!cellToUnpollute.IsValid)
		{
			return;
		}
		Map map = parent.Map;
		map.pollutionGrid.SetPolluted(cellToUnpollute, isPolluted: false);
		currentIntervalPumps++;
		Props.pumpEffecterDef?.Spawn(parent.Position, map).Cleanup();
		if (Props.pumpsPerWastepack > 0 && currentIntervalPumps % Props.pumpsPerWastepack == 0)
		{
			GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Wastepack), parent.Position, parent.Map, ThingPlaceMode.Near, null, (IntVec3 p) => p.GetFirstBuilding(map)?.IsClearableFreeBuilding ?? true, null);
			currentIntervalPumps = 0;
		}
		MoteMaker.MakeAttachedOverlay(parent, ThingDefOf.Mote_PollutionPump, Vector3.zero);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__12))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.CompInspectStringExtra());
		if (stringBuilder.Length > 0)
		{
			stringBuilder.AppendLine();
		}
		if (Active)
		{
			stringBuilder.Append("AbsorbingPollutionNext".Translate(ticksUntilPump.ToStringTicksToPeriod()));
		}
		else if (disabledByArtificialBuildings)
		{
			stringBuilder.Append("CannotAbsorbPollution".Translate());
		}
		else
		{
			stringBuilder.Append("CanAbsorbPollution".Translate());
		}
		return stringBuilder.ToString();
	}

	public override void CompTick()
	{
		base.CompTick();
		if (Props.disabledByArtificialBuildings && parent.IsHashIntervalTick(60))
		{
			disabledByArtificialBuildings = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, Props.radius).Count > 0;
		}
		if (parent.IsHashIntervalTick(60) && Active)
		{
			ticksUntilPump -= 60;
			if (ticksUntilPump <= 0)
			{
				ticksUntilPump = Props.intervalTicks;
				Pump();
			}
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ticksUntilPump, "ticksUntilPump", 0);
		Scribe_Values.Look(ref currentIntervalPumps, "currentIntervalPumps", 0);
		Scribe_Values.Look(ref disabledByArtificialBuildings, "disabledByArtificialBuildings", defaultValue: false);
	}
}
