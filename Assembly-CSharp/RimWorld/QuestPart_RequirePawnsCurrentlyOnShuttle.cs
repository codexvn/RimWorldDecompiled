using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9C RID: 7324
	public class QuestPart_RequirePawnsCurrentlyOnShuttle : QuestPart
	{
		// Token: 0x0600A32E RID: 41774 RVA: 0x002EDC84 File Offset: 0x002EBE84
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			if (compShuttle.requiredPawns.Contains(pawn))
			{
				compShuttle.requiredPawns.Remove(pawn);
			}
		}

		// Token: 0x0600A32F RID: 41775 RVA: 0x002EDCB8 File Offset: 0x002EBEB8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				compShuttle.requiredColonistCount = this.requiredColonistCount;
				compShuttle.requiredItems.Clear();
				compShuttle.requiredPawns.Clear();
				foreach (Thing thing in ((IEnumerable<Thing>)this.shuttle.TryGetComp<CompTransporter>().innerContainer))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && !compShuttle.requiredPawns.Contains(pawn))
					{
						compShuttle.requiredPawns.Add(pawn);
					}
				}
			}
		}

		// Token: 0x0600A330 RID: 41776 RVA: 0x002EDD7C File Offset: 0x002EBF7C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
		}

		// Token: 0x0600A331 RID: 41777 RVA: 0x002EDD96 File Offset: 0x002EBF96
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignl", null, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x040070C8 RID: 28872
		public string inSignal;

		// Token: 0x040070C9 RID: 28873
		public Thing shuttle;

		// Token: 0x040070CA RID: 28874
		public int requiredColonistCount;
	}
}
