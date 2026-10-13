using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F53 RID: 8019
	public class RoomPart_Corpse : RoomPartWorker
	{
		// Token: 0x0600B146 RID: 45382 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_Corpse(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B147 RID: 45383 RVA: 0x0032AAF0 File Offset: 0x00328CF0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			PawnKindDef villager = PawnKindDefOf.Villager;
			if (faction != null)
			{
				faction.RandomPawnKind();
			}
			RoomGenUtility.SpawnCorpses(room, map, RoomPart_Corpse.Count, villager, DamageDefOf.ExecutionCut, null, ThingDefOf.MeleeWeapon_Knife.tools[1], null, null, null, false);
		}

		// Token: 0x04007958 RID: 31064
		private static readonly IntRange Count = IntRange.One;
	}
}
