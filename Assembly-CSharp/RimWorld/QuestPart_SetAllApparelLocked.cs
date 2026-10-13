using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA5 RID: 7333
	public class QuestPart_SetAllApparelLocked : QuestPart
	{
		// Token: 0x0600A359 RID: 41817 RVA: 0x002EE230 File Offset: 0x002EC430
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].apparel != null)
					{
						this.pawns[i].apparel.LockAll();
					}
				}
			}
		}

		// Token: 0x0600A35A RID: 41818 RVA: 0x002EE298 File Offset: 0x002EC498
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

		// Token: 0x0600A35B RID: 41819 RVA: 0x002EE306 File Offset: 0x002EC506
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040070DC RID: 28892
		public string inSignal;

		// Token: 0x040070DD RID: 28893
		public List<Pawn> pawns = new List<Pawn>();
	}
}
