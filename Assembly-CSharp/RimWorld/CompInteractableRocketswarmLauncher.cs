using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompInteractableRocketswarmLauncher : CompInteractable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public CompInteractableRocketswarmLauncher _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.TargetLocation(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__3 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompInteractableRocketswarmLauncher _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

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
		public _003CCompFloatMenuOptions_003Ed__3(int _003C_003E1__state)
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
			CompInteractableRocketswarmLauncher compInteractableRocketswarmLauncher = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass3_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if ((bool)compInteractableRocketswarmLauncher.CanInteract(CS_0024_003C_003E8__locals4.selPawn))
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(compInteractableRocketswarmLauncher.Props.jobString.CapitalizeFirst(), delegate
					{
						CS_0024_003C_003E8__locals4._003C_003E4__this.TargetLocation(CS_0024_003C_003E8__locals4.selPawn);
					}), CS_0024_003C_003E8__locals4.selPawn, compInteractableRocketswarmLauncher.parent);
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
			_003CCompFloatMenuOptions_003Ed__3 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__3(0)
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

	private Building_TurretGun ParentGun => (Building_TurretGun)parent;

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		if (ValidateTarget(target, showMessages: false))
		{
			TargetLocation(target.Pawn);
		}
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__3))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	private void TargetLocation(Pawn caster)
	{
		Find.Targeter.BeginTargeting(ParentGun.AttackVerb, null, allowNonSelectedTargetingSource: true, null, delegate
		{
			if (ParentGun.ForcedTarget.IsValid)
			{
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent);
				caster.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		});
	}

	protected override bool TryInteractTick()
	{
		ParentGun.TryActivateBurst();
		if (ParentGun.CurrentTarget.IsValid)
		{
			ParentGun.Top.ForceFaceTarget(ParentGun.CurrentTarget);
			return true;
		}
		return true;
	}

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
		if (!result.Accepted)
		{
			return result;
		}
		if (activateBy != null && activateBy.WorkTagIsDisabled(WorkTags.Violent))
		{
			return "IsIncapableOfViolence".Translate(activateBy.LabelShort, activateBy);
		}
		return true;
	}
}
