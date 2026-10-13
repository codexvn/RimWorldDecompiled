using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F05 RID: 7941
	public class RoomContents_Animals : RoomContentsWorker
	{
		// Token: 0x0600AFD1 RID: 45009 RVA: 0x00321EF8 File Offset: 0x003200F8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			PawnKindDef kind = (from k in DefDatabase<PawnKindDef>.AllDefsListForReading
			where k.RaceProps.Animal && k.RaceProps.CanDoHerdMigration
			select k).RandomElementByWeight((PawnKindDef x) => Mathf.Lerp(RoomContents_Animals.WildnessRange.min, RoomContents_Animals.WildnessRange.max, x.race.GetStatValueAbstract(StatDefOf.Wildness, null)));
			int randomInRange = RoomContents_Animals.AnimalCount.RandomInRange;
			PawnGenerationRequest request = new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			int num = 0;
			IntVec3 loc;
			while (num < randomInRange && room.TryGetRandomCellInRoom(map, out loc, 2, 0, null, false))
			{
				GenSpawn.Spawn(PawnGenerator.GeneratePawn(request), loc, map, WipeMode.Vanish);
				num++;
			}
		}

		// Token: 0x0400788A RID: 30858
		private static readonly IntRange AnimalCount = new IntRange(1, 3);

		// Token: 0x0400788B RID: 30859
		private static readonly FloatRange WildnessRange = new FloatRange(0.2f, 1f);
	}
}
