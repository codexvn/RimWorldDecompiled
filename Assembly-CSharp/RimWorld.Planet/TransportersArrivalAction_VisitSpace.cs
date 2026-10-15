using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class TransportersArrivalAction_VisitSpace : TransportersArrivalAction
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public MapParent parent;

		public IEnumerable<IThingHolder> pods;

		public Func<FloatMenuAcceptanceReport> _003C_003E9__2;

		public Func<TransportersArrivalAction_VisitSpace> _003C_003E9__3;

		internal FloatMenuAcceptanceReport _003CGetFloatMenuOptions_003Eb__2()
		{
			return CanVisit(pods, parent);
		}

		internal TransportersArrivalAction_VisitSpace _003CGetFloatMenuOptions_003Eb__3()
		{
			return new TransportersArrivalAction_VisitSpace(parent);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__10 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private MapParent parent;

		public MapParent _003C_003E3__parent;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CGetFloatMenuOptions_003Ed__10(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass10_0
					{
						parent = parent,
						pods = pods
					};
					_003C_003E7__wrap1 = TransportersArrivalActionUtility.GetFloatMenuOptions(() => CanVisit(CS_0024_003C_003E8__locals6.pods, CS_0024_003C_003E8__locals6.parent), () => new TransportersArrivalAction_VisitSpace(CS_0024_003C_003E8__locals6.parent), "LaunchTo".Translate(CS_0024_003C_003E8__locals6.parent.Named("LOCATION")), launchAction, CS_0024_003C_003E8__locals6.parent.Tile, UIConfirmationCallback).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__10 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__10(0);
			}
			_003CGetFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			_003CGetFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetFloatMenuOptions_003Ed__.parent = _003C_003E3__parent;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private MapParent parent;

	public override bool GeneratesMap => true;

	public TransportersArrivalAction_VisitSpace()
	{
	}

	public TransportersArrivalAction_VisitSpace(MapParent parent)
	{
		this.parent = parent;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref parent, "parent");
	}

	public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
	{
		FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
		if (!floatMenuAcceptanceReport)
		{
			return floatMenuAcceptanceReport;
		}
		if (parent != null && parent.Tile != destinationTile)
		{
			return false;
		}
		return CanVisit(pods, parent);
	}

	public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
	{
		return !parent.HasMap;
	}

	public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
	{
		Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
		IntVec3 size = Find.World.info.initialMapSize;
		if (parent.def.overrideMapSize.HasValue)
		{
			size = parent.def.overrideMapSize.Value;
		}
		Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(parent.Tile, size, parent.def);
		IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(orGenerateMap);
		if (transporters.IsShuttle())
		{
			TransportersArrivalActionUtility.DropShuttle(transporters[0], orGenerateMap, near, null);
			Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion);
		}
		else
		{
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, orGenerateMap);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion);
		}
	}

	private static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, MapParent parent)
	{
		if (parent == null || !parent.Spawned)
		{
			return false;
		}
		if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__10))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, MapParent parent)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__10(-2)
		{
			_003C_003E3__launchAction = launchAction,
			_003C_003E3__pods = pods,
			_003C_003E3__parent = parent
		};
	}
}
