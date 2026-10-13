using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C73 RID: 7283
	public class QuestPart_PawnKilled : QuestPart
	{
		// Token: 0x0600A24F RID: 41551 RVA: 0x002EA968 File Offset: 0x002E8B68
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent == this.mapParent)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
			}
		}

		// Token: 0x0600A250 RID: 41552 RVA: 0x002EA9BC File Offset: 0x002E8BBC
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A251 RID: 41553 RVA: 0x002EA9CE File Offset: 0x002E8BCE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x0400702A RID: 28714
		public Faction faction;

		// Token: 0x0400702B RID: 28715
		public MapParent mapParent;

		// Token: 0x0400702C RID: 28716
		public string outSignal;
	}
}
