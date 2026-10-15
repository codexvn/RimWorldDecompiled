using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_SpawnThing : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__14 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_SpawnThing _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__14(int _003C_003E1__state)
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
				QuestPart_SpawnThing questPart_SpawnThing = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_SpawnThing).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_SpawnThing.questLookTarget)
					{
						_003C_003E2__current = questPart_SpawnThing.innerSkyfallerThing ?? questPart_SpawnThing.thing;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public Thing thing;

	public Faction factionForFindingSpot;

	public MapParent mapParent;

	public IntVec3 cell = IntVec3.Invalid;

	public bool questLookTarget = true;

	public bool lookForSafeSpot;

	public bool tryLandInShipLandingZone;

	public Thing tryLandNearThing;

	public Pawn mapParentOfPawn;

	public EffecterDef spawnEffecter;

	private Thing innerSkyfallerThing;

	private bool spawned;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__14(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool IncreasesPopulation
	{
		get
		{
			if (!(thing is Pawn val))
			{
				return false;
			}
			return PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle(val), joinPlayer: false, makePrisoners: false);
		}
	}

	public MapParent MapParent
	{
		get
		{
			if (mapParentOfPawn != null)
			{
				return mapParentOfPawn.MapHeld?.Parent;
			}
			return mapParent;
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag != inSignal)
		{
			return;
		}
		if (MapParent == null || MapParent.Destroyed)
		{
			mapParentOfPawn = null;
			mapParent = quest.TryFindNewSuitableMapParentForRetarget();
			cell = IntVec3.Invalid;
		}
		if (!MapParent.HasMap)
		{
			return;
		}
		IntVec3 result = IntVec3.Invalid;
		if (cell.IsValid)
		{
			result = cell;
		}
		else
		{
			if (tryLandInShipLandingZone && !DropCellFinder.TryFindShipLandingArea(MapParent.Map, out result, out var firstBlockingThing))
			{
				if (firstBlockingThing != null)
				{
					Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(firstBlockingThing).CapitalizeFirst()), firstBlockingThing, MessageTypeDefOf.NeutralEvent);
				}
				result = DropCellFinder.TryFindSafeLandingSpotCloseToColony(MapParent.Map, thing.def.Size, factionForFindingSpot);
			}
			if (!result.IsValid && tryLandNearThing != null)
			{
				DropCellFinder.FindSafeLandingSpotNearAvoidingHostiles(tryLandNearThing, MapParent.Map, out result, 35, 15, 25, thing.def.size, null);
			}
			if (!result.IsValid && (!lookForSafeSpot || !DropCellFinder.FindSafeLandingSpot(out result, factionForFindingSpot, MapParent.Map, 35, 15, 25, thing.def.size, null)))
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(MapParent.Map);
				if (!DropCellFinder.TryFindDropSpotNear(intVec, MapParent.Map, out result, allowFogged: false, canRoofPunch: false, allowIndoors: false, thing.def.size))
				{
					result = intVec;
				}
			}
		}
		GenPlace.TryPlaceThing(thing, result, MapParent.Map, ThingPlaceMode.Near, null, null, null);
		spawned = true;
		if (spawnEffecter != null)
		{
			spawnEffecter.SpawnMaintained(thing, thing.Map);
		}
		if (thing is Skyfaller skyfaller && skyfaller.innerContainer.Count == 1)
		{
			innerSkyfallerThing = skyfaller.innerContainer.First();
		}
		else
		{
			innerSkyfallerThing = null;
		}
	}

	public override bool QuestPartReserves(Pawn p)
	{
		if (p != thing)
		{
			if (thing is Skyfaller)
			{
				return ((Skyfaller)thing).innerContainer.Contains(p);
			}
			return false;
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref spawned, "spawned", defaultValue: false);
		if (!spawned && (thing == null || !(thing is Pawn)))
		{
			Scribe_Deep.Look(ref thing, "thing");
		}
		else
		{
			Scribe_References.Look(ref thing, "thing");
		}
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Values.Look(ref cell, "cell");
		Scribe_Values.Look(ref lookForSafeSpot, "lookForSafeSpot", defaultValue: false);
		Scribe_References.Look(ref factionForFindingSpot, "factionForFindingSpot");
		Scribe_Values.Look(ref questLookTarget, "questLookTarget", defaultValue: true);
		Scribe_References.Look(ref innerSkyfallerThing, "innerSkyfallerThing");
		Scribe_Values.Look(ref tryLandInShipLandingZone, "tryLandInShipLandingZone", defaultValue: false);
		Scribe_References.Look(ref tryLandNearThing, "tryLandNearThing");
		Scribe_References.Look(ref mapParentOfPawn, "mapParentOfPawn");
		Scribe_Defs.Look(ref spawnEffecter, "spawnEffecter");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			mapParent = Find.RandomPlayerHomeMap.Parent;
			thing = ThingMaker.MakeThing(ThingDefOf.Silver);
		}
	}
}
