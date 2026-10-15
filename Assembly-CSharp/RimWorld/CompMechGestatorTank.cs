using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public class CompMechGestatorTank : ThingComp, IThingGlower
{
	public enum TankState
	{
		Empty,
		Dormant,
		Proximity
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__18 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMechGestatorTank _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__18(int _003C_003E1__state)
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
			CompMechGestatorTank CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					break;
				}
				if (CS_0024_003C_003E8__locals3.State == TankState.Empty)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Add mech",
						action = delegate
						{
							CS_0024_003C_003E8__locals3.State = TankState.Proximity;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Remove mech",
					action = delegate
					{
						CS_0024_003C_003E8__locals3.State = TankState.Empty;
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			return false;
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
			_003CCompGetGizmosExtra_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__18(0)
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

	private TankState state;

	private float triggerRadius;

	private const int StunTicks = 180;

	private static readonly IntRange GestationFluidFilthRange = new IntRange(2, 4);

	private CompProperties_MechGestatorTank Props => (CompProperties_MechGestatorTank)props;

	public TankState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
			if (parent.Spawned)
			{
				parent.DirtyMapMesh(parent.Map);
				parent.TryGetComp<CompGlower>()?.UpdateLit(parent.Map);
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		return $"{"Contains".Translate()}: {((state == TankState.Empty) ? "Nothing" : "Unknown").Translate().CapitalizeFirst()}";
	}

	public bool ShouldBeLitNow()
	{
		return state != TankState.Empty;
	}

	public override bool DontDrawParent()
	{
		return true;
	}

	public override void PostPostMake()
	{
		triggerRadius = Props.triggerRadiusRange.RandomInRange;
	}

	public override void CompTick()
	{
		if (state == TankState.Proximity && parent.IsHashIntervalTick(250))
		{
			CheckTrigger();
		}
	}

	private void CheckTrigger()
	{
		if (!parent.Spawned)
		{
			return;
		}
		foreach (IntVec3 item in GenRadial.RadialCellsAround(parent.Position, triggerRadius, useCenter: false))
		{
			if (!item.InBounds(parent.Map) || !GenSight.LineOfSight(parent.Position, item, parent.Map))
			{
				continue;
			}
			foreach (Thing thing in item.GetThingList(parent.Map))
			{
				if (thing is Pawn { IsColonist: not false })
				{
					Trigger(parent.Map);
					return;
				}
			}
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		if (mode != DestroyMode.WillReplace && state != 0)
		{
			Trigger(previousMap);
		}
	}

	private void Trigger(Map map)
	{
		if (state == TankState.Empty)
		{
			return;
		}
		State = TankState.Empty;
		IntVec3 loc = parent.OccupiedRect().ExpandedBy(1).EdgeCells.Where(Standable).RandomElementWithFallback(IntVec3.Invalid);
		ScatterDebrisUtility.ScatterFilthAroundThing(parent, map, ThingDefOf.Filth_GestationFluid, GestationFluidFilthRange);
		if (loc.IsValid)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(Props.mechKindOptions.RandomElementByWeight((PawnKindDefWeight x) => x.weight).kindDef, Faction.OfMechanoids, null);
			GenSpawn.Spawn(pawn, loc, map);
			pawn.stances?.stunner?.StunFor(180, null, addBattleLog: false);
			if (!map.lordManager.TryGetLordByJob<LordJob_AssaultColony>(Faction.OfMechanoids, out var lord))
			{
				LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_AssaultColony(Faction.OfMechanoids, canKidnap: false, canTimeoutOrFlee: false, sappers: false, useAvoidGridSmart: false, canSteal: false), map, new List<Pawn> { pawn });
			}
			else
			{
				lord.lord.AddPawn(pawn);
			}
			Messages.Message(Props.triggeredMessage.Formatted(pawn), pawn, MessageTypeDefOf.NegativeEvent);
			Props.triggerSound.PlayOneShot(parent);
		}
		bool Standable(IntVec3 c)
		{
			return c.Standable(map);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__18))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostPrintOnto(SectionLayer layer)
	{
		((state == TankState.Empty) ? Props.emptyGraphic.Graphic : Props.dormantGraphic.Graphic).Print(layer, parent, 0f);
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref state, "state", TankState.Empty);
		Scribe_Values.Look(ref triggerRadius, "triggerRadius", 0f);
	}
}
