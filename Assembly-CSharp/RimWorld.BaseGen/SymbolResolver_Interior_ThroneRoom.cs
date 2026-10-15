using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen;

public class SymbolResolver_Interior_ThroneRoom : SymbolResolver
{
	[CompilerGenerated]
	private sealed class _003CGetPossibleDrapeCells_003Ed__3 : IEnumerable<Pair<IntVec3, Rot4>>, IEnumerable, IEnumerator<Pair<IntVec3, Rot4>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pair<IntVec3, Rot4> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private CellRect rect;

		public CellRect _003C_003E3__rect;

		private IntVec3 throneCell;

		public IntVec3 _003C_003E3__throneCell;

		private int _003Cd_003E5__2;

		private IEnumerator<IntVec3> _003C_003E7__wrap2;

		Pair<IntVec3, Rot4> IEnumerator<Pair<IntVec3, Rot4>>.Current
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
		public _003CGetPossibleDrapeCells_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0113;
				}
				_003C_003E1__state = -1;
				_003Cd_003E5__2 = 0;
				goto IL_0142;
				IL_0113:
				while (_003C_003E7__wrap2.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap2.Current;
					bool flag = true;
					foreach (IntVec3 item in GenAdj.OccupiedRect(current, new Rot4(_003Cd_003E5__2), ThingDefOf.Drape.size))
					{
						if (item == throneCell || rect.IsCorner(item) || BaseGenUtility.AnyDoorAdjacentCardinalTo(item, BaseGen.globalSettings.map))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						_003C_003E2__current = new Pair<IntVec3, Rot4>(current, new Rot4(_003Cd_003E5__2));
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				_003Cd_003E5__2++;
				goto IL_0142;
				IL_0142:
				if (_003Cd_003E5__2 < 4)
				{
					_003C_003E7__wrap2 = rect.GetEdgeCells(new Rot4(_003Cd_003E5__2)).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0113;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pair<IntVec3, Rot4>> IEnumerable<Pair<IntVec3, Rot4>>.GetEnumerator()
		{
			_003CGetPossibleDrapeCells_003Ed__3 _003CGetPossibleDrapeCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPossibleDrapeCells_003Ed__ = this;
			}
			else
			{
				_003CGetPossibleDrapeCells_003Ed__ = new _003CGetPossibleDrapeCells_003Ed__3(0);
			}
			_003CGetPossibleDrapeCells_003Ed__.throneCell = _003C_003E3__throneCell;
			_003CGetPossibleDrapeCells_003Ed__.rect = _003C_003E3__rect;
			return _003CGetPossibleDrapeCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pair<IntVec3, Rot4>>)this).GetEnumerator();
		}
	}

	private static List<Pair<IntVec3, Rot4>> tmpCells = new List<Pair<IntVec3, Rot4>>();

	public override void Resolve(ResolveParams rp)
	{
		Rot4 dir;
		IntVec3 throneCell = GetThroneCell(rp.rect, out dir);
		if (GetPossibleDrapeCells(throneCell, rp.rect).TryRandomElement(out var result))
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Drape;
			resolveParams.rect = CellRect.SingleCell(result.First);
			resolveParams.thingRot = result.Second;
			BaseGen.symbolStack.Push("thing", resolveParams);
		}
		foreach (IntVec3 corner in rp.rect.Corners)
		{
			if (!(corner == throneCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(corner, BaseGen.globalSettings.map))
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = ThingDefOf.Brazier;
				resolveParams2.rect = CellRect.SingleCell(corner);
				resolveParams2.postThingSpawn = delegate(Thing x)
				{
					x.TryGetComp<CompRefuelable>().Refuel(9999f);
				};
				BaseGen.symbolStack.Push("thing", resolveParams2);
			}
		}
		ResolveParams resolveParams3 = rp;
		resolveParams3.singleThingDef = ThingDefOf.Throne;
		resolveParams3.thingRot = dir;
		resolveParams3.rect = CellRect.SingleCell(throneCell);
		BaseGen.symbolStack.Push("thing", resolveParams3);
	}

	private IntVec3 GetThroneCell(CellRect rect, out Rot4 dir)
	{
		tmpCells.Clear();
		tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.maxZ), Rot4.South));
		tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.minZ), Rot4.North));
		tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.minX, 0, rect.CenterCell.z), Rot4.East));
		tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.maxX, 0, rect.CenterCell.z), Rot4.West));
		if (!tmpCells.Where((Pair<IntVec3, Rot4> x) => !BaseGenUtility.AnyDoorAdjacentCardinalTo(x.First, BaseGen.globalSettings.map)).TryRandomElement(out var result))
		{
			tmpCells.TryRandomElement(out result);
		}
		dir = result.Second;
		return result.First;
	}

	[IteratorStateMachine(typeof(_003CGetPossibleDrapeCells_003Ed__3))]
	private IEnumerable<Pair<IntVec3, Rot4>> GetPossibleDrapeCells(IntVec3 throneCell, CellRect rect)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPossibleDrapeCells_003Ed__3(-2)
		{
			_003C_003E3__throneCell = throneCell,
			_003C_003E3__rect = rect
		};
	}
}
