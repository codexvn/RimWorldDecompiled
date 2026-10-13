using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB4 RID: 7092
	public class QuestPart_Filter_BuiltNearSettlement : QuestPart_Filter
	{
		// Token: 0x06009DFA RID: 40442 RVA: 0x002DBBA9 File Offset: 0x002D9DA9
		protected override bool Pass(SignalArgs args)
		{
			return this.settlementFaction != null && args.GetArg<Thing>("SUBJECT").MapHeld.Parent == this.mapParent;
		}

		// Token: 0x06009DFB RID: 40443 RVA: 0x002DBBD3 File Offset: 0x002D9DD3
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.settlementFaction == faction)
			{
				this.settlementFaction = null;
			}
		}

		// Token: 0x06009DFC RID: 40444 RVA: 0x002DBBEC File Offset: 0x002D9DEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.settlementFaction, "settlementFaction", false);
		}

		// Token: 0x04006D69 RID: 28009
		public MapParent mapParent;

		// Token: 0x04006D6A RID: 28010
		public Faction settlementFaction;
	}
}
