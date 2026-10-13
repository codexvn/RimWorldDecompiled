using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x0200399C RID: 14748
	public class SymbolResolver_Interior_SleepingInsects : SymbolResolver
	{
		// Token: 0x060143E6 RID: 82918 RVA: 0x0060AD52 File Offset: 0x00608F52
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null;
		}

		// Token: 0x060143E7 RID: 82919 RVA: 0x0060AD6C File Offset: 0x00608F6C
		public override void Resolve(ResolveParams rp)
		{
			Mathf.Min(rp.rect.Width, rp.rect.Height);
			LordJob_SleepThenAssaultColony lordJob_SleepThenAssaultColony = new LordJob_SleepThenAssaultColony(Faction.OfInsects, true);
			lordJob_SleepThenAssaultColony.awakeOnClamor = true;
			Lord lord = LordMaker.MakeNewLord(Faction.OfInsects, lordJob_SleepThenAssaultColony, BaseGen.globalSettings.map, null);
			foreach (PawnKindDef singlePawnKindDef in PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, rp.threatPoints.Value, null))
			{
				ResolveParams resolveParams = rp;
				resolveParams.faction = Faction.OfInsects;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
			signalAction_DormancyWakeUp.signalTag = rp.sleepingInsectsWakeupSignalTag;
			signalAction_DormancyWakeUp.lord = lord;
			GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
