using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C15 RID: 7189
	public class QuestPart_BiocodeWeapons : QuestPart
	{
		// Token: 0x0600A01A RID: 40986 RVA: 0x002E2814 File Offset: 0x002E0A14
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].equipment != null)
					{
						foreach (ThingWithComps thingWithComps in this.pawns[i].equipment.AllEquipmentListForReading)
						{
							CompBiocodable comp = thingWithComps.GetComp<CompBiocodable>();
							if (comp != null && !comp.Biocoded)
							{
								comp.CodeFor(this.pawns[i]);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A01B RID: 40987 RVA: 0x002E28DC File Offset: 0x002E0ADC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A01C RID: 40988 RVA: 0x002E294A File Offset: 0x002E0B4A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006EBA RID: 28346
		public string inSignal;

		// Token: 0x04006EBB RID: 28347
		public List<Pawn> pawns = new List<Pawn>();
	}
}
