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

public class JobDriver_Fish : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Fish _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
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
			JobDriver_Fish CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				CS_0024_003C_003E8__locals8.FailOn(() => !(CS_0024_003C_003E8__locals8.job.GetTarget(TargetIndex.A).Cell.GetZone(CS_0024_003C_003E8__locals8.pawn.Map) is Zone_Fishing zone_Fishing) || !zone_Fishing.Allowed);
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				int ticks = Mathf.RoundToInt(7500f / CS_0024_003C_003E8__locals8.pawn.GetStatValue(StatDefOf.FishingSpeed));
				Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: false, maintainPosture: true, maintainSleep: false, TargetIndex.A);
				toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
				{
					CS_0024_003C_003E8__locals8.pawn.skills?.Learn(SkillDefOf.Animals, 0.025f);
					CS_0024_003C_003E8__locals8.pawn.GainComfortFromCellIfPossible(1, chairsOnly: true);
				});
				toil.WithEffect(EffecterDefOf.Fishing, () => CS_0024_003C_003E8__locals8.job.GetTarget(TargetIndex.A), null);
				toil.WithProgressBarToilDelay(TargetIndex.B);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals8.CompleteFishingToil();
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
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

	private const TargetIndex SpotInd = TargetIndex.A;

	private const TargetIndex StandInd = TargetIndex.B;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed))
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil CompleteFishingToil()
	{
		Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
		toil.initAction = delegate
		{
			IntVec3 cell = job.GetTarget(TargetIndex.A).Cell;
			List<NegativeFishingOutcomeDef> negativeFishingOutcomes = FishingUtility.GetNegativeFishingOutcomes(pawn, cell);
			bool rare;
			List<Thing> catchesFor = FishingUtility.GetCatchesFor(pawn, cell, animalFishing: false, out rare);
			if (negativeFishingOutcomes.Any())
			{
				NegativeFishingOutcomeDef negativeFishingOutcomeDef = negativeFishingOutcomes.RandomElement();
				pawn.Map.waterBodyTracker.lastNegativeCatchTick = Find.TickManager.TicksGame;
				if (negativeFishingOutcomeDef.damageDef != null)
				{
					DamageInfo dinfo = new DamageInfo(negativeFishingOutcomeDef.damageDef, negativeFishingOutcomeDef.damageAmountRange.RandomInRange);
					dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					pawn.TakeDamage(dinfo);
				}
				if (negativeFishingOutcomeDef.addsHediff != null)
				{
					Hediff hediff = pawn.health.AddHediff(negativeFishingOutcomeDef.addsHediff, null, null);
					if (negativeFishingOutcomeDef.hediffSeverity > 0f)
					{
						hediff.Severity = negativeFishingOutcomeDef.hediffSeverity;
					}
				}
				Find.LetterStack.ReceiveLetter(negativeFishingOutcomeDef.letterLabel, negativeFishingOutcomeDef.letterText.Formatted(pawn.Named("PAWN")), negativeFishingOutcomeDef.letterDef, pawn);
			}
			else if (catchesFor.Any())
			{
				bool flag = false;
				int num = catchesFor.Sum((Thing x) => x.stackCount);
				foreach (Thing item in catchesFor)
				{
					flag |= GenPlace.TryPlaceThing(item, pawn.Position, pawn.Map, ThingPlaceMode.Near, null, null, null);
				}
				if (flag)
				{
					if (rare)
					{
						pawn.Map.waterBodyTracker.lastRareCatchTick = Find.TickManager.TicksGame;
						Find.LetterStack.ReceiveLetter("LetterLabelRareCatch".Translate(), "LetterTextRareCatch".Translate(pawn.Named("PAWN")) + ":\n" + catchesFor.Select((Thing x) => x.LabelCap).ToLineList("  - "), LetterDefOf.PositiveEvent, catchesFor);
					}
					else
					{
						pawn.Map.waterBodyTracker.Notify_Fished(cell, num);
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SlaughteredFish, pawn.Named(HistoryEventArgsNames.Doer)));
					}
				}
			}
			(cell.GetZone(pawn.Map) as Zone_Fishing)?.Notify_Fished();
		};
		toil.PlaySoundAtStart(SoundDefOf.Interact_CatchFish);
		return toil;
	}
}
