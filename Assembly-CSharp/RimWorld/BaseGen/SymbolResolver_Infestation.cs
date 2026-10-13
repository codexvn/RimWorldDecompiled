using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003963 RID: 14691
	public class SymbolResolver_Infestation : SymbolResolver
	{
		// Token: 0x0601433B RID: 82747 RVA: 0x00606A21 File Offset: 0x00604C21
		public override bool CanResolve(ResolveParams rp)
		{
			return Faction.OfInsects != null && !rp.infestationSignalTag.NullOrEmpty();
		}

		// Token: 0x0601433C RID: 82748 RVA: 0x00606A3C File Offset: 0x00604C3C
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Infestation signalAction_Infestation = (SignalAction_Infestation)ThingMaker.MakeThing(ThingDefOf.SignalAction_Infestation, null);
			signalAction_Infestation.signalTag = rp.infestationSignalTag;
			signalAction_Infestation.hivesCount = (rp.hivesCount ?? 1);
			signalAction_Infestation.insectsPoints = rp.insectsPoints;
			signalAction_Infestation.spawnAnywhereIfNoGoodCell = rp.spawnAnywhereIfNoGoodCell.GetValueOrDefault();
			signalAction_Infestation.ignoreRoofedRequirement = rp.ignoreRoofedRequirement.GetValueOrDefault();
			signalAction_Infestation.overrideLoc = rp.overrideLoc;
			signalAction_Infestation.sendStandardLetter = (rp.sendStandardLetter ?? true);
			GenSpawn.Spawn(signalAction_Infestation, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
