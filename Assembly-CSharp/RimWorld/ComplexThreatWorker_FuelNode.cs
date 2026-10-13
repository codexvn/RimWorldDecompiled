using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAC RID: 7852
	public class ComplexThreatWorker_FuelNode : ComplexThreatWorker
	{
		// Token: 0x0600AE26 RID: 44582 RVA: 0x0031AF8C File Offset: 0x0031918C
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			IntVec3 intVec;
			return base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room, parms.map, out intVec, 1, null);
		}

		// Token: 0x0600AE27 RID: 44583 RVA: 0x0031AFC8 File Offset: 0x003191C8
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room, parms.map, out loc, 1, null);
			Thing thing = GenSpawn.Spawn(ThingDefOf.AncientFuelNode, loc, parms.map, WipeMode.Vanish);
			SignalAction_StartWick signalAction_StartWick = (SignalAction_StartWick)ThingMaker.MakeThing(ThingDefOf.SignalAction_StartWick, null);
			signalAction_StartWick.thingWithWick = thing;
			signalAction_StartWick.signalTag = parms.triggerSignal;
			signalAction_StartWick.completedSignalTag = "CompletedStartWickAction" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			if (parms.delayTicks != null)
			{
				signalAction_StartWick.delayTicks = parms.delayTicks.Value;
			}
			GenSpawn.Spawn(signalAction_StartWick, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
			float randomInRange = ComplexThreatWorker_FuelNode.ExplosiveRadiusRandomRange.RandomInRange;
			compExplosive.customExplosiveRadius = new float?(randomInRange);
			threatPointsUsed = randomInRange * 10f;
		}

		// Token: 0x040077CD RID: 30669
		private const string TriggerStartWickAction = "TriggerStartWickAction";

		// Token: 0x040077CE RID: 30670
		private const string CompletedStartWickAction = "CompletedStartWickAction";

		// Token: 0x040077CF RID: 30671
		private static readonly FloatRange ExplosiveRadiusRandomRange = new FloatRange(2f, 12f);

		// Token: 0x040077D0 RID: 30672
		private const float ExplosiveRadiusThreatPointsFactor = 10f;

		// Token: 0x040077D1 RID: 30673
		private const float RoomEntryTriggerChance = 0.25f;
	}
}
