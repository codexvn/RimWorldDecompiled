using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class SpaceMapParent : MapParent, INameableWorldObject, IResourceWorldObject
{
	[CompilerGenerated]
	private sealed class _003CGetShuttleFloatMenuOptions_003Ed__15 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SpaceMapParent _003C_003E4__this;

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
		public _003CGetShuttleFloatMenuOptions_003Ed__15(int _003C_003E1__state)
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
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SpaceMapParent spaceMapParent = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)spaceMapParent).GetShuttleFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, spaceMapParent).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
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

		private void _003C_003Em__Finally2()
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
			_003CGetShuttleFloatMenuOptions_003Ed__15 _003CGetShuttleFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetShuttleFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetShuttleFloatMenuOptions_003Ed__ = new _003CGetShuttleFloatMenuOptions_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetShuttleFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetShuttleFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetShuttleFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTransportersFloatMenuOptions_003Ed__14 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SpaceMapParent _003C_003E4__this;

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
		public _003CGetTransportersFloatMenuOptions_003Ed__14(int _003C_003E1__state)
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
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SpaceMapParent spaceMapParent = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)spaceMapParent).GetTransportersFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, spaceMapParent).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
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

		private void _003C_003Em__Finally2()
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
			_003CGetTransportersFloatMenuOptions_003Ed__14 _003CGetTransportersFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptions_003Ed__ = new _003CGetTransportersFloatMenuOptions_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTransportersFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetTransportersFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetTransportersFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public ThingDef preciousResource;

	public string nameInt;

	public override bool GravShipCanLandOn => !base.HasMap;

	public override string Label
	{
		get
		{
			if (!string.IsNullOrEmpty(nameInt))
			{
				return nameInt;
			}
			return base.Label;
		}
	}

	public string Name
	{
		get
		{
			return nameInt;
		}
		set
		{
			nameInt = value;
		}
	}

	public ThingDef PreciousResource
	{
		get
		{
			return preciousResource;
		}
		set
		{
			preciousResource = value;
		}
	}

	public override MapGeneratorDef MapGeneratorDef => def.mapGenerator ?? MapGeneratorDefOf.Space;

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptions_003Ed__14))]
	public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptions_003Ed__14(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	[IteratorStateMachine(typeof(_003CGetShuttleFloatMenuOptions_003Ed__15))]
	public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetShuttleFloatMenuOptions_003Ed__15(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
	{
		alsoRemoveWorldObject = false;
		if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
		{
			return false;
		}
		foreach (PocketMapParent item in Find.World.pocketMaps.ToList())
		{
			if (item.sourceMap == base.Map && item.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
		}
		if (base.Map.AnyBuildingBlockingMapRemoval)
		{
			return false;
		}
		if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
		{
			return false;
		}
		alsoRemoveWorldObject = true;
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref nameInt, "nameInt");
		Scribe_Defs.Look(ref preciousResource, "preciousResource");
	}
}
