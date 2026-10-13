using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003928 RID: 14632
	public class SymbolResolver_OpenDoorAction : SymbolResolver
	{
		// Token: 0x0601428C RID: 82572 RVA: 0x00601CA8 File Offset: 0x005FFEA8
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_OpenDoor signalAction_OpenDoor = (SignalAction_OpenDoor)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenDoor, null);
			signalAction_OpenDoor.signalTag = rp.openDoorActionSignalTag;
			signalAction_OpenDoor.door = rp.openDoorActionDoor;
			GenSpawn.Spawn(signalAction_OpenDoor, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
