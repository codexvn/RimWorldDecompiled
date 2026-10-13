using System;

namespace RimWorld
{
	// Token: 0x020035CA RID: 13770
	[DefOf]
	public static class ThingSetMakerDefOf
	{
		// Token: 0x060133B8 RID: 78776 RVA: 0x005B5DC3 File Offset: 0x005B3FC3
		static ThingSetMakerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingSetMakerDefOf));
		}

		// Token: 0x0400D2E6 RID: 53990
		public static ThingSetMakerDef MapGen_AncientTempleContents;

		// Token: 0x0400D2E7 RID: 53991
		public static ThingSetMakerDef MapGen_AncientPodContents;

		// Token: 0x0400D2E8 RID: 53992
		public static ThingSetMakerDef MapGen_DefaultStockpile;

		// Token: 0x0400D2E9 RID: 53993
		public static ThingSetMakerDef MapGen_PrisonCellStockpile;

		// Token: 0x0400D2EA RID: 53994
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Default;

		// Token: 0x0400D2EB RID: 53995
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Better;

		// Token: 0x0400D2EC RID: 53996
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplex_SecurityCrate;

		// Token: 0x0400D2ED RID: 53997
		[MayRequireAnomaly]
		public static ThingSetMakerDef MapGen_FleshSackLoot;

		// Token: 0x0400D2EE RID: 53998
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_ScarlandsAncientPodContents;

		// Token: 0x0400D2EF RID: 53999
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_ScarlandsHermeticCrate;

		// Token: 0x0400D2F0 RID: 54000
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_HighValueCrate;

		// Token: 0x0400D2F1 RID: 54001
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_AbandonedColonyStockpile;

		// Token: 0x0400D2F2 RID: 54002
		public static ThingSetMakerDef Reward_ItemsStandard;

		// Token: 0x0400D2F3 RID: 54003
		[MayRequireAnomaly]
		public static ThingSetMakerDef Reward_GrayBox;

		// Token: 0x0400D2F4 RID: 54004
		[MayRequireAnomaly]
		public static ThingSetMakerDef Reward_GrayBoxLowReward;

		// Token: 0x0400D2F5 RID: 54005
		public static ThingSetMakerDef DebugCaravanInventory;

		// Token: 0x0400D2F6 RID: 54006
		public static ThingSetMakerDef DebugQuestDropPodsContents;

		// Token: 0x0400D2F7 RID: 54007
		public static ThingSetMakerDef TraderStock;

		// Token: 0x0400D2F8 RID: 54008
		public static ThingSetMakerDef ResourcePod;

		// Token: 0x0400D2F9 RID: 54009
		public static ThingSetMakerDef RefugeePod;

		// Token: 0x0400D2FA RID: 54010
		public static ThingSetMakerDef Meteorite;

		// Token: 0x0400D2FB RID: 54011
		public static ThingSetMakerDef VisitorGift;

		// Token: 0x0400D2FC RID: 54012
		[MayRequireIdeology]
		public static ThingSetMakerDef Reward_ReliquaryPilgrims;

		// Token: 0x0400D2FD RID: 54013
		[MayRequireOdyssey]
		public static ThingSetMakerDef Reward_UniqueWeapon;

		// Token: 0x0400D2FE RID: 54014
		[MayRequireOdyssey]
		public static ThingSetMakerDef Reward_ArcheanSeed;
	}
}
