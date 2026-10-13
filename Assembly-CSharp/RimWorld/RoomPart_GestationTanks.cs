using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F5A RID: 8026
	public class RoomPart_GestationTanks : RoomPartWorker
	{
		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600B157 RID: 45399 RVA: 0x0032AC8A File Offset: 0x00328E8A
		public new RoomPart_GestationTankDef def
		{
			get
			{
				return (RoomPart_GestationTankDef)this.def;
			}
		}

		// Token: 0x0600B158 RID: 45400 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_GestationTanks(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B159 RID: 45401 RVA: 0x0032AC98 File Offset: 0x00328E98
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_GestationTanks.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomPart_GestationTanks.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientMechGestatorTank, RoomPart_GestationTanks.CountPer100Range, room, CS$<>8__locals1.map, new Rot4?(Rot4.South), new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0), null, 1, null, false, true, new Func<IntVec3, Rot4, Map, Thing>(this.SpawnTank), null);
		}

		// Token: 0x0600B15A RID: 45402 RVA: 0x0032ACF0 File Offset: 0x00328EF0
		private Thing SpawnTank(IntVec3 cell, Rot4 rot, Map map)
		{
			RoomPart_GestationTankDef.State state = this.def.options.RandomElementByWeight((RoomPart_GestationTankDef.TankOption value) => value.weight).state;
			ThingWithComps thingWithComps = (ThingWithComps)GenSpawn.Spawn(ThingDefOf.AncientMechGestatorTank, cell, map, rot, WipeMode.Vanish);
			CompMechGestatorTank comp = thingWithComps.GetComp<CompMechGestatorTank>();
			CompMechGestatorTank.TankState state2;
			if (state == RoomPart_GestationTankDef.State.Proximity)
			{
				state2 = CompMechGestatorTank.TankState.Proximity;
			}
			else if (state == RoomPart_GestationTankDef.State.Dormant)
			{
				state2 = CompMechGestatorTank.TankState.Dormant;
			}
			else
			{
				state2 = CompMechGestatorTank.TankState.Empty;
			}
			comp.State = state2;
			return thingWithComps;
		}

		// Token: 0x0400795B RID: 31067
		private static readonly FloatRange CountPer100Range = new FloatRange(3f, 7f);
	}
}
