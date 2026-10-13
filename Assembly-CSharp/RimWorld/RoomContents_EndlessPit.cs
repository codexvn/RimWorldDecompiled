using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F07 RID: 7943
	public class RoomContents_EndlessPit : RoomContentsWorker
	{
		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x0600AFD8 RID: 45016 RVA: 0x00322086 File Offset: 0x00320286
		private IEnumerable<ThingDef> PitDefs
		{
			get
			{
				yield return ThingDefOf.EndlessPit2x2c;
				yield return ThingDefOf.EndlessPit3x2c;
				yield return ThingDefOf.EndlessPit3x3c;
				yield break;
			}
		}

		// Token: 0x0600AFD9 RID: 45017 RVA: 0x00322090 File Offset: 0x00320290
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			ThingDef thingDef = this.PitDefs.RandomElement<ThingDef>();
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(thingDef, map, out loc, null, 3, 1, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, null), loc, map, thingDef.rotatable ? Rot4.Random : Rot4.North, WipeMode.Vanish, false, false);
		}
	}
}
