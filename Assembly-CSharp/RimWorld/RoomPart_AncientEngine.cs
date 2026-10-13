using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F48 RID: 8008
	public class RoomPart_AncientEngine : RoomPartWorker
	{
		// Token: 0x0600B120 RID: 45344 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_AncientEngine(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B121 RID: 45345 RVA: 0x00329C98 File Offset: 0x00327E98
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect = (from r in room.rects
			where r.Width >= 5 && r.Height >= 5
			select r).RandomElementByWeight((CellRect r) => (float)r.Area);
			if (cellRect == default(CellRect))
			{
				Log.Error("Failed to place ancient grav engine.");
				return;
			}
			Thing engine = RoomPart_AncientEngine.SpawnEngine(map, cellRect);
			RoomPart_AncientEngine.SpawnSignal(map, room, engine);
		}

		// Token: 0x0600B122 RID: 45346 RVA: 0x00329D28 File Offset: 0x00327F28
		private static Thing SpawnEngine(Map map, CellRect largest)
		{
			IntVec3 zero = IntVec3.Zero;
			if (largest.Width % 2 == 0)
			{
				zero.x = 1;
			}
			if (largest.Height % 2 == 0)
			{
				zero.z = 1;
			}
			return GenSpawn.Spawn(ThingDefOf.AncientGravEngine, largest.CenterCell - zero, map, WipeMode.Vanish);
		}

		// Token: 0x0600B123 RID: 45347 RVA: 0x00329D7C File Offset: 0x00327F7C
		private static void SpawnSignal(Map map, LayoutRoom room, Thing engine)
		{
			string signalTag = string.Format("ThingDiscovered_{0}", Find.UniqueIDsManager.GetNextSignalTagID());
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.NeutralEvent;
			signalAction_Letter.letterLabelKey = "LetterAncientGravEngineDiscoveredLabel";
			signalAction_Letter.letterMessageKey = "LetterAncientGravEngineDiscovered";
			signalAction_Letter.lookTargets = engine;
			signalAction_Letter.requireLookTargetsNotDestroyed = true;
			GenSpawn.Spawn(signalAction_Letter, engine.Position, map, WipeMode.Vanish);
			room.SpawnRectTriggersForAction(signalAction_Letter, map);
		}
	}
}
