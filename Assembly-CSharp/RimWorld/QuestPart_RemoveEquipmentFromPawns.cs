using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C82 RID: 7298
	public class QuestPart_RemoveEquipmentFromPawns : QuestPart
	{
		// Token: 0x0600A2A0 RID: 41632 RVA: 0x002EC264 File Offset: 0x002EA464
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i] != null && this.pawns[i].equipment != null)
					{
						this.pawns[i].equipment.DestroyAllEquipment(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x0600A2A1 RID: 41633 RVA: 0x002EC2DC File Offset: 0x002EA4DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400707F RID: 28799
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04007080 RID: 28800
		public string inSignal;
	}
}
