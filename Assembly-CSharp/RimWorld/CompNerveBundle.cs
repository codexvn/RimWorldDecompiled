using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C96 RID: 11414
	public class CompNerveBundle : CompFleshmassHeartChild
	{
		// Token: 0x06010036 RID: 65590 RVA: 0x004A3914 File Offset: 0x004A1B14
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.FleshmassNeuralLump, null);
			thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = this.heart.Biosignature;
			GenSpawn.Spawn(thing, this.parent.Position, prevMap, WipeMode.Vanish);
			Messages.Message("NeuralLumpDroppedMessage".Translate(), thing, MessageTypeDefOf.NeutralEvent, true);
			base.Notify_Killed(prevMap, dinfo);
		}
	}
}
