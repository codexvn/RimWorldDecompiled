using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A5C RID: 14940
	public class QuestNode_GetPawn : QuestNode
	{
		// Token: 0x060146DF RID: 83679 RVA: 0x0061A498 File Offset: 0x00618698
		private IEnumerable<Pawn> ExistingUsablePawns(Slate slate)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.IsGoodPawn(x, slate)
			select x;
		}

		// Token: 0x060146E0 RID: 83680 RVA: 0x0061A4D0 File Offset: 0x006186D0
		protected override bool TestRunInt(Slate slate)
		{
			if (this.mustHaveNoFaction.GetValue(slate) && this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				return false;
			}
			if (this.canGeneratePawn.GetValue(slate) && (this.mustBeFactionLeader.GetValue(slate) || this.mustBeWorldPawn.GetValue(slate) || this.mustBePlayerPrisoner.GetValue(slate) || this.mustBeFreeColonist.GetValue(slate)))
			{
				Log.Warning("QuestNode_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBeWorldPawn, mustBePlayerPrisoner, mustBeFreeColonist");
				return false;
			}
			Pawn pawn;
			if (slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return true;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			if (source.Count<Pawn>() > 0)
			{
				slate.Set<Pawn>(this.storeAs.GetValue(slate), source.RandomElement<Pawn>(), false);
				return true;
			}
			if (!this.canGeneratePawn.GetValue(slate))
			{
				return false;
			}
			Faction faction;
			if (!this.mustHaveNoFaction.GetValue(slate) && !this.TryFindFactionForPawnGeneration(slate, out faction))
			{
				return false;
			}
			FloatRange senRange = this.seniorityRange.GetValue(slate);
			return !this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || !this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && this.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x060146E1 RID: 83681 RVA: 0x0061A620 File Offset: 0x00618820
		private bool TryFindFactionForPawnGeneration(Slate slate, out Faction faction)
		{
			return Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false).Where(delegate(Faction x)
			{
				Map map;
				return (this.excludeFactionDefs.GetValue(slate) == null || !this.excludeFactionDefs.GetValue(slate).Contains(x.def)) && (!this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || x.def.HasRoyalTitles) && (!this.mustBeNonHostileToPlayer.GetValue(slate) || !x.HostileTo(Faction.OfPlayer)) && (!slate.TryGet<Map>("map", out map, false) || !this.mustHaveSettlementOnLayer.GetValue(slate) || !map.Tile.Valid || Find.WorldObjects.AnyFactionSettlementOnLayer(x, map.Tile.Layer)) && (this.allowPermanentEnemyFaction.GetValue(slate).GetValueOrDefault() || !x.def.permanentEnemy) && x.def.techLevel >= this.minTechLevel.GetValue(slate) && (!this.factionMustBePermanent.GetValue(slate) || !x.temporary);
			}).TryRandomElementByWeight(delegate(Faction x)
			{
				if (x.HostileTo(Faction.OfPlayer))
				{
					float? value = this.hostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
				else
				{
					float? value = this.nonHostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
			}, out faction);
		}

		// Token: 0x060146E2 RID: 83682 RVA: 0x0061A674 File Offset: 0x00618874
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn pawn;
			if (QuestGen.slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			int num = source.Count<Pawn>();
			Faction faction;
			if (Rand.Chance(this.canGeneratePawn.GetValue(slate) ? Mathf.Clamp01(1f - (float)num / (float)this.maxUsablePawnsToGenerate.GetValue(slate)) : 0f) && (this.mustHaveNoFaction.GetValue(slate) || this.TryFindFactionForPawnGeneration(slate, out faction)))
			{
				pawn = this.GeneratePawn(slate, null);
			}
			else
			{
				pawn = source.RandomElementByWeight(delegate(Pawn x)
				{
					if (x.Faction != null && x.Faction.HostileTo(Faction.OfPlayer))
					{
						float? value = this.hostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
					else
					{
						float? value = this.nonHostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
				});
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(pawn.Faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
		}

		// Token: 0x060146E3 RID: 83683 RVA: 0x0061A7BC File Offset: 0x006189BC
		private Pawn GeneratePawn(Slate slate, Faction faction = null)
		{
			PawnKindDef pawnKindDef = this.mustBeOfKind.GetValue(slate);
			if (faction == null && !this.mustHaveNoFaction.GetValue(slate))
			{
				if (!this.TryFindFactionForPawnGeneration(slate, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				FloatRange senRange;
				if (!this.seniorityRange.TryGetValue(slate, out senRange))
				{
					senRange = FloatRange.Zero;
				}
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) && source.Any(new Func<RoyalTitleDef, bool>(this.PlayerHasResearchedBedroomRequirementsFor)))
				{
					source = source.Where(new Func<RoyalTitleDef, bool>(this.PlayerHasResearchedBedroomRequirementsFor));
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (this.mustBeOfKind.GetValue(slate) == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
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
			PawnKindDef kind2 = pawnKindDef;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			RoyalTitleDef fixedTitle2 = fixedTitle;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction2, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle2, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x060146E4 RID: 83684 RVA: 0x0061AA5C File Offset: 0x00618C5C
		private bool IsGoodPawn(Pawn pawn, Slate slate)
		{
			if (this.mustBeFactionLeader.GetValue(slate))
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (pawn.Faction != null && this.excludeFactionDefs.GetValue(slate) != null && this.excludeFactionDefs.GetValue(slate).Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < this.minTechLevel.GetValue(slate))
			{
				return false;
			}
			if (this.mustBeOfKind.GetValue(slate) != null && pawn.kindDef != this.mustBeOfKind.GetValue(slate))
			{
				return false;
			}
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (this.seniorityRange.GetValue(slate) != default(FloatRange))
			{
				Pawn_RoyaltyTracker royalty = pawn.royalty;
				if (((royalty != null) ? royalty.MostSeniorTitle : null) == null || !this.seniorityRange.GetValue(slate).IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority))
				{
					return false;
				}
			}
			if (this.factionMustBePermanent.GetValue(slate) && pawn.Faction != null && pawn.Faction.temporary)
			{
				return false;
			}
			if (this.mustBeWorldPawn.GetValue(slate) && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFree.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFreeOrLeader.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (this.mustHaveNoFaction.GetValue(slate) && pawn.Faction != null)
			{
				return false;
			}
			if (this.mustBeFreeColonist.GetValue(slate) && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (this.mustBePlayerPrisoner.GetValue(slate) && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.mustBeNotSuspended.GetValue(slate) && pawn.Suspended)
			{
				return false;
			}
			if (this.mustBeNonHostileToPlayer.GetValue(slate) && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!(this.allowPermanentEnemyFaction.GetValue(slate) ?? true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate))
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !this.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060146E5 RID: 83685 RVA: 0x0061AD8C File Offset: 0x00618F8C
		private bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
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

		// Token: 0x0400E1EB RID: 57835
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1EC RID: 57836
		public SlateRef<bool> mustBeFactionLeader;

		// Token: 0x0400E1ED RID: 57837
		public SlateRef<bool> mustBeWorldPawn;

		// Token: 0x0400E1EE RID: 57838
		public SlateRef<bool> ifWorldPawnThenMustBeFree;

		// Token: 0x0400E1EF RID: 57839
		public SlateRef<bool> ifWorldPawnThenMustBeFreeOrLeader;

		// Token: 0x0400E1F0 RID: 57840
		public SlateRef<bool> mustHaveNoFaction;

		// Token: 0x0400E1F1 RID: 57841
		public SlateRef<bool> mustBeFreeColonist;

		// Token: 0x0400E1F2 RID: 57842
		public SlateRef<bool> mustBePlayerPrisoner;

		// Token: 0x0400E1F3 RID: 57843
		public SlateRef<bool> mustBeNotSuspended;

		// Token: 0x0400E1F4 RID: 57844
		public SlateRef<bool> mustHaveRoyalTitleInCurrentFaction;

		// Token: 0x0400E1F5 RID: 57845
		public SlateRef<bool> mustBeNonHostileToPlayer;

		// Token: 0x0400E1F6 RID: 57846
		public SlateRef<bool?> allowPermanentEnemyFaction;

		// Token: 0x0400E1F7 RID: 57847
		public SlateRef<bool> canGeneratePawn;

		// Token: 0x0400E1F8 RID: 57848
		public SlateRef<bool> mustHaveSettlementOnLayer;

		// Token: 0x0400E1F9 RID: 57849
		public SlateRef<bool> requireResearchedBedroomFurnitureIfRoyal;

		// Token: 0x0400E1FA RID: 57850
		public SlateRef<PawnKindDef> mustBeOfKind;

		// Token: 0x0400E1FB RID: 57851
		public SlateRef<FloatRange> seniorityRange;

		// Token: 0x0400E1FC RID: 57852
		public SlateRef<TechLevel> minTechLevel;

		// Token: 0x0400E1FD RID: 57853
		public SlateRef<List<FactionDef>> excludeFactionDefs;

		// Token: 0x0400E1FE RID: 57854
		public SlateRef<float?> hostileWeight;

		// Token: 0x0400E1FF RID: 57855
		public SlateRef<float?> nonHostileWeight;

		// Token: 0x0400E200 RID: 57856
		public SlateRef<bool> factionMustBePermanent = true;

		// Token: 0x0400E201 RID: 57857
		public SlateRef<int> maxUsablePawnsToGenerate = 10;
	}
}
