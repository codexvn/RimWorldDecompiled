using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3F RID: 7231
	public class QuestPart_EnsureNotDowned : QuestPart
	{
		// Token: 0x0600A0FF RID: 41215 RVA: 0x002E555C File Offset: 0x002E375C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				foreach (Pawn p in this.pawns)
				{
					this.EnsureNotDowned(p);
				}
			}
		}

		// Token: 0x0600A100 RID: 41216 RVA: 0x002E55CC File Offset: 0x002E37CC
		protected void EnsureNotDowned(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			int num = 0;
			while (p.Downed && num++ < 15)
			{
				Hediff hediff = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && h.def.everCurableByItem && !(h is Hediff_MissingPart));
				if (hediff == null)
				{
					break;
				}
				p.health.RemoveHediff(hediff);
			}
			if (p.Downed)
			{
				num = 0;
				while (p.Downed && num++ < 15)
				{
					Hediff hediff2 = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && !(h is Hediff_MissingPart));
					if (hediff2 == null)
					{
						break;
					}
					p.health.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x0600A101 RID: 41217 RVA: 0x002E5688 File Offset: 0x002E3888
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn p) => p == null);
			}
		}

		// Token: 0x04006F49 RID: 28489
		public string inSignal;

		// Token: 0x04006F4A RID: 28490
		public List<Pawn> pawns;
	}
}
