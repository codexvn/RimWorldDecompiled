using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse;

public class Hediff_LaborPushing : HediffWithParents
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_LaborPushing _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__6(int _003C_003E1__state)
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
				Hediff_LaborPushing CS_0024_003C_003E8__locals7 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffWithParents)CS_0024_003C_003E8__locals7).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Force stillborn",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.ForceBirth(-1);
						}
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Force infant illness",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.ForceBirth(0);
						}
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Force healthy",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.ForceBirth(1);
						}
					};
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Force end",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.pawn.health.RemoveHediff(CS_0024_003C_003E8__locals7);
						}
					};
					_003C_003E1__state = 6;
					return true;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = PregnancyUtility.BirthQualityGizmo(CS_0024_003C_003E8__locals7.pawn);
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
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
			_003CGetGizmos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__6(0)
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

	public int laborDuration;

	private Effecter progressBar;

	private RitualOutcomePossibility debugForceBirthOutcome;

	private bool preventLetter;

	public float Progress
	{
		get
		{
			HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
			return 1f - (float)hediffComp_Disappears.ticksToDisappear / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + laborDuration);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void ForceBirth(int positivityIndex, bool dontSendLetter = false)
	{
		Precept_Ritual precept_Ritual = (Precept_Ritual)pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
		debugForceBirthOutcome = precept_Ritual.outcomeEffect.def.outcomeChances.FirstOrDefault((RitualOutcomePossibility o) => o.positivityIndex == positivityIndex);
		preventLetter = dontSendLetter;
		pawn.health.RemoveHediff(this);
	}

	public override void PreRemoved()
	{
		base.PreRemoved();
		LordJob_Ritual lordJob_Ritual = pawn.GetLord()?.LordJob as LordJob_Ritual;
		Precept_Ritual precept_Ritual = (Precept_Ritual)pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
		if (lordJob_Ritual?.Ritual == null || lordJob_Ritual.Ritual.def != PreceptDefOf.ChildBirth || lordJob_Ritual.assignments.FirstAssignedPawn("mother") != pawn)
		{
			float birthQualityFor = PregnancyUtility.GetBirthQualityFor(pawn);
			PregnancyUtility.ApplyBirthOutcome(debugForceBirthOutcome ?? ((RitualOutcomeEffectWorker_ChildBirth)precept_Ritual.outcomeEffect).GetOutcome(birthQualityFor, null), assignments: PregnancyUtility.RitualAssignmentsForBirth(precept_Ritual, pawn), quality: birthQualityFor, ritual: precept_Ritual, genes: geneSet?.GenesListForReading, geneticMother: base.Mother ?? pawn, birtherThing: pawn, father: base.Father, doctor: null, lordJobRitual: null, preventLetter: preventLetter);
		}
		else
		{
			lordJob_Ritual?.ApplyOutcome(1f);
		}
	}

	public override void TickInterval(int delta)
	{
		if (pawn.SpawnedOrAnyParentSpawned)
		{
			if (progressBar == null)
			{
				progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			progressBar.EffectTick(pawn, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = Progress;
			}
		}
		else
		{
			progressBar?.Cleanup();
			progressBar = null;
		}
	}

	public override void PostRemoved()
	{
		base.PostRemoved();
		progressBar?.Cleanup();
		progressBar = null;
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		base.Notify_PawnDied(dinfo, culprit);
		progressBar?.Cleanup();
		progressBar = null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref laborDuration, "laborDuration", 0);
	}
}
