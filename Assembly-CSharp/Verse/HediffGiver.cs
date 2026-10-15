using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse;

public abstract class HediffGiver
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__13 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffGiver _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__13(int _003C_003E1__state)
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
			HediffGiver hediffGiver = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (hediffGiver.hediff == null)
				{
					_003C_003E2__current = "hediff is null";
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[TranslationHandle]
	public HediffDef hediff;

	public List<BodyPartDef> partsToAffect;

	public bool canAffectAnyLivePart;

	public bool allowOnLodgers = true;

	public bool allowOnQuestRewardPawns = true;

	public bool allowOnQuestReservedPawns = true;

	public bool allowOnBeggars = true;

	public int countToAffect = 1;

	public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
	{
	}

	public virtual float ChanceFactor(Pawn pawn)
	{
		if (ModsConfig.BiotechActive && hediff == HediffDefOf.Carcinoma)
		{
			return pawn.GetStatValue(StatDefOf.CancerRate);
		}
		return 1f;
	}

	public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
	{
		return false;
	}

	public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
	{
		if (!allowOnLodgers && pawn.IsQuestLodger())
		{
			return false;
		}
		if (!allowOnQuestRewardPawns && pawn.IsWorldPawn() && pawn.IsQuestReward())
		{
			return false;
		}
		if (!allowOnQuestReservedPawns && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
		{
			return false;
		}
		if (ModsConfig.IdeologyActive && !allowOnBeggars && pawn.kindDef == PawnKindDefOf.Beggar)
		{
			return false;
		}
		if (pawn.ageTracker.CurLifeStage == LifeStageDefOf.HumanlikeBaby && Find.Storyteller.difficulty.babiesAreHealthy)
		{
			return false;
		}
		if (pawn.genes != null && !pawn.genes.HediffGiversCanGive(hediff))
		{
			return false;
		}
		if (pawn.IsMutant && !pawn.mutant.HediffGiversCanGive(hediff))
		{
			return false;
		}
		return HediffGiverUtility.TryApply(pawn, hediff, partsToAffect, canAffectAnyLivePart, countToAffect, outAddedHediffs);
	}

	protected void SendLetter(Pawn pawn, Hediff cause)
	{
		if (PawnUtility.ShouldSendNotificationAbout(pawn))
		{
			if (cause == null)
			{
				Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShortCap, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShortCap, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn);
			}
			else
			{
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShortCap, hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__13))]
	public virtual IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}
}
