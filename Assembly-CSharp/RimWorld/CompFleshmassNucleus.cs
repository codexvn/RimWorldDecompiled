using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompFleshmassNucleus : ThingComp, IActivity, IRoofCollapseAlert
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public int ticks;

		public CompFleshmassNucleus _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			_003C_003E4__this.debugNextMeatTick = GenTicks.TicksGame + ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_1
	{
		public int ticks;

		public CompFleshmassNucleus _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__3()
		{
			_003C_003E4__this.debugActiveTick = GenTicks.TicksGame + ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompFleshmassNucleus _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__21(int _003C_003E1__state)
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
			CompFleshmassNucleus CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Create meat",
					action = delegate
					{
						List<FloatMenuOption> list2 = new List<FloatMenuOption>();
						for (int num2 = 10; num2 >= 0; num2--)
						{
							_003C_003Ec__DisplayClass21_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass21_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals5,
								ticks = num2 * 60
							};
							list2.Add(new FloatMenuOption(num2 + "s", delegate
							{
								CS_0024_003C_003E8__locals8._003C_003E4__this.debugNextMeatTick = GenTicks.TicksGame + CS_0024_003C_003E8__locals8.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list2));
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Go active in...",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 0; i <= 5; i++)
						{
							_003C_003Ec__DisplayClass21_1 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass21_1
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals5,
								ticks = i * 60
							};
							list.Add(new FloatMenuOption(i + "s", delegate
							{
								CS_0024_003C_003E8__locals4._003C_003E4__this.debugActiveTick = GenTicks.TicksGame + CS_0024_003C_003E8__locals4.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__21(0)
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

	private int minNextFireTick;

	private CompActivity activityComp;

	private int debugNextMeatTick = -1;

	private int debugActiveTick = -1;

	private static readonly IntRange InitialMeatDelayRange = new IntRange(5000, 12500);

	private static readonly IntRange MeatClumpRange = new IntRange(3, 6);

	private const int MinRefireTicks = 15000;

	private const int MeatDaysMTB = 1;

	private const int MinMeatPerStack = 20;

	private const float HeartPointsFactor = 0.5f;

	public CompProperties_FleshmassNucleus Props => (CompProperties_FleshmassNucleus)props;

	public CompActivity Activity => activityComp ?? (activityComp = Pawn.TryGetComp<CompActivity>());

	public Pawn Pawn => (Pawn)parent;

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref minNextFireTick, "minNextFireTick", 0);
	}

	public override void PostPostMake()
	{
		minNextFireTick = GenTicks.TicksGame + InitialMeatDelayRange.RandomInRange;
	}

	public override void CompTick()
	{
		if (debugNextMeatTick > 0 && GenTicks.TicksGame == debugNextMeatTick)
		{
			CreateMeat();
		}
		if (debugActiveTick > 0 && GenTicks.TicksGame == debugActiveTick)
		{
			Activity.EnterActiveState();
		}
		if (GenTicks.TicksGame >= minNextFireTick && Rand.MTBEventOccurs(1f, 60000f, 1f))
		{
			CreateMeat();
		}
	}

	public override void Notify_Downed()
	{
		Pawn.Kill(null);
	}

	private void CreateMeat()
	{
		minNextFireTick = GenTicks.TicksGame + 15000;
		int num = Mathf.CeilToInt(Props.activityMeatPerDayCurve.Evaluate(Activity.ActivityLevel));
		int randomInRange = MeatClumpRange.RandomInRange;
		for (int i = 0; i < randomInRange; i++)
		{
			if (CellFinder.TryRandomClosewalkCellNear(Pawn.PositionHeld, Pawn.MapHeld, 3, out var result))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Meat_Twisted);
				thing.stackCount = Rand.RangeInclusive(20, num - 20 * (randomInRange - i - 1));
				GenDrop.TryDropSpawn(thing, result, Pawn.MapHeld, ThingPlaceMode.Near, out var _);
			}
		}
		EffecterDefOf.MeatExplosion.Spawn(Pawn.PositionHeld, Pawn.MapHeld).Cleanup();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__21))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void OnActivityActivated()
	{
		IntVec3 positionHeld = Pawn.PositionHeld;
		Map mapHeld = Pawn.MapHeld;
		FleshbeastUtility.MeatSplatter(10, positionHeld, mapHeld, FleshbeastUtility.MeatExplosionSize.Large);
		Pawn.Destroy();
		float threatPoints = StorytellerUtility.DefaultThreatPointsNow(mapHeld) * 0.5f;
		Building_FleshmassHeart obj = (Building_FleshmassHeart)ThingMaker.MakeThing(ThingDefOf.FleshmassHeart);
		obj.Comp.threatPoints = threatPoints;
		obj.SetFaction(Faction.OfEntities);
		GenSpawn.Spawn(obj, positionHeld, mapHeld);
	}

	public void OnPassive()
	{
	}

	public bool ShouldGoPassive()
	{
		return false;
	}

	public bool CanBeSuppressed()
	{
		return true;
	}

	public bool CanActivate()
	{
		foreach (IntVec3 item in GenAdj.CellsOccupiedBy(Pawn.PositionHeld, Rot4.South, new IntVec2(2, 2)))
		{
			if (!item.SupportsStructureType(Pawn.MapHeld, ThingDefOf.FleshmassHeart.terrainAffordanceNeeded))
			{
				return false;
			}
		}
		return true;
	}

	public string ActivityTooltipExtra()
	{
		return null;
	}

	public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
	{
		if (RCellFinder.TryFindRandomCellNearWith(Pawn.Position, (IntVec3 c) => IsValidCell(c, Pawn.MapHeld), Pawn.MapHeld, out var result, 10))
		{
			SkipUtility.SkipTo(Pawn, result, Pawn.MapHeld);
			Activity.AdjustActivity(Props.activityOnRoofCollapsed);
		}
		return RoofCollapseResponse.RemoveThing;
	}

	private static bool IsValidCell(IntVec3 cell, Map map)
	{
		if (cell.InBounds(map))
		{
			return cell.Walkable(map);
		}
		return false;
	}
}
