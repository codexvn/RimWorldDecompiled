using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD8 RID: 7384
	public class QuestPart_ChangeHeir : QuestPart
	{
		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x0600A494 RID: 42132 RVA: 0x002F24F3 File Offset: 0x002F06F3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.holder;
				yield return this.heir;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x0600A495 RID: 42133 RVA: 0x002F2503 File Offset: 0x002F0703
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A496 RID: 42134 RVA: 0x002F2514 File Offset: 0x002F0714
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		// Token: 0x0600A497 RID: 42135 RVA: 0x002F2566 File Offset: 0x002F0766
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.holder == replace)
			{
				this.holder = with;
			}
			if (this.heir == replace)
			{
				this.heir = with;
			}
		}

		// Token: 0x0600A498 RID: 42136 RVA: 0x002F2588 File Offset: 0x002F0788
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		// Token: 0x040071A0 RID: 29088
		public Faction faction;

		// Token: 0x040071A1 RID: 29089
		public Pawn holder;

		// Token: 0x040071A2 RID: 29090
		public Pawn heir;

		// Token: 0x040071A3 RID: 29091
		public string inSignal;

		// Token: 0x040071A4 RID: 29092
		public bool done;
	}
}
