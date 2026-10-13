using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC5 RID: 7365
	public abstract class QuestPart_TransporterPawns : QuestPart
	{
		// Token: 0x0600A41C RID: 42012 RVA: 0x002F0EC8 File Offset: 0x002EF0C8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (this.pawns != null)
				{
					foreach (Pawn pawn in this.pawns)
					{
						this.Process(pawn);
					}
				}
				if (this.pawnsInTransporter != null)
				{
					foreach (Thing thing in ((IEnumerable<Thing>)this.pawnsInTransporter.TryGetComp<CompTransporter>().innerContainer))
					{
						Pawn pawn2 = thing as Pawn;
						if (pawn2 != null)
						{
							this.Process(pawn2);
						}
					}
				}
			}
		}

		// Token: 0x0600A41D RID: 42013
		public abstract void Process(Pawn pawn);

		// Token: 0x0600A41E RID: 42014 RVA: 0x002F0F90 File Offset: 0x002EF190
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.pawnsInTransporter, "pawnsInTransporter", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A41F RID: 42015 RVA: 0x002F100F File Offset: 0x002EF20F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawns != null)
			{
				this.pawns.Replace(replace, with);
			}
		}

		// Token: 0x0400715D RID: 29021
		public string inSignal;

		// Token: 0x0400715E RID: 29022
		public Thing pawnsInTransporter;

		// Token: 0x0400715F RID: 29023
		public List<Pawn> pawns = new List<Pawn>();
	}
}
