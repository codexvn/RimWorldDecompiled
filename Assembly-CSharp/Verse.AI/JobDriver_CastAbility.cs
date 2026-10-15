using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse.AI;

public class JobDriver_CastAbility : JobDriver_CastVerbOnce
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__0 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CastAbility _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__0(int _003C_003E1__state)
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
			JobDriver_CastAbility CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals14.FailOnDespawnedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals14.FailOn(() => !CS_0024_003C_003E8__locals14.job.ability.CanCast && !CS_0024_003C_003E8__locals14.job.ability.Casting);
				CS_0024_003C_003E8__locals14.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals14.job.ability != null && CS_0024_003C_003E8__locals14.job.def.abilityCasting)
					{
						CS_0024_003C_003E8__locals14.job.ability.StartCooldown(CS_0024_003C_003E8__locals14.job.ability.def.cooldownTicksRange.RandomInRange);
					}
				});
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					CS_0024_003C_003E8__locals14.pawn.pather.StopDead();
				};
				toil2.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, canHitNonTargetPawns: false);
				if (CS_0024_003C_003E8__locals14.job.ability != null && CS_0024_003C_003E8__locals14.job.ability.def.showCastingProgressBar && CS_0024_003C_003E8__locals14.job.verbToUse != null)
				{
					toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals14.job.verbToUse.WarmupProgress);
				}
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__0 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__0(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__0))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__0(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		job.ability?.Notify_StartedCasting();
	}

	public override string GetReport()
	{
		string text = "";
		text = ((job.ability == null || job.ability.def.targetRequired) ? base.GetReport() : ((string)"UsingVerbNoTarget".Translate(job.verbToUse.ReportLabel)));
		if (job.ability != null && job.ability.def.showCastingProgressBar)
		{
			text += " " + "DurationLeft".Translate(job.verbToUse.WarmupTicksLeft.ToStringSecondsFromTicks()) + ".";
		}
		return text;
	}
}
