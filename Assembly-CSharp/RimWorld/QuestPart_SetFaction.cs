using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA7 RID: 7335
	public class QuestPart_SetFaction : QuestPart
	{
		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600A360 RID: 41824 RVA: 0x002EE335 File Offset: 0x002EC535
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

		// Token: 0x0600A361 RID: 41825 RVA: 0x002EE348 File Offset: 0x002EC548
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.things.Count; i++)
				{
					if (this.things[i].Faction != this.faction)
					{
						this.things[i].SetFaction(this.faction, null);
					}
				}
			}
		}

		// Token: 0x0600A362 RID: 41826 RVA: 0x002EE3B6 File Offset: 0x002EC5B6
		public override void Notify_FactionRemoved(Faction f)
		{
			if (f == this.faction)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A363 RID: 41827 RVA: 0x002EE3C8 File Offset: 0x002EC5C8
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.faction;
		}

		// Token: 0x0600A364 RID: 41828 RVA: 0x002EE3D4 File Offset: 0x002EC5D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040070E0 RID: 28896
		public string inSignal;

		// Token: 0x040070E1 RID: 28897
		public Faction faction;

		// Token: 0x040070E2 RID: 28898
		public List<Thing> things = new List<Thing>();
	}
}
