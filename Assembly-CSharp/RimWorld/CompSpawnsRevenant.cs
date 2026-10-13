using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E49 RID: 11849
	public class CompSpawnsRevenant : ThingComp
	{
		// Token: 0x06010AD6 RID: 68310 RVA: 0x004CEBC2 File Offset: 0x004CCDC2
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x06010AD7 RID: 68311 RVA: 0x004CEBD8 File Offset: 0x004CCDD8
		public override void CompTickRare()
		{
			if (this.spawnTick > 0 && Find.TickManager.TicksGame > this.spawnTick && this.parent.MapHeld != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Revenant, Faction.OfEntities, null);
				GenSpawn.Spawn(pawn, this.parent.PositionHeld, this.parent.MapHeld, WipeMode.Vanish);
				CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
				compRevenant.Invisibility.BecomeVisible(true);
				compRevenant.SetState(RevenantState.Escape);
				Find.LetterStack.ReceiveLetter("LetterLabelRevenantEmergence".Translate(), "LetterRevenantEmergence".Translate(), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400ADDD RID: 44509
		public int spawnTick = -1;
	}
}
