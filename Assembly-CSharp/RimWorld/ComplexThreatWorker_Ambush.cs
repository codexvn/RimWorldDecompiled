using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAA RID: 7850
	public class ComplexThreatWorker_Ambush : ComplexThreatWorker
	{
		// Token: 0x0600AE1F RID: 44575 RVA: 0x0031AAE5 File Offset: 0x00318CE5
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || (this.def.signalActionAmbushType == SignalActionAmbushType.Mechanoids && parms.hostileFaction == Faction.OfMechanoids));
		}

		// Token: 0x0600AE20 RID: 44576 RVA: 0x0031AB1C File Offset: 0x00318D1C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = parms.triggerSignal;
			signalAction_Ambush.points = parms.points;
			signalAction_Ambush.ambushType = this.def.signalActionAmbushType;
			signalAction_Ambush.useDropPods = this.def.useDropPods;
			if (this.def.spawnAroundComplex)
			{
				signalAction_Ambush.spawnAround = parms.complexRect.ExpandedBy(5);
			}
			GenSpawn.Spawn(signalAction_Ambush, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += parms.points;
		}

		// Token: 0x040077C9 RID: 30665
		private const int SpawnAroundDistance = 5;
	}
}
