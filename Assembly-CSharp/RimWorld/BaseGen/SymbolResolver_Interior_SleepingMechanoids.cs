using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x0200399E RID: 14750
	public class SymbolResolver_Interior_SleepingMechanoids : SymbolResolver
	{
		// Token: 0x060143EC RID: 82924 RVA: 0x0060AD52 File Offset: 0x00608F52
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null;
		}

		// Token: 0x060143ED RID: 82925 RVA: 0x0060AEA4 File Offset: 0x006090A4
		public override void Resolve(ResolveParams rp)
		{
			LordJob_SleepThenAssaultColony lordJob_SleepThenAssaultColony = new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, rp.sendWokenUpMessage ?? true);
			lordJob_SleepThenAssaultColony.awakeOnClamor = true;
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob_SleepThenAssaultColony, BaseGen.globalSettings.map, null);
			foreach (PawnKindDef singlePawnKindDef in PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), rp.threatPoints.Value, (PawnKindDef pk) => 1f / pk.combatPower))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfMechanoids;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			if (rp.sleepingMechanoidsWakeupSignalTag != null)
			{
				SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
				signalAction_DormancyWakeUp.signalTag = rp.sleepingMechanoidsWakeupSignalTag;
				signalAction_DormancyWakeUp.lord = lord;
				GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}
	}
}
