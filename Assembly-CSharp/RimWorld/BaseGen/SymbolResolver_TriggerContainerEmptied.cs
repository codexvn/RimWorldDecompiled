using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200393F RID: 14655
	public class SymbolResolver_TriggerContainerEmptied : SymbolResolver
	{
		// Token: 0x060142D2 RID: 82642 RVA: 0x006033C2 File Offset: 0x006015C2
		public override bool CanResolve(ResolveParams rp)
		{
			return rp.triggerContainerEmptiedThing != null;
		}

		// Token: 0x060142D3 RID: 82643 RVA: 0x006033D0 File Offset: 0x006015D0
		public override void Resolve(ResolveParams rp)
		{
			TriggerContainerEmptied triggerContainerEmptied = (TriggerContainerEmptied)ThingMaker.MakeThing(ThingDefOf.TriggerContainerEmptied, null);
			triggerContainerEmptied.signalTag = rp.triggerContainerEmptiedSignalTag;
			triggerContainerEmptied.container = rp.triggerContainerEmptiedThing;
			GenSpawn.Spawn(triggerContainerEmptied, rp.triggerContainerEmptiedThing.Position, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
