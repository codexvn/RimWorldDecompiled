using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED2 RID: 7890
	public class RoomContents_CryptosleepCaskets : RoomContentsWorker
	{
		// Token: 0x0600AEDC RID: 44764 RVA: 0x0031E5B9 File Offset: 0x0031C7B9
		public virtual PodContentsType GetContentsType()
		{
			if (!Rand.Bool)
			{
				return PodContentsType.AncientFriendly;
			}
			return PodContentsType.AncientHostile;
		}

		// Token: 0x0600AEDD RID: 44765 RVA: 0x0031E5C8 File Offset: 0x0031C7C8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_CryptosleepCaskets.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomContents_CryptosleepCaskets.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.groupID = Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientCryptosleepCasket, RoomContents_CryptosleepCaskets.CasketRange.RandomInRange, room, CS$<>8__locals1.map, new Rot4?(Rot4.South), null, null, 2, null, false, false, new Func<IntVec3, Rot4, Map, Thing>(CS$<>8__locals1.<FillRoom>g__SpawnAction|0), null);
			RoomContents_CryptosleepCaskets.SpawnLockers(CS$<>8__locals1.map, room);
			base.FillRoom(CS$<>8__locals1.map, room, faction, threatPoints);
		}

		// Token: 0x0600AEDE RID: 44766 RVA: 0x0031E650 File Offset: 0x0031C850
		private static void SpawnLockers(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_CryptosleepCaskets.LockersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientLockerBank, count, RoomContents_CryptosleepCaskets.LockerGroupSizeRange, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0400782E RID: 30766
		private static readonly FloatRange LockersPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x0400782F RID: 30767
		private static readonly IntRange LockerGroupSizeRange = new IntRange(2, 3);

		// Token: 0x04007830 RID: 30768
		private static readonly IntRange CasketRange = new IntRange(2, 4);
	}
}
