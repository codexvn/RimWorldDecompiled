using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PlaceWorker_RitualPosition : PlaceWorker
{
	[CompilerGenerated]
	private sealed class _003CRitualSeatsInRange_003Ed__5 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private Thing thing;

		public Thing _003C_003E3__thing;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private List<Thing> _003Cbuildings_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003CRitualSeatsInRange_003Ed__5(int _003C_003E1__state)
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
			_003Cbuildings_003E5__2 = null;
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
					goto IL_014a;
				}
				_003C_003E1__state = -1;
				_003Cbuildings_003E5__2 = null;
				_003C_003E1__state = -3;
				if (GatheringsUtility.UseWholeRoomAsGatheringArea(center, Find.CurrentMap))
				{
					_003Cbuildings_003E5__2 = tmpRitualSeatsInRoom;
					foreach (Thing containedAndAdjacentThing in center.GetRoom(Find.CurrentMap).ContainedAndAdjacentThings)
					{
						if (containedAndAdjacentThing.TryGetComp<CompRitualSeat>() != null)
						{
							_003Cbuildings_003E5__2.Add(containedAndAdjacentThing);
						}
					}
				}
				else
				{
					_003Cbuildings_003E5__2 = Find.CurrentMap.listerBuildingWithTagInProximity.GetForCell(center, 18f, "RitualSeat", this.thing);
				}
				_003Ci_003E5__3 = 0;
				goto IL_015c;
				IL_014a:
				_003Ci_003E5__3++;
				goto IL_015c;
				IL_015c:
				if (_003Ci_003E5__3 < _003Cbuildings_003E5__2.Count)
				{
					Thing thing = _003Cbuildings_003E5__2[_003Ci_003E5__3];
					if (GatheringsUtility.InGatheringArea(thing.Position, center, Find.CurrentMap) && SpectatorCellFinder.CorrectlyRotatedChairAt(thing.Position, Find.CurrentMap, GenAdj.OccupiedRect(center, rot, def.size)))
					{
						_003C_003E2__current = thing;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_014a;
				}
				_003C_003Em__Finally1();
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
			tmpRitualSeatsInRoom.Clear();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CRitualSeatsInRange_003Ed__5 _003CRitualSeatsInRange_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRitualSeatsInRange_003Ed__ = this;
			}
			else
			{
				_003CRitualSeatsInRange_003Ed__ = new _003CRitualSeatsInRange_003Ed__5(0);
			}
			_003CRitualSeatsInRange_003Ed__.def = _003C_003E3__def;
			_003CRitualSeatsInRange_003Ed__.center = _003C_003E3__center;
			_003CRitualSeatsInRange_003Ed__.rot = _003C_003E3__rot;
			_003CRitualSeatsInRange_003Ed__.thing = _003C_003E3__thing;
			return _003CRitualSeatsInRange_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public const float RitualFocusRange = 4.9f;

	public const SimpleColor RitualFocusConnectionColor = SimpleColor.Green;

	public const SimpleColor RitualSeatConnectionColor = SimpleColor.Yellow;

	private static List<Thing> tmpRitualSeatsInRoom = new List<Thing>();

	public static List<Thing> GetRitualFocusInRange(IntVec3 ritualPosition, Thing ritualPositionThing = null)
	{
		return Find.CurrentMap.listerBuildingWithTagInProximity.GetForCell(ritualPosition, 4.9f, "RitualFocus", ritualPositionThing);
	}

	[IteratorStateMachine(typeof(_003CRitualSeatsInRange_003Ed__5))]
	public static IEnumerable<Thing> RitualSeatsInRange(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRitualSeatsInRange_003Ed__5(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__center = center,
			_003C_003E3__rot = rot,
			_003C_003E3__thing = thing
		};
	}

	public static void DrawRitualSeatConnections(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null, List<Thing> except = null)
	{
		foreach (Thing item in RitualSeatsInRange(def, center, rot, thing))
		{
			if (except == null || !except.Contains(item))
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(center, rot, def.size, AltitudeLayer.MetaOverlays.AltitudeFor()), item.TrueCenter(), SimpleColor.Yellow);
			}
		}
	}

	public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
	{
		List<Thing> ritualFocusInRange = GetRitualFocusInRange(center, thing);
		for (int i = 0; i < ritualFocusInRange.Count; i++)
		{
			Thing thing2 = ritualFocusInRange[i];
			if (thing2.def != def)
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), thing2.TrueCenter(), SimpleColor.Green);
			}
		}
		DrawRitualSeatConnections(def, center, rot, thing);
	}
}
