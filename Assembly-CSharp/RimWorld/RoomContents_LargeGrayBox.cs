using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0E RID: 7950
	public class RoomContents_LargeGrayBox : RoomContents_GrayBox
	{
		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x0600AFF1 RID: 45041 RVA: 0x003223B7 File Offset: 0x003205B7
		protected virtual int MaxCrates { get; } = 20;

		// Token: 0x0600AFF2 RID: 45042 RVA: 0x003223C0 File Offset: 0x003205C0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			this.placedCrates = 0;
			for (int i = 0; i < this.MaxCrates; i++)
			{
				IntVec3 cell;
				if (room.TryGetRandomCellInRoom(ThingDefOf.GrayBox, map, out cell, null, 1, 1, null, false))
				{
					this.SpawnBox(cell, map);
				}
			}
		}

		// Token: 0x0600AFF3 RID: 45043 RVA: 0x00322418 File Offset: 0x00320618
		protected virtual void SpawnBox(IntVec3 cell, Map map)
		{
			ThingSetMakerDef rewardMaker = ThingSetMakerDefOf.Reward_GrayBoxLowReward;
			bool addRewards = true;
			if (this.placedCrates < 1)
			{
				rewardMaker = ThingSetMakerDefOf.Reward_GrayBox;
			}
			else if (Rand.Chance(0.5f))
			{
				addRewards = false;
			}
			this.placedCrates++;
			RoomContents_GrayBox.SpawnBoxInRoom(cell, map, rewardMaker, addRewards);
		}

		// Token: 0x04007895 RID: 30869
		private const int NormalCrates = 1;

		// Token: 0x04007896 RID: 30870
		private const float ChanceEmpty = 0.5f;

		// Token: 0x04007898 RID: 30872
		private int placedCrates;
	}
}
