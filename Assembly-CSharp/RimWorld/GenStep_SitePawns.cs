using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200207F RID: 8319
	public class GenStep_SitePawns : GenStep
	{
		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x0600B659 RID: 46681 RVA: 0x00346421 File Offset: 0x00344621
		public override int SeedPart
		{
			get
			{
				return 897875812;
			}
		}

		// Token: 0x0600B65A RID: 46682 RVA: 0x00346428 File Offset: 0x00344628
		private PawnGroupMakerParms GroupMakerParms(PlanetTile tile, Faction faction, float points, int seed)
		{
			PawnGroupKindDef settlement = this.pawnGroupKindDef;
			if (!faction.def.pawnGroupMakers.Any((PawnGroupMaker maker) => maker.kindDef == this.pawnGroupKindDef))
			{
				settlement = PawnGroupKindDefOf.Settlement;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = settlement,
				tile = tile,
				faction = faction,
				inhabitants = true,
				generateFightersOnly = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(points, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x0600B65B RID: 46683 RVA: 0x003464B8 File Offset: 0x003446B8
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_SitePawns.<>c__DisplayClass8_0 CS$<>8__locals1 = new GenStep_SitePawns.<>c__DisplayClass8_0();
			CS$<>8__locals1.map = map;
			CellRect cellRect;
			IntVec3 baseCenter = MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect) ? cellRect.CenterCell : CS$<>8__locals1.map.Center;
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			Lord lord = LordMaker.MakeNewLord(faction, new LordJob_SitePawns(faction, baseCenter, GenStep_SitePawns.Delay.RandomInRange), CS$<>8__locals1.map, null);
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			PawnGroupMakerParms parms2 = this.GroupMakerParms(CS$<>8__locals1.map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed);
			CellRect var = MapGenerator.GetVar<CellRect>("SpawnRect");
			LookTargets lookTargets = new LookTargets();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(parms2, true))
			{
				IntVec3 loc;
				if (var.TryFindRandomCell(out loc, new Predicate<IntVec3>(CS$<>8__locals1.<Generate>g__Validator|0)))
				{
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					lord.AddPawn(pawn);
					lookTargets.targets.Add(pawn);
				}
			}
			if (lookTargets.Any)
			{
				Find.LetterStack.ReceiveLetter(this.letterLabel, this.letterDesc, LetterDefOf.ThreatSmall, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04007D1B RID: 32027
		public FactionDef factionDef;

		// Token: 0x04007D1C RID: 32028
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04007D1D RID: 32029
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04007D1E RID: 32030
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04007D1F RID: 32031
		private static readonly IntRange Delay = new IntRange(2500, 5000);
	}
}
