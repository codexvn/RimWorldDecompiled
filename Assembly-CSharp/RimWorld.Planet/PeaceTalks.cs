using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet;

public class PeaceTalks : WorldObject
{
	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__12 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PeaceTalks _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

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
		public _003CGetFloatMenuOptions_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PeaceTalks peaceTalks = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)peaceTalks).GetFloatMenuOptions(caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, peaceTalks).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
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

		private void _003C_003Em__Finally2()
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
			_003CGetFloatMenuOptions_003Ed__12 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private Material cachedMat;

	private static readonly SimpleCurve BadOutcomeChanceFactorByNegotiationAbility = new SimpleCurve
	{
		new CurvePoint(0f, 4f),
		new CurvePoint(1f, 1f),
		new CurvePoint(1.5f, 0.4f)
	};

	private const float BaseWeight_Disaster = 0.05f;

	private const float BaseWeight_Backfire = 0.1f;

	private const float BaseWeight_TalksFlounder = 0.2f;

	private const float BaseWeight_Success = 0.55f;

	private const float BaseWeight_Triumph = 0.1f;

	public const float LeaderOffset = 0.05f;

	private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();

	public override Material Material
	{
		get
		{
			if (cachedMat == null)
			{
				cachedMat = MaterialPool.MatFrom(color: (base.Faction == null) ? Color.white : base.Faction.Color, texPath: def.texture, shader: ShaderDatabase.WorldOverlayTransparentLit, renderQueue: 3550);
			}
			return cachedMat;
		}
	}

	public void Notify_CaravanArrived(Caravan caravan)
	{
		Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
		if (pawn == null)
		{
			Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, historical: false);
			return;
		}
		float badOutcomeWeightFactor = GetBadOutcomeWeightFactor(pawn, caravan);
		float num = 1f / badOutcomeWeightFactor;
		tmpPossibleOutcomes.Clear();
		tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
		{
			Outcome_Disaster(caravan);
		}, 0.05f * badOutcomeWeightFactor));
		tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
		{
			Outcome_Backfire(caravan);
		}, 0.1f * badOutcomeWeightFactor));
		tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
		{
			Outcome_TalksFlounder(caravan);
		}, 0.2f));
		tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
		{
			Outcome_Success(caravan);
		}, 0.55f * num));
		tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate
		{
			Outcome_Triumph(caravan);
		}, 0.1f * num));
		tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First();
		pawn.skills.Learn(SkillDefOf.Social, 6000f, direct: true);
		QuestUtility.SendQuestTargetSignals(questTags, "Resolved", this.Named("SUBJECT"));
		Destroy();
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__12))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__12(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	private void Outcome_Disaster(Caravan caravan)
	{
		LongEventHandler.QueueLongEvent(delegate
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, Mathf.Min(randomInRange, Faction.OfPlayer.GoodwillToMakeHostile(base.Faction)), canSendMessage: false, canSendHostilityLetter: false, HistoryEventDefOf.PeaceTalksDisaster, null);
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
			incidentParms.faction = base.Faction;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, ensureCanGenerateAtLeastOnePawn: true);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
			Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, sendLetterIfRelatedPawns: false);
			if (list.Any())
			{
				LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(base.Faction), map, list);
			}
			Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			GlobalTargetInfo globalTargetInfo = (list.Any() ? new GlobalTargetInfo(list[0].Position, map) : GlobalTargetInfo.Invalid);
			TaggedString letterLabel = "LetterLabelPeaceTalks_Disaster".Translate();
			TaggedString letterText = GetLetterText("LetterPeaceTalks_Disaster".Translate(base.Faction.def.pawnsPlural.CapitalizeFirst(), base.Faction.NameColored, Mathf.RoundToInt(randomInRange)), caravan, playerRelationKind);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref letterLabel, ref letterText, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), informEvenIfSeenBefore: true);
			Find.LetterStack.ReceiveLetter(letterLabel, letterText, LetterDefOf.ThreatBig, globalTargetInfo, base.Faction);
		}, "GeneratingMapForNewEncounter", doAsynchronously: false, null);
	}

	private void Outcome_Backfire(Caravan caravan)
	{
		FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
		int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
		Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, canSendMessage: false, canSendHostilityLetter: false, HistoryEventDefOf.PeaceTalksBackfire, null);
		Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind), LetterDefOf.NegativeEvent, caravan, base.Faction);
	}

	private void Outcome_TalksFlounder(Caravan caravan)
	{
		Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.NameColored), caravan, base.Faction.PlayerRelationKind), LetterDefOf.NeutralEvent, caravan, base.Faction);
	}

	private void Outcome_Success(Caravan caravan)
	{
		FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
		int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
		Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, canSendMessage: false, canSendHostilityLetter: false, HistoryEventDefOf.PeaceTalksSuccess, null);
		Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction);
	}

	private void Outcome_Triumph(Caravan caravan)
	{
		FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
		int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
		Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, canSendMessage: false, canSendHostilityLetter: false, HistoryEventDefOf.PeaceTalksTriumph, null);
		ThingSetMakerParams parms = default(ThingSetMakerParams);
		parms.makingFaction = base.Faction;
		parms.techLevel = base.Faction.def.techLevel;
		parms.maxTotalMass = 20f;
		parms.totalMarketValueRange = new FloatRange(500f, 1200f);
		parms.tile = base.Tile;
		List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
		for (int i = 0; i < list.Count; i++)
		{
			caravan.AddPawnOrItem(list[i], addCarriedPawnToWorldPawnsIfAny: true);
		}
		Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.NameColored, randomInRange, GenLabel.ThingsLabel(list)), caravan, playerRelationKind, TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction);
	}

	private int TryGainRoyalFavor(Caravan caravan)
	{
		int num = 0;
		if (base.Faction.def.HasRoyalTitles)
		{
			num = DiplomacyTuning.RoyalFavor_PeaceTalksSuccessRange.RandomInRange;
			BestCaravanPawnUtility.FindBestDiplomat(caravan)?.royalty.GainFavor(base.Faction, num);
		}
		return num;
	}

	private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind, int royalFavorGained = 0)
	{
		TaggedString text = baseText;
		Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
		if (pawn != null)
		{
			text += "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f.ToString("F0"), pawn.Named("PAWN"));
			if (royalFavorGained > 0)
			{
				text += "\n\n" + "PeaceTalksRoyalFavorGain".Translate(pawn.LabelShort, royalFavorGained.ToString(), base.Faction.Named("FACTION"), pawn.Named("PAWN"));
			}
		}
		base.Faction.TryAppendRelationKindChangedInfo(ref text, previousRelationKind, base.Faction.PlayerRelationKind);
		return text;
	}

	private static float GetBadOutcomeWeightFactor(Pawn diplomat, Caravan caravan)
	{
		float statValue = diplomat.GetStatValue(StatDefOf.NegotiationAbility);
		float num = 0f;
		if (ModsConfig.IdeologyActive)
		{
			bool flag = false;
			foreach (Pawn pawn in caravan.pawns)
			{
				if (pawn == caravan.Faction.leader)
				{
					flag = true;
					break;
				}
			}
			num = (flag ? (-0.05f) : 0.05f);
		}
		return GetBadOutcomeWeightFactor(statValue) * (1f + num);
	}

	private static float GetBadOutcomeWeightFactor(float negotationAbility)
	{
		return BadOutcomeChanceFactorByNegotiationAbility.Evaluate(negotationAbility);
	}

	[DebugOutput("Incidents", false)]
	private static void PeaceTalksChances()
	{
		StringBuilder stringBuilder = new StringBuilder();
		AppendDebugChances(stringBuilder, 0f);
		AppendDebugChances(stringBuilder, 1f);
		AppendDebugChances(stringBuilder, 1.5f);
		Log.Message(stringBuilder.ToString());
	}

	private static void AppendDebugChances(StringBuilder sb, float negotiationAbility)
	{
		if (sb.Length > 0)
		{
			sb.AppendLine();
		}
		sb.AppendLine("--- NegotiationAbility = " + negotiationAbility.ToStringPercent() + " ---");
		float badOutcomeWeightFactor = GetBadOutcomeWeightFactor(negotiationAbility);
		float num = 1f / badOutcomeWeightFactor;
		sb.AppendLine("Bad outcome weight factor: " + badOutcomeWeightFactor.ToString("0.##"));
		float num2 = 0.05f * badOutcomeWeightFactor;
		float num3 = 0.1f * badOutcomeWeightFactor;
		float num4 = 0.2f;
		float num5 = 0.55f * num;
		float num6 = 0.1f * num;
		float num7 = num2 + num3 + num4 + num5 + num6;
		sb.AppendLine("Disaster: " + (num2 / num7).ToStringPercent());
		sb.AppendLine("Backfire: " + (num3 / num7).ToStringPercent());
		sb.AppendLine("Talks flounder: " + (num4 / num7).ToStringPercent());
		sb.AppendLine("Success: " + (num5 / num7).ToStringPercent());
		sb.AppendLine("Triumph: " + (num6 / num7).ToStringPercent());
	}
}
