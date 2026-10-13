using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA1 RID: 7073
	public class QuestPart_Filter_AnyColonistAlive : QuestPart_Filter
	{
		// Token: 0x06009DBD RID: 40381 RVA: 0x002DB1F3 File Offset: 0x002D93F3
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.HasMap && this.mapParent.Map.mapPawns.ColonistCount != 0;
		}

		// Token: 0x06009DBE RID: 40382 RVA: 0x002DB224 File Offset: 0x002D9424
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x04006D47 RID: 27975
		public MapParent mapParent;
	}
}
