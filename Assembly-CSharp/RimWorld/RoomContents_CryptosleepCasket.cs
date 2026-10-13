using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED1 RID: 7889
	public class RoomContents_CryptosleepCasket : RoomContentsWorker
	{
		// Token: 0x0600AEDA RID: 44762 RVA: 0x0031E528 File Offset: 0x0031C728
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 3, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			int nextAncientCryptosleepCasketGroupID = Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			PodContentsType type = Gen.RandomEnumValue<PodContentsType>(true);
			Building_AncientCryptosleepCasket pod = RoomGenUtility.SpawnCryptoCasket(centerCell, map, Rot4.Random, nextAncientCryptosleepCasketGroupID, type, ThingSetMakerDefOf.MapGen_ScarlandsAncientPodContents);
			this.ThreatSignal = "OpenedSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			RoomGenUtility.SpawnOpenCryptoCasketSignal(pod, map, this.ThreatSignal);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0400782D RID: 30765
		private const string OpenedSignal = "OpenedSignal";
	}
}
