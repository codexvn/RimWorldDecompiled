using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003944 RID: 14660
	public class SymbolResolver_Ambush : SymbolResolver
	{
		// Token: 0x060142DE RID: 82654 RVA: 0x006036A0 File Offset: 0x006018A0
		public override bool CanResolve(ResolveParams rp)
		{
			return !rp.ambushSignalTag.NullOrEmpty();
		}

		// Token: 0x060142DF RID: 82655 RVA: 0x006036B0 File Offset: 0x006018B0
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = rp.ambushSignalTag;
			signalAction_Ambush.points = (rp.ambushPoints ?? this.DefaultAmbushPoints);
			signalAction_Ambush.ambushType = rp.ambushType.GetValueOrDefault();
			signalAction_Ambush.spawnNear = (rp.spawnNear ?? IntVec3.Invalid);
			signalAction_Ambush.spawnAround = rp.spawnAround.GetValueOrDefault();
			signalAction_Ambush.spawnPawnsOnEdge = rp.spawnPawnsOnEdge.GetValueOrDefault();
			GenSpawn.Spawn(signalAction_Ambush, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x0400DF0F RID: 57103
		private float DefaultAmbushPoints = 200f;
	}
}
