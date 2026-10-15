using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompObeliskDeactivationInteractor : CompInteractable
{
	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__11 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompObeliskDeactivationInteractor _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CCompFloatMenuOptions_003Ed__11(int _003C_003E1__state)
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
				CompObeliskDeactivationInteractor compObeliskDeactivationInteractor = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!compObeliskDeactivationInteractor.ObeliskComp.StudyFinished || compObeliskDeactivationInteractor.ObeliskComp.Activated || compObeliskDeactivationInteractor.ObeliskComp.ActivityComp.Deactivated)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compObeliskDeactivationInteractor).CompFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__11 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__10 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompObeliskDeactivationInteractor _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__10(int _003C_003E1__state)
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
				CompObeliskDeactivationInteractor compObeliskDeactivationInteractor = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!compObeliskDeactivationInteractor.ObeliskComp.StudyFinished || compObeliskDeactivationInteractor.ObeliskComp.Activated || compObeliskDeactivationInteractor.ObeliskComp.ActivityComp.Deactivated)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compObeliskDeactivationInteractor).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__10(0)
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

	private CompObelisk obeliskComp;

	private CompObelisk ObeliskComp => obeliskComp ?? (obeliskComp = parent.GetComp<CompObelisk>());

	private new CompProperties_ObeliskDeactivationInteractor Props => (CompProperties_ObeliskDeactivationInteractor)props;

	public override string ExposeKey => "Deactivation";

	public override bool CanCooldown => false;

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		if (!ObeliskComp.StudyFinished || ObeliskComp.Activated || ObeliskComp.ActivityComp.Deactivated)
		{
			return false;
		}
		if (activateBy != null)
		{
			if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !ReservationUtility.ExistsUnreservedAmountOfDef(parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, Props.shardsRequired, (Thing t) => activateBy.CanReserveAndReach(t, PathEndMode.Touch, Danger.None)))
			{
				return "ObeliskDeactivateMissingShards".Translate(Props.shardsRequired);
			}
		}
		else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, Props.shardsRequired))
		{
			return "ObeliskDeactivateMissingShards".Translate(Props.shardsRequired);
		}
		return base.CanInteract(activateBy, checkOptionalItems);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__10))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__11))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		if (ValidateTarget(target, showMessages: false))
		{
			OrderDeactivation(target.Pawn);
		}
	}

	private void OrderDeactivation(Pawn pawn)
	{
		List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, ThingDefOf.Shard, Props.shardsRequired);
		if (!list.NullOrEmpty())
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent, list[0]);
			job.targetQueueB = (from i in list.Skip(1)
				select new LocalTargetInfo(i)).ToList();
			job.count = Props.shardsRequired;
			job.playerForced = true;
			job.interactableIndex = 1;
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	protected override void OnInteracted(Pawn caster)
	{
		if (!obeliskComp.Activated)
		{
			obeliskComp.ActivityComp.Deactivate();
			parent.GetComp<CompObeliskTriggerInteractor>()?.ResetCooldown();
		}
	}
}
