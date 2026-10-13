using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F04 RID: 7940
	public class RoomContents_AbandonedCamp : RoomContentsWorker
	{
		// Token: 0x0600AFCD RID: 45005 RVA: 0x00321E0C File Offset: 0x0032000C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.Bedroll, map, out loc, null, 3, 1, null, false))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Bedroll, ThingDefOf.Cloth);
			GenSpawn.Spawn(thing, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			this.DropSpawnNear(ThingDefOf.MealSurvivalPack, thing.Position + IntVec3.East, map, RoomContents_AbandonedCamp.SurvivalPacksCountRange.RandomInRange);
			this.DropSpawnNear(ThingDefOf.TorchLamp, thing.Position, map, RoomContents_AbandonedCamp.TorchesCountRange.RandomInRange);
		}

		// Token: 0x0600AFCE RID: 45006 RVA: 0x00321EAC File Offset: 0x003200AC
		private void DropSpawnNear(ThingDef thing, IntVec3 cell, Map map, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				Thing thing2;
				GenDrop.TryDropSpawn(ThingMaker.MakeThing(thing, null), cell, map, ThingPlaceMode.Near, out thing2, null, null, true);
			}
		}

		// Token: 0x04007888 RID: 30856
		private static readonly IntRange SurvivalPacksCountRange = new IntRange(0, 1);

		// Token: 0x04007889 RID: 30857
		private static readonly IntRange TorchesCountRange = new IntRange(0, 1);
	}
}
