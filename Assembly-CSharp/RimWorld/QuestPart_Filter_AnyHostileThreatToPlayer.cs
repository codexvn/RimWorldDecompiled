using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA4 RID: 7076
	public class QuestPart_Filter_AnyHostileThreatToPlayer : QuestPart_Filter
	{
		// Token: 0x06009DC5 RID: 40389 RVA: 0x002DB2E5 File Offset: 0x002D94E5
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.Map != null && GenHostility.AnyHostileActiveThreatToPlayer(this.mapParent.Map, this.countDormantPawnsAsHostile, false);
		}

		// Token: 0x06009DC6 RID: 40390 RVA: 0x002DB315 File Offset: 0x002D9515
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "parent", false);
			Scribe_Values.Look<bool>(ref this.countDormantPawnsAsHostile, "countDormantPawnsAsHostile", false, false);
		}

		// Token: 0x04006D49 RID: 27977
		public MapParent mapParent;

		// Token: 0x04006D4A RID: 27978
		public bool countDormantPawnsAsHostile;
	}
}
