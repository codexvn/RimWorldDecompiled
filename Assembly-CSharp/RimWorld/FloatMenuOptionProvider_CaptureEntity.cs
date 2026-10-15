using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_CaptureEntity : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public FloatMenuContext context;

		public Thing clickedThing;

		public CompHoldingPlatformTarget holdComp;

		public Thing building;

		internal bool _003CGetOptionsFor_003Eb__0(Building_HoldingPlatform x)
		{
			if (!x.Occupied)
			{
				return context.FirstSelectedPawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.Deadly);
			}
			return false;
		}

		internal float _003CGetOptionsFor_003Eb__1(Thing t)
		{
			CompEntityHolder compEntityHolder = t.TryGetComp<CompEntityHolder>();
			if (compEntityHolder != null && compEntityHolder.ContainmentStrength >= clickedThing.GetStatValue(StatDefOf.MinimumContainmentStrength))
			{
				return compEntityHolder.ContainmentStrength / Mathf.Max(clickedThing.PositionHeld.DistanceTo(t.Position), 1f);
			}
			return 0f;
		}

		internal void _003CGetOptionsFor_003Eb__2()
		{
			if (!ContainmentUtility.SafeContainerExistsFor(clickedThing))
			{
				Messages.Message("MessageNoRoomWithMinimumContainmentStrength".Translate(clickedThing.Label), MessageTypeDefOf.ThreatSmall);
			}
			holdComp.targetHolder = building;
			Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, building, clickedThing);
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__3()
		{
			StudyUtility.TargetHoldingPlatformForEntity(context.FirstSelectedPawn, clickedThing);
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

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		private IEnumerable<Building_HoldingPlatform> _003Cbuildings_003E5__2;

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
			_003C_003E8__1 = null;
			_003Cbuildings_003E5__2 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1.context = context;
				_003C_003E8__1.clickedThing = clickedThing;
				if (!_003C_003E8__1.clickedThing.TryGetComp(out _003C_003E8__1.holdComp) || !_003C_003E8__1.holdComp.CanBeCaptured || !_003C_003E8__1.holdComp.StudiedAtHoldingPlatform)
				{
					return false;
				}
				if (!_003C_003E8__1.context.FirstSelectedPawn.CanReserveAndReach(_003C_003E8__1.clickedThing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
				{
					_003C_003E2__current = new FloatMenuOption("CannotGenericWorkCustom".Translate("CaptureLower".Translate(_003C_003E8__1.clickedThing)) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				_003Cbuildings_003E5__2 = from x in _003C_003E8__1.context.FirstSelectedPawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_HoldingPlatform>()
					where !x.Occupied && _003C_003E8__1.context.FirstSelectedPawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.Deadly)
					select x;
				_003C_003E8__1.building = GenClosest.ClosestThing_Global_Reachable(_003C_003E8__1.context.FirstSelectedPawn.Position, _003C_003E8__1.context.FirstSelectedPawn.Map, _003Cbuildings_003E5__2, PathEndMode.ClosestTouch, TraverseParms.For(_003C_003E8__1.context.FirstSelectedPawn, Danger.Some), 9999f, null, delegate(Thing t)
				{
					CompEntityHolder compEntityHolder = t.TryGetComp<CompEntityHolder>();
					return (compEntityHolder != null && compEntityHolder.ContainmentStrength >= _003C_003E8__1.clickedThing.GetStatValue(StatDefOf.MinimumContainmentStrength)) ? (compEntityHolder.ContainmentStrength / Mathf.Max(_003C_003E8__1.clickedThing.PositionHeld.DistanceTo(t.Position), 1f)) : 0f;
				});
				if (_003C_003E8__1.building == null)
				{
					_003C_003E2__current = new FloatMenuOption("CannotGenericWorkCustom".Translate("CaptureLower".Translate(_003C_003E8__1.clickedThing)) + ": " + "NoHoldingPlatformsAvailable".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Capture".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing), delegate
				{
					if (!ContainmentUtility.SafeContainerExistsFor(_003C_003E8__1.clickedThing))
					{
						Messages.Message("MessageNoRoomWithMinimumContainmentStrength".Translate(_003C_003E8__1.clickedThing.Label), MessageTypeDefOf.ThreatSmall);
					}
					_003C_003E8__1.holdComp.targetHolder = _003C_003E8__1.building;
					Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, _003C_003E8__1.building, _003C_003E8__1.clickedThing);
					job.count = 1;
					_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
				_003C_003E1__state = 3;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
				_003C_003E1__state = -1;
				if (_003Cbuildings_003E5__2.Count() > 1)
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Capture".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing) + " (" + "ChooseEntityHolder".Translate() + "...)", delegate
					{
						StudyUtility.TargetHoldingPlatformForEntity(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
					}), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedThing);
					_003C_003E1__state = 4;
					return true;
				}
				break;
			case 4:
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
