using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_TransferEntity : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public FloatMenuContext context;

		public Pawn heldPawn;

		public Thing clickedThing;

		internal bool _003CGetOptionsFor_003Eb__0(Thing b)
		{
			if (!(b is Building_HoldingPlatform building_HoldingPlatform))
			{
				return false;
			}
			if (building_HoldingPlatform.Occupied)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReserve(building_HoldingPlatform))
			{
				return false;
			}
			return true;
		}

		internal float _003CGetOptionsFor_003Eb__1(Thing t)
		{
			CompEntityHolder compEntityHolder = t.TryGetComp<CompEntityHolder>();
			if (compEntityHolder != null && compEntityHolder.ContainmentStrength >= heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength))
			{
				return compEntityHolder.ContainmentStrength / Mathf.Max(heldPawn.PositionHeld.DistanceTo(t.Position), 1f);
			}
			return 0f;
		}

		internal void _003CGetOptionsFor_003Eb__2()
		{
			StudyUtility.TargetHoldingPlatformForEntity(context.FirstSelectedPawn, heldPawn, transferBetweenPlatforms: true, clickedThing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

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
		public _003CGetOptionsFor_003Ed__9(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass9_0
				{
					context = context,
					clickedThing = clickedThing
				};
				if (!CS_0024_003C_003E8__locals17.clickedThing.TryGetComp(out CompEntityHolder comp) || comp.HeldPawn == null)
				{
					return false;
				}
				if (!CS_0024_003C_003E8__locals17.context.FirstSelectedPawn.CanReserveAndReach(CS_0024_003C_003E8__locals17.clickedThing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
				{
					return false;
				}
				CS_0024_003C_003E8__locals17.heldPawn = comp.HeldPawn;
				if (GenClosest.ClosestThing_Global_Reachable(CS_0024_003C_003E8__locals17.context.FirstSelectedPawn.Position, CS_0024_003C_003E8__locals17.context.FirstSelectedPawn.Map, CS_0024_003C_003E8__locals17.context.FirstSelectedPawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_HoldingPlatform>(), PathEndMode.ClosestTouch, TraverseParms.For(CS_0024_003C_003E8__locals17.context.FirstSelectedPawn, Danger.Some), 9999f, delegate(Thing b)
				{
					if (!(b is Building_HoldingPlatform building_HoldingPlatform))
					{
						return false;
					}
					if (building_HoldingPlatform.Occupied)
					{
						return false;
					}
					return CS_0024_003C_003E8__locals17.context.FirstSelectedPawn.CanReserve(building_HoldingPlatform) ? true : false;
				}, delegate(Thing t)
				{
					CompEntityHolder compEntityHolder = t.TryGetComp<CompEntityHolder>();
					return (compEntityHolder != null && compEntityHolder.ContainmentStrength >= CS_0024_003C_003E8__locals17.heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength)) ? (compEntityHolder.ContainmentStrength / Mathf.Max(CS_0024_003C_003E8__locals17.heldPawn.PositionHeld.DistanceTo(t.Position), 1f)) : 0f;
				}) != null)
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TransferEntity".Translate(CS_0024_003C_003E8__locals17.heldPawn) + " (" + "ChooseEntityHolder".Translate() + "...)", delegate
					{
						StudyUtility.TargetHoldingPlatformForEntity(CS_0024_003C_003E8__locals17.context.FirstSelectedPawn, CS_0024_003C_003E8__locals17.heldPawn, transferBetweenPlatforms: true, CS_0024_003C_003E8__locals17.clickedThing);
					}), CS_0024_003C_003E8__locals17.context.FirstSelectedPawn, CS_0024_003C_003E8__locals17.clickedThing);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__9 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__9(0);
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return ModsConfig.AnomalyActive;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__9))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__9(-2)
		{
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}
}
