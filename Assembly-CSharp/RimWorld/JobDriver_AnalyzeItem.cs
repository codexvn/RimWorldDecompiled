using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_AnalyzeItem : JobDriver_StudyItem
{
	[CompilerGenerated]
	private sealed class _003CGetStudyToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_AnalyzeItem _003C_003E4__this;

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
		public _003CGetStudyToils_003Ed__2(int _003C_003E1__state)
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
			JobDriver_AnalyzeItem CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				int num2 = Mathf.CeilToInt((float)Mathf.CeilToInt(CS_0024_003C_003E8__locals6.AnalyzableComp.Props.analysisDurationHours * 2500f) / CS_0024_003C_003E8__locals6.pawn.GetStatValue(StatDefOf.ResearchSpeed));
				Toil toil = Toils_General.Wait(num2).FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A, num2);
				toil.activeSkill = () => SkillDefOf.Intellectual;
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate
				{
					CS_0024_003C_003E8__locals6.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals6.job.GetTarget(TargetIndex.A));
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					CS_0024_003C_003E8__locals6.AnalyzableComp.OnAnalyzed(CS_0024_003C_003E8__locals6.pawn);
				});
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CGetStudyToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetStudyToils_003Ed__2(0)
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

	public CompAnalyzable AnalyzableComp => base.ThingToStudy.TryGetComp<CompAnalyzable>();

	[IteratorStateMachine(typeof(_003CGetStudyToils_003Ed__2))]
	protected override IEnumerable<Toil> GetStudyToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetStudyToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
