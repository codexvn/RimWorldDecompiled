using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003941 RID: 14657
	public class SymbolResolver_TriggerUnfogged : SymbolResolver
	{
		// Token: 0x060142D7 RID: 82647 RVA: 0x00603488 File Offset: 0x00601688
		public override void Resolve(ResolveParams rp)
		{
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = rp.unfoggedSignalTag;
			GenSpawn.Spawn(triggerUnfogged, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
