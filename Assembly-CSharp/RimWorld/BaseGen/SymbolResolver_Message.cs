using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200396B RID: 14699
	public class SymbolResolver_Message : SymbolResolver
	{
		// Token: 0x0601435B RID: 82779 RVA: 0x00607418 File Offset: 0x00605618
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message.signalTag = rp.messageSignalTag;
			signalAction_Message.message = rp.message;
			signalAction_Message.messageType = rp.messageType;
			signalAction_Message.lookTargets = rp.lookTargets;
			GenSpawn.Spawn(signalAction_Message, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
