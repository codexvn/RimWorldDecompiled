using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED8 RID: 7896
	public class RoomContents_HermeticCrate : RoomContentsWorker
	{
		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x0600AEF4 RID: 44788 RVA: 0x0031EA09 File Offset: 0x0031CC09
		public virtual ThingSetMakerDef ThingSetMakerDef { get; } = ThingSetMakerDefOf.MapGen_ScarlandsHermeticCrate;

		// Token: 0x0600AEF5 RID: 44789 RVA: 0x0031EA14 File Offset: 0x0031CC14
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.ThreatSignal = "OpenedSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			RoomGenUtility.SpawnHermeticCrateInRoom(room, map, this.ThingSetMakerDef, true, this.ThreatSignal, new Rot4?(Rot4.Random));
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x04007839 RID: 30777
		private const string OpenedSignal = "OpenedSignal";
	}
}
