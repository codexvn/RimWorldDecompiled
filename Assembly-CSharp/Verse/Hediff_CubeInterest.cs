using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse;

public class Hediff_CubeInterest : Hediff
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__23 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_CubeInterest _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__23(int _003C_003E1__state)
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
			Hediff_CubeInterest hediff_CubeInterest = _003C_003E4__this;
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
				if (!hediff_CubeInterest.InWithdrawal)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Cube withdrawal",
						action = hediff_CubeInterest.StartWithdrawal
					};
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0070;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0070;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0070:
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Cube obsession",
					action = hediff_CubeInterest.DoMentalBreak
				};
				_003C_003E1__state = 2;
				return true;
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
			_003CGetGizmos_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__23(0)
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

	private List<Thing> sculptures = new List<Thing>();

	private Hediff_CubeWithdrawal cachedHediff;

	private const float MTBPlayWantedDays = 1.5f;

	private const int InitialDelayTicks = 1000;

	private const int SculptureUpdateRateTicks = 60;

	private static readonly FloatRange SeverityPerPlay = new FloatRange(0.045f, 0.105f);

	private static readonly SimpleCurve MentalBreakSeverityDaysMTB = new SimpleCurve
	{
		new CurvePoint(0.01f, 5f),
		new CurvePoint(0.5f, 3f),
		new CurvePoint(0.8f, 2f)
	};

	private static readonly SimpleCurve CubeRageSeverityIncrease = new SimpleCurve
	{
		new CurvePoint(0.1f, 0.1f),
		new CurvePoint(0.5f, 0.2f),
		new CurvePoint(1f, 0.4f)
	};

	public Hediff_CubeWithdrawal WithdrawalHediff => cachedHediff ?? (cachedHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_CubeWithdrawal>());

	public bool InWithdrawal => pawn.health.hediffSet.HasHediff(HediffDefOf.CubeWithdrawal);

	public int Sculptures => sculptures.Count;

	public bool InCaravanWithCube
	{
		get
		{
			Caravan caravan = pawn.GetCaravan();
			if (caravan == null)
			{
				return false;
			}
			return CaravanInventoryUtility.HasThings(caravan, ThingDefOf.GoldenCube, 1);
		}
	}

	public override void PostAdd(DamageInfo? dinfo)
	{
		if (!ModLister.CheckAnomaly("Cube interest"))
		{
			pawn.health.RemoveHediff(this);
		}
		else
		{
			base.PostAdd(dinfo);
		}
	}

	public override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (ageTicks > 1000)
		{
			if (Rand.MTBEventOccurs(1.5f, 60000f, delta) && !InWithdrawal && !InCaravanWithCube)
			{
				StartWithdrawal();
			}
			if (Rand.MTBEventOccurs(MentalBreakSeverityDaysMTB.Evaluate(Severity), 60000f, delta) && !pawn.InMentalState && pawn.CurJobDef != JobDefOf.GoldenCubePlay && pawn.SpawnedOrAnyParentSpawned)
			{
				DoMentalBreak();
			}
			if (GenTicks.IsTickIntervalDelta(60, delta))
			{
				UpdateSculptures();
			}
		}
	}

	private void UpdateSculptures()
	{
		bool flag = false;
		for (int num = sculptures.Count - 1; num >= 0; num--)
		{
			if (sculptures[num].DestroyedOrNull() || (!sculptures[num].SpawnedOrAnyParentSpawned && !sculptures[num].IsInCaravan()))
			{
				flag = true;
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DestroyedCubeSculpture);
				sculptures.RemoveAt(num);
				float num2 = CubeRageSeverityIncrease.Evaluate(Severity);
				if (num2 > 0f)
				{
					HealthUtility.AdjustSeverity(pawn, HediffDefOf.CubeRage, num2);
				}
			}
		}
		if (flag)
		{
			Messages.Message("MessageGoldenCubeSculptureDestroyed".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent);
		}
	}

	public override void PostRemoved()
	{
		base.PostRemoved();
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeRage, out var hediff))
		{
			pawn.health.RemoveHediff(hediff);
		}
	}

	protected override void OnStageIndexChanged(int stageIndex)
	{
		if (PawnUtility.ShouldSendNotificationAbout(pawn))
		{
			Messages.Message("MessageGoldenCubeSeverityIncreased".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent);
		}
	}

	public void Notify_BuiltSculpture(Thing sculpture)
	{
		sculptures.Add(sculpture);
	}

	public void Notify_PlayedWith()
	{
		if (InWithdrawal)
		{
			pawn.health.RemoveHediff(WithdrawalHediff);
			cachedHediff = null;
		}
		Severity += SeverityPerPlay.RandomInRange;
		pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.PlayedWithCube);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__23))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DoMentalBreak()
	{
		if (!pawn.InMentalState)
		{
			pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_CubeSculpting".Translate(pawn.Named("PAWN")), MentalBreakDefOf.CubeSculpting);
		}
	}

	private void StartWithdrawal()
	{
		if (!InWithdrawal)
		{
			cachedHediff = (Hediff_CubeWithdrawal)pawn.health.AddHediff(HediffDefOf.CubeWithdrawal, null, null);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref sculptures, "sculptures", LookMode.Reference);
	}
}
