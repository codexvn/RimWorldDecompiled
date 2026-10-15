using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompLaunchable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass48_0
	{
		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public PlanetTile tile;

		public bool isShuttle;

		internal void _003CGetTransportersFloatMenuOptionsAt_003Eb__0()
		{
			launchAction(tile, new TransportersArrivalAction_FormCaravan(isShuttle ? "MessageShuttleArrived" : "MessageTransportPodsArrived"));
		}

		internal void _003CGetTransportersFloatMenuOptionsAt_003Eb__1()
		{
			launchAction(tile, null);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass50_0
	{
		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public PlanetTile tile;

		internal void _003CGetOptionsForTile_003Eb__0()
		{
			launchAction(tile, new TransportersArrivalAction_FormCaravan("MessageShuttleArrived"));
		}

		internal void _003CGetOptionsForTile_003Eb__1()
		{
			launchAction(tile, null);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__36 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompLaunchable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__36(int _003C_003E1__state)
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
				CompLaunchable CS_0024_003C_003E8__locals11 = _003C_003E4__this;
				CompTransporter transporter;
				Command_Action command_Action;
				AcceptanceReport acceptanceReport;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals11).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: End cooldown",
							action = delegate
							{
								CS_0024_003C_003E8__locals11.lastLaunchTick = Find.TickManager.TicksGame - CS_0024_003C_003E8__locals11.Props.cooldownTicks;
							}
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
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					transporter = CS_0024_003C_003E8__locals11.Transporter;
					if (transporter != null && !transporter.Groupable)
					{
						int num2 = 0;
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
					}
					command_Action = new Command_Action
					{
						defaultLabel = "CommandLaunchGroup".Translate(),
						defaultDesc = "CommandLaunchGroupDesc".Translate(),
						icon = LaunchCommandTex,
						alsoClickIfOtherInGroupClicked = false,
						action = delegate
						{
							if (CS_0024_003C_003E8__locals11.AnyInGroupHasAnythingLeftToLoad)
							{
								TaggedString text = "ConfirmSendNotCompletelyLoadedLaunchable".Translate() + ":\n";
								text += CS_0024_003C_003E8__locals11.Transporter.leftToLoad.Select((TransferableOneWay x) => x.AnyThing.LabelCap).ToLineList(" -");
								text += "\n\n" + "ConfirmSendLaunchAnyway".Translate();
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, delegate
								{
									CS_0024_003C_003E8__locals11.StartChoosingDestination(CS_0024_003C_003E8__locals11.TryLaunch, null);
								}));
							}
							else
							{
								CS_0024_003C_003E8__locals11.StartChoosingDestination(CS_0024_003C_003E8__locals11.TryLaunch, null);
							}
						}
					};
					acceptanceReport = CS_0024_003C_003E8__locals11.CanLaunch(null);
					if (!acceptanceReport.Accepted)
					{
						command_Action.Disable(acceptanceReport.Reason);
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 2;
					return true;
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
			_003CCompGetGizmosExtra_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__36(0)
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
	private sealed class _003CGetOptionsForTile_003Ed__50 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private _003C_003Ec__DisplayClass50_0 _003C_003E8__1;

		private bool _003Canything_003E5__2;

		private List<WorldObject> _003CworldObjects_003E5__3;

		private int _003Ci_003E5__4;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap4;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetOptionsForTile_003Ed__50(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
			_003CworldObjects_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass50_0();
					_003C_003E8__1.launchAction = launchAction;
					_003C_003E8__1.tile = tile;
					_003Canything_003E5__2 = false;
					if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(pods, _003C_003E8__1.tile) && !Find.WorldObjects.AnySettlementBaseAt(_003C_003E8__1.tile) && !Find.WorldObjects.AnySiteAt(_003C_003E8__1.tile))
					{
						_003Canything_003E5__2 = true;
						_003C_003E2__current = new FloatMenuOption("FormCaravanHere".Translate(), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1.tile, new TransportersArrivalAction_FormCaravan("MessageShuttleArrived"));
						});
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00fb;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00fb;
				case 2:
					_003C_003E1__state = -3;
					goto IL_01ae;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_01ae:
					if (_003C_003E7__wrap4.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap4.Current;
						_003Canything_003E5__2 = true;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					goto IL_01c8;
					IL_01c8:
					_003Ci_003E5__4++;
					goto IL_01d8;
					IL_00fb:
					_003CworldObjects_003E5__3 = Find.WorldObjects.AllWorldObjects;
					_003Ci_003E5__4 = 0;
					goto IL_01d8;
					IL_01d8:
					if (_003Ci_003E5__4 < _003CworldObjects_003E5__3.Count)
					{
						if (!(_003CworldObjects_003E5__3[_003Ci_003E5__4].Tile != _003C_003E8__1.tile))
						{
							_003C_003E7__wrap4 = _003CworldObjects_003E5__3[_003Ci_003E5__4].GetShuttleFloatMenuOptions(pods, _003C_003E8__1.launchAction).GetEnumerator();
							_003C_003E1__state = -3;
							goto IL_01ae;
						}
						goto IL_01c8;
					}
					if (!_003Canything_003E5__2 && !Find.World.Impassable(_003C_003E8__1.tile))
					{
						_003C_003E2__current = new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1.tile, null);
						});
						_003C_003E1__state = 3;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsForTile_003Ed__50 _003CGetOptionsForTile_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsForTile_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsForTile_003Ed__ = new _003CGetOptionsForTile_003Ed__50(0);
			}
			_003CGetOptionsForTile_003Ed__.tile = _003C_003E3__tile;
			_003CGetOptionsForTile_003Ed__.pods = _003C_003E3__pods;
			_003CGetOptionsForTile_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetOptionsForTile_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTransportersFloatMenuOptionsAt_003Ed__48 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		private bool isShuttle;

		public bool _003C_003E3__isShuttle;

		public CompLaunchable _003C_003E4__this;

		private _003C_003Ec__DisplayClass48_0 _003C_003E8__1;

		private bool _003Canything_003E5__2;

		private List<WorldObject> _003CworldObjects_003E5__3;

		private int _003Ci_003E5__4;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap4;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetTransportersFloatMenuOptionsAt_003Ed__48(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003CworldObjects_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompLaunchable compLaunchable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass48_0();
					_003C_003E8__1.launchAction = launchAction;
					_003C_003E8__1.tile = tile;
					_003C_003E8__1.isShuttle = isShuttle;
					_003Canything_003E5__2 = false;
					if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(compLaunchable.TransportersInGroup, _003C_003E8__1.tile) && !Find.WorldObjects.AnySettlementBaseAt(_003C_003E8__1.tile) && !Find.WorldObjects.AnySiteAt(_003C_003E8__1.tile))
					{
						_003Canything_003E5__2 = true;
						_003C_003E2__current = new FloatMenuOption("FormCaravanHere".Translate(), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1.tile, new TransportersArrivalAction_FormCaravan(_003C_003E8__1.isShuttle ? "MessageShuttleArrived" : "MessageTransportPodsArrived"));
						});
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0117;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0117;
				case 2:
					_003C_003E1__state = -3;
					goto IL_01da;
				case 3:
					_003C_003E1__state = -4;
					goto IL_0267;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_01da:
					if (_003C_003E7__wrap4.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap4.Current;
						_003Canything_003E5__2 = true;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					goto IL_0281;
					IL_0281:
					_003Ci_003E5__4++;
					goto IL_0293;
					IL_0267:
					if (_003C_003E7__wrap4.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap4.Current;
						_003Canything_003E5__2 = true;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					goto IL_0281;
					IL_0117:
					_003CworldObjects_003E5__3 = Find.WorldObjects.AllWorldObjects;
					_003Ci_003E5__4 = 0;
					goto IL_0293;
					IL_0293:
					if (_003Ci_003E5__4 < _003CworldObjects_003E5__3.Count)
					{
						if (!(_003CworldObjects_003E5__3[_003Ci_003E5__4].Tile != _003C_003E8__1.tile))
						{
							if (_003C_003E8__1.isShuttle)
							{
								_003C_003E7__wrap4 = _003CworldObjects_003E5__3[_003Ci_003E5__4].GetShuttleFloatMenuOptions(compLaunchable.TransportersInGroup, _003C_003E8__1.launchAction).GetEnumerator();
								_003C_003E1__state = -3;
								goto IL_01da;
							}
							_003C_003E7__wrap4 = _003CworldObjects_003E5__3[_003Ci_003E5__4].GetTransportersFloatMenuOptions(compLaunchable.TransportersInGroup, _003C_003E8__1.launchAction).GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_0267;
						}
						goto IL_0281;
					}
					if (!_003Canything_003E5__2 && !Find.World.Impassable(_003C_003E8__1.tile))
					{
						_003C_003E2__current = new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate
						{
							_003C_003E8__1.launchAction(_003C_003E8__1.tile, null);
						});
						_003C_003E1__state = 4;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetTransportersFloatMenuOptionsAt_003Ed__48 _003CGetTransportersFloatMenuOptionsAt_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptionsAt_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptionsAt_003Ed__ = new _003CGetTransportersFloatMenuOptionsAt_003Ed__48(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTransportersFloatMenuOptionsAt_003Ed__.tile = _003C_003E3__tile;
			_003CGetTransportersFloatMenuOptionsAt_003Ed__.launchAction = _003C_003E3__launchAction;
			_003CGetTransportersFloatMenuOptionsAt_003Ed__.isShuttle = _003C_003E3__isShuttle;
			return _003CGetTransportersFloatMenuOptionsAt_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public int lastLaunchTick = -1;

	private CompTransporter cachedCompTransporter;

	private CompRefuelable cachedCompRefuelable;

	private CompShuttle cachedCompShuttle;

	public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment");

	public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip");

	private PlanetTile cachedClosest;

	private PlanetTile cachedOrigin;

	private PlanetLayer cachedLayer;

	public CompProperties_Launchable Props => (CompProperties_Launchable)props;

	protected CompTransporter Transporter => cachedCompTransporter ?? (cachedCompTransporter = parent.GetComp<CompTransporter>());

	public virtual CompRefuelable Refuelable => cachedCompRefuelable ?? (cachedCompRefuelable = parent.GetComp<CompRefuelable>());

	private CompShuttle Shuttle => cachedCompShuttle ?? (cachedCompShuttle = parent.GetComp<CompShuttle>());

	protected List<CompTransporter> TransportersInGroup => Transporter.TransportersInGroup(parent.Map);

	private bool AnyInGroupHasAnythingLeftToLoad => Transporter.AnyInGroupHasAnythingLeftToLoad;

	private Thing FirstThingLeftToLoadInGroup => Transporter.FirstThingLeftToLoadInGroup;

	public virtual float FuelLevel => Refuelable.Fuel;

	public virtual float MaxFuelLevel => Refuelable.Props.fuelCapacity;

	public virtual bool RequiresFuelingPort => true;

	private bool AnyInGroupIsUnderRoof
	{
		get
		{
			List<CompTransporter> transportersInGroup = TransportersInGroup;
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				if (transportersInGroup[i].parent.Position.Roofed(parent.Map))
				{
					return true;
				}
			}
			return false;
		}
	}

	private bool AllLaunchablesInGroupHaveFuelForLaunch
	{
		get
		{
			foreach (CompTransporter item in TransportersInGroup)
			{
				if (item.Launchable.FuelLevel < item.Launchable.Props.minFuelCost)
				{
					return false;
				}
				if (item.Launchable.RequiresFuelingPort && !item.Launchable.Refuelable.HasFuel)
				{
					return false;
				}
			}
			return true;
		}
	}

	private float MinFuelLevelInGroup
	{
		get
		{
			List<CompTransporter> transportersInGroup = TransportersInGroup;
			float num = 0f;
			bool flag = false;
			foreach (CompTransporter item in transportersInGroup)
			{
				float fuelLevel = item.Launchable.FuelLevel;
				if (!flag || fuelLevel < num)
				{
					num = fuelLevel;
					flag = true;
				}
			}
			if (flag)
			{
				return num;
			}
			return 0f;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref lastLaunchTick, "lastLaunchTick", 0);
	}

	public override void CompTick()
	{
		if (!Props.cooldownEndedMessage.NullOrEmpty() && lastLaunchTick > 0 && lastLaunchTick + Props.cooldownTicks == Find.TickManager.TicksGame)
		{
			Messages.Message(Props.cooldownEndedMessage.Formatted(parent.LabelCap), parent, MessageTypeDefOf.NeutralEvent, historical: false);
		}
	}

	public virtual AcceptanceReport CanLaunch(float? overrideFuelLevel = null)
	{
		if (!Transporter.LoadingInProgressOrReadyToLaunch)
		{
			return "CommandLaunchGroupFailNotLoaded".Translate();
		}
		if (parent.Spawned && AnyInGroupIsUnderRoof)
		{
			return "CommandLaunchGroupFailUnderRoof".Translate();
		}
		if (overrideFuelLevel.HasValue)
		{
			if (overrideFuelLevel < Props.minFuelCost)
			{
				return "CommandLaunchGroupFailNoFuel".Translate();
			}
		}
		else if (!AllLaunchablesInGroupHaveFuelForLaunch)
		{
			return "CommandLaunchGroupFailNoFuel".Translate();
		}
		if (Transporter.OverMassCapacity)
		{
			return "CommandLaunchGroupFailOverMassCapacity".Translate() + ": " + "MassUsageString".Translate(Transporter.MassUsage.ToString("F0"), Transporter.MassCapacity.ToString("F0"));
		}
		int num = Props.cooldownTicks - Find.TickManager.TicksGame + lastLaunchTick;
		if (Props.cooldownTicks > 0 && lastLaunchTick > 0 && num > 0)
		{
			return "CommandLaunchGroupCooldown".Translate() + " (" + num.ToStringTicksToPeriod() + ")";
		}
		if (Shuttle != null)
		{
			return Shuttle.CanLaunch;
		}
		return true;
	}

	public int MaxLaunchDistanceEver(PlanetLayer layer)
	{
		if (!Transporter.LoadingInProgressOrReadyToLaunch)
		{
			return 0;
		}
		int num = MaxLaunchDistanceAtFuelLevel(MaxFuelLevel, layer);
		if (Props.fixedLaunchDistanceMax >= 0)
		{
			num = Mathf.Min(num, Mathf.RoundToInt((float)Props.fixedLaunchDistanceMax / layer.Def.rangeDistanceFactor));
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__36))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (Transporter.LoadingInProgressOrReadyToLaunch)
		{
			if (Refuelable != null && !Refuelable.HasFuel)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			if (AnyInGroupHasAnythingLeftToLoad)
			{
				return "NotReadyForLaunch".Translate() + ": " + "LaunchableInGroupHasSomethingLeftToLoad".Translate().CapitalizeFirst() + ".";
			}
			if (!AllLaunchablesInGroupHaveFuelForLaunch)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			int num = Props.cooldownTicks - Find.TickManager.TicksGame + lastLaunchTick;
			if (Props.cooldownTicks > 0 && lastLaunchTick > 0 && num > 0)
			{
				return "NotReadyForLaunch".Translate() + ": " + "CommandLaunchGroupCooldown".Translate() + " (" + num.ToStringTicksToPeriod() + ")";
			}
			return "ReadyForLaunch".Translate();
		}
		return null;
	}

	public void StartChoosingDestination(Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
	{
		PlanetTile tile = parent.Tile;
		CameraJumper.TryJump(CameraJumper.GetWorldTarget(new GlobalTargetInfo(tile)));
		Find.WorldSelector.ClearSelection();
		Find.WorldTargeter.BeginTargeting((GlobalTargetInfo t) => ChoseWorldTarget(t, launchAction, overrideFuelLevel), canTargetTiles: true, TargeterMouseAttachment, !CaravanShuttleUtility.IsCaravanShuttle(Transporter), delegate
		{
			PlanetTile planetTile;
			if (cachedLayer != Find.WorldSelector.SelectedLayer || cachedOrigin != tile)
			{
				cachedLayer = Find.WorldSelector.SelectedLayer;
				cachedOrigin = tile;
				planetTile = (cachedClosest = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(tile));
			}
			else
			{
				planetTile = cachedClosest;
			}
			int num = MaxLaunchDistanceEver(planetTile.Layer);
			GenDraw.DrawWorldRadiusRing(planetTile, num, CompPilotConsole.GetThrusterRadiusMat(planetTile));
			if (Refuelable != null)
			{
				int num2 = MaxLaunchDistanceAtFuelLevel(FuelLevel, PlanetLayer.Selected);
				if (num2 < num)
				{
					GenDraw.DrawWorldRadiusRing(planetTile, num2, CompPilotConsole.GetFuelRadiusMat(planetTile));
				}
			}
		}, (GlobalTargetInfo target) => TargetingLabelGetter(target, tile, MaxLaunchDistanceEver(cachedClosest.Layer), TransportersInGroup, launchAction, this, overrideFuelLevel), null, tile, showCancelButton: true);
	}

	private bool ChoseWorldTarget(GlobalTargetInfo target, Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
	{
		if (!Transporter.LoadingInProgressOrReadyToLaunch)
		{
			return true;
		}
		cachedClosest = (cachedOrigin = PlanetTile.Invalid);
		cachedLayer = null;
		PlanetTile tile = parent.Tile;
		return ChoseWorldTarget(target, tile, TransportersInGroup, MaxLaunchDistanceEver(target.Tile.Layer), launchAction, this, overrideFuelLevel);
	}

	public void TryLaunch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
	{
		if (!parent.Spawned)
		{
			Log.Error($"Tried to launch {parent}, but it's unspawned.");
			return;
		}
		List<CompTransporter> transportersInGroup = TransportersInGroup;
		if (transportersInGroup == null)
		{
			Log.Error($"Tried to launch {parent}, but it's not in any group.");
		}
		else
		{
			if (!CanLaunch(null))
			{
				return;
			}
			Map map = parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, passImpassable: true, int.MaxValue, canTraverseLayers: true);
			Current.Game.CurrentMap = map;
			if (num > MaxLaunchDistanceAtFuelLevel(MinFuelLevelInGroup, destinationTile.Layer))
			{
				return;
			}
			Transporter.TryRemoveLord(map);
			int groupID = Transporter.groupID;
			float amount = Mathf.Max(FuelNeededToLaunchAtDist(num, destinationTile.Layer), 1f);
			lastLaunchTick = Find.TickManager.TicksGame;
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				CompTransporter compTransporter = transportersInGroup[i];
				CompRefuelable refuelable = compTransporter.Launchable.Refuelable;
				refuelable?.ConsumeFuel(amount);
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(Props.activeTransporterDef ?? ThingDefOf.ActiveDropPod);
				activeTransporter.Contents = new ActiveTransporterInfo();
				activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, canMergeWithExistingStacks: true, destroyLeftover: true);
				activeTransporter.Contents.sentTransporterDef = parent.def;
				activeTransporter.Rotation = parent.Rotation;
				FlyShipLeaving obj = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeTransporter);
				obj.groupID = groupID;
				obj.destinationTile = destinationTile;
				obj.arrivalAction = arrivalAction;
				obj.worldObjectDef = Props.worldObjectDef ?? WorldObjectDefOf.TravellingTransporters;
				if (compTransporter.parent.HasComp<CompShuttle>())
				{
					activeTransporter.Contents.SetShuttle(compTransporter.parent);
				}
				else
				{
					compTransporter.CleanUpLoadingVars(map);
					compTransporter.parent.Destroy();
				}
				GenSpawn.Spawn(obj, compTransporter.parent.Position, map);
				if (refuelable?.parent is INotifyLaunchableLaunch notifyLaunchableLaunch)
				{
					notifyLaunchableLaunch.Notify_LaunchableLaunched(compTransporter.Launchable);
				}
			}
			CameraJumper.TryHideWorld();
		}
	}

	public void Notify_FuelingPortSourceDeSpawned()
	{
		if (Transporter.CancelLoad())
		{
			Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), parent, MessageTypeDefOf.NegativeEvent);
		}
	}

	public int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
	{
		return MaxLaunchDistanceAtFuelLevel(fuelLevel, parent.Tile.Layer);
	}

	public int MaxLaunchDistanceAtFuelLevel(float fuelLevel, PlanetLayer layer)
	{
		if (fuelLevel < Props.minFuelCost)
		{
			return 0;
		}
		int num = Mathf.FloorToInt(fuelLevel / (Props.fuelPerTile * layer.Def.rangeDistanceFactor));
		if (num <= 0)
		{
			num = int.MaxValue;
		}
		if (Props.fixedLaunchDistanceMax >= 0)
		{
			num = Mathf.Min(num, Mathf.RoundToInt((float)Props.fixedLaunchDistanceMax / layer.Def.rangeDistanceFactor));
		}
		return num;
	}

	public float FuelNeededToLaunchAtDist(float dist, PlanetLayer layer)
	{
		float num = dist * (Props.fuelPerTile * layer.Def.rangeDistanceFactor);
		if (Props.minFuelCost > 0f && num < Props.minFuelCost)
		{
			return Props.minFuelCost;
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptionsAt_003Ed__48))]
	private IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptionsAt(PlanetTile tile, Action<PlanetTile, TransportersArrivalAction> launchAction, bool isShuttle)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptionsAt_003Ed__48(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tile = tile,
			_003C_003E3__launchAction = launchAction,
			_003C_003E3__isShuttle = isShuttle
		};
	}

	public static bool ChoseWorldTarget(GlobalTargetInfo target, PlanetTile tile, IEnumerable<IThingHolder> pods, int maxLaunchDistance, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
	{
		if (!target.IsValid)
		{
			Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return false;
		}
		if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
		{
			Messages.Message("MessageWorldObjectIsInvalid".Translate(target.WorldObject.Named("OBJECT")), MessageTypeDefOf.RejectInput, historical: false);
			return false;
		}
		if (ModsConfig.OdysseyActive && target.HasWorldObject && target.WorldObject.RequiresSignalJammerToReach)
		{
			Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return false;
		}
		float fuelLevel = overrideFuelLevel ?? launchable?.FuelLevel ?? (-1f);
		int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, passImpassable: true, int.MaxValue, canTraverseLayers: true);
		float num2 = ((float?)launchable?.MaxLaunchDistanceAtFuelLevel(fuelLevel, target.Tile.Layer)) ?? (-1f);
		float fuelRequired = launchable?.FuelNeededToLaunchAtDist(num, target.Tile.Layer) ?? (-1f);
		if (maxLaunchDistance >= 0 && num > maxLaunchDistance)
		{
			Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return false;
		}
		if (num2 >= 0f && (float)num > num2)
		{
			Messages.Message("TransportPodNotEnoughFuel".Translate(), MessageTypeDefOf.RejectInput, historical: false);
			return false;
		}
		bool flag = launchable?.Shuttle != null;
		List<FloatMenuOption> list = ((launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : GetOptionsForTile(target.Tile, pods, launchAction)).ToList();
		if (!list.Any())
		{
			if (Find.World.Impassable(target.Tile))
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (flag && fuelLevel < fuelRequired * 2f)
			{
				WorldObject worldObject = target.WorldObject;
				if (worldObject == null || worldObject.Faction?.IsPlayer != true)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), delegate
					{
						launchAction(target.Tile, null);
					}));
					return true;
				}
			}
			launchAction(target.Tile, null);
			return true;
		}
		if (flag && fuelLevel < fuelRequired * 2f)
		{
			WorldObject worldObject2 = target.WorldObject;
			if (worldObject2 == null || worldObject2.Faction?.IsPlayer != true)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Action oldAction = list[i].action;
					list[i].action = delegate
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), oldAction));
					};
				}
			}
		}
		if (list.Count == 1)
		{
			if (!list.First().Disabled)
			{
				list.First().action();
				return true;
			}
			return false;
		}
		Find.WindowStack.Add(new FloatMenu(list));
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsForTile_003Ed__50))]
	public static IEnumerable<FloatMenuOption> GetOptionsForTile(PlanetTile tile, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsForTile_003Ed__50(-2)
		{
			_003C_003E3__tile = tile,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	public static TaggedString TargetingLabelGetter(GlobalTargetInfo target, PlanetTile tile, int maxLaunchDistance, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
	{
		if (!target.IsValid)
		{
			return null;
		}
		if (target.Tile.Layer != tile.Layer && !tile.Layer.HasConnectionPathTo(target.Tile.Layer))
		{
			GUI.color = ColorLibrary.RedReadable;
			return "TransportPodDestinationNoPath".Translate(target.Tile.Layer.Def.Named("LAYER"));
		}
		if (ModsConfig.OdysseyActive)
		{
			WorldObject worldObject = Find.World.worldObjects.WorldObjectAt<WorldObject>(target.Tile);
			if (worldObject != null && worldObject.RequiresSignalJammerToReach)
			{
				GUI.color = ColorLibrary.RedReadable;
				return "TransportPodDestinationRequiresSignalJammer".Translate();
			}
		}
		int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, passImpassable: true, maxLaunchDistance, canTraverseLayers: true);
		if (maxLaunchDistance > 0 && num > maxLaunchDistance)
		{
			GUI.color = ColorLibrary.RedReadable;
			return "TransportPodDestinationBeyondMaximumRange".Translate();
		}
		bool flag = launchable?.Shuttle != null;
		string text = "";
		if (flag)
		{
			float num2 = overrideFuelLevel ?? launchable.FuelLevel;
			float num3 = launchable.FuelNeededToLaunchAtDist(num, target.Tile.Layer);
			text = $"{"Cost".Translate().CapitalizeFirst()}: {"FuelAmount".Translate(num3, ThingDefOf.Chemfuel)}";
			if (num3 > num2)
			{
				text = (text + $" ({"TransportPodNotEnoughFuel".Translate()})").Colorize(ColorLibrary.RedReadable);
			}
		}
		if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
		{
			return string.Empty;
		}
		IEnumerable<FloatMenuOption> source = ((launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : GetOptionsForTile(target.Tile, pods, launchAction));
		if (!source.Any())
		{
			return string.Empty;
		}
		if (source.Count() == 1)
		{
			if (source.First().Disabled)
			{
				GUI.color = ColorLibrary.RedReadable;
			}
			return source.First().Label + "\n" + text;
		}
		if (target.WorldObject is MapParent mapParent)
		{
			return "ClickToSeeAvailableOrders_WorldObject".Translate(mapParent.LabelCap) + "\n" + text;
		}
		return "ClickToSeeAvailableOrders_Empty".Translate() + "\n" + text;
	}

	public void Notify_Arrived()
	{
		lastLaunchTick = Find.TickManager.TicksGame;
	}
}
