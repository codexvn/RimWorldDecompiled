using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D61 RID: 7521
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x0600A895 RID: 43157
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x0600A896 RID: 43158 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x0600A897 RID: 43159 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x0600A898 RID: 43160 RVA: 0x00303D14 File Offset: 0x00301F14
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null)
			{
				IntVec3 intVec;
				return this.TryFindEntryCell(map, out intVec);
			}
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile);
		}

		// Token: 0x0600A899 RID: 43161 RVA: 0x00303D4C File Offset: 0x00301F4C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 existingMapEdgeCell = IntVec3.Invalid;
			if (map != null && !this.TryFindEntryCell(map, out existingMapEdgeCell))
			{
				return false;
			}
			List<Pawn> generatedEnemies = this.GeneratePawns(parms);
			if (!generatedEnemies.Any<Pawn>())
			{
				return false;
			}
			if (map != null)
			{
				return this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}, "GeneratingMapForNewEncounter", false, null, true, false, null);
			return true;
		}

		// Token: 0x0600A89A RID: 43162 RVA: 0x00303DF8 File Offset: 0x00301FF8
		private bool DoExecute(IncidentParms parms, List<Pawn> generatedEnemies, IntVec3 existingMapEdgeCell)
		{
			Map map = parms.target as Map;
			bool flag = false;
			if (map == null)
			{
				map = CaravanIncidentUtility.SetupCaravanAttackMap((Caravan)parms.target, generatedEnemies, false);
				flag = true;
			}
			else
			{
				for (int i = 0; i < generatedEnemies.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(existingMapEdgeCell, map, 4);
					GenSpawn.Spawn(generatedEnemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				}
			}
			this.PostProcessGeneratedPawnsAfterSpawning(generatedEnemies);
			LordJob lordJob = this.CreateLordJob(generatedEnemies, parms);
			if (lordJob != null)
			{
				LordMaker.MakeNewLord(parms.faction, lordJob, map, generatedEnemies);
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(generatedEnemies[0], parms);
			TaggedString baseLetterText = this.GetLetterText(generatedEnemies[0], parms);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(generatedEnemies, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(generatedEnemies[0], parms), parms, generatedEnemies[0], Array.Empty<NamedArgument>());
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			}
			return true;
		}

		// Token: 0x0600A89B RID: 43163 RVA: 0x00303EFC File Offset: 0x003020FC
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x0600A89C RID: 43164 RVA: 0x00303F33 File Offset: 0x00302133
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x0600A89D RID: 43165 RVA: 0x00303F40 File Offset: 0x00302140
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x0600A89E RID: 43166 RVA: 0x00303F4D File Offset: 0x0030214D
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x0600A89F RID: 43167 RVA: 0x00303F5A File Offset: 0x0030215A
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural);
		}
	}
}
