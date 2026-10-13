using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003940 RID: 14656
	public class SymbolResolver_TriggerRect : SymbolResolver
	{
		// Token: 0x060142D5 RID: 82645 RVA: 0x00603424 File Offset: 0x00601624
		public override void Resolve(ResolveParams rp)
		{
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = rp.rectTriggerSignalTag;
			rectTrigger.Rect = rp.rect;
			rectTrigger.destroyIfUnfogged = rp.destroyIfUnfogged.GetValueOrDefault();
			GenSpawn.Spawn(rectTrigger, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
