using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Skygaze : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Skygaze _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__1(int _003C_003E1__state)
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
			JobDriver_Skygaze CS_0024_003C_003E8__locals9 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals9.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
				};
				toil.tickIntervalAction = delegate(int delta)
				{
					float extraJoyGainFactor = CS_0024_003C_003E8__locals9.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(CS_0024_003C_003E8__locals9.pawn.Map);
					JoyUtility.JoyTickCheckEnd(CS_0024_003C_003E8__locals9.pawn, delta, JoyTickFullJoyAction.EndJob, extraJoyGainFactor);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = CS_0024_003C_003E8__locals9.job.def.joyDuration;
				toil.FailOn(() => CS_0024_003C_003E8__locals9.pawn.Position.Roofed(CS_0024_003C_003E8__locals9.pawn.Map));
				toil.FailOn(() => CS_0024_003C_003E8__locals9.pawn.Map.weatherManager.curWeather.preventSkygaze);
				toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(CS_0024_003C_003E8__locals9.pawn));
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
			_003CMakeNewToils_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__1(0)
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

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__1))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		if (base.Map.TileInfo.Layer.Def.isSpace)
		{
			return "Stargazing".Translate();
		}
		if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
		{
			return "WatchingEclipse".Translate();
		}
		if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
		{
			return "WatchingAurora".Translate();
		}
		float num = GenCelestial.CurCelestialSunGlow(base.Map);
		if (num < 0.1f)
		{
			return "Stargazing".Translate();
		}
		if (num < 0.65f)
		{
			if (GenLocalDate.DayPercent(pawn) < 0.5f)
			{
				return "WatchingSunrise".Translate();
			}
			return "WatchingSunset".Translate();
		}
		return "CloudWatching".Translate();
	}
}
