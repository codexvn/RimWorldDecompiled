using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Building_OrbitalTradeBeacon : Building
{
	[CompilerGenerated]
	private sealed class _003CAllPowered_003Ed__7 : IEnumerable<Building_OrbitalTradeBeacon>, IEnumerable, IEnumerator<Building_OrbitalTradeBeacon>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building_OrbitalTradeBeacon _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private IEnumerator<Building_OrbitalTradeBeacon> _003C_003E7__wrap1;

		Building_OrbitalTradeBeacon IEnumerator<Building_OrbitalTradeBeacon>.Current
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
		public _003CAllPowered_003Ed__7(int _003C_003E1__state)
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
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Building_OrbitalTradeBeacon current = _003C_003E7__wrap1.Current;
					CompPowerTrader comp = current.GetComp<CompPowerTrader>();
					if (comp == null || comp.PowerOn)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
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
		IEnumerator<Building_OrbitalTradeBeacon> IEnumerable<Building_OrbitalTradeBeacon>.GetEnumerator()
		{
			_003CAllPowered_003Ed__7 _003CAllPowered_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllPowered_003Ed__ = this;
			}
			else
			{
				_003CAllPowered_003Ed__ = new _003CAllPowered_003Ed__7(0);
			}
			_003CAllPowered_003Ed__.map = _003C_003E3__map;
			return _003CAllPowered_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building_OrbitalTradeBeacon>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_OrbitalTradeBeacon _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__3(int _003C_003E1__state)
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
				Building_OrbitalTradeBeacon building_OrbitalTradeBeacon = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_OrbitalTradeBeacon).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
					{
						_003C_003E2__current = new Command_Action
						{
							action = building_OrbitalTradeBeacon.MakeMatchingStockpile,
							hotKey = KeyBindingDefOf.Misc1,
							defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
							icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile"),
							defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
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
			_003CGetGizmos_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__3(0)
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

	private const float TradeRadius = 7.9f;

	private static List<IntVec3> tradeableCells = new List<IntVec3>();

	public IEnumerable<IntVec3> TradeableCells => TradeableCellsAround(base.Position, base.Map);

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__3))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void MakeMatchingStockpile()
	{
		Designator_ZoneAddStockpile_Resources des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
		des.DesignateMultiCell(TradeableCells.Where((IntVec3 c) => des.CanDesignateCell(c).Accepted));
	}

	public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
	{
		tradeableCells.Clear();
		if (!pos.InBounds(map))
		{
			return tradeableCells;
		}
		Region region = pos.GetRegion(map);
		if (region == null)
		{
			return tradeableCells;
		}
		RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null, delegate(Region r)
		{
			foreach (IntVec3 cell in r.Cells)
			{
				if (cell.InHorDistOf(pos, 7.9f))
				{
					tradeableCells.Add(cell);
				}
			}
			return false;
		}, 16);
		return tradeableCells;
	}

	[IteratorStateMachine(typeof(_003CAllPowered_003Ed__7))]
	public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllPowered_003Ed__7(-2)
		{
			_003C_003E3__map = map
		};
	}
}
