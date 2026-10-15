using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Building_PassengerShuttle : Building, IRenameable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass28_0
	{
		public float fuelInShuttle;

		public Building_PassengerShuttle _003C_003E4__this;

		public Action<int> _003C_003E9__2;

		internal void _003CGetGizmos_003Eb__0()
		{
			int to = Mathf.FloorToInt(Mathf.Min(fuelInShuttle, _003C_003E4__this.MaxFuelLevel - _003C_003E4__this.FuelLevel));
			Dialog_Slider window = new Dialog_Slider((int val) => "RefuelShuttleCount".Translate(val), 1, to, delegate(int count)
			{
				_003C_003E4__this.ConsumeFuelFromInventory(count);
				_003C_003E4__this.RefuelableComp.Refuel(count);
			});
			Find.WindowStack.Add(window);
		}

		internal void _003CGetGizmos_003Eb__2(int count)
		{
			_003C_003E4__this.ConsumeFuelFromInventory(count);
			_003C_003E4__this.RefuelableComp.Refuel(count);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__28 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_PassengerShuttle _003C_003E4__this;

		private _003C_003Ec__DisplayClass28_0 _003C_003E8__1;

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
		public _003CGetGizmos_003Ed__28(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_PassengerShuttle building_PassengerShuttle = _003C_003E4__this;
				int num2;
				string text;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass28_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap1 = ((Building)building_PassengerShuttle).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0090;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0090;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0090:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					num2 = 0;
					foreach (object selectedObject in Find.Selector.SelectedObjects)
					{
						if (selectedObject is ThingWithComps thing && thing.HasComp<CompTransporter>())
						{
							num2++;
						}
					}
					if (num2 > 1)
					{
						return false;
					}
					_003C_003E8__1.fuelInShuttle = building_PassengerShuttle.FuelInShuttle();
					text = null;
					if (_003C_003E8__1.fuelInShuttle <= 0f)
					{
						text = "NoFuelInShuttle".Translate();
					}
					if (Mathf.Approximately(building_PassengerShuttle.FuelLevel, building_PassengerShuttle.MaxFuelLevel))
					{
						text = "ShuttleFullyFueled".Translate();
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandRefuelShuttleFromCargo".Translate(),
						defaultDesc = "CommandRefuelShuttleFromCargoDesc".Translate(),
						icon = RefuelFromCargoIcon.Texture,
						action = delegate
						{
							int to = Mathf.FloorToInt(Mathf.Min(_003C_003E8__1.fuelInShuttle, _003C_003E8__1._003C_003E4__this.MaxFuelLevel - _003C_003E8__1._003C_003E4__this.FuelLevel));
							Dialog_Slider window = new Dialog_Slider((int val) => "RefuelShuttleCount".Translate(val), 1, to, delegate(int count)
							{
								_003C_003E8__1._003C_003E4__this.ConsumeFuelFromInventory(count);
								_003C_003E8__1._003C_003E4__this.RefuelableComp.Refuel(count);
							});
							Find.WindowStack.Add(window);
						},
						Disabled = !text.NullOrEmpty(),
						disabledReason = text
					};
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CGetGizmos_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__28(0)
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

	private string shuttleName;

	private CompRefuelable cachedRefuelableComp;

	private CompLaunchable cachedLaunchableComp;

	private CompTransporter cachedTransporterComp;

	private CompShuttle cachedShuttleComp;

	public static CachedTexture RefuelFromCargoIcon = new CachedTexture("UI/Commands/RefuelPassengerShuttle");

	private static List<Thing> tmpContainedThings = new List<Thing>();

	public CompRefuelable RefuelableComp => cachedRefuelableComp ?? (cachedRefuelableComp = GetComp<CompRefuelable>());

	public CompLaunchable LaunchableComp => cachedLaunchableComp ?? (cachedLaunchableComp = GetComp<CompLaunchable>());

	public CompTransporter TransporterComp => cachedTransporterComp ?? (cachedTransporterComp = GetComp<CompTransporter>());

	public CompShuttle ShuttleComp => cachedShuttleComp ?? (cachedShuttleComp = GetComp<CompShuttle>());

	public string RenamableLabel
	{
		get
		{
			return shuttleName ?? BaseLabel;
		}
		set
		{
			shuttleName = value;
		}
	}

	public string BaseLabel => def.LabelCap;

	public string InspectLabel => RenamableLabel;

	public override string Label => RenamableLabel;

	public float FuelLevel => RefuelableComp.Fuel;

	public float MaxFuelLevel => RefuelableComp.Props.fuelCapacity;

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			ShuttleComp.shipParent.Start();
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__28))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref shuttleName, "shuttleName");
	}

	private float FuelInShuttle()
	{
		float num = 0f;
		foreach (Thing item in (IEnumerable<Thing>)TransporterComp.innerContainer)
		{
			if (RefuelableComp.Props.fuelFilter.Allows(item))
			{
				num += (float)item.stackCount;
			}
		}
		return num;
	}

	private void ConsumeFuelFromInventory(int fuelAmount)
	{
		tmpContainedThings.Clear();
		tmpContainedThings.AddRange(TransporterComp.innerContainer);
		int num = fuelAmount;
		int num2 = tmpContainedThings.Count - 1;
		while (num2 >= 0)
		{
			Thing thing = tmpContainedThings[num2];
			if (RefuelableComp.Props.fuelFilter.Allows(thing))
			{
				Thing thing2 = thing.SplitOff(Mathf.Min(num, thing.stackCount));
				num -= thing2.stackCount;
			}
			if (num > 0)
			{
				num2--;
				continue;
			}
			break;
		}
	}
}
