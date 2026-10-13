using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF1 RID: 7921
	public class RoomContents_AnimalCorpses : RoomContentsWorker
	{
		// Token: 0x0600AF61 RID: 44897 RVA: 0x00320D0C File Offset: 0x0031EF0C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			PawnKindDef kind = (from k in DefDatabase<PawnKindDef>.AllDefsListForReading
			where k.RaceProps.Animal && k.RaceProps.CanDoHerdMigration
			select k).RandomElementByWeight((PawnKindDef x) => Mathf.Lerp(0.2f, 1f, x.race.GetStatValueAbstract(StatDefOf.Wildness, null)));
			int randomInRange = RoomContents_AnimalCorpses.CorpseCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				RoomContents_AnimalCorpses.SpawnCorpse(map, room, kind);
			}
		}

		// Token: 0x0600AF62 RID: 44898 RVA: 0x00320D98 File Offset: 0x0031EF98
		private static Corpse SpawnCorpse(Map map, LayoutRoom room, PawnKindDef kind)
		{
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 2, 0, null, false))
			{
				return null;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			pawn.health.SetDead();
			Corpse corpse = pawn.MakeCorpse(null, null);
			corpse.Age = Mathf.RoundToInt(RoomContents_AnimalCorpses.CorpseAgeDaysRange.RandomInRange * 60000f);
			corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			return (Corpse)GenSpawn.Spawn(corpse, loc, map, WipeMode.Vanish);
		}

		// Token: 0x0400786A RID: 30826
		private static readonly FloatRange CorpseAgeDaysRange = new FloatRange(3f, 30f);

		// Token: 0x0400786B RID: 30827
		private static readonly IntRange CorpseCountRange = new IntRange(1, 3);
	}
}
