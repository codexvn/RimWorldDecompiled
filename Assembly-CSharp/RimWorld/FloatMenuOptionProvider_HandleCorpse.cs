using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_HandleCorpse : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public FloatMenuContext context;

		public Corpse corpse;

		public Building_GibbetCage cage;

		public Building_Grave grave;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(context.FirstSelectedPawn, corpse, cage), JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__1()
		{
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(context.FirstSelectedPawn, corpse, grave), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__8 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

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
		public _003CGetOptionsFor_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			StoragePriority priority;
			IHaulDestination haulDestination;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
				_003C_003E8__1.context = context;
				_003C_003E8__1.corpse = clickedThing as Corpse;
				if (_003C_003E8__1.corpse == null)
				{
					return false;
				}
				_003C_003E8__1.cage = Building_GibbetCage.FindGibbetCageFor(_003C_003E8__1.corpse, _003C_003E8__1.context.FirstSelectedPawn);
				if (_003C_003E8__1.cage != null)
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(_003C_003E8__1.corpse, _003C_003E8__1.cage), delegate
					{
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.corpse, _003C_003E8__1.cage), JobTag.Misc);
					}), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.corpse));
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0131;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0131;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0131:
				if (!_003C_003E8__1.corpse.IsInValidStorage())
				{
					return false;
				}
				priority = StoreUtility.CurrentHaulDestinationOf(_003C_003E8__1.corpse).GetStoreSettings().Priority;
				if (!StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(_003C_003E8__1.corpse, _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.context.FirstSelectedPawn.Map, priority, Faction.OfPlayer, out haulDestination, acceptSamePriority: true) || haulDestination.GetStoreSettings().Priority != priority)
				{
					break;
				}
				_003C_003E8__1.grave = haulDestination as Building_Grave;
				if (_003C_003E8__1.grave != null)
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PrioritizeGeneric".Translate("Burying".Translate(), _003C_003E8__1.corpse.Label).CapitalizeFirst(), delegate
					{
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.corpse, _003C_003E8__1.grave), JobTag.Misc);
					}), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.corpse));
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CGetOptionsFor_003Ed__8 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__8(0);
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

	protected override bool Undrafted => false;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__8))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__8(-2)
		{
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}
}
