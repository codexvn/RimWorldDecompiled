using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x02003DB3 RID: 15795
	public class PeaceTalks : WorldObject
	{
		// Token: 0x1700358F RID: 13711
		// (get) Token: 0x06015874 RID: 88180 RVA: 0x00672A9C File Offset: 0x00670C9C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x06015875 RID: 88181 RVA: 0x00672AFC File Offset: 0x00670CFC
		public void Notify_CaravanArrived(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn == null)
			{
				Messages.Message("MessagePeaceTalksNoDiplomat".Translate(), caravan, MessageTypeDefOf.NegativeEvent, false);
				return;
			}
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(pawn, caravan);
			float num = 1f / badOutcomeWeightFactor;
			PeaceTalks.tmpPossibleOutcomes.Clear();
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Disaster(caravan);
			}, 0.05f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Backfire(caravan);
			}, 0.1f * badOutcomeWeightFactor));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_TalksFlounder(caravan);
			}, 0.2f));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Success(caravan);
			}, 0.55f * num));
			PeaceTalks.tmpPossibleOutcomes.Add(new Pair<Action, float>(delegate()
			{
				this.Outcome_Triumph(caravan);
			}, 0.1f * num));
			PeaceTalks.tmpPossibleOutcomes.RandomElementByWeight((Pair<Action, float> x) => x.Second).First();
			pawn.skills.Learn(SkillDefOf.Social, 6000f, true, false);
			QuestUtility.SendQuestTargetSignals(this.questTags, "Resolved", this.Named("SUBJECT"));
			this.Destroy();
		}

		// Token: 0x06015876 RID: 88182 RVA: 0x00672C88 File Offset: 0x00670E88
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitPeaceTalks.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015877 RID: 88183 RVA: 0x00672C9F File Offset: 0x00670E9F
		private void Outcome_Disaster(Caravan caravan)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				FactionRelationKind playerRelationKind = this.Faction.PlayerRelationKind;
				int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksDisasterRange.RandomInRange;
				Faction.OfPlayer.TryAffectGoodwillWith(this.Faction, Mathf.Min(randomInRange, Faction.OfPlayer.GoodwillToMakeHostile(this.Faction)), false, false, HistoryEventDefOf.PeaceTalksDisaster, null);
				IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, caravan);
				incidentParms.faction = this.Faction;
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, true);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, list, false);
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(incidentParms.faction, new LordJob_AssaultColony(this.Faction, true, true, false, false, true, false, false), map, list);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				GlobalTargetInfo target = list.Any<Pawn>() ? new GlobalTargetInfo(list[0].Position, map, false) : GlobalTargetInfo.Invalid;
				TaggedString label = "LetterLabelPeaceTalks_Disaster".Translate();
				TaggedString text = this.GetLetterText("LetterPeaceTalks_Disaster".Translate(this.Faction.def.pawnsPlural.CapitalizeFirst(), this.Faction.NameColored, Mathf.RoundToInt((float)randomInRange)), caravan, playerRelationKind, 0);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref label, ref text, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, target, this.Faction, null, null, null, 0, true);
			}, "GeneratingMapForNewEncounter", false, null, true, false, null);
		}

		// Token: 0x06015878 RID: 88184 RVA: 0x00672CD0 File Offset: 0x00670ED0
		private void Outcome_Backfire(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksBackfireRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksBackfire, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Backfire".Translate(), this.GetLetterText("LetterPeaceTalks_Backfire".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, 0), LetterDefOf.NegativeEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x06015879 RID: 88185 RVA: 0x00672D78 File Offset: 0x00670F78
		private void Outcome_TalksFlounder(Caravan caravan)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_TalksFlounder".Translate(), this.GetLetterText("LetterPeaceTalks_TalksFlounder".Translate(base.Faction.NameColored), caravan, base.Faction.PlayerRelationKind, 0), LetterDefOf.NeutralEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0601587A RID: 88186 RVA: 0x00672DE8 File Offset: 0x00670FE8
		private void Outcome_Success(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksSuccessRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksSuccess, null);
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Success".Translate(), this.GetLetterText("LetterPeaceTalks_Success".Translate(base.Faction.NameColored, randomInRange), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0601587B RID: 88187 RVA: 0x00672E94 File Offset: 0x00671094
		private void Outcome_Triumph(Caravan caravan)
		{
			FactionRelationKind playerRelationKind = base.Faction.PlayerRelationKind;
			int randomInRange = DiplomacyTuning.Goodwill_PeaceTalksTriumphRange.RandomInRange;
			Faction.OfPlayer.TryAffectGoodwillWith(base.Faction, randomInRange, false, false, HistoryEventDefOf.PeaceTalksTriumph, null);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.makingFaction = base.Faction;
			parms.techLevel = new TechLevel?(base.Faction.def.techLevel);
			parms.maxTotalMass = new float?((float)20);
			parms.totalMarketValueRange = new FloatRange?(new FloatRange(500f, 1200f));
			parms.tile = new PlanetTile?(base.Tile);
			List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				caravan.AddPawnOrItem(list[i], true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPeaceTalks_Triumph".Translate(), this.GetLetterText("LetterPeaceTalks_Triumph".Translate(base.Faction.NameColored, randomInRange, GenLabel.ThingsLabel(list, "  - ")), caravan, playerRelationKind, this.TryGainRoyalFavor(caravan)), LetterDefOf.PositiveEvent, caravan, base.Faction, null, null, null, 0, true);
		}

		// Token: 0x0601587C RID: 88188 RVA: 0x00672FF4 File Offset: 0x006711F4
		private int TryGainRoyalFavor(Caravan caravan)
		{
			int num = 0;
			if (base.Faction.def.HasRoyalTitles)
			{
				num = DiplomacyTuning.RoyalFavor_PeaceTalksSuccessRange.RandomInRange;
				Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
				if (pawn != null)
				{
					pawn.royalty.GainFavor(base.Faction, num);
				}
			}
			return num;
		}

		// Token: 0x0601587D RID: 88189 RVA: 0x00673040 File Offset: 0x00671240
		private string GetLetterText(string baseText, Caravan caravan, FactionRelationKind previousRelationKind, int royalFavorGained = 0)
		{
			TaggedString taggedString = baseText;
			Pawn pawn = BestCaravanPawnUtility.FindBestDiplomat(caravan);
			if (pawn != null)
			{
				taggedString += "\n\n" + "PeaceTalksSocialXPGain".Translate(pawn.LabelShort, 6000f.ToString("F0"), pawn.Named("PAWN"));
				if (royalFavorGained > 0)
				{
					taggedString += "\n\n" + "PeaceTalksRoyalFavorGain".Translate(pawn.LabelShort, royalFavorGained.ToString(), base.Faction.Named("FACTION"), pawn.Named("PAWN"));
				}
			}
			base.Faction.TryAppendRelationKindChangedInfo(ref taggedString, previousRelationKind, base.Faction.PlayerRelationKind, null);
			return taggedString;
		}

		// Token: 0x0601587E RID: 88190 RVA: 0x0067311C File Offset: 0x0067131C
		private static float GetBadOutcomeWeightFactor(Pawn diplomat, Caravan caravan)
		{
			float statValue = diplomat.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
			float num = 0f;
			if (ModsConfig.IdeologyActive)
			{
				bool flag = false;
				using (List<Pawn>.Enumerator enumerator = caravan.pawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == caravan.Faction.leader)
						{
							flag = true;
							break;
						}
					}
				}
				num = (flag ? -0.05f : 0.05f);
			}
			return PeaceTalks.GetBadOutcomeWeightFactor(statValue) * (1f + num);
		}

		// Token: 0x0601587F RID: 88191 RVA: 0x006731B4 File Offset: 0x006713B4
		private static float GetBadOutcomeWeightFactor(float negotationAbility)
		{
			return PeaceTalks.BadOutcomeChanceFactorByNegotiationAbility.Evaluate(negotationAbility);
		}

		// Token: 0x06015880 RID: 88192 RVA: 0x006731C1 File Offset: 0x006713C1
		[DebugOutput("Incidents", false)]
		private static void PeaceTalksChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PeaceTalks.AppendDebugChances(stringBuilder, 0f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1f);
			PeaceTalks.AppendDebugChances(stringBuilder, 1.5f);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06015881 RID: 88193 RVA: 0x006731F4 File Offset: 0x006713F4
		private static void AppendDebugChances(StringBuilder sb, float negotiationAbility)
		{
			if (sb.Length > 0)
			{
				sb.AppendLine();
			}
			sb.AppendLine("--- NegotiationAbility = " + negotiationAbility.ToStringPercent() + " ---");
			float badOutcomeWeightFactor = PeaceTalks.GetBadOutcomeWeightFactor(negotiationAbility);
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

		// Token: 0x0400EF72 RID: 61298
		private Material cachedMat;

		// Token: 0x0400EF73 RID: 61299
		private static readonly SimpleCurve BadOutcomeChanceFactorByNegotiationAbility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 4f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.4f),
				true
			}
		};

		// Token: 0x0400EF74 RID: 61300
		private const float BaseWeight_Disaster = 0.05f;

		// Token: 0x0400EF75 RID: 61301
		private const float BaseWeight_Backfire = 0.1f;

		// Token: 0x0400EF76 RID: 61302
		private const float BaseWeight_TalksFlounder = 0.2f;

		// Token: 0x0400EF77 RID: 61303
		private const float BaseWeight_Success = 0.55f;

		// Token: 0x0400EF78 RID: 61304
		private const float BaseWeight_Triumph = 0.1f;

		// Token: 0x0400EF79 RID: 61305
		public const float LeaderOffset = 0.05f;

		// Token: 0x0400EF7A RID: 61306
		private static List<Pair<Action, float>> tmpPossibleOutcomes = new List<Pair<Action, float>>();
	}
}
