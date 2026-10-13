using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039CC RID: 14796
	public static class QuestGen_Pawns
	{
		// Token: 0x060144E1 RID: 83169 RVA: 0x00610F5C File Offset: 0x0060F15C
		public static Pawn GeneratePawn(this Quest quest, PawnKindDef kindDef, Faction faction, bool allowAddictions = true, IEnumerable<TraitDef> forcedTraits = null, float biocodeWeaponChance = 0f, bool mustBeCapableOfViolence = true, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 0f, float biocodeApparelChance = 0f, bool ensureNonNumericName = false, bool forceGenerateNewPawn = false, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, bool allowPregnant = false)
		{
			return quest.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn, false, false, true, mustBeCapableOfViolence, 1f, false, true, allowPregnant, true, allowAddictions, false, false, false, false, biocodeWeaponChance, 0f, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, null, null, forcedTraits, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false)
			{
				BiocodeApparelChance = biocodeApparelChance
			}, ensureNonNumericName);
		}

		// Token: 0x060144E2 RID: 83170 RVA: 0x0061102C File Offset: 0x0060F22C
		public static Pawn GeneratePawn(this Quest quest, PawnGenerationRequest request, bool ensureNonNumericName = false)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (ensureNonNumericName && (pawn.Name == null || pawn.Name.Numerical))
			{
				Pawn pawn2 = pawn;
				Pawn pawn3 = pawn;
				NameStyle style = NameStyle.Full;
				string forcedLastName = null;
				bool forceNoNick = false;
				Pawn_GeneTracker genes = pawn.genes;
				pawn2.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn3, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x060144E3 RID: 83171 RVA: 0x00611094 File Offset: 0x0060F294
		public static bool GetPawnTest(QuestGen_Pawns.GetPawnParms parms, out Pawn pawn)
		{
			pawn = null;
			if (parms.mustHaveNoFaction && parms.mustHaveRoyalTitleInCurrentFaction)
			{
				return false;
			}
			if (parms.canGeneratePawn && (parms.mustBeFactionLeader || parms.mustBePlayerPrisoner || parms.mustBeFreeColonist))
			{
				Log.Warning("QuestGen_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBePlayerPrisoner, mustBeFreeColonist");
				return false;
			}
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			if (source.Count<Pawn>() > 0)
			{
				pawn = source.RandomElement<Pawn>();
				return true;
			}
			if (!parms.canGeneratePawn)
			{
				return false;
			}
			Faction faction;
			if (!parms.mustHaveNoFaction && !QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
			{
				return false;
			}
			FloatRange senRange = parms.seniorityRange;
			return !parms.mustHaveRoyalTitleInCurrentFaction || !parms.requireResearchedBedroomFurnitureIfRoyal || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x060144E4 RID: 83172 RVA: 0x00611158 File Offset: 0x0060F358
		public static Pawn GetPawn(this Quest quest, QuestGen_Pawns.GetPawnParms parms)
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			int num = source.Count<Pawn>();
			Faction faction;
			Pawn pawn;
			if (Rand.Chance(parms.canGeneratePawn ? Mathf.Clamp01(1f - (float)num / 10f) : 0f) && (parms.mustHaveNoFaction || QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction)))
			{
				pawn = QuestGen_Pawns.GeneratePawn(parms, null);
			}
			else
			{
				pawn = source.RandomElement<Pawn>();
				if (parms.redressPawn)
				{
					PawnGenerator.RedressPawn(pawn, parms.GenerationRequest(pawn.kindDef, pawn.Faction, null));
				}
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				quest.AddPart(new QuestPart_InvolvedFactions
				{
					factions = 
					{
						pawn.Faction
					}
				});
			}
			QuestGen.AddToGeneratedPawns(pawn);
			return pawn;
		}

		// Token: 0x060144E5 RID: 83173 RVA: 0x00611224 File Offset: 0x0060F424
		public static IEnumerable<Pawn> ExistingUsablePawns(QuestGen_Pawns.GetPawnParms parms)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where QuestGen_Pawns.IsGoodPawn(x, parms)
			select x;
		}

		// Token: 0x060144E6 RID: 83174 RVA: 0x00611254 File Offset: 0x0060F454
		private static bool TryFindFactionForPawnGeneration(QuestGen_Pawns.GetPawnParms parms, out Faction faction)
		{
			FactionManager factionManager = Find.FactionManager;
			bool allowTemporaryFactions = parms.allowTemporaryFactions;
			return (from x in factionManager.GetFactions(parms.allowHidden, false, false, TechLevel.Undefined, allowTemporaryFactions)
			where (parms.mustBeOfFaction == null || x == parms.mustBeOfFaction) && (parms.excludeFactionDefs == null || !parms.excludeFactionDefs.Contains(x.def)) && (!parms.mustHaveRoyalTitleInCurrentFaction || x.def.HasRoyalTitles) && (!parms.mustBeNonHostileToPlayer || !x.HostileTo(Faction.OfPlayer)) && (parms.allowPermanentEnemyFaction.GetValueOrDefault() || !x.def.permanentEnemy) && x.def.techLevel >= parms.minTechLevel
			select x).TryRandomElement(out faction);
		}

		// Token: 0x060144E7 RID: 83175 RVA: 0x006112AC File Offset: 0x0060F4AC
		private static Pawn GeneratePawn(QuestGen_Pawns.GetPawnParms parms, Faction faction = null)
		{
			PawnKindDef pawnKindDef = parms.mustBeOfKind;
			if (faction == null && !parms.mustHaveNoFaction)
			{
				if (!QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (parms.mustHaveRoyalTitleInCurrentFaction)
			{
				FloatRange senRange = parms.seniorityRange;
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (parms.requireResearchedBedroomFurnitureIfRoyal)
				{
					if (source.Any((RoyalTitleDef x) => QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)))
					{
						source = from x in source
						where QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)
						select x;
					}
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (parms.mustBeOfKind == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
				where k.titleRequired != null && k.titleRequired == fixedTitle
				select k).TryRandomElement(out pawnKindDef))
				{
					(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
					where k.titleSelectOne != null && k.titleSelectOne.Contains(fixedTitle)
					select k).TryRandomElement(out pawnKindDef);
				}
			}
			else
			{
				fixedTitle = null;
			}
			if (pawnKindDef == null)
			{
				pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
				where kind.race.race.Humanlike
				select kind).RandomElement<PawnKindDef>();
			}
			Pawn pawn = PawnGenerator.GeneratePawn(parms.GenerationRequest(pawnKindDef, faction, fixedTitle));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x060144E8 RID: 83176 RVA: 0x006114B8 File Offset: 0x0060F6B8
		private static bool IsGoodPawn(Pawn pawn, QuestGen_Pawns.GetPawnParms parms)
		{
			if (parms.mustBeFactionLeader)
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (parms.mustBeOfFaction != null && pawn.Faction != parms.mustBeOfFaction)
			{
				return false;
			}
			if (pawn.Faction != null && parms.excludeFactionDefs != null && parms.excludeFactionDefs.Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < parms.minTechLevel)
			{
				return false;
			}
			if (parms.mustBeOfKind != null && pawn.kindDef != parms.mustBeOfKind)
			{
				return false;
			}
			if (parms.mustHaveRoyalTitleInCurrentFaction && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (parms.seniorityRange != default(FloatRange) && (pawn.royalty == null || pawn.royalty.MostSeniorTitle == null || !parms.seniorityRange.IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority)))
			{
				return false;
			}
			if (parms.mustBeWorldPawn && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFree && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFreeOrLeader && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (parms.mustHaveNoFaction && pawn.Faction != null)
			{
				return false;
			}
			if (parms.mustBeFreeColonist && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (parms.mustBePlayerPrisoner && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (parms.mustBeNotSuspended && pawn.Suspended)
			{
				return false;
			}
			if (parms.mustBeNonHostileToPlayer && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!(parms.allowPermanentEnemyFaction ?? true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (parms.requireResearchedBedroomFurnitureIfRoyal)
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return !parms.mustBeCapableOfViolence || !pawn.WorkTagIsDisabled(WorkTags.Violent);
		}

		// Token: 0x060144E9 RID: 83177 RVA: 0x0061177C File Offset: 0x0060F97C
		private static bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
		{
			if (title.bedroomRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < title.bedroomRequirements.Count; i++)
			{
				if (!title.bedroomRequirements[i].PlayerCanBuildNow())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060144EA RID: 83178 RVA: 0x006117C0 File Offset: 0x0060F9C0
		public static QuestPart_ReservePawns ReservePawns(this Quest quest, IEnumerable<Pawn> pawns)
		{
			QuestPart_ReservePawns questPart_ReservePawns = new QuestPart_ReservePawns();
			questPart_ReservePawns.pawns.AddRange(pawns);
			quest.AddPart(questPart_ReservePawns);
			return questPart_ReservePawns;
		}

		// Token: 0x060144EB RID: 83179 RVA: 0x006117E8 File Offset: 0x0060F9E8
		public static QuestPart_TransporterPawns_Feed FeedPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Feed questPart_TransporterPawns_Feed = new QuestPart_TransporterPawns_Feed();
			questPart_TransporterPawns_Feed.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Feed.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Feed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Feed);
			return questPart_TransporterPawns_Feed;
		}

		// Token: 0x060144EC RID: 83180 RVA: 0x00611840 File Offset: 0x0060FA40
		public static QuestPart_TransporterPawns_Tend TendPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Tend questPart_TransporterPawns_Tend = new QuestPart_TransporterPawns_Tend();
			questPart_TransporterPawns_Tend.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Tend.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Tend.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Tend);
			return questPart_TransporterPawns_Tend;
		}

		// Token: 0x060144ED RID: 83181 RVA: 0x00611898 File Offset: 0x0060FA98
		public static QuestPart_TransporterPawns_TendWithMedicine TendPawnsWithMedicine(this Quest quest, ThingDef medicineDef, bool allowSelfTend = true, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_TendWithMedicine questPart_TransporterPawns_TendWithMedicine = new QuestPart_TransporterPawns_TendWithMedicine();
			questPart_TransporterPawns_TendWithMedicine.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_TendWithMedicine.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_TendWithMedicine.medicineDef = medicineDef;
			questPart_TransporterPawns_TendWithMedicine.allowSelfTend = allowSelfTend;
			questPart_TransporterPawns_TendWithMedicine.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_TendWithMedicine);
			return questPart_TransporterPawns_TendWithMedicine;
		}

		// Token: 0x060144EE RID: 83182 RVA: 0x00611900 File Offset: 0x0060FB00
		public static QuestPart_EnsureNotDowned EnsureNotDowned(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_EnsureNotDowned questPart_EnsureNotDowned = new QuestPart_EnsureNotDowned();
			questPart_EnsureNotDowned.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_EnsureNotDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_EnsureNotDowned);
			return questPart_EnsureNotDowned;
		}

		// Token: 0x060144EF RID: 83183 RVA: 0x00611950 File Offset: 0x0060FB50
		public static QuestPart_Bestowing_TargetChangedTitle Bestowing_TargetChangedTitle(this Quest quest, Pawn target, Pawn bestower, RoyalTitleDef currentTitle, string inSignal)
		{
			QuestPart_Bestowing_TargetChangedTitle questPart_Bestowing_TargetChangedTitle = new QuestPart_Bestowing_TargetChangedTitle();
			questPart_Bestowing_TargetChangedTitle.pawn = target;
			questPart_Bestowing_TargetChangedTitle.bestower = bestower;
			questPart_Bestowing_TargetChangedTitle.currentTitle = currentTitle;
			questPart_Bestowing_TargetChangedTitle.inSignal = inSignal;
			questPart_Bestowing_TargetChangedTitle.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
			quest.AddPart(questPart_Bestowing_TargetChangedTitle);
			return questPart_Bestowing_TargetChangedTitle;
		}

		// Token: 0x060144F0 RID: 83184 RVA: 0x00611990 File Offset: 0x0060FB90
		public static QuestPart_SetAllApparelLocked SetAllApparelLocked(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.pawns.AddRange(pawns);
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_SetAllApparelLocked);
			return questPart_SetAllApparelLocked;
		}

		// Token: 0x0400E008 RID: 57352
		public const int MaxUsablePawnsToGenerate = 10;

		// Token: 0x020039CD RID: 14797
		public struct GetPawnParms
		{
			// Token: 0x060144F1 RID: 83185 RVA: 0x006119D8 File Offset: 0x0060FBD8
			public PawnGenerationRequest GenerationRequest(PawnKindDef pawnKind, Faction faction, RoyalTitleDef fixedTitle)
			{
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				bool flag = this.mustBeCapableOfViolence;
				return new PawnGenerationRequest(pawnKind, faction, context, null, true, false, false, true, flag, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			}

			// Token: 0x0400E009 RID: 57353
			public bool mustBeFactionLeader;

			// Token: 0x0400E00A RID: 57354
			public bool mustBeWorldPawn;

			// Token: 0x0400E00B RID: 57355
			public bool ifWorldPawnThenMustBeFree;

			// Token: 0x0400E00C RID: 57356
			public bool ifWorldPawnThenMustBeFreeOrLeader;

			// Token: 0x0400E00D RID: 57357
			public bool mustHaveNoFaction;

			// Token: 0x0400E00E RID: 57358
			public bool mustBeFreeColonist;

			// Token: 0x0400E00F RID: 57359
			public bool mustBePlayerPrisoner;

			// Token: 0x0400E010 RID: 57360
			public bool mustBeNotSuspended;

			// Token: 0x0400E011 RID: 57361
			public bool mustHaveRoyalTitleInCurrentFaction;

			// Token: 0x0400E012 RID: 57362
			public bool mustBeNonHostileToPlayer;

			// Token: 0x0400E013 RID: 57363
			public bool? allowPermanentEnemyFaction;

			// Token: 0x0400E014 RID: 57364
			public bool canGeneratePawn;

			// Token: 0x0400E015 RID: 57365
			public bool requireResearchedBedroomFurnitureIfRoyal;

			// Token: 0x0400E016 RID: 57366
			public PawnKindDef mustBeOfKind;

			// Token: 0x0400E017 RID: 57367
			public Faction mustBeOfFaction;

			// Token: 0x0400E018 RID: 57368
			public FloatRange seniorityRange;

			// Token: 0x0400E019 RID: 57369
			public TechLevel minTechLevel;

			// Token: 0x0400E01A RID: 57370
			public List<FactionDef> excludeFactionDefs;

			// Token: 0x0400E01B RID: 57371
			public bool allowTemporaryFactions;

			// Token: 0x0400E01C RID: 57372
			public bool allowHidden;

			// Token: 0x0400E01D RID: 57373
			public bool mustBeCapableOfViolence;

			// Token: 0x0400E01E RID: 57374
			public bool redressPawn;
		}
	}
}
