using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF2 RID: 7666
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x0600AAC6 RID: 43718 RVA: 0x0030D050 File Offset: 0x0030B250
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && (!ModsConfig.BiotechActive || !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human, 0f) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x0600AAC7 RID: 43719 RVA: 0x0030D0D4 File Offset: 0x0030B2D4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			DevelopmentalStage developmentalStage = Find.Storyteller.difficulty.ChildrenAllowed ? (DevelopmentalStage.Child | DevelopmentalStage.Adult) : DevelopmentalStage.Adult;
			PawnKindDef wildMan = PawnKindDefOf.WildMan;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			DevelopmentalStage developmentalStages = developmentalStage;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(wildMan, faction2, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false));
			pawn.SetFaction(null, null);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			string value = pawn.DevelopmentalStage.Child() ? "FeralChild".Translate().ToString() : pawn.KindLabel;
			TaggedString value2 = pawn.DevelopmentalStage.Child() ? "Child".Translate() : "Person".Translate();
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(value, pawn.Named("PAWN")).CapitalizeFirst();
			TaggedString baseLetterText = this.def.letterText.Formatted(pawn.NameShortColored, value2, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.def.letterDef, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600AAC8 RID: 43720 RVA: 0x0030D2D0 File Offset: 0x0030B4D0
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x0600AAC9 RID: 43721 RVA: 0x0030D307 File Offset: 0x0030B507
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined, TechLevel.Undefined, false, false);
		}
	}
}
