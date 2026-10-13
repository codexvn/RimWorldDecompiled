using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF3 RID: 7923
	public class RoomContents_ChildRoom : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x0600AF69 RID: 44905 RVA: 0x00320EFA File Offset: 0x0031F0FA
		protected override FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(30f, 60f);
			}
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x0600AF6A RID: 44906 RVA: 0x0031F516 File Offset: 0x0031D716
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(3, 6);
			}
		}

		// Token: 0x0600AF6B RID: 44907 RVA: 0x00320F0C File Offset: 0x0031F10C
		protected override void SpawnCorpses(Map map, LayoutRoom room)
		{
			IntVec3 intVec;
			if (!room.TryGetRandomCellInRoom(map, out intVec, 1, 0, null, false))
			{
				return;
			}
			int num = Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f);
			base.SpawnCorpse(intVec, PawnKindDefOf.Villager, num, map, new float?(RoomContents_ChildRoom.YearsAgeChild.RandomInRange), true);
			foreach (IntVec3 intVec2 in GenAdjFast.AdjacentCellsCardinal(intVec))
			{
				if (intVec2.Standable(map))
				{
					num += Mathf.RoundToInt((float)(RoomContents_ChildRoom.OtherAgeOffsetDays.RandomInRange * 60000));
					base.SpawnCorpse(intVec2, PawnKindDefOf.Villager, num, map, new float?(RoomContents_ChildRoom.YearsAgeAdult.RandomInRange), false);
					break;
				}
			}
			IntVec3 c2;
			if (ModsConfig.BiotechActive && RCellFinder.TryFindRandomCellNearWith(intVec, (IntVec3 c) => room.Contains(c, 1), map, out c2, 2, 10))
			{
				FilthMaker.TryMakeFilth(c2, map, ThingDefOf.Filth_Floordrawing, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x0400786F RID: 30831
		private static readonly FloatRange YearsAgeChild = new FloatRange(2f, 3f);

		// Token: 0x04007870 RID: 30832
		private static readonly FloatRange YearsAgeAdult = new FloatRange(20f, 40f);

		// Token: 0x04007871 RID: 30833
		private static readonly IntRange OtherAgeOffsetDays = new IntRange(-2, 2);
	}
}
