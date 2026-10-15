using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.SketchGen;

public class SketchResolver_AncientStorageRoom : SketchResolver
{
	[CompilerGenerated]
	private sealed class _003Cget_PossibleBuildings_003Ed__1 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_PossibleBuildings_003Ed__1(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientCrate;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientBarrel;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_PossibleBuildings_003Ed__1(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private static float WallChance = 0.8f;

	private const float OilSmearChance = 0.15f;

	private static List<IntVec3> tmpCells = new List<IntVec3>();

	private static IEnumerable<ThingDef> PossibleBuildings
	{
		[IteratorStateMachine(typeof(_003Cget_PossibleBuildings_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_PossibleBuildings_003Ed__1(-2);
		}
	}

	protected override bool CanResolveInt(SketchResolveParams parms)
	{
		if (parms.rect.HasValue)
		{
			return parms.sketch != null;
		}
		return false;
	}

	protected override void ResolveInt(SketchResolveParams parms)
	{
		if (!ModLister.CheckIdeology("Ancient storage room"))
		{
			return;
		}
		CellRect rect = parms.rect.Value;
		CellRect cellRect = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any());
		if (cellRect == CellRect.Empty)
		{
			return;
		}
		tmpCells.Clear();
		Rot4 dir = ((cellRect.Width > cellRect.Height) ? Rot4.North : Rot4.East);
		tmpCells.AddRange(cellRect.GetEdgeCells(dir));
		tmpCells.AddRange(cellRect.GetEdgeCells(dir.Opposite));
		ThingDef def = PossibleBuildings.RandomElement();
		foreach (IntVec3 tmpCell in tmpCells)
		{
			if (Rand.Chance(WallChance) && CanPlaceWallAdjacentAt(tmpCell, parms.sketch))
			{
				parms.sketch.AddThing(def, tmpCell, Rot4.North, null, 1, null, null);
			}
		}
		tmpCells.Clear();
		foreach (IntVec3 cell in rect.Cells)
		{
			CellRect rect2 = CellRect.CenteredOn(cell, 2, 2);
			if (!CanPlaceAt(rect2, parms.sketch))
			{
				continue;
			}
			foreach (IntVec3 item in rect2)
			{
				ThingDef thingDef = PossibleBuildings.RandomElement();
				parms.sketch.AddThing(thingDef, item, Rot4.North, null, 1, null, null);
				ScatterDebrisUtility.ScatterAround(item, thingDef.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.15f);
			}
		}
	}

	private bool CanPlaceAt(CellRect rect, Sketch sketch)
	{
		foreach (IntVec3 cell in rect.Cells)
		{
			if (sketch.ThingsAt(cell).Any())
			{
				return false;
			}
		}
		foreach (IntVec3 edgeCell in rect.ExpandedBy(1).EdgeCells)
		{
			if (sketch.ThingsAt(edgeCell).Any((SketchThing t) => t.def != ThingDefOf.Wall && t.def != ThingDefOf.Door))
			{
				return false;
			}
		}
		return true;
	}

	private bool CanPlaceWallAdjacentAt(IntVec3 position, Sketch sketch)
	{
		bool result = false;
		IntVec3[] cardinalDirectionsAndInside = GenAdj.CardinalDirectionsAndInside;
		for (int i = 0; i < cardinalDirectionsAndInside.Length; i++)
		{
			IntVec3 pos = cardinalDirectionsAndInside[i] + position;
			foreach (SketchThing item in sketch.ThingsAt(pos))
			{
				if (item.def == ThingDefOf.Wall)
				{
					result = true;
				}
				else if (item.def == ThingDefOf.Door)
				{
					return false;
				}
			}
		}
		return result;
	}
}
